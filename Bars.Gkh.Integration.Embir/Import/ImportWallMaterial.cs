namespace Bars.Gkh.Integration.Embir.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh.Integration.DataFetcher;

    using Castle.Windsor;
    using Gkh.Import.Impl;
    using Newtonsoft.Json.Linq;

    using CollectionExtensions = Castle.Core.Internal.CollectionExtensions;
    using StringExtensions = Bars.B4.Utils.StringExtensions;

    public class ImportWallMaterial : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key
        {
            get
            {
                return Id;
            }
        }

        public override string CodeImport
        {
            get
            {
                return "ImportEmbir";
            }
        }

        public override string Name
        {
            get
            {
                return "Импорт справочника 'Материалы стен' с ЕМБИР";
            }
        }

        public override string PossibleFileExtensions
        {
            get
            {
                return string.Empty;
            }
        }

        public override string PermissionName
        {
            get
            {
                return "Import.Embir.View";
            }
        }

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public IDomainService<WallMaterial> WallMaterialDomain { get; set; }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var webClientFetcher = new WebClientFetcher();
            var importHelper = new ImportIntegrationHelper(Container);

            var httpQueryBuilder = importHelper.GetHttpQueryBuilder();

            httpQueryBuilder.AddParameter("type", "DictWallMaterial");

            dynamic[] listWallMaterial;

            try
            {
                listWallMaterial = Enumerable.ToArray(webClientFetcher.GetData(httpQueryBuilder));
            }
            catch (Exception)
            {
                listWallMaterial = new JObject[0];
            }

            var wallmaterials = WallMaterialDomain
                .GetAll()
                .AsEnumerable();

            var wallMaterialsByExtId = wallmaterials
                .Where(x => x.ExternalId.IsNotEmpty())
                .GroupBy(x => x.ExternalId)
                .ToDictionary(x => x.Key, y => y.First());

            var wallMaterialsByName = wallmaterials
                .Where(x => StringExtensions.IsEmpty(x.ExternalId))
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, y => y.First());

            var listWallMaterialToSave = new List<WallMaterial>();

            foreach (var dynWallMaterial in listWallMaterial)
            {
                var wallMaterial = (JObject) (dynWallMaterial);

                var wallMaterialExtId = wallMaterial["Id"].ToStr();
                var wallMaterialName = wallMaterial["Name"].ToStr();

                if (!CollectionExtensions.IsNullOrEmpty(wallMaterialName))
                {
                    WallMaterial value;
                    if (wallMaterialsByExtId.TryGetValue(wallMaterialExtId, out value))
                    {
                        var existWallMaterial = value;
                        if (existWallMaterial.Name != wallMaterialName)
                        {
                            existWallMaterial.Name = wallMaterialName;
                            listWallMaterialToSave.Add(existWallMaterial);

                            LogImport.Info("Информация",
                                "Изменена запись в справочнике 'Материалы стен'. Старое значение: {0}. Новое значение из ЕМБИР: {1}"
                                    .FormatUsing(existWallMaterial.Name, wallMaterialName));
                            LogImport.CountChangedRows++;
                        }
                        else
                        {
                            LogImport.Info("Информация",
                                "В справочнике 'Материалы стен' уже есть запись с таким наименованием. Наименование: {0}".FormatUsing(wallMaterialName));
                        }

                        continue;
                    }

                    WallMaterial value1;
                    if (!wallMaterialsByName.TryGetValue(wallMaterialName, out value1))
                    {
                        listWallMaterialToSave.Add(new WallMaterial
                        {
                            Name = wallMaterialName,
                            ExternalId = wallMaterialExtId
                        });

                        LogImport.Info("Информация", "Добавлена запись в справочник 'Материалы стен'. Наименование: {0}".FormatUsing(wallMaterialName));
                        LogImport.CountAddedRows++;
                    }
                    else
                    {
                        var existWallMaterial = value1;
                        existWallMaterial.ExternalId = wallMaterialExtId;
                        listWallMaterialToSave.Add(existWallMaterial);

                        LogImport.Info("Информация",
                            "В справочнике 'Материалы стен' уже есть запись с таким наименованием. Наименование: {0}".FormatUsing(wallMaterialName));
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(Container, listWallMaterialToSave, 10000, true, true);
            LogImport.SetFileName(Name);
            LogImport.ImportKey = Key;
            LogManager.AddLog(LogImport);
            LogManager.FileNameWithoutExtention = Name;
            LogManager.Save();

            return new ImportResult();
        }
    }
}