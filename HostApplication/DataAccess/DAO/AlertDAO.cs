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
    public class AlertDAO : IDisposable
    {
        public void LoadAlertRecord(List<BrefAlertInfo> brefIpInfoList)
        {
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ods.load_alert_info";

                    command.Parameters.Add(new PgSqlParameter("_alert_info", PgSqlType.Array)).Value = ConvertToPgSqlArray(brefIpInfoList, connection);

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
        }

        public PgSqlArray ConvertToPgSqlArray(List<BrefAlertInfo> entityList, PgSqlConnection connection)
        {
            PgSqlArray array;
            if (entityList != null && entityList.Count > 0)
            {
                var list = entityList.Select(x => (object)ConvertToPgSqlRow(x, connection, true)).ToArray();
                array = new PgSqlArray(list, PgSqlType.Row, 1, list.Length);
            }
            else
            {
                array = new PgSqlArray(PgSqlType.Row, 0);
            }

            return array;
        }

        public PgSqlRow ConvertToPgSqlRow(BrefAlertInfo baseEntity, PgSqlConnection connection, bool isToArray = false)
        {
            PgSqlRow row = new PgSqlRow("ods.tvp_alert_info", connection);
            row[0] = baseEntity.SID;
            row[1] = baseEntity.IP;
            row[2] = baseEntity.FirstLostTime;
            row[3] = baseEntity.SecondLostTime;
            row[4] = false;
            row[5] = string.IsNullOrEmpty(baseEntity.RecoveryTime.ToString()) ? DateTime.MinValue : baseEntity.RecoveryTime;
            return row;
        }

        public List<BrefSendMessageInfo> GetAlertInfo()
        {
            List<BrefSendMessageInfo> result = new List<BrefSendMessageInfo>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                PgSqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT alert.sid, alert.ip, monitor.telephone, alert.first_lost_time, alert.second_lost_time, alert.recovery_time FROM ods.alert_info AS alert INNER JOIN ods.monitor_ip_info AS monitor ON alert.ip = monitor.ip WHERE monitor.mark_for_delete = FALSE AND alert.send = FALSE;";

                try
                {
                    using (PgSqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new BrefSendMessageInfo()
                            {
                                SID = (long)reader[0],
                                IP = reader[1].ToString(),
                                Telephone = reader[2].ToString(),
                                FirstLostTime = DateTime.Parse(reader[3].ToString()),
                                SecondLostTime = DateTime.Parse(reader[4].ToString()),
                                RecoveryTime = reader[5] == null ? DateTime.MinValue : DateTime.Parse(reader[5].ToString())
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

        public void FinishSendMessage(List<long> sidList)
        {
            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                using (PgSqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    PgSqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE ods.alert_info SET send = true where sid in (SELECT public.fn_split_bigints(:sid_list))";

                    command.Parameters.Add(new PgSqlParameter("sid_list", PgSqlType.VarChar)).Value = string.Join(",", sidList.ToArray());

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
        }

        public void Dispose()
        {

        }

        public List<BrefAlertInfo> GetPreNotRecoveryAlert()
        {
            List<BrefAlertInfo> result = new List<BrefAlertInfo>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                PgSqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "with temp_table AS( SELECT alert.sid, alert.ip, alert.first_lost_time, alert.second_lost_time, row_number() over(partition by alert.ip order by alert.first_lost_time desc) as index FROM ods.alert_info AS alert INNER JOIN ods.monitor_ip_info AS monitor ON alert.ip = monitor.ip WHERE monitor.mark_for_delete = FALSE AND alert.recovery_time = '0001-01-01 00:00:00' and alert.create_date > :from and alert.create_date < :to order by ip, first_lost_time desc ) SELECT * FROM temp_table where index = 1;";
                //command.CommandText = "with temp_table AS( SELECT alert.sid, alert.ip, alert.first_lost_time, alert.second_lost_time, row_number() over(partition by alert.ip order by alert.first_lost_time desc) as index FROM ods.alert_info AS alert WHERE alert.recovery_time = '0001-01-01 00:00:00' order by ip, first_lost_time desc ) SELECT * FROM temp_table where index = 1;";
                command.Parameters.Add(new PgSqlParameter("from", PgSqlType.TimeStamp)).Value = DateTime.Now.Date;
                command.Parameters.Add(new PgSqlParameter("to", PgSqlType.TimeStamp)).Value = DateTime.Now.AddDays(1).Date;

                try
                {
                    using (PgSqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new BrefAlertInfo()
                            {
                                SID = long.Parse(reader[0].ToString()),
                                IP = reader[1].ToString(),
                                FirstLostTime = DateTime.Parse(reader[2].ToString()),
                                SecondLostTime = DateTime.Parse(reader[3].ToString()),
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

        public List<string> GetTodayAlert()
        {
            List<string> result = new List<string>();

            using (PgSqlConnection connection = ConnectionUtil.Instance.GetPgSqlConnection())
            {
                connection.Open();
                PgSqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from ods.alert_info where create_date > :from and create_date < :to";
                command.Parameters.Add(new PgSqlParameter("from", PgSqlType.TimeStamp)).Value = DateTime.Now.Date;
                command.Parameters.Add(new PgSqlParameter("to", PgSqlType.TimeStamp)).Value = DateTime.Now.AddDays(1).Date;

                try
                {
                    using (PgSqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader[0].ToString());
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
    }
}
