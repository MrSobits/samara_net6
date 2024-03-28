namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Properties;

    using Castle.Windsor;

    // Данный класс служит сборкой для Отчет для прокуратуры (ежемесячный)
    // Внимание Данная сборка меняется в Томске 
    public class MonthlyProsecutorsOfficeServiceData : IMonthlyProsecutorsOfficeServiceData
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiRealityObject> inspectRoDomain { get; set; }

        public IDomainService<ActCheck> serviceActCheck { get; set; }

        public IDomainService<Disposal> serviceDisposal { get; set; }

        public IDomainService<DocumentGji> serviceDocument { get; set; }

        public IDomainService<DisposalTypeSurvey> serviceDisposalTypeSurvey { get; set; }
        
        public IDomainService<Resolution> serviceResolution { get; set; }

        public IDomainService<ResolutionDispute> serviceResolutionDispute { get; set; }

        protected List<string> execCodes = new List<string>() { "0", "2", "4", "8", "11", "18", "9" };
        protected List<TypeJurPerson> typesJurPersons = new List<TypeJurPerson>()
                                      {
                                          TypeJurPerson.ManagingOrganization,
                                          TypeJurPerson.SupplyResourceOrg,
                                          TypeJurPerson.Builder,
                                          TypeJurPerson.ServOrg,
                                          TypeJurPerson.RenterOrg
                                      };

        public virtual byte[] GetTemplate()
        {
            return Resources.MonthlyProsecutorsOffice;
        }

        public virtual MonthlyProsecutorsOfficeData GetData(DateTime reportDate, List<long> municipalityes)
        {
            var data = new MonthlyProsecutorsOfficeData();
            
            var inspectionsId = inspectRoDomain.GetAll()
                               .WhereIf(
                                   municipalityes.Count > 0,
                                   y => municipalityes.Contains(y.RealityObject.Municipality.Id))
                               .Select(y => y.Inspection.Id)
                               .Distinct()
                               .ToList();
            var disposals =
                serviceDisposal.GetAll()
                              .Where(x => inspectionsId.Contains(x.Inspection.Id))
                              .Select(x => new
                              {
                                  x.Id,
                                  StageId = x.Stage.Id,
                                  DocumentDate = x.DocumentDate.HasValue ? x.DocumentDate.Value : DateTime.MinValue,
                                  Code = x.KindCheck != null ? x.KindCheck.Code : 0
                              })
                              .ToList();
            
            var disposalStageIds = disposals.Select(x => x.StageId).ToList();

            var dispNotPlannedStageIds =
                disposals.Where(
                    x =>
                    x.Code == TypeCheck.NotPlannedDocumentation || x.Code == TypeCheck.NotPlannedExit
                    || x.Code == TypeCheck.NotPlannedDocumentationExit).Select(x => x.StageId).ToList();

            var dispNotPlannedIds =
                disposals.Where(
                    x =>
                    x.Code == TypeCheck.NotPlannedDocumentation || x.Code == TypeCheck.NotPlannedExit
                    || x.Code == TypeCheck.NotPlannedDocumentationExit).Select(x => x.Id).ToList();

            var dispNotPlannedProcStageIds = 
                    serviceDisposalTypeSurvey
                    .GetAll()
                    .Where(x => dispNotPlannedIds.Contains(x.Disposal.Id))
                    .Where(x => x.TypeSurvey.Code == "3")
                    .Select(x => x.Disposal.Stage.Id)
                    .ToList();

            FillValuesInRows1_3_1(data, reportDate, disposalStageIds);
            FillValuesInRows4_7(data, reportDate, disposalStageIds, dispNotPlannedStageIds, dispNotPlannedProcStageIds);
            FillValuesInRows8_11(data, reportDate, disposalStageIds, dispNotPlannedStageIds, dispNotPlannedProcStageIds);

            return data;
        }

        protected virtual void FillValuesInRows1_3_1(MonthlyProsecutorsOfficeData data, DateTime reportDate, List<long> disposalStageIds)
        {
            var startYearDate = new DateTime(reportDate.Year, 1, 1);
            var startMonthDate = new DateTime(reportDate.Year, reportDate.Month, 1);

            var disposals = serviceDisposal.GetAll()
                                .WhereIf(reportDate != DateTime.MinValue, x => x.DocumentDate <= reportDate)
                              .Where(x => serviceActCheck.GetAll().Any(y => y.Stage.Parent.Id == x.Stage.Id))
                              .Where(x => disposalStageIds.Contains(x.Stage.Id))
                              .Select(x => new DisposalProxy
                              {
                                  Id = x.Id,
                                  Code = x.KindCheck != null ? x.KindCheck.Code : 0,
                                  TypeJurPerson = x.Inspection.TypeJurPerson,
                                  DocumentDate = x.DocumentDate,
                                  PersonInspection = x.Inspection.PersonInspection,
                                  TypeEntrepreneurship = x.Inspection.Contragent != null ? x.Inspection.Contragent.TypeEntrepreneurship : 0
                              })
                              .ToList();

            var disposalsYear = disposals.Where(x => x.DocumentDate >= startYearDate).ToList();
            var disposalsMonth = disposals.Where(x => x.DocumentDate >= startMonthDate).ToList();

            var disposalsYearIdList = disposalsYear.Select(x => x.Id).Distinct().ToList();
            var disposalsMonthIdList = disposalsMonth.Select(x => x.Id).Distinct().ToList();

            var notPlannedDisposaldForYear = disposalsYear.Where(x => x.Code == TypeCheck.NotPlannedDocumentation
                                                                || x.Code == TypeCheck.NotPlannedExit
                                                                || x.Code == TypeCheck.NotPlannedDocumentationExit)
                                                          .ToList();

            var notPlannedDisposaldForMonth = disposalsMonth.Where(x => x.Code == TypeCheck.NotPlannedDocumentation
                                                                || x.Code == TypeCheck.NotPlannedExit
                                                                || x.Code == TypeCheck.NotPlannedDocumentationExit)
                                                            .ToList();

            var disposalsProsecutorAgreementYear = serviceDisposalTypeSurvey
                .GetAll()
                .Where(x => disposalsYearIdList.Contains(x.Disposal.Id))
                .Where(x => x.TypeSurvey.Code == "3")
                .Select(x => x.Disposal.Id)
                .ToList();

            var disposalsProsecutorAgreementMonth = 
                    serviceDisposalTypeSurvey
                    .GetAll()
                    .Where(x => disposalsMonthIdList.Contains(x.Disposal.Id))
                    .Where(x => x.TypeSurvey.Code == "3")
                    .Select(x => x.Disposal.Id)
                    .ToList();
            
            data.param1_1 = disposalsYearIdList.Count();
            data.param1_2 = disposalsMonthIdList.Count();
            data.param2_1 = notPlannedDisposaldForYear.Select(x => x.Id).Distinct().Count();
            data.param2_2 = notPlannedDisposaldForMonth.Select(x => x.Id).Distinct().Count();

            // Заполняем ячейку param3_1 и param3_2
            FillValuesInRows3(data, notPlannedDisposaldForYear, notPlannedDisposaldForMonth);
            
            data.param3_1_1 = disposalsProsecutorAgreementYear.Distinct().Count();
            data.param3_1_2 = disposalsProsecutorAgreementMonth.Distinct().Count();
        }

        protected virtual void FillValuesInRows3(MonthlyProsecutorsOfficeData data, List<DisposalProxy> disposalsYear, List<DisposalProxy> disposalsMonth)
        {
            data.param3_1 = disposalsYear.Where(x => this.typesJurPersons.Contains(x.TypeJurPerson) && x.PersonInspection != PersonInspection.PhysPerson).Select(x => x.Id).Distinct().Count();
            data.param3_2 = disposalsMonth.Where(x => this.typesJurPersons.Contains(x.TypeJurPerson) && x.PersonInspection != PersonInspection.PhysPerson).Select(x => x.Id).Distinct().Count();
        } 

        protected virtual void FillValuesInRows4_7(MonthlyProsecutorsOfficeData data, DateTime reportDate, List<long> disposalStageIds, List<long> dispNotPlannedStageIds, List<long> dispNotPlannedProcStageIds)
        {
            var startYearDate = new DateTime(reportDate.Year, 1, 1);
            var startMonthDate = new DateTime(reportDate.Year, reportDate.Month, 1);
            
            var servicePrescription = this.Container.Resolve<IDomainService<Prescription>>();

            var prescriptionsYear = servicePrescription.GetAll()
                .Where(x => x.DocumentDate >= startYearDate)
                .Where(x => x.DocumentDate <= reportDate)
                .Where(x => x.Stage.Parent != null && disposalStageIds.Contains(x.Stage.Parent.Id))
                .Select(x => new PrescriptionProxy
                {
                    Id = x.Id,
                    Code = x.Executant.Code,
                    ParentStageId = x.Stage.Parent.Id,
                    TypeEntrepreneurship = x.Contragent != null ? x.Contragent.TypeEntrepreneurship : 0
                })
                .ToList();

            var prescriptionsMonth = servicePrescription.GetAll()
                .Where(x => x.DocumentDate >= startMonthDate)
                .Where(x => x.DocumentDate <= reportDate)
                .Where(x => x.Stage.Parent != null && disposalStageIds.Contains(x.Stage.Parent.Id))
                .Select(x => new PrescriptionProxy
                {
                    Id = x.Id,
                    Code = x.Executant.Code,
                    ParentStageId = x.Stage.Parent.Id,
                    TypeEntrepreneurship = x.Contragent != null ? x.Contragent.TypeEntrepreneurship : 0
                })
                .ToList();
            
            data.param4_1 = prescriptionsYear.Select(x => x.Id).Distinct().Count();
            data.param4_2 = prescriptionsMonth.Select(x => x.Id).Distinct().Count();

            var prescriptionsYearWithDispNotPlanned = prescriptionsYear.Where(x => dispNotPlannedStageIds.Contains(x.ParentStageId));
            var prescriptionsMonthWithDispNotPlanned = prescriptionsMonth.Where(x => dispNotPlannedStageIds.Contains(x.ParentStageId));

            data.param5_1 = prescriptionsYearWithDispNotPlanned.Select(x => x.Id).Distinct().Count();
            data.param5_2 = prescriptionsMonthWithDispNotPlanned.Select(x => x.Id).Distinct().Count();

            FillValuesInRows6(
                data,
                prescriptionsYearWithDispNotPlanned.ToList(),
                prescriptionsMonthWithDispNotPlanned.ToList());

            data.param7_1 = prescriptionsYear.Where(x => dispNotPlannedProcStageIds.Contains(x.ParentStageId)).Select(x => x.Id).Distinct().Count();
            data.param7_2 = prescriptionsMonth.Where(x => dispNotPlannedProcStageIds.Contains(x.ParentStageId)).Select(x => x.Id).Distinct().Count();
        }

        protected virtual void FillValuesInRows6(MonthlyProsecutorsOfficeData data, List<PrescriptionProxy> prescriptionsYear, List<PrescriptionProxy> prescriptionsMonth)
        {
            data.param6_1 = prescriptionsYear.Where(x => this.execCodes.Contains(x.Code)).Select(x => x.Id).Distinct().Count();
            data.param6_2 = prescriptionsMonth.Where(x => this.execCodes.Contains(x.Code)).Select(x => x.Id).Distinct().Count();
        } 

        protected virtual void FillValuesInRows8_11(MonthlyProsecutorsOfficeData data, DateTime reportDate, List<long> disposalStageIds, List<long> dispNotPlannedStageIds, List<long> dispNotPlannedProcStageIds)
        {

            var startYearDate = new DateTime(reportDate.Year, 1, 1);
            var startMonthDate = new DateTime(reportDate.Year, reportDate.Month, 1);
       
            var resolutionYearTmp = serviceResolution.GetAll()
                .Where(x => x.DocumentDate >= startYearDate)
                .Where(x => x.DocumentDate <= reportDate)
                .Where(x => x.Sanction.Code == "1")
                .Where(x => x.Stage.Parent != null && disposalStageIds.Contains(x.Stage.Parent.Id))
                .Select(x => new ResolutionProxy
                {
                    Id = x.Id,
                    Code = x.Executant.Code,
                    ParentStageId = x.Stage.Parent.Id,
                    TypeEntrepreneurship = x.Contragent != null ? x.Contragent.TypeEntrepreneurship : 0
                })
                .ToList();

            var resolutionMonthTmp = serviceResolution.GetAll()
                .Where(x => x.DocumentDate >= startMonthDate)
                .Where(x => x.DocumentDate <= reportDate)
                .Where(x => x.Sanction.Code == "1")
                .Where(x => x.Stage.Parent != null && disposalStageIds.Contains(x.Stage.Parent.Id))
                .Select(x => new ResolutionProxy
                {
                    Id = x.Id,
                    Code = x.Executant.Code,
                    ParentStageId = x.Stage.Parent.Id,
                    TypeEntrepreneurship = x.Contragent != null ? x.Contragent.TypeEntrepreneurship : 0
                })
                .ToList();
            
            var resolutionTmpIds = resolutionYearTmp.Select(x => x.Id).ToList();

            var resolutionCanceled = 
                    serviceResolutionDispute
                    .GetAll()
                    .Where(x => resolutionTmpIds.Contains(x.Resolution.Id))
                    .Where(x => x.CourtVerdict.Code == "2")
                    .Select(x => x.Resolution.Id)
                    .ToList();
            
            var resolutionYear = resolutionYearTmp.Where(x => !resolutionCanceled.Contains(x.Id)).ToList();
            var resolutionMonth = resolutionMonthTmp.Where(x => !resolutionCanceled.Contains(x.Id)).ToList();

            data.param8_1 = resolutionYear.Where(x => disposalStageIds.Contains(x.ParentStageId)).Select(x => x.Id).Distinct().Count();
            data.param8_2 = resolutionMonth.Where(x => disposalStageIds.Contains(x.ParentStageId)).Select(x => x.Id).Distinct().Count();

            var resolutionYearWithDispNotPlanned = resolutionYear.Where(x => dispNotPlannedStageIds.Contains(x.ParentStageId)).ToList();
            var resolutionMonthWithDispNotPlanned = resolutionMonth.Where(x => dispNotPlannedStageIds.Contains(x.ParentStageId)).ToList();

            data.param9_1 = resolutionYearWithDispNotPlanned.Select(x => x.Id).Distinct().Count();
            data.param9_2 = resolutionMonthWithDispNotPlanned.Select(x => x.Id).Distinct().Count();

            FillValuesInRows10(data, resolutionYearWithDispNotPlanned, resolutionMonthWithDispNotPlanned);

            data.param11_1 = resolutionYear.Where(x => dispNotPlannedProcStageIds.Contains(x.ParentStageId)).Select(x => x.Id).Distinct().Count();
            data.param11_2 = resolutionMonth.Where(x => dispNotPlannedProcStageIds.Contains(x.ParentStageId)).Select(x => x.Id).Distinct().Count();
        }

        protected virtual void FillValuesInRows10(MonthlyProsecutorsOfficeData data, List<ResolutionProxy> prescriptionsYear, List<ResolutionProxy> prescriptionsMonth)
        {
            data.param10_1 = prescriptionsYear.Where(x => this.execCodes.Contains(x.Code)).Select(x => x.Id).Distinct().Count();
            data.param10_2 = prescriptionsMonth.Where(x => this.execCodes.Contains(x.Code)).Select(x => x.Id).Distinct().Count();
        } 

        protected class DisposalProxy
        {
            public long Id { get; set; }

            public TypeCheck Code { get; set; }

            public DateTime? DocumentDate { get; set; }

            public PersonInspection PersonInspection { get; set; }

            public TypeJurPerson TypeJurPerson { get; set; }

            public TypeEntrepreneurship TypeEntrepreneurship { get; set; }
        }

        protected class PrescriptionProxy
        {
            public long Id { get; set; }

            public string Code { get; set; }

            public long ParentStageId { get; set; }

            public TypeEntrepreneurship TypeEntrepreneurship { get; set; }
        }

        protected class ResolutionProxy
        {
            public long Id { get; set; }

            public string Code { get; set; }

            public long ParentStageId { get; set; }

            public TypeEntrepreneurship TypeEntrepreneurship { get; set; }
        }
    }
}
