using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace OFrameLibrary.Util
{
    internal class OctreeQuantizer : Quantizer
    {
        int _maxColors;
        Octree _octree;

        /// <summary>
        /// Construct the octree quantizer
        /// </summary>
        /// <remarks>
        /// The Octree quantizer is a two pass algorithm. The initial pass sets up the octree,
        /// the second pass quantizes a color based on the nodes in the tree
        /// </remarks>
        /// <param name="maxColors">The maximum number of colors to return</param>
        /// <param name="maxColorBits">The number of significant bits</param>
        public OctreeQuantizer(int maxColors, int maxColorBits)
            : base(false)
        {
            if (maxColors > 255)
            {
                throw new ArgumentOutOfRangeException("maxColors", maxColors, "The number of colors should be less than 256");
            }
            if ((maxColorBits < 1) | (maxColorBits > 8))
            {
                throw new ArgumentOutOfRangeException("maxColorBits", maxColorBits, "This should be between 1 and 8");
            }
            _octree = new Octree(maxColorBits);
            _maxColors = maxColors;
        }

        protected override ColorPalette GetPalette(ColorPalette original)
        {
            var palette = _octree.Palletize(_maxColors - 1);

            for (var index = 0; index < palette.Count; index++)
            {
                original.Entries[index] = (Color)palette[index];
            }
            original.Entries[_maxColors] = Color.FromArgb(0, 0, 0, 0);

            return original;
        }

        protected override void InitialQuantizePixel(Color32 pixel)
        {
            _octree.AddColor(pixel);
        }

        protected override byte QuantizePixel(Color32 pixel)
        {
            var paletteIndex = (byte)_maxColors;

            if (pixel.Alpha > 0)
            {
                paletteIndex = (byte)_octree.GetPaletteIndex(pixel);
            }
            return paletteIndex;
        }

        // private classes...
        class Octree
        {
            /// <summary>
            /// Mask used when getting the appropriate pixels for a given node
            /// </summary>
            static readonly int[] mask = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

            /// <summary>
            /// Maximum number of significant bits in the image
            /// </summary>
            int _maxColorBits;

            /// <summary>
            /// Cache the previous color quantized
            /// </summary>
            int _previousColor;

            /// <summary>
            /// Store the last node quantized
            /// </summary>
            OctreeNode _previousNode;

            /// <summary>
            /// Array of reducible nodes
            /// </summary>
            OctreeNode[] _reducibleNodes;

            /// <summary>
            /// The root of the octree
            /// </summary>
            OctreeNode _root;

            /// <summary>
            /// Construct the octree
            /// </summary>
            /// <param name="maxColorBits">The maximum number of significant bits in the image</param>
            public Octree(int maxColorBits)
            {
                _maxColorBits = maxColorBits;
                Leaves = 0;
                _reducibleNodes = new OctreeNode[9];
                _root = new OctreeNode(0, _maxColorBits, this);
                _previousColor = 0;
                _previousNode = null;
            }

            /// <summary>
            /// Get/Set the number of leaves in the tree
            /// </summary>
            public int Leaves { get; set; }

            /// <summary>
            /// Add a given color value to the octree
            /// </summary>
            /// <param name="pixel"></param>
            public void AddColor(Color32 pixel)
            {
                if (_previousColor == pixel.ARGB)
                {
                    if (null == _previousNode)
                    {
                        _previousColor = pixel.ARGB;
                        _root.AddColor(pixel, _maxColorBits, 0, this);
                    }
                    else
                    {
                        _previousNode.Increment(pixel);
                    }
                }
                else
                {
                    _previousColor = pixel.ARGB;
                    _root.AddColor(pixel, _maxColorBits, 0, this);
                }
            }

            /// <summary>
            /// Get the palette index for the passed color
            /// </summary>
            /// <param name="pixel"></param>
            /// <returns></returns>
            public int GetPaletteIndex(Color32 pixel)
            {
                return _root.GetPaletteIndex(pixel, 0);
            }

            /// <summary>
            /// Convert the nodes in the octree to a palette with a maximum of colorCount colors
            /// </summary>
            /// <param name="colorCount">The maximum number of colors</param>
            /// <returns>An arraylist with the palettized colors</returns>
            public ArrayList Palletize(int colorCount)
            {
                while (Leaves > colorCount)
                {
                    Reduce();
                }
                var palette = new ArrayList(Leaves);
                var paletteIndex = 0;
                _root.ConstructPalette(palette, ref paletteIndex);

                return palette;
            }

            /// <summary>
            /// Reduce the depth of the tree
            /// </summary>
            public void Reduce()
            {
                int index;

                for (index = _maxColorBits - 1; (index > 0) && (null == _reducibleNodes[index]); index--)
                {

                }
                var node = _reducibleNodes[index];
                _reducibleNodes[index] = node.NextReducible;

                Leaves -= node.Reduce();

                _previousNode = null;
            }

            /// <summary>
            /// Return the array of reducible nodes
            /// </summary>
            protected OctreeNode[] ReducibleNodes()
            {
                return _reducibleNodes;
            }

            /// <summary>
            /// Keep track of the previous node that was quantized
            /// </summary>
            /// <param name="node">The node last quantized</param>
            protected void TrackPrevious(OctreeNode node)
            {
                _previousNode = node;
            }

            /// <summary>
            /// Class which encapsulates each node in the tree
            /// </summary>
            protected class OctreeNode
            {
                readonly

                /// <summary>
                /// Pointers to any child nodes
                /// </summary>
                OctreeNode[] _children;

                /// <summary>
                /// Blue component
                /// </summary>
                int _blue;

                /// <summary>
                /// Green Component
                /// </summary>
                int _green;

                /// <summary>
                /// Flag indicating that this is a leaf node
                /// </summary>
                bool _leaf;

                /// <summary>
                /// Pointer to next reducible node
                /// </summary>
                OctreeNode _nextReducible;

                /// <summary>
                /// The index of this node in the palette
                /// </summary>
                int _paletteIndex;

                /// <summary>
                /// Number of pixels in this node
                /// </summary>
                int _pixelCount;

                /// <summary>
                /// Red component
                /// </summary>
                int _red;

                /// <summary>
                /// Construct the node
                /// </summary>
                /// <param name="level">The level in the tree = 0 - 7</param>
                /// <param name="colorBits">The number of significant color bits in the image</param>
                /// <param name="octree">The tree to which this node belongs</param>
                public OctreeNode(int level, int colorBits, Octree octree)
                {
                    _leaf = (level == colorBits);

                    _red = _green = _blue = 0;
                    _pixelCount = 0;

                    if (_leaf)
                    {
                        octree.Leaves++;
                        _nextReducible = null;
                        _children = null;
                    }
                    else
                    {
                        var repNodes = octree.ReducibleNodes();
                        _nextReducible = repNodes[level];
                        repNodes[level] = this;
                        _children = new OctreeNode[8];
                    }
                }

                /// <summary>
                /// Get/Set the next reducible node
                /// </summary>
                public OctreeNode NextReducible
                {
                    get
                    {
                        return _nextReducible;
                    }
                }

                /// <summary>
                /// Add a color into the tree
                /// </summary>
                /// <param name="pixel">The color</param>
                /// <param name="colorBits">The number of significant color bits</param>
                /// <param name="level">The level in the tree</param>
                /// <param name="octree">The tree to which this node belongs</param>
                public void AddColor(Color32 pixel, int colorBits, int level, Octree octree)
                {
                    if (_leaf)
                    {
                        Increment(pixel);

                        octree.TrackPrevious(this);
                    }
                    else
                    {
                        checked
                        {
                            var shift = 7 - level;
                            var index = ((pixel.Red & mask[level]) >> (shift - 2)) |
                            ((pixel.Green & mask[level]) >> (shift - 1)) |
                            ((pixel.Blue & mask[level]) >> (shift));

                            var child = _children[index];

                            if (null == child)
                            {
                                child = new OctreeNode(level + 1, colorBits, octree);
                                _children[index] = child;
                            }

                            child.AddColor(pixel, colorBits, level + 1, octree);
                        }
                    }
                }

                /// <summary>
                /// Traverse the tree, building up the color palette
                /// </summary>
                /// <param name="palette">The palette</param>
                /// <param name="paletteIndex">The current palette index</param>
                public void ConstructPalette(ArrayList palette, ref int paletteIndex)
                {
                    if (_leaf)
                    {
                        _paletteIndex = paletteIndex++;

                        palette.Add(Color.FromArgb(_red / _pixelCount, _green / _pixelCount, _blue / _pixelCount));
                    }
                    else
                    {
                        for (var index = 0; index < 8; index++)
                        {
                            if (null != _children[index])
                            {
                                _children[index].ConstructPalette(palette, ref paletteIndex);
                            }
                        }
                    }
                }

                /// <summary>
                /// Return the palette index for the passed color
                /// </summary>
                public int GetPaletteIndex(Color32 pixel, int level)
                {
                    var paletteIndex = _paletteIndex;

                    if (!_leaf)
                    {
                        checked
                        {
                            var shift = 7 - level;
                            var index = ((pixel.Red & mask[level]) >> (shift - 2)) |
                            ((pixel.Green & mask[level]) >> (shift - 1)) |
                            ((pixel.Blue & mask[level]) >> (shift));

                            if (null != _children[index])
                            {
                                paletteIndex = _children[index].GetPaletteIndex(pixel, level + 1);
                            }
                            else
                            {
                                throw new ArgumentException("Didn't expect this!");
                            }
                        }
                    }

                    return paletteIndex;
                }

                /// <summary>
                /// Increment the pixel count and add to the color information
                /// </summary>
                public void Increment(Color32 pixel)
                {
                    _pixelCount++;
                    _red += pixel.Red;
                    _green += pixel.Green;
                    _blue += pixel.Blue;
                }

                /// <summary>
                /// Reduce this node by removing all of its children
                /// </summary>
                /// <returns>The number of leaves removed</returns>
                public int Reduce()
                {
                    _red = _green = _blue = 0;
                    var children = 0;

                    for (var index = 0; index < 8; index++)
                    {
                        if (null != _children[index])
                        {
                            _red += _children[index]._red;
                            _green += _children[index]._green;
                            _blue += _children[index]._blue;
                            _pixelCount += _children[index]._pixelCount;
                            ++children;
                            _children[index] = null;
                        }
                    }

                    _leaf = true;

                    return (children - 1);
                }
            }
        }
    }
}
