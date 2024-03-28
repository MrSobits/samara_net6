namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Enums;

    /// <summary>
    /// Интерцептор "Договор непосредственного управления жилым домом"
    /// </summary>
    public class RealityObjectDirectManagContractInterceptor : EmptyDomainInterceptor<RealityObjectDirectManagContract>
    {
        /// <summary>
        /// Домен-сервис "Жилой дом договора управляющей организации"
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectsService { get; set; }
        
        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectDirectManagContract> service, RealityObjectDirectManagContract entity)
        {
            var serviceContractRobject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var realObj = 
                serviceContractRobject.GetAll()
                    .Where(x => x.ManOrgContract.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

            foreach (var objId in realObj)
            {
                serviceContractRobject.Delete(objId);
            }

            this.Container.Release(serviceContractRobject);

            return this.Success();
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<RealityObjectDirectManagContract> service, RealityObjectDirectManagContract entity)
        {
            var serviceContractRobject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var serviceRobject = this.Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                var newContractRealobj = new ManOrgContractRealityObject
                                             {
                                                 ManOrgContract = entity,
                                                 RealityObject = serviceRobject.Load(entity.RealityObjectId)
                                             };

                serviceContractRobject.Save(newContractRealobj);

                return this.Success();
            }
            catch (Exception e)
            {
                return this.Failure("Произошла ошибка при сохранении");
            }
            finally
            {
                this.Container.Release(serviceContractRobject);
                this.Container.Release(serviceRobject);
            }
        }

        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectDirectManagContract> service, RealityObjectDirectManagContract entity)
        {
            if (entity.RealityObjectId == 0)
            {
                return Failure("Не удалось получить жилой дом");
            }

            var result = this.CheckDay(entity);

            return result.Success ? this.CheckDate(entity) : result;
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<RealityObjectDirectManagContract> service, RealityObjectDirectManagContract entity)
        {
            return this.CheckDate(entity);
        }

        /// <summary>
        /// Проверка пересечений с другими договорами по дому
        /// </summary>
        /// <param name="entity">Базовый класс договоров управления</param>
        /// <returns></returns>
        private IDataResult CheckDate(RealityObjectDirectManagContract entity)
        {
            if (entity.EndDate.HasValue && entity.StartDate > entity.EndDate)
            {
                return this.Failure("Дата начала должна быть меньше даты окончания");
            }

            if (entity.IsServiceContract
                && (entity.DateStartService < entity.StartDate
                    || (entity.DateEndService.HasValue && entity.DateEndService < entity.DateStartService)
                    || (entity.EndDate.HasValue && entity.EndDate < entity.DateEndService)))
            {
                return this.Failure(
                        "Дата начала оказания услуг должна быть меньше даты окончания оказания услуг. Так же период оказания услуг должен включаться в период непосредсвенного управления");
            }

            var contract = this.ManOrgContractRealityObjectsService.GetAll()
            .Where(x => x.RealityObject.Id == entity.RealityObjectId)
            .Where(x => x.ManOrgContract.Id != entity.Id)
            .Select(x => new
            {
                x.ManOrgContract.TypeContractManOrgRealObj,
                x.ManOrgContract.DocumentDate,
                x.ManOrgContract.StartDate,
                x.ManOrgContract.EndDate
            })
            .FirstOrDefault(x => (x.StartDate <= entity.StartDate && (!x.EndDate.HasValue || x.EndDate >= entity.StartDate))
                    || (!entity.EndDate.HasValue && x.StartDate >= entity.StartDate)
                    || (entity.EndDate.HasValue && x.StartDate <= entity.EndDate && (!x.EndDate.HasValue || x.EndDate >= entity.EndDate)));

            if (contract != null)
            {
                return this.Failure(string.Format("Текущий договор пересекается с существующим договором: договор {0} от {1}",
                    contract.TypeContractManOrgRealObj.GetEnumMeta().Display,
                    contract.DocumentDate.HasValue
                        ? contract.DocumentDate.Value.ToShortDateString()
                        : DateTime.MinValue.ToShortDateString()));
            }

            return this.Success();
        }

        /// <summary>
        /// Проверка на промежуток между договорами управления в один день 
        /// </summary>
        /// <param name="entity">Договор непосредственного управления жилым домом</param>
        /// <returns>Результат проверки</returns>
        private IDataResult CheckDay(RealityObjectDirectManagContract entity)
        {
            var endDateManOrgContract = this.ManOrgContractRealityObjectsService.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObjectId)
                .Where(x => x.ManOrgContract.Id != entity.Id)
                .OrderByDescending(x => x.ManOrgContract.EndDate)
                .Select(x => x.ManOrgContract.EndDate).FirstOrDefault();

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