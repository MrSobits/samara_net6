// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.DataBase.DataBaseHead
// Assembly: KP50.IFMX.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4C75160A-0424-4438-B01F-5EF7B8BCCD1C
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.IFMX.Common.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class DataBaseHead : IDisposable//, IDataBaseCommon
    {
        protected static string sKernelAliasRest = "_kernel.";
        protected static string sDataAliasRest = "_data.";
        protected static string sUploadAliasRest = "_upload.";
        protected static string tbluser = "";
        protected static string sDecimalType = "numeric";
        protected static string sCharType = "character";
        protected static string sUniqueWord = "distinct";
        protected static string sNvlWord = "coalesce";
        protected static string sConvToNum = "::numeric";
        protected static string sConvToInt = "::int";
        protected static string sConvToChar = "::character";
        protected static string sConvToVarChar = "::varchar";
        protected static string sConvToDate = "::date";
        protected static string sDefaultSchema = "public.";
        protected static string s0hour = "interval '0 hour'";
        protected static string sUpdStat = "analyze";
        protected static string sCrtTempTable = "temp";
        protected static string sUnlogTempTable = "";
        protected static string sCurDate = "current_date";
        protected static string sCurDateTime = "now()";
        protected static string DateNullString = "Null::date";
        protected static string sFirstWord = "limit";
        protected static string sDateTimeType = "timestamp";
        protected static string sLockMode = "";
        protected readonly string pgDefaultSchema = "public";
        public const string ConfPref = "W";
        public bool LongType;
        private List<IDataReader> _readers;
        private Dictionary<string, IDbConnection> _connections;

        protected static string tableDelimiter
        {
            get
            {
                return DBManager.tableDelimiter;
            }
        }

        public DataBaseHead()
        {
            this._connections = new Dictionary<string, IDbConnection>();
            this._readers = new List<IDataReader>();
        }

        ~DataBaseHead()
        {
            this.Close();
        }

        public virtual void Dispose()
        {
            this.Close();
        }

        public void Close()
        {
            if (this._readers != null)
            {
                for (int index = this._readers.Count - 1; index >= 0; --index)
                {
                    if (this._readers[index] != null)
                    {
                        this._readers[index].Close();
                        this._readers[index].Dispose();
                        this._readers.RemoveAt(index);
                    }
                }
            }
            if (this._readers != null)
                this._readers.Clear();
            this._readers = (List<IDataReader>)null;
            if (this._connections != null)
            {
                foreach (KeyValuePair<string, IDbConnection> connection in this._connections)
                {
                    if (connection.Value != null)
                    {
                        try
                        {
                            ((MyDbConnection)connection.Value).RealClose();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }
            if (this._connections != null)
                this._connections.Clear();
            this._connections = (Dictionary<string, IDbConnection>)null;
        }

        protected void CloseReader(ref IDataReader reader)
        {
            if (reader == null)
                return;
            if (!reader.IsClosed)
                reader.Close();
            reader.Dispose();
            this._readers.Remove(reader);
            reader = (IDataReader)null;
        }

        public static string MDY(int month, int day, int year)
        {
            return DBManager.MDY(month, day, year);
        }
    }
}
