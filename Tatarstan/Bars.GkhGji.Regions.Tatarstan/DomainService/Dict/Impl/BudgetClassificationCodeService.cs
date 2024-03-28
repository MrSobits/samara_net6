namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode;

    using Castle.Windsor;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис для работы с КБК
    /// </summary>
    public class BudgetClassificationCodeService : IBudgetClassificationCodeService
    {
        private readonly IDomainService<BudgetClassificationCodeMunicipality> budgetClassificationCodeMunicipalityDomain;
        private readonly IDomainService<BudgetClassificationCode> budgetClassificationCodeDomain;
        private readonly IDomainService<BudgetClassificationCodeArticleLaw> budgetClassificationCodeArticleLawDomain;
        private readonly IWindsorContainer container;

        public BudgetClassificationCodeService(IDomainService<BudgetClassificationCodeMunicipality> budgetClassificationCodeMunicipalityDomain, 
            IDomainService<BudgetClassificationCode> budgetClassificationCodeDomain,
            IDomainService<BudgetClassificationCodeArticleLaw> budgetClassificationCodeArticleLawDomain,
            IWindsorContainer container)
        {
            this.budgetClassificationCodeMunicipalityDomain = budgetClassificationCodeMunicipalityDomain;
            this.budgetClassificationCodeDomain = budgetClassificationCodeDomain;
            this.budgetClassificationCodeArticleLawDomain = budgetClassificationCodeArticleLawDomain;
            this.container = container;
        }

        /// <inheritdoc />
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var kbkId = baseParams.Params.GetAsId("kbkId");
            if (kbkId == default(long))
            {
                return new BaseDataResult(false, "Некорректные данные");
            }
            
            var municipalities = this.budgetClassificationCodeMunicipalityDomain.GetAll()
                .Where(x => x.BudgetClassificationCode.Id == kbkId)
                .Select(x => x.Municipality)
                .ToList();

            return new BaseDataResult(new
            {
                municipalitiesIds = string.Join(", ", municipalities.Select(x => x.Id)),
                municipalitiesNames = string.Join(", ", municipalities.Select(x => x.Name))
            });
        }

        /// <inheritdoc />
        public IDataResult SaveMunicipalities(BaseParams baseParams)
        {
            var kbkId = baseParams.Params.GetAsId("kbkId");
            if (kbkId == default(long))
            {
                return new BaseDataResult(false, "Некорректные данные");
            }

            var municipalitiesIds = baseParams.Params.GetAs<string>("municipalitiesIds").ToLongArray();

            var existsMunicipalities = this.budgetClassificationCodeMunicipalityDomain.GetAll()
                .Where(x => x.BudgetClassificationCode.Id == kbkId)
                .Select(x => new
                {
                    x.Id,
                    MunicipalityId = x.Municipality.Id
                })
                .ToDictionary(x => x.Id, x => x.MunicipalityId);

            var municipalitiesForAdd = municipalitiesIds.Where(x => !existsMunicipalities.Values.Contains(x));
            var kbkMunicipalitiesForDelete = existsMunicipalities.Where(x => !municipalitiesIds.Contains(x.Value))
                .Select(x => x.Key);


            kbkMunicipalitiesForDelete
                .ForEach(x => this.budgetClassificationCodeMunicipalityDomain.Delete(x));

            municipalitiesForAdd
                .ForEach(x => 
                this.budgetClassificationCodeMunicipalityDomain.Save(new BudgetClassificationCodeMunicipality
                {
                    BudgetClassificationCode = new BudgetClassificationCode { Id = kbkId },
                    Municipality = new Municipality { Id = x }
                }));

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult SaveOrUpdate(BaseParams baseParams)
        {
            using (var transaction = this.container.Resolve<IDataTransaction>())
            {
                try
                {
                    var kbkId = baseParams.Params.GetAsId();

                    var dataResult = kbkId == default 
                        ? this.budgetClassificationCodeDomain.Save(baseParams) 
                        : this.budgetClassificationCodeDomain.Update(baseParams);

                    if (!dataResult.Success)
                    {
                        return dataResult;
                    }

                    kbkId = kbkId == default
                        ? ((List<BudgetClassificationCode>)dataResult.Data).Single().Id
                        : kbkId;

                    this.SaveArticlesLaw(kbkId, baseParams);

                    transaction.Commit();
                    return dataResult;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void SaveArticlesLaw(long kbkId, BaseParams baseParams)
        {
            var articlesLawObj = baseParams.Params.GetAs<Dictionary<string, object>[]>("records").Single().Get("ArticleLaw") as List<object>;
            var articlesLaw = articlesLawObj.Select(x => (long)x).ToList();
            if (articlesLaw == null || !articlesLaw.Any())
            {
                return;
            }

            var existingArticlesLawIds = this.budgetClassificationCodeArticleLawDomain.GetAll()
                .Where(x => x.BudgetClassificationCode.Id == kbkId)
                .Select(x => x.ArticleLaw.Id)
                .ToList();

            var articlesLawForAdd = articlesLaw
                .Where(x => !existingArticlesLawIds.Contains(x))
                .Select(x => new BudgetClassificationCodeArticleLaw
                {
                    BudgetClassificationCode = new BudgetClassificationCode { Id = kbkId },
                    ArticleLaw = new ArticleLawGji { Id = x }
                });
            var articlesLawForDelete = existingArticlesLawIds.Where(x => !articlesLaw.Contains(x));

            this.budgetClassificationCodeArticleLawDomain.GetAll()
                .Where(x => x.BudgetClassificationCode.Id == kbkId && articlesLawForDelete.Contains(x.ArticleLaw.Id))
                .Delete();

            foreach (var kbkArticleLaw in articlesLawForAdd)
            {
                this.budgetClassificationCodeArticleLawDomain.Save(kbkArticleLaw);
            }
        }
    }
}
