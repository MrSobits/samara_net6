namespace Bars.Gkh.Import.Fund
{
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;

    using Castle.Windsor;

    using System.Linq;

    using Ionic.Zip;

    public class TechPassportImport : ITechPassportImport
    {
        public IRepository<RealityObject> RealtyObjectRepository { get; set; }

        public IRepository<TehPassport> TehPassportRepository { get; set; }

        public IRepository<TehPassportValue> TehPassportValueRepository { get; set; }

        public IWindsorContainer Container { get; set; }

        public void Import(FileData zipFile, ILogImport logImport, bool replaceData)
        {
            // словарь соответствия идентификатора жилого дома и его технического паспорта
            var realtyObjectTechPassportDict = TehPassportRepository.GetAll()
                .Select(x => new { roId = x.RealityObject.Id, x.Id })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x.First().Id);

            // словарь существующих значений техпаспорта
            // id дома -> FormCode -> CellCode -> ExistingTechPassportValueProxy
            var realtyObjectTechPassportDataDict = TehPassportValueRepository.GetAll()
                .Select(x => new
                    {
                        roId = x.TehPassport.RealityObject.Id, 
                        x.FormCode,
                        x.CellCode,
                        x.Value,
                        x.Id,
                        tehPassportId = x.TehPassport.Id,
                        x.ObjectVersion,
                        x.ObjectCreateDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key, 
                    x => x.GroupBy(y => y.FormCode)
                          .ToDictionary(
                              y => y.Key,
                              y => y.GroupBy(z => z.CellCode)
                                    .ToDictionary(
                                        z => z.Key,
                                        z => z.Select(v => new ExistingTechPassportValueProxy
                                                               {
                                                                   Id = v.Id,
                                                                   ObjectCreateDate = v.ObjectCreateDate,
                                                                   ObjectVersion = v.ObjectVersion,
                                                                   TechPassportId = v.tehPassportId,
                                                                   FormCode = v.FormCode,
                                                                   CellCode = v.CellCode,
                                                                   Value = v.Value
                                                               }).First())));

            //словарь соответствия федерального номера и идентификатора жилого дома в нашей системе
            var dictRobject = this.RealtyObjectRepository.GetAll()
                .Where(x => x.FederalNum != null)
                .Select(x => new { x.Id, x.FederalNum })
                .AsEnumerable()
                .GroupBy(x => x.FederalNum)
                .ToDictionary(x => x.Key, y => y.First().Id);
            
            var importParts = Container.ResolveAll<ITechPassportPartImport>();

            var failedList = new List<string>();

            foreach (var importPart in importParts)
            {
                bool successfullyUnzipped;

                using (var memoryStream = GetFileMemoryStream(zipFile.Data, importPart.Code, "csv", out successfullyUnzipped))
                {
                    if (!successfullyUnzipped)
                    {
                        failedList.Add(importPart.Code);
                    }
                    else
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        importPart.Import(memoryStream, logImport, dictRobject, realtyObjectTechPassportDict, realtyObjectTechPassportDataDict, replaceData);
                    }
                }
            }

            foreach (var failed in failedList)
            {
                logImport.Warn(failed, string.Format("Не удалось распаковать файл {0} из архива", failed));
            }
        }

        private MemoryStream GetFileMemoryStream(byte[] data, string filePrefix, string fileExtension, out bool success)
        {
            var result = new MemoryStream();
            success = false;

            using (var zipFile = ZipFile.Read(new MemoryStream(data)))
            {
                var zipEntry = zipFile.FirstOrDefault(x => x.FileName.Contains(filePrefix) && x.FileName.EndsWith(fileExtension));
                if (zipEntry != null)
                {
                    zipEntry.Extract(result);
                    result.Seek(0, SeekOrigin.Begin);
                    success = true;
                }
            }

            return result;
        }
    }
}