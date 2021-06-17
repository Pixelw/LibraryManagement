using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Chapter12_winform.utils {
    public class SqlHelper {
        public readonly SqlConnection connection;

        public SqlHelper() {
            connection = new SqlConnection();
            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();
            stringBuilder.UserID = "Liyang";
            stringBuilder.Password = "aaaa123789";
            stringBuilder.DataSource = "lapi.pixelw.tech,51433";
            stringBuilder.InitialCatalog = "library";
            connection.ConnectionString = stringBuilder.ConnectionString;
        }

        public void Open() {
            connection.Open();
        }

        public void Close() {
            connection.Close();
        }

        public int ExecuteNonQuery(String sqlstr) {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sqlstr;
            return command.ExecuteNonQuery();
        }

        public int ExecuteNonQuery(String sqlStr, params SqlParameter[] parameters) {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sqlStr;
            command.Parameters.AddRange(parameters);
            return command.ExecuteNonQuery();
        }

        public int ExecuteNonQuery(SqlParameter[] parameters, String sp_name) {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sp_name;
            command.Parameters.AddRange(parameters);
            return command.ExecuteNonQuery();
        }

        public object ExecuteScalar(String sqlStr) {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sqlStr;
            return command.ExecuteScalar();
        }

        public object ExecuteScalar(String sqlStr, params SqlParameter[] parameters) {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sqlStr;
            command.Parameters.AddRange(parameters);
            return command.ExecuteScalar();
        }

        public object ExecuteScalar(SqlParameter[] sqlParameters, String sp_name) {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sp_name;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(sqlParameters);
            return command.ExecuteScalar();
        }

        public DataTable ExecuteTable(String sqlStr) {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sqlStr;
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = command;
            DataTable table = new DataTable();
            dataAdapter.Fill(table);
            return table;
        }

        public DataTable ExecuteTable(string sqlStr, params SqlParameter[] parameters) {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sqlStr;
            command.Parameters.AddRange(parameters);
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = command;
            DataTable table = new DataTable();
            dataAdapter.Fill(table);
            return table;
        }
    }
}