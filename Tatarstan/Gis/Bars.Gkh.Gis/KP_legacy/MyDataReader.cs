// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.DataBase.MyDataReader
// Assembly: KP50.IFMX.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4C75160A-0424-4438-B01F-5EF7B8BCCD1C
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.IFMX.Common.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Data;

    public class MyDataReader
    {
        protected IDataReader _reader;
        protected IDbCommand _command;

        public object this[int i]
        {
            get
            {
                if (this._reader == null)
                    return (object)DBNull.Value;
                return this._reader[i];
            }
        }

        public object this[string name]
        {
            get
            {
                if (this._reader == null)
                    return (object)DBNull.Value;
                return this._reader[name];
            }
        }

        public void setReaderAndCommand(IDataReader reader, IDbCommand command)
        {
            this.Close();
            this._reader = reader;
            this._command = command;
        }

        public bool Read()
        {
            if (this._reader != null)
                return this._reader.Read();
            return false;
        }

        public void Close()
        {
            if (this._reader != null)
            {
                if (!this._reader.IsClosed)
                {
                    this._reader.Close();
                    this._reader.Dispose();
                }
                this._reader = (IDataReader)null;
            }
            if (this._command == null)
                return;
            this._command.Dispose();
            this._command = (IDbCommand)null;
        }
    }
}
