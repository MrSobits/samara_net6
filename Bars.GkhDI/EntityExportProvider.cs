namespace Bars.GkhDi
{
    using Entities;
    using Gkh.ImportExport;

    public class EntityExportProvider : IEntityExportProvider
    {
        public void FillContainer(EntityExportContainer container)
        {
            container.Add(typeof(PeriodicityTemplateService), "(Раскрытие информации)Периодичность услуг");
            container.Add(typeof(TaxSystem), "(Раскрытие информации)Система налогообложения");
            container.Add(typeof(PeriodDi), "(Раскрытие информации)Отчетные периоды");
            container.Add(typeof(TemplateService), "(Раскрытие информации)Услуги");
            container.Add(typeof(SupervisoryOrg), "(Раскрытие информации)Контролирующие органы");
            container.Add(typeof(GroupWorkTo), "(Раскрытие информации)Группы работ по ТО");
            container.Add(typeof(GroupWorkPpr), "(Раскрытие информации)Группы ППР");
            container.Add(typeof(WorkTo), "(Раскрытие информации)Планово-предупредительные работы");
            container.Add(typeof(WorkPpr), "(Раскрытие информации)Работы по ТО");
        }
    }
}