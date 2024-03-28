namespace Bars.GkhCr.Navigation
{
    using Bars.B4;

    public class SpecialMonitoringSmrMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "SpecialMonitoringSmr";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки Мониторига СМР для владельцев специальных счетов";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Мониторинг СМР").Add("График выполнения работ", "specialobjectcredit/{0}/scheduleexecutionwork")
                .AddRequiredPermission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.View").WithIcon("icon-text-list-numbers");
            root.Add("Мониторинг СМР").Add("Ход выполнения работ", "specialobjectcredit/{0}/progressexecutionwork")
                .AddRequiredPermission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.View").WithIcon("icon-chart-curve");
            root.Add("Мониторинг СМР").Add("Численность рабочих", "specialobjectcredit/{0}/workerscountwork")
                .AddRequiredPermission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.WorkersCount.View");
            root.Add("Мониторинг СМР").Add("Документы", "specialobjectcredit/{0}/documentworkcr")
                .AddRequiredPermission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.Document.View").WithIcon("icon-paste-plain");
        }
    }
}