using System.Data;
using MySqlConnector;

namespace PRDBMS.DAL
{
    /// <summary>
    /// Thin MySQL helper for ADO.NET usage. Intended to be called from BLL, not directly from UI.
    /// </summary>
    public sealed class DbHelper
    {
        private readonly ConnectDatabase _db;

        public DbHelper(ConnectDatabase db)
        {
            _db = db;
        }

        public DataTable GetDataTable(string sqlOrProc, CommandType commandType, params MySqlParameter[] parameters)
        {
            var dt = new DataTable();
            _db.openConnection();

            using (var cmd = new MySqlCommand(sqlOrProc, _db.getConnection))
            {
                cmd.CommandType = commandType;
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                using (var da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            _db.closeConnection();
            return dt;
        }

        public int ExecuteNonQuery(string sqlOrProc, CommandType commandType, params MySqlParameter[] parameters)
        {
            _db.openConnection();
            using (var cmd = new MySqlCommand(sqlOrProc, _db.getConnection))
            {
                cmd.CommandType = commandType;
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                var affected = cmd.ExecuteNonQuery();
                _db.closeConnection();
                return affected;
            }
        }

        public object ExecuteScalar(string sqlOrProc, CommandType commandType, params MySqlParameter[] parameters)
        {
            _db.openConnection();
            using (var cmd = new MySqlCommand(sqlOrProc, _db.getConnection))
            {
                cmd.CommandType = commandType;
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                var value = cmd.ExecuteScalar();
                _db.closeConnection();
                return value;
            }
        }
    }
}
