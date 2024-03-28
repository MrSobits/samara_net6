namespace Bars.Gkh.Gis.KP_legacy
{
    using System.Data;

    public class MyDbConnection : IDbConnection
    {
        IDbConnection _connection;

        public IDbConnection RealConnection
        {
            get { return this._connection; }
        }

        public MyDbConnection(IDbConnection connection)
        {
            this._connection = connection;
        }

        public void RealClose()
        {
            if (Points.FullLogging)
            {
                this.LoggingTemporaryTables(this._connection);
                
            }
            DBManager.ClearConnection(this);
            this._connection.Close();
        }

        // Summary: 
        //     write to log opened temporary files
        //
        // Returns:
        //     A string containing names temporary files
        public void LoggingTemporaryTables(IDbConnection conDb)
        {
            return;
            string sql = "select table_schema||'.'||table_name||'@'||table_catalog as name from information_schema.tables where table_type ='LOCAL TEMPORARY'";
            var dt = DBManager.ExecSQLToTable(conDb, sql);
        }


        // Summary:
        //     Gets or sets the string used to open a database.
        //
        // Returns:
        //     A string containing connection settings.
        public string ConnectionString
        {
            get
            {
                return this._connection.ConnectionString;
            }
            set
            {
                this._connection.ConnectionString = value;
            }
        }

        //
        // Summary:
        //     Gets the time to wait while trying to establish a connection before terminating
        //     the attempt and generating an error.
        //
        // Returns:
        //     The time (in seconds) to wait for a connection to open. The default value
        //     is 15 seconds.
        public int ConnectionTimeout { get { return this._connection.ConnectionTimeout; } }

        //
        // Summary:
        //     Gets the name of the current database or the database to be used after a
        //     connection is opened.
        //
        // Returns:
        //     The name of the current database or the name of the database to be used once
        //     a connection is open. The default value is an empty string.
        public string Database { get { return this._connection.Database; } }

        //
        // Summary:
        //     Gets the current state of the connection.
        //
        // Returns:
        //     One of the System.Data.ConnectionState values.
        public ConnectionState State { get { return this._connection.State; } }

        // Summary:
        //     Begins a database transaction.
        //
        // Returns:
        //     An object representing the new transaction.
        public IDbTransaction BeginTransaction()
        {
            return this._connection.BeginTransaction();
        }

        //
        // Summary:
        //     Begins a database transaction with the specified System.Data.IsolationLevel
        //     value.
        //
        // Parameters:
        //   il:
        //     One of the System.Data.IsolationLevel values.
        //
        // Returns:
        //     An object representing the new transaction.
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return this._connection.BeginTransaction(il);
        }

        //
        // Summary:
        //     Changes the current database for an open Connection object.
        //
        // Parameters:
        //   databaseName:
        //     The name of the database to use in place of the current database.
        public void ChangeDatabase(string databaseName)
        {
            this._connection.ChangeDatabase(databaseName);
        }

        //
        // Summary:
        //     Не закрывает соединение
        public void Close()
        {

        }

        //
        // Summary:
        //     Creates and returns a Command object associated with the connection.
        //
        // Returns:
        //     A Command object associated with the connection.
        public IDbCommand CreateCommand()
        {
            return this._connection.CreateCommand();
        }

        //
        // Summary:
        //     Opens a database connection with the settings specified by the ConnectionString
        //     property of the provider-specific Connection object.
        public void Open()
        {
            this._connection.Open();
        }

        public void Dispose()
        {
            this._connection.Dispose();
        }
    }
}