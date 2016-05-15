using DataAccess.Model;
using System;
using System.Globalization;
using System.IO;

namespace CommonService.LogInformation
{
    public class TranslateFileFormat
    {
        public string ConvertFormatToCSV(string filePath, SerializableDictionary<string, DateFormatInfo> parseInfo)
        {
            string[] files = Directory.GetFiles(filePath, "*.log", SearchOption.TopDirectoryOnly);
            string targetFilePath = string.Empty;

            if (files.Length > 0)
            {
                Directory.CreateDirectory(Path.Combine(filePath, "History"));
                targetFilePath = Path.Combine(filePath, "History", DateTime.Now.ToString("ddMMyyyyHHmmss") + ".csv");
                string targetErrorFormatPath = Path.Combine(filePath, "History", DateTime.Now.ToString("ddMMyyyyHHmmss") + "Error" + ".csv");

                FileStream targetFile = new FileStream(targetFilePath, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(targetFile);

                FileStream targetErrorFile = new FileStream(targetErrorFormatPath, FileMode.Create);
                StreamWriter streamErrorWriter = new StreamWriter(targetErrorFile);

                FileStream sourceFile = null;
                StreamReader streamReader = null;

                try
                {
                    foreach (string path in files)
                    {
                        sourceFile = new FileStream(path, FileMode.Open);
                        streamReader = new StreamReader(sourceFile);

                        string[] pathArray = path.Split('\\');
                        string currentFileName = pathArray[pathArray.Length - 1];

                        if (parseInfo.ContainsKey(currentFileName))
                        {
                            DateFormatInfo currentDateFormat = parseInfo[currentFileName];

                            while (!streamReader.EndOfStream)
                            {
                                DateTime localTime = DateTime.MaxValue;
                                DateTime remoteTime = DateTime.MaxValue;

                                DateTime tempLocalTime;
                                DateTime tempRemoteTime;

                                string currentLine = streamReader.ReadLine();
                                string localTimeS = currentLine.Substring(0, currentDateFormat.LocalDateLength)
                                    .Trim(currentDateFormat.TrimChar);

                                foreach (string format in currentDateFormat.LocalDateFormat)
                                {
                                    if (DateTime.TryParseExact(localTimeS, format, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out tempLocalTime))
                                    {
                                        localTime = tempLocalTime;
                                        break;
                                    }
                                }

                                int ipEndIndex = currentLine.IndexOf(' ', currentDateFormat.LocalDateLength + 1);
                                string ipAddress = currentLine.Substring(currentDateFormat.LocalDateLength + 1,
                                    ipEndIndex - currentDateFormat.LocalDateLength - 1);

                                string remoteTimeS = currentLine.Substring(ipEndIndex + 1, currentDateFormat.RemoteDateLength)
                                    .Trim(currentDateFormat.TrimChar);

                                foreach (string format in currentDateFormat.RemoteDateFormat)
                                {
                                    if (DateTime.TryParseExact(remoteTimeS, format, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out tempRemoteTime))
                                    {
                                        remoteTime = tempRemoteTime;
                                        break;
                                    }
                                }

                                DateTime localTimeWithRemoteYear = new DateTime(remoteTime.Year, localTime.Month, localTime.Day, localTime.Hour, localTime.Minute, localTime.Second);

                                string message = currentLine.Substring(currentDateFormat.RemoteDateLength + ipEndIndex+ 1,
                                    currentLine.Length - currentDateFormat.RemoteDateLength - ipEndIndex - 1).Replace(',', ' ');

                                string logLevel = currentFileName.Substring(5, 1);

                                string newLine = string.Format("{0},{1},{2},{3},{4}", ipAddress, logLevel, localTime.ToString("yyyy-MM-dd HH:mm:ss"), remoteTime.ToString("yyyy-MM-dd HH:mm:ss"), message.Trim());

                                if (remoteTime == DateTime.MaxValue || localTime == DateTime.MaxValue)
                                {
                                    streamErrorWriter.WriteLine(currentFileName + " " + currentLine);
                                }
                                else
                                {
                                    streamWriter.WriteLine(newLine);
                                }
                            }
                        }

                        streamReader.Close();
                        sourceFile.Close();
                    }
                }
                catch
                {

                }
                finally
                {
                    streamReader.Close();
                    sourceFile.Close();

                    streamWriter.Close();
                    targetFile.Close();

                    streamErrorWriter.Close();
                    targetErrorFile.Close();
                }
            }
            else
            {
                //director is empty.
            }

            return targetFilePath;
        }

    }
}
