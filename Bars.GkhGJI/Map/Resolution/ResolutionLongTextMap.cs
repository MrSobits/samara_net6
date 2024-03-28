namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Определение постановления ГЖИ"</summary>
    public class ResolutionLongTextMap : BaseEntityMap<ResolutionLongText>
    {
        
        public ResolutionLongTextMap() : 
                base("Определение постановления ГЖИ", "GJI_RESOLUTION_LT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Established, "Дата документа").Column("ESTABLISHED");
            Reference(x => x.Resolution, "ДЛ, вынесшее определение").Column("RESOLUTION_ID").NotNull();
        }
    }
    public class ResolutionLongTextNHibernateMapping : ClassMapping<ResolutionLongText>
    {
        public ResolutionLongTextNHibernateMapping()
        {    
            this.Property(
                x => x.Established,
                mapper =>
                {
                    mapper.Type<BinaryBlobType>();
                });            
        }
    }
}
