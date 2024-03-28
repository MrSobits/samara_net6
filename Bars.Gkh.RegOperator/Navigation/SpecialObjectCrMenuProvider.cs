namespace Bars.Gkh.RegOperator.Navigation
{
    using Bars.B4;

    public class SpecialObjectCrMenuProvider : INavigationProvider
    {
        public string Key => "SpecialObjectCr";

        public string Description => "Меню карточки объекта КР для владельцев специальных счетов";

        public void Init(MenuItem root)
        {
            root.Add("Паспорт объекта", "specialobjectcredit/{0}/edit").WithIcon("icon-book-addresses");
            root.Add("Договоры", "specialobjectcredit/{0}/contractcr").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.ContractCrViewCreate.View").WithIcon("icon-book-edit");
            root.Add("Протоколы, акты", "specialobjectcredit/{0}/protocol").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.Protocol.View").WithIcon("icon-page-white-edit");
            root.Add("Дефектные ведомости", "specialobjectcredit/{0}/defectlist").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.DefectListViewCreate.View").WithIcon("icon-page-white-powerpoint");
            root.Add("Виды работ", "specialobjectcredit/{0}/typeworkcr").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.TypeWork.View").WithIcon("icon-wrench-orange");
            root.Add("Средства по источникам финансирования", "specialobjectcredit/{0}/financesourceres").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.FinanceSourceRes.View").WithIcon("icon-money");
            root.Add("Лицевые счета", "specialobjectcredit/{0}/personalaccount").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.PersonalAccount.View").WithIcon("icon-table-key");
            root.Add("Сметный расчет по работе", "specialobjectcredit/{0}/estimatecalculation").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.EstimateCalculationViewCreate.View").WithIcon("icon-calculator");
            root.Add("Квалификационный отбор", "specialobjectcredit/{0}/qualification").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.Qualification.View").WithIcon("icon-medal-bronze-1");
            root.Add("Конкурсы", "specialobjectcredit/{0}/competition").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.Competition.View").WithIcon("icon-medal-bronze-1");
            root.Add("Мониторинг СМР", "specialobjectcredit/{0}/monitoringsmr").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.View").WithIcon("icon-chart-bar");
            root.Add("Акты выполненных работ", "specialobjectcredit/{0}/performedworkact").AddRequiredPermission("GkhCr.SpecialObjectCr.Register.PerformedWorkActViewCreate.View").WithIcon("icon-script-edit");
        }
    }
}