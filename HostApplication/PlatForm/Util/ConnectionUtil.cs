using Devart.Data.PostgreSql;
using System.Configuration;

namespace PlatForm.Util
{
    public class ConnectionUtil
    {
        private readonly string ConnString = ConfigurationManager.ConnectionStrings["Customer"].ToString();

        private static ConnectionUtil instance = new ConnectionUtil();

        private ConnectionUtil()
        {

        }

        public static ConnectionUtil Instance
        {
            get
            {
                return instance;
            }
        }

        public PgSqlConnection GetPgSqlConnection()
        {
            PgSqlConnection connection = new PgSqlConnection(ConnString);
            return connection;
        }
    }
}
