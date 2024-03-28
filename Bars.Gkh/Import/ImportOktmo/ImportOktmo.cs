using Bars.Gkh.Entities.Dicts;
using NHibernate.Type;

namespace Bars.Gkh.Import.ImportOktmo
{
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using System.Linq;
    using System.Diagnostics;
    using System.IO;
    using System.Collections.Generic;
    using System.Text;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Castle.Windsor;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Impl;
    using NHibernate;
    using NHibernate.Persister.Entity;
    using Npgsql;

    using System;
    using System.Reflection;

    /// <summary>
    /// Импорт данных из ОКТМО
    /// </summary>
    public class ImportOktmo : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public ILogImportManager LogManager { get; set; }
        public IWindsorContainer Container { get; set; }
        public ILogImport LogImport { get; set; }
        public ILogImportManager LogImportManager { get; set; }

        protected List<Municipality> _items;

        public IDomainService<Fias> FiasService { get; set; }

        protected Dictionary<string, TypeMunicipality> MoLevelDict = new Dictionary<string, TypeMunicipality>
        {
            {"сельское поселение",TypeMunicipality.Settlement},
            {"городское поселение",TypeMunicipality.UrbanSettlement},
            {"муниципальный район",TypeMunicipality.MunicipalArea},
            {"городской округ",TypeMunicipality.UrbanArea},
            {"внутригородская территория города федерального значения",TypeMunicipality.InterCityArea}
        };

        public IDomainService<Municipality> moService { get; set; }

        public IDomainService<FiasAddress> FiasAddressService { get; set; }

        public IDomainService<FiasOktmo> FiasOktmoService { get; set; }

        protected List<Municipality> allMo;

        protected ILogImport Logger;

        private Boolean IsFirstLoad;

