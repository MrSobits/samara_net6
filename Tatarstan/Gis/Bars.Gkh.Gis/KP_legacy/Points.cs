namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public static class Points               //список доступных банков данных
    //----------------------------------------------------------------------
    {
        public static _PointWebData pointWebData;  //необходимые данные об основной базе для web-клиента
        public static _Point Point;

        //public static bool IsSmr;   //признак базы Самары - пока в лоб!
        //public static bool IsPoint; //имеется ли центральная база
        //public static bool IsFabric;//установлена фабрика серверов БД
        //public static DateTime dateOper = DateTime.MinValue;
        //public static RecordMonth CalcMonth; //текущий расчетный месяц
        //public static List<RecordMonth> CalcMonths = new List<RecordMonth>(); //список расчетных месяцев
        //public static RecordMonth BeginWork; //дата начала работы системы
        //public static RecordMonth BeginCalc; //дата начала расчетов (глубина перерасчетов)

        public static bool Is50 { get { return Points.pointWebData.is50; } set { Points.pointWebData.is50 = value; } }
        public static bool IsDemo { get { return Points.pointWebData.isDemo; } set { Points.pointWebData.isDemo = value; } }
        /// <summary>
        /// Регион установки
        /// </summary>
        public static Regions.Region Region { get { return Regions.GetById(Points.pointWebData.region); } set { Points.pointWebData.region = (int)value; } }
        /// <summary>
        /// Тип  платежного кода по умолчанию
        /// </summary>
        public static int DefaultPkodType { get { return Points.pointWebData.DefaultPkodType; } set { Points.pointWebData.DefaultPkodType = (int)value; } }
        public static bool IsSmr { get { return Points.pointWebData.isSamara; } set { Points.pointWebData.isSamara = value; } }
        public static bool IsPoint { get { return Points.pointWebData.isPoint; } set { Points.pointWebData.isPoint = value; } }
        public static bool IsFabric { get { return Points.pointWebData.isFabric; } set { Points.pointWebData.isFabric = value; } }
        public static bool IsCalcSubsidy { get { return Points.pointWebData.isCalcSubsidy; } set { Points.pointWebData.isCalcSubsidy = value; } }
        /// <summary>
        /// Режим перерасчета начислений
        /// </summary>
        public static RecalcModes RecalcMode { get { return Points.pointWebData.recalcMode; } set { Points.pointWebData.recalcMode = value; } }
        /// <summary>
        /// Тип финансовой системы
        /// </summary>
        public static bool isFinances { get { return Points.pointWebData.isFinances; } set { Points.pointWebData.isFinances = value; } }
        public static DateTime DateOper { get { return Points.pointWebData.dateOper; } set { Points.pointWebData.dateOper = value; } }
        public static RecordMonth CalcMonth { get { return Points.pointWebData.calcMonth; } set { Points.pointWebData.calcMonth = value; } }
        public static Dictionary<int, RecordMonth> calcMonthAreas { get { return Points.pointWebData.calcMonthAreas; } set { Points.pointWebData.calcMonthAreas = value; } }
        public static RecordMonth BeginWork { get { return Points.pointWebData.beginWork; } set { Points.pointWebData.beginWork = value; } }
        public static RecordMonth BeginCalc { get { return Points.pointWebData.beginCalc; } set { Points.pointWebData.beginCalc = value; } }
        public static bool isInitSuccessfull { get { return Points.pointWebData.isInitSuccessfull; } set { Points.pointWebData.isInitSuccessfull = value; } }   //данные (признаки, расчетные месяцы, банки данных и т.д.) при первом запуске успешно загружены
        /// <summary>
        /// Признак, сохранять ли вводимые показания ПУ в основной банк
        /// </summary>
        public static bool SaveCounterReadingsToRealBank { get { return Points.pointWebData.SaveCounterReadingsToRealBank; } set { Points.pointWebData.SaveCounterReadingsToRealBank = value; } }
        /// <summary>
        /// Признак, что используется клон базы данных
        /// </summary>
        public static bool isClone { get { return Points.pointWebData.isClone; } set { Points.pointWebData.isClone = value; } }
        /// <summary>
        /// Запускать фоновые потоки обработки задач
        /// </summary>
        public static bool StartBackgroundThreads { get { return Points.pointWebData.StartBackgroundThreads; } set { Points.pointWebData.StartBackgroundThreads = value; } }

        /// <summary>
        /// Параметры распределения оплат в финансовой системе
        /// </summary>
        public static PackDistributionParameters packDistributionParameters;

        public static List<RecordMonth> CalcMonths { get { return Points.pointWebData.calcMonths; } set { Points.pointWebData.calcMonths = value; } }

        public static bool IsMultiHost { get { return MultiHost.IsMultiHost; } } //признак мультихостинга (Минстрой)
        public static string Pref;           //префикс центральной базы ifmx:erc_kernel или исходный каталог для fdb:tk0
        public static string g_kod_erc;      //глобальный префикс платежного кода (default)

        public static List<_Point> PointList = new List<_Point>(); //список БД
        public static List<_Server> Servers = new List<_Server>(); //список локальных серверов БД

        public static string mainPageName { get { return Points.pointWebData.MainPageName; } set { Points.pointWebData.MainPageName = value; } }

        /// <summary>
        /// Признак, что в таблице counters есть поле ngp_cnt
        /// если хотя бы в одном банке поля ngp_cnt нет, то IsIpuHasNgpCnt = false 
        /// </summary>
        public static bool IsIpuHasNgpCnt { get { return Points.pointWebData.isIpuHasNgpCnt; } set { Points.pointWebData.isIpuHasNgpCnt = value; } }

        /// <summary>
        /// Параметр управления новыми записями через series
        /// </summary>
        public static bool isUseSeries { get { return Points.pointWebData.isUseSeries; } set { Points.pointWebData.isUseSeries = value; } }

        /// <summary>
        /// Тип функции генерации платежных кодов
        /// </summary>
        public static FunctionsTypesGeneratePkod functionTypeGeneratePkod { set; get; }

        //public static ParamsBasePkod BasePkod { get; set; }

        /// <summary> Логгировать ли все события/запросы в хосте </summary>
        public static bool FullLogging { get; set; }

        /// <summary> Действует новый режим нормативов(меняется реализации получения нормативов при расчете,отображение в хар-х жилья) </summary>
        public static bool isNewNorms { get { return Points.pointWebData.isNewNorms; } set { if (value) Points.pointWebData.isNewNorms = value; } }
        //
        public static void SetPointWebData(_PointWebData p)
        {
            Points.IsDemo = p.isDemo;
            Points.Is50 = p.is50;
            Points.IsSmr = p.isSamara;
            Points.Region = Regions.GetById(p.region);
            Points.DefaultPkodType = p.DefaultPkodType;
            Points.IsPoint = p.isPoint;
            Points.CalcMonth = p.calcMonth;
            Points.calcMonthAreas = p.calcMonthAreas;
            Points.BeginWork = p.beginWork;
            Points.BeginCalc = p.beginCalc;
            Points.CalcMonths = p.calcMonths;
            Points.DateOper = p.dateOper;
            Points.isFinances = p.isFinances;
            Points.isInitSuccessfull = p.isInitSuccessfull;
            Points.SaveCounterReadingsToRealBank = p.SaveCounterReadingsToRealBank;
            Points.isClone = p.isClone;
            Points.StartBackgroundThreads = p.StartBackgroundThreads;
            Points.IsCalcSubsidy = p.isCalcSubsidy;
            Points.mainPageName = p.MainPageName;
            Points.RecalcMode = p.recalcMode;
            Points.IsIpuHasNgpCnt = p.isIpuHasNgpCnt;
            Points.isUseSeries = p.isUseSeries;
            Points.FullLogging = p.FullLogging;
            Points.isNewNorms = p.isNewNorms;
        }

        public static _PointWebData GetPointWebData()
        {
            _PointWebData p = new _PointWebData(false);

            p.isDemo = Points.IsDemo;
            p.is50 = Points.Is50;
            p.isSamara = Points.IsSmr;
            p.DefaultPkodType = Points.DefaultPkodType;
            p.region = (int)Points.Region;
            p.isPoint = Points.IsPoint;
            p.calcMonth = Points.CalcMonth;
            p.calcMonthAreas = Points.calcMonthAreas;
            p.beginWork = Points.BeginWork;
            p.beginCalc = Points.BeginCalc;
            p.calcMonths = Points.CalcMonths;
            p.dateOper = Points.DateOper;
            p.isFinances = Points.isFinances;
            p.isInitSuccessfull = Points.isInitSuccessfull;
            p.SaveCounterReadingsToRealBank = Points.SaveCounterReadingsToRealBank;
            p.isClone = Points.isClone;
            p.StartBackgroundThreads = Points.StartBackgroundThreads;
            p.isCalcSubsidy = Points.IsCalcSubsidy;
            p.MainPageName = Points.mainPageName;
            p.recalcMode = Points.RecalcMode;
            p.isIpuHasNgpCnt = Points.IsIpuHasNgpCnt;
            p.isUseSeries = Points.isUseSeries;
            return p;
        }

        //
        public static _Server GetServer(int nzp_server)
        {
            foreach (_Server zap in Points.Servers)
            {
                if (nzp_server == zap.nzp_server)
                {
                    return zap;
                }
            }
            return new _Server(); //ошибка!
        }

        //определение префиксов Points.PointList
        static _Point GetPoint(string pref, List<_Point> pointlist)
        {
            int nzp_wp;
            if (Int32.TryParse(pref, out nzp_wp))
            {
                foreach (_Point point in pointlist)
                {
                    if (point.nzp_wp.ToString() == pref) return point;
                }
            }
            else
            {
                foreach (_Point point in pointlist)
                {
                    if (point.pref == pref) return point;
                }
            }
            return new _Point();
        }
        static _Point GetPoint(int nzp_wp, List<_Point> pointlist)
        {
            foreach (_Point point in pointlist)
            {
                if (point.nzp_wp == nzp_wp) return point;
            }
            return new _Point();
        }
        static string GetPref(int nzp_wp, List<_Point> pointlist)
        {
            if (nzp_wp > 0)
                return Points.GetPoint(nzp_wp, pointlist).pref;

            return Points.Pref;
        }
        static int GetPref(string pref, List<_Point> pointlist)
        {
            if (!String.IsNullOrEmpty(pref))
                return Points.GetPoint(pref, pointlist).nzp_wp;

            return 0;
        }

        public static _Point GetPoint(string pref)
        {
            return Points.GetPoint(pref, Points.PointList);
        }
        public static _Point GetPoint(int nzp_wp)
        {
            return Points.GetPoint(nzp_wp, Points.PointList);
        }
        public static string GetPref(int nzp_wp)
        {
            return Points.GetPref(nzp_wp, Points.PointList);
        }
        public static int GetPref(string pref)
        {
            return Points.GetPref(pref, Points.PointList);
        }

        //процедура достает connection по nzp_wp или pref
        public static string GetConnByPref(int nzp_wp, int nzp_server, string pref)
        {
            string conn = Constants.cons_Kernel; //по-умолчанию
            if (!Points.IsFabric)
                return conn;

            if (nzp_wp < 1 && nzp_server < 1 && String.IsNullOrEmpty(pref))
                return conn;

            if (nzp_server < 1)
            {
                foreach (_Point point in Points.PointList)
                {
                    if (point.nzp_wp == nzp_wp || point.pref == pref)
                    {
                        nzp_server = point.nzp_server;
                        break;
                    }
                }
            }

            if (nzp_server < 1)
                return conn;

            foreach (_Server server in Points.Servers)
            {
                if (server.nzp_server == nzp_server)
                {
                    conn = server.conn;
                    break;
                }
            }
            return conn;
        }
        public static string GetConnByPref(int nzp_wp, int nzp_server)
        {
            return Points.GetConnByPref(nzp_wp, nzp_server, "");
        }
        public static string GetConnByPref(string pref)
        {
            return Points.GetConnByPref(0, 0, pref);
        }
        public static string GetConnByServer(int nzp_server)
        {
            return Points.GetConnByPref(0, nzp_server, "");
        }
        public static string GetConnByWp(int nzp_wp)
        {
            return Points.GetConnByPref(nzp_wp, 0, "");
        }
        public static string GetConnKernel(int nzp_wp, int nzp_server)
        {
            return Points.GetConnByPref(nzp_wp, nzp_server, "");
        }

        //вытащить номер потока расчета
        public static int GetCalcNum(string pref)
        {
            return Points.GetCalcNum(0, pref);
        }
        public static int GetCalcNum(int nzp_wp)
        {
            return Points.GetCalcNum(nzp_wp, "");
        }
        public static int GetCalcNum(int nzp_wp, string pref)
        {
            int num = 0;
            if (nzp_wp < 1 && String.IsNullOrEmpty(pref))
                return num;

            foreach (_Point point in Points.PointList)
            {
                if (point.nzp_wp == nzp_wp || point.pref == pref)
                {
                    num = point.flag;
                    break;
                }
            }

            return num;
        }

        /// <summary>Получить расчетный месяц</summary>
        public static RecordMonth GetCalcMonth(CalcMonthParams prms)
        {
            if (prms != null && prms.pref != "" && prms.pref != Points.Pref) return Points.GetPoint(prms.pref).CalcMonth;
            return Points.CalcMonth;
        }

        /// <summary>Получить расчетный месяц</summary>
        /// <param name="pref">Префикс банка данных</param>
        public static RecordMonth GetCalcMonth(string pref)
        {
            var prms = new CalcMonthParams(pref);
            return Points.GetCalcMonth(prms);
        }
    }

    //----------------------------------------------------------------------
    [DataContract]
    public struct _PointWebData   //данные об основной базе для web-клиента
    //----------------------------------------------------------------------
    {
        [DataMember]
        public bool isDemo;
        [DataMember]
        public bool is50;
        [DataMember]
        public bool isSamara;         //признак базы Самары - пока в лоб!
        [DataMember]
        public int region;          //регион
        [DataMember]
        public int DefaultPkodType;          //тип платежного кода по умолчанию
        [DataMember]
        public bool isPoint;          //имеется ли центральная база
        [DataMember]
        public bool isFabric;         //установлена фабрика серверов БД
        [DataMember]
        public bool isBroker;         //выполняется коннект к брокеру

        [DataMember]
        public RecordMonth calcMonth; //текущий расчетный месяц
        [DataMember]
        public Dictionary<int, RecordMonth> calcMonthAreas; //текущий расчетный месяц
        [DataMember]
        public List<RecordMonth> calcMonths; //список расчетных месяцев
        [DataMember]
        public RecordMonth beginWork; //дата начала работы системы
        [DataMember]
        public RecordMonth beginCalc; //дата начала расчетов (глубина перерасчетов)
        [DataMember]
        public DateTime dateOper;     //опердень
        /// <summary>
        /// Тип финансовой системы
        /// </summary>
        [DataMember]
        public bool isFinances;   //наличие финсистемы
        [DataMember]
        public bool isInitSuccessfull;   //данные (признаки, расчетные месяцы, банки данных и т.д.) при первом запуске успешно загружены
        /// <summary>
        /// Признак, сохранять ли вводимые показания ПУ в основной банк
        /// </summary>
        [DataMember]
        public bool SaveCounterReadingsToRealBank;
        [DataMember]
        public bool isClone;
        /// <summary>
        /// Запускать фоновые потоки обработки задач
        /// </summary>
        [DataMember]
        public bool StartBackgroundThreads;

        [DataMember]
        public bool isCalcSubsidy;

        [DataMember]
        public RecalcModes recalcMode;

        [DataMember]
        public string MainPageName;

        [DataMember]
        public bool isIpuHasNgpCnt;

        [DataMember]
        public bool isUseSeries;

        /// <summary>
        /// Сохранять ли в логах все запросы и события хоста
        /// </summary>
        [DataMember]
        public bool FullLogging;

        /// <summary>
        /// Используются новые нормативы
        /// </summary>
        [DataMember]
        public bool isNewNorms;
        /// <summary>
        /// Версия postgres >=9.4
        /// </summary>
        [DataMember]
        public bool postgreSQLVersionOlder94;
        public _PointWebData(bool b)
        {
            this.isDemo = false;
            this.is50 = false;
            this.isSamara = false;
            this.isPoint = false;
            this.DefaultPkodType = 1;
            this.isFabric = false;
            this.isBroker = false;
            this.region = (int)Regions.Region.None;
            this.calcMonth = new RecordMonth();
            this.calcMonthAreas = new Dictionary<int, RecordMonth>();
            this.beginWork = new RecordMonth();
            this.beginCalc = new RecordMonth();
            this.calcMonths = new List<RecordMonth>();
            this.dateOper = DateTime.MinValue;
            this.isFinances = false;
            this.isInitSuccessfull = false;
            this.SaveCounterReadingsToRealBank = false;
            this.isClone = false;
            this.StartBackgroundThreads = true;
            this.isCalcSubsidy = false;
            this.MainPageName = "";
            this.recalcMode = RecalcModes.None;
            this.isIpuHasNgpCnt = false;
            this.isUseSeries = false;
            this.FullLogging = false;
            this.isNewNorms = false;
            this.postgreSQLVersionOlder94 = false;
        }

    }
    //----------------------------------------------------------------------


    //----------------------------------------------------------------------
    [DataContract]
    public struct _Server   //сервер БД (servers)
    //----------------------------------------------------------------------
    {
        string _ip_adr;
        string _login;
        string _pwd;
        string _nserver;
        string _ol_server;
        string _conn;

        [DataMember]
        public bool is_valid { get; set; }

        [DataMember]
        public int nzp_server { get; set; }
        [DataMember]
        public string ip_adr { get { return Utils.ENull(this._ip_adr); } set { this._ip_adr = value; } }
        [DataMember]
        public string login { get { return Utils.ENull(this._login); } set { this._login = value; } } //
        [DataMember]
        public string pwd { get { return Utils.ENull(this._pwd); } set { this._pwd = value; } } //
        [DataMember]
        public string nserver { get { return Utils.ENull(this._nserver); } set { this._nserver = value; } } //
        [DataMember]
        public string ol_server { get { return Utils.ENull(this._ol_server); } set { this._ol_server = value; } } //
        [DataMember]
        public string conn { get { return Utils.ENull(this._conn); } set { this._conn = value; } } //
    };

    public class PackDistributionParameters
    {
        /// <summary>
        /// Стратегии распределения оплат
        /// </summary>
        public enum Strategies
        {
            /// <summary>
            /// стандартная схема, погашение недействующих услуг приоритетно
            /// </summary>
            InactiveServicesFirst = 1,

            /// <summary>
            /// стандартная схема, погашение действующих услуг приоритетно
            /// </summary>
            ActiveServicesFirst = 2,

            /// <summary>
            /// стандартная схема, погашение действующих и недействующих услуг равноправно
            /// </summary>
            NoPriority = 3,

            /// <summary>
            /// Пропорционально вх. сальдо (если оплата до 20 числа),
            /// Пропорционально начислению за месяц с учетом недопоставок (если оплата 20 числа или позднее)
            /// </summary>
            Samara = 0
        }

        /// <summary>
        /// Способы начисления к оплате
        /// </summary>
        public enum ChargeMethods
        {
            /// <summary>
            /// Исходящее сальдо
            /// </summary>
            Outsaldo = 1,

            /// <summary>
            /// Положительная часть исходящего сальдо
            /// </summary>
            PositiveOutsaldo = 2,

            /// <summary>
            /// Начисления за месяц с учетом перерасчетов, недопоставок, изменений сальдо и переплат
            /// </summary>
            MonthlyCalculationWithChangesAndOverpayment = 3,

            /// <summary>
            /// Положительная часть начислений за месяц с учетом перерасчетов, недопоставок, изменений сальдо и переплат
            /// </summary>
            PositiveMonthlyCalculationWithChangesAndOverpayment = 4,

            /// <summary>
            /// Начисления за месяц с учетом перерасчетов, недопоставок и изменений сальдо
            /// </summary>
            MonthlyCalculationWithChanges = 5,

            /// <summary>
            /// Положительная часть начислений за месяц с учетом перерасчетов, недопоставок и изменений сальдо
            /// </summary>
            PositiveMonthlyCalculationWithChanges = 6
        }

        /// <summary>
        /// Порядок гашения пени
        /// </summary>
        public enum OrderRepayPeni
        {
            /// <summary>
            /// гасить пени в первую очередь
            /// </summary>
            First = 1,

            /// <summary>
            /// равномерно гасить пени
            /// </summary>
            Equally = 2,

            /// <summary>
            /// гасить пени в последнюю очередь
            /// </summary>
            Last = 3
        }

        /// <summary>
        /// Эталон для первичного распределения оплат
        /// </summary>
        public enum PaymentDistributionMethods
        {
            /// <summary>
            /// Положительная часть начислено к оплате прошлого месяца
            /// </summary>
            LastMonthPositiveSumCharge = 1,

            /// <summary>
            /// "начислено к оплате прошлого месяца"
            /// </summary>
            LastMonthSumCharge = 2,

            /// <summary>
            ///"положительной части исходящего сальдо прошлого месяца"
            /// </summary>
            CurrentMonthPositiveSumInsaldo = 3,

            /// <summary>
            /// "исходящему сальдо прошлого месяца"
            /// Входящее сальдо текущего месяца
            /// </summary>
            CurrentMonthSumInsaldo = 4,

            /// <summary>
            ///  Положительная часть исходящего сальдо прошлого месяца
            /// </summary>
            LastMonthPositiveSumOutsaldo = 5,

            /// <summary>
            /// положительной части начислено к оплате текущего месяца   
            /// </summary>
            CurrentMonthPositiveSumCharge = 6,

            /// <summary>
            ///  положительной части исходящего сальдо текущего месяца
            /// </summary>
            CurrentMonthPositiveSumOutSaldo = 7
        }

        /// <summary>
        /// Приоритет погашения долга действующих / недействующих услуг
        /// </summary>
        public Strategies strategy { get; set; }

        /// <summary>
        /// Приоритет погашения пени
        /// </summary>
        public OrderRepayPeni repayPeni { get; set; }

        /// <summary>
        /// Распределять ли пачки сразу после загрузки
        /// </summary>
        public bool DistributePackImmediately = false;

        /// <summary>
        /// Рассчитывать суммы к перечислению автоматически при распределении/откате оплат
        /// </summary>
        public bool CalcDistributionAutomatically = false;

        /// <summary>
        /// Выполнять ли протоколирование процесса распределения оплат
        /// </summary>
        public bool EnableLog = false;

        /// <summary>
        /// Способ начисления к оплате
        /// </summary>
        public ChargeMethods chargeMethod { get; set; }

        /// <summary>
        /// Способ начисления к оплате
        /// </summary>
        public PaymentDistributionMethods distributionMethod { get; set; }

        /// <summary>
        /// Плательщик заполняет оплату по услугам
        /// </summary>
        public bool AllowSelectServicesWhilePaying = false;

        /// <summary>
        /// Список услуг, имеющих приоритет при распределении оплат
        /// </summary>
        public List<Service> PriorityServices { get; set; }
    }
    //---------------------------------------------------------------------- 
}