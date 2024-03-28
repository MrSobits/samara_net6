namespace Bars.Gkh.RegOperator.Domain.ImportExport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using B4;
    using B4.Utils;
    using B4.Utils.Annotations;
    using B4.IoC;

    using Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps;

    using Castle.Windsor;
    using Mapping;
    using Serializers;

    public class ImportExportDataProvider
    {
        public IWindsorContainer Container { get; set; }

        public ImportResult<T> Deserialize<T>(FileData fileData,
            DynamicDictionary @params,
            string providerCode,
            string serializerCode = "default") where T : class
        {
            ArgumentChecker.NotNull(fileData, "Импортируемый файл не может быть пустым", "fileData");
            ArgumentChecker.NotNullAndLengthNotLessThan(fileData.Data, 1, "Нет данных для чтения", "fileData");

            var importMap = ImportMapHelper.GetMapByKey(providerCode, Container);

            if (Container.Kernel.HasComponent(typeof (IImportExportSerializer<T>)))
            {
                serializerCode = serializerCode.ToLower();
                var serializers = Container.ResolveAll<IImportExportSerializer<T>>();

                using (Container.Using((object)serializers))
                {
                    var serializer = serializers.FirstOrDefault(x => x.Code == serializerCode)
                                     ?? serializers.FirstOrDefault(x => x.Code == "default");
                    if (serializer == null)
                        throw new NotImplementedException(
                            "Сериализатор с кодом '{0}' не найден".FormatUsing(serializerCode));

                    var ms = new MemoryStream(fileData.Data);
                    var result = serializer.Deserialize(ms, importMap, fileData.FileName, @params);

                    this.FillImportInfo(importMap, result);

                    return result;
                }
            }

            return new ImportResult<T>();
        }

        public Stream Serialize<T>(List<T> data, IImportMap format) where T : class
        {
            ArgumentChecker.NotNull(data, "Нет данных для сериализации", "data");
            ArgumentChecker.NotNull(format, "Не определен формат для сериализации", "format");

            if (Container.Kernel.HasComponent(typeof (IImportExportSerializer<T>)))
            {
                var serializer = Container.Resolve<IImportExportSerializer<T>>();

                using (Container.Using(serializer))
                {
                    return serializer.Serialize(data, format);
                }
            }

            return null;
        }

        private void FillImportInfo<T>(IImportMap importMap, ImportResult<T> importResult)
            where T : class
        {
            if (importMap.Is<PaPaymentInfoInFsGorodImportMap>())
            {
                importResult.GeneralData["importType"] = importResult.GeneralData["FsGorodName"];
            }
            else
            {
                importResult.GeneralData["importType"] = importMap.GetName();
            }
        } 
    }
}