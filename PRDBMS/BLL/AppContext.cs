using System.Configuration;
using PRDBMS.DAL;

namespace PRDBMS.BLL
{
    /// <summary>
    /// Central composition root for the 3-layer structure.
    /// UI should depend on BLL, and BLL depends on DAL.
    /// </summary>
    public static class AppContext
    {
        // Single DB instance for the application.
        public static readonly ConnectDatabase Db = new ConnectDatabase();

        public static readonly DbHelper Data = new DbHelper(Db);

        // NOTE: If you want per-user credentials, extend ConnectDatabase to build
        // the connection string dynamically. For now we use App.config connection string.
    }
}
