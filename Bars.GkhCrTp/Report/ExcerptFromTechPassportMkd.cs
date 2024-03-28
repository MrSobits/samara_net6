namespace Bars.GkhCrTp.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.PassportProvider;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    // Отчет "Выписка из технического паспорта многоквартирного дома"
    public class ExcerptFromTechPassportMkd : BasePrintForm
    {
        #region параметры
        
        private long[] programCrId;
        private List<long> municipailityIds = new List<long>();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcerptFromTechPassportMkd"/> class.
        /// </summary>
        public ExcerptFromTechPassportMkd()
            : base(new ReportTemplateBinary(Properties.Resource.ExcerptFromTechPassportMkdReport))
        {
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Выписка из технического паспорта многоквартирного дома";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "Выписка из технического паспорта многоквартирного дома";
            }
        }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Техпаспорт";
            }
        }

        /// <summary>
        /// Gets the parameters controller.
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ExcerptFromTechPassportMkd";
            }
        }

        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.ExcerptFromTechPassportMkd";
            }
        }

        /// <summary>
        /// The set user parameters.
        /// </summary>
        /// <param name="baseParams">
        /// The base parameters.
        /// </param>
        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityParam = baseParams.Params["municipality"].ToStr();
            this.municipailityIds = string.IsNullOrEmpty(municipalityParam)
                                   ? new List<long>()
                                   : municipalityParam.Split(',').Select(x => x.ToLong()).ToList();

            var progList = baseParams.Params.ContainsKey("programCrId")
                                  ? baseParams.Params["programCrId"].ToString()
                                  : string.Empty;

            this.programCrId = !string.IsNullOrEmpty(progList) ? progList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
            
        }

        public override string ReportGenerator { get; set; }

        /// <summary>
        /// The prepare report.
        /// </summary>
        /// <param name="reportParams">
        /// The report parameters.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт");
            if (passport == null)
            {
                throw new Exception("Не найден провайдер технического паспорта");
            }

            IQueryable<long> objCrId = null;
            if (this.programCrId.Length > 0)
            {
                objCrId =
                    this.Container.Resolve<IDomainService<ObjectCr>>()
                        .GetAll()
                        .WhereIf(this.programCrId.Length > 0, x => this.programCrId.Contains(x.ProgramCr.Id))
                        .Select(x => x.RealityObject.Id);
            }

            var realtyObjQuery = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                    .WhereIf(this.municipailityIds.Count > 0, x => this.municipailityIds.Contains(x.Municipality.Id))
                    .WhereIf(this.programCrId.Length > 0 && objCrId != null, x => objCrId.Contains(x.Id))
                    .Where(x => x.TypeHouse == TypeHouse.ManyApartments && x.ConditionHouse != ConditionHouse.Razed);

            var realtyObjByMunicipalDict = realtyObjQuery
                .Select(x => new
                    {
                        x.Id,
                        municipalName = x.Municipality.Name,
                        RealAddress = x.Address
                    })
                    .AsEnumerable()
                    .OrderBy(x => x.municipalName)
                    .GroupBy(x => x.municipalName)
                    .ToDictionary(x => x.Key, x => x.OrderBy(y => y.RealAddress));

            var realtyObjIds = realtyObjQuery.Select(x => x.Id);
            
            var teсhPassportValueDict = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                .Where(x => realtyObjIds.Contains(x.TehPassport.RealityObject.Id)
                                               && (

                                           // Общие сведения
                                               (x.FormCode == "Form_1" && x.CellCode == "6:1") // Общая площадь
                                               || (x.FormCode == "Form_1" && x.CellCode == "7:1") // в том числе: жилой части здания (кв.м), в т.ч.
                    // Отопление
                                               || (x.FormCode == "Form_3_1" && x.CellCode == "1:3") // Вид
                                               || (x.FormCode == "Form_3_1_2" && x.CellCode == "1:3") // ПУ иУУ
                                               || (x.FormCode == "Form_3_1_2" && x.CellCode == "2:3")
                                               || (x.FormCode == "Form_3_1_3" && x.CellCode == "20:1") // Кол-во ввода
                                               || (x.FormCode == "Form_3_1_2" && x.CellCode == "1:4") // Кол-во ПУ
                    // Холодное водоснабжение
                                               || (x.FormCode == "Form_3_2_CW" && x.CellCode == "1:3") // Вид 
                                               || (x.FormCode == "Form_3_2CW_2" && x.CellCode == "1:3") || (x.FormCode == "Form_3_2CW_2" && x.CellCode == "2:3") // Пу и УУ
                                               || (x.FormCode == "Form_3_2CW_3" && x.CellCode == "3:1") // Кол-во ввода
                                               || (x.FormCode == "Form_3_2CW_2" && x.CellCode == "1:4") // Кол-во ПУ
                    // Горячее водоснабжение
                                               || (x.FormCode == "Form_3_2" && x.CellCode == "1:3") // Вид
                                               || (x.FormCode == "Form_3_2_2" && x.CellCode == "1:3") || (x.FormCode == "Form_3_2_2" && x.CellCode == "2:3") // ПУ и УУ
                                               || (x.FormCode == "Form_3_2_3" && x.CellCode == "11:1") // Кол-во ввода
                                               || (x.FormCode == "Form_3_2_2" && x.CellCode == "1:4") // Кол-во ПУ
                    // Электроснабжение
                                               || (x.FormCode == "Form_3_3" && x.CellCode == "1:3") // Вид
                                               || (x.FormCode == "Form_3_3_2" && x.CellCode == "1:3") // ПУ
                                               || (x.FormCode == "Form_3_3_2" && x.CellCode == "1:4") // кол-во ПУ
                    // Удельная тепловая энергия
                                               || (x.FormCode == "Form_6_3" && x.CellCode == "1:5")))
                                       .Select(x => new
                                       {
                                           realtyObjId = x.TehPassport.RealityObject.Id,
                                           x.FormCode,
                                           x.CellCode,
                                           x.Value
                                       })
                                       .AsEnumerable()
                                       .GroupBy(x => x.realtyObjId)
                                        .ToDictionary(
                                               x => x.Key,
                                               x => x.GroupBy(y => y.FormCode)
                                                    .ToDictionary(
                                                        y => y.Key,
                                                        y => y.ToDictionary(
                                                                z => z.CellCode,
                                                                z => passport.GetTextForCellValue(y.Key, z.CellCode, z.Value))));

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var sectionData = section.ДобавитьСекцию("sectionData");

            var num = 0;
            foreach (var municipality in realtyObjByMunicipalDict)
            {
                var sumAreaMkd = 0M;
                var sumAreaLiving = 0M;

                section.ДобавитьСтроку();

                foreach (var realtyObject in municipality.Value)
                {
                    var dataPassport = new Dictionary<string, Dictionary<string, string>>();

                    if (teсhPassportValueDict.ContainsKey(realtyObject.Id))
                    {
                        dataPassport = teсhPassportValueDict[realtyObject.Id];
                    }
                    
                    sumAreaMkd += this.GetValue(dataPassport, "Form_1", "6:1").ToDecimal();
                    sumAreaLiving += this.GetValue(dataPassport, "Form_1", "7:1").ToDecimal();

                    sectionData.ДобавитьСтроку();

                    sectionData["number"] = ++num;
                    sectionData["addressMkd"] = realtyObject.RealAddress;
                    sectionData["areaMkd"] = this.GetValue(dataPassport, "Form_1", "6:1").ToDouble();
                    sectionData["livingAreaMkd"] = this.GetValue(dataPassport, "Form_1", "7:1").ToDouble();

                    sectionData["heatingType"] = this.GetTypeValue(dataPassport, "Form_3_1");
                    sectionData["heatingPuUu"] = this.GetPUandUUValue(dataPassport, "Form_3_1_2");
                    sectionData["heatingEntersCount"] = this.GetValue(dataPassport, "Form_3_1_3", "20:1").ToInt();
                    sectionData["heatingPuCount"] = this.GetValue(dataPassport, "Form_3_1_2", "1:4").ToInt();

                    sectionData["coldWaterType"] = this.GetTypeValue(dataPassport, "Form_3_2_CW");
                    sectionData["coldWaterPuUu"] = this.GetPUandUUValue(dataPassport, "Form_3_2CW_2");
                    sectionData["coldWaterEntersCount"] = this.GetValue(dataPassport, "Form_3_2CW_2", "1:3").ToInt();
                    sectionData["coldWaterPuCount"] = this.GetValue(dataPassport, "Form_3_2CW_2", "1:4").ToInt();

                    sectionData["hotWaterType"] = this.GetTypeValue(dataPassport, "Form_3_2");
                    sectionData["hotWaterPuUu"] = this.GetPUandUUValue(dataPassport, "Form_3_2_2");
                    sectionData["hotWaterEntersCount"] = this.GetValue(dataPassport, "Form_3_2_3", "11:1").ToInt();
                    sectionData["hotWaterPuCount"] = this.GetValue(dataPassport, "Form_3_2_2", "1:4").ToInt();

                    sectionData["electricityType"] = this.GetTypeValue(dataPassport, "Form_3_3");
                    sectionData["electricityPu"] = this.GetValue(dataPassport, "Form_3_3_2", "1:3") == "Да"
                                                       ? "ПУ"
                                                       : string.Empty;
                    sectionData["electricityEntersCountLight"] = 0;
                    sectionData["electricityEntersCountPower"] = 0;
                    sectionData["electricityPuCount"] = this.GetValue(dataPassport, "Form_3_3_2", "1:4").ToInt();

                    sectionData["specificPowerForVentilationAndPeriod"] = this.GetValue(dataPassport, "Form_6_3", "1:5");
                }

                section["municipality"] = municipality.Key;
                section["areaMKD"] = sumAreaMkd;
                section["areaLiving"] = sumAreaLiving;
            }
        }

        private string GetPUandUUValue(Dictionary<string, Dictionary<string, string>> dataPassport, string FormId)
        {
            var pu = this.GetValue(dataPassport, FormId, "1:3");
            var uu = this.GetValue(dataPassport, FormId, "2:3");

            if (pu == "Да" && uu == "Да")
            {
                return "ПУ и УУ";
            }

            if (pu == "Да" && uu != "Да")
            {
                return "ПУ";
            }

            if (pu != "Да" && uu == "Да")
            {
                return "УУ";
            }

            return string.Empty;
        }

        private string GetValue(Dictionary<string, Dictionary<string, string>> dictData, string codeForm, string codeCell)
        {
            if (dictData.ContainsKey(codeForm))
            {
                if (dictData[codeForm].ContainsKey(codeCell))
                {
                    return dictData[codeForm][codeCell];
                }
            }

            return "0";
        }

        private string GetTypeValue(Dictionary<string, Dictionary<string, string>> dictData, string codeForm)
        {
            if (dictData.ContainsKey(codeForm))
            {
                if (dictData[codeForm].ContainsKey("1:3"))
                {
                    return dictData[codeForm]["1:3"];
                }
            }

            return string.Empty;
        }
    }
}