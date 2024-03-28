// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Global.Finder
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class Finder
    {
        private string _pref;
        private string _point;
        private string _trace;
        private string _uname;
        private string _login;
        private string _remoteLogin;
        private string _database;
        [DataMember]
        public List<string> dopFind;
        [DataMember]
        public List<int> dopPointList;
        [DataMember]
        public int prevPage;
        [DataMember]
        public List<_RolesVal> RolesVal;

        [DataMember]
        public int nzp_user { get; set; }

        [DataMember]
        public int nzp_user_main { get; set; }

        [DataMember]
        public int skip { get; set; }

        [DataMember]
        public int sortby { get; set; }

        [DataMember]
        public int rows { get; set; }

        [DataMember]
        public string pref
        {
            get
            {
                return Utils.ENull(this._pref);
            }
            set
            {
                this._pref = value;
            }
        }

        [DataMember]
        public string point
        {
            get
            {
                return Utils.ENull(this._point);
            }
            set
            {
                this._point = value;
            }
        }

        [DataMember]
        public int nzp_wp { get; set; }

        [DataMember]
        public int nzp_server { get; set; }

        [DataMember]
        public string trace
        {
            get
            {
                return Utils.ENull(this._trace);
            }
            set
            {
                this._trace = value;
            }
        }

        [DataMember]
        public string database
        {
            get
            {
                return Utils.ENull(this._database);
            }
            set
            {
                this._database = value;
            }
        }

        [DataMember]
        public string webUname
        {
            get
            {
                return Utils.ENull(this._uname);
            }
            set
            {
                this._uname = value;
            }
        }

        [DataMember]
        public string webLogin
        {
            get
            {
                return Utils.ENull(this._login);
            }
            set
            {
                this._login = value;
            }
        }

        [DataMember]
        public string remoteLogin
        {
            get
            {
                return Utils.ENull(this._remoteLogin);
            }
            set
            {
                this._remoteLogin = value;
            }
        }

        [DataMember]
        public string date_begin { get; set; }

        [DataMember]
        public int nzp_role { get; set; }

        [DataMember]
        public string bank { get; set; }

        [DataMember]
        public List<_OrderingField> orderings { get; set; }

        [DataMember]
        public int listNumber { get; set; }

        [DataMember]
        public int checkDataBlocking { get; set; }

        [DataMember]
        public string comment_action { get; set; }

        public Finder()
        {
            this.nzp_wp = -999987654;
            this.nzp_server = -999987654;
            this.nzp_user = 0;
            this.nzp_user_main = 0;
            this.webUname = "";
            this.webLogin = "";
            this._remoteLogin = "";
            this.skip = 0;
            this.sortby = 0;
            this.rows = 0;
            this.pref = "";
            this.dopFind = (List<string>)null;
            this.dopPointList = (List<int>)null;
            this.RolesVal = (List<_RolesVal>)null;
            this.prevPage = 0;
            this.date_begin = "";
            this.database = "";
            this.nzp_role = 0;
            this.bank = "";
            this.orderings = (List<_OrderingField>)null;
            this.checkDataBlocking = 1;
            this.listNumber = -1;
            this.comment_action = "";
        }

        public void CopyTo(Finder destination)
        {
            if (destination == null)
                return;
            destination.nzp_wp = this.nzp_wp;
            destination.nzp_server = this.nzp_server;
            destination.nzp_user = this.nzp_user;
            destination.webUname = this.webUname;
            destination.webLogin = this.webLogin;
            destination._remoteLogin = this._remoteLogin;
            destination.skip = this.skip;
            destination.sortby = this.sortby;
            destination.rows = this.rows;
            destination.pref = this.pref;
            destination.dopFind = this.dopFind;
            destination.dopPointList = this.dopPointList;
            destination.RolesVal = this.RolesVal;
            destination.prevPage = this.prevPage;
            destination.date_begin = this.date_begin;
            destination.database = this.database;
            destination.nzp_role = this.nzp_role;
            destination.orderings = this.orderings;
        }

        public bool InPointList(int nzp_wp)
        {
            if (this.dopPointList != null)
                return this.dopPointList.Any<int>((Func<int, bool>)(p => nzp_wp == p));
            return true;
        }
    }
}
