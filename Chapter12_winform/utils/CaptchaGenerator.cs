using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Chapter12_winform.utils {
    public class CaptchaGenerator {
        private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";

        private static readonly string[] AFontNames = new string[] {
            "Arial",
            "Times New Roman",
            "Verdana",
        };

        private static readonly FontStyle[] AFontStyles = new FontStyle[] {
            FontStyle.Bold,
            FontStyle.Italic,
            FontStyle.Regular,
            FontStyle.Strikeout,
            FontStyle.Underline
        };

        private static readonly HatchStyle[] AHatchStyles = new HatchStyle[] {
            HatchStyle.BackwardDiagonal, HatchStyle.Cross,
            HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
            HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
            HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
            HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid,
            HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
            HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard,
            HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
            HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal,
            HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
            HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal,
            HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
            HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard,
            HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
            HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis,
            HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
            HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
        };

        public static string RandomText(int length) {
            var stringChars = new char[length];
            var random = new Random();
            for (var i = 0; i < stringChars.Length; i++) {
                stringChars[i] = Chars[random.Next(Chars.Length)];
            }

            return new String(stringChars);
        }

        public static Image Create(int imgWidth, int imgHeight, string sCaptchaText) {
            var oRandom = new Random();

            var aBackgroundNoiseColor = new int[] {150, 150, 150};
            var aTextColor = new int[] {0, 0, 0};
            var aFontEmSizes = new int[] {15, 20, 25, 30};


            Bitmap oOutputBitmap = new Bitmap(imgWidth, imgHeight, PixelFormat.Format24bppRgb);
            Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            RectangleF oRectangleF = new RectangleF(0, 0, imgWidth, imgHeight);
            Brush oBrush = new HatchBrush(AHatchStyles[oRandom.Next
                (AHatchStyles.Length - 1)], Color.FromArgb(oRandom.Next(100, 255),
                oRandom.Next(100, 255), oRandom.Next(100, 255)), Color.White);
            oGraphics.FillRectangle(oBrush, oRectangleF);

            var oMatrix = new Matrix();
            for (int i = 0; i <= sCaptchaText.Length - 1; i++) {
                oMatrix.Reset();
                int iChars = sCaptchaText.Length;
                int x = imgWidth / (iChars + 1) * i;
                int y = imgHeight / 2;

                //Rotate
                oMatrix.RotateAt(oRandom.Next(-40, 40), new PointF(x, y));
                oGraphics.Transform = oMatrix;

                //文字
                oGraphics.DrawString
                (
                    sCaptchaText.Substring(i, 1),
                    new Font(AFontNames[oRandom.Next(AFontNames.Length - 1)],
                        aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                        AFontStyles[oRandom.Next(AFontStyles.Length - 1)]),
                    new SolidBrush(Color.FromArgb(oRandom.Next(0, 100),
                        oRandom.Next(0, 100), oRandom.Next(0, 100))),
                    x,
                    oRandom.Next((int) (imgHeight * 0.05), (int) (imgHeight * 0.35))
                );
                oGraphics.ResetTransform();
            }

            return oOutputBitmap;
        }
    }
}