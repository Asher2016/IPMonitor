using CommonContract.Model;
using Devart.Data.PostgreSql;
using PlatForm.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.DAO
{
    public class LogInfoDAO : IDisposable
    {
        public LogListContract SearchList(LogCriteria criteria)
        {
            LogListContract result = new LogListContract();
            List<LogContract> logInfoList = new List<LogContract>();
            result.LogInfoList = logInfoList;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();

                using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        PgSqlCommand command = connection.CreateCommand();

                        command.CommandText = "ods.fn_log_info_search";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new PgSqlParameter("_from_date", PgSqlType.TimeStamp)).Value = criteria.FromDate;
                        command.Parameters.Add(new PgSqlParameter("_to_date", PgSqlType.TimeStamp)).Value = criteria.ToDate;
                        command.Parameters.Add(new PgSqlParameter("_search_column", PgSqlType.VarChar)).Value = criteria.SearchColumn;
                        command.Parameters.Add(new PgSqlParameter("_search_text", PgSqlType.VarChar)).Value = criteria.SearchText;
                        command.Parameters.Add(new PgSqlParameter("_page_size", PgSqlType.Interval)).Value = criteria.PageSize;
                        command.Parameters.Add(new PgSqlParameter("_page_index", PgSqlType.Interval)).Value = criteria.PageIndex;
                        command.Parameters.Add(new PgSqlParameter("_log_type", PgSqlType.SmallInt)).Value = criteria.LogType;

                        using (PgSqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.LogInfoList.Add(new LogContract()
                                {
                                    IpAddress = reader[0].ToString(),
                                    LogLevel = reader[1].ToString(),
                                    LocalTime = reader[2].ToString(),
                                    RemoteTime = reader[3].ToString(),
                                    LogMessage = reader[4].ToString(),
                                });
                            }

                            reader.NextResult();

                            while (reader.Read())
                            {
                                result.Count = (long)reader[0];
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        LogHelper.Instance.Error(LogHelper.CommonService, exception.InnerException.Message);
                        throw exception;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        public void DeleteOldData()
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
                            command.CommandText = "DELETE FROM ods.log_information WHERE local_time <= (now() AT TIME ZONE 'UTC-8') - '6 month'::interval";

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

        public void Dispose()
        {
        }
    }
}
