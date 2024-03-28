// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.Ls
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Лицевой счет
    /// </summary>
    [Serializable]
    [DataContract]
    public class Ls : Dom
    {
        string _pkod;
        string _stypek;
        string _state;
        string _fio;
        string _nkvar;
        string _nkvar_n;
        string _uch;
        string _porch;
        string _phone;
        string _mob_phone;
        string _kvar_phone;
        string _nkvar_po;

        //string _remark;

        /// <summary>
        /// Состояния ЛС
        /// </summary>
        public enum States
        {
            /// <summary>
            /// Свойство не задано
            /// </summary>
            None = 0,

            /// <summary>
            /// Открыт
            /// </summary>
            Open = 1,

            /// <summary>
            /// Закрыт
            /// </summary>
            Closed = 2,

            /// <summary>
            /// Состояние не определено
            /// </summary>
            Undefined = 3
        }

        /// <summary>
        /// Типы ЛС
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// Свойство не задано
            /// </summary>
            None = 0,

            /// <summary>
            /// Население
            /// </summary>
            Residents = 1,

            /// <summary>
            /// Бюджет
            /// </summary>
            Municipal = 2,

            /// <summary>
            /// Арендаторы
            /// </summary>
            Renters = 3
        }


        [DataMember]
        public int gil_kol { get; set; }

        [DataMember]
        public int nzp_kvar { get; set; }

        [DataMember]
        public int num_ls { get; set; }

        [DataMember]
        public string num_ls_s { get; set; }

        [DataMember]
        public int nzp_da { get; set; }

        [DataMember]
        public int nzp_old_da { get; set; }

        [DataMember]
        public int pkod10 { get; set; }

        [DataMember]
        public string num_ls_litera { get; set; }

        [DataMember]
        public string pkod
        {
            get { return Utils.ENull(this._pkod); }
            set { this._pkod = value; }
        }

        [DataMember]
        public string pkod_supp { set; get; }

        [DataMember]
        public string stypek
        {
            get { return Utils.ENull(this._stypek); }
            set { this._stypek = value; }
        }

        /// <summary>
        /// состояние ЛС
        /// </summary>
        [DataMember]
        public string state
        {
            get { return Utils.ENull(this._state); }
            set { this._state = value; }
        }

        /// <summary>
        /// код состояния ЛС
        /// </summary>
        [DataMember]
        public int stateID { get; set; }

        /// <summary>
        /// перенос ЛС в новую УК/ЖЭУ
        /// </summary>
        [DataMember]
        public bool moving { get; set; }

        /// <summary>
        /// коды состояния ЛС (при множественном выборе)
        /// </summary>
        [DataMember]
        public List<int> stateIDs { get; set; }

        /// <summary>
        /// дата, на которую/с которой действует состояние ЛС
        /// </summary>
        [DataMember]
        public string stateValidOn { get; set; }

        [DataMember]
        public int checkstateIncalcMonth { get; set; }

        [DataMember]
        public string fio
        {
            get { return Utils.ENull(this._fio); }
            set { this._fio = value; }
        }

        [DataMember]
        public int typek { get; set; }

        [DataMember]
        public string nkvar
        {
            get { return Utils.ENull(this._nkvar); }
            set { this._nkvar = value; }
        }

        [DataMember]
        public string nkvar_n
        {
            get { return Utils.ENull(this._nkvar_n); }
            set { this._nkvar_n = value; }
        }

        [DataMember]
        public string uch
        {
            get { return Utils.ENull(this._uch); }
            set { this._uch = value; }
        }

        [DataMember]
        public string porch
        {
            get { return Utils.ENull(this._porch); }
            set { this._porch = value; }
        }

        [DataMember]
        public string phone
        {
            get { return Utils.ENull(this._phone); }
            set { this._phone = value; }
        }

        [DataMember]
        public string mob_phone
        {
            get { return Utils.ENull(this._mob_phone); }
            set { this._mob_phone = value; }
        }

        [DataMember]
        public string kvar_phone
        {
            get { return Utils.ENull(this._kvar_phone); }
            set { this._kvar_phone = value; }
        }

        [DataMember]
        public string nkvar_po
        {
            get { return Utils.ENull(this._nkvar_po); }
            set { this._nkvar_po = value; }
        }

        //[DataMember]
        //public string remark { get { return Utils.ENull(_remark); } set { _remark = value; } }

        [DataMember]
        public int is_pasportist { get; set; }

        [DataMember]
        public List<Prm> dopParams { get; set; }

        /// <summary>
        /// 0 - не проверять существование лс по адресу, 1- проверять
        /// </summary>
        [DataMember]
        public int chekexistls { get; set; }

        [DataMember]
        public string dat_calc { get; set; }

        /// <summary>
        /// 0 - не генерировать параметры и услуги по номеру ЛС из поля genLsFrim, 1- генерировать
        /// </summary>
        [DataMember]
        public int copy_ls { get; set; }

        /// <summary>
        /// num_ls ЛС, по которому генерируются параметры и услуги
        /// </summary>
        [DataMember]
        public int copy_ls_from { get; set; }

        /// <summary>
        /// 0 - не генерировать ПУ, 1- генерировать
        /// </summary>
        [DataMember]
        public int gen_pu { get; set; }

        /// <summary>
        /// Квартира с (для фильтра)
        /// </summary>
        [DataMember]
        public int kvar_s { set; get; }

        /// <summary>
        /// Квартира по (для фильтра)
        /// </summary>
        [DataMember]
        public int kvar_po { set; get; }

        [DataMember]
        public int ikvar { set; get; }

        /// <summary>
        /// Вернуть группы - используется в LoadKvarList
        /// </summary>
        [DataMember]
        public bool is_get_group { set; get; }

        /// <summary>
        /// Количество дней неоплаты для должников
        /// </summary>
        [DataMember]
        public int unpayment_days { set; get; }


        [DataMember]
        public List<string> dopUsl { get; set; }

        [DataMember]
        public List<string> BankAccounts { get; set; }

        [DataMember]
        public bool withUk { get; set; }

        [DataMember]
        public bool withGeu { get; set; }

        [DataMember]
        public string IRC { get; set; }

        [DataMember]
        public string IRC_address { get; set; }

        [DataMember]
        public string IRC_phone { get; set; }

        /// <summary>
        /// Комфортность (изолированная/коммунальная)
        /// </summary>
        [DataMember]
        public string comf { get; set; }

        /// <summary>
        /// Статус (приватизированная/не приватизированная)
        /// </summary>
        [DataMember]
        public string status { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        [DataMember]
        public string ob_pl { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        [DataMember]
        public string gil_pl { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        public string note { get; set; }


        /// <summary>
        /// количество зарегистрированных жильцов
        /// </summary>
        [DataMember]
        public int reg_gil_kol { get; set; }

        /// <summary>
        /// количество комнат
        /// </summary>
        [DataMember]
        public int rooms_amount { get; set; }

        [DataMember]
        public bool withUchastok { get; set; }

        [DataMember]
        public string mnogkv { get; set; }

        [DataMember]
        public string dat_privat { get; set; }

        public override string getAddress()
        {
            string address = base.getAddress();
            this.getLsAddress(ref address);
            return address;
        }

        public override string getAddressFromUlica()
        {
            string address = base.getAddressFromUlica();
            this.getLsAddress(ref address);
            return address;
        }

        private void getLsAddress(ref string address)
        {
            if (this.nkvar != "" && this.nkvar != "-")
                address += ", кв. " + this.nkvar;
            if (this.nkvar_n != "" && this.nkvar_n != "-")
                address += ", комн. " + this.nkvar_n;
        }

        /// <summary>
        /// Абонент Физ лицо
        /// </summary>
        [DataMember]
        public int PersonId { get; set; }

        /// <summary>
        /// Абонент юр лицо
        /// </summary>
        [DataMember]
        public int NzpPayer { get; set; }

        public Ls()
            : base()
        {
            this.nzp_kvar = Constants._ZERO_;
            this.num_ls = Constants._ZERO_;
            this.num_ls_s = "";
            this.num_ls_litera = "";
            this.pkod = "";
            this.typek = Constants._ZERO_;
            this.stateID = Constants._ZERO_;
            this.moving = false;
            this.stateValidOn = "";
            this.fio = "";
            this.nkvar = "";
            this.nkvar_n = "";
            this.checkstateIncalcMonth = 0;
            this.uch = "";
            this.porch = "";
            this.phone = "";
            this.mob_phone = "";
            this.kvar_phone = "";
            this.nkvar_po = "";

            this.remark = "";
            this.chekexistls = 1;
            this.is_pasportist = 0;
            this.mark = 1;
            this.BankAccounts = new List<string>();
            this.gil_kol = 0;

            this.dopParams = null;
            this.dopUsl = null;
            this.dat_calc = "";

            this.copy_ls = 0;
            this.copy_ls_from = 0;
            this.gen_pu = 0;
            this.kvar_s = 0;
            this.kvar_po = 0;
            this.withUk = this.withGeu = this.withUchastok = false;

            this.PersonId = this.NzpPayer = 0;

            this.IRC = "";
            this.IRC_address = "";
            this.IRC_phone = "";

            this.comf = "";
            this.status = "";

            this.ob_pl = "";
            this.gil_pl = "";
            this.reg_gil_kol = 0;
            this.rooms_amount = 0;
            this.note = "";
            this.mnogkv = "";
            this.dat_privat = "";
        }

        public void CopyAttributes(Ls newLs)
        {
            if (newLs == null)
                return;
            this.CopyTo(newLs);
            newLs.nzp_kvar = this.nzp_kvar;
            newLs.num_ls = this.num_ls;
            newLs.num_ls_s = this.num_ls_s;
            newLs.num_ls_litera = this.num_ls_litera;
            newLs.pkod = this.pkod;
            newLs.typek = this.typek;
            newLs.stateID = this.stateID;
            newLs.moving = this.moving;
            newLs.stateValidOn = this.stateValidOn;
            newLs.fio = this.fio;
            newLs.nkvar = this.nkvar;
            newLs.nkvar_n = this.nkvar_n;
            newLs.checkstateIncalcMonth = this.checkstateIncalcMonth;
            newLs.uch = this.uch;
            newLs.porch = this.porch;
            newLs.phone = this.phone;
            newLs.nkvar_po = this.nkvar_po;
            newLs.remark = this.remark;
            newLs.chekexistls = this.chekexistls;
            newLs.is_pasportist = this.is_pasportist;
            newLs.mark = this.mark;

            newLs.gil_kol = this.gil_kol;
            newLs.reg_gil_kol = this.reg_gil_kol;

            newLs.dopParams = this.dopParams;
            newLs.dopUsl = this.dopUsl;
            newLs.dat_calc = this.dat_calc;

            newLs.copy_ls = this.copy_ls;
            newLs.copy_ls_from = this.copy_ls_from;
            newLs.gen_pu = this.gen_pu;
            newLs.kvar_s = this.kvar_s;
            newLs.kvar_po = this.kvar_po;
            newLs.withUk = this.withUk;
            newLs.withGeu = this.withGeu;
            newLs.withUchastok = this.withUchastok;

            newLs.IRC = this.IRC;
            newLs.IRC_address = this.IRC_address;
            newLs.IRC_phone = this.IRC_phone;

            newLs.comf = this.comf;
            newLs.status = this.status;
            newLs.ob_pl = this.ob_pl;
            newLs.gil_pl = this.gil_pl;
            newLs.rooms_amount = this.rooms_amount;
            newLs.note = this.note;
            newLs.mnogkv = this.mnogkv;
            newLs.dat_privat = this.dat_privat;
        }
    }
}
