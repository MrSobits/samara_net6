namespace Bars.GkhGji
{
    using Entities;
    using Gkh.ImportExport;

    public class EntityExportProvider : IEntityExportProvider
    {
        public void FillContainer(EntityExportContainer container)
        {
            container.Add(typeof(ViolationGji), "Нарушение ГЖИ");
            container.Add(typeof(ViolationFeatureGji), "Нарушения - Характеристика нарушения ГЖИ");
            container.Add(typeof(InspectedPartGji), "Инспектируемая часть ГЖИ");
            container.Add(typeof(ArticleLawGji), "Статьи закона ГЖИ");
            container.Add(typeof(ExecutantDocGji), "Типы исполнителей ГЖИ");
            container.Add(typeof(TypeSurveyGji), "Типы обследований ГЖИ");
            container.Add(typeof(TypeCourtGji), "Виды судов ГЖИ");
            container.Add(typeof(SanctionGji), "Виды санкций ГЖИ");
            container.Add(typeof(CourtVerdictGji), "Решения суда ГЖИ");
            container.Add(typeof(InstanceGji), "Инстанции ГЖИ");
            container.Add(typeof(StatSubjectGji), "Тематики обращений ГЖИ");
            container.Add(typeof(StatSubsubjectGji), "Подтематики ГЖИ");
            container.Add(typeof(ResolveGji), "Обращения - резолюции");
            container.Add(typeof(KindProtocolTsj), "Деятельность ТСЖ - тип протокола");
            container.Add(typeof(ArticleTsj), "Деятельность ТСЖ - статьи устава");
            container.Add(typeof(RedtapeFlagGji), "ГЖИ - признак волокиты");
        }
    }
}