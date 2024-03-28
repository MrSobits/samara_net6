namespace Bars.GkhDi.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;

    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public sealed class CellCodesProxy
    {
        public string Name;

        public string FormCode;

        public string CellCode;

        public string CellCodeAdditional;

        public List<string> CellCodes = new List<string>();
    };

    public class FillingTechPassportForManOrgRatingReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalityIds = new List<long>();

        private Dictionary<long, Dictionary<string, Dictionary<string, string>>> techPassportDataDict;

        #region Cell Codes

		private Dictionary<int, CellCodesProxy> deviceByInfo = new Dictionary<int, CellCodesProxy>()
            {
				{93, new CellCodesProxy()
                         {
                             Name = "Дата проведения энергетического аудита", 
                             FormCode = "Form_6_1_1", 
                             CellCode = "2:1",
                         }},
				{92, new CellCodesProxy()
                         {
                             Name = "Класс энергоэффективности здания", 
                             FormCode = "Form_6_1_1", 
                             CellCode = "1:1",
                         }},
                {91, new CellCodesProxy()
                         {
                             Name = "Нормативный удельный расход", 
                             FormCode = "Form_6_1_2", 
                             CellCode = "2:1",
                         }},
				{90, new CellCodesProxy()
                         {
                             Name = "Фактический удельный расход", 
                             FormCode = "Form_6_1_1", 
                             CellCode = "1:1",
                         }},
				{89, new CellCodesProxy()
                         {
                             Name = "Изготовитель", 
                             FormCode = "Form_4_2_1", 
                             CellCode = "3",
                         }},
				{88, new CellCodesProxy()
                         {
                             Name = "Плановый срок замены", 
                             FormCode = "Form_4_2_1", 
                             CellCode = "13",
                         }},		 
				{87, new CellCodesProxy()
                         {
                             Name = "Год последней модернизации", 
                             FormCode = "Form_4_2_1", 
                             CellCode = "10",
                         }},
				{86, new CellCodesProxy()
                         {
                             Name = "Год ввода в эксплуатацию", 
                             FormCode = "Form_4_2_1", 
                             CellCode = "9",
                         }},			 
				{85, new CellCodesProxy()
                         {
                             Name = "Грузоподъемность", 
                             FormCode = "Form_4_2_1", 
                             CellCode = "5",
                         }},
				{84, new CellCodesProxy()
                         {
                             Name = "Количество остановок", 
                             FormCode = "Form_4_2_1", 
                             CellCode = "7",
                         }},			 
				{83, new CellCodesProxy()
                         {
                             Name = "Номер лифта", 
                             FormCode = "Form_4_2_1", 
                             CellCode = "2",
                         }},
				{82, new CellCodesProxy()
                         {
                             Name = "Номер подъезда", 
                             FormCode = "Form_4_2_1", 
                             CellCode = "1",
                         }},			 
				{81, new CellCodesProxy()
                         {
                             Name = "Количество лифтов", 
                             FormCode = "Form_4_2", 
                             CellCode = "1:1",
                         }},
				{80, new CellCodesProxy()
                         {
                             Name = "Отпуск ресурсов", 
                             FormCode = "Form_3_4_2", 
                             CellCode = "6:1",
                         }},			 
				{79, new CellCodesProxy()
                         {
                             Name = "Установлен прибор коллективного учета", 
                             FormCode = "Form_3_4_3", 
                             CellCode = "1:3",
                             CellCodeAdditional = "1:4"
                         }},			 
				{78, new CellCodesProxy()
                         {
                             Name = "Количество точек ввода", 
                             FormCode = "Form_3_4_2", 
                             CellCode = "7:1",
                         }},
				{77, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта системы", 
                             FormCode = "Form_3_4_2", 
                             CellCode = "5:1",
                         }},			 
				{76, new CellCodesProxy()
                         {
                             Name = "Длина сетей  не соответствующих требованиям", 
                             FormCode = "Form_3_4_2", 
                             CellCode = "4:1",
                         }},	
				{75, new CellCodesProxy()
                         {
                             Name = "Длина сетей соответствующих требованиям", 
                             FormCode = "Form_3_4_2", 
                             CellCode = "3:1",
                         }},			 
				{74, new CellCodesProxy()
                         {
                             Name = "Газоснабжение", 
                             FormCode = "Form_3_4", 
                             CellCode = "1:3",
                         }},
				{73, new CellCodesProxy()
                         {
                             Name = "Отпуск ресурсов", 
                             FormCode = "Form_3_3_3", 
                             CellCode = "16:1",
                         }},			 
				{72, new CellCodesProxy()
                         {
                             Name = "Установлен прибор коллективного учета", 
                             FormCode = "Form_3_3_2", 
                             CellCode = "1:3",
                             CellCodeAdditional = "1:4"
                         }},
				{71, new CellCodesProxy()
                         {
                             Name = "Количество точек ввода", 
                             FormCode = "Form_3_3_3", 
                             CellCode = "15:1",
                         }},			 
				{70, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта системы", 
                             FormCode = "Form_3_3_3", 
                             CellCode = "14:1",
                         }},
				{69, new CellCodesProxy()
                         {
                             Name = "Длина сетей в местах общего пользования", 
                             FormCode = "Form_3_3_3", 
                             CellCode = "13:1",
                         }},			 
				{68, new CellCodesProxy()
                         {
                             Name = "Электроснабжение", 
                             FormCode = "Form_3_3", 
                             CellCode = "1:3",
                         }},
				{67, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта системы", 
                             FormCode = "Form_3_3_Water_2", 
                             CellCode = "2:1",
                         }},			 
				{66, new CellCodesProxy()
                         {
                             Name = "Длина трубопроводов", 
                             FormCode = "Form_3_3_Water_2", 
                             CellCode = "1:1",
                         }},
				{65, new CellCodesProxy()
                         {
                             Name = "Водоотведение", 
                             FormCode = "Form_3_3_Water", 
                             CellCode = "1:3",
                         }},			 
				{64, new CellCodesProxy()
                         {
                             Name = "Отпуск ресурсов", 
                             FormCode = "Form_3_2CW_3", 
                             CellCode = "4:1",
                         }},
				{63, new CellCodesProxy()
                         {
                             Name = "Установлен прибор коллективного учета", 
                             FormCode = "Form_3_2CW_2", 
                             CellCode = "1:3",
                             CellCodeAdditional = "1:4"
                         }},			 
				{62, new CellCodesProxy()
                         {
                             Name = "Количество точек ввода", 
                             FormCode = "Form_3_2CW_3", 
                             CellCode = "3:1",
                         }},
				{61, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта системы", 
                             FormCode = "Form_3_2CW_3", 
                             CellCode = "2:1",
                         }},			 
				{60, new CellCodesProxy()
                         {
                             Name = "Длина трубопроводов", 
                             FormCode = "Form_3_2CW_3", 
                             CellCode = "1:1",
                         }},
				{59, new CellCodesProxy()
                         {
                             Name = "Холодное водоснабжение", 
                             FormCode = "Form_3_2_CW", 
                             CellCode = "1:3",
                         }},			 
				{58, new CellCodesProxy()
                         {
                             Name = "Отпуск ресурсов", 
                             FormCode = "Form_3_2_3", 
                             CellCode = "12:1",
                         }},
				{57, new CellCodesProxy()
                         {
                             Name = "Установлен прибор коллективного учета", 
                             FormCode = "Form_3_2_2", 
                             CellCode = "1:3",
                             CellCodeAdditional = "1:4"
                         }},			 
				{56, new CellCodesProxy()
                         {
                             Name = "Установлен узел управления", 
                             FormCode = "Form_3_2_2", 
                             CellCode = "2:3",
                             CellCodeAdditional = "2:4"
                         }},
				{55, new CellCodesProxy()
                         {
                             Name = "Количество точек ввода", 
                             FormCode = "Form_3_2_3", 
                             CellCode = "11:1",
                         }},			 
				{54, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта системы", 
                             FormCode = "Form_3_2_3", 
                             CellCode = "10:1",
                         }},
				{53, new CellCodesProxy()
                         {
                             Name = "Длина трубопроводов", 
                             FormCode = "Form_3_2_3", 
                             CellCode = "9:1",
                         }},			 
				{52, new CellCodesProxy()
                         {
                             Name = "Горячее водоснабжение", 
                             FormCode = "Form_3_2", 
                             CellCode = "1:3",
                         }},
				{51, new CellCodesProxy()
                         {
                             Name = "Отпуск ресурсов", 
                             FormCode = "Form_3_1_3", 
                             CellCode = "21:1",
                         }},			 
				{50, new CellCodesProxy()
                         {
                             Name = "Установлен прибор коллективного учета", 
                             FormCode = "Form_3_1_2", 
                             CellCode = "1:3",
                             CellCodeAdditional = "1:4"
                         }},
				{49, new CellCodesProxy()
                         {
                             Name = "Установлен узел управления", 
                             FormCode = "Form_3_1_2", 
                             CellCode = "2:3",
                             CellCodeAdditional = "2:4"
                         }},			 
				{48, new CellCodesProxy()
                         {
                             Name = "Количество точек ввода", 
                             FormCode = "Form_3_1_3", 
                             CellCode = "20:1",
                         }},
				{47, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта системы", 
                             FormCode = "Form_3_1_3", 
                             CellCode = "19:1",
                         }},			 
				{46, new CellCodesProxy()
                         {
                             Name = "Длина трубопроводов", 
                             FormCode = "Form_3_1_3", 
                             CellCode = "18:1",
                         }},
				{45, new CellCodesProxy()
                         {
                             Name = "Элеваторы", 
                             FormCode = "Form_3_1_3", 
                             CellCode = "15:1",
                         }},			 
				{44, new CellCodesProxy()
                         {
                             Name = "Отопление", 
                             FormCode = "Form_3_1", 
                             CellCode = "1:3",
                         }},
				{43, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта мусоропроводов", 
                             FormCode = "Form_3_7_3", 
                             CellCode = "6:1",
                         }},			 
				{42, new CellCodesProxy()
                         {
                             Name = "Количество мусоропроводов", 
                             FormCode = "Form_3_7_3", 
                             CellCode = "5:1",
                         }},
				{41, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта помещений общего пользования", 
                             FormCode = "Form_1_3", 
                             CellCode = "4:1",
                         }},			 
				{40, new CellCodesProxy()
                         {
                             Name = "Общая площадь встроенных нежилых помещений", 
                             FormCode = "Form_1_3", 
                             CellCode = "2:1",
                         }},		
				{39, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта подвала", 
                             FormCode = "Form_1_3_3", 
                             CellCode = "5:1",
                         }},    		 
				{36, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта кровли", 
                             FormCode = "Form_5_6_2", 
                             CellCode = "27:1",
                         }}, 
			    {35, new CellCodesProxy()
                {
                    Name = "Площадь кровли, в т.ч. по видам", 
                    FormCode = "Form_5_6_2", 
                    CellCodes = new List<string> { "23:1", "24:1", "25:1", "26:1" }
                }},
                {34, new CellCodesProxy()
                         {
                             Name = "Общая площадь кровли ", 
                             FormCode = "Form_5_6_2", 
                             CellCode = "22:1",
                         }},   
                {33, new CellCodesProxy()
                         {
                             Name = "Год последнего кап.ремонта стен", 
                             FormCode = "Form_5_2_3", 
                             CellCode = "1:1",
                         }},
		        {32, new CellCodesProxy()
                {
                    Name = "Площадь фасада, в т.ч. по видам", 
                    FormCode = "Form_5_8", 
                    CellCodes = new List<string> { "23:1", "24:1", "25:1", "26:1", "27:1", "28:1", "29:1", "30:1", "31:1" }
                        }},
				{31, new CellCodesProxy()
                         {
                             Name = "Общая площадь фасада (кв.м)", 
                             FormCode = "Form_5_8", 
                             CellCode = "22:1",
                         }}, 
                {25, new CellCodesProxy()
                         {
                             Name = "Степень износа перекрытий (%)", 
                             FormCode = "Form_1", 
                             CellCode = "23:1",
                         }},
                {24, new CellCodesProxy()
                         {
                             Name = "Степень износа несущих стен (%)", 
                             FormCode = "Form_1", 
                             CellCode = "22:1",
                         }}, 
                {23, new CellCodesProxy()
                         {
                             Name = "Степень износа фундамента (%)", 
                             FormCode = "Form_1", 
                             CellCode = "21:1",
                         }},
                {22, new CellCodesProxy()
                         {
                             Name = "Степень износа здания (%)", 
                             FormCode = "Form_1", 
                             CellCode = "20:1",
                         }},
                {21, new CellCodesProxy()
                         {
                             Name = "Конструктивные особенности", 
                             FormCode = "Form_1", 
                             CellCode = "19:1",
                         }},
                {20, new CellCodesProxy()
                         {
                             Name = "Тип перекрытий", 
                             FormCode = "Form_5_3", 
                             CellCode = "1:3",
                         }},
                {19, new CellCodesProxy()
                         {
                             Name = "Количество подъездов", 
                             FormCode = "Form_1", 
                             CellCode = "12:1",
                         }},
                {18, new CellCodesProxy()
                         {
                             Name = "Количество этажей, набольшее", 
                             FormCode = "Form_1", 
                             CellCode = "11:1",
                         }},
                {17, new CellCodesProxy()
                         {
                             Name = "Общая площадь встроенных нежилых помещений", 
                             FormCode = "Form_1_3", 
                             CellCode = "2:1",
                         }},
                {16, new CellCodesProxy()
                         {
                             Name = "Государственная собственность (кв. м)", 
                             FormCode = "Form_1", 
                             CellCode = "26:1",
                         }},
                {15, new CellCodesProxy()
                         {
                             Name = "Муниципальная собственность (кв. м)", 
                             FormCode = "Form_1", 
                             CellCode = "25:1",
                         }},
                {14, new CellCodesProxy()
                         {
                             Name = "Частная собственность (кв. м)", 
                             FormCode = "Form_1", 
                             CellCode = "24:1",
                         }},
                {13, new CellCodesProxy()
                         {
                             Name = "Площадь зданиия всего (кв. м)", 
                             FormCode = "Form_1", 
                             CellCode = "6:1",
                         }},
                {12, new CellCodesProxy()
                         {
                             Name = "Количество лицевых счетов", 
                             FormCode = "Form_1", 
                             CellCode = "14:1",
                         }},
                {11, new CellCodesProxy()
                         {
                             Name = "Кол-во проживающих", 
                             FormCode = "Form_1", 
                             CellCode = "13:1",
                         }},
                {10, new CellCodesProxy()
                         {
                             Name = "Кол-во квартир", 
                             FormCode = "Form_1_2", 
                             CellCode = "3",
                         }},
                {8, new CellCodesProxy()
                         {
                             Name = "Серия, тип проекта", 
                             FormCode = "Form_1", 
                             CellCode = "1:1",
                         }},
                {7, new CellCodesProxy()
                         {
                             Name = "Придомовая территория, всего: в том числе", 
                             FormCode = "Form_1_4", 
                             CellCode = "1:4",
                         }},
                {6, new CellCodesProxy()
                         {
                             Name = "Общая площадь земельного участка по документам (кв.м)", 
                             FormCode = "Form_2", 
                             CellCode = "1:3",
                         }},
                {4, new CellCodesProxy()
                         {
                             Name = "Инвентарный номер", 
                             FormCode = "Form_1", 
                             CellCode = "18:1",
                         }}						 
            };
        #endregion

        public FillingTechPassportForManOrgRatingReport()
            : base(new ReportTemplateBinary(Properties.Resources.FillingTechPassportForManOrgRating))
        {
        }

        public override string Name
        {
            get
            {
                return "Заполнение технического паспорта для рейтинга УК";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Заполнение технического паспорта для рейтинга УК";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Раскрытие  информации о деятельности УК";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.FillingTechPassportForManOrgRating";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.Di.FillingTechPassportForManOrgRating";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var municipalityList = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
               .Where(x => this.municipalityIds.Contains(x.Id))
               .Select(x => new { x.Id, x.Name })
               .OrderBy(x => x.Name)
               .ToDictionary(x => x.Id, x => x.Name);

            var manOrgRealtyObjectQuery = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .Where(x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= DateTime.Now)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                .Where(x => x.ManOrgContract.ManagingOrganization != null);

            var conditionHouseList = Enum.GetValues(typeof(ConditionHouse)).Cast<ConditionHouse>().ToList();
            var realtyObjectLandQuery = this.Container.Resolve<IDomainService<RealityObjectLand>>().GetAll().Where(x => x.CadastrNumber != null);
            var realtyObjectQuery = this.Container.Resolve<IDomainService<RealityObject>>().GetAll();
            var emergencyQuery = this.Container.Resolve<IDomainService<EmergencyObject>>().GetAll();

            var manOrgRealtyObjectDict = manOrgRealtyObjectQuery
                .Select(x => new
                                 {
                                     muId = x.RealityObject.Municipality.Id,
                                     moId = x.ManOrgContract.ManagingOrganization.Id,
                                     moName = x.ManOrgContract.ManagingOrganization.Contragent != null ? x.ManOrgContract.ManagingOrganization.Contragent.Name : string.Empty,
                                     roId = x.RealityObject.Id,
                                     x.RealityObject.Address,
                                     column5 = realtyObjectLandQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id),
                                     column9 = x.RealityObject.TypeHouse != TypeHouse.NotSet,
                                     column10 = realtyObjectQuery.Where(y => y.NumberApartments != null).Any(y => y.Id == x.RealityObject.Id) ? 1 : -1,
                                     column26 = conditionHouseList.Contains(x.RealityObject.ConditionHouse),
                                     column27 = emergencyQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id)
                                     ? emergencyQuery.Where(y => y.ActualInfoDate != null).Any(y => y.RealityObject.Id == x.RealityObject.Id) ? 1 : 0
                                     : -1,
                                     column28 = emergencyQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id)
                                     ? emergencyQuery.Where(y => y.DocumentNumber != null && y.DocumentNumber != string.Empty).Any(y => y.RealityObject.Id == x.RealityObject.Id) ? 1 : 0
                                     : -1,
                                     column29 = emergencyQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id)
                                     ? emergencyQuery.Where(y => y.ReasonInexpedient != null).Any(y => y.RealityObject.Id == x.RealityObject.Id) ? 1 : 0
                                     : -1,
                                     column30 = x.RealityObject.WallMaterial != null,
                                     column37 = x.RealityObject.HavingBasement != YesNoNotSet.NotSet,
                                     column38 = realtyObjectQuery.Where(y => y.AreaBasement != null && y.AreaBasement > 0).Any(y => y.Id == x.RealityObject.Id) ? 1 : -1
                                 })
                .AsEnumerable()
                .Distinct()
                .OrderBy(x => x.muId)
                .ThenBy(x => x.moName)
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key,
                              x => x.GroupBy(y => new { y.moId, y.moName })
                                    .ToDictionary(y => y.Key, y => y.ToList()));

            var realtyObjectIdQuery = manOrgRealtyObjectQuery.Select(x => x.RealityObject.Id);

            var formCodesList = this.deviceByInfo.Values.Select(x => x.FormCode).Distinct().ToList();
            
            var techPassport = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                .Where(x => realtyObjectIdQuery.Contains(x.TehPassport.RealityObject.Id))
                .Where(x => formCodesList.Contains(x.FormCode))
                .Select(x => new
                {
                    x.TehPassport.RealityObject.Id,
                    x.FormCode,
                    x.CellCode,
                    x.Value
                })
                .ToList();

            this.techPassportDataDict = techPassport
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                x => x.GroupBy(y => y.FormCode)
                    .ToDictionary(y => y.Key,
                        y => y.ToDictionary(z => z.CellCode, z => z.Value)));
            
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionMo = sectionMu.ДобавитьСекцию("sectionMo");
            var sectionRo = sectionMo.ДобавитьСекцию("sectionRo");

            var i = 0;

            foreach (var municipality in municipalityList)
            {
                if (!manOrgRealtyObjectDict.ContainsKey(municipality.Key))
                {
                    continue;
                }

                var muTotalsDict = this.GetTotalDictionary();

                sectionMu.ДобавитьСтроку();
                sectionMu["muName"] = municipality.Value;

                var percentByMu = 0M;

                foreach (var manOrgDict in manOrgRealtyObjectDict[municipality.Key])
                {
                    sectionMo.ДобавитьСтроку();
                    sectionMo["moName"] = manOrgDict.Key.moName;

                    var percentByMo = 0M;

                    var moTotalsDict = this.GetTotalDictionary();
                    
                    foreach (var realtyObject in manOrgDict.Value)
                    {
                        var sumValuesByRealtyObject = 0M;
                        var countValuesByRealtyObject = 90M;
                        
                        sectionRo.ДобавитьСтроку();
                        sectionRo["column1"] = ++i;
                        sectionRo["column2"] = municipality.Value;
                        sectionRo["column3"] = realtyObject.moName;
                        sectionRo["address"] = realtyObject.Address;

                        sectionRo["column5"] = realtyObject.column5 ? 1 : 0;
                        moTotalsDict[5] += realtyObject.column5 ? 1 : 0;
                        sumValuesByRealtyObject += realtyObject.column5 ? 1 : 0;

                        sectionRo["column9"] = realtyObject.column9 ? 1 : 0;
                        moTotalsDict[9] += realtyObject.column9 ? 1 : 0;
                        sumValuesByRealtyObject += realtyObject.column9 ? 1 : 0;

                        sectionRo["column10"] = realtyObject.column10 >= 0 ? realtyObject.column10.ToStr() : string.Empty;
                        moTotalsDict[10] += realtyObject.column10 >= 0 ? realtyObject.column10 : 0;
                        sumValuesByRealtyObject += realtyObject.column10 >= 0 ? realtyObject.column10 : 0;
                        countValuesByRealtyObject -= realtyObject.column10 < 0 ? 1 : 0;

                        sectionRo["column26"] = realtyObject.column26 ? 1 : 0;
                        moTotalsDict[26] += realtyObject.column26 ? 1 : 0;
                        sumValuesByRealtyObject += realtyObject.column26 ? 1 : 0;

                        sectionRo["column27"] = realtyObject.column27 >= 0 ? realtyObject.column27.ToStr() : string.Empty;
                        moTotalsDict[27] += realtyObject.column27 >= 0 ? realtyObject.column27 : 0;
                        sumValuesByRealtyObject += realtyObject.column27 >= 0 ? realtyObject.column27 : 0;
                        countValuesByRealtyObject -= realtyObject.column27 < 0 ? 1 : 0;

                        sectionRo["column28"] = realtyObject.column28 >= 0 ? realtyObject.column28.ToStr() : string.Empty;
                        moTotalsDict[28] += realtyObject.column28 >= 0 ? realtyObject.column28 : 0;
                        sumValuesByRealtyObject += realtyObject.column28 >= 0 ? realtyObject.column28 : 0;
                        countValuesByRealtyObject -= realtyObject.column28 < 0 ? 1 : 0;

                        sectionRo["column29"] = realtyObject.column29 >= 0 ? realtyObject.column29.ToStr() : string.Empty;
                        moTotalsDict[29] += realtyObject.column29 >= 0 ? realtyObject.column29 : 0;
                        sumValuesByRealtyObject += realtyObject.column29 >= 0 ? realtyObject.column29 : 0;
                        countValuesByRealtyObject -= realtyObject.column29 < 0 ? 1 : 0;

                        sectionRo["column30"] = realtyObject.column30 ? 1 : 0;
                        moTotalsDict[30] += realtyObject.column30 ? 1 : 0;
                        sumValuesByRealtyObject += realtyObject.column30 ? 1 : 0;
                        
                        sectionRo["column37"] = realtyObject.column37 ? 1 : 0;
                        moTotalsDict[37] += realtyObject.column37 ? 1 : 0;
                        sumValuesByRealtyObject += realtyObject.column37 ? 1 : 0;

                        sectionRo["column38"] = realtyObject.column38 >= 0 ? realtyObject.column38.ToStr() : string.Empty;
                        moTotalsDict[38] += realtyObject.column38 >= 0 ? realtyObject.column38 : 0;
                        sumValuesByRealtyObject += realtyObject.column38 >= 0 ? realtyObject.column38 : 0;
                        countValuesByRealtyObject -= realtyObject.column38 < 0 ? 1 : 0;

                        var excludedColumnNumbers = new List<int> { 5, 9, 10, 26, 27, 28, 29, 30, 37, 38 };

                        for (int columnNumber = 4; columnNumber <= 93; columnNumber++)
                        {
                            if (excludedColumnNumbers.Contains(columnNumber))
                            {
                                continue;
                            }

                            if (this.techPassportDataDict.ContainsKey(realtyObject.roId))
                            {
                                var data = this.GetTechpassportData(realtyObject.roId, columnNumber);

                                if (data >= 0)
                                {
                                    sectionRo[string.Format("column{0}", columnNumber)] = data;
                                    moTotalsDict[columnNumber] += data;
                                    sumValuesByRealtyObject += data;
                                }
                                else
                                {
                                    sectionRo[string.Format("column{0}", columnNumber)] = string.Empty;
                                    countValuesByRealtyObject -= 1;
                                }
                            }
                            else
                            {
                                sectionRo[string.Format("column{0}", columnNumber)] = 0;
                            }
                        }

                        var percentByRealtyObject = sumValuesByRealtyObject / countValuesByRealtyObject;
                        percentByMu += percentByRealtyObject;
                        percentByMo += percentByRealtyObject;
                        sectionRo["column94"] = percentByRealtyObject;
                    }

                    // заполнение итогов по УК

                    sectionMo["column2TotalMo"] = municipality.Value;
                    sectionMo["column3TotalMo"] = manOrgDict.Key.moName;
                    foreach (var totalMo in moTotalsDict)
                    {
                        sectionMo[string.Format("column{0}TotalMo", totalMo.Key)] = totalMo.Value;
                    }

                    sectionMo["column94TotalMo"] = percentByMo / manOrgDict.Value.Count;
                    this.AttachDictionaries(muTotalsDict, moTotalsDict);
                }

                // заполнение итогов по Мун.Образованию
                sectionMu["column2TotalMu"] = municipality.Value;
                foreach (var totalMu in muTotalsDict)
                {
                    sectionMu[string.Format("column{0}TotalMu", totalMu.Key)] = totalMu.Value;
                }

                sectionMu["column94TotalMu"] = percentByMu / manOrgRealtyObjectDict[municipality.Key].Values.Sum(x => x.Count());
            }
        }

        private Dictionary<int, int> GetTotalDictionary()
        {
            var result = new Dictionary<int, int>();
            for (int columnNumber = 4; columnNumber <= 93; columnNumber++)
            {
                result[columnNumber] = 0;
            }

            return result;
        }

        private void AttachDictionaries(Dictionary<int, int> totals, Dictionary<int, int> addable)
        {
            foreach (var key in addable.Keys)
            {
                if (totals.ContainsKey(key))
                {
                    totals[key] += addable[key];
                }
                else
                {
                    totals[key] = addable[key];
                }
            }
        }

        private int GetTechpassportData(long roId, int columnNumber)
        {
            var columnData = this.deviceByInfo.ContainsKey(columnNumber) ? this.deviceByInfo[columnNumber] : null;

            if (columnData == null)
            {
                return 0;
            }

            Dictionary<string, string> formData;

            if (this.techPassportDataDict[roId].ContainsKey(columnData.FormCode))
            {
                formData = this.techPassportDataDict[roId][columnData.FormCode];
            }
            else
            {
                return 0;
            }

            switch (columnNumber)
            {
                case 32:
                case 35: return this.GetColumnCustomData(formData, columnData);
                case 31:
                case 34:
                case 51:
                case 58:
                case 64:
                case 73:
                case 80: return this.GreaterThanZero(formData, columnData.CellCode);
                case 42: return formData.ContainsKey(columnData.CellCode) ? (string.IsNullOrEmpty(formData[columnData.CellCode]) ? 0 : -1)  : 0;
                case 44:
                case 52:
                case 59:
                case 65:
                case 68:
                case 74: return this.CheckSystemType(formData, columnData.CellCode, columnNumber);
                case 49:
                case 50:
                case 56:
                case 57:
                case 63:
                case 72:
                case 79: return this.GetColumnAdditionalData(formData, columnData);
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89: return this.GetElevatorData(formData, columnData.CellCode);
                default: return this.GetColumnData(formData, columnData.CellCode);
            }
        }

        private int GetColumnData(Dictionary<string, string> formData, string cellCode)
        {
            if (formData.ContainsKey(cellCode))
            {
                var techPassportValue = formData[cellCode];

                return string.IsNullOrEmpty(techPassportValue) ? 0 : 1;
            }

            return 0;
        }

        private int GetColumnCustomData(Dictionary<string, string> formData, CellCodesProxy cellInfo)
        {
            return cellInfo.CellCodes.Any(x => formData.ContainsKey(x) && !string.IsNullOrEmpty(formData[x])) ? 1 : 0;
        }

        private int GetElevatorData(Dictionary<string, string> formData, string cellCode)
        {
            return formData
                .Where(x => x.Key.EndsWith(":" + cellCode))
                .Any(x => !string.IsNullOrEmpty(x.Value)) ? 1 : 0;
        }

        private int GreaterThanZero(Dictionary<string, string> formData, string cellCode)
        {
            if (formData.ContainsKey(cellCode))
            {
                return formData[cellCode].ToDouble() > 0 ? 1 : 0;
            }

            return 0;
        }

        private int CheckSystemType(Dictionary<string, string> formData, string cellCode, int columnNumber)
        {
            if (formData.ContainsKey(cellCode))
            {
                int systemTypeValue;

                if (int.TryParse(formData[cellCode], out systemTypeValue))
                {
                    if (systemTypeValue == 0)
                    {
                        return 0;
                    }

                    if ((columnNumber == 59 || columnNumber == 65 || columnNumber == 74) && systemTypeValue == 3)
                    {
                        return -1;
                    }

                    if ((columnNumber == 59 || columnNumber == 65 || columnNumber == 74) && systemTypeValue == 3)
                    {
                        return -1;
                    }

                    if (columnNumber == 68 && systemTypeValue == 2)
                    {
                        return -1;
                    }

                    if (columnNumber == 44 && systemTypeValue == 5)
                    {
                        return -1;
                    }

                    if (columnNumber == 52 && systemTypeValue == 6)
                    {
                        return -1;
                    }

                    return 1;
                }

                return -1;
            }

            return -1;
        }

        private int GetColumnAdditionalData(Dictionary<string, string> formData, CellCodesProxy cellInfo)
        {
            if (formData.ContainsKey(cellInfo.CellCode))
            {
                var techPassportValue = formData[cellInfo.CellCode];

                int value;

                if (int.TryParse(techPassportValue, out value))
                {
                    if (formData.ContainsKey(cellInfo.CellCodeAdditional))
                    {
                        var countValue = formData[cellInfo.CellCodeAdditional];

                        int count;

                        if (int.TryParse(countValue, out count))
                        {
                            if (value == 1 && count > 0)
                            {
                                return 1;
                            }

                            if (value == 0 && count == 0)
                            {
                                return -1;
                            }
                        }

                    }

                }
            }
            
            return 0;
        }
    }
}