namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using B4;
    using Entities;
    using Castle.Windsor;
    using System;

    /// <summary>
    /// Интерфейс сервиса "Управление домами (ТСЖ / ЖСК)"
    /// </summary>
    public class ManOrgJskTsjContractService : IManOrgJskTsjContractService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервиса "Жилой дом договора управляющей организации"
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectsService { get; set; }

        /// <summary>
        /// Возвращает объект недвижимости договора
        /// </summary>
        /// <returns></returns>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Жилой дом</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("contractId");

            var robject = 
                Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => x.ManOrgContract.Id == contractId)
                    .Select(x => x.RealityObject)
                    .FirstOrDefault();

            return new BaseDataResult(robject);
        }

        /// <summary>
        /// Проверка на дату договора управления 
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат проверки</returns>
        public IDataResult VerificationDate(BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("contractId");
            var startDate = baseParams.Params.GetAs<DateTime>("startDate");

            var manOrgContractRealityObject = this.ManOrgContractRealityObjectsService.GetAll()
                    .Where(x => x.ManOrgContract.Id == contractId)
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                    })
                    .FirstOrDefault();


            var endDateManOrgContract = this.ManOrgContractRealityObjectsService.GetAll()
                .Where(x => x.RealityObject.Id == manOrgContractRealityObject.Id)
                .Where(x => x.ManOrgContract.Id != contractId)
                .OrderByDescending(x => x.ManOrgContract.EndDate)
                .Select(x => x.ManOrgContract.EndDate).FirstOrDefault();

            if (endDateManOrgContract != null)
            {
                if (startDate.Subtract(endDateManOrgContract.Value).Days > 1000)
                {
                    return new BaseDataResult(false, $"Перерыв в управлении МКД не может быть больше одного дня. Дата окончания предыдущего управления: {endDateManOrgContract.Value.ToShortDateString()}");
                }
            }

            return new BaseDataResult();
        }

    }
}