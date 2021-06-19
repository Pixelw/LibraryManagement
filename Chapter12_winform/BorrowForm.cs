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
        private BookDao _bookDao;
        private UserDao _userDao;
        private Book _selectBook;
        private User _selectUser;
        private SqlDataAdapter _sqlDataAdapter;
        private DataSet _dataSet = new DataSet();

        public delegate void ManageFormRefreshDelegate();

        public event ManageFormRefreshDelegate RefreshDelegate;

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


        public void SetBorrowInfo(Models models) {
            if (models is Book book) {
                _selectBook = book;
                label2.Text = book.Bid + "\n" + book.Bname;
            }
            else if (models is User user) {
                _selectUser = user;
                label3.Text = user.Uid + '\n' + user.Uname;
            }

            SetPrompt();
        }

        private void SetPrompt() {
            button1.Enabled = false;
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

            if (_selectBook != null && _selectUser != null) {
                if (_selectBook.Quantity > 0) {
                    str = "可以借阅";
                    label5.ForeColor = Color.MediumSeaGreen;
                    button1.Enabled = true;
                }
                else {
                    str = "余量不足";
                    label5.ForeColor = Color.Crimson;
                }
            }

            label5.Text = str;
        }

        /// <summary>
        /// 借阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            if (_selectBook == null || _selectUser == null) return;
            if (_userDao == null) {
                _userDao = new UserDao(Program.SqlHelper);
            }

            if (_bookDao == null) {
                _bookDao = new BookDao(Program.SqlHelper);
            }

            var now = TimeUtils.CurrentTimeMillis();
            long returnDate;
            if (radioButton1.Checked) {
                returnDate = TimeUtils.ToMillis(dateTimePicker2.Value);
            }
            else {
                returnDate = TimeUtils.ToMillis(DateTime.Now.AddDays((double) numericUpDown1.Value));
            }

            var borrow = _borrowDao.Add(new Borrow(
                _selectBook.Bid,
                _selectUser.Uid,
                now,
                returnDate
            ));
            _selectBook.Quantity--;
            var book = _bookDao.Update(_selectBook);
            _selectUser.Count++;
            var user = _userDao.Update(_selectUser);
 
            if (borrow && book && user) {
                MessageBox.Show("借阅成功");
                RefreshDelegate?.Invoke();
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

            if (e.ColumnIndex == 4 || e.ColumnIndex == 6) {
                e.Value = TimeUtils.ToDateTimeString(long.Parse(e.Value.ToString().Trim()));
                e.FormattingApplied = true;
            }
            else {
                e.FormattingApplied = false;
            }
        }

        /// <summary>
        /// 还书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e) {
            var ok = MessageBox.Show("归还此书?", "确认", MessageBoxButtons.OKCancel);
            if (ok == DialogResult.OK) {
                if (_bookDao == null) {
                    _bookDao = new BookDao(Program.SqlHelper);
                }

                var success = _borrowDao.ReturnABook(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                var book = _bookDao.GetBook(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
                if (book != null) {
                    book.Quantity++;
                    var bookOk = _bookDao.Update(book);
                    RefreshDelegate?.Invoke();
                }
                else {
                    MessageBox.Show("书库内没有该书", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

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
            string rowFilter = string.Format(
                "[{1}] LIKE '%{0}%' OR [{2}] LIKE '%{0}%' OR [{3}] LIKE '%{0}%'",
                toolStripTextBox1.Text, "用户", "书籍", "书籍ID");
            ((DataTable) dataGridView1.DataSource).DefaultView.RowFilter = rowFilter;
            dataGridView1.Refresh();
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            PrintHelper printHelper = new PrintHelper();
            printHelper.Titles = new string[] {Text};
            printHelper.PrintDataTable(_borrowDao.GetDataTable());
        }

        private void toolStripButton5_Click(object sender, EventArgs e) {
            ExcelUtils.ExportToExcel(_borrowDao.GetDataTable(),
                Text + DateTime.Now.ToString("yyyy-M-d hh-mm-ss") + ".csv", true);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
            if (dataGridView1.SelectedRows.Count > 0) {
                var onBorrow = bool.Parse(dataGridView1.SelectedRows[0].Cells[5].Value.ToString());
                toolStripButton3.Enabled = !onBorrow;
                toolStripButton2.Enabled = onBorrow;
            }
        }
    }
}