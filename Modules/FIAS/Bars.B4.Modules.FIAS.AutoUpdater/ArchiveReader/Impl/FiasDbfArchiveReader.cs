namespace Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Modules.FIAS.AutoUpdater.Helpers;
    using Bars.B4.Utils;

    using Castle.Core;
    using Castle.Windsor;

    using EcmaScript.NET;

    using SharpCompress.Archives;

    /// <summary>
    /// Сервис работы с архивом ФИАС формата .dbf
    /// </summary>
    internal class FiasDbfArchiveReader : FiasArchiveReader
    {
        /// <inheritdoc />
        public override IDictionary<string, string> FiasLinkedFilesDict { get; set; } = new Dictionary<string, string>()
        {
            { "addrob", null }
        };

        /// <inheritdoc />
        public override IDictionary<string, string> FiasHouseLinkedFilesDict { get; set; } = new Dictionary<string, string>()
        {
            { "house", null }
        };

        /// <inheritdoc />
        public override IDataResult UnpackFiles(string archPath, string unpackDir = null)
        {
            try
            {
                var combinedDict = this.FiasLinkedFilesDict
                    .Union(this.FiasHouseLinkedFilesDict)
                    .ToDictionary(x => x.Key, x => x.Value);
                unpackDir = Path.GetDirectoryName(archPath);

                int count = 0;
                using (var arch = ArchiveFactory.Open(archPath))
                {
                    arch.Entries
                        .Where(x => combinedDict.Keys.Select(y => $"{y}{RegionCodeHelper.GetRegionCode()}.dbf").Contains(x.Key.ToLower()))
                        .ForEach(x =>
                        {
                            var filePath = UnpackFile(x, unpackDir);
                            // Находим ключ в словаре по подстроке наименования файла в архиве({Длина имени} - {длина кода региона} - {длина ".dbf"})
                            var key = x.Key.ToLower().Remove(x.Key.Length - RegionCodeHelper.GetRegionCode().Length - 4);

                            if (this.FiasLinkedFilesDict.ContainsKey(key))
                            {
                                FiasLinkedFilesDict[key] = filePath;
                                count++;
                            }
                            
                            if (this.FiasHouseLinkedFilesDict.ContainsKey(key))
                            {
                                FiasHouseLinkedFilesDict[key] = filePath;
                                count++;
                            }
                        });

                    if (count != combinedDict.Count)
                    {
                        throw new FileNotFoundException("Не удалось распаковать все необходимые файлы");
                    }

                    return new BaseDataResult();
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}