namespace Bars.GisIntegration.Base.Map.External.Housing.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.OkiObject
    /// </summary>
    public class OkiObjectMap : BaseEntityMap<OkiObject>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OkiObjectMap() :
            base("OKI_OBJECT")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_OBJECT_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ObjectName, "OBJECT_NAME");
            this.References(x => x.RunBase, "RUN_BASE_ID");
            this.Map(x => x.ManagTo, "MANAG_TO");
            this.Map(x => x.IsUnlimManag, "IS_UNLIM_MANAG");
            this.Map(x => x.IsMoBalance, "IS_MO_BALANCE");
            this.References(x => x.RsoCompany, "RSO_COMPANY_ID");
            this.References(x => x.OkiType, "OKI_TYPE_ID");
            this.References(x => x.MoTerritory, "MO_TERRITORY_ID");
            this.Map(x => x.StartUpFrom, "START_UP_FROM");
            this.Map(x => x.IsAutonom, "IS_AUTONOM");
            this.Map(x => x.Wearout, "WEAROUT");
            this.Map(x => x.Comment, "COMMENT");
            this.Map(x => x.CrashCount, "CRASH_COUNT");
            this.Map(x => x.IsDel, "IS_DEL");
            this.References(x => x.ObjectAddress, "OBJECT_ADDRESS");
            this.References(x => x.ElectroStationType, "ELECTRO_STATION_TYPE_ID");
            this.References(x => x.ElectroSubstantionType, "ELECTRO_SUBSTATION_TYPE_ID");
            this.References(x => x.FuelType, "FUEL_TYPE_ID");
            this.References(x => x.GasNetType, "GAS_NET_TYPE_ID");
            this.References(x => x.WaterIntakeType, "WATER_INTAKE_TYPE_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
