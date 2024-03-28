namespace Bars.Gkh.Interceptors.ManOrg
{
    using System.Linq;
    using System;

    using B4;
    using B4.Utils;
    using Bars.Gkh.Utils;
    using Entities;
    using Enums;

    /// <summary>
    /// Интерцептор Управление домами (ТСЖ / ЖСК)
    /// </summary>
    public class ManOrgJskTsjContractInterceptor : EmptyDomainInterceptor<ManOrgJskTsjContract>
    {
        /// <summary>
        /// Домен-сервис "Жилой дом договора управляющей организации"
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> RoContractDomain { get; set; }

        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<ManOrgJskTsjContract> service, ManOrgJskTsjContract entity)
        {
            if (entity.RealityObjectId == 0)
                return Failure("Не указан идентификатор жилого дома");

            var result = this.CheckDay(entity);

            return result.Success ? this.CheckDates(entity) : result;
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ManOrgJskTsjContract> service, ManOrgJskTsjContract entity)
        {
            var result = this.CheckDates(entity);

            return result;
        }

        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgJskTsjContract> service, ManOrgJskTsjContract entity)
        {
            if (Container.Resolve<IDomainService<ManOrgContractRelation>>().GetAll().Any(x => x.Parent.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Договор передачи управления;");
            }

            var manOrgContractRealObjService = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var manOrgContractRealObjIds = manOrgContractRealObjService.GetAll().Where(x => x.ManOrgContract.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in manOrgContractRealObjIds)
            {
                manOrgContractRealObjService.Delete(value);
            }

            return Success();
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<ManOrgJskTsjContract> service, ManOrgJskTsjContract entity)
        {
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceContractRobject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var newRecord = new ManOrgContractRealityObject
                {
                    ManOrgContract = entity,
                    RealityObject = serviceRobject.Load(entity.RealityObjectId)
                };

            serviceContractRobject.Save(newRecord);

            Container.Release(serviceRobject);
            Container.Release(serviceContractRobject);
            return Success();
        }

        /// <summary>
        /// Метод вызывается после обновления объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterUpdateAction(IDomainService<ManOrgJskTsjContract> service, ManOrgJskTsjContract entity)
        {
            UpdateRealityObject(entity);

            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceRelations = Container.Resolve<IDomainService<ManOrgContractRelation>>();
            var serviceContractRobjects = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var contractRobjectId =
                serviceContractRobjects.GetAll()
                                       .Any(x => x.ManOrgContract.Id == entity.Id
                                                 && x.RealityObject.Id == entity.RealityObjectId);

            // если пришел идентификатор другого жилого дома, 
            // то обновляем существующие ссылки
            if (!contractRobjectId)
            {
                var newRobject = serviceRobject.Load(entity.RealityObjectId);

                var contractRobject = serviceContractRobjects.GetAll()
                                        .FirstOrDefault(x => x.ManOrgContract.Id == entity.Id);

                if (contractRobject != null)
                {
                    contractRobject.RealityObject = newRobject;
                    serviceContractRobjects.Update(contractRobject);
                }

                var relationRobjects =
                    serviceContractRobjects.GetAll()
                        .Where(y => serviceRelations.GetAll().Any(x => x.Parent.Id == entity.Id && x.Children.Id == y.ManOrgContract.Id))
                        .ToList();

                foreach (var relation in relationRobjects)
                {
                    relation.RealityObject = newRobject;
                    serviceRelations.Update(relation);
                }
            }
            return Success();
        }

        /// <summary>
        /// Метод вызывается после удаления объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterDeleteAction(IDomainService<ManOrgJskTsjContract> service, ManOrgJskTsjContract entity)
        {
            UpdateRealityObject(entity);

            return Success();
        }

        /// <summary>
        /// Проверка пересечений с другими договорами по дому
        /// </summary>
        /// <param name="entity">Базовый класс договоров управления</param>
        /// <returns></returns>
        private IDataResult CheckDates(ManOrgBaseContract entity)
        {
            var serviceContractRobject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var serviceContractRelation = Container.Resolve<IDomainService<ManOrgContractRelation>>();

            var contract =
                serviceContractRobject.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObjectId)
                    .Where(x => x.ManOrgContract.Id != entity.Id)
                    .Where(y => !serviceContractRelation.GetAll().Where(x => x.Parent.Id == entity.Id).Select(x => x.Children.Id).Contains(y.ManOrgContract.Id))
                    .Select(x => new
                        {
                            x.ManOrgContract.TypeContractManOrgRealObj,
                            x.ManOrgContract.DocumentDate,
                            x.ManOrgContract.StartDate,
                            x.ManOrgContract.EndDate,
                            x.ManOrgContract.Id
                        })
                    .FirstOrDefault(x => (x.StartDate <= entity.StartDate && (!x.EndDate.HasValue || x.EndDate >= entity.StartDate))
                        || (!entity.EndDate.HasValue && x.StartDate >= entity.StartDate)
                        || (entity.EndDate.HasValue && x.StartDate <= entity.EndDate && (!x.EndDate.HasValue || x.EndDate >= entity.EndDate)));

            if (contract != null)
            {
                return Failure(string.Format("Текущий договор пересекается с существующим договором: договор {0} от {1}",
                    contract.TypeContractManOrgRealObj.GetEnumMeta().Display,
                    contract.DocumentDate.HasValue
                        ? contract.DocumentDate.Value.ToShortDateString()
                        : DateTime.MinValue.ToShortDateString()));
            }

            var childContract = serviceContractRelation.GetAll()
                .Where(x => x.Parent.Id == entity.Id)
                .Select(x => new
                    {
                        x.Children.StartDate,
                        x.Children.EndDate
                    })
                .FirstOrDefault(x => x.StartDate < entity.StartDate
                    || (entity.EndDate.HasValue && x.StartDate > entity.EndDate)
                    || (entity.EndDate.HasValue && !x.EndDate.HasValue)
                    || (entity.EndDate.HasValue && x.EndDate.HasValue && x.EndDate > entity.EndDate));

            if (childContract != null)
            {
                return Failure("Период договора передачи управления выходит за пределы периода договора ТСЖ/ЖСК");
            }

            return Success();
        }

        /// <summary>
        /// Обновление Жилого дома 
        /// </summary>
        /// <param name="entity">Базовый класс договоров управления</param>
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

        /// <summary>
        /// Проверка на промежуток между договорами управления в один день 
        /// </summary>
        /// <param name="entity">Управление домами (ТСЖ / ЖСК)</param>
        /// <returns>Результат проверки</returns>
        private IDataResult CheckDay(ManOrgJskTsjContract entity)
        {
            if (entity.RealityObjectId == 0)
                return this.Failure("Не удалось получить жилой дом");

            var endDateManOrgContract = this.RoContractDomain.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObjectId)
                .Where(x => x.ManOrgContract.Id != entity.Id)
                .OrderByDescending(x => x.ManOrgContract.EndDate)
                .Select(x => x.ManOrgContract.EndDate)
                .FirstOrDefault();

            if (endDateManOrgContract != null)
            {
                if (entity.StartDate.Value.Subtract(endDateManOrgContract.Value).Days > 1000)
                {
                    return this.Failure($"Перерыв в управлении МКД не может быть больше одного дня. Дата окончания предыдущего управления: {endDateManOrgContract.Value.ToShortDateString()}");
                }
            }

            return this.Success();
        }
    }
}