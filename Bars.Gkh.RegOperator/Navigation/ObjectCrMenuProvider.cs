namespace Bars.Gkh.RegOperator.Navigation
{
    using System.Linq;

    using Bars.B4;

    public class ObjectCrMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "ObjectCr";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки объекта КР";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Паспорт объекта", "objectcredit/{0}/edit").WithIcon("icon-book-addresses");
            root.Add("Договоры", "objectcredit/{0}/contractcr").AddRequiredPermission("GkhCr.ObjectCr.Register.ContractCrViewCreate.View").WithIcon("icon-book-edit");
            root.Add("Протоколы, акты", "objectcredit/{0}/protocol").AddRequiredPermission("GkhCr.ObjectCr.Register.Protocol.View").WithIcon("icon-page-white-edit");
            root.Add("Дефектные ведомости", "objectcredit/{0}/defectlist").AddRequiredPermission("GkhCr.ObjectCr.Register.DefectListViewCreate.View").WithIcon("icon-page-white-powerpoint");
            root.Add("Отчеты старших по дому", "objectcredit/{0}/housekeeper").AddRequiredPermission("GkhCr.ObjectCr.Register.HousekeeperReport.View").WithIcon("icon-page-white-powerpoint");
            root.Add("Виды работ", "objectcredit/{0}/typeworkcr").AddRequiredPermission("GkhCr.ObjectCr.Register.TypeWork.View").WithIcon("icon-wrench-orange");
            root.Add("Средства по источникам финансирования", "objectcredit/{0}/financesourceres").AddRequiredPermission("GkhCr.ObjectCr.Register.FinanceSourceRes.View").WithIcon("icon-money");
            root.Add("Лицевые счета", "objectcredit/{0}/personalaccount").AddRequiredPermission("GkhCr.ObjectCr.Register.PersonalAccount.View").WithIcon("icon-table-key");
            root.Add("Сметный расчет по работе", "objectcredit/{0}/estimatecalculation").AddRequiredPermission("GkhCr.ObjectCr.Register.EstimateCalculationViewCreate.View").WithIcon("icon-calculator");
            root.Add("Квалификационный отбор", "objectcredit/{0}/qualification").AddRequiredPermission("GkhCr.ObjectCr.Register.Qualification.View").WithIcon("icon-medal-bronze-1");
            root.Add("Конкурсы", "objectcredit/{0}/competition").AddRequiredPermission("GkhCr.ObjectCr.Register.Competition.View").WithIcon("icon-medal-bronze-1");
            root.Add("Мониторинг СМР", "objectcredit/{0}/monitoringsmr").AddRequiredPermission("GkhCr.ObjectCr.Register.MonitoringSmr.View").WithIcon("icon-chart-bar");
            root.Add("Акты выполненных работ", "objectcredit/{0}/performedworkact").AddRequiredPermission("GkhCr.ObjectCr.Register.PerformedWorkActViewCreate.View").WithIcon("icon-script-edit");
        }
    }
}