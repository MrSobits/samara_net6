namespace Bars.GkhDi.PercentCalculationProvider
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    public partial class PercentCalculation988
    {
        /// <summary>
        /// Режим работы
        /// </summary>
        protected class WorkModeProxy
        {
            /// <summary>
            /// Присутствует информация о режиме работы
            /// </summary>
            public bool WorkMode { get; set; }

            /// <summary>
            /// Присутствует информация о времени приёма граждан
            /// </summary>
            public bool ReceptionCitizens { get; set; }

            /// <summary>
            /// Присутствует информация о времени работы диспетчерской службы
            /// </summary>
            public bool DispatcherWork { get; set; }
        }

        /// <summary>
        /// Финансовая деятельность
        /// </summary>
        protected class FinActivityProxy
        {
            /// <summary>
            /// Система налогообложения
            /// </summary>
            public virtual TaxSystem TaxSystem { get; set; }

            /// <summary>
            /// Тип УО
            /// </summary>
            public TypeManagementManOrg MoTypeManagement { get; set; }
        }

        /// <summary>
        /// Документы финансовой деятельности
        /// </summary>
        protected class FinActivityDocsProxy
        {
            /// <summary>
            /// Бухгалтерский баланс
            /// </summary>
            public FileInfo BookkepingBalance { get; set; }

            /// <summary>
            /// Приложение к бухгалтерскому балансу
            /// </summary>
            public FileInfo BookkepingBalanceAnnex { get; set; }
        }

        /// <summary>
        /// Аудиторская проверка финансовой деятельности
        /// </summary>
        protected class FinActivityAuditDocsProxy
        {
            /// <summary>
            /// Год проверки
            /// </summary>
            public int Year { get; set; }

            /// <summary>
            /// Тип состояния проверки
            /// </summary>
            public TypeAuditStateDi TypeAuditStateDi { get; set; }

            /// <summary>
            /// Файл
            /// </summary>
            public FileInfo File { get; set; }
        }

        /// <summary>
        /// Документы фин деятельности (сметы доходов и Заключение рев коммиссии) в разрезе по годам
        /// </summary>
        protected class FinActivityDocsByYearProxy
        {
            /// <summary>
            /// Год
            /// </summary>
            public int Year { get; set; }

            /// <summary>
            /// Тип документа по годам
            /// </summary>
            public TypeDocByYearDi TypeDocByYearDi { get; set; }
        }

        /// <summary>
        /// Управление домами финансовой деятельности
        /// </summary>
        protected class FinActivityRealityObjectProxy
        {
            /// <summary>
            /// Сумма задолжности
            /// </summary>
            public decimal? SumDebt { get; set; }

            /// <summary>
            /// Сумма фактических расходов
            /// </summary>
            public decimal? SumFactExpense { get; set; }

            /// <summary>
            /// Сумма дохода от управления
            /// </summary>
            public decimal? SumIncomeManage { get; set; }

            /// <summary>
            /// Идентификатор объекта недвижимости
            /// </summary>
            public long? RoId { get; set; }
        }

        /// <summary>
        /// Коммунальные услуги
        /// </summary>
        protected class FinActivityCommunalServiceProxy
        {
            /// <summary>
            /// Услуга
            /// </summary>
            public TypeServiceDi TypeServiceDi { get; set; }

            /// <summary>
            /// Задолженность упр орг за коммунальные услуги
            /// </summary>
            public decimal? DebtManOrgCommunalService { get; set; }
        }

        /// <summary>
        /// Жилой дом в периоде раскрытия информации
        /// </summary>
        protected class DisclosureInfoRealityObjProxy
        {
            /// <summary>
            /// Идентификатор сущности в период раскрытия информации
            /// </summary>
            public long Id { get; set; }
            /// <summary>
            /// Идентификатор дома
            /// </summary>
            public long RoId { get; set; }
            /// <summary>
            /// Дата постройки
            /// </summary>
            public int? BuildYear { get; set; }
            /// <summary>
            /// Дата сдачи в эксплуатацию
            /// </summary>
            public DateTime? DateCommissioning { get; set; }
            /// <summary>
            /// Тип проекта
            /// </summary>
            public string TypeProject { get; set; }
            /// <summary>
            /// Тип дома
            /// </summary>
            public TypeHouse TypeHouse { get; set; }
            /// <summary>
            /// Количество этажей
            /// </summary>
            public int? Floors { get; set; }
            /// <summary>
            /// Максимальное количество этажей
            /// </summary>
            public int? MaximumFloors { get; set; }
            /// <summary>
            /// Количество входов
            /// </summary>
            public int? NumberEntrances { get; set; }
            /// <summary>
            /// Количество лифтов
            /// </summary>
            public int? NumberLifts { get; set; }
            /// <summary>
            /// Количество жилых помещений
            /// </summary>
            public int? NumberApartments { get; set; }
            /// <summary>
            /// Количество нежилых помещений
            /// </summary>
            public int? NumberNonResidentialPremises { get; set; }
            /// <summary>
            /// Количество помещений всего
            /// </summary>
            public int? AllNumberOfPremises
            {
                get
                {
                    if (!this.NumberApartments.HasValue && !this.NumberNonResidentialPremises.HasValue)
                    {
                        return null;
                    }

                    return (this.NumberApartments ?? 0) + (this.NumberNonResidentialPremises ?? 0);
                }
            } 
            /// <summary>
            /// Общая площадь МКД (кв.м.)
            /// </summary>
            public decimal? AreaMkd { get; set; }
            /// <summary>
            /// Площадь жилая, всего (кв. м.)
            /// </summary>
            public decimal? AreaLiving { get; set; }
            /// <summary>
            /// Площадь нежилых помещений
            /// </summary>
            public decimal? AreaNotLivingPremises { get; set; }
            /// <summary>
            /// В т.ч. нежилых, функционального назначения (кв.м.)
            /// </summary>
            public decimal? AreaOfAllNotLivingPremises { get; set; }
            /// <summary>
            /// Адрес МКД
            /// </summary>
            public virtual FiasAddress FiasAddress { get; set; }
            /// <summary>
            /// Материал кровли
            /// </summary>
            public string RoofingMaterial { get; set; }
            /// <summary>
            /// Договоры на использование мест общего пользования
            /// </summary>
            public YesNoNotSet PlaceGeneralUse { get; set; }
            /// <summary>
            /// Способ формирования фонда КР
            /// </summary>
            public CrFundFormationType? AccountFormationVariant { get; set; }
            /// <summary>
            /// Сведения о случая снижения платы
            /// </summary>
            public YesNoNotSet ReductionPayment { get; set; }
            /// <summary>
            /// Данные об использование нежилых помещений
            /// </summary>
            public YesNoNotSet NonResidentialPlacement { get; internal set; }
            /// <summary>
            /// Состояние дома
            /// </summary>
            public ConditionHouse ConditionHouse { get; set; }

            #region Финансовые показатели  по содержанию и текущему ремонту

            /// <summary>
            /// Авансовые платежи на старт периода
            /// </summary>
            public virtual decimal? AdvancePayments { get; set; }

            /// <summary>
            /// переходящие остатки средств
            /// </summary>
            public virtual decimal? CarryOverFunds { get; set; }

            /// <summary>
            ///  задолженность на начало периода
            /// </summary>
            public virtual decimal? Debt { get; set; }

            /// <summary>
            /// начисление за  содержание
            /// </summary>
            public virtual decimal? ChargeForMaintenanceAndRepairsMaintanance { get; set; }

            /// <summary>
            /// начисление за текущий ремонт
            /// </summary>
            public virtual decimal? ChargeForMaintenanceAndRepairsRepairs { get; set; }

            /// <summary>
            /// начисление за управление
            /// </summary>
            public virtual decimal? ChargeForMaintenanceAndRepairsManagement { get; set; }

            /// <summary>
            /// начисление всего
            /// </summary>
            public virtual decimal? ChargeForMaintenanceAndRepairsAll { get; set; }

            /// <summary>
            /// Всего получено денежных средств от владельцев
            /// </summary>
            public virtual decimal? ReceivedCashFromOwners { get; set; }

            /// <summary>
            /// Всего получено денежных средств от владельцев на целевые
            /// </summary>
            public virtual decimal? ReceivedCashFromOwnersTargeted { get; set; }

            /// <summary>
            /// Всего получено денежных средств как субсидии
            /// </summary>
            public virtual decimal? ReceivedCashAsGrant { get; set; }

            /// <summary>
            /// Получено средств за использование общей собственности
            /// </summary>
            public virtual decimal? ReceivedCashFromUsingCommonProperty { get; set; }

            /// <summary>
            /// Получено средств как другие типы платежей
            /// </summary>
            public virtual decimal? ReceivedCashFromOtherTypeOfPayments { get; set; }

            /// <summary>
            /// Средства на балансе по окончанию периода
            /// </summary>
            public virtual decimal? CashBalanceAll { get; set; }

            /// <summary>
            /// Авансовые платежи потребителей на конец периода
            /// </summary>
            public virtual decimal? CashBalanceAdvancePayments { get; set; }

            /// <summary>
            /// Переходящие остатки денежных средств на конец периода
            /// </summary>
            public virtual decimal? CashBalanceCarryOverFunds { get; set; }

            /// <summary>
            /// Задолженность потребителей на конец периода
            /// </summary>
            public virtual decimal? CashBalanceDebt { get; set; }

            /// <summary>
            /// Всего получено денежных средств
            /// </summary>
            public virtual decimal? ReceivedCashAll { get; set; }

            #endregion

            #region Финансовые показатели - Коммунальные услуги

            /// <summary>
            /// Коммунальные услуги - Авансовые платежи на начало периода
            /// </summary>
            public virtual decimal? ComServStartAdvancePay { get; set; }

            /// <summary>
            /// Коммунальные услуги - Переходящие остатки средств на начало периода
            /// </summary>
            public virtual decimal? ComServStartCarryOverFunds { get; set; }

            /// <summary>
            ///  Коммунальные услуги - Задолженность на начало периода
            /// </summary>
            public virtual decimal? ComServStartDebt { get; set; }

            /// <summary>
            /// Коммунальные услуги - Авансовые платежи на конец периода
            /// </summary>
            public virtual decimal? ComServEndAdvancePay { get; set; }

            /// <summary>
            /// Коммунальные услуги - Переходящие остатки средств на конец периода
            /// </summary>
            public virtual decimal? ComServEndCarryOverFunds { get; set; }

            /// <summary>
            ///  Коммунальные услуги - Задолженность на конец периода
            /// </summary>
            public virtual decimal? ComServEndDebt { get; set; }

            /// <summary>
            /// Количество поступивших претензий
            /// </summary>
            public virtual int? ComServReceivedPretensionCount { get; set; }

            /// <summary>
            /// Количество удовлетворенных претензий
            /// </summary>
            public virtual int? ComServApprovedPretensionCount { get; set; }

            /// <summary>
            /// Количество поступивших претензий
            /// </summary>
            public virtual int? ComServNoApprovedPretensionCount { get; set; }

            /// <summary>
            /// Сумма произведенного перерасчета
            /// </summary>
            public virtual decimal? ComServPretensionRecalcSum { get; set; }

            #endregion Финансовые показатели - Коммунальные услуги

            #region Претензии по качеству работ

            /// <summary>
            /// Количество поступивших претензий
            /// </summary>
            public virtual int? ReceivedPretensionCount { get; set; }

            /// <summary>
            /// Количество удовлетворенных претензий
            /// </summary>
            public virtual int? ApprovedPretensionCount { get; set; }

            /// <summary>
            /// Количество поступивших претензий
            /// </summary>
            public virtual int? NoApprovedPretensionCount { get; set; }

            /// <summary>
            /// Сумма произведенного перерасчета
            /// </summary>
            public virtual decimal? PretensionRecalcSum { get; set; }

            #endregion Претензии по качеству работ

            #region Претензионно-исковая работа

            /// <summary>
            /// Направлено претензий потребителям-должникам
            /// </summary>
            public virtual int? SentPretensionCount { get; set; }

            /// <summary>
            /// Направлено исковых заявлений
            /// </summary>
            public virtual int? SentPetitionCount { get; set; }

            /// <summary>
            /// Получено денежных средств по результатам претензионно-исковой работы 
            /// </summary>
            public virtual decimal? ReceiveSumByClaimWork { get; set; }
            #endregion Претензионно-исковая работа
        }

        /// <summary>
        /// Конструктивные элементы техпаспорта
        /// </summary>
        protected class StructElementsTechPassportProxy
        {
            /// <summary>
            /// Заполнен тип фундамента
            /// </summary>
            public bool HasBasementType { get; set; }
            /// <summary>
            /// Заполнена площадь подвала
            /// </summary>
            public bool HasBasementArea { get; set; }
            /// <summary>
            /// Заполнен материал несущих стен
            /// </summary>
            public bool HasTypeWalls { get; set; }
            /// <summary>
            /// Заполнен тип перекрытий
            /// </summary>
            public bool HasTypeFloor { get; set; }
            /// <summary>
            /// Заполнен тип мусоропровода
            /// </summary>
            public bool HasConstructionChute { get; set; }
            /// <summary>
            /// Количество мусопроводов
            /// </summary>
            public int? ChutesNumber { get; set; }
            /// <summary>
            /// Заполнен тип фасада
            /// </summary>
            public bool HasFacade { get; set; }
        }

        /// <summary>
        /// Лифт жилого дома
        /// </summary>
        protected class LiftDiProxy
        {
            /// <summary>
            /// Номер подъезда
            /// </summary>
             public string EntranceNumber { get; set; }

            /// <summary>
            /// Год ввода в эксплуатацию
            /// </summary>
             public string CommissioningYear { get; set; }

            /// <summary>
            /// Тип лифта
            /// </summary>
            public string Type { get; set; }
        }

        /// <summary>
        /// Прибор учёта
        /// </summary>
        protected class MeteringDeviceDiProxy
        {
            /// <summary>
            /// Наличие прибора учёта
            /// </summary>
            public int? ExistMeterDevice { get; set; }

            /// <summary>
            /// Тип интерфейса
            /// </summary>
            public int? InterfaceType { get; set; }

            /// <summary>
            /// Единица измерения
            /// </summary>
            public int? UnutOfMeasure { get; set; }

            /// <summary>
            /// Дата ввода в эксплуатацию
            /// </summary>
            public DateTime? InstallDate { get; set; }

            /// <summary>
            /// Дата проверки/замены
            /// </summary>
            public DateTime? CheckDate { get; set; }
        }

        /// <summary>
        /// Коммунальная услуга
        /// </summary>
        protected class CommunalServiceDiProxy
        {
            /// <summary>
            /// Единица измерения
            /// </summary>
            public string UnitMeasure { get; set; }

            /// <summary>
            /// Цена закупаемых ресурсов
            /// </summary>
            public virtual decimal? PricePurchasedResources { get; set; }

            /// <summary>
            /// Норматив потребления коммунальной услуги в жилых помещениях.
            /// </summary>
            public virtual decimal? ConsumptionNormLivingHouse { get; set; }
            /// <summary>
            /// Единица изменения норматива потребления коммунальной услуги в жилых помещениях.
            /// </summary>
            public virtual string UnitMeasureLivingHouseName { get; set; }

            /// <summary>
            /// Тип оказания услуг
            /// </summary>
            public TypeOfProvisionServiceDi ProvisionService { get; set; }
        }

        /// <summary>
        /// Тарифы для РСО (ресурсоснабжающей организации)
        /// </summary>
        protected class TariffForRsoDiProxy
        {
            /// <summary>
            /// Дата нормативно правового акта
            /// </summary>
            public DateTime? DateNormativeLegalAct { get; set; }

            /// <summary>
            /// Дата начала действия
            /// </summary>
            public DateTime? DateStart { get; internal set; }

            /// <summary>
            /// Номер нормативно правового акта
            /// </summary>
            public string NumberNormativeLegalAct { get; set; }

            /// <summary>
            /// Орган, установивший тариф
            /// </summary>
            public string OrganizationSetTariff { get; internal set; }
        }

        protected class ServiceProxy988 : ServiceProxy
        {
            public decimal? TariffForConsumers { get; set; }

            public YesNoNotSet ScheduledPreventiveMaintanance { get; set; }
        }

        /// <summary>
        /// Сведения об использовании мест общего пользования
        /// </summary>
        protected class InfoAboutUseCommonFacilitiesProxy
        {
            /// <summary>
            /// Вид общего имущества
            /// </summary>
            public string KindCommomFacilities { get; set; }

            /// <summary>
            /// Назначение общего имущества
            /// </summary>
            public string AppointmentCommonFacilities { get; set; }

            /// <summary>
            /// Площадь общего имущества
            /// </summary>
            public decimal? AreaOfCommonFacilities { get; set; }

            /// <summary>
            /// ОГРН
            /// </summary>
            public string Ogrn { get; set; }

            /// <summary>
            /// ИНН
            /// </summary>
            public string Inn { get; set; }

            /// <summary>
            /// Номер договора
            /// </summary>
            public string ContractNumber { get; set; }

            /// <summary>
            /// Дата договора
            /// </summary>
            public virtual DateTime? ContractDate { get; set; }

            /// <summary>
            /// Стоимость по договору в месяц (руб.)
            /// </summary>
            public virtual decimal? CostByContractInMonth { get; set; }

            /// <summary>
            /// Тип арендатора
            /// </summary>
            public LesseeTypeDi LesseeTypeDi { get; set; }
        }

        /// <summary>
        /// Документы сведений об УО объекта недвижимости
        /// </summary>
        protected class DocumentsRealityObjDiProxy
        {
            /// <summary>
            /// Акт состояния общего имущества собственников в многоквартирном доме. Файл
            /// </summary>
            public bool HasFileActState { get; set; }

            /// <summary>
            /// Акт состояния общего имущества собственников в многоквартирном доме. Описание
            /// </summary>
            public string DescriptionActState { get; set; }

            /// <summary>
            /// Перечень работ по содержанию и ремонту. Файл
            /// </summary>
            public bool HasFileCatalogRepair { get; set; }

            /// <summary>
            /// Отчет о выполнение годового плана мероприятий по содержанию и ремонту. Файл
            /// </summary>
            public bool HasFileReportPlanRepair { get; set; }

            /// <summary>
            /// Проводились ли общие собрания собственников помещений в МКД с участием УО
            /// </summary>
            public YesNoNotSet HasGeneralMeetingOfOwners { get; set; }
        }

        protected class EngineerSystemsProxy
        {
            /// <summary>
            /// Тип системы теплоснабжения
            /// <para>"Form_3_1", "1:3"</para>
            /// </summary>
            public bool HasHeatingForm { get; set; }

            /// <summary>
            /// Тип системы горячего водоснабжения
            /// <para>"Form_3_2", "1:3"</para>
            /// </summary>
            public bool HasHotWaterForm { get; set; }

            /// <summary>
            /// Тип системы холодного водоснабжения
            /// <para>"Form_3_2_CW", "1:3"</para>
            /// </summary>
            public bool HasColdWaterForm { get; set; }

            /// <summary>
            /// Тип системы газоснабжения
            /// <para>"Form_3_4", "1:3"</para>
            /// </summary>
            public bool HasGasForm { get; set; }

            /// <summary>
            /// Тип системы вентиляции
            /// <para>"Form_3_5", "1:3"</para>
            /// </summary>
            public bool HasVentilationForm { get; set; }

            /// <summary>
            /// Тип системы пожаротушения
            /// <para>"Form_3_8", "1:3"</para>
            /// </summary>
            public bool HasFirefightingForm { get; set; }

            /// <summary>
            /// Тип системы водостоков
            /// <para>"Form_3_6", "1:3"</para>
            /// </summary>
            public bool HasDrainageForm { get; set; }

            /// <summary>
            /// Тип системы электроснабжения
            /// <para>"Form_3_3", "1:3"</para>
            /// </summary>
            public bool HasPowerForm { get; set; }

            /// <summary>
            /// Кол-во вводов в дом
            /// <para>"Form_3_3_3", "15:1"</para>
            /// </summary>
            public bool HasPowerPointsForm { get; set; }

            /// <summary>
            /// Тип системы водоотведения
            /// <para>"Form_3_3_Water", "1:3"</para>
            /// </summary>
            public bool HasSewageForm { get; set; }

            public static EngineerSystemsProxy InitEngineerSystemsProxy(EngineerSystemsProxy proxy, string formCode, string cellCode, string value)
            {
                var result = !string.IsNullOrWhiteSpace(value) && value != "Не задано";
                if (cellCode == "1:3")
                {
                    switch (formCode)
                    {
                        case "Form_3_1": proxy.HasHeatingForm = result; break;
                        case "Form_3_2": proxy.HasHotWaterForm = result; break;
                        case "Form_3_2_CW": proxy.HasColdWaterForm = result; break;
                        case "Form_3_4": proxy.HasGasForm = result; break;
                        case "Form_3_5": proxy.HasVentilationForm = result; break;
                        case "Form_3_8": proxy.HasFirefightingForm = result; break;
                        case "Form_3_6": proxy.HasDrainageForm = result; break;
                        case "Form_3_3": proxy.HasPowerForm = result; break;
                        case "Form_3_3_Water": proxy.HasSewageForm = result; break;
                    }
                }
                if (formCode == "Form_3_3_3" && cellCode == "15:1")
                {
                    proxy.HasPowerPointsForm = result;
                }

                return proxy;
            }
        }
    }
}
