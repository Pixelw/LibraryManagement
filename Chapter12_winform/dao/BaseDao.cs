using System;
using System.Collections.Generic;
using System.Data;
using Chapter12_winform.utils;

namespace Chapter12_winform.dao {
    
    public abstract class BaseDao<T> {
        protected SqlHelper sqlHelper;
        public BaseDao(SqlHelper sqlHelper) {
            this.sqlHelper = sqlHelper;
            if(sqlHelper.connection.State != ConnectionState.Open) {
                sqlHelper.Open();
            }
        }
        
        public void CloseConnection() {
            sqlHelper.Close();
        }

        public abstract bool Add(T obj);

        public abstract bool Delete(string id);

        public abstract bool Update(T obj);

        public abstract List<T> GetAll();

        public abstract DataTable GetDataTable();

        public bool IllegalArgs() {
            throw new ArgumentException("参数非法");
        }
        
    }
}