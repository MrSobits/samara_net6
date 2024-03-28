namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Pivot;
    using Bars.B4.Modules.Pivot.Enum;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Информация о загруженных дефектных ведомостях
    /// </summary>
    public class DefectListReport : IPrintForm, IPivotModel
    {
        // идентификатор программы КР
        private long programCrId;
        private long[] municipalityIds;

        public IWindsorContainer Container { get; set; }

        public string RequiredPermission
        {
            get { return "Reports.CR.DefectListReport"; }
        }

        public IList<string> ReportFormats { get; private set; }

        public string Desciption
        {
            get { return "Информация о загруженных дефектных ведомостях"; }
        }

        public string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public string ParamsController
        {
            get { return "B4.controller.report.DefectList"; }
        }

        public string Name
        {
            get { return "Информация о загруженных дефектных ведомостях"; }
        }

        public string Params { get; set; }

        public object Data { get; set; }

        public void LoadData()
        {
            var dict = DynamicDictionary.FromString(Params);
            programCrId = dict["programCrId"].ToInt();

            var municipalityIdsList = dict.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split("%2c+").Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            var works = Container.Resolve<IDomainService<DefectList>>().GetAll()
                         .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.Work != null)
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                         .Select(
                             x =>
                             new
                                 {
                                     WorkId = x.Work.Id,
                                     WorkName = x.Work.Name,
                                     ObjectCrId = x.ObjectCr.Id,
                                     state = x.State.Name,
                                     x.State.StartState,
                                 })
                         .AsEnumerable()
                         .GroupBy(x => x.ObjectCrId)
                         .ToDictionary(x => x.Key, x => x.ToList());


            var dictCountDefectWorkKind = Container.Resolve<IDomainService<DefectList>>().GetAll()
                         .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.Work != null)
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                         .Select(x => new { WorkId = x.Work.Id, ObjectCrId = x.ObjectCr.Id })
                         .AsEnumerable()
                         .GroupBy(x => x.ObjectCrId)
                         .ToDictionary(x => x.Key, x => x.Select(y => y.WorkId).Distinct().Count());

             var dictCountWorkKind = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                         .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.Work != null && x.Work.TypeWork == TypeWork.Work && x.Work.Code != "30")
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                         .Select(x => new { WorkId = x.Work.Id, ObjectCrId = x.ObjectCr.Id })
                         .AsEnumerable()
                         .GroupBy(x => x.ObjectCrId)
                         .ToDictionary(x => x.Key, x => x.Select(y => y.WorkId).Distinct().Count());

            var defectListData = Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                         .Where(x => x.ProgramCr.Id == programCrId)
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .Select(
                             x =>
                             new
                                 {
                                     objectCrId = x.Id,
                                     x.RealityObject.Address,
                                     CountDefectWorkKind = dictCountDefectWorkKind.ContainsKey(x.Id) ? dictCountDefectWorkKind[x.Id] : 0,
                                     CountWorkKind = dictCountWorkKind.ContainsKey(x.Id) ? dictCountWorkKind[x.Id] : 0,
                                     municipalityId = x.RealityObject.Municipality.Id,
                                     municipalityName = x.RealityObject.Municipality.Name
                                 })
                         .OrderBy(x => x.municipalityName)
                         .ThenBy(x => x.Address)
                         .ToList();

            var dateForMunicipality = defectListData.GroupBy(x => x.municipalityId)
                                                    .ToDictionary(
                                                        x => x.Key,
                                                        x =>
                                                        string.Format(
                                                            "{0} (Н = {1}; К = {2})",
                                                            x.Select(y => y.municipalityName).First(),
                                                            x.Select(y => y.CountWorkKind).Sum(),
                                                            x.Select(y => y.CountDefectWorkKind).Sum()));

            var table = GetTable();

            foreach (var rec in defectListData)
            {
                if (works.ContainsKey(rec.objectCrId))
                {
                    foreach (var work in works[rec.objectCrId])
                    {
                        var row = table.NewRow();
                        row["id"] = rec.objectCrId;
                        row["MUName"] = dateForMunicipality[rec.municipalityId];
                        row["Address"] = rec.Address;

                        row["AmountTypesWork"] = rec.CountDefectWorkKind;
                        row["NormalAmountTypesWork"] = rec.CountWorkKind;
                        row["Status"] = work.state;

                        row["TypeWork"] = work.WorkName;
                        row["AmountRepairList"] = work.StartState ? (rec.CountDefectWorkKind > 0 ? 1 : 0) : 1;

                        table.Rows.Add(row);
                    }
                }
                else
                {
                    var row = table.NewRow();
                    row["id"] = rec.objectCrId;
                    row["MUName"] = dateForMunicipality[rec.municipalityId];
                    row["Address"] = rec.Address;

                    row["AmountTypesWork"] = rec.CountDefectWorkKind;
                    row["TypeWork"] = string.Empty;
                    row["NormalAmountTypesWork"] = rec.CountWorkKind;

                    row["Status"] = string.Empty;

                    row["AmountRepairList"] = 0;

                    table.Rows.Add(row);
                }
            }

            Data = table;
        }

        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatType = FormatType.Numeric };

            var config = new PivotConfiguration
            {
                Name = "RepairList",
                ModelName = "CR Report.DefectListReport"
            };

            var fields = new List<Field>
                {
                    new Field
                        {
                            Name = "MUName",
                            Area = Area.RowArea,
                            AreaIndex = 0,
                            DisplayName = "Муниципальное образование"
                        },
                    new Field
                        {
                            Name = "Address",
                            Area = Area.RowArea,
                            DisplayName = "Адрес объекта",
                            AreaIndex = 1
                        },
                    new Field
                        {
                            Name = "AmountTypesWork",
                            Area = Area.RowArea,
                            AreaIndex = 2,
                            DisplayName = "Количество видов работ",
                             CellFormat = numericCellFormat
                        },
                   new Field
                        {
                            Name = "NormalAmountTypesWork",
                            Area = Area.RowArea,
                            AreaIndex = 3,
                            DisplayName = "Нормативное количество видов работ",
                             CellFormat = numericCellFormat
                        },

                    new Field
                        {
                            Name = "TypeWork",
                            Area = Area.ColumnArea,
                            AreaIndex = 0,
                            DisplayName = "Виды работ"
                        },
                    new Field
                        {
                            Name = "Status",
                            Area = Area.ColumnArea,
                            AreaIndex = 1,
                            DisplayName = "Статус"
                        },
                   
                    new Field
                        {
                            Name = "AmountRepairList",
                            Area = Area.DataArea,
                            AreaIndex = 1,
                            DisplayName = "Количество дефектных ведомостей",
                            SummaryType = SummaryType.Sum,
                            CellFormat = numericCellFormat
                        }
                };

            config.Fields = fields;
            return config;
        }

        public Stream GetTemplate()
        {
            throw new NotImplementedException();
        }

        public void PrepareReport(ReportParams reportParams)
        {
            throw new NotImplementedException();
        }

        public void SetUserParams(BaseParams baseParams)
        {
        }

        public string ReportGenerator { get; set; }

        private static DataTable GetTable()
        {
            var table = new DataTable();

            table.Columns.Add(new DataColumn("id"));
            table.Columns.Add(new DataColumn("MUName"));
            table.Columns.Add(new DataColumn("Address"));
            table.Columns.Add(new DataColumn("AmountRepairList", typeof(int)));
            table.Columns.Add(new DataColumn("AmountTypesWork", typeof(int)));
            table.Columns.Add(new DataColumn("NormalAmountTypesWork", typeof(int)));
            table.Columns.Add(new DataColumn("TypeWork"));
            table.Columns.Add(new DataColumn("Status"));

            return table;
        }
    }
}