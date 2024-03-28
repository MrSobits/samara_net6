namespace Bars.Gkh.Regions.Tyumen.Import.RealityObjectExaminationImport
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Organization;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;
    using Castle.Windsor;
    using Fasterflect;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.Dicts;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    using NHibernate.Type;

    public class RoImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public virtual IWindsorContainer Container { get; set; }

        public override string Key => RoImport.Id;

        public override string CodeImport => "RealityObjectExaminationImport";

        public override string Name => "Импорт жилых домов (Обследование)";

        public override string PossibleFileExtensions => "xls";

        public override string PermissionName => "Import.RealityObjectExaminationImport.View";

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IRepository<TypeProject> TypeProjectRepository { get; set; }

        public IRepository<RealityObjectStructuralElement> RealityObjectStructuralElementRepository { get; set; }
        public IRepository<RoofingMaterial> RoofingMaterialRepository { get; set; }
        public IRepository<WallMaterial> WallMaterialRepository { get; set; }

        public ILogImportManager LogManager { get; set; }

        #endregion Properties

        private ILogImport logImport;
        private Dictionary<string, TypeProject> typeProjectDict = new Dictionary<string, TypeProject>(); 
        private Dictionary<string,RealityObject> roDict = new Dictionary<string, RealityObject>();
        private List<RealityObjectStructuralElement> strElemList = new List<RealityObjectStructuralElement>();
        private Dictionary<string, Record> records = new Dictionary<string, Record>();
        private List<RealityObject> rObjectToUpdate = new List<RealityObject>();
        private Dictionary<string, RoofingMaterial> roofMaterialDict = new Dictionary<string, RoofingMaterial>();
        private Dictionary<string, WallMaterial> wallMaterialDict = new Dictionary<string, WallMaterial>();
        private List<RealityObjectStructuralElement> strElemToUpdate = new List<RealityObjectStructuralElement>();

        public override ImportResult Import(BaseParams baseParams)
        {
            string message;
            var file = baseParams.Files["FileImport"];

            Init(file.Data);

            if (records.Count < 1)
            {
                return new ImportResult(StatusImport.CompletedWithError, "Нет записей для импорта.");
            }

            InitLog(file.FileName);

            InitDictionaries();

            GetObjectsToUpdate();

            message = Update();

            this.LogManager.Add(file, this.logImport);
            this.LogManager.Save();

            message += this.LogManager.GetInfo();
            var status = logImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError;
            return new ImportResult(status, message, string.Empty, this.LogManager.LogFileId);
        }

        private void InitDictionaries()
        {
            List<string> keyAddress = new List<string>(records.Keys);

            typeProjectDict = TypeProjectRepository.GetAll()
                .Where(x => x.Name != null && x.Name != "")
                .ToList()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, y => y.First());

            roDict = RealityObjectRepository.GetAll()
                .Where(x => keyAddress.Contains(x.FiasAddress.AddressName
                                                             .ToLower()
                                                             .Substring(x.FiasAddress.AddressName.IndexOf(',') + 1)
                                                             .Replace("р-н. ", string.Empty)
                                                             .Replace(" район", string.Empty).Trim()))
                .ToList()
                .GroupBy(x => x.FiasAddress.AddressName
                                                             .ToLower()
                                                             .Substring(x.FiasAddress.AddressName.IndexOf(',') + 1)
                                                             .Replace("р-н. ", string.Empty)
                                                             .Replace(" район", string.Empty).Trim())
                .ToDictionary(x => x.Key, y => y.First());

            strElemList = RealityObjectStructuralElementRepository.GetAll()
                .Where(x => keyAddress.Contains(x.RealityObject.FiasAddress.AddressName
                                                             .ToLower()
                                                             .Substring(x.RealityObject.FiasAddress.AddressName.IndexOf(',') + 1)
                                                             .Replace("р-н. ", string.Empty)
                                                             .Replace(" район", string.Empty).Trim()) && x.State.Id == 49)
                .ToList();

            roofMaterialDict = RoofingMaterialRepository.GetAll()
                .Where(x => x.Name != null && x.Name != "")
                .ToList()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, y => y.First());

            wallMaterialDict = WallMaterialRepository.GetAll()
                .Where(x => x.Name != null && x.Name != "")
                .ToList()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, y => y.First());

        }

        public string ImportFormat(string address)
        {
            return address.ToLower().Substring(address.IndexOf(',')+1).Replace("р-н. ", string.Empty).Replace(" район", string.Empty).Trim();
        }

        public void InitLog(string fileName)
        {
            this.LogManager = this.Container.Resolve<ILogImportManager>();
            if (this.LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogManager.FileNameWithoutExtention = fileName;
            this.LogManager.UploadDate = DateTime.Now;

            this.logImport = this.Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (this.logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.logImport.SetFileName(fileName);
            this.logImport.ImportKey = this.Key;
        }

        private void Init(byte[] fileData)
        {
            using (var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                using (var memoryStreamFile = new MemoryStream(fileData))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    for (int i = 2; i<rows.Count;i++)
                    {
                        var record = new Record { isValidRecord = false, structElemDict = new Dictionary<int, StructElement>() };

                        record.Address = ImportFormat(rows[i][2].Value.Trim());
                        record.BuildYear = rows[i][3].Value.Trim();
                        record.PassportType = rows[i][4].Value.Trim();
                        record.LastRepairYear = rows[i][5].Value.Trim();
                        record.TechDate = rows[i][6].Value.Trim();
                        record.TotalBuildingVolume = rows[i][7].Value.Trim();
                        record.FloorHeight = rows[i][10].Value.Trim();
                        record.AreaMkd = rows[i][11].Value.Trim();
                        record.AreaMkdNotLiv = rows[i][12].Value.Trim();
                        record.FloorCount = rows[i][13].Value.Trim();
                        record.EntranceCount = rows[i][14].Value.Trim();
                        record.LiftCount = rows[i][15].Value.Trim();
                        record.RoofMaterial = rows[i][47].Value.Trim();
                        record.WallMaterial = rows[i][56].Value.Trim();
                        record.RoofType = rows[i][46].Value.Trim() == "плоская" ? "10" : rows[i][46].Value.Trim() == "скатная" ? "20" : "";

                        record.structElemDict[12] = new StructElement { Year = rows[i][16].Value.Trim(), Volume = rows[i][19].Value.Trim(), Wearout = rows[i][18].Value.Trim() };
                        record.structElemDict[7] = new StructElement { Year = rows[i][20].Value.Trim(), Volume = rows[i][23].Value.Trim(), Wearout = rows[i][22].Value.Trim() };
                        record.structElemDict[15] = new StructElement { Year = rows[i][25].Value.Trim(), Volume = rows[i][28].Value.Trim(), Wearout = rows[i][27].Value.Trim() };
                        record.structElemDict[9] = new StructElement { Year = rows[i][29].Value.Trim(), Volume = rows[i][32].Value.Trim(), Wearout = rows[i][31].Value.Trim() };
                        record.structElemDict[10] = new StructElement { Year = rows[i][34].Value.Trim(), Volume = rows[i][37].Value.Trim(), Wearout = rows[i][36].Value.Trim() };
                        record.structElemDict[1] = new StructElement { Year = rows[i][38].Value.Trim(), Volume = rows[i][41].Value.Trim(), Wearout = rows[i][40].Value.Trim() };
                        record.structElemDict[1] = new StructElement { Year = rows[i][42].Value.Trim(), Volume = rows[i][45].Value.Trim(), Wearout = rows[i][44].Value.Trim() };
                        record.structElemDict[16] = new StructElement { Year = rows[i][48].Value.Trim(), Volume = rows[i][51].Value.Trim(), Wearout = rows[i][50].Value.Trim() };
                        record.structElemDict[2] = new StructElement { Year = rows[i][52].Value.Trim(), Volume = rows[i][55].Value.Trim(), Wearout = rows[i][54].Value.Trim() };
                        record.structElemDict[5] = new StructElement { Year = rows[i][57].Value.Trim(), Volume = rows[i][60].Value.Trim(), Wearout = rows[i][59].Value.Trim() };

                        records[record.Address] = record;
                    }
                }
            }
        }

        private void GetObjectsToUpdate()
        {
            foreach(var record in records)
            {
                var rObject = roDict.FirstOrDefault(x => x.Key == record.Key).Value;

                if (rObject == null)
                {
                    this.logImport.Info(record.Key+"; Дом с таким адресом не найден", "");
                    logImport.CountWarning++;
                    continue;
                }

                if (int.TryParse(record.Value.BuildYear, out int bYear))
                {
                    rObject.BuildYear = bYear;
                }

                if (!string.IsNullOrEmpty(record.Value.PassportType))
                {
                    rObject.TypeProject = GetPassportType(record.Value.PassportType);
                }

                if (DateTime.TryParseExact(record.Value.LastRepairYear, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out var dateValue))
                {
                    rObject.DateLastOverhaul = dateValue;
                }

                if (DateTime.TryParse(record.Value.TechDate, out var techDate))
                {
                    rObject.DateTechInspection = techDate;
                }

                if (decimal.TryParse(record.Value.TotalBuildingVolume.Replace(
                        CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                        CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out decimal tbv))
                {
                    rObject.TotalBuildingVolume = tbv;
                }

                if (decimal.TryParse(record.Value.FloorHeight.Replace(
                        CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                        CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out decimal flHeight))
                {
                    rObject.FloorHeight = flHeight;
                }

                if (decimal.TryParse(record.Value.AreaMkd.Replace(
                        CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                        CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out decimal areaMkd))
                {
                    rObject.AreaMkd = areaMkd;
                }

                if (decimal.TryParse(record.Value.AreaMkdNotLiv.Replace(
                        CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                        CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out decimal areaNotLiv))
                {
                    rObject.AreaNotLivingPremises = areaNotLiv;
                }

                if (int.TryParse(record.Value.FloorCount, out int flCount))
                {
                    rObject.MaximumFloors = flCount;
                    rObject.Floors = flCount;
                }

                if (int.TryParse(record.Value.EntranceCount, out int enCount))
                {
                    rObject.NumberEntrances = enCount;
                }

                if (int.TryParse(record.Value.LiftCount, out int liftCount))
                {
                    rObject.NumberLifts = liftCount;
                }

                if (!string.IsNullOrEmpty(record.Value.WallMaterial))
                {
                    rObject.WallMaterial = GetWallMaterial(record.Value.WallMaterial);
                }

                if (!string.IsNullOrEmpty(record.Value.RoofMaterial))
                {
                    rObject.RoofingMaterial = GetRoofMaterial(record.Value.RoofMaterial);
                }

                if (Enum.TryParse(record.Value.RoofType, out TypeRoof typeRoof))
                {
                    rObject.TypeRoof = typeRoof;
                }

                foreach (var elem in record.Value.structElemDict)
                {
                    var elements = strElemList.Where(x => x.StructuralElement.Group.CommonEstateObject.Id == elem.Key && x.RealityObject.Id == rObject.Id);

                    foreach (var roel in elements)
                    {
                        if (roel == null)
                        {
                            continue;
                        }

                        var index = 0;

                        if (int.TryParse(elem.Value.Year, out int year))
                        {
                            roel.LastOverhaulYear = year;
                            index++;
                        }

                        if (decimal.TryParse(elem.Value.Wearout.Replace(
                            CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                            CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out decimal wearout))
                        {
                            roel.Wearout = Math.Round(wearout,5);
                            index++;
                        }

                        if (decimal.TryParse(elem.Value.Volume.Replace(
                            CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                            CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out decimal volume))
                        {
                            roel.Volume = Math.Round(volume, 5);
                            index++;
                        }

                        if (index > 0)
                        {
                            strElemToUpdate.Add(roel);
                        }
                    }
                }
                rObjectToUpdate.Add(rObject);
            }
        }

        private string Update()
        {
            var message = string.Empty;

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var ro in rObjectToUpdate)
                    {
                        RealityObjectRepository.Update(ro);
                        this.logImport.Info(ro.FiasAddress.AddressName+ "; Дом добавлен", "");
                        logImport.CountChangedRows++;
                    }

                    foreach (var el in strElemToUpdate)
                    {
                        RealityObjectStructuralElementRepository.Update(el);
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        this.logImport.IsImported = false;
                        this.Container.Resolve<ILogger>().LogError(e, "Импорт");
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                        this.logImport.Error(this.Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");

                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
            return message;
        }
        private TypeProject GetPassportType(string excelType)
        {
            if (!typeProjectDict.ContainsKey(excelType))
            {
                var newType = new TypeProject { Name = excelType }; 
                TypeProjectRepository.Save(newType);
                typeProjectDict.Add(excelType, newType);
                return newType;
            }

            return typeProjectDict[excelType];
        }

        private WallMaterial GetWallMaterial(string excelMaterial)
        {
            if (!wallMaterialDict.ContainsKey(excelMaterial))
            {
                var newMaterial = new WallMaterial { Name = excelMaterial };
                WallMaterialRepository.Save(newMaterial);
                wallMaterialDict.Add(excelMaterial, newMaterial);
                return newMaterial;
            }

            return wallMaterialDict[excelMaterial];
        }

        private RoofingMaterial GetRoofMaterial(string excelMaterial)
        {
            if (!roofMaterialDict.ContainsKey(excelMaterial))
            {
                var newMaterial = new RoofingMaterial { Name = excelMaterial };
                RoofingMaterialRepository.Save(newMaterial);
                roofMaterialDict.Add(excelMaterial, newMaterial);
                return newMaterial;
            }

            return roofMaterialDict[excelMaterial];
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] { this.PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
                return false;
            }

            return true;
        }
    }
}