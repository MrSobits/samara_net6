namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class TransferSorgRoContractsAction : BaseExecutionAction
    {
        public override string Description
            =>
                "Данные, которые были ранее добавлены в раздел  Жилищный фонд / Реестр жилых домов /Форма редактирования жилого дома / Подраздел «Поставщики жилищных услуг», "
                    +
                    "перенести в: - участники процесса / Роли контрагента /Поставщики жилищных  услуг / форма редактирования /раздел Договора с жилыми домами "
                    +
                    "(по связке «Жилой дом» - контрагент – договор), для поставщиков с типом «поставщик жилищных услуг».";

        public override string Name => "Перенос контрактов организаций поставщиков жил. услуг";

        public override Func<IDataResult> Action => this.TransferSorgRoContracts;

        public BaseDataResult TransferSorgRoContracts()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var servorgContractService = this.Container.Resolve<IDomainService<ServiceOrgContract>>();
                var servorgRoSorgService = this.Container.Resolve<IDomainService<RealityObjectServiceOrg>>();
                var servorgService = this.Container.Resolve<IDomainService<ServiceOrganization>>();
                var servorgRoService = this.Container.Resolve<IDomainService<ServiceOrgRealityObject>>();
                var soDict = new Dictionary<long, ServiceOrganization>();

                try
                {
                    var oldData =
                        servorgRoSorgService.GetAll().Where(x => x.TypeServiceOrg == TypeServiceOrg.ServiceOrganization);

                    foreach (var data in oldData)
                    {
                        //находим организацию поставщика жил. услуг по ид. контрагента
                        var servOrg = (soDict.ContainsKey(data.Organization.Id)
                            ? soDict[data.Organization.Id]
                            : null) ?? servorgService.GetAll()
                                .Where(x => x.Contragent.Id == data.Organization.Id)
                                .Select(x => x)
                                .FirstOrDefault();

                        if (servOrg == null)
                        {
                            continue;
                        }
                        if (!soDict.ContainsKey(data.Organization.Id))
                        {
                            soDict.Add(data.Organization.Id, servOrg);
                        }

                        //новая сущность организации и обслуживаемого ею дома
                        var sorgRo = new ServiceOrgRealityObject
                        {
                            ServiceOrg = servOrg,
                            RealityObject = data.RealityObject
                        };

                        servorgRoService.Save(sorgRo);

                        //новый контракт этой организации с домом
                        var newContract = new ServiceOrgContract
                        {
                            ServOrg = servOrg,
                            DocumentNumber = data.DocumentNum,
                            DocumentDate = data.DocumentDate,
                            DateStart = data.DocumentDate,
                            FileInfo = data.File,
                            RealityObjectId = data.RealityObject.Id
                        };

                        servorgContractService.Save(newContract);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(servorgRoSorgService);
                    this.Container.Release(servorgContractService);
                    this.Container.Release(servorgService);
                    this.Container.Release(servorgRoService);
                }
            }

            return new BaseDataResult();
        }
    }
}