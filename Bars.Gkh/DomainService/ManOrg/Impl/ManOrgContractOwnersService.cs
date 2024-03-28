namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using B4;

    using Castle.Windsor;

    using Entities;

    /// <summary>
    /// Сервис для ManOrgContractOwnersController
    /// </summary>
    public class ManOrgContractOwnersService : IManOrgContractOwnersService
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить информацию по жилому дому
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("contractId");

            //if (contractId < 1)
            //    return new BaseDataResult { Success = false, Message = "Не удалось получить договор" };

            var realObject =
                this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => x.ManOrgContract.Id == contractId)
                    .Select(x => x.RealityObject)
                    .FirstOrDefault();

            return new BaseDataResult(realObject);
        }
    }
}