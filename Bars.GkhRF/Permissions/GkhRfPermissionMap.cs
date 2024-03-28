namespace Bars.GkhRf.Permissions
{
    using Bars.B4;
    using Bars.GkhRf.Entities;

    public class GkhRfPermissionMap : PermissionMap
    {
        public GkhRfPermissionMap()
        {
            Namespace("GkhRf", "Модуль региональный фонд");

            Namespace("GkhRf.ContractRf", "Реестр договоров");
            CRUDandViewPermissions("GkhRf.ContractRf");
            Permission("GkhRf.ContractRf.WithArchiveRecs", "Показать архивные записи");
            Namespace("GkhRf.ContractRf.Column", "Колонки");
            Permission("GkhRf.ContractRf.Column.SumAreaMkd", "Общая площадь МКД");
            Permission("GkhRf.ContractRf.Column.SumAreaLivingOwned", "Площадь в собственности граждан");

            Namespace("GkhRf.Payment", "Реестр оплат КР");
            CRUDandViewPermissions("GkhRf.Payment");
            Permission("GkhRf.Payment.Import", "Импорт"); // реализовать после добавления Импорта

            Namespace("GkhRf.TransferRf", "Информация о перечислениях средств в фонд");
            CRUDandViewPermissions("GkhRf.TransferRf");
            Permission("GkhRf.TransferRf.WithArchiveRecs", "Показать архивные записи");

            Namespace<TransferRfRecord>("GkhRf.TransferRf.EditData", "Редактирование данных");
            Permission("GkhRf.TransferRf.EditData.TrasferRfRec", "Платежное поручение ");
            Permission("GkhRf.TransferRf.EditData.TrasferRfRecObj", "Перечисленная сумма");

            Namespace("GkhRf.RequestTransferRfViewCreate", "Реестр заявок на перечисление денежных средств: Просмотр, создание");
            Permission("GkhRf.RequestTransferRfViewCreate.View", "Просмотр");
            Permission("GkhRf.RequestTransferRfViewCreate.Create", "Создание записей");
            Permission("GkhRf.RequestTransferRfViewCreate.CopyRequestTransferRf", "Копирование заявки");//не реализовано

            Namespace<RequestTransferRf>("GkhRf.RequestTransferRf", "Реестр заявок на перечисление денежных средств: Изменение, удаление");
            Permission("GkhRf.RequestTransferRf.Edit", "Изменение записей");
            Permission("GkhRf.RequestTransferRf.Delete", "Удаление записей");

            Namespace("GkhRf.RequestTransferRf.Field", "Поля");
            Permission("GkhRf.RequestTransferRf.Field.DocumentNum", "Номер документа");
            Permission("GkhRf.RequestTransferRf.Field.DateFrom", "Дата заявки");
            Permission("GkhRf.RequestTransferRf.Field.ManagingOrganization", "Управляющая организация");
            Permission("GkhRf.RequestTransferRf.Field.Inn", "ИНН");
            Permission("GkhRf.RequestTransferRf.Field.Phone", "Телефон");
            Permission("GkhRf.RequestTransferRf.Field.Kpp", "КПП");
            Permission("GkhRf.RequestTransferRf.Field.SettlementAccount", "Расчетный счет");
            Permission("GkhRf.RequestTransferRf.Field.ContragentBank", "Наименование банка");
            Permission("GkhRf.RequestTransferRf.Field.CorrAccount", "Кор. счет");
            Permission("GkhRf.RequestTransferRf.Field.Bik", "БИК");
            Permission("GkhRf.RequestTransferRf.Field.ContractRf", "Договор");
            Permission("GkhRf.RequestTransferRf.Field.ProgramCr", "Программа капремонта");
            Permission("GkhRf.RequestTransferRf.Field.TypeProgramRequest", "Тип программы");
            Permission("GkhRf.RequestTransferRf.Field.Performer", "Исполнитель");
            Permission("GkhRf.RequestTransferRf.Field.File", "Файл");

            Namespace("GkhRf.RequestTransferRf.TransferFund", "Дома, включенные в заявку");
            Permission("GkhRf.RequestTransferRf.TransferFund.Create", "Создание записей");
            Permission("GkhRf.RequestTransferRf.TransferFund.Edit", "Изменение записей");
            Permission("GkhRf.RequestTransferRf.TransferFund.Delete", "Удаление записей");

            Namespace("GkhRf.LimitCheck", "Настройка проверки на наличие лимитов");
            Permission("GkhRf.LimitCheck.View", "Просмотр");

            Namespace("Reports.RF", "Модуль региональный фонд");
            Permission("Reports.RF.CrPaymentInformation", "01_Сведения об оплате КР");
            Permission("Reports.RF.GisuRealObjContract", "Отчет по домам, включенным в договор с ГИСУ");
            Permission("Reports.RF.ProgramCrOwnersSpending", "Расходование средств собственников на реализацию программы капитального ремонта");
            Permission("Reports.RF.InformationRepublicProgramCr", "Информация об участии в Республиканской адресной программе по проведению капитального ремонта МКД");
            Permission("Reports.RF.GisuOrdersReport", "Информация о заключенных договорах между УК и ГКУ Главинвестстрой РТ");
            Permission("Reports.RF.AnalisysProgramCr", "Анализ потребности проведения программы");
            Permission("Reports.RF.JournalAcPayment", "Журнал начислений и оплат");
            Permission("Reports.RF.ExistHouseInContractAndPayment", "Отчет по наличию домов в договоре и в оплатах");
            Permission("Reports.RF.CreatedRealtyObject", "Отчет о созданных объектах недвижимости");
            Permission("Reports.RF.ContractsAvailabilityWithGisu", "Наличие договоров с ГИСУ");
            Permission("Reports.RF.ListHousesByProgramCr", "Перечень домов по программе");
            Permission("Reports.RF.CountOfRequestInRf", "Количество заявок в Региональный фонд");
            Permission("Reports.RF.AllocationFundsToPeopleInCr", "Информация о распределении денежных средств граждан на капитальный ремонт жилых домов");
            Permission("Reports.RF.CalculationsBetweenGisuByManOrg", "Сверка расчетов между ГИСУ и УО");
            Permission("Reports.RF.PaymentCrMonthByRealObj", "Сведения за месяц об оплате КР (по домам)");
            Permission("Reports.RF.InformationCashReceivedRegionalFund", "Информация о поступивших денежных средствах в Региональный фонд");
            Permission("Reports.RF.CitizenFundsIncomeExpense", "Отчет приход и расход по средствам граждан");
            Permission("Reports.RF.InformationRepublicProgramCrPart2", "Информация об участии в Республиканской адресной программе по проведению капитального ремонта МКД часть2");
            Permission("Reports.RF.CorrectionOfLimits", "Корректировка лимитов");
            Permission("Reports.RF.PeopleFundsTransferToGisuInfo", "Информация о перечислении денежных средств граждан в ГИСУ (без пересичления)");
            Permission("Reports.RF.RoomAreaControl", "Контроль площадей помещений");
        }
    }
}