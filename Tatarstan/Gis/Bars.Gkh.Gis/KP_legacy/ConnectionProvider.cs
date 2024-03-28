using System.Data.Common;
using System.Text.RegularExpressions;

using Bars.B4.Config;

namespace Bars.Gkh.Gis.KP_legacy
{
    using Npgsql;

    public class ConnectionProvider
    {
        public string ConnectionString { get; protected set; }

        public ConnectionProvider(IConfigProvider configProvider)
        {
            this.Init(configProvider.GetConfig().ConnString);
        }

        public void Init(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            return (DbConnection)new NpgsqlConnection(this.ConnectionString);
        }

        public void ChangeSchema(string schemaName)
        {
            if (this.ConnectionString.Contains("SearchPath"))
                this.ConnectionString = new Regex(string.Format("{0}=(\\w)+;|{0}=(\\w)+$", (object)"SearchPath"), RegexOptions.IgnoreCase).Replace(this.ConnectionString, string.Format("{0}={1};", (object)"SearchPath", (object)schemaName));
            else
                this.ConnectionString = string.Format("{0}{1}{2}={3}", (object)this.ConnectionString, (object)(this.ConnectionString.EndsWith(";") ? "" : ";"), (object)"SearchPath", (object)schemaName);
        }
    }
}