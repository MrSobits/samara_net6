namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг <see cref="AppealCitsTypeOfFeedback"/>
    /// </summary>
    public class AppealCitsTypeOfFeedbackMap : BaseEntityMap<AppealCitsTypeOfFeedback>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AppealCitsTypeOfFeedbackMap() 
            : base("Bars.GkhGji.Entities.AppealCitsTypeOfFeedback", "CIT_TYPE_OF_FEEDBACK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.AppealCits, "Обращение ГЖИ").Column("APPCIT_ID").NotNull();
            this.Reference(x => x.TypeOfFeedback, "Тип обратной связи").Column("CIT_TOF_ID").NotNull();
            this.Reference(x => x.FileInfo, "Файл").Column("FILE_TOF_ID").Fetch();

        }
    }
}