namespace Bars.GisIntegration.Base.Map.External.Housing.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.NetOkiObject
    /// </summary>
    public class OkiCommunalSourceMap : BaseEntityMap<OkiCommunalSource>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OkiCommunalSourceMap() :
            base("OKI_COMMUNAL_SOURCE")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_COMMUNAL_SOURCE_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.OkiObject, "OKI_OBJECT_ID");
            this.References(x => x.CommunalSource, "COMMUNAL_SOURCE_ID");
            this.References(x => x.HeatType, "HEAT_TYPE_ID");
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.OkiSection, "OKI_SECTION_ID");
            this.Map(x => x.ConnectLoad, "CONNECT_LOAD");
            this.Map(x => x.LossVolume, "LOSS_VOLUME");
            this.Map(x => x.InstallPower, "INSTALL_POWER");
            this.Map(x => x.Industry, "INDUSTRY");
            this.Map(x => x.SocialArea, "SOCIAL_AREA");
            this.Map(x => x.Populance, "POPULANCE");
            this.Map(x => x.AvailPower, "AVAIL_POWER");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
