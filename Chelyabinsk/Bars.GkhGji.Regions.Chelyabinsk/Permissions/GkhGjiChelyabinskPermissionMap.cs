namespace Bars.GkhGji.Regions.Chelyabinsk.Permissions
{
    using B4;
    using Bars.B4.Application;
    using Bars.Gkh.TextValues;
    using Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// PermissionMap для GkhGjiPermissionMap
    /// </summary>
    public class GkhGjiChelyabinskPermissionMap : PermissionMap
    {
        /// <summary>
        /// Интерфейс для описания текстовых значений пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <summary>
        /// Конструктор GkhGjiPermissionMap
        /// </summary>
        public GkhGjiChelyabinskPermissionMap()
        {
            this.Namespace("GkhGji", "Модуль ГЖИ");

            #region Риск-ориентированный подход
            this.Namespace("GkhGji.RiskOrientedMethod", "Риск-ориентированный подход");

            this.Namespace("GkhGji.RiskOrientedMethod.KindKNDDict", "Справочник типов КНД");
            this.CRUDandViewPermissions("GkhGji.RiskOrientedMethod.KindKNDDict");


            this.Namespace("GkhGji.RiskOrientedMethod.EffectiveKNDIndex", "Показатели эффективности КНД");
            this.CRUDandViewPermissions("GkhGji.RiskOrientedMethod.EffectiveKNDIndex");

            this.Namespace("GkhGji.RiskOrientedMethod.ROMCategory", "Расчет коэффициентов риска");
            this.CRUDandViewPermissions("GkhGji.RiskOrientedMethod.ROMCategory");

            this.Namespace("GkhGji.RiskOrientedMethod.ROMCategory.Field", "Поля");
            this.Permission("GkhGji.RiskOrientedMethod.ROMCategory.Field.Vp_Edit", "Коэффициенты - Редактирование");
            this.Permission("GkhGji.RiskOrientedMethod.ROMCategory.Field.Vp_View", "Коэффициенты - Просмотр");

            #endregion

            this.Namespace("GkhGji.CourtPractice", "Судебная и административная практика");

            this.Namespace("GkhGji.CourtPractice.CourtPracticeRegystry", "Реестр судебной и административной практики");
            this.CRUDandViewPermissions("GkhGji.CourtPractice.CourtPracticeRegystry");

            #region Проверки ОМСУ
            this.Namespace("GkhGji.Inspection", "Основания проверок");

            this.Namespace("GkhGji.Inspection.BaseOMSU", "Проверки ОМСУ");
            this.Permission("GkhGji.Inspection.BaseOMSU.View", "Просмотр");
            this.Permission("GkhGji.Inspection.BaseOMSU.Create", "Создание записей");

           

            #region Плановые проверки юр.лиц - поля

            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseOMSU.Field", "Поля");

            this.Permission("GkhGji.Inspection.BaseOMSU.Field.Plan_Edit", "План");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.UriRegistrationNumber_Edit", "Учетный номер проверки в едином реестре проверок");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.UriRegistrationDate_Edit", "Дата присвоения учетного номера");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.DateStart_Edit", "Дата начала проверки");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.InsNumber_Edit", "Номер");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.CountDays_Edit", "Срок проверки (количество дней)");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.TypeBaseOMSU_Edit", "Основание проверки");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.TypeFact_Edit", "Факт проверки");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.Reason_Edit", "Причина");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.TypeForm_Edit", "Форма проверки");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.Inspection.BaseOMSU.Field.ZonalInspections_Edit", "Отделы");

            #endregion Плановые проверки юр.лиц - поля





            #endregion

            #region Справочники
            this.Namespace("GkhGji.Dict", "Справочники");

            this.Namespace("GkhGji.Dict.RegionCode", "Коды регионов");
            this.CRUDandViewPermissions("GkhGji.Dict.RegionCode");

            this.Namespace("GkhGji.Dict.ProsecutorOffice", "Органы прокуратуры");
            this.CRUDandViewPermissions("GkhGji.Dict.ProsecutorOffice");

            this.Namespace("GkhGji.Dict.FLDocType", "Документы физических лиц");
            this.CRUDandViewPermissions("GkhGji.Dict.FLDocType");

            this.Namespace("GkhGji.Dict.EGRNDocType", "Документы ЕГРН");
            this.CRUDandViewPermissions("GkhGji.Dict.EGRNDocType");

            this.Namespace("GkhGji.Dict.EGRNApplicantType", "Категория заявителя ЕГРН");
            this.CRUDandViewPermissions("GkhGji.Dict.EGRNApplicantType");

            this.Namespace("GkhGji.Dict.EGRNObjectType", "Тип объекта запроса ЕГРН");
            this.CRUDandViewPermissions("GkhGji.Dict.EGRNObjectType");

            this.Namespace("GkhGji.Dict.SSTUTransferOrg", "Идентификаторы ССТУ");
            this.CRUDandViewPermissions("GkhGji.Dict.SSTUTransferOrg");

            #endregion

            #region СМЭВ
            //"GkhGji.SMEV.SMEVMVD.View"
            this.Namespace("GkhGji.SMEV", "СМЭВ");

            this.Namespace("GkhGji.SMEV.SMEVMVD", "Запросы в МВД");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVMVD");

            this.Namespace("GkhGji.SMEV.SMEVEGRUL", "Запросы в ЕГРЮЛ");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVEGRUL");

            this.Namespace("GkhGji.SMEV.SMEVEGRUL", "Запросы в ЕГРИП");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVEGRIP");

            this.Namespace("GkhGji.SMEV.SMEVEGRN", "Запросы в ЕГРН");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVEGRN");
            this.Permission("GkhGji.SMEV.SMEVEGRN.Log", "Просмотр лога");

            this.Namespace("GkhGji.SMEV.GISGMP", "Обмен данными с ГИС ГМП");
            this.CRUDandViewPermissions("GkhGji.SMEV.GISGMP");

            this.Namespace("GkhGji.SMEV.GISGMP.ChangeState", "Изменение статусов ГИС ГМП");
            this.CRUDandViewPermissions("GkhGji.SMEV.GISGMP");

            this.Namespace("GkhGji.SMEV.GISERP", "Обмен данными с ГИС ЕРП");
            this.CRUDandViewPermissions("GkhGji.SMEV.GISERP");

            this.Namespace("GkhGji.SMEV.PAYREG", "Реестр платежей");
            this.CRUDandViewPermissions("GkhGji.SMEV.PAYREG");
            #endregion
            
            #region СОПР
            this.Namespace("GkhGji.SOPR", "СОПР");
            this.Namespace("GkhGji.SOPR.Appeal", "Отписаные обращения");
            this.CRUDandViewPermissions("GkhGji.SOPR.Appeal");
            this.Permission("GkhGji.SOPR.Appeal.Vp_Edit", "Принято инспектором");
            this.Namespace("GkhGji.SOPR.Field", "Поля");
            this.Permission("GkhGji.SOPR.Field.Person", "Должностное лицо");           
            this.Permission("GkhGji.SOPR.Field.PersonPhone", "Телефон должностного лица");
            this.Permission("GkhGji.SOPR.Field.Correspondent", "Сведения о заявителе");
            this.Permission("GkhGji.SOPR.Field.AppealText", "Текст обращения");
            this.Permission("GkhGji.SOPR.Field.AppealFile", "Файл обращения");
            this.Permission("GkhGji.SOPR.Field.Confirmed", "Подтверждение");
           
            #endregion СОПР

        }
    }
}