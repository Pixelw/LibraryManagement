using Chapter12_winform.utils;
using System;
using System.Windows.Forms;

namespace Chapter12_winform {
    static class Program {
        public static SqlHelper SqlHelper;
        private static bool _quitting = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            SqlHelper = new SqlHelper();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void Quit(EventArgs e) {
            if (_quitting) {
                return;
            }
            
            var dr = MessageBox.Show("是否退出本系统?", "提示",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dr == DialogResult.OK) {
                _quitting = true;
                SqlHelper.Close();
                Application.Exit();
            }
            else {
                if (e is FormClosingEventArgs args) {
                    args.Cancel = true;
                }
            }
        }
    }
}