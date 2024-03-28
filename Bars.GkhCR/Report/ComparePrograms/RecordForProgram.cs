namespace Bars.GkhCr.Report.ComparePrograms
{
    using System.Collections.Generic;

    using Bars.Gkh.Enums;

    /// <summary>
    /// Информация о доме по программе
    /// </summary>
    internal class RecordForProgram
    {
        public long ObjectCrId { get; set; }
               
        public long RealityObjectId { get; set; }
               
        public long ProgramCrId { get; set; }

        public string Address { get; set; }

        public TypeManagementManOrg TypeManagement { get; set; }

        public string ManagementOrganization { get; set; }

        public long MunicipalityId { get; set; }

        public string Municipality { get; set; }

        public string GroupMunicipality { get; set; }

        public string PlaceAddressManOrg { get; set; }

        public string PostCode { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Flat { get; set; }

        public string Inn { get; set; }

        public string Kpp { get; set; }

        public string Leader { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string MaximumFloors { get; set; }

        public string SeriesHome { get; set; }

        public string CapitalGroup { get; set; }

        public decimal AreaMkd { get; set; }

        public decimal AreaLivingNotLivingMkd { get; set; }

        public decimal AreaLiving { get; set; }

        public decimal AreaLivingOwned { get; set; }

        public decimal SumDevolopmentPsd { get; set; }

        public decimal SumTehInspection { get; set; }

        public decimal SumSmrApproved { get; set; }
        
        public int NumberApartments { get; set; }

        public int NumberLiving { get; set; }

        public string WallMaterial { get; set; }

        public string RoofingMaterial { get; set; }

        public List<TypeWorkCrAndFinanceSourceProxy> FinanceSourceList { get; set; }

        /// <summary>
        /// Год ввода в эксплуатацию
        /// </summary>
        public string YearCommissioning { get; set; }

        public decimal PhysicalWear { get; set; }

        public string YearLastOverhaul { get; set; }

        public void Add(RecordForProgram rec)
        {
            if (rec == null) return;
            this.AreaMkd += rec.AreaMkd;
            this.AreaLivingNotLivingMkd += rec.AreaLivingNotLivingMkd;
            this.AreaLiving += rec.AreaLiving;
            this.AreaLivingOwned += rec.AreaLivingOwned;
            this.NumberApartments += rec.NumberApartments;
            this.NumberLiving += rec.NumberLiving;
        }

    }
}
