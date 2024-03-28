
namespace Bars.Gkh.DomainService
{
	using System;

	using Bars.B4.Modules.States;
	using Bars.Gkh.Enums;

    public class RealityObjectProxy
	{
		public long Id { get; set; }

		public string ExternalId { get; set; }

		public string Municipality { get; set; }

		public string Settlement { get; set; }

		public string PlaceName { get; set; }

		public string Address { get; set; }

		public string HouseGuid { get; set; }

		public string FiasHauseGuid { get; set; }

		public string FullAddress { get; set; }

		public TypeHouse TypeHouse { get; set; }

		public ConditionHouse ConditionHouse { get; set; }

        public bool HasVidecam { get; set; }

		public bool IsCulturalHeritage { get; set; }

		public DateTime? DateDemolition { get; set; }
        
		public int? Floors { get; set; }

		public int? NumberEntrances { get; set; }

		public int? NumberLiving { get; set; }

		public int? NumberApartments { get; set; }

	    public decimal? AreaLivingNotLivingMkd { get; set; }

	    public decimal? AreaMkd { get; set; }

		public decimal? AreaLiving { get; set; }

		public decimal? PhysicalWear { get; set; }

		public int? NumberLifts { get; set; }

		public HeatingSystem HeatingSystem { get; set; }

		public string WallMaterialName { get; set; }

		public TypeRoof TypeRoof { get; set; }

		public DateTime? DateLastOverhaul { get; set; }

		public DateTime? DateCommissioning { get; set; }

		public string CapitalGroup { get; set; }

		public string ManOrgNames { get; set; }

		public string TypeContracts { get; set; }

		public string CodeErc { get; set; }

		public bool IsInsuredObject { get; set; }

		public string GkhCode { get; set; }

		public YesNo IsBuildSocialMortgage { get; set; }

		public State State { get; set; }

		public bool? IsRepairInadvisable { get; set; }

		public bool IsNotInvolvedCr { get; set; }

		public bool? IsInvolvedCrTo2 { get; set; }

		public string District { get; set; }

		public decimal? TotalBuildingVolume { get; set; }

		public int? BuildYear { get; set; }

		public DateTime? PrivatizationDateFirstApartment { get; set; }

		public long? WallMaterialId { get; set; }

		public long? RoofingMaterialId { get; set; }

		public string RoofingMaterialName { get; set; }

	    public CrFundFormationType? AccountFormationVariant { get; set; }

		public string Inn { get; set; }

		public string StartControlDate { get; set; }

		public YesNoNotSet ObjectConstruction { get; set; }
    }
}