using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Voronezh.Entities;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Ionic.Zip;
using Ionic.Zlib;
using System.Text;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
using Bars.Gkh.FileManager;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class FileRegisterService : IFileRegisterService
    {
        public IDomainService<FileRegister> FileRegisterDomain { get; set; }
        public IDomainService<AppealCitsRealityObject> AppealCitsRoDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }
        public IDomainService<ActCheckAnnex> ActCheckAnnexDomain { get; set; }
        public IDomainService<ActSurveyAnnex> ActSurveyAnnexDomain { get; set; }
        public IDomainService<DisposalAnnex> DisposalAnnexDomain { get; set; }
        public IDomainService<PrescriptionAnnex> PrescriptionAnnexDomain { get; set; }
        public IDomainService<PresentationAnnex> PresentationAnnexDomain { get; set; }
        public IDomainService<ProtocolAnnex> ProtocolAnnexDomain { get; set; }
        public IDomainService<ResolutionAnnex> ResolutionAnnexDomain { get; set; }
        public IDomainService<ActRemovalAnnex> ActRemovalAnnexDomain { get; set; }
        public IDomainService<Protocol197Annex> Protocol197AnnexDomain { get; set; }
        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }

        private IFileManager _fileManager;

        public FileRegisterService(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool FormArchieve(FileRegister requestData, IProgressIndicator indicator = null)
        {
            try
            {
                var inspections = InspectionRoDomain.GetAll()
                    .Where(x => x.RealityObject.Id == requestData.RealityObject.Id)
                    .WhereIf(requestData.DateFrom.HasValue, x=> x.Inspection.ObjectCreateDate>=requestData.DateFrom.Value)
                    .WhereIf(requestData.DateTo.HasValue, x => x.Inspection.ObjectCreateDate <= requestData.DateTo.Value)
                    .Select(x => x.Inspection.Id).Distinct().ToList();

                var appeals = AppealCitsRoDomain.GetAll()
                    .Where(x => x.RealityObject.Id == requestData.RealityObject.Id)
                    .WhereIf(requestData.DateFrom.HasValue, x => x.AppealCits.DateFrom >= requestData.DateFrom)
                    .WhereIf(requestData.DateTo.HasValue, x => x.AppealCits.DateFrom <= requestData.DateTo.Value)
                    .Select(x => x.AppealCits.Id).Distinct().ToList();

                var appCitsFiles = AppealCitsRoDomain.GetAll()
                    .Where(x => x.RealityObject.Id == requestData.RealityObject.Id && x.AppealCits.File != null)
                    .WhereIf(requestData.DateFrom.HasValue, x => x.AppealCits.DateFrom >= requestData.DateFrom)
                    .WhereIf(requestData.DateTo.HasValue, x => x.AppealCits.DateFrom <= requestData.DateTo.Value)
                    .Select(x => x.AppealCits.File).ToList();

                var actCheckFiles = ActCheckAnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.ActCheck.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var actSurveyFiles = ActSurveyAnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.ActSurvey.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var disposalFiles = DisposalAnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.Disposal.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var prescriptionFiles = PrescriptionAnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.Prescription.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var presentationFiles = PresentationAnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.Presentation.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var protocolFiles = ProtocolAnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.Protocol.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var resolutionFiles = ResolutionAnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.Resolution.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var actRemovalFiles = ActRemovalAnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.ActRemoval.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var protocol197Files = Protocol197AnnexDomain.GetAll()
                    .Where(x => inspections.Contains(x.Protocol197.Inspection.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var appealCitsAnswerFiles = AppealCitsAnswerDomain.GetAll()
                    .Where(x => x.Addressee != null && appeals.Contains(x.AppealCits.Id) && x.File != null)
                    .Select(x => x.File).ToList();

                var fileZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                appCitsFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Файлы обращений/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                appealCitsAnswerFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Файлы обращений/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                actCheckFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Акты проверок/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }       
                    catch
                    {

                    }
                });

                actSurveyFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Акты обследований/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                disposalFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Приказы/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                prescriptionFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Предписания/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                presentationFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Представления/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                protocolFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Протоколы/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                resolutionFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Постановления/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                actRemovalFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Акты проверки предписаний/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                protocol197Files.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/Протоколы по ст.19.7 КоАП РФ/{x.Name}.{x.Extention}", this._fileManager.GetFile(x));
                    }
                    catch
                    {

                    }
                });

                using (var ms = new MemoryStream())
                {
                    fileZip.Save(ms);

                    var file = _fileManager.SaveFile(ms, $"{requestData.RealityObject.Address}.zip");

                    requestData.File = file;
                    FileRegisterDomain.Update(requestData);
                }
                return true;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception e)
            {

            }

            return false;
        }
    }
}