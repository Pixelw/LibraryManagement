using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chapter12_winform.utils {
    public class SqlHelper {
        public SqlConnection Connection;
        public SqlConnectionStringBuilder StringBuilder;

        public SqlHelper() {
            StringBuilder = new SqlConnectionStringBuilder();
            StringBuilder.UserID = "Liyang";
            StringBuilder.Password = "aaaa123789";
            StringBuilder.DataSource = "lapi.pixelw.tech,51433";
            StringBuilder.InitialCatalog = "library";
            Init();
        }

        public delegate void SetStateDelegate(string text);

        public event SetStateDelegate SetStateText;

        public void Init() {
            ShowState("初始化数据库连接");
            Connection = new SqlConnection();
            Connection.ConnectionString = StringBuilder.ConnectionString;
            Connection.StateChange += OnStateChange;
        }
        

        public void Open() {
            Connection.Open();
        }

        public void Close() {
            Connection.Close();
        }

        public int ExecuteNonQuery(String sqlStr, params SqlParameter[] parameters) {
            CheckConnection(sqlStr);
            SqlCommand command = Connection.CreateCommand();
            command.CommandText = sqlStr;
            command.Parameters.AddRange(parameters);
            var executeNonQuery = command.ExecuteNonQuery();
            ClearState();
            return executeNonQuery;
        }

        public object ExecuteScalar(String sqlStr) {
            CheckConnection(sqlStr);
            SqlCommand command = Connection.CreateCommand();
            command.CommandText = sqlStr;
            var executeScalar = command.ExecuteScalar();
            ClearState();
            return executeScalar;
        }

        public object ExecuteScalar(String sqlStr, params SqlParameter[] parameters) {
            CheckConnection(sqlStr);
            SqlCommand command = Connection.CreateCommand();
            command.CommandText = sqlStr;
            command.Parameters.AddRange(parameters);
            return command.ExecuteScalar();
        }

        public object ExecuteScalar(SqlParameter[] sqlParameters, String sp_name) {
            CheckConnection(sp_name);
            SqlCommand command = Connection.CreateCommand();
            command.CommandText = sp_name;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(sqlParameters);
            return command.ExecuteScalar();
        }

        public DataTable ExecuteTable(String sqlStr) {
            CheckConnection(sqlStr);
            SqlCommand command = Connection.CreateCommand();
            command.CommandText = sqlStr;
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = command;
            DataTable table = new DataTable();
            dataAdapter.Fill(table);
            ClearState();
            return table;
        }

        public DataTable ExecuteTable(string sqlStr, params SqlParameter[] parameters) {
            CheckConnection(sqlStr);
            SqlCommand command = Connection.CreateCommand();
            command.CommandText = sqlStr;
            command.Parameters.AddRange(parameters);
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = command;
            DataTable table = new DataTable();
            dataAdapter.Fill(table);
            ClearState();
            return table;
        }
        
        private void OnStateChange(object sender, StateChangeEventArgs e) {
            switch (e.CurrentState) {
                case ConnectionState.Broken:
                    ShowState("与数据库的连接中断");
                    break;
                case ConnectionState.Closed:
                    ShowState("与数据库的连接关闭");
                    break;
                case ConnectionState.Connecting:
                    ShowState("连接中");
                    break;
                case ConnectionState.Open:
                    ShowState("");
                    break;
            }
        }


        private void CheckConnection(string sql) {
            if (Connection == null) {
                Init();
            }

            switch (Connection.State) {
                case ConnectionState.Broken:
                case ConnectionState.Closed:
                    ShowState("尝试连接...");
                    Close();
                    Open();
                    break;
            }

            ShowSqlState(sql);
        }

        private void ShowSqlState(string sql) {
            if (sql.StartsWith("select", StringComparison.CurrentCultureIgnoreCase)) {
                ShowState("正在查询...");
            }
            else if (sql.StartsWith("insert", StringComparison.CurrentCultureIgnoreCase)) {
                ShowState("正在执行插入操作...");
            }
            else if (sql.StartsWith("delete", StringComparison.CurrentCultureIgnoreCase)) {
                ShowState("正在执行删除操作...");
            }
            else if (sql.StartsWith("update", StringComparison.CurrentCultureIgnoreCase)) {
                ShowState("正在执行更新操作...");
            }
        }

        private void ShowState(string state) {
            SetStateText?.Invoke(state);
        }

        private void ClearState() {
            SetStateText?.Invoke("");
        }
    }
}