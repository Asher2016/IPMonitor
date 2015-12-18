using CommonContract.Model;
using Devart.Data.PostgreSql;
using PlatForm.Util;
using System;
using System.Text;

namespace DataAccess.DAO
{
    public class UserDAO : IDisposable
    {
        public BrefUserInfo CheckUserNameAndPassword(BrefUserInfo brefUserInfo)
        {
            BrefUserInfo result = null;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandText = @"SELECT sid, user_name, password from ods.common_user where user_name=:userName and password=:password and mark_for_delete = false limit 1;";
                    command.Parameters.Add(new PgSqlParameter("userName", PgSqlType.VarChar)).Value = brefUserInfo.UserName;
                    command.Parameters.Add(new PgSqlParameter("password", PgSqlType.VarChar)).Value
                        = MD5Customer.MD5Decrypt(brefUserInfo.Password, MD5Customer.MD5Key);

                    PgSqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        result = new BrefUserInfo() {
                            SID = (long)reader[0],
                            UserName = (string)reader[1],
                            Password = (string)reader[2]
                        };
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            if (null != result)
            {
                result.Password = MD5Customer.MD5Encrypt(result.Password, MD5Customer.MD5Key);
            }

            return result;
        }

        public void Dispose()
        {
        }

        public void ChangePasswd(UserInfo userInfo)
        {
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandText = @"UPDATE ods.common_user SET password=:password WHERE sid = :sid;";
                    string f3 = MD5Customer.MD5Decrypt(userInfo.NewPassword, MD5Customer.MD5Key);
                    command.Parameters.Add(new PgSqlParameter("sid", PgSqlType.VarChar)).Value = userInfo.SID;
                    command.Parameters.Add(new PgSqlParameter("password", PgSqlType.VarChar)).Value
                        = MD5Customer.MD5Decrypt(userInfo.NewPassword, MD5Customer.MD5Key);

                    command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool CheckOldPasswd(UserInfo userInfo)
        {
            bool result = false;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandText = @"SELECT 1 FROM ods.common_user WHERE password = :password and sid = :sid limit 1;";
                    string f3 = MD5Customer.MD5Decrypt(userInfo.OldPassword, MD5Customer.MD5Key);
                    command.Parameters.Add(new PgSqlParameter("sid", PgSqlType.VarChar)).Value = userInfo.SID;
                    command.Parameters.Add(new PgSqlParameter("password", PgSqlType.VarChar)).Value
                        = MD5Customer.MD5Decrypt(userInfo.OldPassword, MD5Customer.MD5Key);

                    PgSqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader.HasRows;
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
    }
}
