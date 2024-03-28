namespace Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4.Modules.FIAS.AutoUpdater.Helpers;
    using Bars.B4.Utils;

    using SharpCompress.Archives;

    /// <summary>
    /// Сервис работы с архивом ФИАС формата .gar
    /// </summary>
    internal class FiasGarArchiveReader : FiasArchiveReader
    {
        /// <inheritdoc />
        public override IDictionary<string, string> FiasLinkedFilesDict { get; set; } = new Dictionary<string, string>
        {
            { "as_addr_obj", null },
            { "as_addr_obj_params", null },
            { "as_mun_hierarchy", null },
            { "as_adm_hierarchy", null }
        };

        /// <inheritdoc />
        public override IDictionary<string, string> FiasHouseLinkedFilesDict { get; set; } = new Dictionary<string, string>
        {
            { "as_houses", null },
            { "as_addr_obj", null },
            { "as_houses_params", null },
            { "as_mun_hierarchy", null }
        };

        /// <inheritdoc />
        public override IDataResult UnpackFiles(string archPath, string unpackDir = null)
        {
            try
            {
                unpackDir = Path.GetDirectoryName(archPath);
                var dateRegularExp = new Regex(@"^" + RegionCodeHelper.GetRegionCode() + @"/\w*_(\d{8})_\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\.XML");
                var combinedFiasFilesKeys = this.FiasLinkedFilesDict
                    .Union(this.FiasHouseLinkedFilesDict)
                    .Select(x => x.Key)
                    .ToList();

                int count = 0;
                using (var arch = ArchiveFactory.Open(archPath))
                {
                    var uploadDate = arch.Entries
                        .Select(x => dateRegularExp.Match(x.Key).Groups[1].Value)
                        .Distinct()
                        .OrderByDescending(x => x)
                        .FirstOrDefault();

                    arch.Entries
                        .Where(x => combinedFiasFilesKeys
                            .Any(y => x.Key.ToLower().Contains($"{RegionCodeHelper.GetRegionCode()}/{y}_{uploadDate}")))
                        .Select(x => new
                        {
                            archiveEntry = x,
                            key = combinedFiasFilesKeys.FirstOrDefault(y => x.Key.ToLower().Contains($"{y}_{uploadDate}"))
                        })
                        .ForEach(x =>
                        {
                            var filePath = UnpackFile(x.archiveEntry, unpackDir);
                                
                            if (this.FiasLinkedFilesDict.ContainsKey(x.key))
                            {
                                FiasLinkedFilesDict[x.key] = filePath;
                            }

                            if (this.FiasHouseLinkedFilesDict.ContainsKey(x.key))
                            {
                                FiasHouseLinkedFilesDict[x.key] = filePath;
                            }

                            count++;
                        });
                }

                if (count != combinedFiasFilesKeys.Count)
                {
                    throw new FileNotFoundException("Не удалось распаковать все необходимые файлы");
                }

                return new BaseDataResult();
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, ex.Message);
            }
        }
    }
}