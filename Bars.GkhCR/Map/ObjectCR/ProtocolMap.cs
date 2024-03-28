/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Gkh.Map;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протокол(акт)"
///     /// </summary>
///     public class ProtocolCrMap : BaseGkhEntityByCodeMap<ProtocolCr>
///     {
///         public ProtocolCrMap() : base("CR_OBJ_PROTOCOL")
///         {
/// 			Map(x => x.DocumentName, "DOCUMENT_NAME", false, 300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM", false, 300);
///             Map(x => x.CountAccept, "COUNT_ACCEPT");
///             Map(x => x.CountVote, "COUNT_VOTE");
///             Map(x => x.CountVoteGeneral, "COUNT_VOTE_GENERAL");
///             Map(x => x.Description, "DESCRIPTION", false, 3000);
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.GradeOccupant, "GRADE_OCCUPANT");
///             Map(x => x.GradeClient, "GRADE_CLIENT");
///             Map(x => x.SumActVerificationOfCosts, "SUM_ACT_VER_OF_COSTS");
///             Map(x => x.OwnerName, "OWNER_NAME", false, 300);
/// 
///             References(x => x.ObjectCr, "OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.Fetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.TypeWork, "TYPE_WORK_ID", ReferenceMapConfig.Fetch);
/// 			References(x => x.TypeDocumentCr, "TYPE_DOCUMENT_CR_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Протокол(акт)"</summary>
    public class ProtocolCrMap : BaseImportableEntityMap<ProtocolCr>
    {
        
        public ProtocolCrMap() : 
                base("Протокол(акт)", "CR_OBJ_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Участник").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.TypeDocumentCr, "Тип документа").Column("TYPE_DOCUMENT_CR_ID").Fetch();
            Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.CountAccept, "Доля принявших участие на кв. м.").Column("COUNT_ACCEPT");
            Property(x => x.CountVote, "Количество голосов на кв. м.").Column("COUNT_VOTE");
            Property(x => x.CountVoteGeneral, "Количество голосов общее на кв. м.").Column("COUNT_VOTE_GENERAL");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(3000);
            Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            Property(x => x.GradeOccupant, "Оценка жителей").Column("GRADE_OCCUPANT");
            Property(x => x.GradeClient, "Оценка заказчика").Column("GRADE_CLIENT");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.SumActVerificationOfCosts, "Сумма Акта сверки данных о расходах").Column("SUM_ACT_VER_OF_COSTS");
            Property(x => x.OwnerName, "Собственник, участвующий в приемке работ").Column("OWNER_NAME").Length(300);
            Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
            Property(x => x.DecisionOms, "Решение ОМС").Column("DECISION_OMS");
        }
    }
}
