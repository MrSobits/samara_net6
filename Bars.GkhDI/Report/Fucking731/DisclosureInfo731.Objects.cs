namespace Bars.GkhDi.Report.Fucking731
{
    using System;
    using System.Collections.Generic;

    public partial class DisclosureInfo731
    {
        #region ManOrgInfo
                public class ManOrgRecord
        {
            public ManOrgRecord()
            {
                ManOrgObject = new List<ManOrgObject>();

                ManOrgTerminateContract = new List<ManOrgTerminateContract>();

                ManOrgMembershipUnion = new List<ManOrgMembershipUnion>();

                ManOrgFundsInfo = new List<ManOrgFundsInfo>();

                ManOrgContractInfo = new List<ManOrgContractInfo>();

                ManOrgAdminResp = new List<ManOrgAdminResp>();

                ManOrgFinActivityManag = new List<ManOrgFinActivityManag>();

                Robject = new List<Robject>();
            }

            public string ManOrg { get; set; }

            public string ManOrgOgrn { get; set; }

            public string ManOrgRegDate { get; set; }

            public string ManOrgRegOrg { get; set; }

            public string ManOrgFactAddress { get; set; }

            public string ManOrgMailAddress { get; set; }

            public string ManOrgPhone { get; set; }

            public string ManOrgEmail { get; set; }

            public string ManOrgWorkMode { get; set; }

            public string ManOrgReceptCits { get; set; }

            public decimal ManOrgObjectsSumArea { get; set; }

            public int ManOrgTerminateContractCount { get; set; }

            public string ExistBookkepingBalance { get; set; }

            public string ExistBookkepingBalanceAnnex { get; set; }

            public List<ManOrgObject> ManOrgObject { get; set; }

            public List<ManOrgTerminateContract> ManOrgTerminateContract { get; set; }

            public List<ManOrgMembershipUnion> ManOrgMembershipUnion { get; set; }

            public List<ManOrgFundsInfo> ManOrgFundsInfo { get; set; }

            public List<ManOrgContractInfo> ManOrgContractInfo { get; set; }
            
            public List<ManOrgAdminResp> ManOrgAdminResp { get; set; }

            public List<ManOrgFinActivityManag> ManOrgFinActivityManag { get; set; }

            public List<Robject> Robject { get; set; }
        }
        public class ManOrgObject
        {
            public int Number { get; set; }

            public string Address { get; set; }

            public decimal AreaLiving{ get; set; }
        }
                public class ManOrgTerminateContract
        {
            public int Number { get; set; }

            public string Address { get; set; }

            public string Reason { get; set; }
        }
                public class ManOrgMembershipUnion
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }

            public string OfficialSite { get; set; }
        }
                public class ManOrgFundsInfo
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public decimal PaymentSize { get; set; }

            public string Date { get; set; }
        }
                public class ManOrgContractInfo
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }

            public decimal Cost { get; set; }

            public string Date { get; set; }
        }
                public class ManOrgAdminResp
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public int CountViolation { get; set; }

            public string Date { get; set; }

            public decimal Sum { get; set; }

            public string DatePayment { get; set; }
        }
                public class ManOrgFinActivityManag
        {
            public int Number { get; set; }

            public string Address { get; set; }

            public decimal AreaMkd { get; set; }

            public decimal PresentedToPay { get; set; }

            public decimal RecievedProvidedService { get; set; }

            public decimal SumDebt { get; set; }
        }

        #endregion ManOrgInfo

        #region RobjectInfo
                public class Robject
        {
            public long RoId { get; set; }

            public long DiRoId { get; set; }

            public string Address { get; set; }

            public int ObjectReductionPaymentCount { get; set; }

            public decimal ObjectReductionPaymentSum { get; set; }

            public ObjectManagService[] ObjectManagService { get; set; }

            public ObjectRepairProvider[] ObjectRepairProvider { get; set; }

            public ObjectHousingProvider[] ObjectHousingProvider { get; set; }

            public ObjectAdditionalProvider[] ObjectAdditionalProvider { get; set; }

            public ObjectCommunalService[] ObjectCommunalService { get; set; }

            public ObjectCommonFacility[] ObjectCommonFacility { get; set; }

            public ObjectCommunalResource[] ObjectCommunalResource { get; set; }

            public ObjectReductionWork[] ObjectReductionWork { get; set; }

            public ObjectReductionPayment[] ObjectReductionPayment { get; set; }
        }

        public class ObjectManagService
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public decimal Tariff { get; set; }

            public string UnitMeasure { get; set; }
        }

        #region ObjectRepair
                public class ObjectRepairProvider
        {
            public long DiRoId { get; set; }

            public string Provider { get; set; }

            public ObjectRepairService[] ObjectRepairServices { get; set; }
        }
                public class ObjectRepairService
        {
            public long Id { get; set; }

            public int Number { get; set; }

            public string Name { get; set; }

            public string Type { get; set; }

            public string UnitMeasure { get; set; }

            public string Tariff { get; set; }

            public string ScheduledPreventiveMaintanance { get; set; }

            public ObjectRepairServicePpr[] ObjectRepairServicePpr { get; set; }

            public ObjectRepairServiceTo[] ObjectRepairServiceTo { get; set; }
        }
                public class ObjectRepairServicePpr
        {
            public int Number { get; set; }

            public string Work { get; set; }

            public decimal Cost { get; set; }

            public decimal FactCost { get; set; }

            public string ReasonRejection { get; set; }

            public string DateStart { get; set; }

            public string DateEnd { get; set; }

            public ObjectRepairServiceDetail[] ObjectRepairServiceDetail { get; set; }
        }
                public class ObjectRepairServiceDetail
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public decimal Volume { get; set; }

            public decimal FactVolume { get; set; }

            public string UnitMeasure { get; set; }
        }
                public class ObjectRepairServiceTo
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public decimal Cost { get; set; }

            public decimal FactCost { get; set; }

            public string DateStart { get; set; }

            public string DateEnd { get; set; }
        }

        #endregion ObjectRepair

        #region ObjectHousing

        public class ObjectHousingProvider
        {
            public string Provider { get; set; }

            public ObjectHousingService[] ObjectHousingService { get; set; }
        }

        public class ObjectHousingService
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public string UnitMeasure { get; set; }

            public string Periodicity { get; set; }

            public decimal Tariff { get; set; }

            public ObjectHousingServiceItem[] ObjectHousingServiceItem { get; set; }
        }

        public class ObjectHousingServiceItem
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public decimal Sum { get; set; }
        }

        #endregion ObjectHousing

        #region ObjectAdditional

        public class ObjectAdditionalProvider
        {
            public string Provider { get; set; }

            public ObjectAdditionalService[] ObjectAdditionalService { get; set; }
        }
                public class ObjectAdditionalService
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public string Periodicity { get; set; }

            public decimal Tariff { get; set; }
        }

        #endregion ObjectAdditional
                public class ObjectCommunalService
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public string Provider { get; set; }

            public decimal Tariff { get; set; }
        }
                public class ObjectCommonFacility
        {
            public int Number { get; set; }

            public string KindCommonFacilities { get; set; }

            public string Lessee { get; set; }

            public string ContractPeriod { get; set; }

            public decimal CostContract { get; set; }

            public string ProtocolNumber { get; set; }

            public string ProtocolDate { get; set; }

            public string TypeContract { get; set; }
        }
                public class ObjectCommunalResource
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public string Provider { get; set; }

            public decimal Volume { get; set; }

            public decimal TariffRso { get; set; }

            public decimal TariffRt { get; set; }

            public decimal TariffCitizens { get; set; }

            public string Decision { get; set; }
        }
                public class ObjectReductionWork
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public string PlanName { get; set; }

            public string DateComplete { get; set; }

            public decimal PlannedReductionExpense { get; set; }

            public decimal FactedReductionExpense { get; set; }

            public string ReasonRejection { get; set; }
        }
                public class ObjectReductionPayment
        {
            public int Number { get; set; }

            public string Name { get; set; }

            public string OrderDate { get; set; }

            public decimal RecalculationSum { get; set; }

            public string UnitMeasure { get; set; }
        }
                public class TariffForRsoProxy
        {
            public long BaseServiceId { get; set; }

            public decimal? Cost { get; set; }

            public string NumberAct { get; set; }

            public DateTime? DateAct { get; set; }

            public string Org { get; set; }
        }

        public struct ProviderProxy
        {
            public long Id { get; set; }

            public string Name { get; set; }
        }

        #endregion RobjectInfo
    }
}