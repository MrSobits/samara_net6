using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Ionic.Zip;
using Ionic.Zlib;
using System.Text;
using Bars.Gkh.FileManager;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.DomainService
{
    public class CrFileRegisterService : ICrFileRegisterService
    {
        public IDomainService<CrFileRegister> FileRegisterDomain { get; set; }
        public IDomainService<ContractCr> ContractCrDomain { get; set; }
        public IDomainService<BuildContract> BuildContractDomain { get; set; }
        public IDomainService<PerformedWorkAct> PerfWorkActDomain { get; set; }
        public IDomainService<BuildControlTypeWorkSmrFile> BuildControlFileDomain { get; set; }

        private IFileManager _fileManager;

        public CrFileRegisterService(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool FormArchieve(CrFileRegister requestData, IProgressIndicator indicator = null)
        {
            try
            {
                var contractFiles = ContractCrDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == requestData.RealityObject.Id && x.File != null)
                    .WhereIf(requestData.DateFrom.HasValue, x => x.File.ObjectCreateDate >= requestData.DateFrom.Value)
                    .WhereIf(requestData.DateTo.HasValue, x => x.File.ObjectCreateDate <= requestData.DateTo.Value)
                    .ToList();

                var buildContractFiles = BuildContractDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == requestData.RealityObject.Id && x.ProtocolFile != null)
                    .WhereIf(requestData.DateFrom.HasValue, x => x.ProtocolFile.ObjectCreateDate >= requestData.DateFrom.Value)
                    .WhereIf(requestData.DateTo.HasValue, x => x.ProtocolFile.ObjectCreateDate <= requestData.DateTo.Value)
                    .ToList();

                var perfWorkActCostFiles = PerfWorkActDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == requestData.RealityObject.Id && x.CostFile != null)
                    .WhereIf(requestData.DateFrom.HasValue, x => x.CostFile.ObjectCreateDate >= requestData.DateFrom.Value)
                    .WhereIf(requestData.DateTo.HasValue, x => x.CostFile.ObjectCreateDate <= requestData.DateTo.Value)
                    .ToList();

                var perfWorkActAdditionFiles = PerfWorkActDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == requestData.RealityObject.Id && x.AdditionFile != null)
                    .WhereIf(requestData.DateFrom.HasValue, x => x.AdditionFile.ObjectCreateDate >= requestData.DateFrom.Value)
                    .WhereIf(requestData.DateTo.HasValue, x => x.AdditionFile.ObjectCreateDate <= requestData.DateTo.Value)
                    .ToList();

                var perfWorkActDocFiles = PerfWorkActDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == requestData.RealityObject.Id && x.DocumentFile != null)
                    .WhereIf(requestData.DateFrom.HasValue, x => x.DocumentFile.ObjectCreateDate >= requestData.DateFrom.Value)
                    .WhereIf(requestData.DateTo.HasValue, x => x.DocumentFile.ObjectCreateDate <= requestData.DateTo.Value)
                    .ToList();

                var buildControlFiles = BuildControlFileDomain.GetAll()
                    .Where(x => x.BuildControlTypeWorkSmr.TypeWorkCr.ObjectCr.RealityObject.Id == requestData.RealityObject.Id && x.FileInfo != null)
                    .WhereIf(requestData.DateFrom.HasValue, x => x.FileInfo.ObjectCreateDate >= requestData.DateFrom.Value)
                    .WhereIf(requestData.DateTo.HasValue, x => x.FileInfo.ObjectCreateDate <= requestData.DateTo.Value)
                    .ToList();

                var fileZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                contractFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/{x.ObjectCr.ProgramCr.Name}/Договоры на услуги/{x.File.Name}.{x.File.Extention}", this._fileManager.GetFile(x.File));
                    }
                    catch
                    {
                        
                    }
                });

                buildContractFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/{x.ObjectCr.ProgramCr.Name}/Договоры подряда/{x.ProtocolFile.Name}.{x.ProtocolFile.Extention}", this._fileManager.GetFile(x.ProtocolFile));
                    }
                    catch
                    {
                        
                    }
                });

                perfWorkActCostFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/{x.ObjectCr.ProgramCr.Name}/Акт № {x.DocumentNum}/Справка о стоимости выполненных работ и затрат/{x.CostFile.Name}.{x.CostFile.Extention}", this._fileManager.GetFile(x.CostFile));
                    }
                    catch
                    {

                    }
                });

                perfWorkActAdditionFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/{x.ObjectCr.ProgramCr.Name}/Акт № {x.DocumentNum}/Приложение к акту/{x.AdditionFile.Name}.{x.AdditionFile.Extention}", this._fileManager.GetFile(x.AdditionFile));
                    }
                    catch
                    {

                    }
                });

                perfWorkActDocFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/{x.ObjectCr.ProgramCr.Name}/Акт № {x.DocumentNum}/Документ акта/{x.DocumentFile.Name}.{x.DocumentFile.Extention}", this._fileManager.GetFile(x.DocumentFile));
                    }
                    catch
                    {

                    }
                });

                buildControlFiles.ForEach(x =>
                {
                    try
                    {
                        fileZip.AddEntry($"{requestData.RealityObject.Address}/{x.BuildControlTypeWorkSmr.TypeWorkCr.ObjectCr.ProgramCr.Name}/Ход выполнения работ/{x.BuildControlTypeWorkSmr.TypeWorkCr.Work.Name}/{x.FileInfo.Name}.{x.FileInfo.Extention}", this._fileManager.GetFile(x.FileInfo));
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