namespace Bars.Gkh.Import.Organization.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhExcel;

    using Castle.Core;

    using NHibernate;

    /// <summary>
    /// Хелпер импорта организаций для непосредственого управления дома
    /// </summary>
    public class DirectManagementImportHelper : BaseOrganizationImportHelper, IOrganizationImportHelper, IInitializable
    {
        public DirectManagementImportHelper(
            IOrganizationImportCommonHelper commonHelper,
            IDictionary<string, long> headersDict,
            ILogImport logImport)
            : base(commonHelper, headersDict, logImport)
        {
        }

        public IRepository<ManOrgContractRealityObject> ManOrgContractRealityObjectRepository { get; set; }

        private List<long> existingRoDirectManagContract = new List<long>();

        private List<RealityObjectDirectManagContract> realityObjectDirectManagContractToCreate = new List<RealityObjectDirectManagContract>();

        private List<ManOrgContractRealityObject> manOrgContractRealityObjectToCreate = new List<ManOrgContractRealityObject>();

        public string OrganizationType { get { return "ну"; } }

        public void Initialize()
        {
            this.existingRoDirectManagContract = this.ManOrgContractRealityObjectRepository.GetAll()
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                .Select(x => x.RealityObject.Id)
                .Distinct()
                .ToList();
        }

        public override IDataResult ProcessLine(GkhExcelCell[] data, Record record)
        {
            return new BaseDataResult();
        }

        protected override void GetOrganizationId(Record record)
        {
        }
        
        protected override void CreateContractIfNotExist(Record record, bool updatePeriodsManOrgs = false)
        {
            
        }

        public bool CreateContract(Record record, out string message, bool updatePeriodsManOrgs)
        {
            if (!this.existingRoDirectManagContract.Contains(record.RealtyObjectId))
            {
                var realityObjectDirectManagContract = new RealityObjectDirectManagContract
                {
                    TypeContractManOrgRealObj = TypeContractManOrg.DirectManag,
                    StartDate = record.ContractStartDate,
                    DocumentDate = record.DocumentDate,
                    DocumentNumber = record.DocumentNumber,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.realityObjectDirectManagContractToCreate.Add(realityObjectDirectManagContract);

                var manOrgContractRealityObject = new ManOrgContractRealityObject
                {
                    ManOrgContract = realityObjectDirectManagContract,
                    RealityObject = new RealityObject { Id = record.RealtyObjectId },
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                this.manOrgContractRealityObjectToCreate.Add(manOrgContractRealityObject);

                this.existingRoDirectManagContract.Add(record.RealtyObjectId);

                // Обновить уо и вид управления дома
                this.CommonHelper.UpdateRealtyObjectManOrg(
                    roId: record.RealtyObjectId,
                    contractStartDate: record.ContractStartDate,
                    manOrgName: string.Empty,
                    manInn: string.Empty,
                    startControlDate: record.ContractStartDate.ToString("dd.MM.YYYY"),
                    typeContract: TypeContractManOrg.DirectManag.GetEnumMeta().Display);

                message = "Успешно";
            }
            else
            {
                message = "Данный дом уже в непосредственном управлении";
            }
           
            return true;
        }

        public void SaveData(IStatelessSession session)
        {
            this.realityObjectDirectManagContractToCreate.ForEach(x => session.Insert(x));
            this.manOrgContractRealityObjectToCreate.ForEach(x => session.Insert(x));
        }
    }
}