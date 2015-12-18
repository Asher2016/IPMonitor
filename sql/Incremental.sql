INSERT INTO config.start_up_config
(
config_type,
config_data,
create_date,
modified_date,
mark_for_delete
)
VALUES
(
'CopyLogConfig',
'<?xml version=''1.0'' encoding=''utf-16''?>
<CopyLogConfig xmlns:xsi=''http://www.w3.org/2001/XMLSchema-instance'' xmlns:xsd=''http://www.w3.org/2001/XMLSchema''>
  <JobName>CopyJobInfo</JobName>
  <JobGroup>LogInfo</JobGroup>

  <Corn>0 0 0 ? * * </Corn>
  <DateFormatInfoDic>
    <SerializableDictionary>
      <key>
        <string>local7.log</string>
      </key>
      <value>
        <DateFormatInfo>
          <LocalDateLength>15</LocalDateLength>
          <LocalDateFormat>
            <string>MMM dd HH:mm:ss</string>
          </LocalDateFormat>
          <RemoteDateLength>20</RemoteDateLength>
          <RemoteDateFormat>
            <string>MMM  d HH:mm:ss yyyy</string>
            <string>MMM dd HH:mm:ss yyyy</string>
            <string>MMM  d yyyy HH:mm:ss</string>
            <string>MMM dd yyyy HH:mm:ss</string>
          </RemoteDateFormat>
          <TrimChar>
            <char>32</char>
          </TrimChar>
        </DateFormatInfo>
      </value>
    </SerializableDictionary>
    <SerializableDictionary>
      <key>
        <string>local5.log</string>
      </key>
      <value>
        <DateFormatInfo>
          <LocalDateLength>15</LocalDateLength>
          <LocalDateFormat>
            <string>MMM dd HH:mm:ss</string>
          </LocalDateFormat>
          <RemoteDateLength>17</RemoteDateLength>
          <RemoteDateFormat>
            <string>MMM  d HH:mm:ss</string>
            <string>MMM dd HH:mm:ss</string>
          </RemoteDateFormat>
          <TrimChar>
            <char>42</char>
            <char>58</char>
            <char>32</char>
          </TrimChar>
        </DateFormatInfo>
      </value>
    </SerializableDictionary>
  </DateFormatInfoDic>
  <CustomDB>
    <ServerName>192.168.184.27</ServerName>
    <Database>net_work_monitor</Database>
    <Username>postgres</Username>
    <Password>xrs123!@#</Password>
  </CustomDB>
  <StartupPath>ss</StartupPath>
  <LogPath>C://Users//Asher//Desktop//2-11272015//logFile</LogPath>
  <TableStruct>ods.log_information(ip_address, log_level, local_time, remote_time, message)</TableStruct>
</CopyLogConfig>',
now(),
now(),
false
);