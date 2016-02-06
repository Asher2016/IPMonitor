using CommonContract.Model;
using Devart.Data.PostgreSql;
using PlatForm.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class LogInfoGuide : IDisposable
    {
        public LogInfoGuideList SearchLogInfoGuideList(LogInfoGuideCriteria criteria)
        {
            LogInfoGuideList result = new LogInfoGuideList();
            List<LogInfoGuideModel> logInfoList = new List<LogInfoGuideModel>();
            result.GuideList = logInfoList;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();

                using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        PgSqlCommand command = connection.CreateCommand();

                        command.CommandText = "ods.fn_log_info_guide_search";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new PgSqlParameter("_search_column", PgSqlType.VarChar)).Value = criteria.SearchColumn;
                        command.Parameters.Add(new PgSqlParameter("_search_text", PgSqlType.VarChar)).Value = criteria.SearchText;
                        command.Parameters.Add(new PgSqlParameter("_page_size", PgSqlType.Interval)).Value = criteria.PageSize;
                        command.Parameters.Add(new PgSqlParameter("_page_index", PgSqlType.Interval)).Value = criteria.PageIndex;

                        using (PgSqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.GuideList.Add(new LogInfoGuideModel()
                                {
                                    KeyWords = reader[0].ToString(),
                                    LogLevel = reader[1].ToString(),
                                    LogInformation = reader[2].ToString(),
                                    Solution = reader[3].ToString()
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

        public List<LogLevelGuide> GetLogLevelGuideList()
        {
            List<LogLevelGuide> result = new List<LogLevelGuide>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                PgSqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT log_level, number, description FROM ods.log_level_guide;";

                try
                {
                    using (PgSqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new LogLevelGuide()
                            {
                                LogLevel = (string)reader[0],
                                Number = (short)reader[1],
                                Description = (string)reader[2]

                            });
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            return result;
        }

        public void Dispose()
        {
        }
    }
}
