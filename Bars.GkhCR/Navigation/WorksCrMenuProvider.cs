namespace Bars.GkhCr.Navigation
{
    using B4;

    /// <summary>
    /// 
    /// </summary>
    public class WorksCrMenuProvider : INavigationProvider
    {
        /// <summary>
        /// Ключ меню
        /// </summary>
        public string Key
        {
            get { return "WorksCr"; }
        }

        /// <summary>
        /// Описание меню
        /// </summary>
        public string Description
        {
            get { return ""; }
        }

        /// <summary>
        /// Метод инициализации
        /// </summary>
        public void Init(MenuItem root)
        {
            root.Add("Паспорт объекта", "workscredit/{0}/{1}/edit");

            root.Add("Обследование объекта", "workscredit/{0}/{1}/inspection")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.Inspection.View");

            root.Add("Договоры", "workscredit/{0}/{1}/contract")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.ContractCrViewCreate.View");

            root.Add("Протоколы, акты", "workscredit/{0}/{1}/protocol")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.Protocol.View");

            root.Add("Дефектные ведомости", "workscredit/{0}/{1}/defectlist")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.DefectListViewCreate.View");

            root.Add("Средства по источникам финансирования", "workscredit/{0}/{1}/finsources")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.FinanceSourceRes.View");

            root.Add("Сметный расчет по работе", "workscredit/{0}/{1}/estimate")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.EstimateCalculationViewCreate.View");

            root.Add("Договоры подряда", "workscredit/{0}/{1}/buildcontract")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.BuildContractViewCreate.View");

            root.Add("Мониторинг СМР", "workscredit/{0}/{1}/smr").AddRequiredPermission("GkhCr.TypeWorkCr.Register.MonitoringSmr.View");

            root.Add("Мониторинг СМР")
                .Add("График выполнения", "workscredit/{0}/{1}/scheduleexecution")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.View");

            root.Add("Мониторинг СМР").Add("Ход выполнения работ", "workscredit/{0}/{1}/progressexecution")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.View");

            root.Add("Мониторинг СМР").Add("Численность рабочих", "workscredit/{0}/{1}/workerscount")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.MonitoringSmr.WorkersCount.View");

            root.Add("Мониторинг СМР").Add("Документы", "workscredit/{0}/{1}/document")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.MonitoringSmr.Document.View");

            root.Add("Акты выполненных работ", "workscredit/{0}/{1}/perfact")
                .AddRequiredPermission("GkhCr.TypeWorkCr.Register.PerformedWorkActViewCreate.View");
        }
    }
}