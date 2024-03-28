namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис экспорта обращений граждан
    /// </summary>
    public class HabarovskExportSuggestionService : IExportSuggestionService
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
            var stateDomain = this.Container.ResolveDomain<State>();
            var sZonalInspectionDomain = this.Container.ResolveDomain<ZonalInspection>();
            var citizenSuggestionDomain = this.Container.ResolveDomain<CitizenSuggestion>();
            var appealCitsCitSuggestationDomain = this.Container.ResolveDomain<AppealCitsCitizenSuggestion>();
            var appealCitsRealityObjectDomain = this.Container.ResolveDomain<AppealCitsRealityObject>();

            using (this.Container.Using(appealCitsDomain, appealCitsCitSuggestationDomain, citizenSuggestionDomain, appealCitsRealityObjectDomain))
            {
                if (appealCitsCitSuggestationDomain.GetAll().Any(x => x.CitizenSuggestion.Id == id))
                {
                    return BaseDataResult.Error("Обращение уже экспортировано реестр ГЖИ");
                }

                var citizenSugg = citizenSuggestionDomain.GetAll().First(x => x.Id == id);

                var zonal = sZonalInspectionDomain.GetAll()
                    .Where(x => x.Name == "Государственная жилищная инспекция Воронежской области").FirstOrDefault();


                NhExtentions.InTransaction(this.Container, () =>
                {
                    var cits = new AppealCits
                    {
                        Correspondent = baseParams.Params.GetAs<string>("ApplicantFio"),
                        CorrespondentAddress = baseParams.Params.GetAs<string>("ApplicantAddress"),
                        Phone = baseParams.Params.GetAs<string>("ApplicantPhone"),
                        Email = baseParams.Params.GetAs<string>("ApplicantEmail"),
                        Description = baseParams.Params.GetAs<string>("Description"),
                        ZonalInspection = zonal != null? zonal: citizenSugg.ExecutorZonalInspection
                    };

                    try
                    {
                        var appDate = citizenSugg.CreationDate;
                        var prodCalendarContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                            .Where(x => x.ProdDate >= appDate && x.ProdDate <= appDate.AddDays(38)).Select(x => x.ProdDate).ToList();

                        DateTime newControlDate = DateTime.Now;

                        //int sartudaysCount = CountDays(DayOfWeek.Saturday, appDate.Value, appDate.Value.AddDays(28));
                        //int sundaysCount = CountDays(DayOfWeek.Sunday, appDate.Value, appDate.Value.AddDays(28));
                        //newControlDate = appDate.Value.AddDays(28 + sartudaysCount + sundaysCount);
                        newControlDate = appDate.AddDays(27);

                        //int celebrDatesCount = 0;
                        //foreach (DateTime dt in prodCalendarContainer)
                        //{
                        //    if (dt <= newControlDate)
                        //    {
                        //        celebrDatesCount++;
                        //    }
                        //}
                        // newControlDate = newControlDate.AddDays(celebrDatesCount);
                        if (prodCalendarContainer.Contains(newControlDate))
                        {
                            for (int i = 0; i <= prodCalendarContainer.Count; i++)
                            {
                                if (prodCalendarContainer.Contains(newControlDate))
                                {
                                    newControlDate = newControlDate.AddDays(-1);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        if (newControlDate.DayOfWeek == DayOfWeek.Saturday)
                        {
                            newControlDate = newControlDate.AddDays(-1);
                        }
                        else if (newControlDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            newControlDate = newControlDate.AddDays(-2);
                        }
                        //если стоит контрольный срок, расчетный не ставится
                       
                         cits.CheckTime = newControlDate;
                        
                    }
                    catch
                    {
                        var appDate = citizenSugg.CreationDate;
                        cits.CheckTime = appDate.AddDays(38);
                    }

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

                    var stateTransferred = stateDomain.GetAll()
                    .Where(x => x.TypeId == "gkh_citizen_suggestion" && x.Code == "02").FirstOrDefault();
                    if (stateTransferred != null)
                    {
                        citizenSugg.State = stateTransferred;
                        citizenSuggestionDomain.Update(citizenSugg);
                    }
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