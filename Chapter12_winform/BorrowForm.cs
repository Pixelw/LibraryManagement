using Chapter12_winform.model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Chapter12_winform.dao;
using Chapter12_winform.utils;
using System.Drawing;

namespace Chapter12_winform {
    public partial class BorrowForm : Form {
        private BorrowDao _borrowDao;
        private Book _selectBook;
        private User _selectUser;
        private SqlDataAdapter _sqlDataAdapter;
        private DataSet _dataSet = new DataSet();

        public BorrowForm() {
            InitializeComponent();
            _borrowDao = new BorrowDao(Program.SqlHelper);
        }

        private void BorrowForm_Load(object sender, EventArgs e) {
            dateTimePicker2.MaxDate = DateTime.Now.AddDays(100);
            dateTimePicker2.MinDate = DateTime.Now;
            SetPrompt();

            _sqlDataAdapter = _borrowDao.GetAllSda();
            RefreshSheet();
        }


        public void SetBook(Book book) {
            _selectBook = book;
            label2.Text = book.Bid + "\n" + book.Bname;
            SetPrompt();
        }

        private void SetPrompt() {
            string str = "请选择";
            if (_selectBook == null) {
                label5.ForeColor = Color.Crimson;
                str += "书籍";
            }

            if (_selectUser == null) {
                if (str.Length > 3) {
                    str += "和";
                }

                str += "用户";
            }

            label5.Text = str;
            if (_selectBook != null && _selectUser != null) {
                label5.Text = "可以借阅";
                label5.ForeColor = Color.MediumSeaGreen;
            }
        }

        public void SetUser(User user) {
            _selectUser = user;
            label3.Text = user.Uid + '\n' + user.Uname;
            SetPrompt();
        }


        private void button1_Click(object sender, EventArgs e) {
            if (_selectBook == null || _selectUser == null) return;
            var now = TimeUtils.CurrentTimeMillis();
            long returnDate;
            if (radioButton1.Checked) {
                returnDate = TimeUtils.ToMillis(dateTimePicker2.Value);
            }
            else {
                returnDate = TimeUtils.ToMillis(DateTime.Now.AddDays((double) numericUpDown1.Value));
            }

            var ok = _borrowDao.Add(new Borrow(
                _selectBook.Bid,
                _selectUser.Uid,
                now,
                returnDate
            ));
            if (ok) {
                MessageBox.Show("借阅成功");
            }
            else {
                MessageBox.Show("借阅失败");
            }

            RefreshSheet();
        }

        private void RefreshSheet() {
            // dataGridView1.SuspendLayout();

            _dataSet.Clear();
            _sqlDataAdapter.Fill(_dataSet);
            dataGridView1.DataSource = _dataSet.Tables[0];
            // dataGridView1.ResumeLayout();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString())) {
                e.FormattingApplied = false;
                return;
            }

            if (e.ColumnIndex == 3 || e.ColumnIndex == 5) {
                e.Value = TimeUtils.ToDateTimeString(long.Parse(e.Value.ToString().Trim()));
                e.FormattingApplied = true;
            }
            else {
                e.FormattingApplied = false;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            var ok = MessageBox.Show("归还此书?", "确认", MessageBoxButtons.OKCancel);
            if (ok == DialogResult.OK) {
                var success = _borrowDao.ReturnABook(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                if (success) {
                    RefreshSheet();
                }
                else {
                    MessageBox.Show("操作失败");
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            var ok = MessageBox.Show("删除这个记录?", "确认", MessageBoxButtons.OKCancel);
            if (ok == DialogResult.OK) {
                var success = _borrowDao.Delete(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                if (success) {
                    RefreshSheet();
                }
                else {
                    MessageBox.Show("操作失败");
                }
            }
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char) Keys.Enter) {
                toolStripButton1_Click(sender, e);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            string rowFilter = string.Format("[{1}] LIKE '%{0}%' OR [{2}] LIKE '%{0}%'", toolStripTextBox1.Text, "用户", "书籍");
            ((DataTable) dataGridView1.DataSource).DefaultView.RowFilter = rowFilter;
            dataGridView1.Refresh();
        }
    }
}