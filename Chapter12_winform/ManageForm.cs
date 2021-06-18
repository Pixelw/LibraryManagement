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

        private readonly int manageType;
        private BaseDao dao;

        private List<Models> _list;

        public delegate void SelectorDelegate(Models models);


        public event SelectorDelegate Selector;


        public ManageForm(int manageType) {
            InitializeComponent();
            this.manageType = manageType;
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
            _list = dao.GetAll();
            foreach (var entity in _list) {
                listView1.Items.Add(NewLvi(entity));
            }

            listView1.EndUpdate();
        }

        private static ListViewItem NewLvi(Models models) {
            ListViewItem item = new ListViewItem();
            if (models is Book book) {
                item.Text = book.Bid;
                item.SubItems.Add(book.Bname);
                item.SubItems.Add(book.Author);
                item.SubItems.Add(book.Bpress);
                item.SubItems.Add(book.Quantity.ToString());
            }
            else if (models is User user) {
                item.Text = user.Uid;
                item.SubItems.Add(user.Uname);
            }
            else if (models is Admin admin) {
                item.Text = admin.Name;
                item.SubItems.Add(string.IsNullOrEmpty(admin.pwd) ? "空" : "有");
                item.SubItems.Add(Form1.RoleDict[admin.role]);
            }

            return item;
        }

        private void SetupColumn() {
            var cs = listView1.Columns;
            cs.Clear();
            switch (manageType) {
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
                // 调用委托回调，到借书界面设置信息
                Selector?.Invoke(_list[listView1.SelectedIndices[0]]);
            }
            else {
                toolStripButton2.Enabled = false;
                toolStripButton3.Enabled = false;
            }
        }

        //add
        private void toolStripButton1_Click(object sender, EventArgs e) {
            var add = new ModifyForm(ModifyForm.ModeAdd, manageType, dao, () => { LoadData(); });

            add.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            var modify = new ModifyForm(ModifyForm.ModeModify, manageType, dao, () => { LoadData(); });
            modify.Show();
            if (_list[listView1.SelectedIndices[0]] is Admin admin) {
                modify.SetData(admin.Name, admin.pwd, admin.role.ToString(), "", "");
            }
            else if (_list[listView1.SelectedIndices[0]] is Book book) {
                modify.SetData(book.Bid, book.Bname, book.Author, book.Bpress, book.Quantity.ToString());
            }
            else if (_list[listView1.SelectedIndices[0]] is User user) {
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

            bool ok = dao.Delete(_list[listView1.SelectedIndices[0]].id);

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
                listView1.Items.AddRange(_list.Where(
                    i => string.IsNullOrEmpty(toolStripTextBox1.Text)
                         || i.ToString().Contains(toolStripTextBox1.Text)
                ).Select(c => NewLvi(c)).ToArray());
            }

            if (dao is BookDao) {
                listView1.Items.Clear();
                listView1.Items.AddRange(_list.Where(
                    i => string.IsNullOrEmpty(toolStripTextBox1.Text)
                         || i.ToString().Contains(toolStripTextBox1.Text)
                ).Select(c => NewLvi(c)).ToArray());
            }

            if (dao is UserDao) {
                listView1.Items.Clear();
                listView1.Items.AddRange(_list.Where(
                    i => string.IsNullOrEmpty(toolStripTextBox1.Text)
                         || i.ToString().Contains(toolStripTextBox1.Text)
                ).Select(c => NewLvi(c)).ToArray());
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e) { }
    }
}