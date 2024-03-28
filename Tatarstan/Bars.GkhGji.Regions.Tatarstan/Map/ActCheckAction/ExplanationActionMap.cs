namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.ExplanationAction;

    public class ExplanationActionMap : JoinedSubClassMap<ExplanationAction>
    {
        public ExplanationActionMap()
            : base("Действие акта проверки с типом \"Получение письменных объяснений\"", "GJI_ACTCHECK_EXPLANATION_ACTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ContrPersType, "Тип контролируемого лица").Column("CONTR_PERS_TYPE");
            this.Reference(x => x.ContrPersContragent, "Контрагент контролируемого лица").Column("CONTR_PERS_CONTRAGENT_ID");
            this.Property(x => x.AttachmentName, "Наименование приложения").Column("ATTACHMENT_NAME");
            this.Reference(x => x.AttachmentFile, "Файл приложения").Column("ATTACHMENT_FILE_ID");
            this.Property(x => x.Explanation, "Объяснение").Column("EXPLANATION");
        }
    }
}