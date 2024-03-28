namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class DocumentsRealityObjService : IDocumentsRealityObjService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetIdByDisnfoId(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

                var disclosureInfoRealityObj = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>().Get(disclosureInfoRealityObjId);

                var service = this.Container.Resolve<IDomainService<DocumentsRealityObj>>();

                var documentsId = service.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                return new BaseDataResult(new { Id = documentsId, disclosureInfoRealityObj })
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
                var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
                var choosenPeriodDiId = baseParams.Params.GetAs<long>("choosenPeriodDiId");
                var act = baseParams.Params["act"].ToBool();
                var listWork = baseParams.Params["listWork"].ToBool();
                var report = baseParams.Params["report"].ToBool();

                var service = this.Container.Resolve<IDomainService<DocumentsRealityObj>>();

                var currentDocumentsRealityObj = service
                    .GetAll()
                    .FirstOrDefault(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId);

                if (currentDocumentsRealityObj == null)
                {
                    var disclosureInfoRealityObj = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>().GetAll().FirstOrDefault(x => x.Id == disclosureInfoRealityObjId);

                    if (disclosureInfoRealityObj == null)
                    {
                        throw new Exception("Не удалось получить раскрытие по дому");
                    }

                    currentDocumentsRealityObj = new DocumentsRealityObj
                    {
                        DisclosureInfoRealityObj = new DisclosureInfoRealityObj 
                        { 
                            Id = disclosureInfoRealityObjId,
                            RealityObject = new RealityObject { Id = disclosureInfoRealityObj.RealityObject.Id}
                        }
                    };
                    service.Save(currentDocumentsRealityObj);
                }

                var fromDocumentsRealityObj = service
                    .GetAll()
                    .FirstOrDefault(x => x.DisclosureInfoRealityObj.PeriodDi.Id == choosenPeriodDiId
                    && x.DisclosureInfoRealityObj.RealityObject.Id == currentDocumentsRealityObj.DisclosureInfoRealityObj.RealityObject.Id);

                if (fromDocumentsRealityObj != null)
                {
                    var fileManager = this.Container.Resolve<IFileManager>();

                    if (act)
                    {
                        if (currentDocumentsRealityObj.FileActState != null)
                        {
                            currentDocumentsRealityObj.FileActState = null;
                            service.Update(currentDocumentsRealityObj);
                        }

                        currentDocumentsRealityObj.FileActState = this.ReCreateFile(fromDocumentsRealityObj.FileActState, fileManager);
                        currentDocumentsRealityObj.DescriptionActState = fromDocumentsRealityObj.DescriptionActState;
                    }

                    if (listWork)
                    {
                        if (currentDocumentsRealityObj.FileCatalogRepair != null)
                        {
                            currentDocumentsRealityObj.FileCatalogRepair = null;
                            service.Update(currentDocumentsRealityObj);
                        }

                        currentDocumentsRealityObj.FileCatalogRepair = this.ReCreateFile(fromDocumentsRealityObj.FileCatalogRepair, fileManager);
                    }

                    if (report)
                    {
                        if (currentDocumentsRealityObj.FileReportPlanRepair != null)
                        {
                            currentDocumentsRealityObj.FileReportPlanRepair = null;
                            service.Update(currentDocumentsRealityObj);
                        }

                        currentDocumentsRealityObj.FileReportPlanRepair = this.ReCreateFile(fromDocumentsRealityObj.FileReportPlanRepair, fileManager);
                    }
                }

                service.Update(currentDocumentsRealityObj);

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
