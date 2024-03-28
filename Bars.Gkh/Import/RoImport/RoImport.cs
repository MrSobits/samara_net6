namespace Bars.Gkh.Import.RoImport
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.Gkh.Entities.Dicts;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;

    using Castle.Windsor;
    using Impl;

    using Microsoft.Extensions.Logging;

    using NHibernate.Type;

    public class RoImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private class Log
        {
            public string Text;

            public bool Success;
        }

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "RoImport"; }
        }

        public override string Name
        {
            get { return "Импорт жилых домов"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.RoImport.View"; }
        }

        private Dictionary<int, Log> logDict = new Dictionary<int, Log>();

        private Dictionary<string, Municipality> _municipalitiesDict;

        private Dictionary<string, List<FiasAddressRecord>> _fiasAdressesByKladrCodeDict;

        private Dictionary<string, string> _aoGuidByKladrCodeMap;

        private Dictionary<string, WallMaterial> _wallMaterialDict;

        private Dictionary<string, IDinamicAddress> _dynamicAddressDict = new Dictionary<string, IDinamicAddress>();

        private List<Record> _realtyObjectsToCreate = new List<Record>();

        public ISessionProvider SessionProvider { get; set; }

        public IFiasRepository fiasRepository { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        private List<RealityObject>  realityObjectToCreate = new List<RealityObject>();

        private List<RealityObjectLand> realityObjectLandToCreate = new List<RealityObjectLand>();

        private List<FiasAddress> fiasAddressToCreate = new List<FiasAddress>(); 

        public IRepository<FiasAddress> FiasAddressRepository { get; set; }

        private static string importHeader = "Импорт Жилых домов из Реформы ЖКХ";

        private readonly Dictionary<string, int> _headersDict = new Dictionary<string, int>();

        private IStructElementImport IStructElementImport;

        private ITechPassportDataImport ITechPassportDataImport;

        private State defaultRealtyObjectState;

        private HashSet<string> techPassportDataFields = new HashSet<string>
        {
            "П18", "П100", "П101", "П105", "П106", "П108", "П11", "П111", "П112", "П113", 
            "П114", "П19", "П20", "П21", "П22", "П23", "П25", "П65", "П66", "П73", "П76", 
            "П77", "П8", "П80", "П81", "П83", "П84", "П87", "П90", "П91", "П94", "П97", 
            "П98"
        };

        public RoImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
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

        private State GetDefaultStateForRealtyObject()
        {
            var realityObject = new RealityObject();

            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(realityObject);

            return realityObject.State;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            defaultRealtyObjectState = GetDefaultStateForRealtyObject();

            if (defaultRealtyObjectState == null)
            {
                return new ImportResult(StatusImport.CompletedWithError, "Не создан начальный статус для сущности 'Жилой дом'!");
            }

            IStructElementImport = Container.ResolveAll<IStructElementImport>().FirstOrDefault();

            ITechPassportDataImport = Container.ResolveAll<ITechPassportDataImport>().FirstOrDefault();

            var file = baseParams.Files["FileImport"];

            InitLog(file.FileName);

            InitDictionaries();

            this.ProcessData(file.Data);

            this.PrepareToSaveData();

            var message = this.SaveData();
        
            this.WriteLogs();

            this.LogImportManager.Add(file, LogImport);
            this.LogImportManager.Save();

            message += this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : (this.LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }
        
        private void InitDictionaries()
        {
            _wallMaterialDict = Container.Resolve<IDomainService<WallMaterial>>().GetAll()
                .Where(x => x.Name != null)
                .AsEnumerable()
                .GroupBy(x => x.Name.ToUpper())
                .ToDictionary(x => x.Key, x => x.First());

            // Словарь всех существующих адресов по коду КЛАДР
            this._fiasAdressesByKladrCodeDict = this.Container.Resolve<IDomainService<Fias>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<FiasAddress>>().GetAll(),
                    x => x.AOGuid,
                    y => y.StreetGuidId,
                    (a, b) => new { fias = a, fiasAddress = b }
                )
                .Where(x => x.fias.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.fias.KladrCode != null)
                .Select(x => new FiasAddressRecord
                {
                    KladrCode = x.fias.KladrCode,
                    AoGuid = x.fias.AOGuid,
                    Id = x.fiasAddress.Id,
                    House = x.fiasAddress.House,
                    Housing = x.fiasAddress.Housing,
                    Building = x.fiasAddress.Building
                })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.KladrCode))
                .GroupBy(x => x.KladrCode)
                .ToDictionary(x => x.Key, x => x.ToList());

            // Словарь aoGiod по коду КЛАДР
            this._aoGuidByKladrCodeMap = this.Container.Resolve<IDomainService<Fias>>().GetAll()
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.KladrCode != null)
                .Select(x => new { x.KladrCode, x.AOGuid })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.KladrCode))
                .GroupBy(x => x.KladrCode)
                .ToDictionary(x => x.Key, x => x.First().AOGuid);

            this._municipalitiesDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Where(x => x.FiasId != null)
                .AsEnumerable()
                .GroupBy(x => x.FiasId)
                .ToDictionary(x => x.Key, x => x.First());
        }

        private void ProcessData(byte[] data)
        {
            var realtyObjects = this.RealityObjectRepository.GetAll()
                .Select(x => new { x.Id, FiasAddressId = x.FiasAddress.Id })
                .AsEnumerable()
                .GroupBy(x => x.FiasAddressId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

            using (var fileMemoryStream = new MemoryStream(data))
            {
                using (var reader = new StreamReader(fileMemoryStream, Encoding.GetEncoding(1251)))
                {
                    var headers = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();

                    var headerLength = headers.Length;

                    if (headerLength <= 1)
                    {
                        throw new Exception("Неверный файл загрузки");
                    }

                    for (var index = 0; index < headerLength; ++index)
                    {
                        _headersDict[headers[index]] = index;
                    }

                    string line;

                    var i = 1;

                    while ((line = reader.ReadLine()) != null)
                    {
                        var record = ProcessLine(line, ++i);
                        if (!record.isValidRecord)
                        {
                            continue;
                        }

                        var codeKladr = record.CodeKladrStreet;

                        if (this._fiasAdressesByKladrCodeDict.ContainsKey(codeKladr))
                        {
                            var fiasAdresses = this._fiasAdressesByKladrCodeDict[codeKladr];
                            var result = fiasAdresses.Where(x => x.House == record.House).ToList();

                            if (string.IsNullOrEmpty(record.Housing))
                            {
                                result = result.Where(x => string.IsNullOrEmpty(x.Housing)).ToList();
                            }
                            else
                            {
                                result = result.Where(x => x.Housing == record.Housing).ToList();
                            }

                            if (string.IsNullOrEmpty(record.Building))
                            {
                                result = result.Where(x => string.IsNullOrEmpty(x.Building)).ToList();
                            }
                            else
                            {
                                result = result.Where(x => x.Building == record.Building).ToList();
                            }

                            if (result.Count == 0)
                            {
                                var aoGuid = fiasAdresses.First().AoGuid;

                                record.FiasAddress = CreateAddressByStreetAoGuid(aoGuid, record.House, record.Housing, record.Building);

                                _realtyObjectsToCreate.Add(record);
                            }
                            else
                            {
                                var realtyObjectIdsByAddress = result.Where(x => realtyObjects.ContainsKey(x.Id)).SelectMany(x => realtyObjects[x.Id]).ToList();

                                if (realtyObjectIdsByAddress.Count == 0)
                                {
                                    // Тут создаем новый ФИАС адрес, т.к. найденный адрес - не адрес дома

                                    var aoGuid = fiasAdresses.First().AoGuid;

                                    record.FiasAddress = CreateAddressByStreetAoGuid(aoGuid, record.House, record.Housing, record.Building);

                                    _realtyObjectsToCreate.Add(record);
                                }
                                else
                                {
                                    // Дом уже в системе
                                    this.AddLog(record.RowNumber, "Данный дом уже есть в системе", false);
                                }
                            }
                        }
                        else
                        {
                            if (this._aoGuidByKladrCodeMap.ContainsKey(codeKladr))
                            {
                                var aoGuid = this._aoGuidByKladrCodeMap[codeKladr];

                                record.FiasAddress = CreateAddressByStreetAoGuid(aoGuid, record.House, record.Housing, record.Building);

                                _realtyObjectsToCreate.Add(record);
                            }
                            else
                            {
                                this.AddLog(record.RowNumber, string.Format("В системе не найдена улица, соответствующая коду КЛАДР {0}", record.CodeKladrStreet), false);
                            }
                        }
                    }
                }
            }
        }

        private Record ProcessLine(string line, int rowNumber)
        {
            var record = new Record { isValidRecord = false, RowNumber = rowNumber};
            var data = line.ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
            if (data.Length <= 1)
            {
                return record;
            }

            record.CodeKladrStreet = GetValue(data, "П2_9");
            
            if (string.IsNullOrEmpty(record.CodeKladrStreet) || record.CodeKladrStreet.Length != 17)
            {
                this.AddLog(record.RowNumber, string.Format("Неверный код КЛАДР улицы: \"{0}\"", record.CodeKladrStreet), false);
                return record;
            }

            record.House = GetValue(data, "П2_12");

            if (string.IsNullOrWhiteSpace(record.House))
            {
                this.AddLog(record.RowNumber, "Не задан номер дома", false);
                return record;
            }

            record.Housing = GetValue(data, "П2_13");
            record.Building = GetValue(data, "П2_14");
            record.BuildYear = GetValue(data, "П27");
            record.AreaMkd = GetValue(data, "П18");
            record.CadastreNumber = GetValue(data, "П9");
            record.TypeHouse = GetValue(data, "П14");
            record.NumberApartments = GetValue(data, "П15");
            record.NumberLiving = GetValue(data, "П16");
            record.AreaLiving = GetValue(data, "П19");
            record.AreaOwned = GetValue(data, "П20");
            record.AreaMunicipalOwned = GetValue(data, "П21");
            record.AreaGovernmentOwned = GetValue(data, "П22");
            record.AreaNotLivingFunctional = GetValue(data, "П23");
            record.MaximumFloors = GetValue(data, "П24");
            record.NumberEntrances = GetValue(data, "П25");
            record.PhysicalWear = GetValue(data, "П29");
            record.ConditionHouse = GetValue(data, "П28");
            record.HeatingSystem = GetValue(data, "П69");
            record.WallMaterial = GetValue(data, "П37");
            
            record.TechPassportData = GetTechPassportData(data);
            record.StructuralElements = GetStructuralElementRecordList(data, record.BuildYear);

            record.isValidRecord = true;

            return record;
        }

        private Dictionary<string, string> GetTechPassportData(string[] data)
        {
            var techPassDataDict = techPassportDataFields
                .Select(x => new { key = x, value = this.GetValue(data, x) })
                .Where(x => !string.IsNullOrWhiteSpace(x.value))
                .ToDictionary(x => x.key, x => x.value);

            return techPassDataDict;
        }

        private List<StructuralElementRecord> GetStructuralElementRecordList(string[] data, string buildYear)
        {
            var listSe = new List<StructuralElementRecord>();

            #region facade

            Action<string, string> addStructuralElement = (field, code) =>
                {
                    var value = this.GetValue(data, field);
                    if (this.GetValidValue(value) == null)
                    {
                        return;
                    }

                    var val = new StructuralElementRecord { Code = code, Volume = value };

                    listSe.Add(val);
                };

            addStructuralElement("П39", "52");
            addStructuralElement("П40", "122");
            addStructuralElement("П41", "126");
            addStructuralElement("П43", "130");
            addStructuralElement("П44", "128");
            addStructuralElement("П48", "49");
            addStructuralElement("П49", "110");
            addStructuralElement("П50", "111");
            addStructuralElement("П53", "119");
            addStructuralElement("П54", "120");

            #endregion facade

            #region overlap

            var overlap = new StructuralElementRecord
            {
                Code = "-1",
                Wearout = GetValue(data, "П32")
            };

            switch (GetValue(data, "П36").ToUpper())
            {
                case "ЖЕЛЕЗОБЕТОННЫЕ":
                    overlap.Code = "56";
                    break;

                case "ДЕРЕВЯННЫЕ":
                    overlap.Code = "201";
                    break;

                case "СМЕШАННЫЕ":
                    overlap.Code = "57";
                    break;
            }


            if (overlap.Code != "-1")
            {
                listSe.Add(overlap);
            }

            #endregion overlap

            #region basement

            if (GetValue(data, "П62").ToUpper() == "ЭКСПЛУАТИРУЕМЫЙ")
            {
                var val = new StructuralElementRecord
                {
                    Code = "257",
                    Volume = GetValue(data, "П63"),
                    LastOverhaulYear = GetValue(data, "П64")
                };

                listSe.Add(val);
            }

            #endregion basement

            #region heatsystem

            var lastOverhaulYear = GetValidValue(GetValue(data, "П72")) ?? buildYear;
            
            if (GetValidValue(GetValue(data, "П70")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "5",
                    Volume = GetValue(data, "П70"),
                    LastOverhaulYear = lastOverhaulYear
                };

                listSe.Add(val);
            }

            if (GetValidValue(GetValue(data, "П71")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "202",
                    Volume = GetValue(data, "П71"),
                    LastOverhaulYear = lastOverhaulYear
                }; 

                listSe.Add(val);
            }

            if (GetValidValue(GetValue(data, "П74")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "213",
                    Volume = GetValue(data, "П74"),
                    LastOverhaulYear = lastOverhaulYear
                };

                listSe.Add(val);
            }


            if (GetValidValue(GetValue(data, "П75")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "214",
                    Volume = GetValue(data, "П75"),
                    LastOverhaulYear = lastOverhaulYear
                };

                listSe.Add(val);
            }

            #endregion heatsystem

            #region walls

            var walls = new StructuralElementRecord
            {
                Code = "-1",
                Wearout = GetValue(data, "П31"),
                LastOverhaulYear = GetValue(data, "П55")
            };

            switch (GetValue(data, "П37").ToUpper())
            {
                case "ДЕРЕВЯННЫЕ":
                    walls.Code = "191"; 
                    break;

                case "КАМЕННЫЕ, КИРПИЧНЫЕ":
                    walls.Code = "53";
                    break;

                case "ПАНЕЛЬНЫЕ":
                    walls.Code = "54";
                    break;

                case "СМЕШАННЫЕ":
                    walls.Code = "192";
                    break;
            }


            if (walls.Code != "-1")
            {
                listSe.Add(walls);
            }

            #endregion walls

            #region  roof

            lastOverhaulYear = GetValidValue(GetValue(data, "П61")) ?? buildYear;

            if (GetValidValue(GetValue(data, "П57")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "107",
                    Volume = GetValue(data, "П57"),
                    LastOverhaulYear = lastOverhaulYear
                };

                listSe.Add(val);
            }

            if (GetValidValue(GetValue(data, "П58")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "45",
                    Volume = GetValue(data, "П58"),
                    LastOverhaulYear = lastOverhaulYear
                };

                listSe.Add(val);
            }

            if (GetValidValue(GetValue(data, "П59")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "109",
                    Volume = GetValue(data, "П59"),
                    LastOverhaulYear = lastOverhaulYear
                };

                listSe.Add(val);
            }


            #endregion roof
            
            #region coldwater

            if (GetValidValue(GetValue(data, "П85")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "7",
                    Volume = GetValue(data, "П85"),
                    LastOverhaulYear = GetValidValue(GetValue(data, "П86")) ?? buildYear
                };

                listSe.Add(val);
            }

            if (GetValidValue(GetValue(data, "П88")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "9",
                    Volume = GetValue(data, "П88"),
                    LastOverhaulYear = GetValidValue(GetValue(data, "П86")) ?? buildYear
                };

                listSe.Add(val);
            }

            #endregion coldwater
            
            #region hotwater

            if (GetValidValue(GetValue(data, "П78")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "15",
                    Volume = GetValue(data, "П78"),
                    LastOverhaulYear = GetValidValue(GetValue(data, "П79")) ?? buildYear
                };

                listSe.Add(val);
            }

            if (GetValidValue(GetValue(data, "П82")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "224",
                    Volume = GetValue(data, "П82"),
                    LastOverhaulYear = GetValidValue(GetValue(data, "П79")) ?? buildYear
                };

                listSe.Add(val);
            }

            #endregion hotwater

            #region waterDrainage

            if (GetValidValue(GetValue(data, "П92")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "21",
                    Volume = GetValue(data, "П92"),
                    LastOverhaulYear = GetValidValue(GetValue(data, "П93")) ?? buildYear
                };

                listSe.Add(val);
            }

            #endregion waterDrainage

            #region electro

            if (GetValidValue(GetValue(data, "П95")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "17",
                    Volume = GetValue(data, "П95"),
                    LastOverhaulYear = GetValidValue(GetValue(data, "П96")) ?? buildYear
                };

                listSe.Add(val);
            }

            #endregion electro

            #region gas

            if (GetValidValue(GetValue(data, "П102")) != null)
            {
                var val = new StructuralElementRecord
                {
                    Code = "13",
                    Volume = GetValue(data, "П102"),
                    LastOverhaulYear = GetValidValue(GetValue(data, "П104")) ?? buildYear
                };

                listSe.Add(val);
            }

            #endregion gas

            return listSe;
        }

        private string GetValidValue(string s1)
        {
            if (!string.IsNullOrWhiteSpace(s1) && s1 != "0" && s1 != "0.00")
            {
                return s1;
            }

            return null;
        }

        /// <summary>
        /// Метод получения из массива строк data значения, соответвующего полю field
        /// </summary>
        private string GetValue(string[] data, string field)
        {
            var result = string.Empty;

            if (_headersDict.ContainsKey(field))
            {
                var index = _headersDict[field];
                if (data.Length > index)
                {
                    result = data[index];
                }
            }

            return result.Trim();
        }


        /// <summary>
        /// Создание FiasAddress на основе aoGuid улицы, номера и корпуса дома
        /// Важно! Корректно работает только для уровня улиц
        /// </summary>
        private FiasAddress CreateAddressByStreetAoGuid(string aoGuid, string house, string housing, string building)
        {
            IDinamicAddress dynamicAddress;
            if (_dynamicAddressDict.ContainsKey(aoGuid))
            {
                dynamicAddress = _dynamicAddressDict[aoGuid];
            }
            else
            {
                dynamicAddress = fiasRepository.GetDinamicAddress(aoGuid);
                _dynamicAddressDict[aoGuid] = dynamicAddress;
            }

            var addressName = new StringBuilder(dynamicAddress.AddressName);

            if (!string.IsNullOrEmpty(house))
            {
                addressName.Append(", д. ");
                addressName.Append(house);

                if (!string.IsNullOrEmpty(housing))
                {
                    addressName.Append(", корп. ");
                    addressName.Append(housing);

                    if (!string.IsNullOrEmpty(building))
                    {
                        addressName.Append(", секц. ");
                        addressName.Append(building);
                    }
                }
            }

            var fiasAddress = new FiasAddress
            {
                AddressGuid = dynamicAddress.AddressGuid,
                AddressName = addressName.ToString(),
                PostCode = dynamicAddress.PostCode,
                StreetGuidId = dynamicAddress.GuidId,
                StreetName = dynamicAddress.Name,
                StreetCode = dynamicAddress.Code,
                House = house,
                Housing = housing,
                Building = building,
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,

                // Поля ниже коррекны только если входной параметр aoGuid улицы
                PlaceAddressName = dynamicAddress.AddressName.Replace(dynamicAddress.Name, string.Empty).Trim(' ').Trim(','),
                PlaceGuidId = dynamicAddress.ParentGuidId,
                PlaceName = dynamicAddress.ParentName
            };

            return fiasAddress;
        }

        private Municipality GetMunicipality(FiasAddress address)
        {
            if (address == null || string.IsNullOrEmpty(address.AddressGuid))
            {
                return null;
            }

            var guidMass = address.AddressGuid.Split('#');

            Municipality result = null;

            foreach (var s in guidMass)
            {
                var t = s.Split('_');

                Guid g;

                Guid.TryParse(t[1], out g);

                if (g != Guid.Empty)
                {
                    var guid = g.ToString();
                    if (_municipalitiesDict.ContainsKey(guid))
                    {
                        result = _municipalitiesDict[guid];
                    }
                }
            }

            return result;
        }

        private string GetAddressForMunicipality(Municipality mo, FiasAddress address)
        {
            if (address == null || mo == null)
            {
                return string.Empty;
            }

            var result = address.AddressName ?? string.Empty;

            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(mo.FiasId))
            {
                return string.Empty;
            }

            IDinamicAddress dinamicAddress;

            if (_dynamicAddressDict.ContainsKey(mo.FiasId))
            {
                dinamicAddress = _dynamicAddressDict[mo.FiasId];
            }
            else
            {
                dinamicAddress = fiasRepository.GetDinamicAddress(mo.FiasId);
                _dynamicAddressDict[mo.FiasId] = dinamicAddress;
            }

            if (dinamicAddress == null)
            {
                return string.Empty;
            }

            if (result.StartsWith(dinamicAddress.AddressName))
            {
                result = result.Replace(dinamicAddress.AddressName, string.Empty).Trim();
            }

            if (result.StartsWith(","))
            {
                result = result.Substring(1).Trim();
            }

            return result;
        }

        private void Apply(RealityObject realityObject, Record record)
        {
            if (!string.IsNullOrEmpty(record.AreaMkd))
            {
                decimal value;
                if (decimalTryParse(record.AreaMkd, out value))
                {
                    realityObject.AreaMkd = value;
                }
            }

            if (!string.IsNullOrEmpty(record.AreaLiving))
            {
                decimal value;
                if (decimalTryParse(record.AreaLiving, out value))
                {
                    realityObject.AreaLiving = value;
                }
            }

            if (realityObject.HasPrivatizedFlats == null)
            {
                realityObject.HasPrivatizedFlats = false;
            }

            realityObject.CadastreNumber = record.CadastreNumber;


            if (realityObject.BuildYear == null && !string.IsNullOrEmpty(record.BuildYear))
            {
                int value;
                if (int.TryParse(record.BuildYear, out value))
                {
                    realityObject.BuildYear = value;
                }
            }

            if (!string.IsNullOrEmpty(record.TypeHouse))
            {
                switch (record.TypeHouse.ToUpper())
                {
                    case "ЖИЛОЙ ДОМ БЛОКИРОВАННОЙ ЗАСТРОЙКИ": realityObject.TypeHouse = TypeHouse.BlockedBuilding;
                        break;

                    case "ОБЪЕКТ ИНДИВИДУАЛЬНОГО ЖИЛИЩНОГО СТРОИТЕЛЬСТВА": realityObject.TypeHouse = TypeHouse.Individual;
                        break;

                    case "ОБЩЕЖИТИЕ": realityObject.TypeHouse = TypeHouse.SocialBehavior; 
                        break;

                    case "МНОГОКВАРТИРНЫЙ ДОМ":
                    case "НЕТ ДАННЫХ":
                    case "ПУСТО":
                        realityObject.TypeHouse = TypeHouse.ManyApartments;
                        break;

                }
            }
            else
            {
                realityObject.TypeHouse = TypeHouse.ManyApartments;
            }


            if (!string.IsNullOrEmpty(record.NumberApartments))
            {
                int value;
                if (int.TryParse(record.NumberApartments, out value))
                {
                    realityObject.NumberApartments = value;
                }
            }

            switch (record.ConditionHouse.ToLower())
            {
                case "аварийный":
                    realityObject.ConditionHouse = ConditionHouse.Emergency;
                    break;
                case "ветхий":
                    realityObject.ConditionHouse = ConditionHouse.Dilapidated;
                    break;
                case "исправный":
                case "нет данных":
                case "":
                case null:
                    realityObject.ConditionHouse = ConditionHouse.Serviceable;
                    break;
            }

            if (record.HeatingSystem.ToUpper() == "ЦЕНТРАЛЬНОЕ")
            {
                realityObject.HeatingSystem = HeatingSystem.Centralized;
            }

            if (!string.IsNullOrEmpty(record.NumberLiving))
            {
                int value;
                if (int.TryParse(record.NumberLiving, out value))
                {
                    realityObject.NumberLiving = value;
                }
            }


            if (!string.IsNullOrEmpty(record.AreaOwned))
            {
                decimal value;
                if (decimalTryParse(record.AreaOwned, out value))
                {
                    realityObject.AreaOwned = value;
                }
            }

            if (!string.IsNullOrEmpty(record.AreaMunicipalOwned))
            {
                decimal value;
                if (decimalTryParse(record.AreaMunicipalOwned, out value))
                {
                    realityObject.AreaMunicipalOwned = value;
                }
            }

            if (!string.IsNullOrEmpty(record.AreaGovernmentOwned))
            {
                decimal value;
                if (decimalTryParse(record.AreaGovernmentOwned, out value))
                {
                    realityObject.AreaGovernmentOwned = value;
                }
            }

            if (!string.IsNullOrEmpty(record.AreaNotLivingFunctional))
            {
                decimal value;
                if (decimalTryParse(record.AreaNotLivingFunctional, out value))
                {
                    realityObject.AreaNotLivingFunctional = value;
                }
            }

            if (!string.IsNullOrEmpty(record.MaximumFloors))
            {
                int value;
                if (int.TryParse(record.MaximumFloors, out value))
                {
                    realityObject.MaximumFloors = value;
                    realityObject.Floors = value;
                }
            }

            if (!string.IsNullOrEmpty(record.NumberEntrances))
            {
                int value;
                if (int.TryParse(record.NumberEntrances, out value))
                {
                    realityObject.NumberEntrances = value;
                }
            }

            if (!string.IsNullOrEmpty(record.PhysicalWear))
            {
                decimal value;
                if (decimalTryParse(record.PhysicalWear, out value))
                {
                    realityObject.PhysicalWear = value;
                }
            }

            if (!string.IsNullOrWhiteSpace(record.WallMaterial))
            {
                var wallMaterial = "-1";

                switch (record.WallMaterial.ToUpper())
                {
                    case "КАМЕННЫЕ, КИРПИЧНЫЕ":
                        wallMaterial = "КИРПИЧНЫЕ";
                        break;

                    case "КРУПНО-БЛОЧНЫЕ":
                    case "ПРОЧИЕ":
                    case "НЕТ ДАННЫХ":
                        break;

                    default:
                        wallMaterial = record.WallMaterial.ToUpper();
                        break;
                }

                if (wallMaterial != "-1" && _wallMaterialDict.ContainsKey(wallMaterial))
                {
                    realityObject.WallMaterial = _wallMaterialDict[wallMaterial];
                }
            }
        }

        private bool decimalTryParse(string str, out decimal value)
        {
            var result =  Decimal.TryParse(
                str.Replace(
                    CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator,
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator),
                out value);
            return result;
        }

        private string SaveData()
        {
            var message = string.Empty;

            SessionProvider.CloseCurrentSession();

            using (var session = SessionProvider.OpenStatelessSession())
            {
                //session.SetBatchSize(1000);

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        this.fiasAddressToCreate.ForEach(x => session.Insert(x));
                        this.realityObjectToCreate.ForEach(x => session.Insert(x));
                        this.realityObjectLandToCreate.ForEach(x => session.Insert(x));

                        if (IStructElementImport != null)
                        {
                            IStructElementImport.SaveData(session);
                        }

                        if (ITechPassportDataImport != null)
                        {
                            ITechPassportDataImport.SaveData(session);
                        }

                        transaction.Commit();
                    }
                    catch (Exception exc)
                    {
                        LogImport.CountAddedRows = 0;

                        try
                        {
                            LogImport.IsImported = false;
                            Container.Resolve<ILogger>().LogError(exc, "Импорт");
                            message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                            LogImport.Error(importHeader, "Произошла неизвестная ошибка. Обратитесь к администратору.");

                            transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, exc);
                        }
                    }
                }
            }

            return message;
        }

        private void PrepareToSaveData()
        {
            // Если в файле есть дублирующиеся дома, загружать последний.
            _realtyObjectsToCreate = _realtyObjectsToCreate
                .GroupBy(x => new { x.CodeKladrStreet, x.House, x.Housing, x.Building })
                .Select(x => x.Select(y => y).Last())
                .ToList();

            foreach (var realtyObjectToCreate in _realtyObjectsToCreate.Where(x => x != null))
            {
                var msg = string.Empty;

                var objectToCreate = realtyObjectToCreate;

                if (objectToCreate.FiasAddressId == null && objectToCreate.FiasAddress == null)
                {
                    this.AddLog(
                        objectToCreate.RowNumber,
                        string.Format(
                            "Объект не создан. В ФИАС не удалось найти адреc, соответствующий KLADR_KOD_STREET: {0}, NUM: {1}, KORPUS: {2}",
                            objectToCreate.CodeKladrStreet,
                            objectToCreate.House,
                            objectToCreate.Housing),
                       false);

                    continue;
                }

                FiasAddress fiasAddress;
                if (objectToCreate.FiasAddressId.HasValue)
                {
                    fiasAddress = FiasAddressRepository.Load(objectToCreate.FiasAddressId.Value);
                }
                else
                {
                    if (this.ValidateEntity(objectToCreate.FiasAddress, ref msg))
                    {
                        fiasAddressToCreate.Add(objectToCreate.FiasAddress);
                    }

                    fiasAddress = objectToCreate.FiasAddress;
                }

                var municipality = GetMunicipality(fiasAddress);

                if (municipality == null)
                {
                    this.AddLog(objectToCreate.RowNumber, "Объект не создан. Не удалось определить муниципальное образование", false);

                    continue;
                }

                var realityObj = new RealityObject
                {
                    FiasAddress = fiasAddress,
                    State = defaultRealtyObjectState,
                    Municipality = municipality,
                    Address = GetAddressForMunicipality(municipality, fiasAddress),
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                Apply(realityObj, objectToCreate);

                this.ValidateEntity(municipality, ref msg);
                this.ValidateEntity(realityObj, ref msg);

                if (!msg.IsEmpty())
                {
                    this.AddLog(objectToCreate.RowNumber, string.Format("Дополнительная информация: {0}", msg), false);
                    continue;
                }

                realityObjectToCreate.Add(realityObj);
                SaveRealityObjectLand(realityObj, objectToCreate);
                SaveRealityObjectStructElements(realityObj, objectToCreate);
                SaveRealityObjectTechPassport(realityObj, objectToCreate);
                this.AddLog(objectToCreate.RowNumber, string.Format("Адрес: {0}. Дом создан",  realityObj.Address), true);
                LogImport.CountAddedRows++;
            }
        }

        /// <summary>
        /// Валидация сущности согласно маппингу
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="entity">Объект</param>
        /// <param name="message">Сообщение</param>
        private bool ValidateEntity<T>(T entity, ref string message)
        {
            var classMeta = this.SessionProvider.GetCurrentSession()
                .SessionFactory.GetClassMetadata(typeof(T));

            var i = 0;
            foreach (var propName in classMeta.PropertyNames)
            {
                var propType = classMeta.GetPropertyType(propName);
                var st = propType as StringType;

                var nullable = classMeta.PropertyNullability[i];

                var val = typeof(T).GetProperty(propName)
                    .GetValue(entity, new object[0]);

                if (!nullable)
                {
                    if (val == null)
                    {
                        message += string.Format("Свойство \"{0}\" не может принимать пустые значения; ", propName);
                        return false;
                    }
                }

                if (st != null)
                {
                    if (st.SqlType.LengthDefined)
                    {
                        if (val != null && val.ToStr().Length > st.SqlType.Length)
                        {
                            message += string.Format("Значение свойства \"{0}\" не может быть длиннее {1}; ",
                                propName,
                                st.SqlType.Length);
                            return false;
                        }
                    }
                }

                ++i;
            }

            return true;
        }

        private void SaveRealityObjectLand(RealityObject realityObject, Record record)
        {
            if (string.IsNullOrWhiteSpace(record.CadastreNumber))
            {
                return;
            }

            var realtyObjectLand = new RealityObjectLand
                {
                    RealityObject = realityObject,
                    CadastrNumber = record.CadastreNumber,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

            this.realityObjectLandToCreate.Add(realtyObjectLand);
        }

        private void SaveRealityObjectStructElements(RealityObject realityObject, Record record)
        {
            if (IStructElementImport != null)
            {
                var log = IStructElementImport.AddToSaveList(realityObject, record.StructuralElements);

                if (!string.IsNullOrWhiteSpace(log))
                {
                    AddLog(record.RowNumber, log, false);
                }
            }
        }

        private void AddLog(int rowNum, string text, bool success)
        {
            if (this.logDict.ContainsKey(rowNum))
            {
                var log = this.logDict[rowNum];

                log.Success &= success;
                log.Text = string.Format("{0}; {1}", text, log.Text);
            }
            else
            {
                this.logDict[rowNum] = new Log { Success = success, Text = text };
            }
        }

        private void WriteLogs()
        {
            if (LogImport.CountError > 0)
            {
                return;
            }

            foreach (var logPair in logDict.OrderBy(x => x.Key))
            {
                var text = string.Format("Строка: {0}; {1}", logPair.Key, logPair.Value.Text);

                if (logPair.Value.Success)
                {
                    LogImport.Info(importHeader, text);
                }
                else
                {
                    LogImport.Warn(importHeader, text);
                }
            }
        }

        private void SaveRealityObjectTechPassport(RealityObject realityObject, Record record)
        {
            if (ITechPassportDataImport != null)
            {
                var log = ITechPassportDataImport.AddToSaveList(realityObject, record.TechPassportData);
            }
        }

        public void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = Key;
        }
    }
}
