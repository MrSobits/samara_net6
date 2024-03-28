namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис экспорта обращений граждан
    /// </summary>
    public class ExportSuggestionService : IExportSuggestionService
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult ExportCitizenSuggestionToGji(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var appealCitsDomain = this.Container.ResolveDomain<AppealCits>();
            var citizenSuggestionDomain = this.Container.ResolveDomain<CitizenSuggestion>();
            var appealCitsCitSuggestationDomain = this.Container.ResolveDomain<AppealCitsCitizenSuggestion>();
            var appealCitsRealityObjectDomain = this.Container.ResolveDomain<AppealCitsRealityObject>();

            if (appealCitsCitSuggestationDomain.GetAll().Any(x => x.CitizenSuggestion.Id == id))
            {
                return BaseDataResult.Error("Обращение уже экспортировано реестр ГЖИ");
            }

            var citizenSugg = citizenSuggestionDomain.GetAll().First(x => x.Id == id);

            using (this.Container.Using(appealCitsDomain, appealCitsCitSuggestationDomain, citizenSuggestionDomain, appealCitsRealityObjectDomain))
            {
                NhExtentions.InTransaction(this.Container, () =>
                {
                    var cits = new AppealCits
                    {
                        Correspondent = baseParams.Params.GetAs<string>("ApplicantFio"),
                        CorrespondentAddress = baseParams.Params.GetAs<string>("ApplicantAddress"),
                        Phone = baseParams.Params.GetAs<string>("ApplicantPhone"),
                        Email = baseParams.Params.GetAs<string>("ApplicantEmail"),
                        Description = baseParams.Params.GetAs<string>("Description"),
                        ZonalInspection = citizenSugg.ExecutorZonalInspection,
                        DateFrom = citizenSugg.CreationDate,
                        DocumentNumber = citizenSugg.Number
                    };

                    appealCitsDomain.Save(cits);

                    appealCitsCitSuggestationDomain.Save(new AppealCitsCitizenSuggestion
                    {
                        AppealCits = cits,
                        CitizenSuggestion = citizenSuggestionDomain.Load(id)
                    });

                    appealCitsRealityObjectDomain.Save(new AppealCitsRealityObject
                        {
                            AppealCits = cits,
                            RealityObject = citizenSugg.RealityObject
                        });
                });
               
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult CheckSuggestionExists(BaseParams baseParams)
        {
            var appealCitsCitSuggestationDomain = this.Container.ResolveDomain<AppealCitsCitizenSuggestion>();

            using (this.Container.Using(appealCitsCitSuggestationDomain))
            {
                var id = baseParams.Params.GetAsId();
                var suggestionExists = appealCitsCitSuggestationDomain.GetAll().Any(x => x.CitizenSuggestion.Id == id);

                return new BaseDataResult((object)suggestionExists);
            }
        }
    }
}