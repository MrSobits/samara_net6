namespace Bars.Gkh.Regions.Yanao.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Yanao.Entities;

    public class RealityObjectExtensionService : IRealityObjectExtensionService
    {
        public IDomainService<RealityObjectExtension> serviceRealityObjectExtension { get; set; }

        public IRepository<RealityObject> RealityObjectRepository{ get; set; }

        public IFileManager FileManager { get; set; }

        public IDomainService<FileInfo> FileInfoService { get; set; }

        public IDataResult SaveTechPassportScanFile(BaseParams baseParams)
        {
            const string fileField = "TechPassportScanFile";

            var filedata = baseParams.Files.ContainsKey(fileField) ? baseParams.Files[fileField] : null;

            if (baseParams.Params.ContainsKey("records"))
            {
                var records = baseParams.Params["records"];

                if ((records is List<object>) && (records as List<object>).Any())
                {
                    var record = (records as List<object>).First();

                    if (record is B4.Utils.DynamicDictionary)
                    {
                        var dict = record as B4.Utils.DynamicDictionary;

                        var roId = dict.GetAs<long>("Id");
                        var hasFile = dict.ContainsKey(fileField) && dict[fileField] != null;

                        DoSave(roId, hasFile, filedata);
                    }
                }
            }

            return new BaseDataResult();
        }

        private void DoSave(long roId, bool hasFile, FileData fileData)
        {
            var extension = this.serviceRealityObjectExtension.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId);

            if (extension != null)
            {
                if (fileData != null)
                {
                    var oldFileId = extension.TechPassportScanFile.Id;

                    var fileInfo = this.FileManager.SaveFile(fileData);

                    extension.TechPassportScanFile = fileInfo;

                    this.serviceRealityObjectExtension.Update(extension);

                    this.FileInfoService.Delete(oldFileId);
                }
                else if (!hasFile)
                {
                    this.serviceRealityObjectExtension.Delete(extension.Id);
                }
            }
            else if (fileData != null)
            {
                var fileInfo = this.FileManager.SaveFile(fileData);

                var realityObjectExtension = new RealityObjectExtension
                        {
                            RealityObject = this.RealityObjectRepository.Load(roId),
                            TechPassportScanFile = fileInfo
                        };

                this.serviceRealityObjectExtension.Save(realityObjectExtension);
            }
        }
    }
}