using OFrameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;

namespace OFrameLibrary.Helpers
{
    public static class ImageHelper
    {
        private static Size GetAspectRatioSize(int maxWidth, int maxHeight, int actualWidth, int actualHeight)
        {
            var oSize = new Size(maxWidth, maxHeight);

            var iFactorX = (float)maxWidth / (float)actualWidth;
            var iFactorY = (float)maxHeight / (float)actualHeight;

            if (iFactorX != 1 || iFactorY != 1)
            {
                if (iFactorX < iFactorY)
                {
                    oSize.Height = (int)Math.Round((float)actualHeight * iFactorX);
                }
                else
                {
                    if (iFactorX > iFactorY)
                    {
                        oSize.Width = (int)Math.Round((float)actualWidth * iFactorY);
                    }
                }
            }

            if (oSize.Height <= 0)
            {
                oSize.Height = 1;
            }
            if (oSize.Width <= 0)
            {
                oSize.Width = 1;
            }
            return oSize;
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();

            for (var i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType == mimeType)
                {
                    return codecs[i];
                }
            }

            return null;
        }

        private static ImageCodecInfo GetJpgCodec()
        {
            var aCodecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo oCodec = null;

            for (var i = 0; i < aCodecs.Length; i++)
            {
                if (aCodecs[i].MimeType.Equals("image/jpeg"))
                {
                    oCodec = aCodecs[i];
                    break;
                }
            }

            return oCodec;
        }

        public static Stream AddText(Stream image, string text, string fontFamily, float size, string color, Point position)
        {
            var bitmap = new Bitmap(image);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                graphics.DrawString(text, new Font(fontFamily, size, GraphicsUnit.Pixel), new SolidBrush(ColorTranslator.FromHtml(color)), position);
            }

            var stream = new MemoryStream();

            var format = bitmap.RawFormat;

            bitmap.Save(stream, format);

            bitmap.Dispose();

