using System;
using System.Windows.Forms;

namespace Chapter12_winform {
    public partial class AboutForm : Form {
        public AboutForm() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            Hide();
        }
    }
}
