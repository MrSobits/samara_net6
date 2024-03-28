namespace Bars.Gkh
{
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Bars.Gkh.ImportExport;
    using Entities.Dicts;

    public class EntityExportProvider : IEntityExportProvider
    {
        public void FillContainer(EntityExportContainer container)
        {
            container.Add(typeof(Role), "Роли", true);
            container.Add(typeof(RolePermission), "Права по ролям", true);
            container.Add(typeof(MultipurposeGlossaryItem), "Универсальные справочники");

            container.Add(typeof(KindRisk), "Виды рисков");
            container.Add(typeof(UnitMeasure), "Единица измерения");
            container.Add(typeof(Specialty), "Специальность");
            container.Add(typeof(OrganizationForm), "Организационная форма");
            container.Add(typeof(Position), "Должности");
            container.Add(typeof(CapitalGroup), "Группы капитальности");
            container.Add(typeof(ConstructiveElementGroup), "Группы конструктивных элементов");
            container.Add(typeof(ConstructiveElement), "Конструктивные элементы");
            container.Add(typeof(MeteringDevice), "Приборы учета");
            container.Add(typeof(TypeOwnership), "Формы собственности");
            container.Add(typeof(TypeProject), "Типы проекта");
            container.Add(typeof(NormativeDoc), "Нормативные документы");
            container.Add(typeof(Work), "Виды работ");
            container.Add(typeof(RoofingMaterial), "Материалы кровли");
            container.Add(typeof(WallMaterial), "Материалы стен");
            container.Add(typeof(WorkKindCurrentRepair), "Виды работ текущего ремонта");
            container.Add(typeof(TypeService), "Типы обслуживания");
        }
    }
}