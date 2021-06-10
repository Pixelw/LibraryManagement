using Chapter12_winform.dao;
using Chapter12_winform.model;
using Chapter12_winform.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Chapter12_winform {
    public partial class ManageForm : Form {
        public const int TypeBook = 1;
        public const int TypeUser = 2;
        public const int TypeAdmin = 3;

        private readonly int type;
        private BaseDao dao;
        private List<Book> _books;
        private List<User> _users;
        private List<Admin> _admins;

        public delegate void SetBookDelegate(Book book);

        public delegate void SetUserDelegate(User user);

        public event SetBookDelegate SelectBook;
        public event SetUserDelegate SelectUser;


        public ManageForm(int manageType) {
            InitializeComponent();
            type = manageType;
        }

        private void ManageForm_Shown(object sender, EventArgs e) {
            SetupData();
        }

        private void SetupData() {
            listView1.Clear();
            listView1.View = View.Details;
            SetupColumn();
            LoadData();
        }

        internal void LoadData() {
            listView1.BeginUpdate();

            listView1.Items.Clear();
            if (dao is BookDao bookDao) {
                _books = bookDao.GetAllBooks();
                foreach (Book book in _books) {
                    var item = NewBookLvi(book);
                    listView1.Items.Add(item);
                }
            }
            else if (dao is UserDao userDao) {
                _users = userDao.GetAllUsers();
                foreach (var user in _users) {
                    listView1.Items.Add(NewUserLvi(user));
                }
            }
            else if (dao is AdminDao adminDao) {
                _admins = adminDao.GetAllAdmins();
                foreach (var admin in _admins) {
                    NewAdminLvi(admin);
                }
            }

            listView1.EndUpdate();
        }

        private static ListViewItem NewBookLvi(Book book) {
            ListViewItem item = new ListViewItem();
            item.Text = book.Bid;
            item.SubItems.Add(book.Bname);
            item.SubItems.Add(book.Author);
            item.SubItems.Add(book.Bpress);
            item.SubItems.Add(book.Quantity.ToString());
            return item;
        }

        private ListViewItem NewUserLvi(User user) {
            ListViewItem item = new ListViewItem();
            item.Text = user.Uid;
            item.SubItems.Add(user.Uname);
            return item;
        }

        private ListViewItem NewAdminLvi(Admin admin) {
            ListViewItem item = new ListViewItem();
            item.Text = admin.name;
            item.SubItems.Add(string.IsNullOrEmpty(admin.pwd) ? "空" : "有");
            item.SubItems.Add(Form1.RoleDict[admin.role]);
            return item;
        }

        private void SetupColumn() {
            var cs = listView1.Columns;
            cs.Clear();
            switch (type) {
                case TypeBook:
                    cs.Add(HeaderBuilder.Build("ID", 40));
                    cs.Add(HeaderBuilder.Build("书名", 200));
                    cs.Add(HeaderBuilder.Build("作者", 100));
                    cs.Add(HeaderBuilder.Build("出版社", 100));
                    cs.Add(HeaderBuilder.Build("数量", 50));
                    dao = new BookDao(Program.SqlHelper);
                    break;
                case TypeUser:
                    cs.Add(HeaderBuilder.Build("ID", 100));
                    cs.Add(HeaderBuilder.Build("姓名", 100));
                    dao = new UserDao(Program.SqlHelper);
                    break;
                case TypeAdmin:
                    cs.Add(HeaderBuilder.Build("用户名", 150));
                    cs.Add(HeaderBuilder.Build("密码", 50));
                    cs.Add(HeaderBuilder.Build("角色", 50));
                    dao = new AdminDao(Program.SqlHelper);
                    break;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count > 0) {
                toolStripButton2.Enabled = true;
                toolStripButton3.Enabled = true;
                if (type == TypeBook && SelectBook != null) {
                    SelectBook(_books[listView1.SelectedIndices[0]]);
                }

                if (type == TypeUser && SelectUser != null) {
                    SelectUser(_users[listView1.SelectedIndices[0]]);
                }
            }
            else {
                toolStripButton2.Enabled = false;
                toolStripButton3.Enabled = false;
            }
        }

        //add
        private void toolStripButton1_Click(object sender, EventArgs e) {
            var add = new ModifyForm(ModifyForm.ModeAdd, type, dao, () => { LoadData(); });

            add.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            var modify = new ModifyForm(ModifyForm.ModeModify, type, dao, () => { LoadData(); });
            modify.Show();
            if (type == TypeAdmin) {
                var admin = _admins[listView1.SelectedIndices[0]];
                modify.SetData(admin.name, admin.pwd, admin.role.ToString(), "", "");
            }
            else if (type == TypeBook) {
                var book = _books[listView1.SelectedIndices[0]];
                modify.SetData(book.Bid, book.Bname, book.Author, book.Bpress, book.Quantity.ToString());
            }
            else if (type == TypeUser) {
                var user = _users[listView1.SelectedIndices[0]];
                modify.SetData(user.Uid, user.Uname, user.Count.ToString(), "", "");
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            string targetDesc = "确定删除此项?\n" + listView1.SelectedItems[0].SubItems[0].Text;
            var result = MessageBox.Show(targetDesc, "删除确认",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK) {
                return;
            }

            bool ok = false;
            if (dao is BookDao bookDao) {
                ok = bookDao.DeleteBook(_books[listView1.SelectedIndices[0]].Bid);
            }

            if (dao is UserDao userDao) {
                ok = userDao.DeleteUser(_users[listView1.SelectedIndices[0]]);
            }

            if (dao is AdminDao adminDao) {
                ok = adminDao.DeleteAdmin(_admins[listView1.SelectedIndices[0]]);
            }

            if (ok) {
                LoadData();
            }
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char) Keys.Enter) {
                toolStripTextBox1_TextChanged(sender, e);
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e) {
            if (dao is AdminDao) {
                listView1.Items.Clear();
                listView1.Items.AddRange(_admins.Where(
                    i => string.IsNullOrEmpty(toolStripTextBox1.Text)
                         || i.ToString().Contains(toolStripTextBox1.Text)
                ).Select(c => NewAdminLvi(c)).ToArray());
            }

            if (dao is BookDao) {
                listView1.Items.Clear();
                listView1.Items.AddRange(_books.Where(
                    i => string.IsNullOrEmpty(toolStripTextBox1.Text)
                         || i.ToString().Contains(toolStripTextBox1.Text)
                ).Select(c => NewBookLvi(c)).ToArray());
            }

            if (dao is UserDao) {
                listView1.Items.Clear();
                listView1.Items.AddRange(_users.Where(
                    i => string.IsNullOrEmpty(toolStripTextBox1.Text)
                         || i.ToString().Contains(toolStripTextBox1.Text)
                ).Select(c => NewUserLvi(c)).ToArray());
            }
        }
    }
}