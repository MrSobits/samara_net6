namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Serialization;
    using Castle.Core.Internal;

    using Castle.Windsor;
    using Gkh.Domain;

    using Newtonsoft.Json;

    /// <summary>
	/// Сервис для работы с сущностью "Объект недвижимости деятельности управляющей организации в периоде раскрытия информации"
	/// </summary>
	public class DisclosureInfoRealityObjService : IDisclosureInfoRealityObjService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Получить объект недвижимости деятельности управляющей организации в периоде раскрытия информации
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
		public IDataResult GetDisclosureInfo(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var realtyObjId = baseParams.Params.GetAs<long>("realtyObjId");
                var manOrgId = baseParams.Params.GetAs<long>("manOrgId");
                var realtyObj = this.Container.Resolve<IDomainService<RealityObject>>().Get(realtyObjId);

                // Получаем раскрытие в доме по раскрытию
                var disclosureInfoRealityObj = this.Container.Resolve<IDisclosureInfoService>().GetDiRoByDi(disclosureInfoId, realtyObjId, manOrgId);
                if (disclosureInfoRealityObj != null)
                {
                    var address = realtyObj.FiasAddress != null ? realtyObj.FiasAddress.AddressName : string.Empty;
                    return new BaseDataResult(new { disclosureInfoRealityObj.Id, disclosureInfoId, realtyObjId, Address = address })
                    {
                        Success = true
                    };
                }

                return new BaseDataResult(new { Id = 0 })
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

		/// <summary>
		/// Получить копию
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
        public IDataResult GetCopyInfo(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

                var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().Load(disclosureInfoId);
                var disclosureInfoRealityObj = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>().Load(disclosureInfoRealityObjId);

                return new BaseDataResult(new
                                            {
                                                ManagingOrgName = disclosureInfo.ManagingOrganization.Contragent.Name,
                                                PeriodName = disclosureInfoRealityObj.PeriodDi.Name,
                                                Address = disclosureInfoRealityObj.RealityObject.FiasAddress.AddressName,
                                                PeriodDiId = disclosureInfoRealityObj.PeriodDi.Id
                                            })
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        public class RealtyObjectPassport
        {
            public long Id { get; set; }

            public int? BuildYear { get; set; }

            public DateTime? DateCommissioning { get; set; }

            public string TypeOfProject { get; set; }

            public TypeHouse TypeHouse { get; set; }

            public YesNo ConditionHouse { get; set; }

            public string TypeOfFormingCr { get; set; }

            public int? Floors { get; set; }

            public int? MaximumFloors { get; set; }

            public int? NumberEntrances { get; set; }

            public int? NumberLifts { get; set; }

            public string EnergyEfficiencyClass { get; set; }

            public int? NumberApartments { get; set; }

            public int? NumberNonResidentialPremises { get; set; }

            public int? AllNumberOfPremises { get; set; }

            public decimal? AreaMkd { get; set; }

            public decimal? AreaLiving { get; set; }

            public decimal? AreaNotLivingPremises { get; set; }

            public decimal? AreaOfAllNotLivingPremises { get; set; }

            public string CadastreNumber { get; set; }

            public decimal? DocumentBasedArea { get; set; }

            public decimal? ParkingArea { get; set; }

            public string ChildrenArea { get; set; }

            public string SportArea { get; set; }

            public CrFundFormationType RawTypeOfFormingCr { get; set; }

            public string OtherArea { get; set; }

            public string OwnerInn { get; set; }

            public string OwnerName { get; set; }

            public DateTime? ProtocolDate { get; set; }

            public string ProtocolNumber { get; set; }

            public decimal? Paysize { get; set; }
        }

        #region GetRealtyObjectPassport

		/// <summary>
		/// Получить паспорт жилого дома
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
        public IDataResult GetRealtyObjectPassport(BaseParams baseParams) {
            return this.GetRealtyObjectPassport(baseParams.Params.GetAsId("diRoId"), baseParams.Params.GetAs<DateTime>("yearStart"), baseParams.Params.GetAsId("diId"));
        }
		
		/// <summary>
		/// Получить паспорт жилого дома
		/// </summary>
		/// <param name="diRoId">Идентификатор для Объект недвижимости деятельности управляющей организации в периоде раскрытия информации</param>
		/// <param name="diId">Идентификатор для Деятельность управляющей организации в периоде раскрытия информации</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult GetRealtyObjectPassport(long diRoId, DateTime? year = null, long? diId = null)
        {
            var disclosureInfoRoDomain = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            var disclosureInfoDomain = this.Container.ResolveDomain<DisclosureInfo>();
            var energyEfficiencyClassDomain = this.Container.ResolveDomain<EnergyEfficiencyClasses>();
		    IRealityObjectDecisionProtocolProxyService roDecisionProtocolProxyService = null;

            // Делаем так, потому что может быть такое,что модуля регопа не будет в текущей сборке
		    if (this.Container.HasComponent<IRealityObjectDecisionProtocolProxyService>())
		    {
		        roDecisionProtocolProxyService = this.Container.Resolve<IRealityObjectDecisionProtocolProxyService>();
		    }

            try
            {
                var record = disclosureInfoRoDomain.Get(diRoId);

                if (record == null || record.RealityObject == null)
                {
                    return BaseDataResult.Error("Не удалось получить запись");
                }

                var ro = record.RealityObject;
				var disclosureInfo = diId != null ? disclosureInfoDomain.Get(diId) : null;

				var childrenArea = this.GetDataFromPassport(ro, DisclosureInfoRealityObjService.TehPassportChildrenArea);
                var sportArea = this.GetDataFromPassport(ro, DisclosureInfoRealityObjService.TehPassportSportArea);
                var otherArea = this.GetDataFromPassport(ro, DisclosureInfoRealityObjService.TehPassportOtherArea);
           
                var documentBasedArea = this.GetDataFromPassport(ro, DisclosureInfoRealityObjService.TehPassportDocumentBasedArea);
                var parkingArea = this.GetDataFromPassport(ro, DisclosureInfoRealityObjService.TehPassportParkingArea);

                var energyEfficiencyId = this.GetDataFromPassport(ro, DisclosureInfoRealityObjService.EnergyEfficiency).ToLong();
                var typeOfFormingCr = this.GetTypeOfFormingCr(ro);

                var result = new RealtyObjectPassport
                {
                    Id = ro.Id,
                    BuildYear = ro.BuildYear,
                    DateCommissioning = ro.DateCommissioning,
                    TypeOfProject =
                        ro.TypeProject == null
                            ? "Неопределен"
                            : ro.TypeProject.Name,
                    TypeHouse = ro.TypeHouse,
                    ConditionHouse = this.IsEmergencyRealityObject(ro),
                    RawTypeOfFormingCr = typeOfFormingCr,
                    TypeOfFormingCr = typeOfFormingCr.GetAttribute<DisplayAttribute>().Value,
                    Floors = ro.Floors,
                    MaximumFloors = ro.MaximumFloors,
                    NumberEntrances = ro.NumberEntrances,
                    NumberLifts = ro.NumberLifts,
                    EnergyEfficiencyClass = energyEfficiencyId != 0 ? energyEfficiencyClassDomain.FirstOrDefault(x => x.Id == energyEfficiencyId).Designation : "Не присвоен",
                    NumberApartments = ro.NumberApartments,
                    NumberNonResidentialPremises = ro.NumberNonResidentialPremises,
                    AllNumberOfPremises =
                        (ro.NumberApartments ?? 0)
                        + (ro.NumberNonResidentialPremises ?? 0),
                    AreaMkd = ro.AreaMkd,
                    AreaLiving = ro.AreaLiving,
                    AreaNotLivingPremises = ro.AreaNotLivingPremises,
                    AreaOfAllNotLivingPremises = ro.AreaNotLivingFunctional,
                    CadastreNumber = ro.CadastreNumber,
                    DocumentBasedArea =
                        documentBasedArea?.ToDecimal(),
                    ParkingArea =
                        parkingArea?.ToDecimal(),
                    ChildrenArea =
                        childrenArea.IsEmpty() || childrenArea == "0"
                            ? "Не имеется"
                            : "Имеется",
                    SportArea =
                        sportArea.IsEmpty() || sportArea == "0"
                            ? "Не имеется"
                            : "Имеется",
                    OtherArea = otherArea
                };

                if (disclosureInfo != null && typeOfFormingCr != CrFundFormationType.Unknown)
	            {
                    if (typeOfFormingCr != CrFundFormationType.RegOpAccount)
                    {
                        result.OwnerInn = disclosureInfo.ManagingOrganization.Contragent.Inn;
                        result.OwnerName = disclosureInfo.ManagingOrganization.Contragent.Name;
                    }

                    var bothProtocolProxy = roDecisionProtocolProxyService?.GetBothProtocolProxy(ro);
                    if (bothProtocolProxy != null)
                    {
                        result.ProtocolDate = bothProtocolProxy.ProtocolDate;
                        result.ProtocolNumber = bothProtocolProxy.ProtocolNumber;
                        result.Paysize = roDecisionProtocolProxyService.GetPaysize(bothProtocolProxy.Id, ro.Id, year);
                    }
                }

                return new BaseDataResult(result);
            }

            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            
            finally
            {
	            this.Container.Release(disclosureInfoRoDomain);
	            this.Container.Release(disclosureInfoDomain);
	            this.Container.Release(energyEfficiencyClassDomain);

                if (roDecisionProtocolProxyService.IsNotNull())
                {
                    this.Container.Release(roDecisionProtocolProxyService);
            }
        }
        }

		/// <summary>
		/// Получить лифты жилого дома
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
        public IDataResult GetRealtyObjectLifts(BaseParams baseParams){
            return this.GetRealtyObjectLifts(baseParams.Params.GetAsId("diRoId"));
        }

	    public class RealtyObjectLift
	    {
	        public int Id { get; set; }

	        public string EntranceNumber { get; set; }

	        public string CommissioningYear { get; set; }

	        public IEnumerable<string> Type { get; set; }
	    }

	    public IDataResult GetRealtyObjectLifts(long diRoId)
        {
            var disclosureDomain = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            try
            {
                var record = disclosureDomain.Get(diRoId);

                if (record == null || record.RealityObject == null)
                {
                    return BaseDataResult.Error("Не удалось получить запись");
                }

                var ro = record.RealityObject;

                var formId = "Form_4_2";
                var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");
                var componentCodes = passport.GetComponentCodes(formId).Where(x => x != "Form_4_2");
                var editors = (List<EditorTechPassport>)passport.GetEditors(formId);
                var tehPassportValues = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                    .Where(x => x.TehPassport.RealityObject.Id == ro.Id && componentCodes.Contains(x.FormCode)).ToArray();
                var ids = tehPassportValues.GroupBy(x => int.Parse(x.CellCode.Split(new char[] { ':' }).First()));

                var result =
                    ids.Select(
                        id =>
                        new RealtyObjectLift
                            {
                                Id = id.Key,
                                EntranceNumber = tehPassportValues.Where(y => y.CellCode == (id.Key + ":1")).Select(y => y.Value).FirstOrDefault(),
                                CommissioningYear = tehPassportValues.Where(y => y.CellCode == (id.Key + ":9")).Select(y => y.Value).FirstOrDefault(),
                                Type = tehPassportValues.Where(y => y.CellCode == (id.Key + ":19")).Select(
                                    y =>
                                        {
                                            var columnEditor = passport.GetEditorByFormAndComponentAndCode(formId, "Form_4_2_1", "19").ToString();
                                            var editorValue = editors.FirstOrDefault(x => x.Type == columnEditor).Values.FirstOrDefault(x => x.Code == y.Value);
                                            return editorValue != null ? editorValue.Name : "";
                                        })
                            }).ToList();

                return new BaseDataResult(result);
            }
            finally
            {
	            this.Container.Release(disclosureDomain);
            }
        }

		/// <summary>
		/// Получить структурные элементы жилого дома
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
        public IDataResult GetRealtyObjectStructElements(BaseParams baseParams) {
            return this.GetRealtyObjectStructElements(baseParams.Params.GetAsId("diRoId"));
        }

        public IDataResult GetRealtyObjectStructElements(long diRoId)
        {
            const string basementTypeFormId = "Form_5_1";
            const string facadesForm = "Form_5_8";
            Tuple<string, string> basementAreaForm = new Tuple<string, string>("Form_5_4", "9:4");
            Tuple<string, string> typeWallsForm = new Tuple<string, string>("Form_5_2", "1:3");
            Tuple<string, string> typeFloorsForm = new Tuple<string, string>("Form_5_3", "1:3");
            Tuple<string, string> constrChuteForm = new Tuple<string, string>("Form_3_7_2", "1:3");
            Tuple<string, string> chutesNumberForm = new Tuple<string, string>("Form_3_7_3", "5:1");


            var disclosureDomain = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            var compareCodeDomain = this.Container.ResolveDomain<TechnicalPassportCompareCode>();
            try
            {
                var record = disclosureDomain.Get(diRoId);
                var passport = this.Container.ResolveAll<IPassportProvider>()
                             .FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");


                if (record == null || record.RealityObject == null || passport == null)
                {
                    return BaseDataResult.Error("Не удалось получить запись");
                }

                var ro = record.RealityObject;
                var tehPassport = this.Container.Resolve<IDomainService<TehPassportValue>>()
                             .GetAll()
                             .Where(x => x.TehPassport.RealityObject.Id == ro.Id)
                             .ToArray();

                //BasementType
                ComponentTechPassport basementTypeCodes =
                    passport.GetComponentBy(basementTypeFormId, basementTypeFormId) as ComponentTechPassport;
                var basementTypeValue =
                    tehPassport.Where(x => x.FormCode == "Form_5_1" && x.Value == "1")
                               .OrderByDescending(x => x.CellCode)
                               .FirstOrDefault();
                CellTechPassport basementType = null;
                if (basementTypeValue != null && basementTypeCodes != null)
                {
                    basementType =
                        basementTypeCodes.Cells.FirstOrDefault(
                            x => x.Code.Split(':').First() == basementTypeValue.CellCode.Split(':').First());
                }

                //BasementArea
                string basementArea = this.GetDataFromPassport(ro, basementAreaForm);

                //ChutesNumber
                var chutesNumber =
                    tehPassport.FirstOrDefault(
                        x => x.FormCode == chutesNumberForm.Item1 && x.CellCode == chutesNumberForm.Item2);

                //Facades
                var facades = new List<Facade>();
                //Оштукатуренный
                var facadesPlastered =
                    tehPassport.FirstOrDefault(x => x.FormCode == facadesForm && x.CellCode == "23:1");
                if (facadesPlastered?.Value.ToDecimal() > 0)
                {
                    facades.Add(new Facade { FacadeType = "Оштукатуренный", EditDate = facadesPlastered.ObjectEditDate });
                }
                //Облицованный плиткой
                var facadesTiled = tehPassport.FirstOrDefault(
                    x => x.FormCode == facadesForm && x.CellCode == "26:1");
                if (facadesTiled?.Value.ToDecimal() > 0)
                {
                    facades.Add(new Facade { FacadeType = "Облицованный плиткой", EditDate = facadesTiled.ObjectEditDate });
                }
                //Окрашенный 
                var facadesPrinted = tehPassport.FirstOrDefault(x => x.FormCode == facadesForm && x.CellCode == "9:1");
                if (facadesPrinted?.Value.ToDecimal() > 0)
                {
                    facades.Add(new Facade { FacadeType = "Окрашенный", EditDate = facadesPrinted.ObjectEditDate });
                }
                //Сайдинг
                var facadesSiding = tehPassport.FirstOrDefault(x => x.FormCode == facadesForm && x.CellCode == "27:1");
                if (facadesSiding?.Value.ToDecimal() > 0)
                {
                    facades.Add(new Facade { FacadeType = "Сайдинг", EditDate = facadesSiding.ObjectEditDate});
                }
                //Соответствует материалу стен 
                var facadesTotalAreaFacade = tehPassport.FirstOrDefault(x => x.FormCode == facadesForm && x.CellCode == "22:1");

                //Не заполнено или Соответствует материалу стен(если указана общая площадь)
                if (!facades.Any())
                {
                    facades.Add(
                        facadesTotalAreaFacade?.Value.ToDecimal() > 0
                            ? new Facade {FacadeType = "Соответствует материалу стен"}
                            : new Facade {FacadeType = "Не заполнено"});
                }

                if (facades.Count > 1)
                {
                    facades = facades.OrderByDescending(x => x.EditDate).Take(1).ToList();
                }

                //RoofTypes
                var roofTypes = new List<Roof>();
                var roofType = ro.TypeRoof.GetEnumMeta().Display;
                var roofingType = ro.RoofingMaterial.Return(x => x.Name);
                roofTypes.Add(new Roof { RoofType = roofType, RoofingType = roofingType });


                var result = new RealtyObjectStructElements
                {
                    Id = ro.Id,
                    BasementType =
                        basementType != null
                            ? basementType.Value
                            : string.Empty,
                    BasementArea = basementArea ?? string.Empty,
                    TypeFloor =
                        this.GetCellValue(
                            tehPassport,
                            typeFloorsForm.Item1,
                            typeFloorsForm.Item2,
                            ref passport),
                    TypeFloorReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeFloorsForm.Item1,
                            typeFloorsForm.Item2,
                            passport, compareCodeDomain),
                    TypeWalls =
                        this.GetCellValue(
                            tehPassport,
                            typeWallsForm.Item1,
                            typeWallsForm.Item2,
                            ref passport),
                    TypeWallsReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeWallsForm.Item1,
                            typeWallsForm.Item2,
                            passport, compareCodeDomain),
                    ConstructionChute =
                        this.GetCellValue(
                            tehPassport,
                            constrChuteForm.Item1,
                            constrChuteForm.Item2,
                            ref passport),
                    ConstructionChuteReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            constrChuteForm.Item1,
                            constrChuteForm.Item2,
                            passport, compareCodeDomain),
                    ChutesNumber =
                        chutesNumber != null
                            ? chutesNumber.Value
                            : string.Empty,
                    Facades = facades,
                    RoofTypes = roofTypes
                };

                return new BaseDataResult(result);
            }
            finally
            {
                this.Container.Release(disclosureDomain);
                this.Container.Release(compareCodeDomain);
            }
        }

        public class RealtyObjectStructElements
        {
            public long Id { get; set; }

            public string BasementType { get; set; }

            public string BasementArea { get; set; }

            public string TypeFloor { get; set; }

            public int? TypeFloorReformaCode { get; set; }

            public string TypeWalls { get; set; }

            public int? TypeWallsReformaCode { get; set; }

            public string ConstructionChute { get; set; }

            public int? ConstructionChuteReformaCode { get; set; }

            public string ChutesNumber { get; set; }

            public List<Facade> Facades { get; set; }

            public List<Roof> RoofTypes { get; set; }
        }

        public class Facade
        {
            public string FacadeType { get; set; }

            [JsonIgnore]
            public DateTime EditDate { get; set; }
        }

        public class Roof
        {
            public string RoofType { get; set; }

            public string RoofingType { get; set; }
        }

		/// <summary>
		/// Получить инженерные системы жилого дома
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
        public IDataResult GetRealtyObjectEngineerSystems(BaseParams baseParams) {
            return this.GetRealtyObjectEngineerSystems(baseParams.Params.GetAsId("diRoId"));
        }

        public IDataResult GetRealtyObjectEngineerSystems(long diRoId)
        {
            Tuple<string, string> typeHeatingForm = new Tuple<string, string>("Form_3_1", "1:3");
            Tuple<string, string> typeHotWaterForm = new Tuple<string, string>("Form_3_2", "1:3");
            Tuple<string, string> typeColdWaterForm = new Tuple<string, string>("Form_3_2_CW", "1:3");
            Tuple<string, string> typeGasForm = new Tuple<string, string>("Form_3_4", "1:3");
            Tuple<string, string> typeVentilationForm = new Tuple<string, string>("Form_3_5", "1:3");
            Tuple<string, string> typeFirefightingForm = new Tuple<string, string>("Form_3_8", "1:3");
            Tuple<string, string> typeDrainageForm = new Tuple<string, string>("Form_3_6", "1:3");
            Tuple<string, string> typePowerForm = new Tuple<string, string>("Form_3_3", "1:3");
            Tuple<string, string> typePowerPointsForm = new Tuple<string, string>("Form_3_3_3", "15:1");
            Tuple<string, string> typeSewageForm = new Tuple<string, string>("Form_3_3_Water", "1:3");
            Tuple<string, string> sewageVolumeForm = new Tuple<string, string>("Form_3_3_Water_2", "3:1");
            
            var disclosureDomain = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            var compareCodeDomain = this.Container.ResolveDomain<TechnicalPassportCompareCode>();
            try
            {
                var record = disclosureDomain.Get(diRoId);
                var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");


                if (record == null || record.RealityObject == null || passport == null)
                {
                    return BaseDataResult.Error("Не удалось получить запись");
                }

                var ro = record.RealityObject;
                var tehPassport = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll().Where(x => x.TehPassport.RealityObject.Id == ro.Id)
                    .ToArray();

                var typePowerPoints = tehPassport.FirstOrDefault(x => x.FormCode == typePowerPointsForm.Item1 && x.CellCode == typePowerPointsForm.Item2);
                var sewageVolume = tehPassport.FirstOrDefault(x => x.FormCode == sewageVolumeForm.Item1 && x.CellCode == sewageVolumeForm.Item2);


                var result = new RealtyObjectEngineerSystems
                {
                    Id = ro.Id,
                    TypeHeating =
                        this.GetCellValue(
                            tehPassport,
                            typeHeatingForm.Item1,
                            typeHeatingForm.Item2,
                            ref passport),
                    TypeHeatingReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeHeatingForm.Item1,
                            typeHeatingForm.Item2,
                            passport, compareCodeDomain),
                    TypeHotWater =
                        this.GetCellValue(
                            tehPassport,
                            typeHotWaterForm.Item1,
                            typeHotWaterForm.Item2,
                            ref passport),
                    TypeHotWaterReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeHotWaterForm.Item1,
                            typeHotWaterForm.Item2,
                            passport, compareCodeDomain),
                    TypeColdWater =
                        this.GetCellValue(
                            tehPassport,
                            typeColdWaterForm.Item1,
                            typeColdWaterForm.Item2,
                            ref passport),
                    TypeColdWaterReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeColdWaterForm.Item1,
                            typeColdWaterForm.Item2,
                            passport, compareCodeDomain),
                    TypeGas =
                        this.GetCellValue(
                            tehPassport,
                            typeGasForm.Item1,
                            typeGasForm.Item2,
                            ref passport),
                    TypeGasReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeGasForm.Item1,
                            typeGasForm.Item2,
                            passport, compareCodeDomain),
                    TypeVentilation =
                        this.GetCellValue(
                            tehPassport,
                            typeVentilationForm.Item1,
                            typeVentilationForm.Item2,
                            ref passport),
                    TypeVentilationReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeVentilationForm.Item1,
                            typeVentilationForm.Item2,
                            passport, compareCodeDomain),
                    Firefighting =
                        this.GetCellValue(
                            tehPassport,
                            typeFirefightingForm.Item1,
                            typeFirefightingForm.Item2,
                            ref passport),
                    FirefightingReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeFirefightingForm.Item1,
                            typeFirefightingForm.Item2,
                            passport, compareCodeDomain),
                    TypeDrainage =
                        this.GetCellValue(
                            tehPassport,
                            typeDrainageForm.Item1,
                            typeDrainageForm.Item2,
                            ref passport),
                    TypeDrainageReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeDrainageForm.Item1,
                            typeDrainageForm.Item2,
                            passport, compareCodeDomain),
                    TypePower =
                        this.GetCellValue(
                            tehPassport,
                            typePowerForm.Item1,
                            typePowerForm.Item2,
                            ref passport),
                    TypePowerReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typePowerForm.Item1,
                            typePowerForm.Item2,
                            passport, compareCodeDomain),
                    TypePowerPoints =
                        typePowerPoints != null
                            ? typePowerPoints.Value
                            : string.Empty,
                    TypeSewage =
                        this.GetCellValue(
                            tehPassport,
                            typeSewageForm.Item1,
                            typeSewageForm.Item2,
                            ref passport),
                    TypeSewageReformaCode =
                        this.GetCellReformaCode(
                            tehPassport,
                            typeSewageForm.Item1,
                            typeSewageForm.Item2,
                            passport, compareCodeDomain),
                    SewageVolume =
                        sewageVolume != null
                            ? sewageVolume.Value
                            : string.Empty
                };

                return new BaseDataResult(result);
            }
            finally
            {
                this.Container.Release(disclosureDomain);
                this.Container.Release(compareCodeDomain);
            }
        }

        public class RealtyObjectEngineerSystems
        {
            public long Id { get; set; }

            public string TypeHeating { get; set; }

            public int? TypeHeatingReformaCode { get; set; }

            public string TypeHotWater { get; set; }

            public int? TypeHotWaterReformaCode { get; set; }

            public string TypeColdWater { get; set; }

            public int? TypeColdWaterReformaCode { get; set; }

            public string TypeGas { get; set; }

            public int? TypeGasReformaCode { get; set; }

            public string TypeVentilation { get; set; }

            public int? TypeVentilationReformaCode { get; set; }

            public string Firefighting { get; set; }

            public int? FirefightingReformaCode { get; set; }

            public string TypeDrainage { get; set; }

            public int? TypeDrainageReformaCode { get; set; }

            public string TypePower { get; set; }

            public int? TypePowerReformaCode { get; set; }

            public string TypePowerPoints { get; set; }

            public string TypeSewage { get; set; }

            public int? TypeSewageReformaCode { get; set; }

            public string SewageVolume { get; set; }
        }

		/// <summary>
		/// Получить приборы учета жилого дома
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
        public IDataResult GetRealtyObjectDevices(BaseParams baseParams) {
            return this.GetRealtyObjectDevices(baseParams.Params.GetAsId("diRoId"));
        }

        public IDataResult GetRealtyObjectDevices(long diRoId)
        {
            var disclosureDomain = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            try
            {
                const string decFormId = "Form_6_6";
                const string readOnlyFormId = "Form_6_6_2";
                const string addsFormId = "Form_6_6_3";
                var record = disclosureDomain.Get(diRoId);
                var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");

                if (record == null || record.RealityObject == null || passport == null)
                {
                    return BaseDataResult.Error("Не удалось получить запись");
                }

                var ro = record.RealityObject;
                var componentCodes = passport.GetComponentCodes(decFormId);
                var editors = (List<EditorTechPassport>)passport.GetEditors(decFormId);

                var tehPassportValues = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                    .Where(x => x.TehPassport.RealityObject.Id == ro.Id && componentCodes.Contains(x.FormCode)).ToArray();

                var valuesFrom662 = tehPassportValues.Where(o => o.FormCode == readOnlyFormId)
                    .GroupBy(o => int.Parse(o.CellCode.Split(':').First()))
                    .Select(o => o.ToList()).ToList();

                var comp = passport.GetComponentBy(decFormId, readOnlyFormId) as ComponentTechPassport;
                if (comp != null)
                {
                    var listToAddToValuesFrom662 = new List<List<TehPassportValue>>();
                    // для каждой обязательной ячейки из грида - проверяем, есть ли она в массиве, если нет, то добавляем
                    foreach (var cell in comp.Cells)
                    {
                        // ищём, была ли данная строка изменена пользователем?
                        var rowWithCurrentCell =
                            valuesFrom662.FirstOrDefault(
                                arr2 => arr2.Any(x => x.CellCode.Split(':').First() == cell.Code.Split(':').First()));
                        // если строка существует в базе, тогда добавляем для этой строки ещё значение текущего cell
                        if (rowWithCurrentCell != null)
                        {
                            if (!rowWithCurrentCell.Any(x => x.Value == cell.Value && x.CellCode == cell.Code))
                            {
                                rowWithCurrentCell.Add(new TehPassportValue
                                {
                                    Value = cell.Value,
                                    CellCode = cell.Code
                                });
                            }
                        }
                        // иначе добавляем полностью новый список
                        else
                        {
                            listToAddToValuesFrom662.Add(new List<TehPassportValue>
                            {
                                new TehPassportValue {Value = cell.Value, CellCode = cell.Code}
                            });
                        }
                    }
                    valuesFrom662.AddRange(listToAddToValuesFrom662);
                }
                var values = valuesFrom662
                    .Union(tehPassportValues.Where(o => o.FormCode == addsFormId)
                    .GroupBy(o => int.Parse(o.CellCode.Split(':').First()))
                    .Select(o => o.ToList()));

                var result =
                    values.Select(
                        x =>
                        new RealtyObjectDevice
                {
                                TypeCommResourse =
                                    this.GetGridCellValue(
                                        x,
                                        "1",
                                        editors.FirstOrDefault(o => o.Code == 260)),
                                ExistMeterDevice =
                                    this.GetGridCellValue(
                                        x,
                                        "2",
                                        editors.FirstOrDefault(o => o.Code == 270)),
                                InterfaceType =
                                    this.GetGridCellValue(
                                        x,
                                        "3",
                                        editors.FirstOrDefault(o => o.Code == 280)),
                                UnutOfMeasure =
                                    this.GetGridCellValue(
                                        x,
                                        "4",
                                        editors.FirstOrDefault(o => o.Code == 290)),
                                InstallDate = this.GetGridCellValue(x, "5"),
                                CheckDate = this.GetGridCellValue(x, "6"),
                                Number = this.GetGridCellValue(x, "7")
                }).ToList();

                return new BaseDataResult(result);
            }
            finally
            {
	            this.Container.Release(disclosureDomain);
            }
        }

        public class RealtyObjectDevice
        {
            public string Number { get; set; }

            public string TypeCommResourse { get; set; }

            public string ExistMeterDevice { get; set; }

            public string InterfaceType { get; set; }

            public string UnutOfMeasure { get; set; }

            public string InstallDate { get; set; }

            public string CheckDate { get; set; }
        }

        /// <summary>
        /// Получение информации по управлению домом
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult GetRealtyObjectHouseManaging(BaseParams baseParams)
        {
            var disclosureDomain = this.Container.ResolveDomain<DisclosureInfoRealityObj>();
            var disclosureMoDomain = this.Container.ResolveDomain<DisclosureInfo>();
            var manOrgBaseContractDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();
            var diRoId = baseParams.Params.GetAsId("diRoId");
            var diManorgId = baseParams.Params.GetAsId("diManorgId");
            try
            {
                var record = disclosureDomain.Get(diRoId);

                if (record?.RealityObject == null)
                {
                    return BaseDataResult.Error("Не удалось получить запись");
                }

                var diManorg = disclosureMoDomain.Get(diManorgId);

                if (diManorg?.ManagingOrganization == null)
                {
                    return BaseDataResult.Error("Не удалось получить управляющую организацию дома");
                }

                var dateStart = DateTime.MinValue;
                var dateEnd = DateTime.MaxValue;
                var periodDi = record.PeriodDi;
                if (periodDi.IsNotNull())
                {
                    dateStart = periodDi.DateStart ?? DateTime.MinValue;
                    dateEnd = periodDi.DateEnd ?? DateTime.MaxValue;
                }

                var contract = manOrgBaseContractDomain.GetAll()
                    .Where(x => x.RealityObject.Id == record.RealityObject.Id && x.ManOrgContract.ManagingOrganization.Id == diManorg.ManagingOrganization.Id)
                    .Where(x => 
                        (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate.Value <= dateEnd) && 
                        (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value >= dateStart))
                    .Select(x => x.ManOrgContract)
                    .OrderByDescending(x => x.DocumentDate)
                    .FirstOrDefault();

                if (contract.IsNull())
                {
                    return BaseDataResult.Error("Не удалось получить актуальный договор управления");
                }

                string contractFoundation = null,
                    documentName = null;

                if (contract.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK)
                {
                    var manOrgContractOwners = contract as ManOrgContractOwners;
                    if (manOrgContractOwners.IsNotNull())
                    {
                        contractFoundation = manOrgContractOwners.ContractFoundation.GetAttribute<DisplayAttribute>()?.Value;
                        documentName = manOrgContractOwners.FileInfo?.Name;
                    }
                }
                else if (contract.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ)
                {
                    var manOrgJskTsjContract = contract as ManOrgJskTsjContract;
                    if (manOrgJskTsjContract.IsNotNull())
                    {
                        contractFoundation = manOrgJskTsjContract.ContractFoundation.GetAttribute<DisplayAttribute>()?.Value;
                        documentName = manOrgJskTsjContract.FileInfo?.Name;
                    }
                }
                else if (contract.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK)
                {
                    contractFoundation = contract.DocumentName;
                    documentName = contract.DocumentName;
                }

                var result = new RealityObjectHouseManagingInfo
                {
                    DateStart = contract.StartDate,
                    ContractFoundation = contractFoundation,
                    DocumentName = documentName,
                    DocumentDate = contract.DocumentDate,
                    DocumentNumber = contract.DocumentNumber,
                    DateEnd = contract.EndDate,
                    ContractStopReason = contract.ContractStopReason.GetAttribute<DisplayAttribute>()?.Value
                };

                return new BaseDataResult(result);
            }
            finally
            {
                this.Container.Release(disclosureDomain);
                this.Container.Release(manOrgBaseContractDomain);
                this.Container.Release(disclosureMoDomain);
            }
        }

        public class RealityObjectHouseManagingInfo
        {
            public DateTime? DateStart { get; set; }
            public DateTime? DateEnd { get; set; }
            public string ContractFoundation { get; set; }
            public string DocumentName { get; set; }
            public DateTime? DocumentDate { get; set; }
            public string DocumentNumber { get; set; }
            public string ContractStopReason { get; set; }
        }

        /// <summary>
        /// Возвращает значение любой ячейки, по строке, номеру колонки и, если присутствует Editor, то значение из него. 
        /// При отсутствии значения - дефолтное значение из Editor, если его нет - то пустая строка. 
        /// </summary>
        /// <param name="tehPassportRow">Перечисление значений строки</param>
        /// <param name="column">Номер колонки</param>
        /// <param name="editor">Editor, (если значение брать с него)</param>
        /// <returns></returns>
        private string GetGridCellValue(IEnumerable<TehPassportValue> tehPassportRow, string column, EditorTechPassport editor = null)
        {
            var tehPassportValue = tehPassportRow.FirstOrDefault(o => o.CellCode.Split(':')[1] == column);
            var value = tehPassportValue == null ? string.Empty : tehPassportValue.Value;
            int code;

            if (editor != null && int.TryParse(value, out code))
            {
                var keyValue = editor.Values.FirstOrDefault(x => x.Code == value);
                value = keyValue != null ? keyValue.Name : string.Empty;
            }
            else if (editor != null && value.IsNullOrEmpty())
            {
                var keyValue = editor.Values.FirstOrDefault(x => x.Code == "0");
                value = keyValue != null ? keyValue.Name : string.Empty;
            }

            return value;
        }

        private string GetCellValue(IEnumerable<TehPassportValue> tehPassportRow, string formCode, string cellCode, ref IPassportProvider passport)
        {
            TehPassportValue passportValue = tehPassportRow.FirstOrDefault(x => x.FormCode == formCode && x.CellCode == cellCode);
            if (passportValue != null)
            {
                return passport.GetTextForCellValue(formCode, cellCode, passportValue.Value);
            }
            return String.Empty;
        }

        private int? GetCellReformaCode(IEnumerable<TehPassportValue> tehPassportRow, string formCode, string cellCode,
            IPassportProvider passport, IDomainService<TechnicalPassportCompareCode> compareCodeDomain)
        {
            var passportValue = tehPassportRow.FirstOrDefault(x => x.FormCode == formCode && x.CellCode == cellCode);
            if (passportValue == null)
            {
                return null;
            }

            var cellValue = passport.GetCellValue(formCode, cellCode, passportValue.Value);
            if (cellValue == null)
            {
                return null;
            }

            var reformaCodeStr = compareCodeDomain.FirstOrDefault(x => x.FormCode == formCode && x.CellCode == cellCode && x.Value == cellValue)
                ?.CodeReforma;

            int.TryParse(reformaCodeStr, out var result);
            return result == default(int) ? null : (int?)result;
        }


        private static readonly Tuple<string, string> EnergyEfficiency = new Tuple<string, string>("Form_6_1", "1:1");
        private static readonly Tuple<string, string> TehPassportDocumentBasedArea = new Tuple<string, string>("Form_2", "1:3");
        private static readonly Tuple<string, string> TehPassportParkingArea = new Tuple<string, string>("Form_2", "16:3");
        private static readonly Tuple<string, string> TehPassportChildrenArea = new Tuple<string, string>("Form_2", "9:3");
        private static readonly Tuple<string, string> TehPassportSportArea = new Tuple<string, string>("Form_2", "10:3");
        private static readonly Tuple<string, string> TehPassportOtherArea = new Tuple<string, string>("Form_2", "11:3");

        private string GetDataFromPassport(RealityObject ro, Tuple<string, string> key)
        {
            string result = null;

            var techPassportService = this.Container.Resolve<ITechPassportService>();
            var data = techPassportService.GetValue(ro.Id, key.Item1, key.Item2);
            if (data != null)
            {
                result = data.Value;
            }


            return result;
        }

        private CrFundFormationType GetTypeOfFormingCr(RealityObject ro)
        {
            var typeOfFormingCrProvider = this.Container.Resolve<ITypeOfFormingCrProvider>();
            return typeOfFormingCrProvider.GetTypeOfFormingCr(ro);
        }

		/// <summary>
		/// Проверка дома на аварийность
		/// </summary>
		/// <param name="ro">Жилой дом</param>
		/// <returns></returns>
		private YesNo IsEmergencyRealityObject(RealityObject ro)
        {
            var domain = this.Container.ResolveDomain<EmergencyObject>();
            var emergencyInfo =
                (from v in domain.GetAll() where v.RealityObject.Id == ro.Id select v)
                    .FirstOrDefault();

            return (ro.ConditionHouse == ConditionHouse.Emergency || emergencyInfo != null)
                ? YesNo.Yes
                : YesNo.No;
        }

		#endregion

		/// <summary>
		/// Сохранить объект недвижимости деятельности управляющей организации в периоде раскрытия информации
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
		public IDataResult SaveDisclosureInfo(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var realtyObjId = baseParams.Params.GetAs<long>("realtyObjId");
                var manOrgId = baseParams.Params.GetAs<long>("manOrgId");

                var realtyObj = this.Container.Resolve<IDomainService<RealityObject>>().Load(realtyObjId);
                var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().Load(disclosureInfoId);

                // Пробуем получить дом в периоде
                var disclosureInfoRealityObj =
                    this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>()
                        .GetAll()
                        .FirstOrDefault(x =>
                            x.RealityObject.Id == realtyObjId && x.PeriodDi.Id == disclosureInfo.PeriodDi.Id && x.ManagingOrganization.Id == manOrgId);

                if (disclosureInfoRealityObj == null)
                {
                    // Если дома нет то создаем его
                    disclosureInfoRealityObj = new DisclosureInfoRealityObj
                    {
                        Id = 0,
                        PeriodDi = disclosureInfo.PeriodDi,
                        RealityObject = realtyObj,
                        ReductionPayment = YesNoNotSet.NotSet,
                        NonResidentialPlacement = YesNoNotSet.NotSet,
                        PlaceGeneralUse = YesNoNotSet.NotSet,
                        ManagingOrganization = new ManagingOrganization { Id = manOrgId}
                    };
                    this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>().Save(disclosureInfoRealityObj);

                    // Так же сразу создаем сущность документов по данному дому
                    var documentsRealityObj = new DocumentsRealityObj
                    {
                        Id = 0,
                        DisclosureInfoRealityObj = disclosureInfoRealityObj
                    };
                    this.Container.Resolve<IDomainService<DocumentsRealityObj>>().Save(documentsRealityObj);
                }

                // Если дом есть то вероятно он уже в пользовании другой УК поэтому создаем новую связку
                var disclosureInfoRelation = new DisclosureInfoRelation
                {
                    Id = 0,
                    DisclosureInfo = disclosureInfo,
                    DisclosureInfoRealityObj = disclosureInfoRealityObj
                };
                this.Container.Resolve<IDomainService<DisclosureInfoRelation>>().Save(disclosureInfoRelation);

                return
                    new BaseDataResult(
                        new
                            {
                                disclosureInfoRealityObj.Id,
                                disclosureInfoId,
                                realtyObjId,
                                Address = realtyObj.FiasAddress != null ? realtyObj.FiasAddress.AddressName : string.Empty
                            });
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}
