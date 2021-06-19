using System;
using System.Collections.Generic;
using System.Data;
using Chapter12_winform.model;
using Chapter12_winform.utils;

namespace Chapter12_winform.dao {
    
    public abstract class BaseDao {
        protected SqlHelper sqlHelper;
        public BaseDao(SqlHelper sqlHelper) {
            this.sqlHelper = sqlHelper;
        }
        
        public abstract bool Add(Models obj);

        public abstract bool Delete(string id);

        public abstract bool Update(Models obj);

        public abstract List<Models> GetAll();

        public abstract DataTable GetDataTable();

        public bool IllegalArgs() {
            throw new ArgumentException("参数非法");
        }
        
    }
}