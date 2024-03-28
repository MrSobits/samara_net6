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
    using Castle.Windsor;
    using Domain;
    using Entities;
    using Enums.Import;
    using Gkh.Import;
    using Gkh.Import.Impl;
    using GkhExcel;
    using NHibernate.Linq;

    public class MskCeoPointImport : GkhImportBase
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
            get { return "MskCeoPointImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт статусов ООИ ДПКР (Москва)"; }
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
            get { return "Import.MskDpkrImport.View"; }
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
            var existsData = GetExistsData();

            var dpkrInfoToUpdate = new List<DpkrInfo>();

            foreach (var row in data)
            {
                var ro = existsData.Get(row.Uid);

                if(ro == null)
                    continue;

                foreach (var points in row.CeoPoints)
                {
                    if (!ro.ContainsKey(points.Key))
                        continue;

                    foreach (var dpkrInfo in ro[points.Key])
                    {
                        dpkrInfo.Point = points.Value;

                        dpkrInfoToUpdate.Add(dpkrInfo);
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(Container, dpkrInfoToUpdate, 10000, true, true);
        }

        private Dictionary<string, Dictionary<CeoType, DpkrInfo[]>> GetExistsData()
        {
            var ceoInfoDomain = Container.ResolveDomain<DpkrInfo>();

            try
            {
                return ceoInfoDomain.GetAll()
                    .Fetch(x => x.RealityObjectInfo)
                    .ToArray()
                    .GroupBy(x => x.RealityObjectInfo.Uid)
                    .ToDictionary(x => x.Key, z => z.GroupBy(x => x.CeoType).ToDictionary(x => x.Key, a => a.ToArray()));
            }
            finally
            {
                Container.Release(ceoInfoDomain);
            }
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

                var points = new List<Row>();
                using (var memoryStreamFile = new MemoryStream(file.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    if (rows.Count < 3)
                    {
                        throw new Exception("Некорректный формат");
                    }

                    InitHeaders(rows[1]);

                    for (var i = 3; i < rows.Count; i++)
                    {
                        var tempRow = rows[i];

                        var row = Extract(tempRow);

                        if (row == null)
                            continue;

                        points.Add(row);
                    }
                }

                return points;
            }
        }

        private readonly Dictionary<int, int> _headersDict = new Dictionary<int, int>();

        private void InitHeaders(GkhExcelCell[] row)
        {
            var ceoTypeDict = new Dictionary<string, int>();

            foreach (var ceoType in (CeoType[])Enum.GetValues(typeof(CeoType)))
            {
                ceoTypeDict.Add(ceoType.GetEnumMeta().Description.ToUpper(), (int)ceoType);
            }

            for (var index = 0; index < row.Length; ++index)
            {
                var header = row[index].Value.ToUpper().Trim();

                 if (ceoTypeDict.ContainsKey(header))
                {
                    _headersDict.Add(ceoTypeDict.Get(header), index);
                }
            }
        }

        private Row Extract(GkhExcelCell[] row)
        {
            var uid = row[0].Value.Trim();

            if (uid.IsEmpty())
            {
                return null;
            }

            var result = new Row(uid);

            foreach (var ceoType in (CeoType[])Enum.GetValues(typeof(CeoType)))
            {
                var ceoTypeInt = (int)ceoType;
                var point = GetValue(row, ceoTypeInt).ToInt();

                if (point > 0)
                {
                    result.CeoPoints.Add(ceoType, point);
                }

            }

            return result;
        }


        private string GetValue(GkhExcelCell[] data, int field)
        {
            var result = string.Empty;

            if (_headersDict.ContainsKey(field))
            {
                var index = _headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result.Trim();
        }

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            return true;
        }

        private class Row
        {
            public Row(string uid)
            {
                Uid = uid;
                CeoPoints = new Dictionary<CeoType, int>();
            }

            public string Uid { get; private set; }

            public Dictionary<CeoType, int> CeoPoints { get; private set; }
        }
    }
}
