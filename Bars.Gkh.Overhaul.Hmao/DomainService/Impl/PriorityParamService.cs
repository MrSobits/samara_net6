namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities;
    using PriorityParams;

    public class PriorityParamService : IPriorityParamService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult List(BaseParams baseParams)
        {
            var result =
                Container.ResolveAll<IPriorityParams>()
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        Type = x.TypeParam
                    })
                    .OrderBy(x => x.Type)
                    .ThenBy(x => x.Name)
                    .ToList();

            return new ListDataResult(result, result.Count);
        }

        public IDataResult SaveQuantParams(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<QuantPriorityParam>>();

            InTransaction(() =>
            {
                var records = baseParams.Params.GetAs<List<QuantPriorityParam>>("records");

                foreach (var record in records)
                {
                    if (record.Id > 0)
                    {
                        service.Update(record);
                    }
                    else
                    {
                        service.Save(record);
                    }
                }
            });

            return new BaseDataResult();
        }

        public IDataResult SaveQualityParams(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<QualityPriorityParam>>();

            InTransaction(() =>
            {
                var records = baseParams.Params.GetAs<List<QualityPriorityParam>>("records");

                foreach (var record in records)
                {
                    if (record.Id > 0)
                    {
                        service.Update(record);
                    }
                    else
                    {
                        service.Save(record);
                    }
                }
            });

            return new BaseDataResult();
        }

        private void InTransaction(Action action)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
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