using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chapter12_winform {
    public partial class ServerForm : Form {
        public ServerForm() {
            InitializeComponent();
        }
        private void ServerForm_Load(object sender, EventArgs e) {
            textBox1.Text = Program.SqlHelper.StringBuilder.DataSource;
            textBox2.Text = "51433";
            textBox3.Text = Program.SqlHelper.StringBuilder.UserID;
            textBox4.Text = Program.SqlHelper.StringBuilder.InitialCatalog;
        }
    }
}
