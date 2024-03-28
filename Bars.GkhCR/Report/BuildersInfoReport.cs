namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет информация о подрядчиках
    /// </summary>
    public class BuildersInfoReport : BasePrintForm
    {
        private List<long> municipalityIds = new List<long>();
        private long programCrId;
        private DateTime reportDate = DateTime.Now;

        public BuildersInfoReport() : base(new ReportTemplateBinary(Properties.Resources.BuildersInfo))
        {
        }

        public IWindsorContainer Container { get; set; }

        #region Свойства

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.BuildersInfo";
            }
        }

        public override string Name
        {
            get { return "Информация о подрядчиках"; }
        }

        public override string Desciption
        {
            get { return "Информация о подрядчиках"; }
        }

        public override string GroupName
        {
            get { return "Формы программы"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.BuildersInfo"; }
        }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params.ContainsKey("programCrId") ? baseParams.Params["programCrId"].ToInt() : 0;
            var date = baseParams.Params.ContainsKey("reportDate") ? baseParams.Params["reportDate"].ToDateTime() : DateTime.MinValue;
            this.reportDate = date != DateTime.MinValue ? date : DateTime.Now;
            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
                                   ? baseParams.Params["municipalityIds"].ToString()
                                   : string.Empty;

            if (!string.IsNullOrEmpty(municipalityIdsList))
            {
                this.municipalityIds = municipalityIdsList.Split(',').Select(x => x.ToLong()).ToList();
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (this.programCrId <= 0)
            {
                throw new Exception("Не указан параметр \"Программа кап.ремонта\".");
            }

            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);

            reportParams.SimpleReportParams["ReportDate"] = this.reportDate.ToShortDateString();

            if (programCr != null)
            {
                reportParams.SimpleReportParams["ProgramYear"] = programCr.Period.DateStart.Year;
                reportParams.SimpleReportParams["ProgramName"] = programCr.Name;
            }

            var municipalities =
                this.Container.Resolve<IDomainService<Municipality>>()
                         .GetAll()
                         .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                         .Select(x => new { x.Id, x.Name })
                         .ToList();

            var realtyObjCountByMunicipal = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                         .Where(x => x.ProgramCr.Id == this.programCrId)
                         .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .GroupBy(x => x.RealityObject.Municipality.Id)
                         .Select(x => new
                         {
                             x.Key,
                             Count = x.Count()
                         })
                         .ToDictionary(x => x.Key, x => x.Count);
            
            var contragentCountByMunicipality = this.Container.Resolve<IDomainService<Qualification>>().GetAll()
                        .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                        .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                        .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Id)
                        .Select(y => new
                        {
                            y.Key,
                            CtrgCount = y.Count(),
                            UniqCtrgCount = y.Select(x => x.Builder.Contragent.Id).Distinct().Count(),
                            ObjCrHasQual = y.Select(x => x.ObjectCr.RealityObject.Id).Distinct().Count()
                        })
                       .ToDictionary(x => x.Key);
            
            var qualMembers = this.Container.Resolve<IDomainService<QualificationMember>>().GetAll()
                         .Where(x => x.Period.Id == programCr.Period.Id)
                         .Select(x => new { x.Id, x.Name })
                         .ToList();

            var voiceMembers = this.Container.Resolve<IDomainService<VoiceMember>>().GetAll()
                         .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Qualification.ObjectCr.RealityObject.Municipality.Id))
                         .Where(x => x.Qualification.ObjectCr.ProgramCr.Id == this.programCrId)
                         .Where(x => x.TypeAcceptQualification == TypeAcceptQualification.Yes)
                         .Where(x => !x.DocumentDate.HasValue || x.DocumentDate <= this.reportDate)
                         .Select(x => new
                         {
                             MunicipalityId = x.Qualification.ObjectCr.RealityObject.Municipality.Id,
                             QualificationMemberId = x.QualificationMember.Id,
                             BuilderId = x.Qualification.Builder.Id,
                             QualificationId = x.Qualification.Id,
                         })
                         .AsEnumerable()
                         .GroupBy(x => x.MunicipalityId)
                         .ToDictionary(
                             x => x.Key,
                             y => new
                             {
                                 AllUniqBuildersCount = y.Select(x => new { x.QualificationMemberId, x.BuilderId })
                                                         .GroupBy(x => x.BuilderId)
                                                         .ToDictionary(x => x.Key, z => z.Select(x => x.QualificationMemberId).Distinct().Count())
                                                         .Count(x => x.Value == qualMembers.Count),

                                 AllUniqRoCount = y.Select(x => new { x.QualificationMemberId, ObjectCrId = x.QualificationId })
                                                   .GroupBy(x => x.ObjectCrId)
                                                   .ToDictionary(x => x.Key, z => z.Select(x => x.QualificationMemberId).Distinct().Count())
                                                   .Count(x => x.Value == qualMembers.Count),

                                 Dict = y.GroupBy(x => x.QualificationMemberId)
                                 .ToDictionary(
                                             x => x.Key,
                                             z => new
                                             {
                                                 UniqBuildersCount = z.Select(w => w.BuilderId).Distinct().Count(),
                                                 UniqRoCount = z.Select(w => w.QualificationId).Distinct().Count()
                                             })
                             });

            var vertColumn = reportParams.ComplexReportParams.ДобавитьСекцию("Column");
            foreach (var qualMember in qualMembers)
            {
                vertColumn.ДобавитьСтроку();
                vertColumn["QualificationMember"] = qualMember.Name;

                vertColumn["AcceptUniqBuilderCnt"] = string.Format("$AcceptUniqBuilderCnt{0}$", qualMember.Id);
                vertColumn["AcceptRealObjCnt"] = string.Format("$AcceptRealObjCnt{0}$", qualMember.Id);

                vertColumn["SumAcceptUniqBuilderCnt"] = string.Format("$SumAcceptUniqBuilderCnt{0}$", qualMember.Id);
                vertColumn["SumAcceptRealObjCnt"] = string.Format("$SumAcceptRealObjCnt{0}$", qualMember.Id);
            }

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");
            int i = 0;
            foreach (var municipality in municipalities.OrderBy(x => x.Name))
            {
                i++;

                sectionMunicipality.ДобавитьСтроку();
                sectionMunicipality["Number"] = i;
                sectionMunicipality["Municipality"] = municipality.Name;
                sectionMunicipality["RoCnt"] = realtyObjCountByMunicipal.ContainsKey(municipality.Id)
                                                   ? realtyObjCountByMunicipal[municipality.Id]
                                                   : 0;
                sectionMunicipality["UniqCtrgCnt"] = contragentCountByMunicipality.ContainsKey(municipality.Id)
                                                         ? contragentCountByMunicipality[municipality.Id].UniqCtrgCount
                                                         : 0;
                sectionMunicipality["CtrgCnt"] = contragentCountByMunicipality.ContainsKey(municipality.Id)
                                                     ? contragentCountByMunicipality[municipality.Id].CtrgCount
                                                     : 0;
                sectionMunicipality["RoHasRec"] = contragentCountByMunicipality.ContainsKey(municipality.Id)
                                                      ? contragentCountByMunicipality[municipality.Id].ObjCrHasQual
                                                      : 0;

                var tempVoiceMembers = voiceMembers.ContainsKey(municipality.Id) ? voiceMembers[municipality.Id] : null;

                if (tempVoiceMembers != null)
                {
                    foreach (var qualMember in qualMembers)
                    {
                        sectionMunicipality["AcceptUniqBuilderCnt" + qualMember.Id] = tempVoiceMembers.Dict.ContainsKey(qualMember.Id)
                            ? tempVoiceMembers.Dict[qualMember.Id].UniqBuildersCount : 0;
                        sectionMunicipality["AcceptRealObjCnt" + qualMember.Id] = tempVoiceMembers.Dict.ContainsKey(qualMember.Id)
                            ? tempVoiceMembers.Dict[qualMember.Id].UniqRoCount : 0;
                    }

                    sectionMunicipality["AcceptUniqAllBuilderCnt"] = tempVoiceMembers.AllUniqBuildersCount;
                    sectionMunicipality["AcceptAllRealObjCnt"] = tempVoiceMembers.AllUniqRoCount;
                }
                else
                {
                    foreach (var qualMember in qualMembers)
                    {
                        sectionMunicipality["AcceptUniqBuilderCnt" + qualMember.Id] = 0;
                        sectionMunicipality["AcceptRealObjCnt" + qualMember.Id] = 0;
                    }

                    sectionMunicipality["AcceptUniqAllBuilderCnt"] = 0;
                    sectionMunicipality["AcceptAllRealObjCnt"] = 0;
                }
            }

            var municipalitiesKzn = this.Container.Resolve<IDomainService<Municipality>>().GetAll().Where(x => x.Group == "г. Казань").Select(x => x.Id).ToList();
            var sectionSummary = reportParams.ComplexReportParams.ДобавитьСекцию("sectionSum");
            sectionSummary.ДобавитьСтроку();
            sectionSummary["Sum"] = "ИТОГО по РТ";
            sectionSummary["SumRoCnt"] = realtyObjCountByMunicipal.Where(x => !municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value);
            sectionSummary["SumUniqCtrgCnt"] = contragentCountByMunicipality.Where(x => !municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.UniqCtrgCount);
            sectionSummary["SumCtrgCnt"] = contragentCountByMunicipality.Where(x => !municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.CtrgCount);
            sectionSummary["SumRoHasRec"] = contragentCountByMunicipality.Where(x => !municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.ObjCrHasQual);

            foreach (var qualMember in qualMembers)
            {
                sectionSummary["SumAcceptUniqBuilderCnt" + qualMember.Id] = voiceMembers.Where(x => !municipalitiesKzn.Contains(x.Key))
                    .Select(x => x.Value.Dict).Where(x => x.Keys.Contains(qualMember.Id)).Sum(x => x[qualMember.Id].UniqBuildersCount);
                sectionSummary["SumAcceptRealObjCnt" + qualMember.Id] = voiceMembers.Where(x => !municipalitiesKzn.Contains(x.Key))
                    .Select(x => x.Value.Dict).Where(x => x.Keys.Contains(qualMember.Id)).Sum(x => x[qualMember.Id].UniqRoCount);
            }

            sectionSummary["SumAcceptUniqAllBuilderCnt"] = voiceMembers.Where(x => !municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.AllUniqBuildersCount);
            sectionSummary["SumAcceptAllRealObjCnt"] = voiceMembers.Where(x => !municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.AllUniqRoCount);

            sectionSummary.ДобавитьСтроку();
            sectionSummary["Sum"] = "ИТОГО по г. Казань";
            sectionSummary["SumRoCnt"] = realtyObjCountByMunicipal.Where(x => municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value);
            sectionSummary["SumUniqCtrgCnt"] = contragentCountByMunicipality.Where(x => municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.UniqCtrgCount);
            sectionSummary["SumCtrgCnt"] = contragentCountByMunicipality.Where(x => municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.CtrgCount);
            sectionSummary["SumRoHasRec"] = contragentCountByMunicipality.Where(x => municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.ObjCrHasQual);
            foreach (var qualMember in qualMembers)
            {
                sectionSummary["SumAcceptUniqBuilderCnt" + qualMember.Id] = voiceMembers.Where(x => municipalitiesKzn.Contains(x.Key))
                    .Select(x => x.Value.Dict).Where(x => x.Keys.Contains(qualMember.Id)).Sum(x => x[qualMember.Id].UniqBuildersCount);
                sectionSummary["SumAcceptRealObjCnt" + qualMember.Id] = voiceMembers.Where(x => municipalitiesKzn.Contains(x.Key))
                    .Select(x => x.Value.Dict).Where(x => x.Keys.Contains(qualMember.Id)).Sum(x => x[qualMember.Id].UniqRoCount);
            }

            sectionSummary["SumAcceptUniqAllBuilderCnt"] = voiceMembers.Where(x => municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.AllUniqBuildersCount);
            sectionSummary["SumAcceptAllRealObjCnt"] = voiceMembers.Where(x => municipalitiesKzn.Contains(x.Key)).Sum(x => x.Value.AllUniqRoCount);

            sectionSummary.ДобавитьСтроку();
            sectionSummary["Sum"] = "ИТОГО";
            sectionSummary["SumRoCnt"] = realtyObjCountByMunicipal.Sum(x => x.Value);
            sectionSummary["SumUniqCtrgCnt"] = contragentCountByMunicipality.Sum(x => x.Value.UniqCtrgCount);
            sectionSummary["SumCtrgCnt"] = contragentCountByMunicipality.Sum(x => x.Value.CtrgCount);
            sectionSummary["SumRoHasRec"] = contragentCountByMunicipality.Sum(x => x.Value.ObjCrHasQual);
            foreach (var qualMember in qualMembers)
            {
                sectionSummary["SumAcceptUniqBuilderCnt" + qualMember.Id] = voiceMembers
                    .Select(x => x.Value.Dict).Where(x => x.Keys.Contains(qualMember.Id)).Sum(x => x[qualMember.Id].UniqBuildersCount);
                sectionSummary["SumAcceptRealObjCnt" + qualMember.Id] = voiceMembers
                    .Select(x => x.Value.Dict).Where(x => x.Keys.Contains(qualMember.Id)).Sum(x => x[qualMember.Id].UniqRoCount);
            }

            sectionSummary["SumAcceptUniqAllBuilderCnt"] = voiceMembers.Sum(x => x.Value.AllUniqBuildersCount);
            sectionSummary["SumAcceptAllRealObjCnt"] = voiceMembers.Sum(x => x.Value.AllUniqRoCount);
        }
    }
}