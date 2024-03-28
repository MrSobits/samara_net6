namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class BaseJurPersonContragentService : IBaseJurPersonContragentService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var service = this.Container.Resolve<IDomainService<Contragent>>();
            var locGovernmentService = this.Container.Resolve<IDomainService<LocalGovernment>>();
            var politicAuthorityService = this.Container.Resolve<IDomainService<PoliticAuthority>>();
            
            using (this.Container.Using(service, locGovernmentService, politicAuthorityService))
            {
                var data = service.GetAll()
                    .Where(x => politicAuthorityService.GetAll().Any(y => y.Contragent.Id == x.Id)
                                || locGovernmentService.GetAll().Any(y => y.Contragent.Id == x.Id))
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();
                var result = data.Order(loadParams).Paging(loadParams).ToList();

                return new ListDataResult(result, totalCount);
            }
        }

        public static string GetGeneratedSql(IQueryable queryable, NHibernate.ISession session)
        {
            var sessionImp = (NHibernate.Engine.ISessionImplementor)session;
            var nhLinqExpression = new NHibernate.Linq.NhLinqExpression(queryable.Expression, sessionImp.Factory);
            var translatorFactory = new NHibernate.Hql.Ast.ANTLR.ASTQueryTranslatorFactory();
            var translators = translatorFactory.CreateQueryTranslators(
                nhLinqExpression, 
                null, 
                false, 
                sessionImp.EnabledFilters, 
                sessionImp.Factory);

            return translators[0].SQLString;
        }

        public IDataResult AddContragents(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<BaseJurPersonContragent>>();
            var serviceJurPerson = this.Container.Resolve<IDomainService<BaseJurPerson>>();
            var serviceContragent = this.Container.Resolve<IDomainService<Contragent>>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {

                try
                {
                    var inspectionId = baseParams.Params.GetAs<long>("inspectionId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    var existRecs = service.GetAll()
                        .Where(x => x.BaseJurPerson.Id == inspectionId)
                        .Where(x => objectIds.Contains(x.Contragent.Id))
                        .Select(x => x.Contragent.Id)
                        .Distinct()
                        .ToList();

                    var inspection = serviceJurPerson.Load(inspectionId);

                    foreach (var id in objectIds)
                    {
                        // Если среди существующих контрагентов данной инспекции уже есть такой контрагент, то пролетаем мимо
                        if (existRecs.Contains(id))
                            continue;

                        // Если такого контрагента еще нет, то добавляем
                        var newObj = new BaseJurPersonContragent
                        {
                            BaseJurPerson = inspection,
                            Contragent = serviceContragent.Load(id)
                        };

                        service.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(service);
                    this.Container.Release(serviceJurPerson);
                    this.Container.Release(serviceContragent);
                }
            }
        }
    }
}