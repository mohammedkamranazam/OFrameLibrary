using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OFrameLibrary.Util
{
    internal abstract class Quantizer
    {
        private readonly int _pixelSize;
        private readonly bool _singlePass;

        public Quantizer(bool singlePass)
        {
            _singlePass = singlePass;
            _pixelSize = Marshal.SizeOf(typeof(Color32));
        }

        public Bitmap Quantize(Image source)
        {
            var height = source.Height;
            var width = source.Width;

            var bounds = new Rectangle(0, 0, width, height);

            var copy = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            var output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            using (var g = Graphics.FromImage(copy))
            {
                g.PageUnit = GraphicsUnit.Pixel;

                g.DrawImage(source, bounds);
            }

            BitmapData sourceData = null;

            try
            {
                sourceData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                if (!_singlePass)
                {
                    FirstPass(sourceData, width, height);
                }

                output.Palette = GetPalette(output.Palette);

                SecondPass(sourceData, output, width, height, bounds);
            }
            finally
            {
                copy.UnlockBits(sourceData);
            }

            return output;
        }

        protected virtual void FirstPass(BitmapData sourceData, int width, int height)
        {
            var pSourceRow = sourceData.Scan0;

            for (var row = 0; row < height; row++)
            {
                var pSourcePixel = pSourceRow;

                for (var col = 0; col < width; col++)
                {
                    InitialQuantizePixel(new Color32(pSourcePixel));
                    pSourcePixel = (IntPtr)((Int32)pSourcePixel + _pixelSize);
                }

                pSourceRow = (IntPtr)((long)pSourceRow + sourceData.Stride);
            }
        }

        /// <summary>
        /// Retrieve the palette for the quantized image
        /// </summary>
        /// <param name="original">Any old palette, this is overwritten</param>
        /// <returns>The new color palette</returns>
        protected abstract ColorPalette GetPalette(ColorPalette original);

        /// <summary>
        /// Override this to process the pixel in the first pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <remarks>
        /// This function need only be overridden if your quantize algorithm needs two passes,
        /// such as an Octree quantizer.
        /// </remarks>
        protected virtual void InitialQuantizePixel(Color32 pixel)
        {
        }

        /// <summary>
        /// Override this to process the pixel in the second pass of the algorithm
        /// </summary>
        /// <param name="pixel">The pixel to quantize</param>
        /// <returns>The quantized value</returns>
        protected abstract byte QuantizePixel(Color32 pixel);

        protected virtual void SecondPass(BitmapData sourceData, Bitmap output, int width, int height, Rectangle bounds)
        {
            BitmapData outputData = null;

            try
            {
                outputData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                var pSourceRow = sourceData.Scan0;
                var pSourcePixel = pSourceRow;
                var pPreviousPixel = pSourcePixel;

                var pDestinationRow = outputData.Scan0;
                var pDestinationPixel = pDestinationRow;

                var pixelValue = QuantizePixel(new Color32(pSourcePixel));

                Marshal.WriteByte(pDestinationPixel, pixelValue);

                for (var row = 0; row < height; row++)
                {
                    pSourcePixel = pSourceRow;

                    pDestinationPixel = pDestinationRow;

                    for (var col = 0; col < width; col++)
                    {
                        if (Marshal.ReadByte(pPreviousPixel) != Marshal.ReadByte(pSourcePixel))
                        {
                            pixelValue = QuantizePixel(new Color32(pSourcePixel));

                            pPreviousPixel = pSourcePixel;
                        }

                        Marshal.WriteByte(pDestinationPixel, pixelValue);

                        pSourcePixel = (IntPtr)((long)pSourcePixel + _pixelSize);
                        pDestinationPixel = (IntPtr)((long)pDestinationPixel + 1);
                    }

                    pSourceRow = (IntPtr)((long)pSourceRow + sourceData.Stride);

                    pDestinationRow = (IntPtr)((long)pDestinationRow + outputData.Stride);
                }
            }
            finally
            {
                output.UnlockBits(outputData);
            }
        }

        // public structs...
        /// <summary>
        /// Struct that defines a 32 bpp color
        /// </summary>
        /// <remarks>
        /// This struct is used to read data from a 32 bits per pixel image
        /// in memory, and is ordered in this manner as this is the way that
        /// the data is laid out in memory
        /// </remarks>
        [StructLayout(LayoutKind.Explicit)]
        public struct Color32
        {
            [FieldOffset(0)]
            private byte _Blue;

            [FieldOffset(1)]
            private byte _Green;

            [FieldOffset(2)]
            private byte _Red;

            [FieldOffset(3)]
            private byte _Alpha;

            [FieldOffset(0)]
            private int _ARGB;

            public Color32(IntPtr pSourcePixel)
            {
                this = (Color32)Marshal.PtrToStructure(pSourcePixel, typeof(Color32));
            }

            /// <summary>
            /// Holds the alpha component of the colour
            /// </summary>
            public byte Alpha
            {
                get
                {
                    return _Alpha;
                }

                set
                {
                    _Alpha = value;
                }
            }

            /// <summary>
            /// Permits the color32 to be treated as an int32
            /// </summary>
            public int ARGB
            {
                get
                {
                    return _ARGB;
                }

                set
                {
                    _ARGB = value;
                }
            }

            /// <summary>
            /// Holds the blue component of the colour
            /// </summary>
            public byte Blue
            {
                get
                {
                    return _Blue;
                }

                set
                {
                    _Blue = value;
                }
            }

            /// <summary>
            /// Return the color for this Color32 object
            /// </summary>
            public Color Color
            {
                get
                {
                    return Color.FromArgb(Alpha, Red, Green, Blue);
                }
            }

            /// <summary>
            /// Holds the green component of the colour
            /// </summary>
            public byte Green
            {
                get
                {
                    return _Green;
                }

                set
                {
                    _Green = value;
                }
            }

            /// <summary>
            /// Holds the red component of the colour
            /// </summary>
            public byte Red
            {
                get
                {
                    return _Red;
                }

                set
                {
                    _Red = value;
                }
            }
        }
    }
}
