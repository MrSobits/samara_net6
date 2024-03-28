namespace Bars.GisIntegration.Base.Map.External.Housing.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.NetPart
    /// </summary>
    public class NetPartMap : BaseEntityMap<NetPart>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public NetPartMap() :
            base("OKI_NET_PART")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_NET_PART_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.OkiObject, "OKI_OBJECT_ID");
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.NetPartName, "NET_PART_NAME");
            this.Map(x => x.Diameter, "DIAMETER");
            this.Map(x => x.Length, "LENGTH");
            this.Map(x => x.ChangeKmCnt, "CHANGE_KM_CNT");
            this.Map(x => x.Wearout, "WEAROUT");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
            this.References(x => x.GasPressure, "GAS_PRESSURE_ID");
            this.References(x => x.Voltage, "VOLTAGE_ID");
        }

    }
}
