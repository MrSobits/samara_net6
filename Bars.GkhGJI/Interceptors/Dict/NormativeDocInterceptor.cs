namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities.Dict;

    public class NormativeDocInterceptor : Bars.Gkh.Interceptors.NormativeDocInterceptor
    {
        public override IDataResult BeforeDeleteAction(IDomainService<NormativeDoc> service, NormativeDoc entity)
        {
            // Внимание если в дальнешем будет принято решение что нормативный документ можно удалять 
            // Неважно уществуют ли нарушения по нему или нет , то тогданужн оперегенерить поле NormativeDocNames в таблице ViolationGji
            var violNpdDomain = this.Container.Resolve<IDomainService<ViolationNormativeDocItemGji>>();

            using (this.Container.Using(violNpdDomain))
            {
                return base.BeforeDeleteAction(service, entity).Success
                    ? violNpdDomain.GetAll().Any(x => x.NormativeDocItem.NormativeDoc.Id == entity.Id)
                        ? this.Failure("Пункты данного нормативного документа содержатся в нарушениях ГЖИ.")
                        : this.Success()
                    : this.Failure(base.BeforeDeleteAction(service, entity).Message);
            }
        }

        public override IDataResult AfterUpdateAction(IDomainService<NormativeDoc> service, NormativeDoc entity)
        {
            var violationsDomain = this.Container.Resolve<IViolationNormativeDocItemService>();
            var npdDomain = this.Container.Resolve<IDomainService<NormativeDocItem>>();

            using (this.Container.Using(violationsDomain, npdDomain))
            {
                return violationsDomain.UpdaeteViolationsByNpd(npdDomain.GetAll().Where(x => x.NormativeDoc.Id == entity.Id));
            }
        }
    }
}
