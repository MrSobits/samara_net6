// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Меню, навигация
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhDi
{
    using Bars.B4;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {
            var disInfo = root.Add("Раскрытие информации").Add("Раскрытие информации по 731 (988) ПП РФ");
            disInfo.AddRequiredPermission("GkhDi.View");
            disInfo.Add("Раскрытие информации по 731 (988) ПП РФ", "B4.controller.DisclosureInfo").WithIcon("diIcon");
            disInfo.Add("Массовый расчет процентов", "masspercentcalculation").WithIcon("diIcon").AddRequiredPermission("GkhDi.MassPercCalc");
            disInfo.Add("Массовая генерация отчета по 731 (988) ПП РФ", "masscalcreport").WithIcon("diIcon").AddRequiredPermission("GkhDi.MassGenerateReport");

            var dict = root.Add("Справочники").Add("Раскрытие информации");
            dict.Add("Отчетные периоды", "perioddi").AddRequiredPermission("GkhDi.Dict.PeriodDi.View");
            dict.Add("Услуги", "templateservice").AddRequiredPermission("GkhDi.Dict.TemplateService.View");
            dict.Add("Периодичность услуг", "periodicitytempserv").AddRequiredPermission("GkhDi.Dict.PeriodicityTempServ.View");
            dict.Add("Контролирующие органы", "supervisoryorg").AddRequiredPermission("GkhDi.Dict.SupervisoryOrg.View");
            dict.Add("Система налогооблажения", "taxsystem").AddRequiredPermission("GkhDi.Dict.TaxSystem.View");
            dict.Add("Группы работ по ТО", "groupworkto").AddRequiredPermission("GkhDi.Dict.GroupWorkTo.View");
            dict.Add("Группы ППР", "groupworkppr").AddRequiredPermission("GkhDi.Dict.GroupPpr.View");
            dict.Add("Планово-предупредительные работы", "dictworkppr").AddRequiredPermission("GkhDi.Dict.Ppr.View");
            dict.Add("Работы по ТО", "dictworkto").AddRequiredPermission("GkhDi.Dict.WorkTo.View");
            dict.Add("Прочие услуги из сторонних систем", "templateotherservice").AddRequiredPermission("GkhDi.Dict.TemplateOtherService.View");
        }
    }
}