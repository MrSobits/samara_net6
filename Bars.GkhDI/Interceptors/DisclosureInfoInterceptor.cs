namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;

    public class DisclosureInfoInterceptor : EmptyDomainInterceptor<DisclosureInfo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DisclosureInfo> service, DisclosureInfo entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);
            if (entity.HasLicense == 0)
            {
                entity.HasLicense = YesNoNotSet.NotSet;
            }
            return Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<DisclosureInfo> service, DisclosureInfo entity)
        {
            // Сразу сохраняем финансовую деятельность раскрытия
            var finActivity = new FinActivity
            {
                Id = 0,
                DisclosureInfo = entity
            };
            this.Container.Resolve<IDomainService<FinActivity>>().Save(finActivity);

            // Сразу сохраняем документы финансовой деятельности раскрытия
            var finActivityDocs = new FinActivityDocs
            {
                Id = 0,
                DisclosureInfo = entity
            };
            this.Container.Resolve<IDomainService<FinActivityDocs>>().Save(finActivityDocs);

            // Сразу сохраняем документы раскрытия
            var documents = new Documents
            {
                Id = 0,
                DisclosureInfo = entity,
                NotAvailable = false
            };
            this.Container.Resolve<IDomainService<Documents>>().Save(documents);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<DisclosureInfo> service, DisclosureInfo entity)
        {
            var disInfoRelationService = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            var disInfoRelationIds = disInfoRelationService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in disInfoRelationIds)
            {
                disInfoRelationService.Delete(id);
            }

            var adminRespService = this.Container.Resolve<IDomainService<AdminResp>>();
            var adminRespIds = adminRespService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in adminRespIds)
            {
                adminRespService.Delete(id);
            }

            var documentsService = this.Container.Resolve<IDomainService<Documents>>();
            var documentsIds = documentsService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in documentsIds)
            {
                documentsService.Delete(id);
            }

            var finActivityService = this.Container.Resolve<IDomainService<FinActivity>>();
            var finActivityIds = finActivityService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in finActivityIds)
            {
                finActivityService.Delete(id);
            }

            var finActivityCommunalService = this.Container.Resolve<IDomainService<FinActivityCommunalService>>();
            var finActivityCommunalServiceIds = finActivityCommunalService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in finActivityCommunalServiceIds)
            {
                finActivityCommunalService.Delete(id);
            }

            var finActivityDocsService = this.Container.Resolve<IDomainService<FinActivityDocs>>();
            var finActivityDocsIds = finActivityDocsService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in finActivityDocsIds)
            {
                finActivityDocsService.Delete(id);
            }

            var finActivityManagCategoryService = this.Container.Resolve<IDomainService<FinActivityManagCategory>>();
            var finActivityManagCategoryIds = finActivityManagCategoryService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in finActivityManagCategoryIds)
            {
                finActivityManagCategoryService.Delete(id);
            }

            var finActivityManagRealityObjService = this.Container.Resolve<IDomainService<FinActivityManagRealityObj>>();
            var finActivityManagRealityObjIds = finActivityManagRealityObjService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in finActivityManagRealityObjIds)
            {
                finActivityManagRealityObjService.Delete(id);
            }

            var finActivityRepairCategoryService = this.Container.Resolve<IDomainService<FinActivityRepairCategory>>();
            var finActivityRepairCategoryIds = finActivityRepairCategoryService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in finActivityRepairCategoryIds)
            {
                finActivityRepairCategoryService.Delete(id);
            }

            var finActivityRepairSourceService = this.Container.Resolve<IDomainService<FinActivityRepairSource>>();
            var finActivityRepairSourceIds = finActivityRepairSourceService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in finActivityRepairSourceIds)
            {
                finActivityRepairSourceService.Delete(id);
            }

            var fundsInfoService = this.Container.Resolve<IDomainService<FundsInfo>>();
            var fundsInfoIds = fundsInfoService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in fundsInfoIds)
            {
                fundsInfoService.Delete(id);
            }

            var groupDiService = this.Container.Resolve<IDomainService<GroupDi>>();
            var groupDiIds = groupDiService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in groupDiIds)
            {
                groupDiService.Delete(id);
            }

            var informationOnContractsService = this.Container.Resolve<IDomainService<InformationOnContracts>>();
            var informationOnContractsIds = informationOnContractsService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in informationOnContractsIds)
            {
                informationOnContractsService.Delete(id);
            }

            var archiveDiPercentService = this.Container.Resolve<IDomainService<ArchiveDiPercent>>();
            var archiveDiPercentIds = archiveDiPercentService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in archiveDiPercentIds)
            {
                archiveDiPercentService.Delete(id);
            }

            var disclosureInfoPercentService = this.Container.Resolve<IDomainService<DisclosureInfoPercent>>();
            var disclosureInfoPercentIds = disclosureInfoPercentService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in disclosureInfoPercentIds)
            {
                disclosureInfoPercentService.Delete(id);
            }

            var disclosureInfoRelationService = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();

            var disclosureInfoRelationIds = disclosureInfoRelationService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in disclosureInfoRelationIds)
            {
                disclosureInfoRelationService.Delete(id);
            }

            var diRealObjIds = disclosureInfoRelationService.GetAll().Where(x => x.DisclosureInfo.Id == entity.Id).Select(x => x.Id).ToArray();

            // выбираем id домов, которые находятся под управлением только удаляемого УК
            var diRealObjIdsHasOneRelation = disclosureInfoRelationService
                .GetAll()
                .Where(x => diRealObjIds.Contains(x.DisclosureInfoRealityObj.Id))
                .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                .ToDictionary(x => x.Key, y => y.Count())
                .Where(x => x.Value == 1)
                .Select(x => x.Key)
                .ToArray();

            var disclosureInfoRealityObjService = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();

            foreach (var id in diRealObjIdsHasOneRelation)
            {
                disclosureInfoRealityObjService.Delete(id);
            }

            return this.Success();
        }
    }
}
