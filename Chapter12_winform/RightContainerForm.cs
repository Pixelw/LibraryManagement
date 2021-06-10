using System;
using System.Windows.Forms;

namespace Chapter12_winform {
    public partial class RightContainerForm : Form {
       public ManageForm Book { get; set; }
       public ManageForm User { get; set; }


        public RightContainerForm() {
            InitializeComponent();
        }
        
        private void RightContainerForm_Load(object sender, EventArgs e) {
            Book = new ManageForm(ManageForm.TypeBook);
            Book.TopLevel = false;
            Book.Dock = DockStyle.Fill;
            Book.FormBorderStyle = FormBorderStyle.None;
            splitContainer1.Panel1.Controls.Clear();
            splitContainer1.Panel1.Controls.Add(Book);
            User = new ManageForm(ManageForm.TypeUser);
            User.TopLevel = false;
            User.FormBorderStyle = FormBorderStyle.None;
            User.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(User);

            Book.Show();
            User.Show();

            Text = "选择书籍和用户";
        }
    }
}