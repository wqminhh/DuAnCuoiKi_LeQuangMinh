using System.Configuration;
using System.Data;
using MySqlConnector;

namespace PRDBMS.DAL
{
    public static class GLOBAL
    {
        // Dùng cho phân quyền trong app, KHÔNG dùng cho DB login
        public static string username;
        public static string password;
        public static string roles; // nếu bạn muốn lưu role
    }

    public class ConnectDatabase
    {
        // Chỉ lấy từ App.config (DB user cố định)
        private static readonly string strConn =
            ConfigurationManager.ConnectionStrings["PhoneStoreDb"]?.ConnectionString
            ?? "Server=localhost;Port=3306;Database=phonestore_management;Uid=phonestore_user;Pwd=123456;SslMode=None;AllowPublicKeyRetrieval=True;CharSet=utf8mb4;";

        private readonly MySqlConnection conNV;
        private readonly MySqlConnection conAD;

        public ConnectDatabase()
        {
            // Cả 2 connection đều dùng cùng 1 DB account (ổn định)
            conNV = new MySqlConnection(strConn);
            conAD = new MySqlConnection(strConn);
        }

        public MySqlConnection getConnection => conNV;
        public MySqlConnection getConnectionAdmin => conAD;

        public void openConnection()
        {
            if (conNV.State == ConnectionState.Closed)
                conNV.Open();
        }

        public void closeConnection()
        {
            if (conNV.State == ConnectionState.Open)
                conNV.Close();
        }

        public void openConnectionADmin()
        {
            if (conAD.State == ConnectionState.Closed)
                conAD.Open();
        }

        public void closeConnectionAdmin()
        {
            if (conAD.State == ConnectionState.Open)
                conAD.Close();
        }
    }
}
