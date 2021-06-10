
using System.Windows.Forms;

namespace Chapter12_winform.utils {
    class HeaderBuilder {

        public static ColumnHeader Build(string text, int width) {
            ColumnHeader column = new ColumnHeader {
                Text = text,
                Width = width,
                TextAlign = HorizontalAlignment.Left
            };
            return column;
        }
    }
}
