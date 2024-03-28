namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;

    using Castle.Windsor;
    using Entities;
    using Enum;

    public class WorkService : IWorkService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SaveWithFinanceType(BaseParams baseParams, IDomainService<Work> service)
        {
            Work work = null;

            InTransaction(() =>
            {
                work = ((List<Work>)service.Save(baseParams).Data)[0];
                var finSources = baseParams.Params.GetAs<long[]>("FinSources");
                UpdateWorkFinSources(finSources, work);
            });

            return new BaseDataResult(work.ReturnSafe(w => w.Id));
        }

        public IDataResult UpdateWithFinanceType(BaseParams baseParams, IDomainService<Work> service)
        {
            InTransaction(() =>
            {
                var work = ((List<Work>)service.Update(baseParams).Data)[0];
                var finSources = baseParams.Params.GetAs<long[]>("FinSources");
                UpdateWorkFinSources(finSources, work);
            });
            return new BaseDataResult();
        }

        public IDataResult DeleteWithFinanceType(BaseParams baseParams, IDomainService<Work> service)
        {
            try
            {
                InTransaction(() =>
                {
                    var workIds = baseParams.Params.GetAs<long[]>("records", ignoreCase: true);
                    var finSourceService = Container.Resolve<IDomainService<WorkTypeFinSource>>();
                    foreach (var id in workIds)
                    {
                        var sources = finSourceService.GetAll().Where(x => x.Work.Id == id).ToList();
                        foreach (var s in sources)
                        {
                            finSourceService.Delete(s.Id);
                        }

                        service.Delete(id);
                    }
                });
            }
            catch (ValidationException exception)
            {
                return new BaseDataResult(false, string.Format("Существуют зависимые записи: {0}", exception.Message));
            }
            
            return new BaseDataResult();
        }

        private void UpdateWorkFinSources(long[] newFinSourceTypes, Work work)
        {
            var finSourceService = Container.Resolve<IDomainService<WorkTypeFinSource>>();
            var oldFinSources = finSourceService.GetAll().Where(x => x.Work.Id == work.Id);

            foreach (var type in Enum.GetValues(typeof(TypeFinSource)).Cast<TypeFinSource>())
            {
                if (newFinSourceTypes.Contains((int)type) && !oldFinSources.Select(x => x.TypeFinSource).ToList().Contains(type))
                {
                    finSourceService.Save(new WorkTypeFinSource
                    {
                        TypeFinSource = type,
                        Work = work
                    });
                }
                else if (!newFinSourceTypes.Contains((int)type) && oldFinSources.Select(x => x.TypeFinSource).ToList().Contains(type))
                {
                    var workTypeFinSource = oldFinSources.FirstOrDefault(x => x.TypeFinSource == type);
                    finSourceService.Delete(workTypeFinSource.Id);
                }
            }
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
