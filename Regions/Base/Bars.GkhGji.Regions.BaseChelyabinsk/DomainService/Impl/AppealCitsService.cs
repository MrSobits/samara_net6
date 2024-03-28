namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    using Converter = Bars.B4.DomainService.BaseParams.Converter;

    public class AppealCitsService : GkhGji.DomainService.AppealCitsService<ViewAppealCitizensBaseChelyabinsk>
    {
        public IQueryable<ViewAppealCitizensBaseChelyabinsk> AddUserFilters(IQueryable<ViewAppealCitizensBaseChelyabinsk> query)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceAppealCitsExecutant = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();

            try
            {
                var municipalityList = userManager.GetMunicipalityIds();
                var inspectorsList = userManager.GetInspectorIds();
                inspectorsList.Clear();//убираем проверку на инспекторов
                return query.WhereIf(
                        municipalityList.Count > 0,
                        x => municipalityList.Contains((long)x.MunicipalityId) || !x.MunicipalityId.HasValue)
                         .WhereIf(
                             inspectorsList.Count > 0,
                             x =>
                             serviceAppealCitsExecutant.GetAll()
                                                       .Any(
                                                           y =>
                                                           inspectorsList.Contains(y.Author.Id)
                                                           || inspectorsList.Contains(y.Executant.Id)
                                                           || inspectorsList.Contains(y.Controller.Id)));
            }
            finally
            {

                this.Container.Release(userManager);
                this.Container.Release(serviceAppealCitsExecutant);
            }
        }

        public override IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var serviceAppealCitizensGji = Container.Resolve<IDomainService<ViewAppealCitizensBaseChelyabinsk>>();
            var serviceAppealCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var relatedAppealDomain = Container.Resolve<IDomainService<RelatedAppealCits>>();
            var subjectDomainService = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var executantDomainService = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var acroDomainService = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var appealCitsSourceService = Container.Resolve<IDomainService<AppealCitsSource>>();
            var appealCitsDomain = Container.Resolve<IDomainService<AppealCits>>();
            var appealCitsStatSubjectService = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();

            try
            {
                var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
                var ids = baseParams.Params.ContainsKey("Id") ? baseParams.Params["Id"].ToStr() : string.Empty;

                var listIds = new List<long>();
                if (!string.IsNullOrEmpty(ids))
                {
                    listIds = ids.Split(',').Select(id => id.ToLong()).ToList();
                }

                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var authorId = baseParams.Params.GetAs<long>("authorId");

                var executantIdStr = baseParams.Params.GetAs<string>("executantId");
                var showAllExecutants = executantIdStr?.Contains("All") ?? false;
                var executantIds = showAllExecutants ? new long[0] : executantIdStr.ToLongArray();

                var controllerIdStr = baseParams.Params.GetAs<string>("controllerId");
                var showAllControllers = controllerIdStr?.Contains("All") ?? false;
                var controllerIds = showAllControllers ? new long[0] : controllerIdStr.ToLongArray();

                var dateFromStart = baseParams.Params.GetAs("dateFromStart", DateTime.Now.AddYears(-1));
                var dateFromEnd = baseParams.Params.GetAs("dateFromEnd", DateTime.MinValue);
                var isShowOnlyFromEais = baseParams.Params.GetAs("showOnlyFromEais", false);
                var isShowFavorites = baseParams.Params.GetAs("showFavorites", false);

                var checkTimeStart = baseParams.Params.GetAs("checkTimeStart", DateTime.MinValue);
                var checkTimeEnd = baseParams.Params.GetAs("checkTimeEnd", DateTime.MinValue);
                var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);
                var relatesToId = baseParams.Params.GetAs<long>("relatesToId");
                var matchRelated = baseParams.Params.GetAs<bool>("matchRelated");
                var showExtensTimes = baseParams.Params.GetAs("showExtensTimes", false);

                var statSubsubjectGjiId = baseParams.Params.GetAs<long>("statSubsubjectGjiId");
                var statSubjectId = baseParams.Params.GetAs<long>("statSubjectId");

                if (dateFromStart != DateTime.MinValue)
                {
                    dateFromStart = dateFromStart.AddDays(-1);
                }

                if (checkTimeStart != DateTime.MinValue)
                {
                    checkTimeStart = checkTimeStart.AddDays(-1);
                }

                if (dateFromEnd != DateTime.MinValue)
                {
                    dateFromEnd = dateFromEnd.AddDays(1);
                }

                if (checkTimeEnd != DateTime.MinValue)
                {
                    checkTimeEnd = checkTimeEnd.AddDays(1);
                }

                long[] relatedIds = null;
                string[] relatesToAddress = null;
                string[] relatesToMunicipalityFiasId = null;

                var appCitsWithFavoritesIds = appealCitsStatSubjectService.GetAll()
                        .Where(x => x.Subject.TrackAppealCits == true ||
                        x.Subsubject.TrackAppealCits == true ||
                        x.Feature.TrackAppealCits == true)
                        .Select(x => x.AppealCits.Id)
                        .Distinct();

                List<long> appealsByStatsub = new List<long>();
                if (statSubjectId > 0 || statSubsubjectGjiId > 0)
                {

                    appealsByStatsub = appealCitsStatSubjectService.GetAll()
                     .Where(x => x.AppealCits.DateFrom >= dateFromStart)
                     .WhereIf(statSubjectId > 0, x => x.Subject != null)
                     .WhereIf(statSubsubjectGjiId > 0, x => x.Subsubject != null)
                     .WhereIf(statSubjectId > 0, x => x.Subject.Id == statSubjectId)
                     .WhereIf(statSubsubjectGjiId > 0, x => x.Subsubject.Id == statSubsubjectGjiId)
                     .Select(x => x.AppealCits.Id)
                     .ToList();
                }

                if (relatesToId > 0)
                {
                    var related = relatedAppealDomain.GetAll()
                        .Where(rac => rac.Parent.Id == relatesToId)
                        .Select(rac => rac.Children)
                        .ToArray();
                    relatedIds = related.Select(rac => rac.Id).ToArray();

                    if (matchRelated)
                    {
                        var relatesToAttributes = acroDomainService.GetAll()
                            .Where(acro => acro.AppealCits.Id == relatesToId)
                            .Select(acro => new
                            {
                                acro.RealityObject.Municipality.FiasId,
                                acro.RealityObject.Address
                            })
                            .ToArray();

                        relatesToAddress = relatesToAttributes.Select(i => i.Address).ToArray();
                        relatesToMunicipalityFiasId = relatesToAttributes.Select(i => i.FiasId).ToArray();
                    }
                }

                string[] filterForExecutants = null;
                string[] filterForControllers = null;
                string[] filterForRevenueSourceNames = null;
                string[] revenueSourceDates = null;
                string[] revenueSourceNumbers = null;
                if (loadParams.DataFilter?.Filters != null && loadParams.DataFilter.Filters.Any())
                {
                    var dataFilterForExecutants = loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "Executants");
                    var dataFilterForControllers = loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "Controllers");
                    var dataFilterForRevenueSourceNames = loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceNames");
                    var dataFilterForRevenueSourceDates = loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceDates");
                    var dataFilterForRevenueSourceNumbers = loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceNumbers");

                    if (dataFilterForExecutants != null && dataFilterForExecutants.Value.IsNotEmpty())
                    {
                        filterForExecutants = dataFilterForExecutants.Value.Trim().Replace(" ", "").ToLower().Split(",");
                    }
                    if (dataFilterForControllers != null && dataFilterForControllers.Value.IsNotEmpty())
                    {
                        filterForControllers = dataFilterForControllers.Value.Trim().Replace(" ", "").ToLower().Split(",");
                    }
                    if (dataFilterForRevenueSourceNames != null && dataFilterForRevenueSourceNames.Value.IsNotEmpty())
                    {
                        filterForRevenueSourceNames = dataFilterForRevenueSourceNames.Value.Trim().Replace(" ", "").ToLower().Split(",");
                    }
                    if (dataFilterForRevenueSourceDates != null && dataFilterForRevenueSourceDates.Value.IsNotEmpty())
                    {
                        revenueSourceDates = dataFilterForRevenueSourceDates.Value.Trim().Replace(" ", "").ToLower().Split(",");
                    }
                    if (dataFilterForRevenueSourceNumbers != null && dataFilterForRevenueSourceNumbers.Value.IsNotEmpty())
                    {
                        revenueSourceNumbers = dataFilterForRevenueSourceNumbers.Value.Trim().Replace(" ", "").ToLower().Split(",");
                    }

                    loadParams.DataFilter.Filters = loadParams.DataFilter.Filters.Where(f => !new[] { "Executants", "Controllers", "RevenueSourceNames", "RevenueSourceDates", "RevenueSourceNumbers" }.Contains(f.DataIndex));
                }

                var executantAppealCitsIdsQuery = filterForExecutants.IsNotEmpty() ? executantDomainService.GetAll()
                    .Where(x => filterForExecutants.Contains(x.Executant.Fio.Replace(" ", "").ToLower()) || x.Executant.Fio.Replace(" ", "").ToLower().Contains(filterForExecutants[0]))
                    .Select(x => x.AppealCits.Id) : null;

                var controllerAppealCitsIdsQuery = filterForControllers.IsNotEmpty() ? executantDomainService.GetAll()
                    .Where(x => filterForControllers.Contains(x.Controller.Fio.Replace(" ", "").ToLower()) || x.Controller.Fio.Replace(" ", "").ToLower().Contains(filterForControllers[0]))
                    .Select(x => x.AppealCits.Id) : null;

                //var query = this.ApplyFilters(this.AddUserFilters(serviceAppealCitizensGji.GetAll()), baseParams)
                //    .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                //    .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                //    .WhereIf(
                //        realityObjectId > 0,
                //        x => x.RealityObjectIds.Contains($"/{realityObjectId}/"))
                //    .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom > dateFromStart)
                //    .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                //    .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime > checkTimeStart)
                //    .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                //    .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                //    .WhereIf(showExtensTimes, x => x.ExtensTime != null)
                //    .WhereIf(
                //        authorId > 0,
                //        x => serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id && y.Author.Id == authorId))
                //    .WhereIf(showAllExecutants || executantIds.IsNotEmpty(),
                //        x => serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id && (showAllExecutants || executantIds.Contains(y.Executant.Id))))
                //    .WhereIf(showAllControllers || controllerIds.IsNotEmpty(),
                //        x => serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id && (showAllControllers || controllerIds.Contains(y.Controller.Id))))
                //    .WhereIf(relatesToId > 0 && !matchRelated, x => relatedIds.Contains(x.Id))
                //    .WhereIf(relatesToId > 0 && matchRelated, x => !relatedIds.Contains(x.Id))
                //    .WhereIf(
                //        matchRelated,
                //        x => acroDomainService.GetAll().Where(acro => acro.AppealCits.Id == x.Id)
                //            .Any(acro => relatesToAddress.Contains(acro.RealityObject.Address)
                //                && relatesToMunicipalityFiasId.Contains(acro.RealityObject.Municipality.FiasId)) && x.Id != relatesToId)
                //    .WhereIf(executantAppealCitsIdsQuery != null, x => executantAppealCitsIdsQuery.Any(y => y == x.Id))
                //    .WhereIf(controllerAppealCitsIdsQuery != null, x => controllerAppealCitsIdsQuery.Any(y => y == x.Id))
                //    .Select(
                //        x =>
                //        new
                //        {
                //            x.Id,
                //            x.State,
                //            Name = $"{x.Number} ({x.NumberGji})",
                //            ManagingOrganization = x.ContragentName,
                //            x.Number,
                //            x.NumberGji,
                //            x.DateFrom,
                //            x.CheckTime,
                //            x.QuestionsCount,
                //            x.Municipality,
                //            x.CountRealtyObj,
                //            Executant = x.Executant != null ? x.Executant.Fio : string.Empty,
                //            Tester = x.Tester.Fio,
                //            SuretyResolve = x.SuretyResolve.Name ?? "",
                //            x.ExecuteDate,
                //            x.SuretyDate,
                //            x.ZonalInspection,
                //            ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : string.Empty,
                //            x.Correspondent,
                //            x.CorrespondentAddress,
                //            x.RealObjAddresses,
                //            x.AppealCits.SpecialControl,
                //            KindStatement = x.AppealCits.KindStatement != null ? x.AppealCits.KindStatement.Name : string.Empty,
                //            HasExecutant = serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id),
                //            x.ExtensTime,
                //            x.AppealCits.Description,
                //            Subjects = x.SubjectsName,
                //            SubSubjects = x.SubSubjectsName,
                //            Features = x.FeaturesName,
                //            Executants = x.SubjectExecutantsFio,
                //            Controllers = x.ControllersFio,
                //            x.AppealCits.SSTUExportState,
                //            x.AnswerDate,
                //            x.RevenueSourceNames,
                //            x.RevenueSourceDates,
                //            x.RevenueSourceNumbers
                //        })
                //    .Filter(loadParams, this.Container);
                List<long> acrolist = new List<long>();
                if (realityObjectId > 0)
                {
                    acrolist = acroDomainService.GetAll().Where(acro => acro.RealityObject.Id == realityObjectId).Select(acro => acro.AppealCits.Id).ToList();
                }

                var query2 = appealCitsDomain.GetAll()
                   .WhereIf(isShowFavorites, x => appCitsWithFavoritesIds.Contains(x.Id))
                   .WhereIf(appealsByStatsub.Count > 0, x => appealsByStatsub.Contains(x.Id))
                   .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                   .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                   .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom > dateFromStart)
                   .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                   .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime > checkTimeStart)
                   .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                   .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                   .WhereIf(isShowOnlyFromEais, x=> x.AppealUid.HasValue)
                   .WhereIf(showExtensTimes, x => x.ExtensTime != null)
                   .WhereIf(realityObjectId > 0, x => acrolist.Contains(x.Id))
                   .WhereIf(
                       authorId > 0,
                       x => serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id && y.Author.Id == authorId))
                   .WhereIf(showAllExecutants || executantIds.IsNotEmpty(),
                       x => serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id && (showAllExecutants || executantIds.Contains(y.Executant.Id))))
                     .WhereIf(
                           controllerIds.Length > 0,
                           x => x.OrderContragent != null && controllerIds.ToList().Contains(x.OrderContragent.Id))
                   .WhereIf(relatesToId > 0 && !matchRelated, x => relatedIds.Contains(x.Id))
                   .WhereIf(relatesToId > 0 && matchRelated, x => !relatedIds.Contains(x.Id))
                   .WhereIf(
                       matchRelated,
                       x => acroDomainService.GetAll().Where(acro => acro.AppealCits.Id == x.Id)
                           .Any(acro => relatesToAddress.Contains(acro.RealityObject.Address)
                               && relatesToMunicipalityFiasId.Contains(acro.RealityObject.Municipality.FiasId)) && x.Id != relatesToId)
                   .WhereIf(executantAppealCitsIdsQuery != null, x => executantAppealCitsIdsQuery.Any(y => y == x.Id))
                   .WhereIf(controllerAppealCitsIdsQuery != null, x => controllerAppealCitsIdsQuery.Any(y => y == x.Id))
                   .Select(
                       x =>
                       new
                       {
                           x.Id,
                           x.State,
                           Name = $"{x.Number} ({x.NumberGji})",
                           ManagingOrganization = x.ManagingOrganization != null ? x.ManagingOrganization.Contragent.Name : "",
                           x.Number,
                           x.NumberGji,
                           x.DateFrom,
                           x.CheckTime,
                           x.QuestionsCount,
                           OrderContragent = x.OrderContragent != null? x.OrderContragent.Name:"",
                           StateCode = x.State.Code,
                           Executant = x.Executant != null ? x.Executant.Fio : string.Empty,
                           Tester = x.Tester != null ? x.Tester.Fio : "",
                           SuretyResolve = x.SuretyResolve != null ? x.SuretyResolve.Name : "",
                           x.ExecuteDate,
                           x.SuretyDate,
                           x.ZonalInspection,
                           x.CaseDate,
                           ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : string.Empty,
                           x.Correspondent,
                           x.CorrespondentAddress,
                           x.SpecialControl,
                           KindStatement = x.KindStatement != null ? x.KindStatement.Name : string.Empty,
                           HasExecutant = serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id),
                           x.ExtensTime,
                           x.Description,//=ReplaceInvalidXmlCharacterReferences(!string.IsNullOrEmpty(x.Description)? x.Description:""),
                           x.Municipality,
                           x.MunicipalityId,
                           x.AppealUid,
                           //Subjects = x.SubjectsName,
                           //SubSubjects = x.SubSubjectsName,
                           //Features = x.FeaturesName,
                           x.Executors,
                           x.Testers,
                           //Controllers = x.ControllersFio,
                           x.SSTUExportState,
                           x.AnswerDate,
                           x.RealityAddresses,
                           x.IncomingSources,
                           x.IncomingSourcesName,
                           x.ControlDateGisGkh,
                           FavoriteFilter = isShowFavorites,
                           FavoriteFilterCondition = isShowFavorites ? FavoriteFilterCheck(x.Id) : false,
                           x.FastTrack
                           //x.RevenueSourceDates,
                           //x.RevenueSourceNumbers
                       }).Filter(loadParams, this.Container); ;
               //var query2 = query
               //        .Select(x=> new
               //        {
               //            x.Id,
               //            x.AnswerDate,
               //            x.AppealUid,
               //            x.CheckTime,
               //            x.Correspondent,
               //            x.CorrespondentAddress,
               //            x.DateFrom,
               //            x.Description,
               //            x.Executant,
               //            x.Executants,
               //            x.ExecutantsFio,
               //            x.ExecuteDate,
               //            x.ExtensTime,
               //            x.HasExecutant,
               //            x.KindStatement,
               //            x.ManagingOrganization,
               //            x.Name,
               //            x.Number,
               //            x.NumberGji,
               //            x.QuestionsCount,
               //            x.RealObjAddresses,
               //            x.RevenueSourceNames,
               //            x.SpecialControl,
               //            x.SSTUExportState,
               //            x.State,
               //            x.SuretyDate,
               //            x.SuretyResolve,
               //            x.Tester,
               //            x.ZonalInspection,
               //            x.ZoneName
               //        })
                 

                totalCount = query2.Count();

                query2 = query2
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.DateFrom)
                    .Order(loadParams);

                if (usePaging)
                {
                    query2 = query2.Paging(loadParams);
                }

                return query2.ToList();
            }
            finally
            {
                this.Container.Release(serviceAppealCitizensGji);
                this.Container.Release(serviceAppealCitsExecutant);
                this.Container.Release(relatedAppealDomain);
                this.Container.Release(subjectDomainService);
                this.Container.Release(executantDomainService);
                this.Container.Release(acroDomainService);
                this.Container.Release(appealCitsSourceService);
                this.Container.Release(appealCitsStatSubjectService);
            }
        }

        private static readonly Regex EmojiRegex = new Regex("&#x?[A-Fa-f0-9]+;");
        private static string ReplaceInvalidXmlCharacterReferences(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                    return "";
                if (input.IndexOf("🙏") == -1)
                    return input;

                return input.Replace("🙏", "!");
            }
            catch (Exception e)
            {
                return "";
            }
        }

        private bool FavoriteFilterCheck(long acId)
        {
            var inspAppealService = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var actRemovalService = Container.Resolve<IDomainService<ActRemoval>>();
            var appealOrderService = Container.Resolve<IDomainService<AppealOrder>>();

            try
            {
                //Сразу проверяем принятый инспектором ответ, т.к. это быстрее
                var isConfirmedOrders = appealOrderService.GetAll()
                    .Where(x => x.AppealCits.Id == acId)
                    .Select(x => x.Confirmed)
                    .ToList();
                foreach(var isConfirmedOrder in isConfirmedOrders)
                {
                    if (isConfirmedOrder == Gkh.Enums.YesNoNotSet.Yes)
                    {
                        return true;
                    }
                }

                //Проверяем акты устранений, если проверок нет, сразу false для ускорения
                var inspIds = inspAppealService.GetAll()
                    .Where(x => x.Id == acId)
                    .Select(x => x.Inspection.Id)
                    .ToList();
                if (inspIds.Count <= 0)
                {
                    return false;
                }

                var isRemovedViols = actRemovalService.GetAll()
                    .Where(x => inspIds.Contains(x.Inspection.Id))
                    .Select(x => x.TypeRemoval)
                    .ToList();
                foreach (var isRemoved in isRemovedViols)
                {
                    if (isRemoved == Gkh.Enums.YesNoNotSet.Yes)
                    {
                        return true;
                    }
                }
            }
            finally
            {
                Container.Release(inspAppealService);
                Container.Release(appealOrderService);
                Container.Release(actRemovalService);
            }

            return false;
        }
    }
}
