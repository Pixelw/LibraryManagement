using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Chapter12_winform {
    public partial class Form1 : Form {
        public const int RoleOperator = 1;

        public const int RoleAdmin = 2;

        public const int RoleSuperUser = 99;

        public static readonly Dictionary<int, string> RoleDict = new Dictionary<int, string>();

        public Form1() {
            InitializeComponent();
            RoleDict.Add(RoleOperator, "图书操作员");
            RoleDict.Add(RoleAdmin, "系统管理员");
            RoleDict.Add(RoleSuperUser, "(调试)超级用户");
        }

        LoginForm loginForm;
        private BorrowForm _borrowForm;
        private ManageForm _bookManage;
        private ManageForm _userManage;
        private ManageForm _adminManage;


        private void Form1_Load(object sender, EventArgs e) {
            IsMdiContainer = true;
            Logout();
            loginForm = new LoginForm(this);
            loginForm.StartPosition = FormStartPosition.CenterParent;
            loginForm.TopMost = true;
            Console.WriteLine("Form1_load");
        }

        private void Form1_Shown(object sender, EventArgs e) {
            loginForm.ShowDialog();
        }

        public void LoginSetRole(int role) {
            Text += "：" + RoleDict[role];
            toolStripStatusLabel1.Text = Text;
            switch (role) {
                case RoleOperator:
                    toolStrip1.Visible = true;
                    break;
                case RoleAdmin:
                    toolStrip2.Visible = true;
                    break;
                case RoleSuperUser:
                    toolStrip1.Visible = true;
                    toolStrip2.Visible = true;
                    break;
                default:
                    MessageBox.Show("未知角色" + role, "错误");
                    break;
            }
        }

        private void Logout() {
            toolStrip1.Visible = false;
            toolStrip2.Visible = false;
        }

        private void Borrow_Click(object sender, EventArgs e) {
            if (_borrowForm != null && !_borrowForm.IsDisposed) {
                return;
            }

            if (_bookManage != null && !_bookManage.IsDisposed) {
                _bookManage.Close();
            }

            if (_userManage != null && !_userManage.IsDisposed) {
                _userManage.Close();
            }

            var right = new RightContainerForm();
            right.MdiParent = this;
            right.Show();

            _borrowForm = new BorrowForm();
            _borrowForm.MdiParent = this;
            // 设置右侧点击事件， 使用delegate event 传递到左侧
            right.Book.SelectBook += _borrowForm.SetBook;
            right.User.SelectUser += _borrowForm.SetUser;
            _borrowForm.Show();

            LayoutMdi(MdiLayout.TileVertical);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            Program.Quit(e);
        }

        // book
        private void toolStripButton1_Click(object sender, EventArgs e) {
            _bookManage = new ManageForm(ManageForm.TypeBook);
            _bookManage.MdiParent = this;
            _bookManage.Show();
        }

        // user
        private void toolStripButton2_Click(object sender, EventArgs e) {
            _userManage = new ManageForm(ManageForm.TypeUser);
            _userManage.MdiParent = this;
            _userManage.Show();
        }

        //admin
        private void toolStripButton3_Click(object sender, EventArgs e) {
            _adminManage = new ManageForm(ManageForm.TypeAdmin);
            _adminManage.MdiParent = this;
            _adminManage.Show();
        }

        private void Form1_Resize(object sender, EventArgs e) {
            if (_borrowForm != null && !_borrowForm.IsDisposed) {
                LayoutMdi(MdiLayout.TileVertical);
            }
        }

        private void 借书和还书BToolStripMenuItem_Click(object sender, EventArgs e) {
            toolStripButton1_Click(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e) {
            toolStripStatusLabel3.Text = DateTime.Now.ToString();
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {

        }
    }
}