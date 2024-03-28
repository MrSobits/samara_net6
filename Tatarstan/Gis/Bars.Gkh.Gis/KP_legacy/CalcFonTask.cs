// Decompiled with JetBrains decompiler
// Type: STCLINE.KP50.Interfaces.CalcFonTask
// Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
// Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll
namespace Bars.Gkh.Gis.KP_legacy
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CalcFonTask : FonTaskWithYearMonth
    {
        private CalcFonTask.Types taskType;
        private int task;
        public string parameters;

        [DataMember]
        public int nzp { get; set; }

        [DataMember]
        public int nzpt { get; set; }

        [DataMember]
        public int Task
        {
            get
            {
                return this.task;
            }
            set
            {
                this.task = value;
                this.taskType = CalcFonTask.GetTypeById(value);
            }
        }

        [DataMember]
        public CalcFonTask.Types TaskType
        {
            get
            {
                return this.taskType;
            }
            set
            {
                this.taskType = value;
                this.task = (int)value;
            }
        }

        [DataMember]
        public int prior { get; set; }

        [DataMember]
        public int QueueNumber { get; set; }

        [DataMember]
        public new string processType
        {
            get
            {
                return "Задачи";
            }
        }

        [DataMember]
        public new ProcessTypes processTypeID
        {
            get
            {
                return ProcessTypes.CalcNach;
            }
        }

        [DataMember]
        public new string processName
        {
            get
            {
                switch (this.taskType)
                {
                    case CalcFonTask.Types.DistributePack:
                        return "Распределение пачки оплат";
                    case CalcFonTask.Types.CancelPackDistribution:
                        return "Отмена распределения пачки оплат";
                    case CalcFonTask.Types.CancelDistributionAndDeletePack:
                        return "Удаление пачки оплат";
                    case CalcFonTask.Types.UpdatePackStatus:
                        return "Обновление статуса пачки";
                    case CalcFonTask.Types.DistributeOneLs:
                        return "Распределение оплаты";
                    case CalcFonTask.Types.taskGetFakturaWeb:
                        return "Формирование платежного документа для портала";
                    case CalcFonTask.Types.taskToTransfer:
                        return "Учет средств к перечислению поставщикам услуг";
                    case CalcFonTask.Types.taskPreparePrintInvoices:
                        return "Подготовка данных для печати счетов";
                    case CalcFonTask.Types.taskAutomaticallyChangeOperDay:
                        return "Смена операционного дня по расписанию";
                    case CalcFonTask.Types.taskDisassembleFile:
                        return "Разбор файла с наследуемой информацией";
                    case CalcFonTask.Types.taskLoadFile:
                        return "Загрузка файла";
                    case CalcFonTask.Types.taskGeneratePkod:
                        return "Присвоение платежный кодов";
                    case CalcFonTask.Types.taskLoadKladr:
                        return "Загрузка КЛАДР";
                    case CalcFonTask.Types.taskRecalcDistribSumOutSaldo:
                        return "Перерасчет исходящего сальдо";
                    case CalcFonTask.Types.taskUpdateAddress:
                        return "Обновление адресов";
                    case CalcFonTask.Types.taskCalculateAnalytics:
                        return "Подсчет аналитики";
                    case CalcFonTask.Types.taskLoadFileFromSZ:
                        return "Загрузка файла из СЗ";
                    case CalcFonTask.Types.taskUnloadFileForSZ:
                        return "Выгрузка файла для СЗ";
                    case CalcFonTask.Types.ReCalcKomiss:
                        return "Расчет комиссий с оплат";
                    case CalcFonTask.Types.AddPrimaryKey:
                        return "Добавление первичных ключей";
                    case CalcFonTask.Types.AddIndexes:
                        return "Добавление индексов";
                    case CalcFonTask.Types.AddForeignKey:
                        return "Добавление внешних ключей";
                    case CalcFonTask.Types.taskCalcGku:
                        return "Расчет тарифов и учет расходов";
                    case CalcFonTask.Types.taskCalcCharge:
                        return "Расчет начислений и сальдо";
                    case CalcFonTask.Types.taskCalcChargeForReestr:
                        return "Подсчет сальдо для реестра";
                    case CalcFonTask.Types.taskCalcChargeForDelReestr:
                        return "Подсчет сальдо для лицевых счетов, входящих в удаленный реестр";
                    case CalcFonTask.Types.taskCalcReport:
                        return "Подсчет сводной информации по домам";
                    case CalcFonTask.Types.taskCalcGil:
                        return "Расчет количества жильцов";
                    case CalcFonTask.Types.taskCalcRashod:
                        return "Расчет расходов коммунальных";
                    case CalcFonTask.Types.taskCalcNedo:
                        return "Расчет недопоставок";
                    case CalcFonTask.Types.taskFull:
                        return "Полный расчет без перерасчета";
                    case CalcFonTask.Types.taskSaldo:
                        return "Расчет сальдо текущего месяца";
                    case CalcFonTask.Types.taskWithReval:
                        return "Полный расчет с перерасчетом";
                    case CalcFonTask.Types.uchetOplatArea:
                        return "Учет распределенных оплат в лицевых счетах управляющей компании";
                    case CalcFonTask.Types.uchetOplatBank:
                        return "Учет распределенных оплат в лицевых счетах банка данных";
                    case CalcFonTask.Types.taskRefreshAP:
                        return "Обновление адресов центрального банка";
                    case CalcFonTask.Types.taskKvar:
                        return "Расчет лицевого счета";
                    default:
                        return "";
                }
            }
        }

        public bool callReportAlone
        {
            get
            {
                if (this.taskType != CalcFonTask.Types.taskFull && this.taskType != CalcFonTask.Types.taskCalcCharge && this.taskType != CalcFonTask.Types.taskSaldo)
                    return this.taskType == CalcFonTask.Types.taskWithReval;
                return true;
            }
        }

        public bool calcFull
        {
            get
            {
                if (this.taskType != CalcFonTask.Types.taskFull)
                    return this.taskType == CalcFonTask.Types.taskDefault;
                return true;
            }
        }

        public CalcFonTask()
        {
            this.nzp = -999987654;
            this.nzpt = 0;
            this.task = -999987654;
            this.prior = 0;
            this.QueueNumber = -999987654;
        }

        public CalcFonTask(int queueNumber)
            : this()
        {
            this.QueueNumber = queueNumber;
        }

        public static CalcFonTask.Types GetTypeById(int taskId)
        {
            if (taskId == 0)
                return CalcFonTask.Types.taskDefault;
            if (taskId == 1)
                return CalcFonTask.Types.taskFull;
            if (taskId == 2)
                return CalcFonTask.Types.taskSaldo;
            if (taskId == 3)
                return CalcFonTask.Types.taskWithReval;
            if (taskId == 101)
                return CalcFonTask.Types.taskCalcGil;
            if (taskId == 111)
                return CalcFonTask.Types.taskCalcRashod;
            if (taskId == 121)
                return CalcFonTask.Types.taskCalcNedo;
            if (taskId == 131)
                return CalcFonTask.Types.taskCalcGku;
            if (taskId == 141)
                return CalcFonTask.Types.taskCalcCharge;
            if (taskId == 200)
                return CalcFonTask.Types.taskCalcReport;
            if (taskId == 222)
                return CalcFonTask.Types.DistributePack;
            if (taskId == 228)
                return CalcFonTask.Types.DistributeOneLs;
            if (taskId == 223)
                return CalcFonTask.Types.CancelPackDistribution;
            if (taskId == 224)
                return CalcFonTask.Types.CancelDistributionAndDeletePack;
            if (taskId == 227)
                return CalcFonTask.Types.UpdatePackStatus;
            if (taskId == 4)
                return CalcFonTask.Types.uchetOplatArea;
            if (taskId == 142)
                return CalcFonTask.Types.taskCalcChargeForReestr;
            if (taskId == 143)
                return CalcFonTask.Types.taskCalcChargeForDelReestr;
            if (taskId == 5)
                return CalcFonTask.Types.uchetOplatBank;
            if (taskId == 301)
                return CalcFonTask.Types.taskGetFakturaWeb;
            if (taskId == 302)
                return CalcFonTask.Types.taskToTransfer;
            if (taskId == 303)
                return CalcFonTask.Types.taskPreparePrintInvoices;
            if (taskId == 304)
                return CalcFonTask.Types.taskAutomaticallyChangeOperDay;
            if (taskId == 305)
                return CalcFonTask.Types.taskDisassembleFile;
            if (taskId == 307)
                return CalcFonTask.Types.taskGeneratePkod;
            if (taskId == 306)
                return CalcFonTask.Types.taskLoadFile;
            if (taskId == 312)
                return CalcFonTask.Types.taskLoadFileFromSZ;
            if (taskId == 313)
                return CalcFonTask.Types.taskUnloadFileForSZ;
            if (taskId == 308)
                return CalcFonTask.Types.taskLoadKladr;
            if (taskId == 875)
                return CalcFonTask.Types.ReCalcKomiss;
            if (taskId == 309)
                return CalcFonTask.Types.taskRecalcDistribSumOutSaldo;
            if (taskId == 310)
                return CalcFonTask.Types.taskUpdateAddress;
            if (taskId == 311)
                return CalcFonTask.Types.taskCalculateAnalytics;
            if (taskId == 878)
                return CalcFonTask.Types.AddIndexes;
            if (taskId == 879)
                return CalcFonTask.Types.AddForeignKey;
            return taskId == 877 ? CalcFonTask.Types.AddPrimaryKey : CalcFonTask.Types.Unknown;
        }

        public static bool TaskPack(CalcFonTask.Types task)
        {
            if (task != CalcFonTask.Types.DistributePack && task != CalcFonTask.Types.CancelPackDistribution)
                return task == CalcFonTask.Types.CancelDistributionAndDeletePack;
            return true;
        }

        public static bool TaskCalc(CalcFonTask.Types task)
        {
            if (task != CalcFonTask.Types.taskDefault && task != CalcFonTask.Types.taskFull && (task != CalcFonTask.Types.taskSaldo && task != CalcFonTask.Types.taskWithReval) && (task != CalcFonTask.Types.taskCalcGil && task != CalcFonTask.Types.taskCalcRashod && (task != CalcFonTask.Types.taskCalcNedo && task != CalcFonTask.Types.taskCalcGku)) && (task != CalcFonTask.Types.taskCalcCharge && task != CalcFonTask.Types.taskCalcReport))
                return task == CalcFonTask.Types.taskGetFakturaWeb;
            return true;
        }

        public static bool TaskCalcKvar(CalcFonTask.Types task)
        {
            return task == CalcFonTask.Types.taskKvar;
        }

        public static bool TaskRefresh(CalcFonTask.Types task)
        {
            return task == CalcFonTask.Types.taskRefreshAP;
        }

        public static bool TaskEqual(CalcFonTask.Types existingTask, CalcFonTask.Types newTask)
        {
            return existingTask == newTask;
        }

        public enum Types
        {
            Unknown = -1,
            taskDefault = 0,
            taskFull = 1,
            taskSaldo = 2,
            taskWithReval = 3,
            uchetOplatArea = 4,
            uchetOplatBank = 5,
            taskRefreshAP = 10,
            taskKvar = 33,
            taskCalcGil = 101,
            taskCalcRashod = 111,
            taskCalcNedo = 121,
            taskCalcGku = 131,
            taskCalcCharge = 141,
            taskCalcChargeForReestr = 142,
            taskCalcChargeForDelReestr = 143,
            taskCalcReport = 200,
            DistributePack = 222,
            CancelPackDistribution = 223,
            CancelDistributionAndDeletePack = 224,
            taskCalcSubsidyRequest = 225,
            taskCalcSaldoSubsidy = 226,
            UpdatePackStatus = 227,
            DistributeOneLs = 228,
            taskGetFakturaWeb = 301,
            taskToTransfer = 302,
            taskPreparePrintInvoices = 303,
            taskAutomaticallyChangeOperDay = 304,
            taskDisassembleFile = 305,
            taskLoadFile = 306,
            taskGeneratePkod = 307,
            taskLoadKladr = 308,
            taskRecalcDistribSumOutSaldo = 309,
            taskUpdateAddress = 310,
            taskCalculateAnalytics = 311,
            taskLoadFileFromSZ = 312,
            taskUnloadFileForSZ = 313,
            ReCalcKomiss = 875,
            AddPrimaryKey = 877,
            AddIndexes = 878,
            AddForeignKey = 879,
        }
    }

    // Decompiled with JetBrains decompiler
    // Type: STCLINE.KP50.Global.ProcessTypes
    // Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
    // MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
    // Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

    public enum ProcessTypes
    {
        None = 0,
        CalcSaldoUK = 1,
        CalcNach = 2,
        Bill = 3,
        PayDoc = 5,
    }

    // Decompiled with JetBrains decompiler
    // Type: STCLINE.KP50.Interfaces.FonTaskWithYearMonth
    // Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
    // MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
    // Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll

    [DataContract]
    public class FonTaskWithYearMonth : FonTask
    {
        public RecordMonth YM;
        public RecordMonth YM_po;

        [DataMember]
        public int month_
        {
            get
            {
                return this.YM.month_;
            }
            set
            {
                this.YM.month_ = value;
            }
        }

        [DataMember]
        public int year_
        {
            get
            {
                return this.YM.year_;
            }
            set
            {
                this.YM.year_ = value;
            }
        }

        [DataMember]
        public string year_month
        {
            get
            {
                return this.YM.name;
            }
        }

        [DataMember]
        public int month_po
        {
            get
            {
                return this.YM_po.month_;
            }
            set
            {
                this.YM_po.month_ = value;
            }
        }

        [DataMember]
        public int year_po
        {
            get
            {
                return this.YM_po.year_;
            }
            set
            {
                this.YM_po.year_ = value;
            }
        }

        public FonTaskWithYearMonth()
        {
            this.YM = Points.CalcMonth;
            this.YM_po = Points.CalcMonth;
        }
    }

    // Decompiled with JetBrains decompiler
    // Type: STCLINE.KP50.Interfaces.FonTask
    // Assembly: KP50.Globals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
    // MVID: 090DFEBB-F431-4179-80BA-946CB69C8BD3
    // Assembly location: C:\Repos\gkh\packages\Bars.KP60.Host\lib\net40\KP50.Globals.dll
    [DataContract]
    public class FonTask : Finder
    {
        protected FonTask.Statuses status;
        protected int kod_info;
        public string dat_when;

        [DataMember]
        public int num { get; set; }

        [DataMember]
        public int KodInfo
        {
            get
            {
                return this.kod_info;
            }
            set
            {
                this.kod_info = value;
                this.status = FonTask.GetStatusById(value);
            }
        }

        [DataMember]
        public int nzp_key { get; set; }

        [DataMember]
        public FonTask.Statuses Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                this.kod_info = (int)this.status;
            }
        }

        [DataMember]
        public virtual string StatusName
        {
            get
            {
                if (this.status == FonTask.Statuses.InProcess)
                    return FonTask.GetStatusName(this.status) + " (" + this.progress.ToString("P") + ")";
                return FonTask.GetStatusName(this.status);
            }
        }

        [DataMember]
        public string dat_in { get; set; }

        [DataMember]
        public string dat_in_po { get; set; }

        [DataMember]
        public string dat_work { get; set; }

        [DataMember]
        public string dat_out { get; set; }

        [DataMember]
        public string txt { get; set; }

        public string prms { get; set; }

        [DataMember]
        public string processName
        {
            get
            {
                return "Фоновый процесс от " + this.dat_in;
            }
        }

        [DataMember]
        public string processType
        {
            get
            {
                return "Фоновый процесс";
            }
        }

        [DataMember]
        public ProcessTypes processTypeID
        {
            get
            {
                return ProcessTypes.None;
            }
        }

        [DataMember]
        public Decimal progress { get; set; }

        public FonTask()
        {
            this.num = 0;
            this.nzp_key = 0;
            this.kod_info = -999987654;
            this.dat_in = "";
            this.dat_in_po = "";
            this.dat_work = "";
            this.dat_out = "";
            this.txt = "";
            this.prms = "";
            this.progress = new Decimal(0);
            this.dat_when = "";
        }

        public static FonTask.Statuses GetStatusById(int id)
        {
            if (id == 3)
                return FonTask.Statuses.InQueue;
            if (id == 0)
                return FonTask.Statuses.InProcess;
            if (id == 2)
                return FonTask.Statuses.Completed;
            if (id == -1)
                return FonTask.Statuses.Failed;
            return id == 1 ? FonTask.Statuses.New : FonTask.Statuses.None;
        }

        public static string GetStatusName(FonTask.Statuses status)
        {
            switch (status)
            {
                case FonTask.Statuses.Failed:
                    return "Ошибка";
                case FonTask.Statuses.InProcess:
                    return "Выполняется";
                case FonTask.Statuses.New:
                    return "Новое";
                case FonTask.Statuses.Completed:
                    return "Выполнено";
                case FonTask.Statuses.InQueue:
                    return "В очереди";
                default:
                    return "";
            }
        }

        public static string GetStatusNameById(int id)
        {
            return FonTask.GetStatusName(FonTask.GetStatusById(id));
        }

        public void SetStatus(FonTask.Statuses status)
        {
            this.kod_info = (int)status;
        }

        public static int getKodInfo(int nzpAct)
        {
            switch (nzpAct)
            {
                case 532:
                    return 3;
                case 533:
                    return 0;
                case 534:
                    return 2;
                case 535:
                    return -1;
                default:
                    return -999987654;
            }
        }

        public enum Statuses
        {
            None = -10,
            Failed = -1,
            InProcess = 0,
            New = 1,
            Completed = 2,
            InQueue = 3,
        }
    }
}