            return stream;
        }

        public static Image Base64ToImage(string base64String, string urlToSave)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            var memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);

            memoryStream.Write(imageBytes, 0, imageBytes.Length);
            var image = Image.FromStream(memoryStream, true);

            image.Save(urlToSave);

            return image;
        }

        public static Stream Base64ToStream(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            var memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);

            memoryStream.Write(imageBytes, 0, imageBytes.Length);

            return memoryStream;
        }

        public static Stream Compress(int quality, Stream imageStream)
        {
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            var codec = GetEncoderInfo("image/jpeg");
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            var compressedStream = new MemoryStream();

            var img = Image.FromStream(imageStream);
            img.Save(compressedStream, codec, encoderParams);
            img.Dispose();

            return compressedStream;
        }

        public static Stream Compress(int quality, Stream imageStream, string outputPath)
        {
            var compressedStream = Compress(quality, imageStream);

            Save(compressedStream, outputPath);

            return compressedStream;
        }

        public static Stream Crop(Stream image, Rectangle rectangle)
        {
            var original = new Bitmap(image);
            var bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppPArgb);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(original, new Rectangle(0, 0, rectangle.Width, rectangle.Height), rectangle, GraphicsUnit.Pixel);
            }

            var stream = new MemoryStream();

            var format = original.RawFormat;

            bitmap.Save(stream, format);

            original.Dispose();
            bitmap.Dispose();

            return stream;
        }

        public static MemoryStream Crop(Image OriginalImage, int Width, int Height, int X, int Y)
        {
            try
            {
                using (var bmp = new Bitmap(Width, Height))
                {
                    bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                    using (var Graphic = Graphics.FromImage(bmp))
                    {
                        Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, Width, Height), X, Y, Width, Height, GraphicsUnit.Pixel);
                        var ms = new MemoryStream();
                        bmp.Save(ms, OriginalImage.RawFormat);
                        return ms;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        public static Image Flip(Image original, RotateFlipType type)
        {
            var result = new Bitmap(original);
            result.RotateFlip(type);

            return result;
        }

        public static MemoryStream GetStreamFromFile(string absolutePath)
        {
            var image = Image.FromFile(absolutePath);

            var stream = new MemoryStream();

            image.Save(stream, ImageFormat.Jpeg);

            image.Dispose();

            return stream;
        }

        public static void GetWidthAndHeight(Stream imageStream, out int width, out int height)
        {
            var img = Image.FromStream(imageStream);

            width = img.Width;
            height = img.Height;
        }

        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                var imageBytes = ms.ToArray();

                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Image InsertImage(Image original, Image other, int x, int y)
        {
            using (var b = new Bitmap(original))
            {
                using (var g = Graphics.FromImage(b))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;

                    using (Image j = new Bitmap(other))
                    {
                        g.DrawImage(j, new Rectangle(x, y, j.Width, j.Height));
                    }

                    var newImage = Image.FromHbitmap(b.GetHbitmap());
                    return newImage;
                }
            }
        }

        public static bool IsImageExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case "jpg":
                case "jpeg":
                case "gif":
                case "png":
                case "bmp":
                    return true;
            }
            return false;
        }

        public static Image Opacity(Image original, float opacity)
        {
            var result = new Bitmap(original.Width, original.Height);
            using (var g = Graphics.FromImage(result))
            {
                var cm = new ColorMatrix();
                cm.Matrix33 = opacity;

                var ia = new ImageAttributes();
                ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(original, new Rectangle(0, 0, result.Width, result.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, ia);
            }

            return result;
        }

        public static Stream Resize(int width, Stream imageStream, bool highQuality)
        {
            if (highQuality)
            {
                return Resize(width, imageStream, CompositingQuality.HighQuality, SmoothingMode.HighQuality, InterpolationMode.HighQualityBicubic);
            }
            else
            {
                return Resize(width, imageStream, CompositingQuality.HighSpeed, SmoothingMode.HighSpeed, InterpolationMode.Low);
            }
        }

        public static Stream Resize(int width, Stream imageStream, bool highQuality, string outputPath)
        {
            var resizedStream = Resize(width, imageStream, highQuality);

            Save(resizedStream, outputPath);

            return resizedStream;
        }

        public static Stream Resize(int width, Stream imageStream, CompositingQuality compositingQuality, SmoothingMode smoothingMode, InterpolationMode interpolationMode)
        {
            var image = Image.FromStream(imageStream);

            var thumbnailSize = width;
            int newWidth, newHeight;

            if (image.Width > image.Height)
            {
                newWidth = thumbnailSize;
                newHeight = image.Height * thumbnailSize / image.Width;
            }
            else
            {
                newWidth = image.Width * thumbnailSize / image.Height;
                newHeight = thumbnailSize;
            }

            var bitmap = new Bitmap(newWidth, newHeight);

            var graphic = Graphics.FromImage(bitmap);

            graphic.CompositingQuality = compositingQuality;
            graphic.SmoothingMode = smoothingMode;
            graphic.InterpolationMode = interpolationMode;

            var rectangle = new Rectangle(0, 0, newWidth, newHeight);
            graphic.DrawImage(image, rectangle);

            var resizedStream = new MemoryStream();
            bitmap.Save(resizedStream, ImageFormat.Png);

            graphic.Dispose();
            bitmap.Dispose();
            image.Dispose();

            return resizedStream;
        }

        public static Stream Resize(int width, Stream imageStream, string outputPath, CompositingQuality compositingQuality, SmoothingMode smoothingMode, InterpolationMode interpolationMode)
        {
            var resizedStream = Resize(width, imageStream, compositingQuality, smoothingMode, interpolationMode);

            Save(resizedStream, outputPath);

            return resizedStream;
        }

        public static bool ResizeImage(string sourceFile, int maxWidth, int maxHeight, bool preserverAspectRatio, int quality)
        {
            Image sourceImage;

            try
            {
                sourceImage = Image.FromFile(sourceFile);
            }
            catch (OutOfMemoryException)
            {
                return false;
            }

            return ResizeImage(sourceImage, sourceFile, maxWidth, maxHeight, preserverAspectRatio, quality);
        }

        public static bool ResizeImage(Image sourceImage, string targetFile, int maxWidth, int maxHeight, bool preserverAspectRatio, int quality)
        {
            maxWidth = maxWidth == 0 ? sourceImage.Width : maxWidth;
            maxHeight = maxHeight == 0 ? sourceImage.Height : maxHeight;

            Size oSize;
            if (preserverAspectRatio)
            {
                oSize = GetAspectRatioSize(maxWidth, maxHeight, sourceImage.Width, sourceImage.Height);
            }
            else
            {
                oSize = new Size(maxWidth, maxHeight);
            }
            Image oResampled;

            if (sourceImage.PixelFormat == PixelFormat.Indexed || sourceImage.PixelFormat == PixelFormat.Format1bppIndexed || sourceImage.PixelFormat == PixelFormat.Format4bppIndexed || sourceImage.PixelFormat == PixelFormat.Format8bppIndexed || sourceImage.PixelFormat.ToString() == "8207")
            {
                oResampled = new Bitmap(oSize.Width, oSize.Height, PixelFormat.Format24bppRgb);
            }
            else
            {
                oResampled = new Bitmap(oSize.Width, oSize.Height, sourceImage.PixelFormat);
            }
            var oGraphics = Graphics.FromImage(oResampled);

            Rectangle oRectangle;

            if (quality > 80)
            {
                oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                oRectangle = new Rectangle(-1, -1, oSize.Width + 1, oSize.Height + 1);
            }
            else
            {
                oRectangle = new Rectangle(0, 0, oSize.Width, oSize.Height);
            }
            oGraphics.FillRectangle(new SolidBrush(Color.White), oRectangle);

            oGraphics.DrawImage(sourceImage, oRectangle);

            sourceImage.Dispose();

            var extension = Path.GetExtension(targetFile).ToLower();

            if (extension == ".jpg" || extension == ".jpeg")
            {
                var oCodec = GetJpgCodec();

                if (oCodec != null)
                {
                    var aCodecParams = new EncoderParameters(1);
                    aCodecParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    oResampled.Save(targetFile, oCodec, aCodecParams);
                }
                else
                {
                    oResampled.Save(targetFile);
                }
            }
            else
            {
                switch (extension)
                {
                    case ".gif":
                        try
                        {
                            var quantizer = new OctreeQuantizer(255, 8);
                            using (var quantized = quantizer.Quantize(oResampled))
                            {
                                quantized.Save(targetFile, System.Drawing.Imaging.ImageFormat.Gif);
                            }
                        }
                        catch (SecurityException)
                        {
                            oResampled.Save(targetFile, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        break;

                    case ".png":
                        oResampled.Save(targetFile, System.Drawing.Imaging.ImageFormat.Png);
                        break;

                    case ".bmp":
                        oResampled.Save(targetFile, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                }
            }
            oGraphics.Dispose();
            oResampled.Dispose();

            return true;
        }

        public static void Save(Stream imageStream, string outputPath)
        {
            var img = Image.FromStream(imageStream);

            img.Save(outputPath);

            img.Dispose();
        }

        public static Image Scale(Image original, int width, int height)
        {
            var result = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(original, new Rectangle(0, 0, result.Width, result.Height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
            }

            return result;
        }

        public static bool ValidateImage(string filePath)
        {
            Image sourceImage;

            try
            {
                sourceImage = Image.FromFile(filePath);
                sourceImage.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private const string errorMessage = "Could not recognize image format.";

        private static Dictionary<byte[], Func<BinaryReader, Size>> imageFormatDecoders = new Dictionary<byte[], Func<BinaryReader, Size>>()
        {
            { new byte[]{ 0x42, 0x4D }, DecodeBitmap},
            { new byte[]{ 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, DecodeGif },
            { new byte[]{ 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, DecodeGif },
            { new byte[]{ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, DecodePng },
            { new byte[]{ 0xff, 0xd8 }, DecodeJfif },
        };

        /// <summary>
        /// Gets the dimensions of an image.
        /// </summary>
        /// <param name="path">The path of the image to get the dimensions of.</param>
        /// <returns>The dimensions of the specified image.</returns>
        /// <exception cref="ArgumentException">The image was of an unrecognized format.</exception>
        public static Size GetDimensions(string path)
        {
            using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                try
                {
                    return GetDimensions(binaryReader);
                }
                catch (ArgumentException e)
                {
                    if (e.Message.StartsWith(errorMessage))
                    {
                        throw new ArgumentException(errorMessage, "path", e);
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the dimensions of an image.
        /// </summary>
        /// <param name="path">The path of the image to get the dimensions of.</param>
        /// <returns>The dimensions of the specified image.</returns>
        /// <exception cref="ArgumentException">The image was of an unrecognized format.</exception>
        public static Size GetDimensions(BinaryReader binaryReader)
        {
            int maxMagicBytesLength = imageFormatDecoders.Keys.OrderByDescending(x => x.Length).First().Length;

            byte[] magicBytes = new byte[maxMagicBytesLength];

            for (int i = 0; i < maxMagicBytesLength; i += 1)
            {
                magicBytes[i] = binaryReader.ReadByte();

                foreach (var kvPair in imageFormatDecoders)
                {
                    if (magicBytes.StartsWith(kvPair.Key))
                    {
                        return kvPair.Value(binaryReader);
                    }
                }
            }

            throw new ArgumentException(errorMessage, "binaryReader");
        }

        private static bool StartsWith(this byte[] thisBytes, byte[] thatBytes)
        {
            for (int i = 0; i < thatBytes.Length; i += 1)
            {
                if (thisBytes[i] != thatBytes[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static short ReadLittleEndianInt16(this BinaryReader binaryReader)
        {
            byte[] bytes = new byte[sizeof(short)];
            for (int i = 0; i < sizeof(short); i += 1)
            {
                bytes[sizeof(short) - 1 - i] = binaryReader.ReadByte();
            }
            return BitConverter.ToInt16(bytes, 0);
        }

        private static int ReadLittleEndianInt32(this BinaryReader binaryReader)
        {
            byte[] bytes = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); i += 1)
            {
                bytes[sizeof(int) - 1 - i] = binaryReader.ReadByte();
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        private static Size DecodeBitmap(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(16);
            int width = binaryReader.ReadInt32();
            int height = binaryReader.ReadInt32();
            return new Size(width, height);
        }

        private static Size DecodeGif(BinaryReader binaryReader)
        {
            int width = binaryReader.ReadInt16();
            int height = binaryReader.ReadInt16();
            return new Size(width, height);
        }

        private static Size DecodePng(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(8);
            int width = binaryReader.ReadLittleEndianInt32();
            int height = binaryReader.ReadLittleEndianInt32();
            return new Size(width, height);
        }

        private static Size DecodeJfif(BinaryReader binaryReader)
        {
            while (binaryReader.ReadByte() == 0xff)
            {
                byte marker = binaryReader.ReadByte();
                short chunkLength = binaryReader.ReadLittleEndianInt16();

                if (marker == 0xc0)
                {
                    binaryReader.ReadByte();

                    int height = binaryReader.ReadLittleEndianInt16();
                    int width = binaryReader.ReadLittleEndianInt16();
                    return new Size(width, height);
                }

                binaryReader.ReadBytes(chunkLength - 2);
            }

            throw new ArgumentException(errorMessage);
        }

        public static Size GetDimensionFromURL(string imageURL)
        {
            Stream str = null;
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(imageURL);
            HttpWebResponse wRes = (HttpWebResponse)(wReq).GetResponse();
            str = wRes.GetResponseStream();

            var imageOrig = System.Drawing.Image.FromStream(str);

            return new Size(imageOrig.Width, imageOrig.Height);
        }
    }
}