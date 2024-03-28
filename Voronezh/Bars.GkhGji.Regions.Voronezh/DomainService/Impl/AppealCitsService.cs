namespace Bars.GkhGji.Regions.Voronezh.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

    public class AppealCitsService : BaseChelyabinsk.DomainService.Impl.AppealCitsService
    {

        public IGkhUserManager UserManager { get; set; }
        /// <summary>
		/// Получить список обращений из view
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <param name="totalCount">Общее количество</param>
		/// <param name="usePaging">Использовать пагинацию?</param>
		/// <returns>Результат выполнения запроса</returns>
        public override IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var serviceAppealCitizensGji = this.Container.Resolve<IDomainService<ViewAppealCitizensBaseChelyabinsk>>();
            var relatedAppealDomain = this.Container.Resolve<IDomainService<RelatedAppealCits>>();
            var subjectDomainService = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var executantDomainService = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var acroDomainService = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var appealCitsSourceService = this.Container.Resolve<IDomainService<AppealCitsSource>>();
            var appealCitsStatSubjectService = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var appealCitsDomain = Container.Resolve<IDomainService<AppealCits>>();
            var appealCitsAnswerService = Container.Resolve<IDomainService<AppealCitsAnswer>>();
            var zonalInspectionInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            Operator thisOperator = UserManager.GetActiveOperator();
            ZonalInspection zonal = null;
            try
            {
                if (thisOperator?.Inspector != null)
                {
                    var zonalInsp = zonalInspectionInspectorDomain.GetAll()
                         .Where(x => x.Inspector == thisOperator.Inspector && x.ZonalInspection.IsNotGZHI).FirstOrDefault();
                    if (zonalInsp != null)
                    {
                        zonal = zonalInsp.ZonalInspection;
                    }
                }
                var loadParams = baseParams.GetLoadParam();
                var listIds = baseParams.Params.ContainsKey("Id") ? GetIds(baseParams.Params.GetAs<string>("Id")) : GetIds(String.Empty);

                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var authorId = GetIds(baseParams.Params.GetAs<string>("authorId"));
                var executantId = GetIds(baseParams.Params.GetAs<string>("executantId"));
                var responcibleId = GetIds(baseParams.Params.GetAs<string>("responcibleId"));
                var controllerId = GetIds(baseParams.Params.GetAs<string>("controllerId"));
                var signerIds = GetIds(baseParams.Params.GetAs<string>("signerId"));

                var statSubsubjectGjiId = baseParams.Params.GetAs<long>("statSubsubjectGjiId");
                var statSubjectId = baseParams.Params.GetAs<long>("statSubjectId");
                var relatesToId = baseParams.Params.GetAs<long>("relatesToId");
                var anotherRelatesToId = baseParams.Params.GetAs<long>("anotherRelatesToId");
                var dateFromStart = baseParams.Params.GetAs("dateFromStart", relatesToId == 0 ? DateTime.Now.AddMonths(-24) : DateTime.MinValue);


                var dateFromEnd = baseParams.Params.GetAs("dateFromEnd", DateTime.MinValue);

                var checkTimeStart = baseParams.Params.GetAs("checkTimeStart", DateTime.MinValue);
                var checkTimeEnd = baseParams.Params.GetAs("checkTimeEnd", DateTime.MinValue);
                var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);

                var matchRelated = baseParams.Params.GetAs<bool>("matchRelated");
                var showExtensTimes = baseParams.Params.GetAs("showExtensTimes", false);



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


                long[] relatedIds = null;
                string[] relatesToAddress = null;
                string[] relatesToMunicipalityFiasId = null;

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

                if (anotherRelatesToId > 0)
                {
                    var related = relatedAppealDomain.GetAll()
                        .Where(rac => rac.Children.Id == anotherRelatesToId)
                        .Select(rac => rac.Parent)
                        .ToArray();
                    relatedIds = related.Select(rac => rac.Id).ToArray();

                    //if (matchRelated)
                    //{
                    //    var relatesToAttributes = acroDomainService.GetAll()
                    //        .Where(acro => acro.AppealCits.Id == relatesToId)
                    //        .Select(acro => new
                    //        {
                    //            acro.RealityObject.Municipality.FiasId,
                    //            acro.RealityObject.Address
                    //        })
                    //        .ToArray();

                    //    relatesToAddress = relatesToAttributes.Select(i => i.Address).ToArray();
                    //    relatesToMunicipalityFiasId = relatesToAttributes.Select(i => i.FiasId).ToArray();
                    //}
                }

                //var query = this.ApplyFilters(this.AddUserFilters(serviceAppealCitizensGji.GetAll()), baseParams)
                //      .WhereIf(appealsByStatsub.Count > 0, x =>  appealsByStatsub.Contains(x.Id))
                //    .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                //    .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                //    .WhereIf(
                //        realityObjectId > 0,
                //        x => x.RealityObjectIds.Contains(string.Format("/{0}/", realityObjectId)))
                //    .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom > dateFromStart)
                //    .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                //    .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime > checkTimeStart)
                //    .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                //    .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                //    .WhereIf(showExtensTimes, x => x.ExtensTime != null)
                //    .WhereIf(
                //        authorId.Count() > 0,
                //        x => executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id && authorId.Contains(y.Author.Id)))
                //    .WhereIf(
                //        executantId.Count() > 0,
                //        x => executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id && executantId.Contains(y.Executant.Id) ))
                //    .WhereIf(
                //        controllerId.Count() > 0,
                //        x => executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id && controllerId.Contains(y.Controller.Id)))
                //    .WhereIf(relatesToId > 0 && !matchRelated, x => relatedIds.Contains(x.Id))
                //    .WhereIf(relatesToId > 0 && matchRelated, x => !relatedIds.Contains(x.Id))
                //    .WhereIf(anotherRelatesToId > 0 , x => relatedIds.Contains(x.Id))
                //    .WhereIf(matchRelated, x=> x.DateFrom.HasValue && x.DateFrom.Value >= DateTime.Now.AddYears(-1))
                //    .WhereIf(
                //        matchRelated,
                //        x => acroDomainService.GetAll().Where(acro => acro.AppealCits.Id == x.Id)
                //            .Any(
                //                acro => relatesToAddress.Contains(acro.RealityObject.Address)
                //                    && relatesToMunicipalityFiasId.Contains(acro.RealityObject.Municipality.FiasId)) && x.Id != relatesToId)
                //    .Select(
                //        x =>
                //            new
                //            {
                //                x.Id,
                //                x.State,
                //                Name = string.Format("{0} ({1})", x.Number, x.NumberGji),
                //                ManagingOrganization = x.ContragentName,
                //                x.Number,
                //                x.NumberGji,
                //                x.DateFrom,
                //                x.SSTUExportState,
                //                x.SOPR,
                //                x.QuestionStatus,
                //                x.CheckTime,
                //                IsSopr = x.State.Code == "СОПР"? true:false,
                //                x.QuestionsCount,
                //                x.Municipality,
                //            //    x.CountRealtyObj,
                //                Executant = x.Executant.Fio,
                //                Tester = x.Tester.Fio,
                //                SuretyResolve = x.SuretyResolve.Name,
                //                x.ExecuteDate,
                //                x.SuretyDate,
                //                x.ZonalInspection,
                //                ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : string.Empty,
                //                x.Correspondent,
                //                x.CorrespondentAddress,
                //                x.RealObjAddresses,
                //                x.AppealCits.SpecialControl,
                //                KindStatement = x.AppealCits.KindStatement.Name,
                //                HasExecutant = executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id),
                //                x.ExtensTime,
                //     //           x.Subjects,
                //                x.ExecutantNames,
                //   //             SubSubjects = x.SubSubjectsName,
                //          //      Features = x.FeaturesName,
                //                Executants = x.ControllersFio,
                //                x.RevenueSourceNames,
                //                x.RevenueSourceDates,
                //                x.AnswerDate,
                //                x.RevenueSourceNumbers
                //            })
                //    .Filter(loadParams, this.Container);

                //totalCount = query.Count();

                //query = query
                //    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                //    .Order(loadParams);

                //if (usePaging)
                //{
                //    query = query.Paging(loadParams);
                //}

                //return query.ToList();

                var signersAppealCitsIdsQuery = appealCitsAnswerService.GetAll()
                    .Where(x => signerIds.Contains(x.Signer.Id))
                    .Select(x => x.AppealCits.Id)
                    .ToArray();

                List<long> acrolist = new List<long>();
                if (realityObjectId > 0)
                {
                    acrolist = acroDomainService.GetAll().Where(acro => acro.RealityObject.Id == realityObjectId).Select(acro => acro.AppealCits.Id).ToList();
                }

                var query2 = appealCitsDomain.GetAll()
                         .WhereIf(appealsByStatsub.Count > 0, x => appealsByStatsub.Contains(x.Id))
                         .WhereIf(zonal != null, x => x.ZonalInspection == zonal)
                       .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                       .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                       .WhereIf(realityObjectId > 0, x => acrolist.Contains(x.Id))
                       .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom > dateFromStart)
                       .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                       .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime > checkTimeStart)
                       .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                       .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                       .WhereIf(showExtensTimes, x => x.ExtensTime != null)
                       .WhereIf(signersAppealCitsIdsQuery.Length > 0, x => signersAppealCitsIdsQuery.Contains(x.Id))
                       .WhereIf(
                           authorId.Count() > 0,
                           x => executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id && authorId.Contains(y.Author.Id)))
                       .WhereIf(
                           executantId.Count() > 0,
                           x => executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id && executantId.Contains(y.Executant.Id)))
                           .WhereIf(
                           responcibleId.Count() > 0,
                           x => executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id && y.IsResponsible && responcibleId.Contains(y.Executant.Id)))
                       .WhereIf(
                           controllerId.Count() > 0,
                           x => x.OrderContragent != null && controllerId.Contains(x.OrderContragent.Id))
                       .WhereIf(relatesToId > 0 && !matchRelated, x => relatedIds.Contains(x.Id))
                       .WhereIf(relatesToId > 0 && matchRelated, x => !relatedIds.Contains(x.Id))
                       .WhereIf(anotherRelatesToId > 0, x => relatedIds.Contains(x.Id))
                       .WhereIf(matchRelated, x => x.DateFrom.HasValue && x.DateFrom.Value >= DateTime.Now.AddYears(-1))
                       .WhereIf(
                           matchRelated,
                           x => acroDomainService.GetAll().Where(acro => acro.AppealCits.Id == x.Id)
                               .Any(
                                   acro => relatesToAddress.Contains(acro.RealityObject.Address)
                                       && relatesToMunicipalityFiasId.Contains(acro.RealityObject.Municipality.FiasId)) && x.Id != relatesToId)
                   .Select(
                       x =>
                       new
                       {
                           x.Id,
                           x.State,
                           Name = $"{x.Number} ({x.NumberGji})",
                           ManagingOrganization = x.ManagingOrganization != null ? x.ManagingOrganization.Contragent.Name : "",
                           x.DocumentNumber,
                           x.Number,
                           x.NumberGji,
                           x.DateFrom,
                           x.CheckTime,
                           x.QuestionsCount,
                           x.QuestionStatus,
                           Executant = x.Executant != null ? x.Executant.Fio : string.Empty,
                           Tester = x.Tester != null ? x.Tester.Fio : "",
                           SuretyResolve = x.SuretyResolve != null ? x.SuretyResolve.Name : "",
                           x.ExecuteDate,
                           StateCode = x.State.Code,
                           x.SuretyDate,
                           x.CaseDate,
                           x.ZonalInspection,
                           ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : string.Empty,
                           x.Correspondent,
                           x.CorrespondentAddress,
                           AnswerDocNum = GetAnswerDocs(x.Id),
                           AnswerSugFile = GetFiles(x.Id),
                           x.SpecialControl,
                           KindStatement = x.KindStatement != null ? x.KindStatement.Name : string.Empty,
                           HasExecutant = executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id),
                           x.ExtensTime,
                           x.Description,
                           x.Municipality,
                           x.MunicipalityId,
                           x.AppealUid,
                           x.IncomingSourcesName,
                           x.Phone,
                           SoprDate = x.CaseDate,
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
                           OrderContragent = x.OrderContragent != null ? x.OrderContragent.Name : "",
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
                this.Container.Release(relatedAppealDomain);
                this.Container.Release(subjectDomainService);
                this.Container.Release(executantDomainService);
                this.Container.Release(acroDomainService);
                this.Container.Release(appealCitsSourceService);
            }
        }

        private string GetAnswerDocs(long appId)
        {
            var AppealCitsAnswerDomain = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();
            var answers = AppealCitsAnswerDomain.GetAll()
                .Where(x => x.AppealCits.Id == appId)
                .Where(x => x.File != null && x.TypeAppealAnswer != GkhGji.Enums.TypeAppealAnswer.Note)
                .Where(x => x.DocumentNumber != null && x.DocumentNumber != "")
                .Select(x => x.DocumentNumber).ToList();
            string data = string.Empty;
            foreach (string str in answers)
            {
                if (string.IsNullOrEmpty(data))
                {
                    data = str;
                }
                else
                {
                    data += "," + str;
                }
            }
            return data;
        }
        private List<FileInfo> GetFiles(long appId)
        {
            var AppealCitsAnswerDomain = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();
            var answers = AppealCitsAnswerDomain.GetAll()
                .Where(x => x.AppealCits.Id == appId)
                 .Where(x => x.DocumentNumber != null && x.DocumentNumber != "")
                .Where(x => x.File != null && x.TypeAppealAnswer != GkhGji.Enums.TypeAppealAnswer.Note)
                .Select(x => x.File).ToList();
            return answers;
        }

        private HashSet<long> GetIds(string ids)
        {
            var listIds = new HashSet<long>();
            if (!string.IsNullOrEmpty(ids))
            {
                foreach (var item in ids.ToLongArray())
                {
                    listIds.Add(item);
                }
            }
            return listIds;
        }
    }
}