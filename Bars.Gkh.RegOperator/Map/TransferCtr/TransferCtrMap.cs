/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Заявка на перечисление средств подрядчикам"
///     /// </summary>
///     public class TransferCtrMap : BaseImportableEntityMap<TransferCtr>
///     {
///         public TransferCtrMap() : base("RF_TRANSFER_CTR")
///         {
///             Map(x => x.DocumentName, "DOCUMENT_NAME", false, 300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.Perfomer, "PERFOMER_NAME", false, 300);
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.PaymentType, "TYPE_PAYMENT", true);
///             Map(x => x.TypeProgramRequest, "TYPE_PROGRAM_REQUEST", true);
///             Map(x => x.PaymentPurposeDescription, "PAY_PURPOSE_DESCR");
///             Map(x => x.IsExport, "IS_EXPORT");
///             Map(x => x.Comment, "COMMENT", false, 1000);
///             Map(x => x.KindPayment, "KIND_PAYMENT", true);
///             Map(x => x.PaidSum, "PAID_SUM", true, 0m);
///             Map(x => x.Sum, "CSUM", true, 0m);
///             Map(x => x.TransferGuid, "TRANSFER_GUID", true);
///             Map(x => x.PaymentDate, "PAYMENT_DATE");
///             Map(x => x.PaymentPriority, "PAYMENT_PRIORITY", false);
///             Map(x => x.IsEditPurpose, "IS_EDIT_PURPOSE", true, false);
///             Map(x => x.TypeCalculationNds, "TYPE_CALC_NDS", true, 0);
/// 
///             References(x => x.FinSource, "FIN_SOURCE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Contract, "BUILD_CONTRACT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.ObjectCr, "OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Builder, "BUILDER_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.ProgramCr, "PROGRAM_CR_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ContragentBank, "CONTRAGENT_BANK_ID", ReferenceMapConfig.Fetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.TypeWorkCr, "TYPE_WORK_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RegOperator, "REG_OPER_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RegopCalcAccount, "CALC_ACCOUNT_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ProgramCrType, "PROG_CR_TYPE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Document, "DOC_FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Заявка на перечисление средств подрядчикам"</summary>
    public class TransferCtrMap : BaseImportableEntityMap<TransferCtr>
    {
        
        public TransferCtrMap() : 
                base("Заявка на перечисление средств подрядчикам", "RF_TRANSFER_CTR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeProgramRequest, "Тип программы заявки перечисления рег.фонда").Column("TYPE_PROGRAM_REQUEST").NotNull();
            Reference(x => x.ProgramCr, "Программа кап. ремонта").Column("PROGRAM_CR_ID").Fetch();
            Reference(x => x.ObjectCr, "Объект КР").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.ContragentBank, "Банк контрагента").Column("CONTRAGENT_BANK_ID").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Property(x => x.Perfomer, "Исполнитель").Column("PERFOMER_NAME").Length(300);
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "Номер зявки").Column("DOCUMENT_NUM").Length(250);
            Property(x => x.DocumentNumPp, "Номер зявки ПП").Column("DOCUMENT_NUM_PP");
            Property(x => x.DateFromPp, "Дата заявки ПП").Column("DATE_FROM_PP");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.DateFrom, "Дата заявки").Column("DATE_FROM");
            Property(x => x.PaymentType, "Тип платежа").Column("TYPE_PAYMENT").NotNull();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
            Reference(x => x.Contract, "Договор подряда").Column("BUILD_CONTRACT_ID").NotNull().Fetch();
            Reference(x => x.FinSource, "Разрез финансирования").Column("FIN_SOURCE_ID").Fetch();
            Reference(x => x.TypeWorkCr, "Вид работы").Column("TYPE_WORK_ID").Fetch();
            Property(x => x.PaymentPurposeDescription, "Назначение платежа").Column("PAY_PURPOSE_DESCR").Length(250);
            Reference(x => x.RegOperator, "Региональный оператор").Column("REG_OPER_ID").Fetch();
            Reference(x => x.RegopCalcAccount, "Расчетный счет регионального оператора").Column("CALC_ACCOUNT_ID").Fetch();
            Reference(x => x.ProgramCrType, "Типы программы КР").Column("PROG_CR_TYPE_ID").Fetch();
            Property(x => x.Comment, "Комментарий").Column("COMMENT").Length(1000);
            Property(x => x.KindPayment, "Вид платежа").Column("KIND_PAYMENT").NotNull();
            Property(x => x.Sum, "Сумма к оплате").Column("CSUM").DefaultValue(0m).NotNull();
            Property(x => x.PaidSum, "Оплаченная сумма").Column("PAID_SUM").DefaultValue(0m).NotNull();
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");
            Property(x => x.TransferGuid, "Гуид").Column("TRANSFER_GUID").Length(250).NotNull();
            Property(x => x.IsExport, "Сформирован документ").Column("IS_EXPORT");
            Property(x => x.PaymentPriority, "Очередность платежа").Column("PAYMENT_PRIORITY").Length(250);
            Property(x => x.IsEditPurpose, "Редактировать назначение платежа").Column("IS_EDIT_PURPOSE").DefaultValue(false).NotNull();
            Reference(x => x.Document, "Документ").Column("DOC_FILE_ID").Fetch();
            Property(x => x.TypeCalculationNds, "Вид расчета НДС").Column("TYPE_CALC_NDS").NotNull();
        }
    }
}
