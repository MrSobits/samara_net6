namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.Gkh.Map;

    public class PreventiveActionMap : GkhJoinedSubClassMap<Entities.PreventiveAction.PreventiveAction>
    {
        /// <inheritdoc />
        public PreventiveActionMap()
            : base("GJI_DOCUMENT_PREVENTIVE_ACTION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.ActionType, "Вид мероприятия").Column("ACTION_TYPE").NotNull();
            Property(x => x.VisitType, "Тип визита").Column("VISIT_TYPE");
            Property(x => x.ControlledPersonType, "Тип контролируемого лица").Column("CONTROLLED_PERSON_TYPE");
            Property(x => x.FullName, "ФИО").Column("FULL_NAME");
            Property(x => x.PhoneNumber, "Телефон").Column("PHONE");
            Property(x => x.FileName, "Наименование файла").Column("FILE_NAME");
            Property(x => x.FileNumber, "Номер файла").Column("FILE_NUMBER");
            Property(x => x.FileDate, "Дата файла").Column("FILE_DATE");
            Property(x => x.ErknmRegistrationNumber, "Учетный номер ПМ в ЕРКНМ").Column("ERKNM_REGISTRATION_NUMBER");
            Property(x => x.ErknmRegistrationDate, "Дата присвоения учетного номера / идентификатора ЕРКНМ").Column("ERKNM_REGISTRATION_DATE");
            Reference(x => x.File, "Файл").Column("FILE_ID");
            Reference(x => x.ControlledPersonAddress, "Адрес нахождения контролируемого лица").Column("CONTROLLED_PERSON_ADDRESS_ID");
            Reference(x => x.Head, "Руководитель").Column("HEAD_ID");
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.ControlledOrganization, "Контролируемое лицо").Column("CONTROLLED_ORGANIZATION_ID").Fetch();
            Reference(x => x.Plan, "План").Column("PLAN_ID").Fetch();
            Reference(x => x.ZonalInspection, "Орган ГЖИ").Column("ZONAL_INSPECTION_ID").NotNull();
            Reference(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE_ID");
        }
    }
}