namespace Bars.GkhDi.Permissions
{
    using B4;
    using Entities;

    public class GkhDiPermissionMap : PermissionMap
    {
        public GkhDiPermissionMap()
        {
            this.Namespace("GkhDi", "Модуль раскрытия информации УК");

            #region Справочники

            this.Namespace("GkhDi.Dict", "Справочники");
            this.Namespace("GkhDi.Dict.PeriodDi", "Отчетные периоды");

            this.CRUDandViewPermissions("GkhDi.Dict.PeriodDi");

            this.Namespace("GkhDi.Dict.TemplateService", "Услуги");
            this.CRUDandViewPermissions("GkhDi.Dict.TemplateService");

            this.Namespace("GkhDi.Dict.TemplateOtherService", "Прочие услуги из сторонних систем");
            this.CRUDandViewPermissions("GkhDi.Dict.TemplateOtherService");

            this.Namespace("GkhDi.Dict.PeriodicityTempServ", "Периодичность услуг");
            this.CRUDandViewPermissions("GkhDi.Dict.PeriodicityTempServ");

            this.Namespace("GkhDi.Dict.SupervisoryOrg", "Контролирующие органы");
            this.CRUDandViewPermissions("GkhDi.Dict.SupervisoryOrg");

            this.Namespace("GkhDi.Dict.TaxSystem", "Система налогообложения");
            this.CRUDandViewPermissions("GkhDi.Dict.TaxSystem");

            this.Namespace("GkhDi.Dict.GroupWorkTo", "Группы работ по ТО");
            this.CRUDandViewPermissions("GkhDi.Dict.GroupWorkTo");

            this.Namespace("GkhDi.Dict.GroupPpr", "Группы ППР");
            this.CRUDandViewPermissions("GkhDi.Dict.GroupPpr");

            this.Namespace("GkhDi.Dict.Ppr", "Планово-предупредительные работы");
            this.CRUDandViewPermissions("GkhDi.Dict.Ppr");

            this.Namespace("GkhDi.Dict.WorkTo", "Отчетные периоды");
            this.CRUDandViewPermissions("GkhDi.Dict.WorkTo");

            #endregion Справочники

            #region Импорт

            this.Namespace("GkhDi.Import", "Импорт");

            this.Namespace("GkhDi.Import.DisclosureInfoImport", "Импорт раскрытия информации");
            this.Permission("GkhDi.Import.DisclosureInfoImport.View", "Просмотр");
            this.Permission("GkhDi.Import.DisclosureInfoImport.Active", "Активность");

            this.Namespace("GkhDi.Import.CommunalPay", "Импорт из комплат");

            this.Permission("GkhDi.Import.CommunalPay.View", "Просмотр");
            this.Permission("GkhDi.MassPercCalc", "Массовый расчет процентов");
            this.Permission("GkhDi.MassGenerateReport", "Массовая генерация отчета по 731 (988) ПП РФ");
            this.Permission("GkhDi.View", "Просмотр");

            #endregion Импорт

            #region Сведения об УО

            this.Namespace<DisclosureInfo>("GkhDi.Disinfo", "Сведения об УО");
            this.Permission("GkhDi.Disinfo.PercCalc", "Расчет процентов");

            this.Namespace("GkhDi.Disinfo.TerminateContract", "Сведения о расторгнутых договорах");
            this.Permission("GkhDi.Disinfo.TerminateContract.TerminateContractField", "Случаи расторжения договоров управления в данном отчетном периоде");
            this.Permission("GkhDi.Disinfo.TerminateContract.ManagingRealityObjBtn", "Управление жилым домом");

            this.Namespace("GkhDi.Disinfo.FinActivity", "Финансовая деятельность");

            this.Namespace("GkhDi.Disinfo.FinActivity.GeneralData", "Общие сведения");

            this.Namespace("GkhDi.Disinfo.FinActivity.GeneralData.Fields", "Поля");

            this.Permission("GkhDi.Disinfo.FinActivity.GeneralData.Fields.TaxSystem", "Система налогооблажения");

            this.Namespace("GkhDi.Disinfo.FinActivity.GeneralData.Audit", "Аудиторские проверки");
            this.Permission("GkhDi.Disinfo.FinActivity.GeneralData.Audit.Add", "Добавление");
            this.Permission("GkhDi.Disinfo.FinActivity.GeneralData.Audit.Delete", "Удаление");
            this.Permission("GkhDi.Disinfo.FinActivity.GeneralData.Audit.Edit", "Изменение");

            this.Namespace("GkhDi.Disinfo.FinActivity.Docs", "Документы");
            this.Permission("GkhDi.Disinfo.FinActivity.Docs.Save", "Изменение");

            this.Namespace("GkhDi.Disinfo.FinActivity.Docs.ByYear", "Документы в разсрезе годов");
            this.Permission("GkhDi.Disinfo.FinActivity.Docs.ByYear.Add", "Добавление");
            this.Permission("GkhDi.Disinfo.FinActivity.Docs.ByYear.Delete", "Удаление");
            this.Permission("GkhDi.Disinfo.FinActivity.Docs.ByYear.Edit", "Изменение");

            this.Namespace("GkhDi.Disinfo.FinActivity.ManagRealityObj", "Управление домами");
            this.Permission("GkhDi.Disinfo.FinActivity.ManagRealityObj.PresentedToRepayColumn", "Предъявлено к оплате");
            this.Permission("GkhDi.Disinfo.FinActivity.ManagRealityObj.ReceivedProvidedServiceColumn", "Получено за предоставленные услуги");
            this.Permission("GkhDi.Disinfo.FinActivity.ManagRealityObj.SumDebtColumn", "Сумма задолжености");

            this.Namespace("GkhDi.Disinfo.FundsInfo", "Сведения о фондах");
            this.Permission("GkhDi.Disinfo.FundsInfo.Add", "Добавление");
            this.Permission("GkhDi.Disinfo.FundsInfo.Edit", "Изменение");
            this.Permission("GkhDi.Disinfo.FundsInfo.FundsInfoField", "Действующие договоры за отчетный период имеются");

            this.Namespace("GkhDi.Disinfo.InformationOnContracts", "Сведения о договорах");
            this.Permission("GkhDi.Disinfo.InformationOnContracts.Add", "Добавление");
            this.Permission("GkhDi.Disinfo.InformationOnContracts.Edit", "Изменение");
            this.Permission("GkhDi.Disinfo.InformationOnContracts.InformationOnContractsField", "Действующие договоры за отчетный период имеются");

            this.Namespace("GkhDi.Disinfo.AdminResp", "Административная ответственность");
            this.Permission("GkhDi.Disinfo.AdminResp.Add", "Добавление");
            this.Permission("GkhDi.Disinfo.AdminResp.Delete", "Удаление");
            this.Permission("GkhDi.Disinfo.AdminResp.Edit", "Изменение");
            this.Permission("GkhDi.Disinfo.AdminResp.AdminRespField", "Случаи привлечения к административной ответственности	");

            this.Namespace("GkhDi.Disinfo.License", "Лицензии");
            this.CRUDandViewPermissions("GkhDi.Disinfo.License");

            this.Namespace("GkhDi.Disinfo.Docs", "Документы");
            this.Permission("GkhDi.Disinfo.Docs.Edit", "Изменение");
            this.Permission("GkhDi.Disinfo.Docs.AddCopy", "Добавить копированием");

            #endregion

            #region Объекты в управлении

            this.Namespace<DisclosureInfo>("GkhDi.DisinfoRealObj", "Объекты в управлении");
            this.Namespace("GkhDi.DisinfoRealObj.Actions", "Действия");
            this.Permission("GkhDi.DisinfoRealObj.Actions.CopyServicePeriod", "Копирование сведений об услугах из периода в период");
            this.Permission("GkhDi.DisinfoRealObj.Actions.LoadWorkPprGroupAction", "Заполнение сведений по работам по текущему ремонту");

            this.Namespace("GkhDi.DisinfoRealObj.Service", "Сведения об услугах");
            this.Permission("GkhDi.DisinfoRealObj.Service.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.Service.Delete", "Удаление");
            this.Permission("GkhDi.DisinfoRealObj.Service.Edit", "Изменение");
            this.Permission("GkhDi.DisinfoRealObj.Service.Copy", "Копирование");

            this.Namespace("GkhDi.DisinfoRealObj.Service.Communal", "Коммунальная услуга");
            this.Permission("GkhDi.DisinfoRealObj.Service.Communal.TypeOfProvisionService", "Тип предоставления услуги");
            this.Permission("GkhDi.DisinfoRealObj.Service.Communal.Provider", "Поставщик");
            this.Permission("GkhDi.DisinfoRealObj.Service.Communal.VolumePurchasedResources", "Объем закупаемых ресурсов");
            this.Permission("GkhDi.DisinfoRealObj.Service.Communal.PricePurchasedResources", "Цена закупаемых ресурсов");
            this.Permission("GkhDi.DisinfoRealObj.Service.Communal.KindElectricitySupply", "Вид электроснабжения");
            this.Permission("GkhDi.DisinfoRealObj.Service.Communal.ConsumptionNormsPanel", "Нормативы потребления");
            this.Permission("GkhDi.DisinfoRealObj.Service.Communal.ConsumptionNormsNpaGrid", "НПА нормативов потребления");

            this.Namespace("GkhDi.DisinfoRealObj.Service.Housing", "Жилищная услуга");
            this.Permission("GkhDi.DisinfoRealObj.Service.Housing.TypeOfProvisionService", "Тип предоставления услуги");
            this.Permission("GkhDi.DisinfoRealObj.Service.Housing.Provider", "Поставщик");
            this.Permission("GkhDi.DisinfoRealObj.Service.Housing.Periodicity", "Периодичность выполнения");
            this.Permission("GkhDi.DisinfoRealObj.Service.Housing.Equipment", "Оборудование");

            this.Namespace("GkhDi.DisinfoRealObj.Service.Repair", "Ремонт услуга");
            this.Permission("GkhDi.DisinfoRealObj.Service.Repair.TypeOfProvisionService", "Тип предоставления услуги");
            this.Permission("GkhDi.DisinfoRealObj.Service.Repair.Provider", "Поставщик");
            this.Permission("GkhDi.DisinfoRealObj.Service.Repair.UnitMeasure", "Ед. измерения");

            this.Namespace("GkhDi.DisinfoRealObj.Service.CapRepair", "Кап. ремонт услуга");
            this.Permission("GkhDi.DisinfoRealObj.Service.CapRepair.TypeOfProvisionService", "Тип предоставления услуги");
            this.Permission("GkhDi.DisinfoRealObj.Service.CapRepair.Provider", "Поставщик");
            this.Permission("GkhDi.DisinfoRealObj.Service.CapRepair.RegionalFund", "Ед. измерения");
            this.Permission("GkhDi.DisinfoRealObj.Service.CapRepair.AddWork", "Добавление работ");

            this.Namespace("GkhDi.DisinfoRealObj.Service.Control", "Управление МКД услуга");
            this.Permission("GkhDi.DisinfoRealObj.Service.Control.Provider", "Поставщик");
            this.Permission("GkhDi.DisinfoRealObj.Service.Control.UnitMeasure", "Ед. измерения");

            this.Namespace("GkhDi.DisinfoRealObj.Service.Additional", "Дополнительная услуга");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.Periodicity", "Периодичность выполнения");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.Provider", "Поставщик");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.OGRN", "ОГРН");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.DateRegistration", "Дата регистрации");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.Document", "Документ");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.Number", "Номер");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.DateFrom", "От(дата документа)");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.DateStart", "Дата начала");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.DateEnd", "Дата окончания");
            this.Permission("GkhDi.DisinfoRealObj.Service.Additional.Sum", "Ед. измерения");

            this.Namespace("GkhDi.DisinfoRealObj.PlanWorkServiceRepair", "План работ по содержанию и ремонту");
            this.Permission("GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Delete", "Удаление");

            this.Namespace("GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Work", "Работы");
            this.Permission("GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Work.Save", "Изменение записи");
            this.Permission("GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Work.Delete", "Удаление записи");
            this.Permission("GkhDi.DisinfoRealObj.PlanWorkServiceRepair.Work.Load", "Заполнить из сведений об услугах");

            this.Namespace("GkhDi.DisinfoRealObj.PlanReductionExpense", "План мер по снижению расходов");
            this.Permission("GkhDi.DisinfoRealObj.PlanReductionExpense.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.PlanReductionExpense.Delete", "Удаление");           
            this.Permission("GkhDi.DisinfoRealObj.PlanReductionExpense.View", "Просмотр");

            this.Namespace("GkhDi.DisinfoRealObj.PlanReductionExpense.Details", "Меры по снижению расходов");
            this.Permission("GkhDi.DisinfoRealObj.PlanReductionExpense.Details.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.PlanReductionExpense.Details.Delete", "Удаление");
            this.Permission("GkhDi.DisinfoRealObj.PlanReductionExpense.Details.Edit", "Изменение");

            this.Namespace("GkhDi.DisinfoRealObj.InfoAboutReductionPayment", "Сведения о случаях снижения платы");

            this.Namespace("GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Field", "Поля");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Field.OrderNum_View", "Номер приказа");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Field.OrderDate_View", "Дата приказа");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Delete", "Удаление");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Edit", "Изменение");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutReductionPayment.PaymentReductionField", "Случаи снижения платы");

            this.Namespace("GkhDi.DisinfoRealObj.InfoAboutPaymentCommunal", "Сведения об оплатах коммунальных услуг");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutPaymentCommunal.View", "Просмотр");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutPaymentCommunal.Edit", "Изменение");

            this.Namespace("GkhDi.DisinfoRealObj.InfoAboutPaymentHousing", "Сведения об оплатах жилищных услуг");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutPaymentHousing.View", "Просмотр");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutPaymentHousing.Edit", "Изменение");

            this.Namespace("GkhDi.DisinfoRealObj.NonResidentialPlacement", "Сведения об использовании нежилых помещений");
            this.Permission("GkhDi.DisinfoRealObj.NonResidentialPlacement.NonResidentialPlacement", "Наличие данных об использовании нежилых помещений");
            this.Permission("GkhDi.DisinfoRealObj.NonResidentialPlacement.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.NonResidentialPlacement.Delete", "Удаление");
            this.Permission("GkhDi.DisinfoRealObj.NonResidentialPlacement.Edit", "Изменение");

            this.Namespace("GkhDi.DisinfoRealObj.NonResidentialPlacement.Devices", "Приборы учета");
            this.Permission("GkhDi.DisinfoRealObj.NonResidentialPlacement.Devices.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.NonResidentialPlacement.Devices.Delete", "Удаление");
            this.Permission("GkhDi.DisinfoRealObj.NonResidentialPlacement.Devices.Edit", "Изменение");

            this.Namespace("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities", "Сведения об использовании мест общего пользования");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.PlaceGeneralUse", "Наличие данных об использовании мест общего пользования");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Delete", "Удаление");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Edit", "Изменение");

            this.Namespace("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field", "Поля");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.Patronymic", "Отчество");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.Comment", "Комментарий");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.SigningContractDate", "Дата подписания договора");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.DayMonthPeriodIn", "День месяца начало периода");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.DayMonthPeriodOut", "День месяца окончания периода");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.IsLastDayMonthPeriodIn", "Последний день месяца начало периода");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.IsLastDayMonthPeriodOut", "Последний день месяца окончания периода");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.IsNextMonthPeriodIn", "День следующего месяца начало периода");
            this.Permission("GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.IsNextMonthPeriodOut", "День следующего месяца окончания периода");

            this.Namespace("GkhDi.DisinfoRealObj.DocumentsRealityObj", "Документы");
            this.Permission("GkhDi.DisinfoRealObj.DocumentsRealityObj.Edit", "Изменение");
            this.Permission("GkhDi.DisinfoRealObj.DocumentsRealityObj.AddCopy", "Добавить копированием");

            this.Namespace("GkhDi.DisinfoRealObj.DocumentsRealityObj.ByYear", "Документы по годам");
            this.Permission("GkhDi.DisinfoRealObj.DocumentsRealityObj.ByYear.Add", "Добавление");
            this.Permission("GkhDi.DisinfoRealObj.DocumentsRealityObj.ByYear.Delete", "Удаление");
            this.Permission("GkhDi.DisinfoRealObj.DocumentsRealityObj.ByYear.Edit", "Изменение");

            this.Namespace("GkhDi.DisinfoRealObj.FinancialPerformance", "Финансовые показатели");
            this.Permission("GkhDi.DisinfoRealObj.FinancialPerformance.FinancialPanel_View", "Просмотр - Общая информация");
            this.Permission("GkhDi.DisinfoRealObj.FinancialPerformance.FinancialPanel_Edit", "Редактирование - Общая информация");

            this.Permission("GkhDi.DisinfoRealObj.FinancialPerformance.PretensionQualityWork_View", "Просмотр - Претензии по качеству работ");
            this.Permission("GkhDi.DisinfoRealObj.FinancialPerformance.PretensionQualityWork_Edit", "Редактирование - Претензии по качеству работ");

            this.Permission("GkhDi.DisinfoRealObj.FinancialPerformance.DiRealObjClaimWork_View", "Просмотр - Претензионно-исковая работа");
            this.Permission("GkhDi.DisinfoRealObj.FinancialPerformance.DiRealObjClaimWork_Edit", "Редактирование - Претензионно-исковая работа");

            this.Permission("GkhDi.DisinfoRealObj.FinancialPerformance.CommunalService_View", "Просмотр - Коммунальные услуги");
            this.Permission("GkhDi.DisinfoRealObj.FinancialPerformance.CommunalService_Edit", "Редактирование - Коммунальные услуги");

            #endregion Объекты в управлении

            this.Namespace("GkhDi.OtherService", "Прочие услуги");
            this.Permission("GkhDi.OtherService.View", "Просмотр");
            this.Permission("GkhDi.OtherService.Delete", "Удаление");
            this.Permission("GkhDi.OtherService.Edit", "Изменение");

            this.Namespace("Reports.Di", "Модуль Раскрытие информации");
            this.Permission("Reports.Di.PercentCalculation", "Отчет по раскрытию информации по ПП РФ №731");
            this.Permission("Reports.Di.B3PercentCalculation", "Отчет по раскрытию информации по ПП РФ №731 до 2013");
            this.Permission("Reports.Di.WeeklyDI", "Еженедельный отчет по раскрытию информации");
            this.Permission("Reports.Di.FillingDataForRankingManOrg", "Заполнение данных для рейтинга УК в разделе Деятельность УК");
            this.Permission("Reports.Di.FillGeneralDataRatingCR", "Заполнение общей информации об УК для Рейтинга УК");
            this.Permission("Reports.Di.FillFinanceDataRatingCr", "Заполнение финансовых показателей для рейтинга УК");
            this.Permission("Reports.Di.FillingTechPassportForManOrgRating", "Заполнение технического паспорта для рейтинга УК");
            this.Permission("Reports.Di.FillGenInformationHousesRankingYK", "Заполнение общей информации по домам для Рейтинга УК");
            this.Permission("Reports.Di.ChangeCommunalServicesTariff", "Изменение тарифов на коммунальные услуги");

            this.Namespace("GkhDi.EmptyFieldsLog", "Журнал заполнения полей");
            this.Permission("GkhDi.EmptyFieldsLog.DisclosureInfoEmptyFieldsGrid", "Управляющая организация - Просмотр");
            this.Permission("GkhDi.EmptyFieldsLog.DisclosureInfoRealityObjEmptyFieldsGrid", "Объекты в управлении - Просмотр");

            this.RealtyObjectPassport();
        }

        private void RealtyObjectPassport()
        {
            this.Namespace("GkhDi.DisinfoRealObj.RealtyObjectPassport", "Паспорт дома");
            this.Permission("GkhDi.DisinfoRealObj.RealtyObjectPassport.PassportPanelMainInformation_View", "Общие сведения - просмотр");
            this.Permission("GkhDi.DisinfoRealObj.RealtyObjectPassport.LiftsInfoMainInformation_View", "Cведения о лифтах - просмотр");
            this.Permission("GkhDi.DisinfoRealObj.RealtyObjectPassport.DevicesInfoMainInformation_View", "Приборы учёта - просмотр");
            this.Permission("GkhDi.DisinfoRealObj.RealtyObjectPassport.StructElementsInformation_View", "Конструктивные элементы - просмотр");
            this.Permission("GkhDi.DisinfoRealObj.RealtyObjectPassport.EngineerSystemsInformation_View", "Инженерные системы - просмотр");
            this.Permission("GkhDi.DisinfoRealObj.RealtyObjectPassport.HouseManaging_View", "Управление домом - просмотр");

            this.Namespace("GkhDi.DisinfoRealObj.RealtyObjectPassport.PassportPanelMainInformation", "Общие сведения");
            this.Permission("GkhDi.DisinfoRealObj.RealtyObjectPassport.PassportPanelMainInformation.OwnerInfoContainer", "Сведения о владельце специального счета");
            this.Permission("GkhDi.DisinfoRealObj.RealtyObjectPassport.PassportPanelMainInformation.ProtocolInfoContainer", "Реквизиты протокола общего собрания собственников помещений");
        }
    }
}