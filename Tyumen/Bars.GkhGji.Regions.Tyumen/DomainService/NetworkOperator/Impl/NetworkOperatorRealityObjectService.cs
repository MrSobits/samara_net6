namespace Bars.GkhGji.Regions.Tyumen.DomainService.NetworkOperator.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tyumen.Entities;
    using Bars.GkhGji.Regions.Tyumen.Entities.Dicts;

    using Castle.Windsor;

    public class NetworkOperatorRealityObjectService : INetworOperatorRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<NetworkOperatorRealityObject> NopRoDomain { get; set; }

        public IDomainService<NetworkOperatorRealityObjectTechDecision> NopRoTDecDomain { get; set; }

        public IDataResult SaveTechDecisions(BaseParams baseParams)
        {
            InTransaction(() =>
            {
                var id = baseParams.Params.GetAs<long>("recordId");
                var entity = NopRoDomain.Get(id);
                var techDecisions = baseParams.Params.GetAs<string>("techDecisions").ToLongArray();
                var oldDecisions = NopRoTDecDomain.GetAll().Where(x => x.NetworkOperatorRealityObject.Id == entity.Id).ToArray();
                var toRemove = oldDecisions.Where(x => !techDecisions.Contains(x.TechDecision.Id)).ToArray();
                var toAdd = techDecisions.Where(x => oldDecisions.All(y => y.TechDecision.Id != x)).ToArray();
                foreach (var decision in toRemove)
                {
                    NopRoTDecDomain.Delete(decision.Id);
                }

                foreach (var techDecision in toAdd)
                {
                    NopRoTDecDomain.Save(new NetworkOperatorRealityObjectTechDecision
                    {
                        NetworkOperatorRealityObject = entity,
                        TechDecision = new TechDecision
                        {
                            Id = techDecision
                        }
                    });
                }
            });

            return new BaseDataResult();
        }

        /// <summary>Открыть транзакцию</summary>
        /// <returns>Экземпляр IDataTransaction</returns>
        protected virtual IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }

        protected virtual void InTransaction(Action action)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}