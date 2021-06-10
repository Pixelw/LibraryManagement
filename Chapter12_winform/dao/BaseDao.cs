using Chapter12_winform.utils;

namespace Chapter12_winform.dao {
    
    public abstract class BaseDao {
        protected SqlHelper sqlHelper;
        public BaseDao(SqlHelper sqlHelper) {
            this.sqlHelper = sqlHelper;
            if(sqlHelper.connection.State != System.Data.ConnectionState.Open) {
                sqlHelper.Open();
            }
        }
        

        public void CloseConnection() {
            sqlHelper.Close();
        }
    }
}