using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Chapter12_winform.model;
using Chapter12_winform.utils;

namespace Chapter12_winform.dao {
    public class AdminDao : BaseDao {
        public AdminDao(SqlHelper sqlHelper) : base(sqlHelper) { }

        public int Login(Admin admin) {
            DataTable dataTable = sqlHelper.ExecuteTable("select * from T_Admin where Admin=@ADMIN and password=@PWD",
                new SqlParameter("@ADMIN", admin.name),
                new SqlParameter("@PWD", admin.pwd));
            if (dataTable.Rows.Count > 0) {
                return int.Parse(dataTable.Rows[0][2].ToString());
            }
            else {
                return -1;
            }
        }

        public List<Admin> GetAllAdmins() {
            var datatable = sqlHelper.ExecuteTable("select * from T_Admin");
            List<Admin> admins = new List<Admin>();
            foreach (DataRow datatableRow in datatable.Rows) {
                Admin admin = new Admin(
                    datatableRow[0].ToString().Trim(),
                    datatableRow[1].ToString().Trim(),
                    int.Parse(datatableRow[2].ToString().Trim()));
                admins.Add(admin);
            }

            return admins;
        }

        public bool AddNewAdmin(Admin admin) {
            int i = sqlHelper.ExecuteNonQuery("insert into T_Admin values (@name, @pwd, @role)",
                new SqlParameter("@name", admin.name),
                new SqlParameter("@pwd", admin.pwd),
                new SqlParameter("@role", admin.role));
            return i > 0;
        }

        public bool UpdateAdmin(Admin admin) {
            int i = sqlHelper.ExecuteNonQuery("update T_Admin set password=@P, role=@R where Admin=@A",
                new SqlParameter("@P", admin.pwd),
                new SqlParameter("@R", admin.role),
                new SqlParameter("@A", admin.name));
            return i > 0;
        }

        public bool DeleteAdmin(Admin admin) {
            int i = sqlHelper.ExecuteNonQuery("delete from T_Admin where Admin=@BID",
                new SqlParameter("@BID", admin.name));
            return i > 0;
        }
    }
}