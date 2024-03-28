namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Источник поступления обращения"</summary>
    public class AppealCitsInfoMap : BaseEntityMap<AppealCitsInfo>
    {

        public AppealCitsInfoMap() :
                base("Журнал учета регистрации обращений", "GJI_APPEAL_CITIZENS_INFO")
        {
        }

        protected override void Map()
        {
            Property(x => x.DocumentNumber, "Номер документа").Column("DOC_NUM");
            Property(x => x.AppealDate, "Дата обращения").Column("APPEAL_DATE");
            Property(x => x.OperationDate, "Дата операции").Column("OPERATION_DATE");
            Property(x => x.Correspondent, "Корреспондент").Column("CORRESPONDENT_NAME").Length(150);
            Property(x => x.OperationType, "Тип операции").Column("OPERATION_TYPE");
            Property(x => x.Operator, "Автор последней операции").Column("OPERATOR");
        }
    }
}
