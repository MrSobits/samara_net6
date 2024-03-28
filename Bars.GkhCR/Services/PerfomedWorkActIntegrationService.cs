namespace Bars.GkhCr.Services
{
    using Bars.Gkh.DomainService;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;

    /// <summary>
    /// Сервис для получение данных из Акта выполненных работ
    /// </summary>
    public class PerfomedWorkActIntegrationService : IPerfomedWorkActIntegrationService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Доме сервис Акт выполненных работ
        /// </summary>
        public IDomainService<PerformedWorkAct> PerformedWorkActDomain { get; set; }

        /// <summary>
        /// Получение данных из Акта выполненных работ
        /// </summary>
        /// <param name="ids">Id жилых домов</param>
        /// <returns></returns>
        public Dictionary<long, IEnumerable<PerfomedWorkActProxy>> GetPerfomedWorkActProxies(long[] ids)
        {
            var result = this.PerformedWorkActDomain.GetAll()
                .Where(x => ids.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.State.FinalState)
                .Select(x => new
                {
                    x.ObjectCr.RealityObject.Id,
                    x.TypeWorkCr.Work.Name,
                    x.DateFrom
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(
                        y => new PerfomedWorkActProxy
                        {
                            TypesWorkOverhaul = y.Name,
                            DatePerformance = y.DateFrom
                        }));

            return result;
        }
    }
}