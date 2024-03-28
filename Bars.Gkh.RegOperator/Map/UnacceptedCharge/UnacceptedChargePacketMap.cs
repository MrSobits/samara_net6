/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.UnacceptedCharge
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class UnacceptedChargePacketMap : BaseImportableEntityMap<UnacceptedChargePacket>
///     {
///         public UnacceptedChargePacketMap() : base("REGOP_UNACCEPT_C_PACKET")
///         {
///             Map(x => x.CreateDate, "CCREATE_DATE", true);
///             Map(x => x.Description, "CDESCRIPTION", false, 1000);
///             Map(x => x.PacketState, "PACKET_STATE", true);
///             Map(x => x.UserName, "USER_NAME", false, 100);
/// 
///             Bag(x => x.Charges, m =>
///             {
///                 m.Access(Accessor.NoSetter);
///                 m.Key(k => k.Column("PACKET_ID"));
///                 m.Lazy(CollectionLazy.Lazy);
///                 m.Fetch(CollectionFetchMode.Select);
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Пакет неподтвержденных начислений"</summary>
    public class UnacceptedChargePacketMap : BaseImportableEntityMap<UnacceptedChargePacket>
    {
        
        public UnacceptedChargePacketMap() : 
                base("Пакет неподтвержденных начислений", "REGOP_UNACCEPT_C_PACKET")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CreateDate, "Дата формирования пакета").Column("CCREATE_DATE").NotNull();
            Property(x => x.Description, "Описание пакета").Column("CDESCRIPTION").Length(1000);
            Property(x => x.PacketState, "Состояние пакета").Column("PACKET_STATE").NotNull();
            Property(x => x.UserName, "Имя (ФИО) пользователя, который произвел расчет").Column("USER_NAME").Length(100);
        }
    }

    public class UnacceptedChargePacketNHibernateMapping : ClassMapping<UnacceptedChargePacket>
    {
        public UnacceptedChargePacketNHibernateMapping()
        {
            Bag(
                x => x.Charges,
                m =>
                    {
                        m.Access(Accessor.NoSetter);
                        m.Key(k => k.Column("PACKET_ID"));
                        m.Lazy(CollectionLazy.Lazy);
                        m.Fetch(CollectionFetchMode.Select);
                    },
                action => action.OneToMany());
        }
    }
}
