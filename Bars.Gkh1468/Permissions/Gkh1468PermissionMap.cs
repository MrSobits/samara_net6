namespace Bars.Gkh1468.Permissions
{
    using B4;
    using Entities;

    public class Gkh1468PermissionMap : PermissionMap
    {
        public Gkh1468PermissionMap()
        {
            this.Namespace("Gkh1468", "Модуль ЖКХ 1468");

            #region Жилые дома
            this.Namespace("Gkh.RealityObject.Register.PublicServOrg", "Поставщики услуг");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.PublicServOrg");
            #endregion

            #region Участники процесса
            this.Namespace("Gkh1468.Orgs", "Участники процесса");

            this.Namespace("Gkh1468.Orgs.PublicServiceOrg", "Поставщики ресурсов");
            this.CRUDandViewPermissions("Gkh1468.Orgs.PublicServiceOrg");

            this.Namespace("Gkh1468.Orgs.PublicServiceOrg.RealtyObject", "Жилые дома");
            this.Permission("Gkh1468.Orgs.PublicServiceOrg.RealtyObject.View", "Просмотр");
            this.Permission("Gkh1468.Orgs.PublicServiceOrg.RealtyObject.Edit", "Редактирование (добавление/удаление)");

            this.Namespace("Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj", "Договора РСО");
            this.Permission("Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj.RealityObjectResOrgService", "Предоставляемые услуги");
            this.Permission("Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj.View", "Просмотр");
            this.Permission("Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj.Edit", "Редактирование (добавление/удаление)");
            #endregion

            #region Паспорта по ПП РФ 1468

            this.Namespace("Gkh1468.Passport", "Паспорта по ПП РФ 1468");

            this.Namespace("Gkh1468.Passport.ImportFrom1468", "Импорт паспортов 1468");
            this.Permission("Gkh1468.Passport.ImportFrom1468.View", "Просмотр");
            this.Permission("Gkh1468.Passport.ImportFrom1468.Import", "Импорт");
            this.Permission("Gkh1468.Passport.ImportFrom1468.ImportWoDigitSign", "Импорт без проверки ЭЦП");

            this.Namespace("Gkh1468.Passport.Oki", "Реестр паспортов ОКИ");
            this.Permission("Gkh1468.Passport.Oki.View", "Просмотр");

            this.Namespace("Gkh1468.Passport.House", "Реестр паспортов дома");
            this.Permission("Gkh1468.Passport.House.View", "Просмотр");

            this.Namespace<OkiProviderPassport>("Gkh1468.Passport.MyOki", "Мои паспорта ОКИ");
            this.Permission("Gkh1468.Passport.MyOki.View", "Просмотр");
            this.Permission("Gkh1468.Passport.MyOki.Create", "Создание");
            this.Permission("Gkh1468.Passport.MyOki.Edit", "Редактирование");
            this.Permission("Gkh1468.Passport.MyOki.Delete", "Удаление");
            this.Permission("Gkh1468.Passport.MyOki.Sign", "Создание подписи паспорта");

            this.Namespace<HouseProviderPassport>("Gkh1468.Passport.MyHouse", "Мои паспорта домов");
            this.Permission("Gkh1468.Passport.MyHouse.View", "Просмотр");
            this.Permission("Gkh1468.Passport.MyHouse.Create", "Создание");
            this.Permission("Gkh1468.Passport.MyHouse.Edit", "Редактирование");
            this.Permission("Gkh1468.Passport.MyHouse.Delete", "Удаление");
            this.Permission("Gkh1468.Passport.MyHouse.Sign", "Создание подписи паспорта");
            this.Permission("Gkh1468.Passport.MyHouse.CreateByAllRo", "Создание паспортов по всем домам");

            #endregion Паспорта по ПП РФ 1468

            #region Справочники

            this.Namespace("Gkh1468.Dictionaries", "Справочники");

            this.Namespace("Gkh1468.Dictionaries.PublicService", "Коммунальные услуги");
            this.CRUDandViewPermissions("Gkh1468.Dictionaries.PublicService");
            
            this.Namespace("Gkh1468.Dictionaries.PassportStruct", "Справочник структур паспортов");
            this.CRUDandViewPermissions("Gkh1468.Dictionaries.PassportStruct");
            this.Permission("Gkh1468.Dictionaries.PassportStruct.Import", "Импорт структуры паспортов");
            this.Permission("Gkh1468.Dictionaries.PassportStruct.Export", "Экспорт структуры паспортов");

            this.Namespace("Gkh1468.Dictionaries.TypeCustomer", "Вид бюджета");
            this.CRUDandViewPermissions("Gkh1468.Dictionaries.TypeCustomer");
            #endregion
        }
    }
}