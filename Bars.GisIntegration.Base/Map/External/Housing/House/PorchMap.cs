namespace Bars.GisIntegration.Base.Map.External.Housing.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.Porch
    /// </summary>
    public class PorchMap : BaseEntityMap<Porch>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PorchMap() :
            base("GF_PORCH")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("PORCH_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.House, "HOUSE_ID");
            References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.PorchGuid, "PORCH_GUID");
            this.Map(x => x.PorchNumber, "PORCH_NUMBER");
            this.Map(x => x.Floor, "FLOOR");
            this.Map(x => x.BuildDate, "BUILD_YEAR");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
