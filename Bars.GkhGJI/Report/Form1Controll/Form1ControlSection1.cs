using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.GkhGji.Report.Form1Controll
{  
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    class Form1ControlSection1 : BaseReportSection
    {
        public IWindsorContainer Container { get; set; }

        public Form1ControlSection1(List<long> inspections, DateTime dateStart, DateTime dateEnd, IWindsorContainer container)
            : base(inspections, dateStart, dateEnd)
        {
            Container = container;
        }

        private long[] disposalsIds;        

        private List<DisposalsIdsPart1> disposalsIdsPart1 = new List<DisposalsIdsPart1>();

        private List<DisposalsIdsPart1> disposalsIdsRows0203 = new List<DisposalsIdsPart1>();

        private List<DisposalsIdsPart1> disposalsIdsRows0809 = new List<DisposalsIdsPart1>();

        private List<long> baseProsClaimsExecutionInspection = new List<long>();

        private List<DisposalWithExpert> disposalExperts = new List<DisposalWithExpert>();

        private int GetCell1_5()
        {
            int start = 1000;
            var servInspectionsDisposalsIds =
                Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .WhereIf(this.dateStart != DateTime.MinValue, x => x.Parent.DocumentDate >= this.dateStart)
                    .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Parent.DocumentDate <= this.dateEnd);

            var tmpFirst = inspectionsIds.Length > start ? inspectionsIds.Take(1000).ToArray() : inspectionsIds;

            var inspectionsDisposalsIds =
                servInspectionsDisposalsIds.Where(x => tmpFirst.Contains(x.Parent.Inspection.Id))
                            .Select(x => new { disposalId = x.Parent.Id, inspectionId = x.Parent.Inspection.Id })
                            .ToList();

            while (start < inspectionsIds.Length)
            {
                var tmp = inspectionsIds.Skip(start).Take(1000).ToArray();


                inspectionsDisposalsIds.AddRange(servInspectionsDisposalsIds.Where(x => tmp.Contains(x.Parent.Inspection.Id))
                            .Select(x => new { disposalId = x.Parent.Id, inspectionId = x.Parent.Inspection.Id })
                            .ToList());

                start += 1000;
            }

            disposalsIds = inspectionsDisposalsIds.Select(x => x.disposalId).ToArray();

            var disposalsRow01 = new List<long>();

            for (var i = 0; i < disposalsIds.Length; i += 1000)
            {
                var takeCount = disposalsIds.Length - i < 1000 ? disposalsIds.Length - i : 1000;
                var tempList = disposalsIds.Skip(i).Take(takeCount).ToArray();
                disposalsRow01.AddRange(this.Container.Resolve<IDomainService<Disposal>>()
                         .GetAll()
                         .Where(x => tempList.Contains(x.Id))
                         .Where(x => x.Inspection.TypeBase == TypeBase.PlanJuridicalPerson
                         || x.Inspection.TypeBase == TypeBase.ProsecutorsClaim
                         || x.Inspection.TypeBase == TypeBase.DisposalHead
                         || x.Inspection.TypeBase == TypeBase.CitizenStatement)
                         .Where(
                             x =>
                             x.KindCheck.Code != TypeCheck.InspectionSurvey
                             && x.KindCheck.Code != TypeCheck.Monitoring)
                         .Select(x => x.Id)
                         .ToList());
            }

            return disposalsRow01.Count;
        }

        private int GetCell2_5()
        {
            var start = 1000;

            var servDisposalsIdsPart1 =
                this.Container.Resolve<IDomainService<Disposal>>().GetAll();

            var tmpFirst = disposalsIds.Length > start ? disposalsIds.Take(1000).ToArray() : disposalsIds;

            disposalsIdsPart1 =
                servDisposalsIdsPart1.Where(x => tmpFirst.Contains(x.Id))
                            .Select(x => new DisposalsIdsPart1
                                             {
                                                 Id = x.Id, 
                                                 TypeAgreementProsecutor = x.TypeAgreementProsecutor, 
                                                 TypeBase = x.Inspection.TypeBase, 
                                                 Code = (TypeCheck?)x.KindCheck.Code, 
                                                 InspectionId = x.Inspection.Id, 
                                                 StageId = x.Stage.Id
                                             })
                            .ToList();

            while (start < disposalsIds.Length)
            {
                var tmp = disposalsIds.Skip(start).Take(1000).ToArray();

                disposalsIdsPart1.AddRange(servDisposalsIdsPart1.Where(x => tmp.Contains(x.Id))
                            .Select(x => new DisposalsIdsPart1
                                             {
                                                 Id = x.Id,
                                                 TypeAgreementProsecutor = x.TypeAgreementProsecutor, 
                                                 TypeBase = x.Inspection.TypeBase,
                                                 Code = (TypeCheck?)x.KindCheck.Code,
                                                 InspectionId = x.Inspection.Id, 
                                                 StageId = x.Stage.Id
                                             })
                            .ToList());

                start += 1000;
            }

            this.disposalsIdsRows0203 = disposalsIdsPart1.Where(
                             x =>
                             x.TypeBase == TypeBase.PlanJuridicalPerson
                             || x.TypeBase == TypeBase.ProsecutorsClaim
                             || x.TypeBase == TypeBase.DisposalHead
                             || x.TypeBase == TypeBase.CitizenStatement)
                         .Where(
                             x =>
                             x.Code == TypeCheck.NotPlannedExit
                             || x.Code == TypeCheck.NotPlannedDocumentation
                             || x.Code == TypeCheck.NotPlannedDocumentationExit)
                         .Where(x =>
                             x.Code != TypeCheck.InspectionSurvey
                             && x.Code != TypeCheck.Monitoring)
                             .ToList();
            
            var disposalsCount02 = disposalsIdsRows0203.Count();

            return disposalsCount02;
        }

        private int GetCell3_5()
        {
            var prescriptionParentStageIds = new List<long>();

            for (var i = 0; i < inspectionsIds.Length; i += 1000)
            {
                var takeCount = inspectionsIds.Length - i < 1000 ? inspectionsIds.Length - i : 1000;
                var tempList = inspectionsIds.Skip(i).Take(takeCount).ToArray();
                prescriptionParentStageIds.AddRange(this.Container.Resolve<IDomainService<Prescription>>()
                         .GetAll()
                         .Where(x => tempList.Contains(x.Inspection.Id))
                         .Select(x => x.Stage.Parent.Id)
                         .ToList());
            }

            var disposalsCount03 = disposalsIdsRows0203
                .Where(x => prescriptionParentStageIds.Contains(x.StageId))
                .Where(x => x.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement).ToList();
            return disposalsCount03.Count();
        }

        private int GetCell5_5()
        {
           var disposalsCount03 = disposalsIdsRows0203
                .Where(x => x.TypeBase == TypeBase.CitizenStatement)
                .Where(x => x.TypeAgreementProsecutor == TypeAgreementProsecutor.NotRequiresAgreement);
            return disposalsCount03.Count();
        }

        private int GetCell7_5()
        {
            var disposalsCount07 = disposalsIdsRows0203
                                   .Count(x => x.TypeBase == TypeBase.ProsecutorsClaim);
            return disposalsCount07;
        }

        private int GetCell8_5()
        {
            disposalsIdsRows0809 = disposalsIdsRows0203
                                   .Where(x => x.TypeBase == TypeBase.ProsecutorsClaim)
                                   .ToList();

            var start = 1000;
            var servBaseProsClaims = this.Container.Resolve<IDomainService<BaseProsClaim>>().GetAll();

            var tmpFirst = inspectionsIds.Length > start ? inspectionsIds.Take(1000).ToArray() : inspectionsIds;

            // требования прокуратуры
            var baseProsClaims =
                servBaseProsClaims.Where(x => tmpFirst.Contains(x.Id))
                            .Select(x => new { x.TypeBaseProsClaim, claimId = x.Id })
                            .ToList();

            while (start < inspectionsIds.Length)
            {
                var tmp = inspectionsIds.Skip(start).Take(1000).ToArray();

                baseProsClaims.AddRange(servBaseProsClaims.Where(x => tmp.Contains(x.Id))
                            .Select(x => new { x.TypeBaseProsClaim, claimId = x.Id })
                            .ToList());
                start += 1000;
            }

            // требования с типом Предоставление специалиста
            var baseProsClaimsProvidingSpecialistIds =
                baseProsClaims.Where(x => x.TypeBaseProsClaim == TypeBaseProsClaim.ProvidingSpecialist)
                              .Select(x => x.claimId)
                              .ToList();

            // требования с типом Проведение проверки
            this.baseProsClaimsExecutionInspection =
                baseProsClaims.Where(x => x.TypeBaseProsClaim == TypeBaseProsClaim.ExecutionInspection)
                              .Select(x => x.claimId)
                              .ToList();

            // количество распоряжений, 1. У которых в поле "Вид проверки" значения с кодами 2, 4, 9
            // 2. В поле "Основание обследования" значение = Требование прокуратуры, 
            // 3. У зависимого требования прокуратуры в поле "Тип требования: значение = Предоставление специалиста
            var disposalsCount08 = disposalsIdsRows0809
                .Where(x => baseProsClaimsProvidingSpecialistIds.Contains(x.InspectionId))
                .Select(x => x.Id)
                .Distinct()
                .Count();
            return disposalsCount08;
        }

        private int GetCell9_5()
        {
            var disposalsCount09 = disposalsIdsRows0809
                .Where(x => this.baseProsClaimsExecutionInspection.Contains(x.InspectionId))
                .Select(x => x.Id)
                .Distinct()
                .Count();
            return disposalsCount09;
        }

        private int GetCell10_5()
        {
            var disposalsCount10 = disposalsIdsPart1
                                   .Where(x => x.TypeBase == TypeBase.ProsecutorsClaim)
                                   .Select(x => x.Id)
                                   .Distinct()
                                   .Count();
            return disposalsCount10;
        }

        private int GetCell11_5()
        {
            var disposalsCount11 = disposalsIdsPart1
                       .Count(x => x.Code == TypeCheck.PlannedDocumentation);
            return disposalsCount11;
        }

        private int GetCell12_5()
        {
            var start = 1000;
            var servDisposalExperts = this.Container.Resolve<IDomainService<DisposalExpert>>().GetAll();

            var tmpFirst = disposalsIds.Length > start ? disposalsIds.Take(1000).ToArray() : disposalsIds;

            // требования прокуратуры
            this.disposalExperts =
                servDisposalExperts.Where(x => tmpFirst.Contains(x.Disposal.Id))
                            .Where(x => x.Disposal.KindCheck.Code != TypeCheck.InspectionSurvey
                            && x.Disposal.KindCheck.Code != TypeCheck.Monitoring)
                            .Select(x => new DisposalWithExpert() { Id = x.Disposal.Id, Code = x.Disposal.KindCheck.Code})
                            .ToList();

            while (start < disposalsIds.Length)
            {
                var tmp = disposalsIds.Skip(start).Take(1000).ToArray();

                disposalExperts.AddRange(servDisposalExperts.Where(x => tmp.Contains(x.Disposal.Id))
                            .Where(x => x.Disposal.KindCheck.Code != TypeCheck.InspectionSurvey
                                    && x.Disposal.KindCheck.Code != TypeCheck.Monitoring)
                            .Select(x => new DisposalWithExpert() { Id = x.Disposal.Id, Code = x.Disposal.KindCheck.Code })
                            .ToList());
                start += 1000;
            }

            var disposalExpertsIds = disposalExperts.Select(x => x.Id).ToList();
            
            var disposalsCount12 = disposalsIdsPart1
                    .Where(x => disposalExpertsIds.Contains(x.Id))
                    .Where(x => x.Code != TypeCheck.InspectionSurvey && x.Code != TypeCheck.Monitoring)
                    .Select(x => x.Id)
                    .Distinct()
                    .Count();
            return disposalsCount12;
        }

        private int GetCell13_5()
        {
            var disposalExpertsIds = disposalExperts.Select(x => x.Id).ToList();
            
            var disposalsCount13 = disposalsIdsPart1
                    .Where(x => disposalExpertsIds.Contains(x.Id))
                    .Where(x => x.Code == TypeCheck.NotPlannedDocumentation
                    || x.Code == TypeCheck.NotPlannedExit
                    || x.Code == TypeCheck.NotPlannedDocumentationExit)
                    .Where(x => x.Code != TypeCheck.InspectionSurvey && x.Code != TypeCheck.Monitoring)
                    .Select(x => x.Id)
                    .Distinct()
                    .Count();
            return disposalsCount13;
        }

        private int GetCell14_5()
        {
            var disposalsCount14 = this.disposalsIdsPart1
                       .Count(x => x.Code == TypeCheck.PlannedDocumentation || x.Code == TypeCheck.PlannedDocumentationExit);
            return disposalsCount14;
        }
        
        private class DisposalsIdsPart1
        {
            public long Id;

            public TypeAgreementProsecutor TypeAgreementProsecutor;

            public TypeBase TypeBase;

            public TypeCheck? Code;

            public long InspectionId;

            public long StageId;
        }

        private class DisposalWithExpert
        {
            public long Id;

            public TypeCheck Code;
        }
    }
}
