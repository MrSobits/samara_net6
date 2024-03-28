namespace Bars.GkhCr.Regions.Nso.Navigation
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
            root.Add("Паспорт объекта", "B4.controller.objectcr.Edit").WithIcon("icon-book-addresses");
            root.Add("Договоры", "B4.controller.objectcr.ContractCr")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.ContractCrViewCreate.View")
                .WithIcon("icon-book-edit");
            root.Add("Протоколы, акты", "B4.controller.objectcr.Protocol")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.Protocol.View")
                .WithIcon("icon-page-white-edit");
            root.Add("Средства по источникам финансирования", "B4.controller.objectcr.FinanceSourceRes")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.FinanceSourceRes.View")
                .WithIcon("icon-money");
            root.Add("Лицевые счета", "B4.controller.objectcr.PersonalAccount")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.PersonalAccount.View")
                .WithIcon("icon-table-key");
            root.Add("Виды работ", "B4.controller.objectcr.TypeWorkCr")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.TypeWork.View")
                .WithIcon("icon-wrench-orange");
            root.Add("Дефектные ведомости", "B4.controller.objectcr.DefectList")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.DefectListViewCreate.View")
                .WithIcon("icon-page-white-powerpoint");
            root.Add("Квалификационный отбор", "B4.controller.objectcr.Qualification")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.Qualification.View")
                .WithIcon("icon-medal-bronze-1");
            root.Add("Конкурсы", "B4.controller.objectcr.Competition")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.Competition.View")
                .WithIcon("icon-medal-bronze-1");
            root.Add("Договоры подряда", "B4.controller.objectcr.BuildContract")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.BuildContractViewCreate.View")
                .WithIcon("icon-page-white-paste-table");
            root.Add("Мониторинг СМР", "B4.controller.objectcr.MonitoringSmr")
                .AddRequiredPermission("GkhCr.ObjectCr.Register.MonitoringSmr.View")
                .WithIcon("icon-chart-bar");

            if (root.Items.All(x => x.Caption != "Акты выполненных работ"))
            {
                root.Add("Акты выполненных работ", "B4.controller.objectcr.PerformedWorkActRo")
                    .AddRequiredPermission("GkhCr.ObjectCr.Register.PerformedWorkActViewCreate.View")
                    .WithIcon("icon-script-edit");
            }
        }
    }
}
