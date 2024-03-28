namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "Протокол(акт)"</summary>
    public class SpecialProtocolCrMap : BaseImportableEntityMap<SpecialProtocolCr>
    {
        public SpecialProtocolCrMap() : 
                base("Протокол(акт)", "CR_SPECIAL_OBJ_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            this.Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            this.Property(x => x.CountAccept, "Доля принявших участие на кв. м.").Column("COUNT_ACCEPT");
            this.Property(x => x.CountVote, "Количество голосов на кв. м.").Column("COUNT_VOTE");
            this.Property(x => x.CountVoteGeneral, "Количество голосов общее на кв. м.").Column("COUNT_VOTE_GENERAL");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(3000);
            this.Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            this.Property(x => x.GradeOccupant, "Оценка жителей").Column("GRADE_OCCUPANT");
            this.Property(x => x.GradeClient, "Оценка заказчика").Column("GRADE_CLIENT");
            this.Property(x => x.SumActVerificationOfCosts, "Сумма Акта сверки данных о расходах").Column("SUM_ACT_VER_OF_COSTS");
            this.Property(x => x.OwnerName, "Собственник, участвующий в приемке работ").Column("OWNER_NAME").Length(300);
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
            this.Property(x => x.DecisionOms, "Решение ОМС").Column("DECISION_OMS");

            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.Contragent, "Участник").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.TypeDocumentCr, "Тип документа").Column("TYPE_DOCUMENT_CR_ID").Fetch();
            this.Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