        #region IGkhImport Members

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "OktmoImport"; }
        }

        public override string Name
        {
            get { return "Импорт муниципальных образований"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.Gkh.View"; }
        }

        public int SplitCount
        {
            get { return 12; }
        }

        protected Dictionary<string,Fias> actualFias;
        protected List< Fias> allFias;

        //protected Dictionary<string, Fias> allFias;

        public void ProcessLine(string archiveName, string[] splits)
        {
            var now = DateTime.Now;

            // если ОКТМО отсутствует, то берем временный (установленный территориальным органом Росстата в проекте изменений к ОКТМО)
            var oktmo = !String.IsNullOrEmpty(splits[5].Trim()) ? splits[5].Trim() : splits[6].Trim();
            var okato = splits[4].Trim();

            if (String.IsNullOrEmpty(oktmo))
            {
                return;
            }

            if (oktmo.Length != 8)
            {
                return;
            }

            if (splits[7] != null && splits[7].ToLower().Contains("территория"))
            {
                return;
            }

            var fias = allFias.Where(x => 
                //x.ActStatus == FiasActualStatusEnum.Actual &&
                ((!String.IsNullOrEmpty(okato) && x.OKATO == okato) || x.OKTMO == oktmo))
                .OrderBy(x => x.AOLevel)
                .FirstOrDefault();

            if ((fias == null || okato == "92401000000" || okato == "22401000000") && (oktmo != "98660000" || oktmo != "098660000"))
            {
                return;
            }

            var m = allMo.FirstOrDefault(x => x.Okato == okato || x.Oktmo == oktmo);

            if (m == null)
            {
                m = new Municipality
                {
                    CheckCertificateValidity = false,
                    Code = string.Empty,
                    Okato = okato,
                    FiasId = fias != null ? fias.AOGuid : null,
                    Name = splits[7],
                    Oktmo = oktmo,
                    ObjectCreateDate = now,
                    ObjectEditDate = now,
                    IsOld = false
                };

                m.Level = MoLevelDict.ContainsKey(splits[2]) ? MoLevelDict[splits[2]] : TypeMunicipality.Settlement;

                if (m.Level == TypeMunicipality.UrbanArea)
                {
                    m.Name = m.Name.Replace("город", "городской округ");
                }

                Add(m);

                LogImport.CountAddedRows++;
            }
            else
            {
                    m.Name = splits[7];
                    m.Oktmo = oktmo;
                    m.Okato = okato;

                    if (IsFirstLoad && !m.IsOld)
                    {
                        m.IsOld = true;
                    }

                    m.Level = MoLevelDict.ContainsKey(splits[2]) ? MoLevelDict[splits[2]] : TypeMunicipality.Settlement;

                    if (m.Level == TypeMunicipality.UrbanArea)
                    {
                        m.Name = m.Name.Replace("город", "городской округ");
                    }

                    moService.Update(m);

                    LogImport.CountChangedRows++;

                    LogImport.Info(m.Name,string.Format("Обновлено существующее МО: {0}, октмо : {1},  окато : {2}", m.Name, m.Oktmo, m.Okato));
            }
        }

        protected virtual void Add(Municipality item)
        {
            _items.Add(item);
            LogImport.Info(item.Name,string.Format("Добавлено новое МО: {0}, октмо : {1},  окато : {2}", item.Name, item.Oktmo, item.Okato));
        }

        public void ImportInternal(Stream file, string fileName, ILogImport logger = null)
        {

            if (file.CanSeek)
            {
                file.Position = 0;
            }

            moService = Container.Resolve<IDomainService<Municipality>>();
            FiasService = Container.Resolve<IDomainService<Fias>>();

            allMo = GetAllMo();
            _items = new List<Municipality>();


            allFias = FiasService.GetAll()
                .Where(x => x.AOLevel != FiasLevelEnum.Extr)
                .Where(x => x.AOLevel != FiasLevelEnum.Sext)
                .Where(x => x.AOLevel != FiasLevelEnum.Street)
                .Where(x => ((x.OKATO != null && x.OKATO.Length > 0) || (x.OKTMO != null && x.OKTMO.Length > 0)))
                .ToList();

            actualFias = allFias.Where(x => x.ActStatus == FiasActualStatusEnum.Actual).GroupBy(x => x.AOGuid).ToDictionary(x => x.Key, arg => arg.First());

            IsFirstLoad = !allMo.Any(x => x.IsOld);

            foreach (var mo in allMo)
            {
                if (mo != null)
                {
                    var fiasId = mo.FiasId;

                    var fias =  !string.IsNullOrEmpty(fiasId) && actualFias.ContainsKey(fiasId) ? actualFias[fiasId] : null;

                    if (fias != null && !String.IsNullOrEmpty(fias.OKATO) && mo.Okato != fias.OKATO)
                    {
                        mo.Okato = fias.OKATO;
                        moService.Update(mo);
                    }
                }
            }

            using (file)
            {
                using (var sr = new StreamReader(file, Encoding.GetEncoding(1251)))
                {
                    var line = string.Empty;
                    while (!sr.EndOfStream && (line = sr.ReadLine()) != null)
                    {
                        try
                        {

                            if (line.IsEmpty())
                            {
                                continue;
                            }

                            var splits = line.Split(new[] { ";" }, StringSplitOptions.None);

                            if (splits.Length < SplitCount)
                            {
                                throw new ArgumentException(
                                    string.Format(
                                        "Invalid line detected. Expected part count: {2}. File: {0}, Line: {1}",
                                        fileName, string.Join("|", splits), SplitCount));
                            }


                            ProcessLine(fileName, splits);
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.Message + line);
                        }
                    }
                }
            }

            SaveRecords(_items);
        }

        protected virtual void SaveRecords(List<Municipality> records)
        {
            if (records != null)
            {

               TransactionHelper.InsertInManyTransactions(Container, records);

                records.Clear();
            }
        }

        internal AbstractEntityPersister GetMetaData(Type type, ISession session)
        {
            return session.SessionFactory.GetClassMetadata(type) as AbstractEntityPersister;
        }
        
        public override ImportResult Import(B4.BaseParams baseParams)
        {
            var sw = new Stopwatch();
            sw.Start();

            var file = baseParams.Files["FileImport"];

            LogImport.Info("Старт импорта ОКТМО", string.Format("Старт импорта ОКТМО: {0}", file.FileName));

            using (var archiveMemoryStream = new MemoryStream(file.Data))
            {
                try
                {
                   ImportInternal(archiveMemoryStream, file.FileName);
                   MakeTree();
                   FillEmptyFiasAddress();
                   LinkMkdToMo();
                  // GenerateAddress();
                }
                catch (Exception exc)
                {
                    LogImport.Error("Ошибка", exc.Message);
                }
            }

            //GenerateFiasAddress();

            LogImport.SetFileName(file.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Add(file, LogImport);
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;

            sw.Stop();

            return new ImportResult(statusImport, "");
        }

        public override bool Validate(B4.BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }
        #endregion


        /// <summary>
        /// Проставить связь родительское мо - дочернее мо
        /// </summary>
        public void MakeTree()
        {
            var allMos = GetAllMo();

            foreach (var mo in allMos)
            {
                var oktmo = mo.Oktmo.ToLong().ToString();

                if (!mo.Oktmo.ToLong().ToString().EndsWith("000") && oktmo != "0" && oktmo.Length > 5)
                {
                    var parentOktmo = String.Format("{0}000", oktmo.Substring(0, oktmo.Length - 3)).ToLong();

                    var parentMo = allMos.FirstOrDefault(x => x.Oktmo == parentOktmo.ToString());

                    if (parentMo != null)
                    {
                        mo.ParentMo = parentMo;

                        mo.FiasId = parentMo.FiasId;

                        moService.Update(mo);

                        LogImport.Info("Привязка к родительскому мо", string.Format("МО {0} привязано как дочернее к МО:{1} ", mo.Name, parentMo.Name));
                    }
                    else
                    {
                        LogImport.Warn("Ошибка привязки к родительскому мо", string.Format("Не удалось найти родительское МО для {0}", mo.Name));
                    }
                }
            }
        }


        public List<Municipality> GetAllMo()
        {
            var queryMo = Container.Resolve<IRepository<Municipality>>().GetAll();
            var interceptors = Container.ResolveAll<IDomainServiceReadInterceptor<Municipality>>();

            var allMos = interceptors.Aggregate(queryMo, (current, interceptor) => interceptor.BeforeGetAll(current))
                .Where(x => x.Okato != "92401000000" && x.Okato != "22401000000").ToList();

            return allMos;
        }

        /// Заполнить пустые фполя фиас адреса

        public void FillEmptyFiasAddress()
        {
            var result =  new List<FiasAddress>();

            var emtyFiasAddr = FiasAddressService.GetAll().Where(x => (x.PlaceGuidId == null || x.PlaceGuidId.Length == 0) && 
                                                                      (x.AddressGuid != null && x.AddressGuid.Length >0)).ToList();

            foreach (var fadr in emtyFiasAddr)
            {
                if (String.IsNullOrEmpty(fadr.AddressGuid)) continue;

                var guids = fadr.AddressGuid.Split("#").Select(x => new
                {
                    code = x.Split("_")[0],
                    aoguid = x.Split("_").Length > 1 ? x.Split("_")[1] : ""
                });

                foreach (var guid in guids)
                {
                    if (guid.aoguid.Length == 36)
                    {
                        switch (guid.code)
                        {
                            case "3": fadr.PlaceGuidId = guid.aoguid; break;
                            case "4": fadr.PlaceGuidId = guid.aoguid; break;
                            case "6": fadr.PlaceGuidId = guid.aoguid; break;
                            case "7": fadr.StreetGuidId = guid.aoguid; break;
                            case "90": fadr.StreetGuidId = guid.aoguid; break;
                            default:
                                break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(fadr.PlaceGuidId))
                {
                    fadr.PlaceName = actualFias.ContainsKey(fadr.PlaceGuidId) ? actualFias[fadr.PlaceGuidId].ReturnSafe(x => String.Format("{0}. {1}", x.ShortName, x.OffName)) : "";
                }

                if (!string.IsNullOrEmpty(fadr.StreetGuidId))
                {
                    fadr.StreetName = actualFias.ContainsKey(fadr.PlaceGuidId) ? actualFias[fadr.PlaceGuidId].ReturnSafe(x => String.Format("{0}. {1}", x.ShortName, x.OffName)) : "";
                }

                result.Add(fadr);
            }

            TransactionHelper.InsertInManyTransactions(Container, result);

        }

        /// <summary>
        /// Привязать дома к новым МО
        /// </summary>
        public void LinkMkdToMo()
        {
            var result = new Dictionary<long, Municipality>();

            var realityService = Container.Resolve<IDomainService<RealityObject>>();

            var allMkd = realityService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.FiasAddress,
                    x.FiasAddress.PlaceGuidId,
                    x.Address,
                    MoId = x.Municipality.Id,
                }).ToList();

            var allMos = GetAllMo().ToList();

            // Ручные корректировки привязки домов к МО
            var allFiasOktmo = FiasOktmoService.GetAll().ToList().ToDictionary(x => x.FiasGuid, arg => arg.Municipality);

            foreach (var mkd in allMkd)
            {
                Municipality mo = null;

                // ищем ручную привязку
                if (mkd.PlaceGuidId != null && allFiasOktmo.Any())
                {
                    mo = allFiasOktmo.ContainsKey(mkd.PlaceGuidId) ? allFiasOktmo[mkd.PlaceGuidId] : null;
                }

                // ищем привязку по ФИАС
                if (mo == null)
                {
                    if (mkd.PlaceGuidId != null)
                    {
                        var fias = actualFias.ContainsKey(mkd.PlaceGuidId) ? actualFias[mkd.PlaceGuidId] : null;

                        if (fias != null)
                        {
                            var oktmo = fias.OKTMO.ToLong().ToString();

                            mo = allMos.FirstOrDefault(x => x.Oktmo.ToStr() == oktmo);
                        }
                    }
                    else
                    {
                        LogImport.Warn("Некорректный код фиас адреса дома, невозможно определить привязку к мо", string.Format("Жилой дом {0} НЕ БЫЛ ПРИВЯЗАН к МО", mkd.Address));
                    }
                }

                if (mo != null)
                {
                    result.Add(mkd.Id, mo);
                    LogImport.Info("Привязка жилого дома", string.Format("Жилой дом {0} привязан к МО:{1} ", mkd.Address, mo.Name));
                }
                else
                {
                    LogImport.Warn("Ошибка привязки дома к МО", string.Format("Жилой дом {0} НЕ БЫЛ ПРИВЯЗАН к МО", mkd.Address));
                }

            }

            const string query = "UPDATE GKH_REALITY_OBJECT SET STL_MUNICIPALITY_ID = {0} WHERE ID = {1}";
            const string mo_query = "UPDATE GKH_REALITY_OBJECT SET MUNICIPALITY_ID = {0} WHERE ID = {1}";

            using (var con = new NpgsqlConnection(Container.Resolve<IConfigProvider>().GetConfig().ConnString))
            {
                con.Open();

                foreach (var obj in result)
                {
                    ExecuteCommand(String.Format(query, obj.Value.Id, obj.Key), con);

                    if (obj.Value.ParentMo != null)
                    {
                        ExecuteCommand(String.Format(mo_query, obj.Value.ParentMo.Id, obj.Key), con);
                    }
                }
            }
        }

        private static void ExecuteCommand(String query, NpgsqlConnection con)
        {
            if (query.Length == 0)
            {
                return;
            }

            using (var com = new NpgsqlCommand(query, con))
            {
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        com.ExecuteNonQuery();

                        tr.Commit();

                        Trace.Write(query);
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        public void GenerateFiasAddress()
        {
            var result = new Dictionary<long, string>();

            var allAdr = FiasAddressService.GetAll()
                .Join(FiasService.GetAll(),
                    x => x.PlaceGuidId,
                    y => y.AOGuid,
                    (x, y) => new
                    {
                        x.Id,
                        x.PlaceName,
                        x.StreetName,
                        x.House,
                        x.Housing,
                        y.OKTMO,
                        y.OKATO,
                        y.ActStatus
                    }).Where(x => x.ActStatus ==  FiasActualStatusEnum.Actual)
                    .ToList();


            var allMos = GetAllMo()
                .ToDictionary(x => x.Oktmo.ToString(), arg => arg);

            foreach (var adr in allAdr)
            {
                var mo = allMos.ContainsKey(adr.OKTMO) ? allMos[adr.OKTMO] : null;

                if(mo != null)
                {
                    var sb = new StringBuilder();

                    if (mo.ParentMo != null)
                    {
                        sb.Append(mo.ParentMo.Name);
                        sb.Append(", ");
                    }

                    if (mo.Level != TypeMunicipality.UrbanArea)
                    {
                        sb.Append(mo.Name);
                        sb.Append(", ");
                    }

                    sb.Append(String.Format("{0}, {1}, д. {2}", adr.PlaceName, adr.StreetName, adr.House));

                    if (!string.IsNullOrEmpty(adr.Housing))
                    {
                        sb.Append(" корп. ");
                        sb.Append(adr.Housing);
                    }

                    //var adrText = MakeFiasAddressText(adr, mo);
                    result.Add(adr.Id, sb.ToString());
                }
            }


            var stop = true;
        }

        private string MakeFiasAddressText(FiasAddress adr, Municipality mo)
        {
            var sb = new StringBuilder();

            if (mo.ParentMo != null)
            {
                sb.Append(mo.ParentMo.Name);
                sb.Append(", ");
            }

            if (mo.Level != TypeMunicipality.UrbanArea)
            {
                sb.Append(mo.Name);
                sb.Append(", ");
            }

            sb.Append(String.Format("{0}, {1}, д. {2}", adr.PlaceName, adr.StreetName, adr.House));

            if (!string.IsNullOrEmpty(adr.Housing))
            {
                sb.Append(" корп. ");
                sb.Append(adr.Housing);
            }

            return sb.ToString();
        }


        public void GenerateAddress()
        {
            var roService = Container.Resolve<IDomainService<RealityObject>>();

            var allRo = roService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Municipality,
                    x.FiasAddress
                })
                .ToList();

            var result = new Dictionary<long, string>();

            foreach (var ro in allRo)
            {
                var sb = new StringBuilder();

                //if (ro.Municipality != null && ro.Municipality.ParentMo != null)
                //{
                //    sb.Append(GetTypeName(ro.Municipality.ParentMo.Level));
                //    sb.Append(ro.Municipality.ParentMo.Name);
                //}

                //if (ro.Municipality != null)
                //{
                //    if (sb.Length > 0)
                //    {
                //        sb.Append(", ");
                //    }
                //    sb.Append(GetTypeName(ro.Municipality.Level));
                //    sb.Append(ro.Municipality.Name);
                //}

                //sb.Append(", ");
                sb.Append(ro.FiasAddress.PlaceName);

                sb.Append(", ");
                sb.Append(ro.FiasAddress.StreetName);

                sb.Append(", д. ");
                sb.Append(ro.FiasAddress.House);

                if (!string.IsNullOrEmpty(ro.FiasAddress.Housing))
                {
                    sb.Append(" корп. ");
                    sb.Append(ro.FiasAddress.Housing);
                }

                result.Add(ro.Id, sb.ToString());
            }

            const string query = "UPDATE gkh_reality_object SET address = '{0}' WHERE ID = {1}";

            using (var con = new NpgsqlConnection(Container.Resolve<IConfigProvider>().GetConfig().ConnString))
            {
                con.Open();

                foreach (var obj in result)
                {
                    ExecuteCommand(String.Format(query, obj.Value, obj.Key), con);
                }
            }
        }

    }
}
