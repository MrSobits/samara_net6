using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Pivot;
using Bars.B4.Modules.Pivot.Enum;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;

namespace Bars.GkhGji.Report
{
    using B4.Modules.Reports;

    public class OlapByInspectionReport : IPrintForm, IPivotModel
    {
        private DateTime dateStart = DateTime.Now;
        private DateTime dateEnd = DateTime.Now;

        public IWindsorContainer Container { get; set; }

        public string RequiredPermission
        {
            get
            {
                return "Reports.GJI.OlapByInspectionReport";
            }
        }

        public IList<string> ReportFormats
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return "Отчет по проверкам";
            }
        }

        public string Desciption
        {
            get
            {
                return "Отчет по проверкам";
            }
        }

        public string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public string ParamsController
        {
            get
            {
                return "B4.controller.report.OlapByInspectionReport";
            }
        }

        public string Params { get; set; }

        public object Data { get; set; }

        public void LoadData()
        {
            var dict = DynamicDictionary.FromString(Params);

            dateStart = dict.GetAs<DateTime>("startDate");
            dateEnd = dict.GetAs<DateTime>("endDate");

            var inspectionRealObjDomain = Container.ResolveDomain<InspectionGjiRealityObject>();
            var docGjiDomain = Container.ResolveDomain<DocumentGji>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var inspViolDomain = Container.ResolveDomain<InspectionGjiViol>();
            var appCitStatSubjDomain = Container.ResolveDomain<AppealCitsStatSubject>();
            var inspGjiAppCitsDomain = Container.ResolveDomain<InspectionAppealCits>();
            var dispTypeSurveyDomain = Container.ResolveDomain<DisposalTypeSurvey>();
            var manOrgRealObjDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
            var disposalDomain = Container.ResolveDomain<Disposal>();
            var typeSurveyDomain = Container.ResolveDomain<TypeSurveyKindInspGji>();
            var inspViolStageDomain = Container.ResolveDomain<InspectionGjiViolStage>();
            
            try
            {
                var inspectionQuery = disposalDomain.GetAll()
                    .Where(x => dateStart <= x.DocumentDate && x.DocumentDate <= dateEnd);

                var inspectionRealObjQuery = inspectionRealObjDomain.GetAll()
                    .Where(x => x.Inspection.TypeBase == TypeBase.PlanJuridicalPerson 
                        || x.Inspection.TypeBase == TypeBase.CitizenStatement)
                    .Where(x => inspectionQuery.Any(y => y.Inspection.Id == x.Inspection.Id));

                var disposalInspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Select(x => new
                    {
                        x.DocumentGji.Id,
                        x.Inspector.ShortFio
                    })
                    .AsEnumerable()
                    .Distinct()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.ShortFio).AggregateWithSeparator(", "));

                var inspectionRealObj = inspectionRealObjQuery
                    .Select(x => new
                    {
                        x.Inspection.Id,
                        RoId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        MuName = x.RealityObject.Municipality.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key);


                var violations = inspViolDomain.GetAll()
                    .Where(x => inspectionQuery.Any(y => y.Inspection.Id == x.Inspection.Id))
                    .Select(x => new
                    {
                        x.Violation.Id,
                        x.Violation.Name,
						InspectionId = x.Inspection.Id,
                    })
                    .AsEnumerable()
                    .Distinct()
                    .ToList(); 

                var manOrgByRo =
                    manOrgRealObjDomain.GetAll()
                             .Where(x => x.ManOrgContract.ManagingOrganization != null && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now))
                             .Where(x => inspectionRealObjQuery.Any(y => x.RealityObject.Id == y.RealityObject.Id))
                             .Select(x => new
                             {
                                 RealObjId = x.RealityObject.Id,
                                 ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name
                             })
                             .AsEnumerable()
                             .GroupBy(x => x.RealObjId)
                             .ToDictionary(
                                 x => x.Key,
                                 y => y.Select(x => x.ManOrgName).FirstOrDefault());

                var disposals = disposalDomain.GetAll()
                  .Where(x => dateStart <= x.DocumentDate && x.DocumentDate <= dateEnd)
                  .Where(x => x.KindCheck != null && x.Inspection != null)
                  .Where(x => x.Inspection.TypeBase == TypeBase.PlanJuridicalPerson
                        || x.Inspection.TypeBase == TypeBase.CitizenStatement)
                  .Select(x => new
                    {
                        x.TypeDisposal,
                        x.KindCheck.Code,
                        x.Id,
                        x.TypeAgreementProsecutor,
                        x.TypeAgreementResult,
                        InspId = x.Inspection.Id,
                        x.Inspection.TypeBase
                    })
                    .ToArray();

                var disposalChildrens = disposalDomain.GetAll()
                    .Join(
                        docGjiDomain.GetAll(),
                        x => x.Stage.Id,
                        y => y.Stage.Parent.Id,
                        (a, b) => new { Disposal = a, Document = b })
                    .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.ActCheck
                        || x.Document.TypeDocumentGji == TypeDocumentGji.Prescription
                        || x.Document.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Select(x => new
                    {
                        DispId = x.Disposal.Id,
                        x.Document.Id,
                        x.Document.TypeDocumentGji
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.DispId)
                    .ToDictionary(x => x.Key);

                var docViols = inspViolStageDomain.GetAll()
                    .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.ActCheck
                                || x.Document.TypeDocumentGji == TypeDocumentGji.Prescription
                                || x.Document.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => dateStart <= x.Document.DocumentDate && x.Document.DocumentDate <= dateEnd)
                    .Select(x => new
                    {
                        DocId = x.Document.Id,
                        ViolId = x.InspectionViolation.Violation.Id
                    })
                    .AsEnumerable()
                    .Distinct()
                    .Select(x => "{0}_{1}".FormatUsing(x.DocId, x.ViolId))
                    .ToHashSet();

                var docsWithoutViol = docGjiDomain.GetAll()
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.ActCheck
                                || x.TypeDocumentGji == TypeDocumentGji.Prescription
                                || x.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => !inspViolStageDomain.GetAll().Any(y => y.Document.Id == x.Id))
                    .Select(x => x.Id)
                    .ToHashSet();

				violations.Add(new { Id = 0L, Name = "Нарушения не выявлены", InspectionId = 0L });

                var typeCheck = new List<TypeCheck> { TypeCheck.PlannedExit, TypeCheck.NotPlannedExit, TypeCheck.PlannedDocumentation, TypeCheck.NotPlannedDocumentation };
                var typeSurveys = typeSurveyDomain.GetAll()
                    .Where(x => typeCheck.Contains(x.KindCheck.Code))
                    .Select(x => new { x.TypeSurvey.Id, x.TypeSurvey.Name, x.KindCheck.Code })
                    .AsEnumerable()
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key);

                var typeSurveyByDisp = dispTypeSurveyDomain.GetAll()
                    .Where(x => dateStart <= x.Disposal.DocumentDate && x.Disposal.DocumentDate <= dateEnd)
                    .Select(x => new {DispId = x.Disposal.Id, SurvId = x.TypeSurvey.Id})
                    .AsEnumerable()
                    .GroupBy(x => x.DispId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.SurvId));



                var appCitsCntByInsp = inspGjiAppCitsDomain.GetAll()
                    .Where(x => inspectionQuery.Any(y => y.Inspection.Id == x.Inspection.Id))
                    .Select(x => new {InspId = x.Inspection.Id, AppId = x.AppealCits.Id})
                    .AsEnumerable()
                    .GroupBy(x => x.InspId)
                    .ToDictionary(x => x.Key, y => y.Count());

                var statSubjCodeByInsp = appCitStatSubjDomain.GetAll()
                    .Join(inspGjiAppCitsDomain.GetAll(),
                        x => x.AppealCits.Id,
                        y => y.AppealCits.Id,
                        (a, b) => new { AppStatSubj = a, InspApp = b })
                    .Where(x => inspectionQuery.Any(y => y.Inspection.Id == x.InspApp.Inspection.Id))
                    .Select(x => new
                    {
                        InspId = x.InspApp.Inspection.Id,
                        x.AppStatSubj.Subsubject.Code
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.InspId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Code));


				var table = new DataTable();
				table.Columns.Add(new DataColumn("MuName"));
				table.Columns.Add(new DataColumn("ManOrg"));
				table.Columns.Add(new DataColumn("Address"));
				table.Columns.Add(new DataColumn("Inspectors"));
				table.Columns.Add(new DataColumn("CountInfo"));
				table.Columns.Add(new DataColumn("Violation"));
				table.Columns.Add(new DataColumn("InspType"));
				table.Columns.Add(new DataColumn("KindCheck"));
				table.Columns.Add(new DataColumn("TypeSurvey"));
				table.Columns.Add(new DataColumn("TypeDocument"));
				var docCountColumn = new DataColumn("DocumentCount", typeof (int));
				docCountColumn.DefaultValue = DBNull.Value;
				table.Columns.Add(docCountColumn);
                var countInfoList = new List<CountInfo> { CountInfo.DocCount, CountInfo.AppCitCount };
                var typeDocuments = new List<TypeDocumentGji> { TypeDocumentGji.Disposal, TypeDocumentGji.ActCheck, TypeDocumentGji.Prescription, TypeDocumentGji.Protocol };
                var typeInspections = (TypeInspection[]) Enum.GetValues(typeof (TypeInspection));
                var emptyList = new List<string>();
                foreach (var disposal in disposals)
                {
                   var inspRealObj = inspectionRealObj.Get(disposal.InspId);

                    if (inspRealObj == null)
                    {
                        continue;
                    }

                    foreach (var realObj in inspRealObj)
                    {
                        foreach (var countInfo in countInfoList)
                        {
                                foreach (TypeInspection typeInspection in typeInspections)
                                {
                                    foreach (var kindCheck in typeCheck)
                                    {
                                        if (!typeSurveys.ContainsKey(kindCheck))
                                        {
                                            continue;
                                        }

                                        foreach (var typeSurvey in typeSurveys[kindCheck])
                                        {
                                            foreach (var typeDocument in typeDocuments)
                                            {
                                                var row = table.NewRow();

                                                if (disposal.Code == kindCheck)
                                                {
                                                    if(typeSurveyByDisp.ContainsKey(disposal.Id))
                                                    {
                                                        if (typeSurveyByDisp[disposal.Id].Contains(typeSurvey.Id))
                                                        {
															row["MuName"] = realObj.MuName;
															row["ManOrg"] = manOrgByRo.Get(realObj.RoId);
															row["Address"] = realObj.Address;
															row["Inspectors"] = disposalInspectors.Get(disposal.Id);
															row["CountInfo"] = countInfo.GetEnumMeta().Display;
															row["InspType"] = typeInspection.GetEnumMeta().Display;
															row["KindCheck"] = kindCheck.GetEnumMeta().Display;
															row["TypeSurvey"] = typeSurvey.Name;
															row["TypeDocument"] = typeDocument.GetEnumMeta().Display;
															
                                                            if (disposalChildrens.ContainsKey(disposal.Id))
                                                            {
                                                                var statSubjs = statSubjCodeByInsp.Get(disposal.Id) ?? emptyList;

                                                                if (countInfo == CountInfo.AppCitCount)
                                                                {
                                                                    if (typeDocument == TypeDocumentGji.Disposal)
                                                                    {
                                                                        row["DocumentCount"] = appCitsCntByInsp.Get(disposal.InspId);
                                                                    }

                                                                    continue;
                                                                } 

                                                                if ((disposal.TypeDisposal == TypeDisposalGji.DocumentGji && typeInspection == TypeInspection.Control) ||
                                                                    (disposal.TypeBase == TypeBase.PlanJuridicalPerson && typeInspection == TypeInspection.Planned) ||
                                                                     (disposal.TypeBase == TypeBase.CitizenStatement &&
                                                                         (disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement && statSubjs.Contains("2") && typeInspection == TypeInspection.ThreatToLife) ||
                                                                         (disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement && statSubjs.Contains("3") && typeInspection == TypeInspection.InflictionOfHarm) ||
                                                                         (disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.NotRequiresAgreement && disposal.TypeAgreementResult == TypeAgreementResult.NotSet && typeInspection == TypeInspection.ConsumerProtection) ||
                                                                         (statSubjs.Contains("7") && typeInspection == TypeInspection.DecMeetingOfOwners)))
                                                                {
																	foreach (var violation in violations.Where(x => x.InspectionId == disposal.InspId).ToArray())
																	{
																		row["Violation"] = violation.Name;

																		var childDocsCount = disposalChildrens[disposal.Id]
																			.WhereIf(typeDocument != TypeDocumentGji.Disposal, x => x.TypeDocumentGji == typeDocument)
																			.WhereIf(typeDocument == TypeDocumentGji.Disposal, x => x.TypeDocumentGji == TypeDocumentGji.ActCheck)
																			.WhereIf(violation.Id > 0, x => docViols.Contains("{0}_{1}".FormatUsing(x.Id, violation.Id)))
																			.WhereIf(violation.Id == 0, x => docsWithoutViol.Contains(x.Id))
																			.Count();

																		if (childDocsCount > 0)
																		{
																			row["DocumentCount"] = typeDocument == TypeDocumentGji.Disposal
																				? (int?) 1
																				: (int?) childDocsCount;
																		}
																		else
																		{
																			row["DocumentCount"] = DBNull.Value;
																		}
																	}
                                                                }
                                                            } else if(typeDocument == TypeDocumentGji.Disposal && countInfo == CountInfo.DocCount)
                                                            {
                                                                row["DocumentCount"] = 1;
                                                            }


                                                        
                                                            table.Rows.Add(row);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                        }
                    }
                }

				this.Data = table;
            }
            finally
            {
                Container.Release(inspectionRealObjDomain); 
                Container.Release(docGjiDomain ); 
                Container.Release(docInspectorDomain ); 
                Container.Release(inspViolDomain ); 
                Container.Release(appCitStatSubjDomain );
                Container.Release(inspGjiAppCitsDomain ); 
                Container.Release(dispTypeSurveyDomain ); 
                Container.Release(manOrgRealObjDomain );  
                Container.Release(disposalDomain ); 
                Container.Release(typeSurveyDomain ); 
                Container.Release(inspViolStageDomain ); 
            }
        }

        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatString = "n", FormatType = FormatType.Numeric };

            var config = new PivotConfiguration
                             {
                                 Name = "OlapByInspection",
                                 ModelName = "Report.OlapByInspectionReport",
                                 RowsPerPage = 20
                             };

            var fields = new List<Field>
                             {
                                     new Field
                                     {
                                         Name = "MuName",
                                         Area = Area.RowArea,
                                         AreaIndex = 0,
                                         DisplayName = "Муниципальное образование"
                                     },
                                     new Field
                                     {
                                         Name = "Inspectors",
                                         Area = Area.RowArea,
                                         DisplayName = "Инспекторы",
                                         AreaIndex = 2
                                     },
                                     new Field
                                     {
                                         Name = "Address",
                                         Area = Area.RowArea,
                                         DisplayName = "Адрес МКД",
                                         AreaIndex = 3
                                     },
                                     new Field
                                     {
                                         Name = "CountInfo",
                                         Area = Area.RowArea,
                                         DisplayName = "Показатель",
                                         AreaIndex = 4
                                     },
                                     new Field
                                     {
                                         Name = "Violation",
                                         Area = Area.ColumnArea,
                                         DisplayName = "Нарушение",
                                         AreaIndex = 5
                                     },
                                     new Field
                                     {
                                         Name = "InspType",
                                         Area = Area.ColumnArea,
                                         DisplayName = "Основания проверки",
                                         AreaIndex = 6
                                     },
                                     new Field
                                     {
                                         Name = "KindCheck",
                                         Area = Area.ColumnArea,
                                         DisplayName = "Вид проверки",
                                         AreaIndex = 7
                                     },
                                     new Field
                                     {
                                         Name = "TypeSurvey",
                                         Area = Area.ColumnArea,
                                         DisplayName = "Тип обследования",
                                         AreaIndex = 8
                                     },
                                     new Field
                                     {
                                         Name = "TypeDocument",
                                         Area = Area.ColumnArea,
                                         DisplayName = "Тип документа",
                                         AreaIndex = 9
                                     },
                                     new Field
                                     {
                                         Name = "DocumentCount",
                                         Area = Area.DataArea,
                                         DisplayName = "Количество документов",
                                         AreaIndex = 10,
                                         SummaryType = SummaryType.Sum
                                     }
                             };

            config.Fields = fields;
            return config;
        }

        public Stream GetTemplate()
        {
            throw new NotImplementedException();
        }

        public void PrepareReport(ReportParams reportParams)
        {
            throw new NotImplementedException();
        }

        public void SetUserParams(BaseParams baseParams)
        {
        }

        public string ReportGenerator { get; set; }

        private enum TypeInspection
        {
            [Display("Плановые")]
            Planned = 10,

            [Display("Контрольная")]
            Control = 20,

            [Display("Cогласование с прокуратурой (угроза жизни и здоровью, имуществу и т.п.)")]
            ThreatToLife = 30,

            [Display("Cогласование с прокуратурой (причинение вреда и т.п.)")]
            InflictionOfHarm = 40,

            [Display("Защита прав потребителей")]
            ConsumerProtection = 50,

            [Display("Нарушение порядка принятия решений общего собрания собственников")]
            DecMeetingOfOwners = 60,

            //[Display("По требованию прокуратуры")]
            //ProsClaim = 70,

            //[Display("По обращению граждан")]
            //Citizen = 80
        }

        private enum CountInfo
        {
            [Display("Кол-во документов")]
            DocCount = 10,

            [Display("Кол-во обращений")]
            AppCitCount = 20,
        }
    }
}            