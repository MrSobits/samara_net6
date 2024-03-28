namespace Bars.GkhCr.Regions.Tatarstan.Permissions
{
    using Bars.B4;

    public class GkhCrPermissionMap : PermissionMap
    {
        public GkhCrPermissionMap()
        {
            this.Namespace("GkhCr.OutdoorProgram", "Реестр программ благоустройства дворов");
            this.CRUDandViewPermissions("GkhCr.OutdoorProgram");

            this.Namespace("GkhCr.Dict.WorkRealityObjectOutdoor", "Виды работ по благоустройству дворов");
            this.CRUDandViewPermissions("GkhCr.Dict.WorkRealityObjectOutdoor");

            this.Namespace("GkhCr.Dict.ElementOutdoor", "Элементы двора");
            this.CRUDandViewPermissions("GkhCr.Dict.ElementOutdoor");

            this.Namespace("GkhCr.ObjectOutdoorCr", "Объекты программы благоустройства дворов");
            this.CRUDandViewPermissions("GkhCr.ObjectOutdoorCr");

            this.Namespace("GkhCr.ObjectOutdoorCr.Registries", "Реестры");
            this.Namespace("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit", "Паспорт объекта");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.View", "Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Edit", "Изменение");
            this.AddObjectOutdoorCrEditFieldsPermissions();

            this.Namespace("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork", "Виды работ");
            this.CRUDandViewPermissions("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork");
            this.AddObjectOutdoorCrTypeWorkFieldsPermissions();

            this.Permission("GkhCr.ObjectCrViewCreate.GjiNumberFill", "Заполнить номер ГЖИ");
        }

        private void AddObjectOutdoorCrEditFieldsPermissions()
        {
            this.Namespace("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields", "Поля");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.RealityObjectOutdoor_View", "Объект - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.RealityObjectOutdoor_Edit", "Объект - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.RealityObjectOutdoorProgram_View", "Программа - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.RealityObjectOutdoorProgram_Edit", "Программа - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.WarrantyEndDate_View", "Дата окончания гарантийных обязательств - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.WarrantyEndDate_Edit", "Дата окончания гарантийных обязательств - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.GjiNum_View", "Номер ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.GjiNum_Edit", "Номер ГЖИ - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.Description_View", "Примечание - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.Description_Edit", "Примечание - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.MaxAmount_View", "Предельная сумма (руб.) - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.MaxAmount_Edit", "Предельная сумма (руб.) - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactStartDate_View", "Фактическая дата начала работ - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactStartDate_Edit", "Фактическая дата начала работ - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.SumSmr_View", "Сумма на СМР - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.SumSmr_Edit", "Сумма на СМР - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateGjiReg_View", "Дата регистрации ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateGjiReg_Edit", "Дата регистрации ГЖИ - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateEndBuilder_View", "Дата регистрации организацией осуществляющей Стройконтроль - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateEndBuilder_Edit", "Дата регистрации организацией осуществляющей Стройконтроль - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateStartWork_View", "Дата начала работ согласно заключенному договору - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateStartWork_Edit", "Дата начала работ согласно заключенному договору - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactAmountSpent_View", "Фактически освоенная сумма (руб.) - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactAmountSpent_Edit", "Фактически освоенная сумма (руб.) - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactEndDate_View", "Фактическая дата окончания работ - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactEndDate_Edit", "Фактическая дата окончания работ - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.SumSmrApproved_View", "Утвержденная сумма на СМР - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.SumSmrApproved_Edit", "Утвержденная сумма на СМР - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateStopWorkGji_View", "Дата остановки работ ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateStopWorkGji_Edit", "Дата остановки работ ГЖИ - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateAcceptGji_View", "Дата принятия ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateAcceptGji_Edit", "Дата принятия ГЖИ - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateEndWork_View", "Дата принятия организацией осуществляющей Стройконтроль - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateEndWork_Edit", "Дата принятия организацией осуществляющей Стройконтроль - Изменение");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.СommissioningDate_View", "Дата ввода в эксплуатацию - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.СommissioningDate_Edit", "Дата ввода в эксплуатацию - Изменение");
        }
        private void AddObjectOutdoorCrTypeWorkFieldsPermissions()
        {
            this.Namespace("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields", "Поля");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.WorkRealityObjectOutdoor_View", "Вид работы - Просмотр");

            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Volume_View", "Объем - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Volume_Edit", "Объем - Изменение");

            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Sum_View", "Сумма (руб.) - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Sum_Edit", "Сумма (руб.) - Изменение");

            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Description_View", "Примечание - Просмотр");
            this.Permission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Description_Edit", "Примечание - Изменение");
        }

    }
}