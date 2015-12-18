using CommonContract.Model;
using Devart.Data.PostgreSql;
using PlatForm.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DataAccess.DAO
{
    public class UserIPDAO : IDisposable
    {
        public UserIPList Search(UserIPCriteria criteria)
        {
            UserIPList result = new UserIPList();
            List<UserIPInfo> userIpList = new List<UserIPInfo>();
            result.UserIPPageList = userIpList;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                try
                {
                    using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        using (PgSqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "ods.ip_user_map_search";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new PgSqlParameter("_search_column", PgSqlType.VarChar)).Value = criteria.SearchColumn;
                            command.Parameters.Add(new PgSqlParameter("_search_text", PgSqlType.VarChar)).Value = criteria.SearchText;
                            command.Parameters.Add(new PgSqlParameter("_page_size", PgSqlType.Interval)).Value = criteria.PageSize;
                            command.Parameters.Add(new PgSqlParameter("_page_index", PgSqlType.Interval)).Value = criteria.PageIndex;

                            try
                            {
                                using (PgSqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.UserIPPageList.Add(new UserIPInfo()
                                        {
                                            SID = (long)reader[0],
                                            IPAddress = (string)reader[1],
                                            UserName = (string)reader[2]
                                        });
                                    }

                                    reader.NextResult();

                                    while (reader.Read())
                                    {
                                        result.Count = (long)reader[0];
                                    }
                                }
                            }
                            catch(Exception exception)
                            {
                                transaction.Rollback();
                                throw exception;
                            }
                        }

                        transaction.Commit();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        public void Delete(long sid)
        {
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();

                using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (PgSqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE ods.ip_user_map SET mark_for_delete = true WHERE sid = :_sid";
                            command.Parameters.Add(new PgSqlParameter("_sid", PgSqlType.BigInt)).Value = sid;

                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (Exception exception)
                            {
                                transaction.Rollback();
                                throw exception;
                            }

                            transaction.Commit();
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public UserIPInfo Edit(long sid)
        {
            UserIPInfo result = new UserIPInfo();
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();

                using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (PgSqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "SELECT sid, ip_address, user_name FROM ods.ip_user_map where sid = :_sid;";
                            command.Parameters.Add(new PgSqlParameter("_sid", PgSqlType.BigInt)).Value = sid;

                            try
                            {
                                using (PgSqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.SID = (long)reader[0];
                                        result.IPAddress = (string)reader[1];
                                        result.UserName = (string)reader[2];
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                transaction.Rollback();
                                throw exception;
                            }
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        public void AddOrUpdate(UserIPInfo userIPInfo)
        {
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                try
                {
                    using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        using (PgSqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "ods.add_or_update_user_ip_map";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add("_sid", PgSqlType.BigInt).Value = userIPInfo.SID;
                            command.Parameters.Add("_ip_address", PgSqlType.VarChar).Value = userIPInfo.IPAddress;
                            command.Parameters.Add("_user_name", PgSqlType.VarChar).Value = userIPInfo.UserName;

                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch(Exception exception)
                            {
                                transaction.Rollback();
                                throw exception;
                            }
                        }
                        transaction.Commit();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool IsExist(long sid, string ipAddress)
        {
            bool result = false;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                using (PgSqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT 1 FROM ods.ip_user_map where ip_address = :_ip_address and sid != :_sid and mark_for_delete = false limit 1;";
                    command.Parameters.Add(new PgSqlParameter("_ip_address", PgSqlType.VarChar)).Value = ipAddress;
                    command.Parameters.Add(new PgSqlParameter("_sid", PgSqlType.BigInt)).Value = sid;

                    try
                    {
                        using (PgSqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                result = reader.HasRows;
                            }
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        public void Dispose()
        {
        }
    }
}
