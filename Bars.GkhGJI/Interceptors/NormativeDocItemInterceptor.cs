using Bars.B4.IoC;
using Bars.Gkh.Entities.Dicts;
using Bars.GkhGji.DomainService;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;

    public class NormativeDocItemInterceptor : EmptyDomainInterceptor<NormativeDocItem>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<NormativeDocItem> service, NormativeDocItem entity)
        {
            // Внимание если в дальнешем будет принято решение что нормативный документ можно удалять 
            // Неважно уществуют ли нарушения по нему или нет , то тогданужн оперегенерить поле NormativeDocNames в таблице ViolationGji
            var violNpdDomain = Container.Resolve<IDomainService<ViolationNormativeDocItemGji>>();

            using (Container.Using(violNpdDomain))
            {
                var listNpd = violNpdDomain.GetAll()
                        .Where(x => x.NormativeDocItem.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToList();

                if (listNpd.Any())
                {
                    return this.Failure("Данный пункт нормативного - документа содержатся в нарушениях ГЖИ .");
                }
            }

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<NormativeDocItem> service, NormativeDocItem entity)
        {

            var violationsDomain = this.Container.Resolve<IViolationNormativeDocItemService>();
            var npdDomain = this.Container.Resolve<IDomainService<NormativeDocItem>>();

            try
            {
                violationsDomain.UpdaeteViolationsByNpd(npdDomain.GetAll().Where(x => x.Id == entity.Id));

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(violationsDomain);
                Container.Release(npdDomain);
            }

            return Success();
        }
    }
}
