namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Определение постановления ГЖИ"</summary>
    public class AppealCitsDefinitionLongTextMap : BaseEntityMap<AppealCitsDefinitionLongText>
    {
        
        public AppealCitsDefinitionLongTextMap() : 
                base("Определение постановления ГЖИ", "GJI_APPCIT_DEFINITION_LT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decided, "Дата документа").Column("DECIDED");
            Property(x => x.Established, "Дата документа").Column("ESTABLISHED");
            Reference(x => x.AppealCitsDefinition, "ДЛ, вынесшее определение").Column("DEFINITION_ID").NotNull();
        }
    }
    public class AppealCitsDefinitionLongTextNHibernateMapping : ClassMapping<AppealCitsDefinitionLongText>
    {
        public AppealCitsDefinitionLongTextNHibernateMapping()
        {
            this.Property(
                x => x.Decided,
                mapper =>
                {
                    mapper.Type<BinaryBlobType>();
                });
            this.Property(
                x => x.Established,
                mapper =>
                {
                    mapper.Type<BinaryBlobType>();
                });            
        }
    }
}
