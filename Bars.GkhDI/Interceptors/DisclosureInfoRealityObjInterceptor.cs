namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class DisclosureInfoRealityObjInterceptor : EmptyDomainInterceptor<DisclosureInfoRealityObj>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<DisclosureInfoRealityObj> service, DisclosureInfoRealityObj entity)
        {
            var documentsRealityObjService = this.Container.Resolve<IDomainService<DocumentsRealityObj>>();
            var documentsRealityObjIds = documentsRealityObjService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in documentsRealityObjIds)
            {
                documentsRealityObjService.Delete(id);
            }

            var disInfoRelationService = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            var disInfoRelationIds = disInfoRelationService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in disInfoRelationIds)
            {
                disInfoRelationService.Delete(id);
            }

            var finActivityRealityObjCommunalService = this.Container.Resolve<IDomainService<FinActivityRealityObjCommunalService>>();
            var finActivityRealityObjCommunalIds = finActivityRealityObjCommunalService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in finActivityRealityObjCommunalIds)
            {
                finActivityRealityObjCommunalService.Delete(id);
            }

            var infoAboutPaymentCommunalService = this.Container.Resolve<IDomainService<InfoAboutPaymentCommunal>>();
            var infoAboutPaymentCommunalIds = infoAboutPaymentCommunalService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in infoAboutPaymentCommunalIds)
            {
                infoAboutPaymentCommunalService.Delete(id);
            }

            var infoAboutPaymentHousingService = this.Container.Resolve<IDomainService<InfoAboutPaymentHousing>>();
            var infoAboutPaymentHousingIds = infoAboutPaymentHousingService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in infoAboutPaymentHousingIds)
            {
                infoAboutPaymentHousingService.Delete(id);
            }

            var infoAboutReductionPaymentService = this.Container.Resolve<IDomainService<InfoAboutReductionPayment>>();
            var infoAboutReductionPaymentIds = infoAboutReductionPaymentService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in infoAboutReductionPaymentIds)
            {
                infoAboutReductionPaymentService.Delete(id);
            }

            var infoAboutUseCommonFacilitiesService = this.Container.Resolve<IDomainService<InfoAboutUseCommonFacilities>>();
            var infoAboutUseCommonFacilitiesIds = infoAboutUseCommonFacilitiesService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in infoAboutUseCommonFacilitiesIds)
            {
                infoAboutUseCommonFacilitiesService.Delete(id);
            }

            var nonResidentialPlacementService = this.Container.Resolve<IDomainService<NonResidentialPlacement>>();
            var nonResidentialPlacementIds = nonResidentialPlacementService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in nonResidentialPlacementIds)
            {
                nonResidentialPlacementService.Delete(id);
            }

            var archiveDiRoPercentService = this.Container.Resolve<IDomainService<ArchiveDiRoPercent>>();
            var archiveDiRoPercentIds = archiveDiRoPercentService.GetAll().Where(x => x.DiRealityObject.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in archiveDiRoPercentIds)
            {
                archiveDiRoPercentService.Delete(id);
            }

            var diRealObjPercentService = this.Container.Resolve<IDomainService<DiRealObjPercent>>();
            var diRealObjPercentIds = diRealObjPercentService.GetAll().Where(x => x.DiRealityObject.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in diRealObjPercentIds)
            {
                diRealObjPercentService.Delete(id);
            }

            var planReductionExpenseService = this.Container.Resolve<IDomainService<PlanReductionExpense>>();
            var planReductionExpenseIds = planReductionExpenseService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in planReductionExpenseIds)
            {
                planReductionExpenseService.Delete(id);
            }

            var planWorkServiceRepairService = this.Container.Resolve<IDomainService<PlanWorkServiceRepair>>();
            var planWorkServiceRepairIds = planWorkServiceRepairService.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in planWorkServiceRepairIds)
            {
                planWorkServiceRepairService.Delete(id);
            }

            var otherServiceRep = this.Container.Resolve<IDomainService<OtherService>>();
            var otherServiceIds = otherServiceRep.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in otherServiceIds)
            {
                otherServiceRep.Delete(id);
            }

            var communalServiceRep = this.Container.Resolve<IDomainService<CommunalService>>();
            var communalServiceIds = communalServiceRep.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in communalServiceIds)
            {
                communalServiceRep.Delete(id);
            }

            var housingServiceRep = this.Container.Resolve<IDomainService<HousingService>>();
            var housingServiceIds = housingServiceRep.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in housingServiceIds)
            {
                housingServiceRep.Delete(id);
            }

            var repairServiceRep = this.Container.Resolve<IDomainService<RepairService>>();
            var repairServiceIds = repairServiceRep.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in repairServiceIds)
            {
                repairServiceRep.Delete(id);
            }

            var capRepairServiceRep = this.Container.Resolve<IDomainService<CapRepairService>>();
            var capRepairServiceIds = capRepairServiceRep.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in capRepairServiceIds)
            {
                capRepairServiceRep.Delete(id);
            }

            var controlServiceRep = this.Container.Resolve<IDomainService<ControlService>>();
            var controlServiceIds = controlServiceRep.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in controlServiceIds)
            {
                controlServiceRep.Delete(id);
            }

            var additionalServiceRep = this.Container.Resolve<IDomainService<AdditionalService>>();
            var additionalServiceIds = additionalServiceRep.GetAll().Where(x => x.DisclosureInfoRealityObj.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in additionalServiceIds)
            {
                additionalServiceRep.Delete(id);
            }

            return this.Success();
        }
    }
}
