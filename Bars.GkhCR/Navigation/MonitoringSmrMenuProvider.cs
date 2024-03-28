namespace Bars.GkhCr.Navigation
{
    using Bars.B4;

    public class MonitoringSmrMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "MonitoringSmr";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки Мониторига СМР";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Мониторинг СМР").Add("График выполнения работ", "objectcredit/{0}/scheduleexecutionwork")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.View").WithIcon("icon-text-list-numbers");
            root.Add("Мониторинг СМР").Add("Ход выполнения работ", "objectcredit/{0}/progressexecutionwork")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.View").WithIcon("icon-chart-curve");
            root.Add("Мониторинг СМР").Add("Численность рабочих", "objectcredit/{0}/workerscountwork")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.MonitoringSmr.WorkersCount.View");
            root.Add("Мониторинг СМР").Add("Документы", "objectcredit/{0}/documentworkcr")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.MonitoringSmr.Document.View").WithIcon("icon-paste-plain");
        }
    }
}