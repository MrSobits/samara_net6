namespace Bars.Gkh.Overhaul
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Namespace("Gkh.RealityObject.Register.StructElem", "Конструктивные характеристики");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.StructElem");
            this.Permission("Gkh.RealityObject.Register.StructElem.PlanRepairYear_View", "Плановый год ремонта - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.AdjustedYear_View", "Скорректированный год - Просмотр");

            this.Namespace("Gkh.RealityObject.Register.StructElem.Field", "Поля");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.Condition_View", "Состояние - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.Condition_Edit", "Состояние - Редактирование");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.LastOverhaulYear_View", "Год установки - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.LastOverhaulYear_Edit", "Год установки - Редактирование");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.Wearout_View", "Износ - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.Wearout_Edit", "Износ - Редактирование");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.WearoutAct_View", "Износ актуализация - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.WearoutAct_Edit", "Износ актуализация - Редактирование");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.Volume_View", "Объем - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.Volume_Edit", "Объем - Редактирование");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.ElementName_View", "Наименование - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.ElementName_Edit", "Наименование - Редактирование");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.SystemType_View", "Тип системы - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.SystemType_Edit", "Тип системы - Редактирование");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.NetworkLength_View", "Протяженность сетей - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.NetworkLength_Edit", "Протяженность сетей - Редактирование");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.NetworkPower_View", "Мощность - Просмотр");
            this.Permission("Gkh.RealityObject.Register.StructElem.Field.NetworkPower_Edit", "Мощность - Редактирование");

            #region Импорт
            this.Namespace("Import.CommonRealtyObjectImport", "Импорт жилых домов (универсальный)");
            this.Permission("Import.CommonRealtyObjectImport.View", "Просмотр");

            #endregion

            this.Dictionaries();

            this.Reports();
        }

        private void Dictionaries()
        {
            this.Permission("Gkh.Orgs.Contragent.Register.Bank.Name_View", "Наименование - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Register.Bank.Name_Edit", "Наименование - Редактирование");
            this.Permission("Gkh.Orgs.Contragent.Register.Bank.CreditOrg_View", "Кредитная организация - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Register.Bank.CreditOrg_Edit", "Кредитная организация - Редактирование");
            this.Permission("Gkh.Orgs.Contragent.Register.Bank.File_View", "Файл - Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Register.Bank.File_Edit", "Файл - Редактирование");

            this.Namespace("Ovrhl.Dictionaries", "Справочники");

            this.Namespace("Ovrhl.Dictionaries.PaymentSizeCr", "Размер взноса на капремонт");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.PaymentSizeCr");

            this.Namespace("Gkh.Dictionaries.WorkPrice", "Справочник расценок работ");
            this.CRUDandViewPermissions("Gkh.Dictionaries.WorkPrice");
            this.Namespace("Gkh.Dictionaries.WorkPrice.Field", "Поля");
            this.Permission("Gkh.Dictionaries.WorkPrice.Field.CapitalGroup", "Группа капитальности");
            this.Namespace("Gkh.Dictionaries.WorkPrice.Column", "Колонки");
            this.Permission("Gkh.Dictionaries.WorkPrice.Column.CapitalGroup", "Группа капитальности");

            this.Namespace("Gkh.Dictionaries.Work.TypeFinSource", "Источники финансирования");
            this.CRUDandViewPermissions("Gkh.Dictionaries.Work.TypeFinSource");

            this.Namespace("Ovrhl.Dictionaries.CommonEstateObject", "ООИ (Объекты общего имущества)");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CommonEstateObject");

            this.Namespace("Ovrhl.Dictionaries.CommonEstateObject.Group", "Группы");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CommonEstateObject.Group");

            this.Namespace("Ovrhl.Dictionaries.CommonEstateObject.Group.Attribute", "Атрибуты группы");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CommonEstateObject.Group.Attribute");

            this.Namespace("Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl", "Конструктивные элементы");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl");
            this.Permission("Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.ReformCode_Edit", "Код реформы ЖКХ - Редактирование");

            this.Namespace("Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.Work", "Работы по конструктивному элементу");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.Work");

            this.Namespace("Ovrhl.Dictionaries.CommonEstateObject.Group.Formula", "Параметры формулы");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CommonEstateObject.Group.Formula");

            this.Namespace("Ovrhl.Dictionaries.GroupType", "Тип группы ООИ (Объекты общего имущества)");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.GroupType");

            this.Namespace("Ovrhl.Dictionaries.Job", "Работы");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.Job");

            this.Namespace("GkhRegOp.Settings.PaymentSizeCrNew", "Размеры взноса на капремонт");
            this.Permission("GkhRegOp.Settings.PaymentSizeCrNew.View", "Просмотр");
        }

        private void Reports()
        {
            this.Permission("Reports.GkhOverhaul.LackOfRequiredStructEls", "Дома с отсутствующими обязательными КЭ");
            this.Permission("Reports.GkhOverhaul.PublishedProgramReport", "Отчет по опубликованной программе");
        }
    }
}