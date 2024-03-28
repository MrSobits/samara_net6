/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
/// 
///     public class UnacceptedPaymentMap : BaseImportableEntityMap<Entities.UnacceptedPayment>
///     {
///         public UnacceptedPaymentMap()
///             : base("REGOP_UNACCEPT_PAY")
///         {
///             Map(x => x.Accepted, "ACCEPTED", true);
///             Map(x => x.Guid, "PGUID", true, 40);
///             Map(x => x.PaymentDate, "PAYMENT_DATE", true);
///             Map(x => x.Sum, "PAYMENT_SUM", true);
///             Map(x => x.PenaltySum, "PENALTY_SUM");
///             Map(x => x.PaymentType, "PAYMENT_TYPE", true);
///             Map(x => x.DocNumber, "DOC_NUMBER");
///             Map(x => x.DocDate, "DOC_DATE");
///             Map(x => x.TransferGuid, "TRANSFER_GUID");
/// 
///             References(x => x.Packet, "PACKET_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.PersonalAccount, "ACC_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Неподтвержденная оплата"</summary>
    public class UnacceptedPaymentMap : BaseImportableEntityMap<UnacceptedPayment>
    {
        
        public UnacceptedPaymentMap() : 
                base("Неподтвержденная оплата", "REGOP_UNACCEPT_PAY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Packet, "Ссылка на пакет неподтвержденных оплат").Column("PACKET_ID").NotNull().Fetch();
            Reference(x => x.PersonalAccount, "Л/С").Column("ACC_ID").NotNull().Fetch();
            Property(x => x.Sum, "Сумма оплаты").Column("PAYMENT_SUM").NotNull();
            Property(x => x.PenaltySum, "Сумма оплаты пени").Column("PENALTY_SUM");
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE").NotNull();
            Property(x => x.PaymentType, "Тип оплаты").Column("PAYMENT_TYPE").NotNull();
            Property(x => x.Guid, "Guid оплаты").Column("PGUID").Length(40).NotNull();
            Property(x => x.Accepted, "Подтверждено").Column("ACCEPTED").NotNull();
            Property(x => x.DocNumber, "Номер документа").Column("DOC_NUMBER").Length(250);
            Property(x => x.DocDate, "Дата документа").Column("DOC_DATE");
            Property(x => x.TransferGuid, "Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer").Column("TRANSFER_GUID").Length(250);
        }
    }
}
