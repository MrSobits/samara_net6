namespace Bars.Gkh.Ris.ExecutionAction
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Автогенерация договоров
    /// </summary>
    internal class GenerateContracts : BaseExecutionAction
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Автогенерация договоров";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Автогенерация договоров управления УК с собственниками для управляющей организации: ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ УК \"УЮТ - СЕРВИС\"";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Generate;

        /// <summary>
        /// Метод генерации
        /// </summary>
        /// <returns>результат</returns>
        public BaseDataResult Generate()
        {
            var countContract = 999;

            var manorg = this.GetManOrg();

            if (manorg == null)
            {
                return BaseDataResult.Error("Не удалось получить УО");
            }

            var realityObjectDomain = this.Container.ResolveRepository<RealityObject>();
            var managingOrgRealityObjectDomain = this.Container.ResolveRepository<ManagingOrgRealityObject>();
            var manOrgContractRealityObjectDomain = this.Container.ResolveRepository<ManOrgContractRealityObject>();
            var manOrgContractOwnersDomain = this.Container.ResolveRepository<ManOrgContractOwners>();

            try
            {
                //this.DeleteErrorRecords(managingOrgRealityObjectDomain, manOrgContractRealityObjectDomain, manOrgContractOwnersDomain);

                var freeHouses =
                    realityObjectDomain.GetAll()
                        .Where(x => !managingOrgRealityObjectDomain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                        .Where(x => !manOrgContractRealityObjectDomain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                        .Take(countContract)
                        .ToArray();

                var index = 2;

                foreach (var freeHouse in freeHouses)
                {
                    //связь УО с домом
                    var managingOrgRealityObject = new ManagingOrgRealityObject
                    {
                        RealityObject = freeHouse,
                        ManagingOrganization = manorg
                    };

                    managingOrgRealityObjectDomain.Save(managingOrgRealityObject);

                    //договор ук с собственниками
                    var contract = new ManOrgContractOwners
                    {
                        TypeContractManOrgRealObj = TypeContractManOrg.ManagingOrgOwners,
                        ManagingOrganization = manorg,
                        ContractFoundation = ManOrgContractOwnersFoundation.OwnersMeetingProtocol,
                        ProtocolNumber = (index).ToString(),
                        ProtocolDate = new DateTime(2015, 12, 1),
                        ProtocolFileInfo = this.GetFileInfo(false),
                        DocumentNumber = (index).ToString(),
                        DocumentDate = new DateTime(2016, 1, 1),
                        StartDate = new DateTime(2016, 2, 1),
                        EndDate = new DateTime(2017, 2, 1),
                        PlannedEndDate = new DateTime(2017, 2, 1),
                        FileInfo = this.GetFileInfo(true),
                        Note = "Примечание к договору",
                        InputMeteringDeviceValuesBeginDate = 1,
                        InputMeteringDeviceValuesEndDate = 20,
                        DrawingPaymentDocumentDate = 15,
                        ThisMonthPaymentDocDate = true
                    };

                    manOrgContractOwnersDomain.Save(contract);

                    //связь договора с домом
                    var manOrgContractRealityObject = new ManOrgContractRealityObject
                    {
                        RealityObject = freeHouse,
                        ManOrgContract = contract
                    };

                    manOrgContractRealityObjectDomain.Save(manOrgContractRealityObject);

                    index++;
                }
            }
            finally
            {
                this.Container.Release(realityObjectDomain);
                this.Container.Release(managingOrgRealityObjectDomain);
                this.Container.Release(manOrgContractRealityObjectDomain);
                this.Container.Release(manOrgContractOwnersDomain);
            }

            return new BaseDataResult(true);
        }

        private ManagingOrganization GetManOrg()
        {
            var manOrgDomain = this.Container.ResolveDomain<ManagingOrganization>();

            // var name = "ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ УК \"УЮТ - СЕРВИС\"";

            var contragentId = 33975;

            try
            {
                return
                    manOrgDomain.GetAll()

                        //.FirstOrDefault(x => x.Contragent.Name == name);
                        .FirstOrDefault(x => x.Contragent.Id == contragentId);
            }
            finally
            {
                this.Container.Release(manOrgDomain);
            }
        }

        private FileInfo GetFileInfo(bool contract)
        {
            if (contract)
            {
                return this.Container.Resolve<IFileManager>().SaveFile(
                    "Договор управления",
                    "txt",
                    Encoding.UTF8.GetBytes("Договор управления"));
            }

            return this.Container.Resolve<IFileManager>().SaveFile(
                "Протокол общего собрания",
                "txt",
                Encoding.UTF8.GetBytes("Протокол общего собрания"));
        }

        private void DeleteErrorRecords(
            IRepository<ManagingOrgRealityObject> managingOrgRealityObjectDomain,
            IRepository<ManOrgContractRealityObject> manOrgContractRealityObjectDomain,
            IRepository<ManOrgContractOwners> manOrgContractOwnersDomain)
        {
            var errormanagingOrgRealityObjects = managingOrgRealityObjectDomain.GetAll()
                .Where(y => y.ManagingOrganization.Id == 13669)
                .Where(x => x.ObjectCreateDate > new DateTime(2016, 4, 7))
                .ToList();
            var errormanOrgContractRealityObjects = manOrgContractRealityObjectDomain.GetAll()
                .Where(y => y.ManOrgContract.ManagingOrganization.Id == 13669)
                .Where(x => x.ObjectCreateDate > new DateTime(2016, 4, 7)).ToList();
            var errorContrs = errormanOrgContractRealityObjects.Select(x => x.ManOrgContract).ToList();

            foreach (var errormanagingOrgRealityObject in errormanagingOrgRealityObjects)
            {
                managingOrgRealityObjectDomain.Delete(errormanagingOrgRealityObject.Id);
            }

            foreach (var errormanOrgContractRealityObject in errormanOrgContractRealityObjects)
            {
                manOrgContractRealityObjectDomain.Delete(errormanOrgContractRealityObject.Id);
            }

            foreach (var errorContr in errorContrs)
            {
                manOrgContractOwnersDomain.Delete(errorContr.Id);
            }
        }
    }
}