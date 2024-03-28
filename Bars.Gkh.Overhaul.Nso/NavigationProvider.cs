namespace Bars.Gkh.Overhaul.Nso
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            //root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт жилых домов для Новосибирска", "nsorealityobjimport").AddRequiredPermission("Import.NsoRealtyObjectImport.View");
            //root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт из фонда домов (часть 3)", "roimportfromfundpart3").AddRequiredPermission("Import.RoImportFromFundPart3.View");
            //root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт из фонда домов (часть 5)", "roimportfromfundpart5").AddRequiredPermission("Import.RoImportFromFundPart5.View");
            root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт работ по конструктивным элементам", "worksimportbystructelements").AddRequiredPermission("Import.WorksImportByStructElements.View");
            root.Add("Администрирование").Add("Импорты").Add("Импорт КПКР для Новосибирска", "nsokpkrimport").AddRequiredPermission("Import.KpkrForNsk.View");
            
            root.Add("Участники процесса").Add("Контрагенты").Add("Кредитные организации", "creditorg").AddRequiredPermission("Ovrhl.CreditOrg.View");

            root.Add("Справочники").Add("Капитальный ремонт").Add("Операции по счету", "accountoperation").AddRequiredPermission("Ovrhl.Dictionaries.AccountOperation.View");

            var longProgram = root.Add("Капитальный ремонт").Add("Региональная программа");

            longProgram.Add("Долгосрочная программа", "dpkr").AddRequiredPermission("Ovrhl.LongTermProgram.View").WithIcon("longProgram");
            longProgram.Add("Субсидирование", "subsidy").AddRequiredPermission("Ovrhl.Subcidy.View").WithIcon("subsidy");
            longProgram.Add("Опубликованные программы", "publicationprogs").AddRequiredPermission("Ovrhl.PublicationProgs.View").WithIcon("publishPrograms");
            longProgram.Add("Дефицит по МО", "shortprogramdef").AddRequiredPermission("Ovrhl.ShortProgramDeficit.View").WithIcon("deficitMo");
            longProgram.Add("Краткосрочная программа", "shortprogram").AddRequiredPermission("Ovrhl.ShortProgram.View").WithIcon("shortProgram");
            longProgram.Add("Загрузка программы", "loadprogram").AddRequiredPermission("Ovrhl.LoadProgram.View");

            var programParams = root.Add("Капитальный ремонт").Add("Параметры программы капитального ремонта");
            programParams.Add("Версии программы", "dpkr_versions").AddRequiredPermission("Ovrhl.ProgramVersions.View").WithIcon("programVersion");
            programParams.Add("Тариф по типам домов", "realestatetyperate").AddRequiredPermission("Ovrhl.RealEstateTypeRate.View");
            programParams.Add("Параметры очередности", "priorityparam").AddRequiredPermission("Ovrhl.PriorityParam.View").WithIcon("unitMeasure");

            var longProgramObjects = root.Add("Капитальный ремонт").Add("Объекты региональной программы капитального ремонта");
            longProgramObjects.Add("Реестр объектов региональной программы", "longtermprobject").AddRequiredPermission("Ovrhl.LongTermProgramObject.View").WithIcon("longProgramRegistry");
            longProgramObjects.Add("Счет невыясненных сумм", "suspenseaccount").AddRequiredPermission("Ovrhl.SuspenseAccount.View");
            
            root.Add("Администрирование").Add("Экспорт данных").Add("Экспорт тарифа", "B4.controller.TariffExport").AddRequiredPermission("Administration.ExportTariff.View");
        }

        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }
    }
}