namespace Bars.Gkh.Import.FiasHelper
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Castle.Core;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Фиас хелпер, обрабатывающий адреса по включению, а не по строгому совпадению
    /// </summary>
    public class UnstrongFiasHelper : IFiasHelper, IInitializable
    {
        #region Fields

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Корневой элемент
        /// </summary>
        private FiasProxy _rootElement;

        /// <summary>
        /// все элементы, кроме улиц
        /// </summary>
        private Dictionary<string, FiasProxy> _fullFiasProxyDictionary;

        /// <summary>
        /// все элементы, кроме улиц
        /// </summary>
        private Dictionary<string, FiasFullProxy> _fullFiasFullProxyDictionary;

        /// <summary>
        /// Словарь городов: название города - гуиды городов с таким названием
        /// </summary>
        private Dictionary<string, List<string>> _placeGuidByPlaceNameDictionary;

        /// <summary>
        /// Словарь улиц: кладр - гуид предка
        /// </summary>
        private Dictionary<string, List<string>> placeAoGuidByStreetKladrCode;

        /// <summary>
        /// Словарь улиц: гуид предка - словарь (кладр - FiasProxyWithKladr)
        /// </summary>
        private Dictionary<string, Dictionary<string, FiasProxyWithKladr>> _streetDataByStreetKladrCodeByParentGuid;

        /// <summary>
        /// Словарь: гуид предка - словарь (название улицы - FiasProxyWithKladr)
        /// </summary>
        private Dictionary<string, Dictionary<string, List<FiasProxyWithKladr>>> _streetDataByNameByParentGuid;

        /// <summary>
        /// ?
        /// </summary>
        private Dictionary<string, List<string>> mirrorsDict = new Dictionary<string, List<string>>();
        

        /// <summary>
        /// Кэш уже найденных адресов по кладр
        /// Ключ имеет вид branchGuid + "#" + streetKladrCode;
        /// </summary>
        private Dictionary<string, SearchResult> streetsKladrDict = new Dictionary<string, SearchResult>();
        
        #endregion

        #region Public methods

        /// <summary>
        /// Формирование всех словарей из базы
        /// </summary>
        public void Initialize()
        {
            var fiasRepository = Container.Resolve<IRepository<Fias>>().GetAll();

            //-----формируем словарь типа FiasFullProxy-----
            var fiasFullProxyData = fiasRepository
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.AOLevel != FiasLevelEnum.Street)
                .Where(x => x.AOLevel != FiasLevelEnum.Extr)
                .Where(x => x.AOLevel != FiasLevelEnum.Sext)
                .Where(x => x.AOGuid != null && x.AOGuid != "")
                .Select(x => new FiasFullProxy
                {
                    AoGuid = x.AOGuid,
                    ParentGuid = x.ParentGuid,
                    ActStatus = x.ActStatus,
                    MirrorGuid = x.MirrorGuid,
                    OffName = x.OffName,
                    ShortName = x.ShortName,
                    AOLevel = x.AOLevel,
                    CodeRecord = x.CodeRecord
                })
                .ToArray();

            _fullFiasFullProxyDictionary = fiasFullProxyData
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ActStatus).First());

            //-----формируем словарь типа FiasProxy-----
            var fiasProxyData = fiasFullProxyData
                .Select(x => new FiasProxy
                {
                    AoGuid = x.AoGuid,
                    ParentGuid = x.ParentGuid,
                    ActStatus = x.ActStatus,
                    MirrorGuid = x.MirrorGuid,
                    CodeRecord = x.CodeRecord
                })
                .ToArray();

            _fullFiasProxyDictionary = fiasProxyData
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ActStatus).First());

            //-----формируем дерево элементов-----
            var sourceForTree = fiasProxyData
                .Where(x => string.IsNullOrWhiteSpace(x.MirrorGuid) && x.ActStatus == FiasActualStatusEnum.Actual)
                .GroupBy(x => string.IsNullOrWhiteSpace(x.ParentGuid) ? "root" : x.ParentGuid)
                .ToDictionary(x => x.Key, x => x.ToList());

            if (sourceForTree["root"].Count() == 0)
                throw new Exception("В таблице ФИАС не найден элемент с пустым ParentGuid, нет корневого элемента для дерева адресов");
            else if (sourceForTree["root"].Count() > 1)
                throw new Exception($"В таблице ФИАС найдено {sourceForTree["root"].Count()} элементов с пустым ParentGuid, корневой элемент не однозначен");

            _rootElement = sourceForTree["root"].First();

            GenerateTree(_rootElement, sourceForTree);

            //-----обрабатываем зеркалированные элементы-----
            var mirror = fiasProxyData
                .Where(x => !string.IsNullOrWhiteSpace(x.MirrorGuid))
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .ToList();

            //-----добавляем ветки для зеркалированных элементов-----
            mirror.Where(x => _fullFiasProxyDictionary.ContainsKey(x.MirrorGuid)).ForEach(x =>
            {
                x.Childen = this.ChildrenDeepClone(this._fullFiasProxyDictionary[x.MirrorGuid]);
            });

            mirror.Where(x => this._fullFiasProxyDictionary.ContainsKey(x.ParentGuid)).ForEach(x =>
            {
                if (this._fullFiasProxyDictionary[x.ParentGuid].Childen != null)
                {
                    this._fullFiasProxyDictionary[x.ParentGuid].Childen.Add(x);
                }
                else
                {
                    this._fullFiasProxyDictionary[x.ParentGuid].Childen = new List<FiasProxy> { x };
                }
            });

            //-----формируем список городов-----
            _placeGuidByPlaceNameDictionary = fiasFullProxyData
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new { x.AoGuid, x.OffName, x.ShortName })
                .AsEnumerable()
                .Select(x => new
                {
                    name = NormalizeToCompare((x.OffName ?? string.Empty) + " " + (x.ShortName ?? string.Empty)),
                    x.AoGuid
                })
                .GroupBy(x => x.name)
                .ToDictionary(x => x.Key, x => x.Select(y => y.AoGuid).Distinct().ToList());

            //-----формируем список улиц-----
            var streets = fiasRepository
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.AOLevel == FiasLevelEnum.Street || x.AOLevel == FiasLevelEnum.Extr || x.AOLevel == FiasLevelEnum.Sext || x.AOLevel == FiasLevelEnum.PlanningStruct)
                .Where(x => x.ParentGuid != null && x.ParentGuid != "")
                .Select(x => new FiasProxyWithKladr
                {
                    AoGuid = x.AOGuid,
                    KladrCode = x.KladrCode,
                    ParentGuid = x.ParentGuid,
                    OffName = x.OffName,
                    ShortName = x.ShortName,
                    CodeRecord = x.CodeRecord,
                    PostalCode = x.PostalCode,
                    AOLevel = x.AOLevel,
                    KladrCurrStatus = x.KladrCurrStatus
                })
                .ToArray();

            var streetsWithKladr = streets
                .Where(x => !string.IsNullOrWhiteSpace(x.KladrCode))
                .ToArray();

            placeAoGuidByStreetKladrCode = streetsWithKladr
                .GroupBy(x => x.KladrCode)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ParentGuid).Distinct().ToList());

            _streetDataByStreetKladrCodeByParentGuid = streetsWithKladr
                .GroupBy(x => x.ParentGuid)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.KladrCode).ToDictionary(y => y.Key, y => y.First()));

            _streetDataByNameByParentGuid = streets
                .Where(x => x.KladrCurrStatus == 0)
                .GroupBy(x => x.ParentGuid)
                .ToDictionary(
                    x => x.Key, //ParentGuid
                    x => x.GroupBy(у => NormalizeToCompare((у.OffName ?? string.Empty) + (у.ShortName ?? string.Empty))).ToDictionary(
                        y => y.Key, //OffName
                        y => y.ToList()
                        ));

            //-----словарь для зеркал-----
            mirror.ForEach(x =>
            {
                if (this._streetDataByStreetKladrCodeByParentGuid.ContainsKey(x.MirrorGuid))
                {
                    this._streetDataByStreetKladrCodeByParentGuid[x.AoGuid] = this._streetDataByStreetKladrCodeByParentGuid[x.MirrorGuid];
                }

                if (this._streetDataByNameByParentGuid.ContainsKey(x.MirrorGuid))
                {
                    this._streetDataByNameByParentGuid[x.AoGuid] = this._streetDataByNameByParentGuid[x.MirrorGuid];
                }

                if (this.mirrorsDict.ContainsKey(x.MirrorGuid))
                {
                    this.mirrorsDict[x.MirrorGuid].Add(x.AoGuid);
                }
                else
                {
                    this.mirrorsDict[x.MirrorGuid] = new List<string> { x.AoGuid };
                }
            });
        }

        /// <summary>
        /// Возвращает DinamicAddress улицы по guid муниципального образования и названию города и улицы
        /// </summary>
        /// <param name="branchGuid">Guid муниципального образования</param>
        /// <param name="placeName">Город</param>
        /// <param name="streetName">Улица</param>
        /// <param name="faultReason">Ошибка обработки</param>
        /// <param name="address">Найденный адрес</param>
        /// <returns>true, если успешно</returns>
        public bool FindInBranch(string branchGuid, string placeName, string streetName, ref string faultReason, out DynamicAddress address)
        {
            address = null;
            placeName = placeName.ToLower();
            streetName = streetName.ToLower();

            //Поиск в словаре ранее искавшихся
            var key = SearchCache.GetKey(branchGuid,placeName,streetName);

            var cacheResult = SearchCache.GetResult(key);

            if (cacheResult!=null)
            {          
                switch (cacheResult.Error)
                {
                    case ErrorType.NoError:
                        address = cacheResult.Address;
                        return true;
                    case ErrorType.MultipleExistance:
                        faultReason = $"(Повтор) В населенном пункте \"{placeName}\" найдено несколько соответствующих улиц \"{streetName}\"";
                        return false;
                    case ErrorType.Absence:
                        faultReason = $"(Повтор) В населенном пункте \"{placeName}\" не найдена соответствующая улица \"{streetName}\"";
                        return false;
                }                
            }

            //-----Поиск муниципального образования-----
            ReverseFiasProxy municipalityBranchReverse = DeepSearch(_rootElement, branchGuid).FirstOrDefault();

            if (municipalityBranchReverse == null)
            {
                faultReason = $"В структуре ФИАС не найдена актуальная запись для муниципального образования {branchGuid}";
                return false;
            }

            FiasProxy municipalityBranch = _fullFiasProxyDictionary[branchGuid];       
              
            //-----Поиск города-----
            ReverseFiasProxy place;            

            if (string.IsNullOrEmpty(placeName))
            {
                place = municipalityBranchReverse; //если название города пустое, пропускаем этот уровень фиаса
            }
            else
            {
                //поиск города по нахождению названия в словаре городов
                var placePairs = _placeGuidByPlaceNameDictionary.Where(x => x.Key == NormalizeToCompare(placeName)); 

                if (placePairs.Count() == 0)
                {
                    faultReason = $"Не удалось найти по названию населенный пункт \"{placeName}\", МО {branchGuid}";
                    return false;
                }
                else if(placePairs.Count() > 1)
                {
                    //ошибка, если несколько названий фиас городов включены в placeName
                    faultReason = $"Найдено по названию  \"{placePairs.Count()}\" \"{placeName}\" населенных пунктов: {string.Join(", ", placePairs.Select(x => x.Key))}, МО {branchGuid}";
                    return false;
                }

                //несколько мо с одним названием это норма
                List<string> placeGiuds = placePairs.First().Value;

                //поиск объекта города по гуидам
                List<ReverseFiasProxy> placeBranches = DeepSearch(municipalityBranch, placeGiuds, municipalityBranchReverse);

                if (placeBranches.Count > 1)
                {
                    faultReason = $"По идентификаторам \"{string.Join(", ", placeGiuds)}\" в базе найдено {placeBranches.Count} населенных пунктов: {string.Join(", ", placeBranches.Select(x => x.AoGuid))}, город {placeName}, МО {branchGuid}";
                    return false;
                }
                else if (placeBranches.Count == 0)
                {
                    faultReason = $"По идентификаторам \"{string.Join(", ", placeGiuds)}\" в базе не найдено населенных пунктов, город {placeName}, МО {branchGuid}";
                    return false;
                }

                place = placeBranches.First();
            }

            //-----Поиск улицы-----
            if (!string.IsNullOrEmpty(streetName))
            {        
                //выбираем ветку населенного пункта
                if (!_streetDataByNameByParentGuid.ContainsKey(place.AoGuid))
                {
                    SearchCache.Add(key, ErrorType.Absence);                    

                    faultReason =  $"В словаре улиц нет ветки для населенного пункта с идентификатором \"{place.AoGuid}\", город {placeName}, МО {branchGuid}";
                    return false;
                }                
                
                Dictionary<string, List<FiasProxyWithKladr>> streetDataByName = _streetDataByNameByParentGuid[place.AoGuid];

                //ищем улицу по названию
                var placePairs = streetDataByName.Where(x => x.Key == NormalizeToCompare(streetName));

                if (placePairs.Count() == 0)
                {
                    SearchCache.Add(key, ErrorType.Absence);

                    faultReason = $"Не найдена по названию улица \"{streetName}\", город {placeName}, МО {branchGuid}";
                    return false;
                }
                else if (placePairs.Count() > 1)
                {
                    SearchCache.Add(key, ErrorType.MultipleExistance);

                    faultReason = $"Найдено по названию \"{streetName}\" {placePairs.Count()} улиц: {string.Join(", ", placePairs.Select(x => x.Key))}, город {placeName}, МО {branchGuid}";
                    return false;
                }

                var streets = placePairs.First().Value;

                //проверяем, сколько записей в базе сопоставлено этой улице
                if (streets.Count > 1)
                {
                    SearchCache.Add(key, ErrorType.MultipleExistance);

                    faultReason = $"Найдено в базе {streets.Count} улиц с названием \"{streetName}\": {string.Join(", ", streets.Select(x => x.AoGuid))}, город {placeName}, МО {branchGuid}";                    
                    return false;
                }

                if (streets.Count == 0)
                {
                    SearchCache.Add(key, ErrorType.Absence);

                    faultReason = $"Не найдено в базе улиц с названием \"{streetName}\", город {placeName}, МО {branchGuid}";                    
                    return false;
                }

                FiasProxyWithKladr streetdata = streets.First();

                //создаем новый DynamicAddress
                var addressStreetName = string.IsNullOrWhiteSpace(streetdata.ShortName)
                    ? streetdata.OffName
                    : $"{streetdata.ShortName}. {streetdata.OffName}";

                address = new DynamicAddress
                {
                    AddressGuid = string.Format("{0}_{1}", (byte)streetdata.AOLevel, streetdata.AoGuid),
                    AddressName = addressStreetName,
                    PostCode = streetdata.PostalCode,
                    Name = addressStreetName,
                    Code = streetdata.CodeRecord,
                    GuidId = streetdata.AoGuid
                };
            }
            else
            {
                //если название улицы пустое, формируем адрес сразу из города

                var placeInfo = this._fullFiasFullProxyDictionary[place.AoGuid];

                var placeAddressName = string.IsNullOrWhiteSpace(placeInfo.ShortName)
                    ? placeInfo.OffName
                    : $"{placeInfo.ShortName}. {placeInfo.OffName}";

                address = new DynamicAddress
                {
                    AddressGuid = string.Format("{0}_{1}", (byte)placeInfo.AOLevel, placeInfo.AoGuid),
                    AddressName = placeAddressName,
                    Name = placeAddressName,
                    Code = placeInfo.CodeRecord,
                    GuidId = placeInfo.AoGuid
                };
                place = place.Parent;
            }

            //дополняем адрес предками и всем таким
            GetFullAddress(address, place);

            SearchCache.Add(key, address);

            return true;
        }

        /// <summary>
        /// очищает кэш поиска
        /// </summary>
        public void ClearCache()
        {
            SearchCache.Clear();
        }

        public bool HasValidStreetKladrCode(string streetKladrCode)
        {
            return this.placeAoGuidByStreetKladrCode.ContainsKey(streetKladrCode);
        }

        public bool FindInBranchByKladr(string branchGuid, string streetKladrCode, ref string faultReason, out DynamicAddress address)
        {
            address = null;
            var mixedKey = branchGuid + "#" + streetKladrCode;

            if (this.streetsKladrDict.ContainsKey(mixedKey))
            {
                var result = this.streetsKladrDict[mixedKey];
                if (result.Error == ErrorType.NoError)
                {
                    address = result.Address;
                    return true;
                }

                switch (result.Error)
                {
                    case ErrorType.MultipleExistance:
                        faultReason = "В заданном населенном пункте найдено несколько соответствующих улиц.";
                        break;

                    case ErrorType.Absence:
                        faultReason = "В заданном населенном пункте не найдена соответствующая улица.";
                        break;
                }

                return false;
            }

            if (!this.IncludeInBranch(branchGuid))
            {
                faultReason = "В структуре ФИАС не найдена актуальная запись для муниципального образования.";
                return false;
            }

            if (!this.HasValidStreetKladrCode(streetKladrCode))
            {
                faultReason = "Заданный код КЛАДР не зарегистрирован в ФИАС";
                return false;
            }

            var municipalityBranch = this._fullFiasProxyDictionary[branchGuid];

            var municipalityBranchReverse = this.DeepSearch(this._rootElement, branchGuid).First();

            var placeGuids = this.placeAoGuidByStreetKladrCode[streetKladrCode].ToList(); // Создаем новый список, чтобы не портить словарь

            if (placeGuids.Count > 1)
            {
                faultReason = "По данному коду кладр в системе существует несколько улиц";
                return false;
            }

            if (this.mirrorsDict.ContainsKey(placeGuids.First()))
            {
                placeGuids.AddRange(this.mirrorsDict[placeGuids.First()]);
            }

            var placeBranches = this.DeepSearch(municipalityBranch, placeGuids, municipalityBranchReverse);

            if (placeBranches.Count > 1)
            {
                faultReason = "В заданном МО найдено несколько соответствующих населенных пунктов.";
                return false;
            }

            if (placeBranches.Count == 0)
            {
                if (placeGuids.Contains(municipalityBranch.AoGuid))
                {
                    placeBranches.Add(municipalityBranchReverse);
                }
                else
                {
                    faultReason = "В заданном МО не найден соответствующий населенный пункт.";
                    return false;
                }
            }

            var place = placeBranches.First();

            var placeCode = place.AoGuid;

            var streetdata = this._streetDataByStreetKladrCodeByParentGuid[placeCode][streetKladrCode];

            var streetName = string.IsNullOrWhiteSpace(streetdata.ShortName)
                                 ? streetdata.OffName
                                 : string.Format("{0}. {1}", streetdata.ShortName, streetdata.OffName);

            address = new DynamicAddress
            {
                AddressGuid = string.Format("{0}_{1}", (byte)streetdata.AOLevel, streetdata.AoGuid),
                AddressName = streetName,
                PostCode = streetdata.PostalCode,
                Name = streetName,
                Code = streetdata.CodeRecord,
                GuidId = streetdata.AoGuid
            };

            this.GetFullAddress(address, place);

            this.streetsKladrDict[mixedKey] = new SearchResult { Address = address, Error = ErrorType.NoError };

            return true;
        }

        public FiasAddress CreateFiasAddress(DynamicAddress address, string house, string letter, string housing, string building)
        {
            if (address == null)
            {
                return null;
            }

            var addressName = new StringBuilder(address.AddressName);

            if (!string.IsNullOrEmpty(house))
            {
                addressName.Append(", д. ");
                addressName.Append(house);

                if (!string.IsNullOrEmpty(letter))
                {
                    addressName.Append(", лит. ");
                    addressName.Append(letter);
                }

                if (!string.IsNullOrEmpty(housing))
                {
                    addressName.Append(", корп. ");
                    addressName.Append(housing);
                }

                if (!string.IsNullOrEmpty(building))
                {
                    addressName.Append(", секц. ");
                    addressName.Append(building);
                }
            }

            var fiasAddress = new FiasAddress
            {
                AddressGuid = address.AddressGuid,
                AddressName = addressName.ToString(),
                PostCode = address.PostCode,
                StreetGuidId = address.GuidId,
                StreetName = address.Name,
                StreetCode = address.Code,
                Letter = letter,
                House = house,
                Housing = housing,
                Building = building,

                PlaceAddressName = address.AddressName.Replace(address.Name, string.Empty).Trim(' ').Trim(','),
                PlaceGuidId = address.ParentGuidId,
                PlaceName = address.ParentName,
                PlaceCode = address.PlaceCode
            };

            return fiasAddress;
        }

        #endregion 

        #region Private methods

        /// <summary>
        /// Рекурсивно озвращает список FiasProxy из списка Childen записи
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        private List<FiasProxy> ChildrenDeepClone(FiasProxy rec)
        {
            if (rec.Childen == null)
            {
                return null;
            }

            return rec.Childen.Select(x =>
                    new FiasProxy
                    {
                        ActStatus = x.ActStatus,
                        AoGuid = x.AoGuid,
                        CodeRecord = x.CodeRecord,
                        MirrorGuid = x.MirrorGuid,
                        ParentGuid = x.ParentGuid,
                        Childen = this.ChildrenDeepClone(x)
                    }).ToList();
        }

        /// <summary>
        /// Рекурсивно заполняет поле Childen по AoGuid
        /// </summary>
        /// <param name="record">FiasProxy, которую нужно заполнить</param>
        /// <param name="fiasDict">Словарь, из которого брать элементы для заполнения</param>
        private void GenerateTree(FiasProxy record, Dictionary<string, List<FiasProxy>> fiasDict)
        {
            if (!fiasDict.ContainsKey(record.AoGuid))
            {
                return;
            }

            record.Childen = fiasDict[record.AoGuid];

            record.Childen.ForEach(x => this.GenerateTree(x, fiasDict));
        }


        private List<ReverseFiasProxy> BuildReverseList(FiasProxy record, ReverseFiasProxy parent = null)
        {
            var me = new ReverseFiasProxy { AoGuid = record.AoGuid, Parent = parent };

            var list = new List<ReverseFiasProxy> { me };

            if (record.Childen != null)
            {
                list.AddRange(record.Childen.SelectMany(x => this.BuildReverseList(x, me)).ToList());
            }

            return list;
        }

        /// <summary>
        /// Проверяет наличие элемента в дереве адресов
        /// </summary>
        /// <param name="guid">guid элемента</param>
        /// <returns>true, если найден</returns>
        public bool IncludeInBranch(string guid)
        {
            return DeepSearch(_rootElement, guid).Any();
        }

        /// <summary>
        /// Поиск элементов по guid
        /// </summary>
        /// <param name="record">Корневой элемент, с которого начинать поиск</param>
        /// <param name="placeGuids">список guid</param>
        /// <param name="parent">(для рекурсии)</param>
        /// <returns></returns>
        private List<ReverseFiasProxy> DeepSearch(FiasProxy record, List<string> placeGuids, ReverseFiasProxy parent = null)
        {
            var res = new List<ReverseFiasProxy>();

            if (parent == null)
            {
                parent = new ReverseFiasProxy { AoGuid = record.AoGuid, Parent = null };
            }

            if (placeGuids.Contains(record.AoGuid))
            {
                res.Add(parent);
            }

            if (record.Childen == null)
            {
                return res;
            }

            res.AddRange(record.Childen.SelectMany(x => this.DeepSearch(x, placeGuids, new ReverseFiasProxy { AoGuid = x.AoGuid, Parent = parent })));

            return res;
        }

        /// <summary>
        /// Поиск элемента по guid
        /// </summary>
        /// <param name="record">Корневой элемент, с которого начинать поиск</param>
        /// <param name="placeGuid">список guid</param>
        /// <param name="parent">Текущий элемент (для рекурсии)</param>
        /// <returns></returns>
        private List<ReverseFiasProxy> DeepSearch(FiasProxy record, string placeGuid, ReverseFiasProxy parent = null)
        {
            var res = new List<ReverseFiasProxy>();

            if (parent == null)
            {
                parent = new ReverseFiasProxy { AoGuid = record.AoGuid, Parent = null };
            }

            if (record.AoGuid == placeGuid)
            {
                res.Add(parent);
            }

            if (record.Childen == null)
            {
                return res;
            }

            res.AddRange(record.Childen.SelectMany(x => this.DeepSearch(x, placeGuid, new ReverseFiasProxy { AoGuid = x.AoGuid, Parent = parent })));

            return res;
        }

        private void GetFullAddress(DynamicAddress address, ReverseFiasProxy place)
        {
            if (place != null)
            {
                var fiasPlaceData = this._fullFiasFullProxyDictionary[place.AoGuid];

                if (string.IsNullOrEmpty(address.ParentGuidId))
                {
                    address.ParentGuidId = place.AoGuid;
                }

                if (string.IsNullOrEmpty(address.ParentName))
                {
                    address.ParentName = string.IsNullOrWhiteSpace(fiasPlaceData.ShortName)
                                             ? fiasPlaceData.OffName
                                             : string.Format("{0}. {1}", fiasPlaceData.ShortName, fiasPlaceData.OffName);
                }

                if (string.IsNullOrEmpty(address.PlaceCode))
                {
                    address.PlaceCode = fiasPlaceData.CodeRecord;
                }
            }

            while (place != null)
            {
                var fiasData = this._fullFiasFullProxyDictionary[place.AoGuid];

                var addressGuid = string.Format("{0}_{1}", (byte)fiasData.AOLevel, place.AoGuid);
                var addressName = string.IsNullOrWhiteSpace(fiasData.ShortName) ? fiasData.OffName : string.Format("{0}. {1}", fiasData.ShortName, fiasData.OffName);

                address.AddressGuid = addressGuid + "#" + address.AddressGuid;
                address.AddressName = addressName + ", " + address.AddressName;

                place = place.Parent;
            }
        }

        private string NormalizeToCompare(string str)
        {
            if (str == null)
                return null;

            return str.Trim().Replace(" ","").Replace(",", "").Replace(".", "").ToLower();
        }

        #endregion

        #region nested classes

        /// <summary>
        /// Реализация кэша поиска по адресу
        /// </summary>
        private static class SearchCache
        {
            /// <summary>
            /// Кэш уже найденных адресов по названиям
            /// </summary>
            private static Dictionary<string, SearchResult> _cachedSearchResultDict = new Dictionary<string, SearchResult>();

            /// <summary>
            /// Добавить запись о неуспешном поиске в кеш
            /// </summary>
            /// <param name="key">ключ</param>
            /// <param name="result">ошибка</param>
            internal static void Add(string key, ErrorType result)
            {
                _cachedSearchResultDict[key] = new SearchResult { Error = result };
            }

            /// <summary>
            /// Добавить запись о успешном поиске в кеш
            /// </summary>
            /// <param name="key">ключ</param>
            /// <param name="address">найденный адрес</param>
            internal static void Add(string key, DynamicAddress address)
            {
                _cachedSearchResultDict[key] = new SearchResult { Address = address, Error = ErrorType.NoError };
            }

            internal static void Clear()
            {
                _cachedSearchResultDict.Clear();
            }

            /// <summary>
            /// Проверка наличия ключа в кеше
            /// </summary>
            internal static bool ContainsKey(string key)
            {
                return _cachedSearchResultDict.ContainsKey(key);
            }

            /// <summary>
            /// Формирование ключа
            /// </summary>
            /// <param name="branchGuid">гуид</param>
            /// <param name="placeName"></param>
            /// <param name="streetName"></param>
            /// <returns></returns>
            internal static string GetKey(string branchGuid, string placeName, string streetName)
            {
                return branchGuid + "#" + placeName + "#" + streetName;
            }

            /// <summary>
            /// Возвращает результат из кеша
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            internal static SearchResult GetResult(string key)
            {
                if (!_cachedSearchResultDict.ContainsKey(key))
                    return null;

                return _cachedSearchResultDict[key];
            }
        }
        #endregion
    }
}