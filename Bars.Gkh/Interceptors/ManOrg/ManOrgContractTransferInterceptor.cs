namespace Bars.Gkh.Interceptors.ManOrg
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Enums;
    using Entities;
    using Bars.Gkh.Utils;

    public class ManOrgContractTransferInterceptor : EmptyDomainInterceptor<ManOrgContractTransfer>
    {
        public IDomainService<ManOrgContractRealityObject> RoContractDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ManOrgContractTransfer> service, ManOrgContractTransfer entity)
        {
            if (entity.JskTsjContractId == 0)
            {
                return Failure("Не указан основной договор с ТСЖ/ЖСК");
            }

            var result = CheckDates(entity);

            if (!result.Success)
            {
                return result;
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ManOrgContractTransfer> service, ManOrgContractTransfer entity)
        {
            var result = CheckDates(entity);

            if (!result.Success)
            {
                return result;
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgContractTransfer> service, ManOrgContractTransfer entity)
        {
            var manOrgContractRealObjService = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var manOrgContractRealObjIds = manOrgContractRealObjService.GetAll().Where(x => x.ManOrgContract.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var i in manOrgContractRealObjIds)
            {
                manOrgContractRealObjService.Delete(i);
            }

            var manOrgContractRelationService = Container.Resolve<IDomainService<ManOrgContractRelation>>();
            var manOrgContractRelationIds = manOrgContractRelationService.GetAll().Where(x => x.Children.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var i in manOrgContractRelationIds)
            {
                manOrgContractRelationService.Delete(i);
            }

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<ManOrgContractTransfer> service, ManOrgContractTransfer entity)
        {
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceContract = Container.Resolve<IDomainService<ManOrgBaseContract>>();
            var serviceContractRobject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var serviceContractRealition = Container.Resolve<IDomainService<ManOrgContractRelation>>();

            var robjectId =
                serviceContractRobject.GetAll()
                    .Where(x => x.ManOrgContract.Id == entity.JskTsjContractId)
                    .Select(x => x.RealityObject.Id)
                    .FirstOrDefault();

            var newRecord = new ManOrgContractRealityObject
                {
                    ManOrgContract = entity,
                    RealityObject = serviceRobject.Load(robjectId)
                };

            serviceContractRobject.Save(newRecord);

            var relation = new ManOrgContractRelation
                {
                    Parent = serviceContract.Load(entity.JskTsjContractId),
                    Children = entity,
                    TypeRelation = TypeContractRelation.TransferTsjUk
                };

            serviceContractRealition.Save(relation);

            Container.Release(serviceRobject);
            Container.Release(serviceContract);
            Container.Release(serviceContractRobject);
            Container.Release(serviceContractRealition);

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ManOrgContractTransfer> service, ManOrgContractTransfer entity)
        {
            UpdateRealityObject(entity);

            return Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ManOrgContractTransfer> service, ManOrgContractTransfer entity)
        {
            UpdateRealityObject(entity);

            return Success();
        }

        //проверка пересечения с родительским договором и другими договорами передачи управления
        private IDataResult CheckDates(ManOrgContractTransfer entity)
        {
            var serviceRelation = Container.Resolve<IDomainService<ManOrgContractRelation>>();

            var parent =
                serviceRelation.GetAll()
                    .Where(x => x.Parent.Id == entity.JskTsjContractId)
                    .Select(x => new
                        {
                            x.Parent.StartDate,
                            x.Parent.EndDate
                        })
                    .FirstOrDefault();

            if (parent != null)
            {
                if (entity.StartDate < parent.StartDate
                    || (parent.EndDate.HasValue && entity.StartDate > parent.EndDate)
                    || (!entity.EndDate.HasValue && parent.EndDate.HasValue)
                    || (entity.EndDate.HasValue && parent.EndDate.HasValue && entity.EndDate > parent.EndDate))
                {
                    return Failure("Период договора передачи управления выходит за пределы периода договора ТСЖ/ЖСК");
                }

                var relation = serviceRelation.GetAll()
                    .Where(x => x.Parent.Id == entity.JskTsjContractId)
                    .Where(x => x.Children.Id != entity.Id)
                    .Select(x => new
                        {
                            x.Children.DocumentDate,
                            x.Children.StartDate,
                            x.Children.EndDate
                        })
                    .FirstOrDefault(x => (x.StartDate <= entity.StartDate && (!x.EndDate.HasValue || x.EndDate >= entity.StartDate))
                        || (!entity.EndDate.HasValue && x.StartDate >= entity.StartDate)
                        || (entity.EndDate.HasValue && x.StartDate <= entity.EndDate && (!x.EndDate.HasValue || x.EndDate >= entity.EndDate)));

                if (relation != null)
                {
                    return Failure(string.Format("Период договора передачи управления пересекается с договором от {0}",
                                              relation.DocumentDate.ToDateTime().ToShortDateString()));
                }
            }

            return Success();
        }

        private void UpdateRealityObject(ManOrgBaseContract entity)
        {
            var roService = Container.Resolve<IDomainService<RealityObject>>();

            var robjects = RoContractDomain.GetAll()
                .Where(x => x.ManOrgContract.Id == entity.Id)
                .Select(x => x.RealityObject.Id)
                .ToList();

            var now = DateTime.Now.Date;

            var currentContracts = RoContractDomain.GetAll()
                .Where(x => robjects.Contains(x.RealityObject.Id))
                .Where(x => x.ManOrgContract.StartDate <= now)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= now)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    InnManOrg = x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                    StartControlDate = x.ManOrgContract.StartDate,
                    x.ManOrgContract.TypeContractManOrgRealObj
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => new
                {
                    ManOrgs = y.AggregateWithSeparator(x => x.ManOrgName, ", "),
                    InnManOrgs = y.AggregateWithSeparator(x => x.InnManOrg, ", "),
                    StartControlDate = y.AggregateWithSeparator(x => x.StartControlDate?.ToString("dd.MM.yyyy"), ", "),
                    TypesContract = y.AggregateWithSeparator(x => x.TypeContractManOrgRealObj.GetEnumMeta().Display, ", ")
                });

            foreach (var roId in robjects)
            {
                var contract = currentContracts.Get(roId);

                var ro = roService.Load(roId);

                if (contract != null)
                {
                    ro.ManOrgs = contract.ManOrgs;
                    ro.InnManOrgs = contract.InnManOrgs;
                    ro.StartControlDate = contract.StartControlDate;
                    ro.TypesContract = contract.TypesContract;
                }
                else
                {
                    ro.ManOrgs = null;
                    ro.InnManOrgs = null;
                    ro.StartControlDate = null;
                    ro.TypesContract = null;
                }

                roService.Update(ro);
            }
        }
    }
}