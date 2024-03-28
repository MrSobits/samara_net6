namespace Bars.GisIntegration.Base.Map.External.Administration.System
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.DataSupplier
    /// </summary>
    public class DataSupplierMap : BaseEntityMap<DataSupplier>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DataSupplierMap() :
            base("DATA_SUPPLIER")
        {
            //Устанавливаем схему РИС
            this.Schema("MASTER");

            this.Id(x => x.Id, m =>
            {
                m.Column("DATA_SUPPLIER_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.TableSchemaId, "TABLE_SCHEMA_ID");
            this.Map(x => x.Inn, "INN", false, 12);
            this.Map(x => x.Kpp, "KPP", false, 9);
            this.Map(x => x.Ogrn, "OGRN", false, 15);
            this.Map(x => x.DataSupplierName, "DATA_SUPPLIER");
            this.References(x=>x.ContragentType, "CONTRAGENT_TYPE_ID");
        }
    }
}
