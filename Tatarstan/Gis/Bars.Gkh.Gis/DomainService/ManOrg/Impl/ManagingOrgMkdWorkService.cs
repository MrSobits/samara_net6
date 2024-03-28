namespace Bars.Gkh.Gis.DomainService.ManOrg.Impl
{
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities.ManOrg;
    using Gkh.Entities.Dicts;
    using System;
    using System.Linq;

    /// <summary>
    /// Сервис для ManagingOrgMkdWorkController
    /// </summary>
    public class ManagingOrgMkdWorkService: IManagingOrgMkdWorkService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавить работы по МКД
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        public IDataResult AddMkdWorks(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var mkdWorkRepository = this.Container.ResolveDomain<ContentRepairMkdWork>();
                var manOrgServiceRepository = this.Container.ResolveDomain<ManOrgBilWorkService>();
                var manOrgMkdWorkRepository = this.Container.ResolveDomain<ManOrgBilMkdWork>();

                try
                {
                    var serviceWorkId = baseParams.Params.GetAs<long>("serviceWorkId"); // идентификатор услуги управляющей организации
                    var mkdWorkIds = baseParams.Params.GetAs("mkdWorkIds", new long[0]); // идентификаторы выбранных работ по МКД
                    
                    var manOrgService = manOrgServiceRepository.Get(serviceWorkId); // услуга управляющей организации

                    var existingMkdWorkIds = manOrgMkdWorkRepository.GetAll()
                        .Where(x => x.WorkService.Id == serviceWorkId)
                        .Select(x => x.MkdWork.Id)
                        .Distinct()
                        .ToArray(); // идентификаторы работ по МКД, которые уже были добавлены

                    var newMkdWorkIds = mkdWorkIds.Except(existingMkdWorkIds); // идентификаторы новых работ по МКД

                    foreach (var newId in newMkdWorkIds)
                    {
                        var newManOrgBilMkdWork = new ManOrgBilMkdWork
                        {
                            MkdWork = mkdWorkRepository.Get(newId),
                            WorkService = manOrgService
                        };

                        manOrgMkdWorkRepository.Save(newManOrgBilMkdWork);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = exception.Message };
                }
                finally 
                {
                    this.Container.Release(mkdWorkRepository);
                    this.Container.Release(manOrgServiceRepository);
                    this.Container.Release(manOrgMkdWorkRepository);
                }
            }
        }
    }
}
