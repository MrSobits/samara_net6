namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Import.KamchatkaRealtyObject
{
    using Bars.B4.Modules.FIAS;

    public class RealtyObjectRecord
    {
        public int RowNumber { get; set; }

        public bool isValidRecord { get; set; }

        public long? FiasAddressId { get; set; }

        public FiasAddress FiasAddress { get; set; }

        public string StreetAoGuid { get; set; }

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

        public string FacadeLastRepairYear { get; set; }

        public string RoofLastRepairYear { get; set; }

        public string ElectricityLastRepairYear { get; set; }

        public string HeatSystemLastRepairYear { get; set; }

        public string ColdWaterLastRepairYear { get; set; }

        public string HotWaterLastRepairYear { get; set; }

        public string SewerageLastRepairYear { get; set; }

        public string ConditionHouse { get; set; }

        public string FacadeType { get; set; }

        public string RoofType { get; set; }

        public string HeatsystemType { get; set; }

        public string SewerageType { get; set; }

        public string ElectricitySystemType { get; set; }

        //+
        public string WallMaterial { get; set; }

        public string RoofingMaterial { get; set; }

        public string CapitalGroup { get; set; }

        public string FoundationType { get; set; }

        public string FoundationArea { get; set; }
        
        public string FoundationWearout { get; set; }

        public string FoundationLastRepairYear { get; set; }
        
        public string RoofWearout { get; set; }
        
        public string FacadeWearout { get; set; }
        
        public string ColdWaterSystemWearout { get; set; }
        
        public string HotWaterSystemWearout { get; set; }
        
        public string ColdWaterSystemType { get; set; }
        
        public string HotWaterSystemType { get; set; }

        public string SewerageWearout { get; set; }

        public string HeatsystemWearout { get; set; }

        public string ElectricityWearout { get; set; }
        

        public string ElectricNetworksLength { get; set; }

        public string HeatSystemPipeLength { get; set; }

        public string ColdWaterPipeLength { get; set; }

        public string HotWaterPipeLength { get; set; }

        public string SeweragePipeLength { get; set; }


        public string ElectroDeviceCount { get; set; }

        public string HeatDeviceCount { get; set; }

        public string ColdDeviceCount { get; set; }

        public string HotDeviceCount { get; set; }



    }
}