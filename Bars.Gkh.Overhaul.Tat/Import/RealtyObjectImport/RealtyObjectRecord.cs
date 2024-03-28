namespace Bars.Gkh.Overhaul.Tat.Import
{
    using B4.Modules.FIAS;

    public class RealtyObjectRecord
    {
        public bool isValidRecord { get; set; }

        public long? FiasAddressId { get; set; }

        public FiasAddress FiasAddress { get; set; }

        public string StreetAoGuid { get; set; }

        /// <summary>
        /// Идентификатор в файле
        /// </summary>
        public string ExternalId { get; set; }

        public string LocalityName { get; set; }

        public string StreetName { get; set; }

        public string House { get; set; }

        public string Housing { get; set; }

        public string TypeHouse { get; set; }

        public string BuildYear { get; set; }

        public string AreaMkd { get; set; }

        public string Floors { get; set; }

        public string MaximumFloors { get; set; }

        public string NumberEntrances { get; set; }

        public string NumberApartments { get; set; }

        public string NumberLiving { get; set; }

        public string AreaLiving { get; set; }

        public string AreaLivingOwned { get; set; }

        public string PrivatizationDateFirstApartment { get; set; }

        public string FacadeArea { get; set; }

        public string RoofArea { get; set; }

        public string BasementArea { get; set; }

        public string FacadeLastRepairYear { get; set; }

        public string RoofLastRepairYear { get; set; }

        public string BasementLastRepairYear { get; set; }

        public string LiftLastRepairYear { get; set; }

        public string ElectricityLastRepairYear { get; set; }

        public string HeatSystemLastRepairYear { get; set; }

        public string ColdWaterLastRepairYear { get; set; }

        public string HotWaterLastRepairYear { get; set; }

        public string SewerageLastRepairYear { get; set; }

        public string GasLastRepairYear { get; set; }


        public string NumberLifts { get; set; }

        public string ConditionHouse { get; set; }


        public string FacadeType { get; set; }

        public string RoofType { get; set; }

        public string BasementType { get; set; }

        public string HeatsystemType { get; set; }

        public string WaterSystemType { get; set; }

        public string SewerageType { get; set; }

        public string GasSystemType { get; set; }

        public string ElectricitySystemType { get; set; }

        public string LiftType { get; set; }

        public string ColdWaterPipeLength { get; set; }

        public string HotWaterPipeLength { get; set; }
    }
}