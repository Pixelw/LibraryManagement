using Chapter12_winform.dao;
using Chapter12_winform.model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chapter12_winform {
    public partial class ModifyForm : Form {
        public const int ModeAdd = 10;
        public const int ModeModify = 20;

        private int mode;
        private int type;
        private BaseDao dao;
        public Action OnSubmit;
        private Captcha _captcha;

        public ModifyForm(int mode, int type, BaseDao dao, Action onSubmit) {
            InitializeComponent();
            this.mode = mode;
            this.type = type;
            this.dao = dao;
            OnSubmit = onSubmit;
        }

        /// <summary>
        /// 提交按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            if (!_captcha.IsCorrect(textBox6.Text)) {
                label6.Text = "验证码错误";
                label6.ForeColor = Color.Red;
                return;
            }
            bool success = false;
            try {
                if (dao is BookDao bookDao) {
                    var book = new Book();
                    book.Bid = textBox1.Text.Trim();
                    book.Bname = textBox2.Text;
                    book.Author = textBox3.Text;
                    book.Bpress = textBox4.Text;
                    book.Quantity = int.Parse(textBox5.Text.Trim());
                    if (mode == ModeAdd) {
                        success = bookDao.AddBook(book);
                    }
                    else {
                        success = bookDao.UpdateBook(book);
                    }
                }

                if (dao is UserDao userDao) {
                    var user = new User();
                    user.Uid = textBox1.Text.Trim();
                    user.Uname = textBox2.Text;
                    if (mode == ModeAdd) {
                        success = userDao.AddNewUser(user);
                    }
                    else {
                        success = userDao.UpdateUser(user);
                    }
                }

                if (dao is AdminDao adminDao) {
                    var admin = new Admin(
                        textBox1.Text.Trim(),
                        textBox2.Text.Trim(),
                        int.Parse(comboBox1.SelectedValue.ToString())
                    );
                    if (mode == ModeAdd) {
                        success = adminDao.AddNewAdmin(admin);
                    }
                    else {
                        success = adminDao.UpdateAdmin(admin);
                    }
                }

                if (success) {
                    OnSubmit();
                    Close();
                }
            }
            catch (Exception exception) {
                string s = "发生异常：\n" + exception.Message;
                MessageBox.Show(s);
            }
        }

        public void SetData(string t1, string t2, string t3, string t4, string t5) {
            if (type == ManageForm.TypeAdmin) {
                comboBox1.SelectedValue = int.Parse(t3);
            }
            textBox1.Text = t1;
            textBox2.Text = t2;
            textBox3.Text = t3;
            textBox4.Text = t4;
            textBox5.Text = t5;
        }

        /// <summary>
        /// 根据type和mode设置label和模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyForm_Load(object sender, EventArgs e) {
            switch (type) {
                case ManageForm.TypeBook:
                    labelTitle.Text = "图书";
                    label1.Text = "ID/ISBN";
                    label2.Text = "书名";
                    label3.Text = "作者";
                    label4.Text = "出版社";
                    label5.Text = "数量";
                    label5.Visible = true;
                    textBox5.Visible = true;
                    break;
                case ManageForm.TypeUser:
                    labelTitle.Text = "用户";
                    label1.Text = "ID/身份证";
                    label2.Text = "名字";
                    
                    label3.Visible = false;
                    textBox3.Visible = false;
                    label4.Visible = false;
                    textBox4.Visible = false;
                    break;
                case ManageForm.TypeAdmin:
                    labelTitle.Text = "系统用户";
                    label1.Text = "登录名";
                    label2.Text = "密码";
                    textBox2.UseSystemPasswordChar = true;
                    label3.Text = "角色";
                    textBox3.Visible = false;

                    var bs = new BindingSource();
                    bs.DataSource = Form1.RoleDict;
                    comboBox1.Visible = true;
                    comboBox1.DataSource = bs;
                    comboBox1.DisplayMember = "Value";
                    comboBox1.ValueMember = "Key";

                    label4.Visible = false;
                    textBox4.Visible = false;
                    break;
            }

            switch (mode) {
                case ModeAdd:
                    labelTitle.Text += "添加/入库";
                    break;
                case ModeModify:
                    labelTitle.Text += "信息修改";
                    textBox1.ReadOnly = true;
                    break;
            }

            NewCaptcha();
        }

        public class OnSubmitResult {
            public Action OnSuccess { get; set; }
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            NewCaptcha();
        }

        private void NewCaptcha() {
            _captcha = new Captcha(4, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = _captcha.Image;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char) Keys.Enter) {
                button1_Click(sender, e);
            }
        }
    }
}