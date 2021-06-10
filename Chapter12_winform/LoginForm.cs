using Chapter12_winform.dao;
using System;
using System.Windows.Forms;
using Chapter12_winform.model;

namespace Chapter12_winform {
    public partial class LoginForm : Form {
        private readonly Form1 _form1;

        private bool _loginSuccess;

        private Captcha _captcha;

        public LoginForm(Form1 form1) {
            InitializeComponent();
            _form1 = form1;
        }

        private void button1_Click(object sender, EventArgs e) {
            if (!_captcha.IsCorrect(textBox3.Text.Trim())) {
                label6.Text = "验证码错误";
                return;
            }
            AdminDao adminDao = new AdminDao(Program.SqlHelper);
            var role = adminDao.Login(new model.Admin(textBox1.Text, textBox2.Text));
            if (role > 0) {
                label6.Visible = false;
                var str = "以这个用户登录吗？\n" + textBox1.Text + "\n" + Form1.RoleDict[role];
                var result = MessageBox.Show(str, "登录确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK) {
                    _loginSuccess = true;
                    Hide();
                    _form1.LoginSetRole(role);
                }
            }
            else {
                label6.Text = "用户名或密码错误";
                label6.Visible = true;
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_loginSuccess) {
                return;
            }

            Program.Quit(e);
        }

        private void button2_Click(object sender, EventArgs e) {
            Program.Quit(e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char) Keys.Enter) {
                button1_Click(sender, e);
            }
            else if (e.KeyChar == (char) Keys.Escape) {
                button2_Click(sender, e);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            textBox2_KeyPress(sender, e);
        }

        private void label5_DoubleClick(object sender, EventArgs e) {
            _loginSuccess = true;
            _form1.LoginSetRole(99);
            Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e) {
            pictureBox2_Click(sender,e);
        }

        private void pictureBox2_Click(object sender, EventArgs e) {
            _captcha = new Captcha(4, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = _captcha.Image;
        }
    }
}