namespace Bars.Gkh.Regions.Msk.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.Import.Impl;
    using Castle.Windsor;
    using Domain;
    using Domain.CollectionExtensions;
    using Entities;
    using Enums.Import;
    using Gkh.Import;
    using GkhExcel;

    /// <summary>
    /// Импорт состояний для сервиса
    /// </summary>
    public class MskCeoStateForServiceImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get { return Id; }
        }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport
        {
            get { return "MskCeoStateForServiceImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        { 
            get { return "Импорт состояний ООИ для сервиса (Москва)"; }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "xlsx"; }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.MskCeoStateServicePanel.View"; }
        }

        public virtual IWindsorContainer Container { get; set; }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var data = Extract(baseParams.Files["FileImport"]);

            ProcessData(data);

            return new ImportResult(StatusImport.CompletedWithoutError, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessData(List<Row> data)
        {
            var ceoInfoDomain = Container.ResolveDomain<DpkrInfo>();

            var uids = data.Select(x => x.Uid).ToArray();

            var existsData = GetExistsData(uids);

            var dpkrInfoToUpdate = new List<DpkrInfo>();

            foreach (var row in data)
            {
                var ro = existsData.Get(row.Uid);

                if(ro == null)
                    continue;

                foreach (var states in row.CeoStates)
                {
                    if (!ro.ContainsKey(states.Key))
                        continue;

                    foreach (var dpkrInfo in ro[states.Key])
                    {
                        dpkrInfo.CeoStates = states.Value;

                        dpkrInfoToUpdate.Add(dpkrInfo);
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(Container, dpkrInfoToUpdate, 1000, true, true);
        }

        private Dictionary<string, Dictionary<CeoType, DpkrInfo[]>> GetExistsData(string[] uids)
        {
            var ceoInfoDomain = Container.ResolveDomain<DpkrInfo>();
            var result = new Dictionary<string, Dictionary<CeoType, DpkrInfo[]>>();

            foreach (var section in uids.Section(1000))
            {
                var portion = ceoInfoDomain.GetAll()
                    .Where(x => section.Contains(x.RealityObjectInfo.Uid))
                    .GroupBy(x => x.RealityObjectInfo.Uid)
                    .ToDictionary(x => x.Key, z => z.GroupBy(x => x.CeoType).ToDictionary(x => x.Key, a => a.ToArray()));

                foreach (var item in portion)
                {
                    if (!result.ContainsKey(item.Key))
                        result[item.Key] = new Dictionary<CeoType, DpkrInfo[]>();

                    foreach (var type in item.Value)
                    {
                        result[item.Key][type.Key] = type.Value;
                    }
                }
            }

            return result;
        }

        private List<Row> Extract(FileData file)
        {
            using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                if (file.Extention == "xlsx")
                {
                    excel.UseVersionXlsx();
                }

                var ceoStates = new List<Row>();

                using (var memoryStreamFile = new MemoryStream(file.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    if (rows.Count < 3)
                    {
                        throw new Exception("Некорректный формат");
                    }

                    InitHeaders(rows[2]);

                    for (var i = 3; i < rows.Count; i++)
                    {
                        var tempRow = rows[i];

                        var row = Extract(tempRow);

                        if (row == null)
                            continue;

                        ceoStates.Add(row);
                    }
                }

                return ceoStates;
            }
        }

        private readonly Dictionary<string, int> _headersDict = new Dictionary<string, int>();

        private void InitHeaders(GkhExcelCell[] cells)
        {
            _headersDict["UID"] = -1;
            _headersDict["MKDOG1O01 MZI_ES_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O02 MZI_CO_cherdak_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O02 MZI_CO_etaj_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O02 MZI_CO_tech_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O03 MZI_GS_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O04 MZI_HVS_etaj_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O04 MZI_HVS_pojar_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O04 MZI_HVS_tech_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O05 MZI_GVS_cherdak_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O05 MZI_GVS_etaj_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O05 MZI_GVS_tech_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O06 MZI_Kanal_tech_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O06 MZI_Kanal_etaj_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O07 MZI_Mys_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O09 MZI_SB_PPADY_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG3O01 MZI_Fasad_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG3O01 MZI_Fasad_styki_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG3O02 MZI_Kon_el_Balk_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG3O04 MZI_Kon_el_Vodootvod_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG4O06 MZI_Podval_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG5O01 MZI_Krov_Ocenka_krov".ToUpper()] = -1;

            for (int i = 0; i < cells.Length; i++)
            {
                var key = cells[i].Value.ToStr().ToUpper();

                if (_headersDict.ContainsKey(key))
                {
                    _headersDict[key] = i;
                }
            }
        }
        private Row Extract(GkhExcelCell[] row)
        {
            var uid = GetValue(row, "UID");

            if (uid.IsEmpty())
            {
                return null;
            }

            var result = new Row(uid);

            AddSingleStates(result, row);
            AddOtoplenieM(result, row);
            AddGvsM(result, row);
            AddFasad(result, row);

            return result;
        }

        private void AddSingleStates(Row row, GkhExcelCell[] cells)
        {
            row.CeoStates[CeoType.Electro] = new List<string> {GetValue(cells, "MKDOG1O01 MZI_ES_Ocenka")};
            row.CeoStates[CeoType.Otoplenie] = new List<string> {GetValue(cells, "MKDOG1O01 MZI_ES_Ocenka")};
            row.CeoStates[CeoType.Gas] = new List<string> {GetValue(cells, "MKDOG1O03 MZI_GS_Ocenka")};
            row.CeoStates[CeoType.Pv] = new List<string> {GetValue(cells, "MKDOG1O04 MZI_HVS_pojar_Ocenka")};
            row.CeoStates[CeoType.Hvs_M] = new List<string> {GetValue(cells, "MKDOG1O04 MZI_HVS_tech_Ocenka")};
            row.CeoStates[CeoType.Gvs] = new List<string> {GetValue(cells, "MKDOG1O05 MZI_GVS_etaj_Ocenka")};
            row.CeoStates[CeoType.Kan_M] = new List<string> {GetValue(cells, "MKDOG1O06 MZI_Kanal_tech_Ocenka")};
            row.CeoStates[CeoType.Kan] = new List<string> {GetValue(cells, "MKDOG1O06 MZI_Kanal_etaj_Ocenka")};
            row.CeoStates[CeoType.Mus] = new List<string> {GetValue(cells, "MKDOG1O07 MZI_Mys_Ocenka")};
            row.CeoStates[CeoType.Ppiadu] = new List<string> {GetValue(cells, "MKDOG1O09 MZI_SB_PPADY_Ocenka")};
            row.CeoStates[CeoType.Vdsk] = new List<string> {GetValue(cells, "MKDOG3O04 MZI_Kon_el_Vodootvod_Ocenka")};
            row.CeoStates[CeoType.Podval] = new List<string> {GetValue(cells, "MKDOG4O06 MZI_Podval_Ocenka")};
            row.CeoStates[CeoType.Krov] = new List<string> {GetValue(cells, "MKDOG5O01 MZI_Krov_Ocenka_krov")};
        }

        private void AddOtoplenieM(Row row, GkhExcelCell[] cells)
        {
            var cherdak = GetValue(cells, "MKDOG1O02 MZI_CO_cherdak_Ocenka");
            var podval = GetValue(cells, "MKDOG1O02 MZI_CO_tech_Ocenka");

            row.CeoStates[CeoType.Otoplenie_M] = new List<string>
            {
                !cherdak.IsEmpty() ? "чердак - " + cherdak : null,
                !podval.IsEmpty() ? "подвал - " + podval : null
            };
        }

        private void AddGvsM(Row row, GkhExcelCell[] cells)
        {
            var cherdak = GetValue(cells, "MKDOG1O05 MZI_GVS_cherdak_Ocenka");
            var podval = GetValue(cells, "MKDOG1O05 MZI_GVS_tech_Ocenka");

            row.CeoStates[CeoType.Gvs_M] = new List<string>
            {
                !cherdak.IsEmpty() ? "чердак - " + cherdak : null,
                !podval.IsEmpty() ? "подвал - " + podval : null
            };
        }

        private void AddFasad(Row row, GkhExcelCell[] cells)
        {
            var fasad = GetValue(cells, "MKDOG3O01 MZI_Fasad_Ocenka");
            var styki = GetValue(cells, "MKDOG3O01 MZI_Fasad_styki_Ocenka");
            var balkon = GetValue(cells, "MKDOG3O02 MZI_Kon_el_Balk_Ocenka");

            row.CeoStates[CeoType.Fasad] = new List<string>
            {
                !fasad.IsEmpty() ? "фасад - " + fasad : null,
                !styki.IsEmpty() ? "стыки - " + styki : null,
                !balkon.IsEmpty() ? "балконы - " + balkon : null
            };
        }

        private string GetValue(GkhExcelCell[] row, string key)
        {
            key = key.ToUpper();

            if (!_headersDict.ContainsKey(key))
                return "";

            var index = _headersDict[key];

            if (index < 0 && index >= row.Length)
                return "";

            return row[index].Value.ToStr();
        }

        private class Row
        {
            public Row(string uid)
            {
                Uid = uid;
                CeoStates = new Dictionary<CeoType, List<string>>();
            }

            public string Uid { get; private set; }

            public Dictionary<CeoType, List<string>> CeoStates { get; private set; }
        }
    }
}
