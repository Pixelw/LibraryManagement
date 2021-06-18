using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Chapter12_winform.model;
using Chapter12_winform.utils;

namespace Chapter12_winform.dao {
    public class BorrowDao : BaseDao {
        public BorrowDao(SqlHelper sqlHelper) : base(sqlHelper) { }

        private static List<Models> NewList(DataTable dataTable) {
            var borrows = new List<Models>();
            foreach (DataRow row in dataTable.Rows) {
                Borrow borrow = new Borrow();
                borrow.Bid = row[0].ToString().Trim();
                borrow.Uid = row[1].ToString().Trim();
                borrow.Date = long.Parse(row[2].ToString().Trim());
                borrow.OnBorrow = int.Parse(row[3].ToString().Trim()) == 1;
                borrow.ReturnDate = long.Parse(row[4].ToString().Trim());
                borrows.Add(borrow);
            }

            return borrows;
        }

        public bool ReturnABook(string id) {
            int i = sqlHelper.ExecuteNonQuery(
                "update T_Borrow set onBorrow=0 where id=@ID",
                new SqlParameter("@ID", id));
            return i > 0;
        }

        public SqlDataAdapter GetAllSda() {
            return new SqlDataAdapter(
                "select B.id as ID, O.Bname as 书籍,U.Uname as 用户, B.date as 借阅日期, B.onBorrow as 正在借阅, B.returnDate as 预定归还日期 from T_Borrow B join T_book O on B.Bid collate Chinese_PRC_90_CI_AI_KS_SC_UTF8= O.Bid collate Chinese_PRC_90_CI_AI_KS_SC_UTF8 join T_user U on B.Uid collate  Chinese_PRC_90_CI_AI_KS_SC_UTF8= U.Uid collate Chinese_PRC_90_CI_AI_KS_SC_UTF8",
                sqlHelper.connection);
        }


        public override bool Add(Models obj) {
            if (obj is Borrow borrow) {
                int i = sqlHelper.ExecuteNonQuery("insert into T_Borrow values (@BID, @UID, @DATE, 1, @RED)",
                    new SqlParameter("@BID", borrow.Bid),
                    new SqlParameter("@UID", borrow.Uid),
                    new SqlParameter("@DATE", borrow.Date),
                    new SqlParameter("@RED", borrow.ReturnDate));
                return i > 0;
            }

            return IllegalArgs();
        }

        public override bool Delete(string id) {
            int i = sqlHelper.ExecuteNonQuery("delete from T_Borrow where id=@BID",
                new SqlParameter("@BID", id));
            return i > 0;
        }

        public override bool Update(Models obj) {
            if (obj is Borrow borrow) {
                int i = sqlHelper.ExecuteNonQuery(
                    "update T_Borrow set Uid=@UID, date=@DATE, onBorrow=@on, returnDate=@RED where Bid=@BID",
                    new SqlParameter("@BID", borrow.Bid),
                    new SqlParameter("@UID", borrow.Uid),
                    new SqlParameter("@DATE", borrow.Date),
                    new SqlParameter("@RED", borrow.ReturnDate),
                    new SqlParameter("@on", borrow.OnBorrow ? 1 : 0));
                return i > 0;
            }

            return IllegalArgs();
        }
        

        public override List<Models> GetAll() {
            var dataTable = sqlHelper.ExecuteTable("select * from T_Borrow");
            return NewList(dataTable);
        }

        public override DataTable GetDataTable() {
            return sqlHelper.ExecuteTable("select * from T_Borrow");
        }
    }
}