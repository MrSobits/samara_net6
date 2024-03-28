namespace Bars.Gkh.Regions.Msk.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Castle.Windsor;
    using Config;
    using Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Nso.Entities;
    using PassportProvider;

    public class TypeWorkReport : BasePrintForm
    {
        private int _startYear;
        private int _endYear;

        #region DomainServices

        public IWindsorContainer Container { get; set; }
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }
        public IDomainService<CommonEstateObject> CommonEstateObjectDomain { get; set; }
        public IRepository<RealityObject> RealityObjectRepo { get; set; }
        public IDomainService<TehPassportValue> PassportValueDomain { get; set; }
        public IGkhParams GkhParams { get; set; }

        #endregion

        #region Properties && .ctor

        public TypeWorkReport()
            : base(new ReportTemplateBinary(Properties.Resources.TypeWorkReport))
        {
        }

        public override string Name { get { return "Отчет по видам работ"; } }

        public override string Desciption { get { return "Отчет по видам работ"; } }

        public override string GroupName { get { return "Капитальный ремонт"; } }

        public override string ParamsController { get { return "B4.controller.report.TypeWorkReport"; } }

        public override string RequiredPermission { get { return "Reports.Msk.TypeWorkReport"; } }

        public override string ReportGenerator { get; set; }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            _startYear = baseParams.Params.GetAs<int>("startYear");
            _endYear = baseParams.Params.GetAs<int>("endYear");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (_startYear > _endYear)
            {
                throw new Exception("Год начала не может быть больше года окончания");
            }

            var versionRecordList = VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion.IsMain)
                .Where(x => x.Year >= _startYear && x.Year <= _endYear)
                .Select(x => new
                {
                    x.CommonEstateObjects,
                    x.Sum,
                    x.Year,

                    RoId = x.RealityObject.Id,
                    x.RealityObject.District,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address
                })
                .ToList();

            var records = versionRecordList
                .GroupBy(x => x.RoId)
                .Select(x => x.First())
                .ToList();

            #region Сбор данных по ООИ

            var ceoList = CommonEstateObjectDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToList();

            reportParams.SimpleReportParams["startYear"] = _startYear;
            reportParams.SimpleReportParams["endYear"] = _endYear;

            var ooiInfoList = versionRecordList
                .Select(x => new
                {
                    x.RoId,
                    CeoId = ceoList.FirstOrDefault(y => y.Name == x.CommonEstateObjects).Return(z => z.Id),
                    x.Sum,
                    x.Year
                })
                .ToList();

            var ooiInfoByCeoIdSumDict = ooiInfoList
                .GroupBy(x => x.CeoId)
                .ToDictionary(x => x.Key, x => x.SafeSum(y => y.Sum));

            var ooiInfoByRoIdSumDict = ooiInfoList
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.SafeSum(y => y.Sum));

            var ooiInfoByRoDict = ooiInfoList
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.Select(y => new
                {
                    y.CeoId,
                    y.Sum,
                    y.Year
                })
                .GroupBy(y => y.CeoId)
                .ToDictionary(y => y.Key, y => y.First()));

            var oois = versionRecordList
                .Select(x => new
                {
                    x.CommonEstateObjects,
                    CeoId = ceoList.FirstOrDefault(y => y.Name == x.CommonEstateObjects).Return(z => z.Id)
                })
                .GroupBy(x => x.CommonEstateObjects)
                .Select(x => x.First())
                .ToList();

            if (oois.Any())
            {
                var ooiSection = reportParams.ComplexReportParams.ДобавитьСекцию("ooiSection");
                foreach (var ooi in oois)
                {
                    ooiSection.ДобавитьСтроку();
                    ooiSection["ooi"] = ooi.CommonEstateObjects;
                    ooiSection["ooiSum"] = string.Format("$ooiSum{0}$", ooi.CeoId);
                    ooiSection["ooiYear"] = string.Format("$ooiYear{0}$", ooi.CeoId);
                    ooiSection["totalByOoi"] = ooiInfoByCeoIdSumDict[ooi.CeoId];
                }
            }

            #endregion

            #region Сбор данных по лифтам

            var passport = Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");
            if (passport == null)
            {
                throw new Exception("Не найден провайдер технического паспорта");
            }

            #endregion

            var tpQuery = PassportValueDomain.GetAll()
                .Where(x => x.FormCode == "Form_4_2_1")
                .Where(x => x.CellCode.Contains(":15") || x.CellCode.Contains(":18"))
                .Select(x => new
                {
                    RoId = x.TehPassport.RealityObject.Id,
                    Municipality = x.TehPassport.RealityObject.Municipality.Name,
                    x.TehPassport.RealityObject.Address,
                    x.TehPassport.RealityObject.District,
                    x.CellCode,
                    x.Value
                });

            var tehPassportRobjects = PassportValueDomain.GetAll()
                .Where(x => x.FormCode == "Form_4_2_1")
                .Where(x => x.CellCode.Contains(":18"))
                .Select(x => new
                {
                    RoId = x.TehPassport.RealityObject.Id,
                    Municipality = x.TehPassport.RealityObject.Municipality.Name,
                    x.TehPassport.RealityObject.Address,
                    x.TehPassport.RealityObject.District,
                    x.Value
                })
                .ToList()
                .Where(x =>
                {
                    if (x.Value == null) return false;

                    var splited = x.Value.Split('-');

                    if (splited.Length != 2) return false;

                    var val = splited[1].ToInt();

                    if (val >= _startYear && val <= _endYear) return true;

                    return false;
                })
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, z => z.First());

            var tehPassportValuesDictByRo = tpQuery
                .ToList()
                .Select(x =>
                {
                    var array = x.CellCode.Split(':');
                    return new
                    {
                        x.RoId,
                        Num = array[0].Trim(),
                        Cell = array[1].Trim(),
                        x.Value
                    };
                })
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Num)
                    .ToDictionary(y => y.Key, y => new
                    {
                        Sum = y.Where(z => z.Cell == "15")
                            .Select(z => z.Value.ToDecimal())
                            .FirstOrDefault(),
                        Year = y.Where(z => z.Cell == "18")
                            .Where(z => z.Value.Split('-').Length == 2)
                            .Select(z => z.Value.Split('-')[1].ToInt())
                            .FirstOrDefault(z => z != 0)
                    }));

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 0;
            var totalByElevator = 0M;
            var grandTotal = 0M;

            var data = records
                .Select(x => new
                {
                    x.RoId,
                    x.District,
                    x.Municipality,
                    x.Address
                })
                .Union(tehPassportRobjects.Values
                    .Select(x => new
                    {
                        x.RoId,
                        x.District,
                        x.Municipality,
                        x.Address
                    }))
                .Distinct(x => x.RoId)
                .OrderBy(x => x.District)
                .ThenBy(x => x.Municipality)
                .ThenBy(x => x.Address)
                .ToArray();

            foreach (var record in data)
            {
                var totalByRo = 0M;

                section.ДобавитьСтроку();
                section["num"] = ++num;
                section["district"] = record.District;
                section["municipality"] = record.Municipality;
                section["address"] = record.Address;

                if (ooiInfoByRoDict.ContainsKey(record.RoId))
                {
                    foreach (var roCeoId in ooiInfoByRoDict[record.RoId].Keys)
                    {
                        section[string.Format("ooiSum{0}", roCeoId)] = ooiInfoByRoDict[record.RoId][roCeoId].Sum;
                        section[string.Format("ooiYear{0}", roCeoId)] = ooiInfoByRoDict[record.RoId][roCeoId].Year;

                        totalByRo += ooiInfoByRoDict[record.RoId][roCeoId].Sum;
                    }
                }

                if (tehPassportValuesDictByRo.ContainsKey(record.RoId))
                {
                    var sum = tehPassportValuesDictByRo[record.RoId]
                        .Where(x => x.Value.Year >= _startYear && x.Value.Year <= _endYear)
                        .SafeSum(x => x.Value.Sum);

                    section["elevatorSum"] = sum != 0 ? (decimal?)sum : null;

                    var year = tehPassportValuesDictByRo[record.RoId]
                        .Where(x => x.Value.Year >= _startYear && x.Value.Year <= _endYear)
                        .OrderByDescending(x => x.Value.Year)
                        .Select(x => x.Value.Year)
                        .FirstOrDefault();

                    section["elevatorYear"] = year != 0 ? (int?)year : null;
                }

                totalByElevator += section["elevatorSum"].ToDecimal();

                totalByRo += section["elevatorSum"].ToDecimal();

                section["totalByRo"] = totalByRo;

                grandTotal += totalByRo;
            }

            reportParams.SimpleReportParams["totalByElevator"] = totalByElevator;

            reportParams.SimpleReportParams["grandTotal"] = grandTotal;
        }
    }
}