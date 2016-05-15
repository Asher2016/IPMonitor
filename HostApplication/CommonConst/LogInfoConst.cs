using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonConst
{
    public static class LogInfoConst
    {
        public const string PSQL = "PSQL\\psql";

        public const string SMSConsole = "SMS\\SMSConsole";

        public const string UTF8 = "UTF8";

        public const string PGUSER = "PGUSER";

        public const string PGPASSWORD = "PGPASSWORD";

        public const string PGCLIENTENCODING = "PGCLIENTENCODING";

        public const string ProcessCopyFromArguments = "-h {0} -d {1} -c \"\\copy {2} from '{3}' with csv HEADER ENCODING 'utf-8' \"";

        public const string ProcessCopyToArguments = "-h {0} -d {1} -c \"\\copy {2} to '{3}' with csv HEADER ENCODING 'utf-8' \"";
    }
}
