namespace Bars.Gkh.Interceptors.ManOrg
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Utils;
    using Entities;

    /// <summary>
    ///  Интерцептор "Управление домом, договор УК с собственниками"
    /// </summary>
    public class ManOrgContractOwnersInterceptor : EmptyDomainInterceptor<ManOrgContractOwners>
    {
        /// <summary>
        /// Домен-сервис "Жилой дом договора управляющей организации"
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> RoContractDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ManOrgContractOwners> service, ManOrgContractOwners entity)
        {
            var result = this.CheckDay(entity);

            return result.Success ? this.CheckDates(entity) : result;
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ManOrgContractOwners> service, ManOrgContractOwners entity)
        {
            return this.CheckDates(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgContractOwners> service, ManOrgContractOwners entity)
        {
            this.RoContractDomain.GetAll()
                .Where(x => x.ManOrgContract.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => this.RoContractDomain.Delete(x));

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<ManOrgContractOwners> service, ManOrgContractOwners entity)
        {
            this.CreateNewContractRobject(entity);

            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ManOrgContractOwners> service, ManOrgContractOwners entity)
        {
            this.UpdateRealityObject(entity);

            var contractRobjectId = this.RoContractDomain.GetAll()
                    .Where(x => x.ManOrgContract.Id == entity.Id
                        && x.RealityObject.Id == entity.RealityObjectId)
                    .Select(x => x.Id)
                    .FirstOrDefault();

            //проверяем, изменился ли жилой дом договора
            if (contractRobjectId > 0)
            {
                this.RoContractDomain.Delete(contractRobjectId);
                this.CreateNewContractRobject(entity);
            }

            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ManOrgContractOwners> service, ManOrgContractOwners entity)
        {
            this.UpdateRealityObject(entity);

            return this.Success();
        }

        private void CreateNewContractRobject(ManOrgBaseContract entity)
        {
            var serviceRobject = this.Container.Resolve<IDomainService<RealityObject>>();

            var newRecord = new ManOrgContractRealityObject
            {
                ManOrgContract = entity,
                RealityObject = serviceRobject.Load(entity.RealityObjectId)
            };

            this.RoContractDomain.Save(newRecord);
        }

        //проверка пересечений с другими договорами по дому
        private IDataResult CheckDates(ManOrgBaseContract entity)
        {
            var contract = this.RoContractDomain.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObjectId)
                    .Where(x => x.ManOrgContract.Id != entity.Id)
                    .Select(x => new
                    {
                        x.ManOrgContract.TypeContractManOrgRealObj,
                        x.ManOrgContract.ManagingOrganization.Contragent.Name,
                        x.ManOrgContract.DocumentDate,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate
                    })
                    .FirstOrDefault(x => (x.StartDate <= entity.StartDate && (!x.EndDate.HasValue || x.EndDate >= entity.StartDate))
                        || (!entity.EndDate.HasValue && x.StartDate >= entity.StartDate)
                        || (entity.EndDate.HasValue && x.StartDate <= entity.EndDate && (!x.EndDate.HasValue || x.EndDate >= entity.EndDate)));

            if (contract != null)
            {
                return this.Failure(
                    $"Текущий договор пересекается с существующим договором: договор {contract.TypeContractManOrgRealObj.GetEnumMeta().Display}" +
                        $" от {(contract.DocumentDate.HasValue ? contract.DocumentDate.Value.ToShortDateString() : DateTime.MinValue.ToShortDateString())}");
            }

            return this.Success();
        }

        private void UpdateRealityObject(ManOrgBaseContract entity)
        {
            var roService = this.Container.Resolve<IRepository<RealityObject>>();

            try
            {
                var robjects = this.RoContractDomain.GetAll()
                    .Where(x => x.ManOrgContract.Id == entity.Id)
                    .Select(x => x.RealityObject.Id)
                    .ToList();

                var now = DateTime.Now.Date;

                var currentContracts = this.RoContractDomain.GetAll()
                    .Where(x => robjects.Contains(x.RealityObject.Id))
                    .Where(x => x.ManOrgContract.StartDate <= now)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= now)
                    .Select(
                        x => new
                        {
                            RoId = x.RealityObject.Id,
                            ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                            InnManOrg = x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                            StartControlDate = x.ManOrgContract.StartDate, 
                            x.ManOrgContract.TypeContractManOrgRealObj
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(
                        x => x.Key,
                        y => new
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
            finally
            {
                this.Container.Release(roService);
            }
        }


        /// <summary>
        /// Проверка на промежуток между договорами управления в один день 
        /// </summary>
        /// <param name="entity">Управление домом, договор УК с собственниками</param>
        /// <returns>Результат проверки</returns>
        private IDataResult CheckDay(ManOrgContractOwners entity)
        {
            if (entity.RealityObjectId == 0)
            {
                return this.Failure("Не удалось получить жилой дом");
            }

            var endDateManOrgContract = this.RoContractDomain.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObjectId)
                .Where(x => x.ManOrgContract.Id != entity.Id)
                .OrderByDescending(x => x.ManOrgContract.EndDate)
                .Select(x => x.ManOrgContract.EndDate)
                .FirstOrDefault();

            if (endDateManOrgContract != null)
            {
                if (entity.StartDate != null && entity.StartDate.Value.Subtract(endDateManOrgContract.Value).Days > 1000)
                {
                    return this.Failure($"Перерыв в управлении МКД не может быть больше одного дня. Дата окончания предыдущего управления: {endDateManOrgContract.Value.ToShortDateString()}");
                }
            }

            return this.Success();
        }
    }
}