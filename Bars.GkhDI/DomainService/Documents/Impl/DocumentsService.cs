namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class DocumentsService : IDocumentsService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetIdByDisnfoId(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

                var disclosureInfo = Container.Resolve<IDomainService<DisclosureInfo>>()
                    .GetAll()
                    .Where(x => x.Id == disclosureInfoId)
                    .Select(x => new
                    {
                        x.PeriodDi,
                        x.ManagingOrganization.TypeManagement,
                        ContragentName = x.ManagingOrganization.Contragent.Name,
                        PeriodDiName = x.PeriodDi.Name
                    }).FirstOrDefault();

                var service = this.Container.Resolve<IDomainService<Documents>>();

                var documentsId = service
                    .GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                return new BaseDataResult(new { Id = documentsId, disclosureInfo })
                    {
                        Success = true
                    };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        public IDataResult CopyDocs(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var choosenPeriodDiId = baseParams.Params.GetAs<long>("choosenPeriodDiId");
                var projectContract = baseParams.Params["projectContract"].ToBool();
                var communalService = baseParams.Params["communalService"].ToBool();
                var apartmentService = baseParams.Params["apartmentService"].ToBool();

                var service = this.Container.Resolve<IDomainService<Documents>>();

                var currentDocuments = service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId);

                if (currentDocuments == null)
                {
                    currentDocuments = new Documents
                        {
                            DisclosureInfo =
                                this.Container.Resolve<IDomainService<DisclosureInfo>>().Get(disclosureInfoId)
                        };
                    service.Save(currentDocuments);
                }

                var fromDocuments = service
                    .GetAll()
                    .FirstOrDefault(
                        x =>
                        x.DisclosureInfo.ManagingOrganization.Id ==
                        currentDocuments.DisclosureInfo.ManagingOrganization.Id
                        && x.DisclosureInfo.PeriodDi.Id == choosenPeriodDiId);

                if (fromDocuments != null)
                {
                    var fileManager = this.Container.Resolve<IFileManager>();

                    if (projectContract)
                    {
                        if (currentDocuments.FileProjectContract != null)
                        {
                            currentDocuments.FileProjectContract = null;
                            service.Update(currentDocuments);
                        }

                        currentDocuments.FileProjectContract = this.ReCreateFile(fromDocuments.FileProjectContract, fileManager);
                        currentDocuments.DescriptionProjectContract = fromDocuments.DescriptionProjectContract;
                    }

                    if (communalService)
                    {
                        if (currentDocuments.FileCommunalService != null)
                        {
                            currentDocuments.FileCommunalService = null;
                            service.Update(currentDocuments);
                        }

                        currentDocuments.FileCommunalService = this.ReCreateFile(fromDocuments.FileCommunalService, fileManager);
                        currentDocuments.DescriptionCommunalTariff = fromDocuments.DescriptionCommunalTariff;
                        currentDocuments.DescriptionCommunalCost = fromDocuments.DescriptionCommunalCost;
                    }

                    if (apartmentService)
                    {
                        if (currentDocuments.FileServiceApartment != null)
                        {
                            currentDocuments.FileServiceApartment = null;
                            service.Update(currentDocuments);
                        }

                        currentDocuments.FileServiceApartment = this.ReCreateFile(fromDocuments.FileServiceApartment, fileManager);
                    }
                }

                service.Update(currentDocuments);

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        protected FileInfo ReCreateFile(FileInfo fileInfo, IFileManager fileManager)
        {
            if (fileInfo == null)
            {
                return null;
            }

            var fileInfoStream = fileManager.GetFile(fileInfo);
            var newFileInfo = fileManager.SaveFile(fileInfoStream, string.Format("{0}.{1}", fileInfo.Name, fileInfo.Extention));
            return newFileInfo;
        }
    }
}
