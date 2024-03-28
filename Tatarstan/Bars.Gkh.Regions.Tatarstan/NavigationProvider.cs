namespace Bars.Gkh.Regions.Tatarstan
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public string Key => MainNavigationInfo.MenuName;

        public string Description => MainNavigationInfo.MenuDescription;

        public void Init(MenuItem root)
        {
            root.Add("Жилищный фонд")
                .Add("Объекты жилищного фонда")
                .Add("Настройка добавления домов", "roletypehousepermission")
                .AddRequiredPermission("Gkh.RealityObject.RoleTypeHousePermission.View");
                
            root.Add("Жилищный фонд")
                .Add("Аварийность")
                .Add("Объекты строительства", "constructionobject")
                .AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.View")
                .WithIcon("constructionObject");
            root.Add("Жилищный фонд").Add("Объекты жилищного фонда").Add("Нормативы потребления", "normconsumption").AddRequiredPermission("Gkh.NormConsumption.View");

            root.Add("Жилищный фонд")
                .Add("Аварийность")
                .Add("Массовая смена статусов объектов строительства", "constructionobjectmasschangestate")
                .AddRequiredPermission("Gkh.ConstructionObjectMassStateChange.View")
                .WithIcon("constructionObjectMassStateChange");

            root.Add("Справочники")
                .Add("Общие")
                .Add("Отчетные периоды нормативов потребления", "periodnormconsumption")
                .AddRequiredPermission("Gkh.Dictionaries.PeriodNormConsumption.View")
                .WithIcon("PeriodNormConsumption");

            root.Add("Жилищный фонд")
                .Add("Объекты жилищного фонда")
                .Add("Расщепление платежей", "chargessplitting")
                .AddRequiredPermission("Gkh.ChargesSplitting.View");

            var gkhDictsRoot = root.Add("Справочники").Add("Жилищно-коммунальное хозяйство");
            gkhDictsRoot
                .Add("Процент планируемых оплат", "planpaymentspercentage")
                .AddRequiredPermission("Gkh.Dictionaries.PlanPaymentsPercentage.View")
                .WithIcon("PeriodNormConsumption");       
            gkhDictsRoot
                .Add("Коммунальный ресурс", "communalresource")
                .AddRequiredPermission("Gkh.Dictionaries.CommunalResource.View");
            gkhDictsRoot
                .Add("Услуги по договорам управления", "managementcontractservice")
                .AddRequiredPermission("Gkh.Dictionaries.ManagementContractService.View");
            gkhDictsRoot
                .Add("Вид потребителя", "typecustomer")
                .AddRequiredPermission("Gkh1468.Dictionaries.TypeCustomer.View");
            gkhDictsRoot
                .Add("Коммунальные услуги", "publicservice")
                .AddRequiredPermission("Gkh1468.Dictionaries.PublicService.View");

            var efRatingDict = root.Add("Справочники").Add("Рейтинг эффективности");
            efRatingDict.Add("Периоды рейтинга эффективност", "efratingperiod").AddRequiredPermission("Gkh.Dictionaries.EfficiencyRating.Period.View");

            var efRatingRoot = root.Add("Участники процесса").Add("Рейтинг эффективности УО");
            efRatingRoot.Add("Рейтинг эффективности управляющих организаций", "efmanagingorganization")
                .WithIcon("efRating")
                .AddRequiredPermission("Gkh.Orgs.EfficiencyRating.ManagingOrganization.View");

            efRatingRoot.Add("Аналитические показатели", "efanalitics")
                .WithIcon("analitics")
                .AddRequiredPermission("Gkh.Orgs.EfficiencyRating.Analitics.View");

            efRatingRoot.Add("Редактор рейтинга эффективности", "efficiencyratingconstructor")
                .WithIcon("constructor")
                .AddRequiredPermission("Gkh.Orgs.EfficiencyRating.Constructor.View");

            root.Add("Участники процесса").Add("Роли контрагента").Add("ВДГО", "gasequipmentorg").AddRequiredPermission("Gkh.Orgs.GasEquipmentOrg.View");

            root.Add("Администрирование")
                .Add("Интеграция с внешними системами")
                .Add("Интеграция с ЕГСО ОВ", "egsointegration")
                .AddRequiredPermission("Administration.OutsideSystemIntegrations.EgsoIntegration.View");

            root.Add("Администрирование")
                .Add("Логи")
                .Add("Логи интеграции данных по тарифам", "tariffdataintegrationlog")
                .AddRequiredPermission("Administration.TariffDataIntegrationLog.View");

            var fssp = root.Add("Претензионная работа")
                    .Add("ФССП");
                
                fssp.Add("Реестр судебных распоряжений по ЖКУ", "courtordergku")
                    .WithIcon("courtordergku")
                    .AddRequiredPermission("Clw.Fssp.CourtOrderGku.View");
                
                fssp.Add("Оплаты по ЖКУ", "paymentgku")
                    .WithIcon("payment")
                    .AddRequiredPermission("Clw.Fssp.PaymentGku.View");
        }
    }
}