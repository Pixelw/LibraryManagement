using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Chapter12_winform.model;
using Chapter12_winform.utils;

namespace Chapter12_winform.dao {
    public class UserDao : BaseDao {
        public UserDao(SqlHelper sqlHelper) : base(sqlHelper) { }

        public User GetUser(String uid) {
            User user = new User();
            DataTable dataTable = sqlHelper.ExecuteTable("select * from T_user where Uid=@UID",
                new SqlParameter("@UID", uid));
            if (dataTable.Rows.Count <= 0) {
                return null;
            }
            else {
                user.Uid = dataTable.Rows[0][0].ToString().Trim();
                user.Uname = dataTable.Rows[0][1].ToString().Trim();
                return user;
            }
        }
        
        public override bool Add(Models obj) {
            if (obj is User user) {
                int i = sqlHelper.ExecuteNonQuery("insert into T_user values (@UID, @NAME,0)",
                    new SqlParameter("@UID", user.Uid),
                    new SqlParameter("@NAME", user.Uname));
                return i > 0;
            }

            return IllegalArgs();
        }

        public override bool Delete(string id) {
            int i = sqlHelper.ExecuteNonQuery("delete from T_user where Uid=@BID",
                new SqlParameter("@BID", id));
            return i > 0;
        }

        public override bool Update(Models obj) {
            if (obj is User user) {
                int i = sqlHelper.ExecuteNonQuery("update T_user set Uname=@NAME, count=@C where Uid=@UID",
                    new SqlParameter("@NAME", user.Uname),
                    new SqlParameter("@UID", user.Uid),
                    new SqlParameter("@C", user.Count));
                return i > 0;
            }

            return IllegalArgs();
        }
        

        public override List<Models> GetAll() {
            var users = new List<Models>();
            DataTable dataTable = sqlHelper.ExecuteTable("select * from T_user");
            foreach (DataRow row in dataTable.Rows) {
                int c = string.IsNullOrEmpty(row[2].ToString().Trim()) ? 0 : int.Parse(row[2].ToString().Trim());
                users.Add(new User(row[0].ToString().Trim(),
                    row[1].ToString().Trim(),
                    c
                ));
            }
            return users;
        }

        public override DataTable GetDataTable() {
            return sqlHelper.ExecuteTable("select * from T_user");
        }
    }
}