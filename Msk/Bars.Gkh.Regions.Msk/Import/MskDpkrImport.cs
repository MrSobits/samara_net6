namespace Bars.Gkh.Regions.Msk.Import.CommonRealtyObjectImport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Domain;
    using Enums.Import;
    using Gkh.Import;
    using Entities;
    using Gkh.Import.Impl;
    using Utils;
    using GkhExcel;
    using Castle.Windsor;

    public class MskDpkrImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public virtual IWindsorContainer Container { get; set; }

        public virtual IDomainService<RealityObjectInfo> RealityObjectInfoDomain { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "MskDpkrImport"; }
        }

        public override string Name
        {
            get { return "Импорт ДПКР (Москва)"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xlsx"; }
        }

        public override string PermissionName
        {
            get { return "Import.MskDpkrImport.View"; }
        }

        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
            var isLiftInfo = baseParams.Params.GetAs<bool>("isLiftInfo");

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

                using (var memoryStreamFile = new MemoryStream(file.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    if (rows.Count < 4)
                    {
                        throw new Exception("Не корректный формат");
                    }

                    InitHeader(rows[3], rows[2]);

                    if (isLiftInfo)
                    {
                        return ImportLiftInfo(rows);
                    }

                    var existRealObjInfo = RealityObjectInfoDomain.GetAll()
                        .ToList()
                        .GroupBy(x => x.Uid)
                        .ToDictionary(x => x.Key, y => y.First());

                    var roInfoToSave = new List<RealityObjectInfo>();
                    var dpkrInfoToSave = new List<DpkrInfo>();
                    var realObjInfoToUpdate = new HashSet<long>();

                    for (var i = 4; i < rows.Count; i++)
                    {
                        var tempRow = rows[i];

                        var uid = GetValue(tempRow, "UID");

                        var realObjInfo = existRealObjInfo.Get(uid) ?? new RealityObjectInfo
                        {
                            Uid = uid
                        };

                        realObjInfo.Okrug = GetValue(tempRow, "OKRUG");
                        realObjInfo.Raion = GetValue(tempRow, "RAION");
                        realObjInfo.Address = GetValue(tempRow, "ADDRESS");
                        realObjInfo.UnomCode = GetValue(tempRow, "UNOM");
                        realObjInfo.MziCode = GetValue(tempRow, "MZICODE");
                        realObjInfo.Serial = GetValue(tempRow, "SERIES");
                        realObjInfo.BuildingYear = GetValue(tempRow, "YEARBUILDING").ToInt();
                        realObjInfo.TotalArea = GetValue(tempRow, "TOTALAREA").ToDecimal();
                        realObjInfo.LivingArea = GetValue(tempRow, "LIVINGAREA").ToDecimal();
                        realObjInfo.NoLivingArea = GetValue(tempRow, "NONLIVINGAREA").ToDecimal();
                        realObjInfo.FloorCount = GetValue(tempRow, "FLOOR").ToInt();
                        realObjInfo.PorchCount = GetValue(tempRow, "PORCH").ToInt();
                        realObjInfo.FlatCount = GetValue(tempRow, "FLAT").ToInt();
                        realObjInfo.AllDelay = GetValue(tempRow, "ALLDELAY").ToDecimal();
                        realObjInfo.Points = GetValue(tempRow, "POINT").ToDecimal();
                        realObjInfo.IndexNumber = GetValue(tempRow, "NUMBER").ToInt();

                        if (realObjInfo.Id != 0)
                        {
                            realObjInfoToUpdate.Add(realObjInfo.Id);
                        }

                        foreach (var ceoType in (CeoType[])Enum.GetValues(typeof(CeoType)))
                        {
                            var ceoTypeInt = (int)ceoType;
                            var period = GetValue(tempRow, "{0}_PERIOD".FormatUsing(ceoTypeInt));

                            var dpkrInfo = new DpkrInfo
                            {
                                RealityObjectInfo = realObjInfo,
                                CeoType = ceoType,
                                CeoState = GetValue(tempRow, "{0}_STATUS".FormatUsing(ceoTypeInt)).To<CeoState>(),
                                Delay = GetValue(tempRow, "{0}_DELAY".FormatUsing(ceoTypeInt)).ToDecimal(),
                                LifeTime = GetValue(tempRow, "{0}_LIFETIME".FormatUsing(ceoTypeInt)).ToInt(),
                                LastRepairYear = GetValue(tempRow, "{0}_LASTYEAR".FormatUsing(ceoTypeInt)).ToInt(),
                                Period = period
                            };

                            SetPeriod(realObjInfo, ceoType, period);

                            dpkrInfoToSave.Add(dpkrInfo);
                        }

                        roInfoToSave.Add(realObjInfo);
                    }

                    var sessions = Container.Resolve<ISessionProvider>();

                    if (realObjInfoToUpdate.Count > 0)
                    {
                        using (Container.Using(sessions))
                        {
                            var session = sessions.OpenStatelessSession();

                            for (var i = 0; i < realObjInfoToUpdate.Count; i += 1000)
                            {
                                var ids = realObjInfoToUpdate.Skip(i).Take(1000).ToList();

                                session.CreateQuery(" delete from DpkrInfo" +
                                                    " where RealityObjectInfo in (:ids)")
                                    .SetParameterList("ids", ids)
                                    .ExecuteUpdate();
                            }
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(Container, roInfoToSave, 10000, true, true);
                    TransactionHelper.InsertInManyTransactions(Container, dpkrInfoToSave, 10000, true, true);
                }
            }

            return new ImportResult(StatusImport.CompletedWithoutError, string.Empty);
        }

        private ImportResult ImportLiftInfo(List<GkhExcelCell[]> rows)
        {
            var existRealObjInfo = RealityObjectInfoDomain.GetAll()
                        .ToList()
                        .GroupBy(x => x.Uid)
                        .ToDictionary(x => x.Key, y => y.First());

            var roInfoToUpdate = new List<RealityObjectInfo>();
            var liftInfoToSave = new List<LiftInfo>();
            var unknownUids = new HashSet<string>();
            var realObjInfoLiftPeriod = new Dictionary<RealityObjectInfo, List<string>>();

            for (var i = 4; i < rows.Count; i++)
            {
                var tempRow = rows[i];

                var uid = GetValue(tempRow, "UID");

                var roInfo = existRealObjInfo.Get(uid);

                if (roInfo == null)
                {
                    unknownUids.Add(uid);
                    continue;
                }


                var liftInfo = new LiftInfo()
                {
                    RealityObjectInfo = roInfo,
                    LifeTime = GetValue(tempRow, "LIFTLIFETIME"),
                    Period = GetValue(tempRow, "LIFTPERIOD"),
                    Capacity = GetValue(tempRow, "CAPACITY").ToInt(),
                    Porch = GetValue(tempRow, "PORCH"),
                    StopCount = GetValue(tempRow, "STOP").ToInt(),
                    InstallationYear = GetValue(tempRow, "YEARINSTALLATION").ToInt()
                };

                if (!liftInfo.LifeTime.IsEmpty())
                {
                    liftInfo.LifeTime = liftInfo.LifeTime.ToDateTime().ToShortDateString();
                }

                if (realObjInfoLiftPeriod.ContainsKey(roInfo))
                {
                    realObjInfoLiftPeriod.Get(roInfo).Add(liftInfo.Period);
                }
                else
                {
                    realObjInfoLiftPeriod.Add(roInfo, new List<string> { liftInfo.Period });
                }

                liftInfoToSave.Add(liftInfo);
            }


            foreach (var roLiftPeriod in realObjInfoLiftPeriod)
            {
                var roInfo = roLiftPeriod.Key;

                roInfo.LiftPeriod = roLiftPeriod.Value.Distinct().OrderBy(x => x).AggregateWithSeparator(", ");

                roInfoToUpdate.Add(roInfo);
            }

            var sessions = Container.Resolve<ISessionProvider>();

            if (roInfoToUpdate.Count > 0)
            {
                using (Container.Using(sessions))
                {
                    var session = sessions.OpenStatelessSession();

                    var allIds = roInfoToUpdate.Select(x => x.Id).ToList();

                    for (var i = 0; i < allIds.Count; i += 1000)
                    {
                        var ids = allIds.Skip(i).Take(1000).ToList();

                        session.CreateQuery(" delete from LiftInfo" +
                                            " where RealityObjectInfo in (:ids)")
                            .SetParameterList("ids", ids)
                            .ExecuteUpdate();
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(Container, roInfoToUpdate, 10000, true, true);
            TransactionHelper.InsertInManyTransactions(Container, liftInfoToSave, 10000, true, true);

            var status = unknownUids.Count > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError;
            var message = unknownUids.Count > 0
                ? "Не определенные uid: {0}".FormatUsing(unknownUids.Distinct().AggregateWithSeparator(", "))
                : string.Empty;

            return new ImportResult(status, message);
        }

        private void SetPeriod(RealityObjectInfo roInfo, CeoType type, string period)
        {
            switch (type)
            {
                    case CeoType.Electro: roInfo.EsPeriod = period; break;
                    case CeoType.Gas: roInfo.GasPeriod = period; break;
                    case CeoType.Hvs: roInfo.HvsPeriod = period; break;
                    case CeoType.Hvs_M: roInfo.HvsmPeriod = period; break;
                    case CeoType.Gvs: roInfo.GvsPeriod = period; break;
                    case CeoType.Gvs_M: roInfo.GvsmPeriod = period; break;
                    case CeoType.Kan: roInfo.KanPeriod = period; break;
                    case CeoType.Kan_M: roInfo.KanmPeriod = period; break;
                    case CeoType.Otoplenie: roInfo.OtopPeriod = period; break;
                    case CeoType.Otoplenie_M: roInfo.OtopmPeriod = period; break;
                    case CeoType.Mus: roInfo.MusPeriod = period; break;
                    case CeoType.Ppiadu: roInfo.PpiaduPeriod = period; break;
                    case CeoType.Pv: roInfo.PvPeriod = period; break;
                    case CeoType.Fasad: roInfo.FasPeriod = period; break;
                    case CeoType.Krov: roInfo.KrovPeriod = period; break;
                    case CeoType.Vdsk: roInfo.VdskPeriod = period; break;
            }
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

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }


        private void InitHeader(GkhExcelCell[] row, GkhExcelCell[] prevRow)
        {
            headersDict["UID"] = -1;
            headersDict["OKRUG"] = -1;
            headersDict["RAION"] = -1;
            headersDict["ADDRESS"] = -1;
            headersDict["UNOM"] = -1;
            headersDict["MZICODE"] = -1;
            headersDict["SERIES"] = -1;
            headersDict["YEARBUILDING"] = -1;
            headersDict["TOTALAREA"] = -1;
            headersDict["LIVINGAREA"] = -1;
            headersDict["NONLIVINGAREA"] = -1;
            headersDict["FLOOR"] = -1;
            headersDict["PORCH"] = -1;
            headersDict["FLAT"] = -1;
            headersDict["ALLDELAY"] = -1;
            headersDict["POINT"] = -1;
            headersDict["NUMBER"] = -1;

            headersDict["CAPACITY"] = -1;
            headersDict["STOP"] = -1;
            headersDict["YEARINSTALLATION"] = -1;
            headersDict["LIFTLIFETIME"] = -1;
            headersDict["LIFTPERIOD"] = -1;

            var ceoTypeDict = new Dictionary<string, int>();

            foreach (var ceoType in  (CeoType[]) Enum.GetValues(typeof(CeoType)))
            {
                ceoTypeDict.Add(ceoType.GetEnumMeta().Description.ToUpper(), (int)ceoType);
            }

            for (var index = 0; index < row.Length; ++index)
            {
                var header = row[index].Value.ToUpper();
                var prevRowHeader = prevRow[index].Value.ToUpper();
                if (headersDict.ContainsKey(header))
                {
                    headersDict[header] = index;
                }
                else if (!prevRowHeader.IsEmpty() && ceoTypeDict.ContainsKey(prevRowHeader))
                {
                    headersDict.Add("{0}_{1}".FormatUsing(ceoTypeDict.Get(prevRowHeader), header), index);
                }
            }
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (headersDict.ContainsKey(field))
            {
                var index = headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result.Trim();
        }
    }
}