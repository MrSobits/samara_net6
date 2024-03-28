namespace Bars.GisIntegration.Base.Map.External.Housing.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.House
    /// </summary>
    public class HouseMap : BaseEntityMap<House>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public HouseMap() :
            base("GF_HOUSE")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("HOUSE_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.CadastrNumber, "CADASTR_NUMBER");
            this.Map(x => x.EgrpNumber, "EGRP_NUMBER");
            this.Map(x => x.IsGknNone, "IS_GKN_NONE");
            this.Map(x => x.HouseFiasGuid, "HOUSE_FIAS_GUID");
            this.References(x => x.FiasAddress, "FIAS_ADDRESS_ID");
            this.References(x => x.MoTerritory, "MO_TERRITORY_ID");
            this.References(x => x.TimeZone, "TIME_ZONE_ID");
            this.Map(x => x.TotalSquare, "TOTAL_SQUARE");
            this.Map(x => x.BuildSquare, "BUILD_SQUARE");
            this.Map(x => x.LiveSquare, "LIVE_SQUARE");
            this.Map(x => x.NonliveSquare, "NONLIVE_SQUARE");
            this.Map(x => x.MopSquare, "MOP_SQUARE");
            this.Map(x => x.ProjectSeries, "PROJECT_SERIES");
            this.References(x => x.BuildProjectType, "BUILD_PROJECT_TYPE_ID");
            this.References(x => x.InnerWallType, "INNER_WALL_TYPE_ID");
            this.Map(x => x.BuildYear, "BUILD_YEAR");
            this.Map(x => x.StartUpYear, "START_UP_YEAR");
            this.Map(x => x.LastCrYear, "LAST_CR_YEAR");
            this.Map(x => x.RestoreYear, "RESTORE_YEAR");
            this.References(x => x.HouseType, "HOUSE_TYPE_ID");
            this.References(x => x.HouseState, "HOUSE_STATE_ID");
            this.Map(x => x.IsCultureHeritage, "IS_CULTURE_HERITAGE");
            this.Map(x => x.Wearout, "WEAROUT");
            this.Map(x => x.WearoutDate, "WEAROUT_DATE");
            this.Map(x => x.FloorCount, "FLOOR_COUNT");
            this.Map(x => x.MaxFloor, "MAX_FLOOR");
            this.Map(x => x.MinFloor, "MIN_FLOOR");
            this.Map(x => x.UndergroudFloorCount, "UNDERGROUD_FLOOR_COUNT");
            this.References(x => x.EnergyEfficiencyClass, "ENERGY_EFFICIENCY_CLASS_ID");
            this.Map(x => x.EnergySurveyDate, "ENERGY_SURVEY_DATE");
            this.References(x => x.CrFormingType, "CR_FORMING_TYPE_ID");
            this.References(x => x.ManagMode, "MANAG_MODE_ID");
            this.References(x => x.ManagementOrganization, "MANAG_ORG_ID");
            this.Map(x => x.HeatSquare, "HEAT_SQUARE");
            this.Map(x => x.LifeCycleStage, "LIFE_CYCLE_STAGE");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
