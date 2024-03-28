namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;

    public class FixManOrgContractsRealObjectsAction : BaseExecutionAction
    {
        public override string Description => "Исправление ошибки привязки нескольких домов к одному договору, таблица GKH_MORG_CONTRACT_REALOBJ";

        public override string Name => "Исправление ошибки привязки нескольких домов к одному договору";

        public override Func<IDataResult> Action => this.FixManOrgContracts;

        private BaseDataResult FixManOrgContracts()
        {
            var manOrgContractRealityObjectRepository = this.Container.ResolveRepository<ManOrgContractRealityObject>();
            var manOrgBaseContractRepository = this.Container.ResolveRepository<ManOrgBaseContract>();
            using (this.Container.Using(manOrgContractRealityObjectRepository, manOrgBaseContractRepository))
            {
                var manOrgBaseContracts = manOrgContractRealityObjectRepository.GetAll()
                    .GroupBy(mc => mc.ManOrgContract.Id)
                    .Where(gr => gr.Count() > 1)
                    .Select(r => r.Key).ToList();

                var manOrgContractRealityObjects = manOrgContractRealityObjectRepository.GetAll()
                    .Where(mc => manOrgBaseContracts.Contains(mc.ManOrgContract.Id))
                    .ToList();

                foreach (long contractId in manOrgBaseContracts)
                {
                    var oldContract = manOrgBaseContractRepository.Get(contractId);

                    foreach (var manOrgContractRealityObject in manOrgContractRealityObjects.Where(mc => mc.ManOrgContract == oldContract).ToList())
                    {
                        var newContract = new ManOrgBaseContract
                        {
                            DocumentDate = oldContract.DocumentDate,
                            DocumentName = oldContract.DocumentName,
                            DocumentNumber = oldContract.DocumentNumber,
                            EndDate = oldContract.EndDate,
                            ExternalId = oldContract.ExternalId,
                            FileInfo = oldContract.FileInfo,
                            ManagingOrganization = oldContract.ManagingOrganization,
                            Note = oldContract.Note,
                            ObjectCreateDate = oldContract.ObjectCreateDate,
                            ObjectEditDate = oldContract.ObjectEditDate,
                            PlannedEndDate = oldContract.PlannedEndDate,
                            StartDate = oldContract.StartDate,
                            TerminateReason = oldContract.TerminateReason,
                            TypeContractManOrgRealObj = oldContract.TypeContractManOrgRealObj
                        };
                        manOrgBaseContractRepository.Save(newContract);
                        manOrgContractRealityObject.ManOrgContract = newContract;
                        manOrgContractRealityObjectRepository.Update(manOrgContractRealityObject);
                    }

                    manOrgBaseContractRepository.Delete(oldContract.Id);
                }
            }

            return new BaseDataResult();
        }
    }
}