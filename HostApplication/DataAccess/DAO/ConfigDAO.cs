using DataAccess.Model;
using Devart.Data.PostgreSql;
using PlatForm.Util;
using System;

namespace DataAccess.DAO
{
    public class ConfigDAO : IDisposable
    {
        public CopyLogConfig GetConfig()
        {
            CopyLogConfig customerDataBase = null;
            string configData = string.Empty;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandText = @"SELECT config_data from config.start_up_config WHERE config_type = 'CopyLogConfig' AND mark_for_delete = FALSE;";
                    configData = (string)command.ExecuteScalar();
                }
                finally
                {
                    connection.Close();
                }
            }

            if (!string.IsNullOrEmpty(configData))
            {
                customerDataBase = XmlConvertor.XmlToObject<CopyLogConfig>(configData);
            }

            return customerDataBase;
        }

        public void Dispose()
        {
        }
    }
}
