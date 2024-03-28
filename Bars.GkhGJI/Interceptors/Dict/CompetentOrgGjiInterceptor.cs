namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class CompetentOrgGjiInterceptor : EmptyDomainInterceptor<CompetentOrgGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<CompetentOrgGji> service, CompetentOrgGji entity)
        {
            //if (Container.Resolve<IDomainService<AppealCitsRequest>>().GetAll().Any(x => x.CompetentOrg.Id == entity.Id))
            //{
            //    return Failure("Существуют связанные записи в следующих таблицах: Запросы в обращениях граждан;");
            //}

            return this.Success();
        }
    }
}