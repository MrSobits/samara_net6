namespace Bars.GisIntegration.Smev.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Smev.Entity;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Smev3;

    using NHibernate.Mapping.ByCode.Conformist;

    public class StorableSmev3ResponseMap : PersistentObjectMap<StorableSmev3Response>
    {
        /// <inheritdoc />
        public StorableSmev3ResponseMap()
            : base(nameof(StorableSmev3Response), "STORABLE_SMEV3_RESPONSE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.requestGuid, "Идентификатор запроса").Column("REQUEST_GUID");
            Property(x => x.Response, "Объект ответа").Column("RESPONSE");
        }
        
        public class StorableSmev3ResponseMapNHibernateMapping : ClassMapping<StorableSmev3Response>
        {
            public StorableSmev3ResponseMapNHibernateMapping()
            {
                this.Property(x => x.Response, m =>
                {
                    m.Type<ImprovedBinaryJsonType<Smev3Response>>();
                });
            }
        }
    }
}