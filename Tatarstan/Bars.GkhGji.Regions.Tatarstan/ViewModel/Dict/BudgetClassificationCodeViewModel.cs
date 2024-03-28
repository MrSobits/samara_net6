namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode;

    public class BudgetClassificationCodeViewModel : BaseViewModel<BudgetClassificationCode>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<BudgetClassificationCode> domainService, BaseParams baseParams)
        {
            var kbkMunicipalityDomain = this.Container.ResolveDomain<BudgetClassificationCodeMunicipality>();
            var kbkArticleLawDomain = this.Container.ResolveDomain<BudgetClassificationCodeArticleLaw>();

            using (this.Container.Using(kbkMunicipalityDomain, kbkArticleLawDomain))
            {
                var kbkMunicipalitiesDict = kbkMunicipalityDomain.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.BudgetClassificationCode.Id)
                    .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.Municipality.Name)));

                var kbkArticleLawDict = kbkArticleLawDomain.GetAll()
                    .GroupBy(x => x.BudgetClassificationCode.Id)
                    .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.ArticleLaw.Name)));
                
                return domainService.GetAll()
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Kbk,
                        Municipalities = kbkMunicipalitiesDict.ContainsKey(x.Id)
                            ? kbkMunicipalitiesDict[x.Id]
                            : null,
                        ArticleLaw = kbkArticleLawDict.Get(x.Id),
                        x.StartDate,
                        x.EndDate
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<BudgetClassificationCode> domainService, BaseParams baseParams)
        {
            var kbkArticlesLawDomain = this.Container.Resolve<IDomainService<BudgetClassificationCodeArticleLaw>>();
            using (this.Container.Using(kbkArticlesLawDomain))
            {
                var id = baseParams.Params.GetAsId();
                var kbkArticleLawDict = kbkArticlesLawDomain.GetAll()
                    .GroupBy(x => x.BudgetClassificationCode.Id)
                    .ToDictionary(x => x.Key, 
                        x => x.Select(y => new { y.ArticleLaw.Id, y.ArticleLaw.Name }));

                var result = domainService.GetAll()
                    .Where(x => x.Id == id)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Kbk,
                        x.StartDate,
                        x.EndDate,
                        ArticleLaw = kbkArticleLawDict.Get(id),
                    })
                    .SingleOrDefault();

                return new BaseDataResult(result);
            }
        }
    }
}
