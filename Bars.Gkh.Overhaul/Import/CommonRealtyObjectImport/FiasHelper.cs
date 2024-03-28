namespace Bars.Gkh.Overhaul.Import.CommonRealtyObjectImport
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FiasHelper
    {
        public class FiasProxy
        {
            public string AoGuid;

            public string MirrorGuid;

            public string ParentGuid;

            public FiasActualStatusEnum ActStatus;

            public List<FiasProxy> Childen;

            public string CodeRecord;
        }

        public class FiasFullProxy
        {
            public string AoGuid;

            public string MirrorGuid;

            public string ParentGuid;

            public FiasLevelEnum AOLevel;

            public FiasActualStatusEnum ActStatus;

            public string OffName;

            public string ShortName;

            public string CodeRecord;
        }

        public class FiasProxyWithKladr
        {
            public string AoGuid;

            public string ParentGuid;

            public string OffName;

            public string ShortName;

            public string KladrCode;

            public string CodeRecord;

            public string PostalCode;

            public FiasLevelEnum AOLevel;

            public int? KladrCurrStatus;
        }

        public class ReverseFiasProxy
        {
            public string AoGuid;

            public ReverseFiasProxy Parent;
        }

        public enum ErrorType
        {
            NoError,
            MultipleExistance,
            Absence
        }

        public class FoundAddress
        {
            public DynamicAddress Address;

            public ErrorType Error;
        }

        private FiasProxy root;

        private Dictionary<string, FiasProxy> fiasDict;

        private Dictionary<string, FiasFullProxy> fiasfullDict;

        private Dictionary<string, List<string>> placeAoGuidsByPlaceName;

        private Dictionary<string, List<string>> placeAoGuidByStreetKladrCode;

        private Dictionary<string, Dictionary<string, FiasProxyWithKladr>> streetDataByStreetKladrCodeByParentGuid;

        // Данный словарик введен т.к некоторые улицы привязаны не к конкретному городу, а к какому-либо населенному
        //  пункту, в следствии этого их адрес не бьется через МО и приходится вводить такую адрес-прокладку
        private Dictionary<string, ReverseFiasProxy> placeAreaDict;

        private Dictionary<string, Dictionary<string, List<FiasProxyWithKladr>>> streetDataByNameByParentGuid;

        private Dictionary<string, List<string>> mirrorsDict = new Dictionary<string, List<string>>();

        private Dictionary<string, FoundAddress> streetsDict = new Dictionary<string, FoundAddress>();

        private Dictionary<string, FoundAddress> streetsKladrDict = new Dictionary<string, FoundAddress>();

        public FiasHelper(IWindsorContainer Container)
        {
            var fiasRepository = Container.Resolve<IRepository<Fias>>().GetAll();

            var fiasData = fiasRepository
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

            this.fiasfullDict = fiasData
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ActStatus).First());

            var temp = fiasData
                .Select(x => new FiasProxy
                {
                    AoGuid = x.AoGuid,
                    ParentGuid = x.ParentGuid,
                    ActStatus = x.ActStatus,
                    MirrorGuid = x.MirrorGuid,
                    CodeRecord = x.CodeRecord
                })
                .ToArray();

            var sourceForTree = temp
                .Where(x => string.IsNullOrWhiteSpace(x.MirrorGuid))
                .GroupBy(x => string.IsNullOrWhiteSpace(x.ParentGuid) ? "root" : x.ParentGuid)
                .ToDictionary(x => x.Key, x => x.ToList());

            this.fiasDict = temp
                .GroupBy(x => x.AoGuid)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ActStatus).First());

            if (!sourceForTree.ContainsKey("root"))
            {
                throw new Exception("Ошибка ФИАС");
            }

            this.root = sourceForTree["root"].OrderByDescending(x => x.ActStatus).First();

            this.GenerateTree(this.root, sourceForTree);

            var mirror = temp
                .Where(x => !string.IsNullOrWhiteSpace(x.MirrorGuid))
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .ToList();

            mirror.Where(x => this.fiasDict.ContainsKey(x.MirrorGuid)).ForEach(x =>
            {
                x.Childen = this.ChildrenDeepClone(this.fiasDict[x.MirrorGuid]);
            });

            mirror.Where(x => this.fiasDict.ContainsKey(x.ParentGuid)).ForEach(x =>
            {
                if (this.fiasDict[x.ParentGuid].Childen != null)
                {
                    this.fiasDict[x.ParentGuid].Childen.Add(x);
                }
                else
                {
                    this.fiasDict[x.ParentGuid].Childen = new List<FiasProxy> { x };
                }
            });

            this.FilterNonActual(this.root);

            this.placeAoGuidsByPlaceName = fiasData
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new { x.AoGuid, x.OffName, x.ShortName })
                .AsEnumerable()
                .Select(x => new
                {
                    name = ((x.OffName ?? string.Empty) + " " + (x.ShortName ?? string.Empty)).Trim().ToLower(),
                    x.AoGuid
                })
                .GroupBy(x => x.name)
                .ToDictionary(x => x.Key, x => x.Select(y => y.AoGuid).Distinct().ToList());

            var streets = fiasRepository
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.AOLevel == FiasLevelEnum.Street || x.AOLevel == FiasLevelEnum.Extr || x.AOLevel == FiasLevelEnum.Sext)
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
                .Where(x => x.KladrCurrStatus == 0)
                .Where(x => !string.IsNullOrWhiteSpace(x.KladrCode))
                .ToArray();

            this.placeAoGuidByStreetKladrCode = streetsWithKladr
                .GroupBy(x => x.KladrCode)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ParentGuid).Distinct().ToList());

            placeAreaDict = fiasRepository
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.AOLevel == FiasLevelEnum.Place)
                .Where(x => x.ParentGuid != null && x.ParentGuid != "")
                .GroupBy(x => x.AOGuid)
                .ToDictionary(x => x.Key, y => y.Select(x => new ReverseFiasProxy
                {
                    AoGuid = x.AOGuid,
                    Parent = new ReverseFiasProxy
                    {
                        AoGuid = x.ParentGuid
                    }
                }).First());

            this.streetDataByStreetKladrCodeByParentGuid = streetsWithKladr
                .GroupBy(x => x.ParentGuid)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.KladrCode).ToDictionary(y => y.Key, y => y.First()));

            this.streetDataByNameByParentGuid = streets
                .GroupBy(x => x.ParentGuid)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(у => ((у.OffName ?? string.Empty) + " " + (у.ShortName ?? string.Empty)).Trim().ToLower())
                          .ToDictionary(y => y.Key, y => y.ToList()));

            mirror.ForEach(x =>
            {
                if (this.streetDataByStreetKladrCodeByParentGuid.ContainsKey(x.MirrorGuid))
                {
                    this.streetDataByStreetKladrCodeByParentGuid[x.AoGuid] = this.streetDataByStreetKladrCodeByParentGuid[x.MirrorGuid];
                }

                if (this.streetDataByNameByParentGuid.ContainsKey(x.MirrorGuid))
                {
                    this.streetDataByNameByParentGuid[x.AoGuid] = this.streetDataByNameByParentGuid[x.MirrorGuid];
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

        private List<FiasProxy> ChildrenDeepClone(FiasProxy rec)
        {
            if (rec.Childen == null)
            {
                return null;
            }

            var res = rec.Childen.Select(x =>
                new FiasProxy
                {
                    ActStatus = x.ActStatus,
                    AoGuid = x.AoGuid,
                    CodeRecord = x.CodeRecord,
                    MirrorGuid = x.MirrorGuid,
                    ParentGuid = x.ParentGuid,
                    Childen = this.ChildrenDeepClone(x)
                })
                .ToList();

            return res;
        }

        private void GenerateTree(FiasProxy record, Dictionary<string, List<FiasProxy>> fiasDict)
        {
            if (!fiasDict.ContainsKey(record.AoGuid))
            {
                return;
            }

            record.Childen = fiasDict[record.AoGuid];

            record.Childen.ForEach(x => this.GenerateTree(x, fiasDict));
        }

        private void FilterNonActual(FiasProxy record)
        {
            if (record.Childen == null)
            {
                return;
            }

            record.Childen = record.Childen.Where(x => x.ActStatus == FiasActualStatusEnum.Actual).ToList();

            record.Childen.ForEach(this.FilterNonActual);
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

        public bool HasBranch(string guid)
        {
            return this.DeepSearch(this.root, guid).Any();
        }

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
                var fiasPlaceData = this.fiasfullDict[place.AoGuid];

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
                var fiasData = this.fiasfullDict[place.AoGuid];

                var addressGuid = string.Format("{0}_{1}", (byte)fiasData.AOLevel, place.AoGuid);
                var addressName = string.IsNullOrWhiteSpace(fiasData.ShortName) ? fiasData.OffName : string.Format("{0}. {1}", fiasData.ShortName, fiasData.OffName);

                address.AddressGuid = addressGuid + "#" + address.AddressGuid;
                address.AddressName = addressName + ", " + address.AddressName;

                place = place.Parent;
            }
        }

        public bool FindInBranch(
            string branchGuid,
            string placeName,
            string streetName,
            ref string faultReason,
            out DynamicAddress address,
            bool allowEmptyStreet = false)
        {
            address = null;
            placeName = placeName.ToLower();
            streetName = streetName.ToLower();

            var mixedKey = branchGuid + "#" + placeName + "#" + streetName;

            if (this.streetsDict.ContainsKey(mixedKey))
            {
                var result = this.streetsDict[mixedKey];
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

            if (!this.HasBranch(branchGuid))
            {
                faultReason = "В структуре ФИАС не найдена актуальная запись для муниципального образования.";
                return false;
            }

            var municipalityBranch = this.fiasDict[branchGuid];
            ReverseFiasProxy place;

            var municipalityBranchReverse = this.DeepSearch(this.root, branchGuid).First();

            if (string.IsNullOrEmpty(placeName))
            {
                place = municipalityBranchReverse;
            }
            else
            {
                if (!this.placeAoGuidsByPlaceName.ContainsKey(placeName))
                {
                    faultReason = "Не удалось найти соответствующий населенный пункт в системе.";
                    return false;
                }

                var placeGuids = this.placeAoGuidsByPlaceName[placeName];

                var placeBranches = this.DeepSearch(municipalityBranch, placeGuids, municipalityBranchReverse);

                if (placeBranches.Count > 1)
                {
                    faultReason = "В заданном МО найдено несколько соответствующих населенных пунктов.";
                    return false;
                }

                if (placeBranches.Count == 0)
                {
                    faultReason = "В заданном МО не найден соответствующий населенный пункт.";
                    return false;
                }

                place = placeBranches.First();
            }

            var placeCode = place.AoGuid;

            if (!allowEmptyStreet || !string.IsNullOrEmpty(streetName))
            {
                if (!this.streetDataByNameByParentGuid.ContainsKey(placeCode))
                {
                    faultReason = "В заданном населенном пункте не найдена соответствующая улица.";
                    this.streetsDict[mixedKey] = new FoundAddress { Error = ErrorType.Absence };
                    return false;
                }

                var streetDataByName = this.streetDataByNameByParentGuid[placeCode];

                if (!streetDataByName.ContainsKey(streetName))
                {
                    faultReason = "В заданном населенном пункте не найдена соответствующая улица.";
                    this.streetsDict[mixedKey] = new FoundAddress { Error = ErrorType.Absence };
                    return false;
                }

                var streets = streetDataByName[streetName];

                if (streets.Count > 1)
                {
                    faultReason = "В заданном населенном пункте найдено несколько соответствующих улиц.";
                    this.streetsDict[mixedKey] = new FoundAddress { Error = ErrorType.MultipleExistance };
                    return false;
                }

                if (streets.Count == 0)
                {
                    faultReason = "В заданном населенном пункте не найдена соответствующая улица.";
                    this.streetsDict[mixedKey] = new FoundAddress { Error = ErrorType.Absence };
                    return false;
                }

                var streetdata = streets.First();

                var addressStreetName = string.IsNullOrWhiteSpace(streetdata.ShortName)
                                     ? streetdata.OffName
                                     : string.Format("{0}. {1}", streetdata.ShortName, streetdata.OffName);

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
                address = new DynamicAddress();
            }

            this.GetFullAddress(address, place);

            this.streetsDict[mixedKey] = new FoundAddress { Address = address, Error = ErrorType.NoError };

            return true;
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

            if (!this.HasBranch(branchGuid))
            {
                faultReason = "В структуре ФИАС не найдена актуальная запись для муниципального образования.";
                return false;
            }

            if (!this.HasValidStreetKladrCode(streetKladrCode))
            {
                faultReason = "Заданный код КЛАДР не зарегистрирован в ФИАС";
                return false;
            }

            var municipalityBranch = this.fiasDict[branchGuid];

            var municipalityBranchReverse = this.DeepSearch(this.root, branchGuid).First();

            if (this.placeAoGuidByStreetKladrCode[streetKladrCode].Count > 1)
            {
                faultReason = "По данному коду кладр в системе существует несколько улиц";
                return false;
            }

            var placeGuids = new List<string>(this.placeAoGuidByStreetKladrCode[streetKladrCode]);
            var firstPlace = placeGuids.First();

            if (this.mirrorsDict.ContainsKey(firstPlace))
            {
                placeGuids.AddRange(this.mirrorsDict[firstPlace]);
            }

            var placeBranches = this.DeepSearch(municipalityBranch, placeGuids, municipalityBranchReverse);

            // Признак что данная улица не привязана к МО
            var noMoLink = false;
            if (placeBranches.Count == 0)
            {
                // Проверяем не относится ли улица к н.п
                if (placeAreaDict.ContainsKey(firstPlace))
                {
                    noMoLink = true;
                    placeBranches.Add(placeAreaDict[firstPlace]);
                } 
            }

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

            var streetdata = this.streetDataByStreetKladrCodeByParentGuid[placeCode][streetKladrCode];

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

            // Если улица не привязана к МО, но МО указанный для этой улицы найден - то 
            //  так как мы не можем знать есть ли связь между улицей и МО навярняка - полагаемся на файл,
            //  потому что в нашей системе улицы могут быть привязаны к населенному пункту и не иметь привязки к МО 
            //  смотреть переписку по задаче GKH-167
            GetFullAddress(address, noMoLink && place.Parent != null
                ? new ReverseFiasProxy
                {
                    AoGuid = municipalityBranch.AoGuid,
                    Parent = new ReverseFiasProxy { AoGuid = municipalityBranch.ParentGuid }
                }
                : place);

            streetsKladrDict[mixedKey] = new FoundAddress { Address = address, Error = ErrorType.NoError };

            return true;
        }
    }
}