using System;
using System.Drawing;
using Chapter12_winform.utils;

namespace Chapter12_winform.model {
    public class Captcha {
        public Image Image { get; set; }
        public string Text { get; set; }

        public bool IsCorrect(string input) {
            return string.Equals(Text, input, StringComparison.CurrentCultureIgnoreCase);
        }

        public Captcha(Image image, string text) {
            Image = image;
            Text = text;
        }

        public Captcha(int length, int width, int height) {
            Text = CaptchaGenerator.RandomText(length);
            Image = CaptchaGenerator.Create(width, height, Text);
        }
    }
}