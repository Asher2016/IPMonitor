using CommonContract.Model;
using Devart.Data.PostgreSql;
using PlatForm.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataAccess.DAO
{
    public class IPMonitorDAO : IDisposable
    {
        public void LoadMonitorRecord(List<BrefIPInfo> brefIpInfoList)
        {
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        PgSqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "ods.load_ip_monitor_record";

                        command.Parameters.Add(new PgSqlParameter("_ip_monitor_info", PgSqlType.Array)).Value = ConvertToPgSqlArray(brefIpInfoList, connection);

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

        public PgSqlArray ConvertToPgSqlArray(List<BrefIPInfo> entityList, PgSqlConnection connection)
        {
            PgSqlArray array;
            if (entityList != null && entityList.Count > 0)
            {
                var list = entityList
                                    .Select(x => (object)ConvertToPgSqlRow(x, connection, true))
                                    .ToArray();
                array = new PgSqlArray(list, PgSqlType.Row, 1, list.Length);
            }
            else
            {
                array = new PgSqlArray(PgSqlType.Row, 0);
            }

            return array;
        }

        public PgSqlRow ConvertToPgSqlRow(BrefIPInfo baseEntity, PgSqlConnection connection, bool isToArray = false)
        {
            PgSqlRow row = new PgSqlRow("ods.tvp_bref_ip_info", connection);
            row[0] = baseEntity.IP;
            row[1] = baseEntity.LostTime;
            row[2] = baseEntity.RecoveryTime;

            return row;
        }

        public List<IPRegionPair> GetIpListForMonitor()
        {
            List<IPRegionPair> result = new List<IPRegionPair>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandText = @"select ip,name from ods.monitor_ip_info AS ip_info left join ods.region_info AS region_info ON ip_info.region_sid = region_info.sid where ip_info.mark_for_delete = false";

                    try
                    {
                        using (PgSqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new IPRegionPair()
                                {
                                    IP = reader[0].ToString(),
                                    Region = reader[1].ToString()
                                });
                            }
                        }
                    }
                    catch(Exception exception)
                    {
                        throw exception;
                    }
                }
                finally
                {
                    connection.Close();
                }
                
            }

            return result;
        }

        public void Dispose()
        {
            
        }

        public IPMonitorListModel GetIPRegionList(IPRegionListCriteria criteria)
        {
            IPMonitorListModel result = new IPMonitorListModel();
            result.IPRegionList = new List<BrefIPRegionInfo>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        PgSqlCommand command = connection.CreateCommand();
                        command.CommandText = "ods.fn_search_ip_region_info";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new PgSqlParameter("_search_column", PgSqlType.VarChar)).Value = criteria.SearchColumn;
                        command.Parameters.Add(new PgSqlParameter("_search_text", PgSqlType.VarChar)).Value = criteria.SearchText;
                        command.Parameters.Add(new PgSqlParameter("_page_size", PgSqlType.Interval)).Value = criteria.PageSize;
                        command.Parameters.Add(new PgSqlParameter("_page_index", PgSqlType.Interval)).Value = criteria.PageIndex;
                        command.Parameters.Add(new PgSqlParameter("_region", PgSqlType.VarChar)).Value = criteria.Region;

                        using (PgSqlDataReader reader = command.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {
                                result.IPRegionList.Add(new BrefIPRegionInfo()
                                {
                                    SID = long.Parse(reader[0].ToString()),
                                    IP = reader[1].ToString(),
                                    Region = reader[2].ToString(),
                                    RegionDisplayName = reader[3].ToString(),
                                    Model = reader[4].ToString(),
                                    Telephone = reader[5].ToString()
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
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        public void AddOrUpdateIPRegion(BrefIPRegionInfo brefIpRegionInfo)
        {
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        PgSqlCommand command = connection.CreateCommand();

                        command.CommandText = "ods.fn_add_or_update_ip_region_info";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new PgSqlParameter("_sid", PgSqlType.BigInt)).Value = brefIpRegionInfo.SID;
                        command.Parameters.Add(new PgSqlParameter("_ip", PgSqlType.VarChar)).Value = brefIpRegionInfo.IP;
                        command.Parameters.Add(new PgSqlParameter("_region", PgSqlType.VarChar)).Value = brefIpRegionInfo.Region;
                        command.Parameters.Add(new PgSqlParameter("_model", PgSqlType.VarChar)).Value = brefIpRegionInfo.Model;
                        command.Parameters.Add(new PgSqlParameter("_telephone", PgSqlType.VarChar)).Value = brefIpRegionInfo.Telephone;

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

                RedisHelper.DeleteCache(brefIpRegionInfo.IP);
                List<IPRegionPair> result = new List<IPRegionPair>();

                using (IPMonitorDAO dao = new IPMonitorDAO())
                {
                    result = dao.GetIpListForMonitor();
                }

                RedisHelper.LoadIPList(result);
            }
        }

        public void DeleteIPRegion(long sid)
        {
            string ip = string.Empty;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        PgSqlCommand command = connection.CreateCommand();

                        command.CommandType = CommandType.Text;
                        command.CommandText = "UPDATE ods.monitor_ip_info SET mark_for_delete = true where sid = :_sid";
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

                string deleteIP = EditIPRegion(sid).IP;
                RedisHelper.DeleteCache(deleteIP);
                RedisHelper.LoadIPList(RedisHelper.GetIPList().Where(x => x.IP != deleteIP).ToList());
            }
        }

        public BrefIPRegionInfo EditIPRegion(long sid)
        {
            BrefIPRegionInfo result = new BrefIPRegionInfo();
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();

                    PgSqlCommand command = connection.CreateCommand();

                    command.CommandText = @"SELECT ip_info.sid, ip_info.ip,region_info.name, ip_info.model, ip_info.telephone FROM ods.monitor_ip_info AS ip_info inner join ods.region_info AS region_info ON region_info.sid = ip_info.region_sid where ip_info.sid = :_sid;";
                    command.Parameters.Add(new PgSqlParameter("_sid", PgSqlType.BigInt)).Value = sid;

                    using (PgSqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.SID = (long)reader[0];
                            result.IP = reader[1].ToString();
                            result.Region = reader[2].ToString();
                            result.Model = reader[3].ToString();
                            result.Telephone = reader[4].ToString();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        public AlertInfoListModel GetAlertInfo(AlertInfoCriteria criteria)
        {
            AlertInfoListModel result = new AlertInfoListModel();
            result.BrefAlertInfoList = new List<BrefAlertInfo>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        PgSqlCommand command = connection.CreateCommand();
                        command.CommandText = "ods.fn_search_alert_info";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new PgSqlParameter("_from_date", PgSqlType.TimeStamp)).Value = criteria.FromDate;
                        command.Parameters.Add(new PgSqlParameter("_to_date", PgSqlType.TimeStamp)).Value = criteria.ToDate;
                        command.Parameters.Add(new PgSqlParameter("_search_column", PgSqlType.VarChar)).Value = criteria.SearchColumn;
                        command.Parameters.Add(new PgSqlParameter("_search_text", PgSqlType.VarChar)).Value = criteria.SearchText;
                        command.Parameters.Add(new PgSqlParameter("_page_size", PgSqlType.Interval)).Value = criteria.PageSize;
                        command.Parameters.Add(new PgSqlParameter("_page_index", PgSqlType.Interval)).Value = criteria.PageIndex;
                        command.Parameters.Add(new PgSqlParameter("_region", PgSqlType.VarChar)).Value = criteria.Region;
                        command.Parameters.Add(new PgSqlParameter("_is_send", PgSqlType.Boolean)).Value = criteria.IsSend;

                        using (PgSqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.BrefAlertInfoList.Add(new BrefAlertInfo()
                                {
                                    IP = reader[0].ToString(),
                                    Region = reader[1].ToString(),
                                    Model = reader[2].ToString(),
                                    FirstLostTime = DateTime.Parse(reader[3].ToString()),
                                    SecondLostTime = DateTime.Parse(reader[4].ToString()),
                                    RecoveryTime = DateTime.Parse(reader[5].ToString()),
                                    IsSend = bool.Parse(reader[6].ToString())
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
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        public MonitorRecordListModel GetMonitorRecord(MonitorRecordCriteria criteria)
        {
            MonitorRecordListModel result = new MonitorRecordListModel();
            result.BrefIPInfoList = new List<BrefIPInfo>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                try
                {
                    connection.Open();
                    using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        PgSqlCommand command = connection.CreateCommand();
                        command.CommandText = "ods.fn_search_monitor_info";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new PgSqlParameter("_from_date", PgSqlType.TimeStamp)).Value = criteria.FromDate;
                        command.Parameters.Add(new PgSqlParameter("_to_date", PgSqlType.TimeStamp)).Value = criteria.ToDate;
                        command.Parameters.Add(new PgSqlParameter("_search_column", PgSqlType.VarChar)).Value = criteria.SearchColumn;
                        command.Parameters.Add(new PgSqlParameter("_search_text", PgSqlType.VarChar)).Value = criteria.SearchText;
                        command.Parameters.Add(new PgSqlParameter("_page_size", PgSqlType.Interval)).Value = criteria.PageSize;
                        command.Parameters.Add(new PgSqlParameter("_page_index", PgSqlType.Interval)).Value = criteria.PageIndex;
                        command.Parameters.Add(new PgSqlParameter("_region", PgSqlType.VarChar)).Value = criteria.Region;

                        using (PgSqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.BrefIPInfoList.Add(new BrefIPInfo()
                                {
                                    IP = reader[0].ToString(),
                                    Region = reader[1].ToString(),
                                    Model = reader[2].ToString(),
                                    LostTime = DateTime.Parse(reader[3].ToString()),
                                    RecoveryTime = DateTime.Parse(reader[4].ToString())
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
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        public List<IPRegionPair> GetAllIpListStatus()
        {
            List<IPRegionPair> result = new List<IPRegionPair>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                try
                {
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandText = @"select ip_info.ip, region_info.name from ods.monitor_ip_info AS ip_info INNER JOIN ods.region_info AS region_info ON ip_info.region_sid = region_info.sid and ip_info.mark_for_delete = false;";
                    command.CommandType = CommandType.Text;

                    using (PgSqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new IPRegionPair()
                            {
                                IP = reader[0].ToString(),
                                Region = reader[1].ToString()
                            });
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        public bool IsExist(long sid, string ip)
        {
            bool result = false;

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                try
                {
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandText = @"select 1 from ods.monitor_ip_info where ip = :_ip and sid != :_sid and mark_for_delete = FALSE  limit 1;";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new PgSqlParameter("_ip", PgSqlType.VarChar)).Value = ip;
                    command.Parameters.Add(new PgSqlParameter("_sid", PgSqlType.BigInt)).Value = sid;

                    using (PgSqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

            return result;
        }
    }
}
