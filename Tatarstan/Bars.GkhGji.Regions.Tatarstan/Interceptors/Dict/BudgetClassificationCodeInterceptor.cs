namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode;

    using NHibernate.Linq;
    using NHibernate.Util;

    public class BudgetClassificationCodeInterceptor : EmptyDomainInterceptor<BudgetClassificationCode>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<BudgetClassificationCode> service, BudgetClassificationCode entity)
        {
            var kbkMunicipalitiesDomain = this.Container.ResolveDomain<BudgetClassificationCodeMunicipality>();
            var kbkArticleLawDomain = this.Container.ResolveDomain<BudgetClassificationCodeArticleLaw>();

            using (this.Container.Using(kbkMunicipalitiesDomain, kbkArticleLawDomain))
            {
                var kbkMunicipalitiesForDelete = kbkMunicipalitiesDomain.GetAll()
                    .Where(x => x.BudgetClassificationCode == entity)
                    .Select(x => x.Id)
                    .AsEnumerable();

                kbkMunicipalitiesForDelete.ForEach(x => kbkMunicipalitiesDomain.Delete(x));

                kbkArticleLawDomain.GetAll()
                    .Where(x => x.BudgetClassificationCode == entity)
                    .Delete();
            }
            return base.BeforeDeleteAction(service, entity);
        }
    }
}
