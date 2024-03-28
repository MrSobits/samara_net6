namespace Bars.Gkh.Overhaul.Tat
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            //root.Add("Администрирование").Add("Импорты").Add("Импорт жилых домов для Новосибирска", "nsorealityobjimport").AddRequiredPermission("Import.TatRealtyObjectImport.View");
            root.Add("Администрирование").Add("Массовое удаление").Add("Массовое удаление КЭ", "massdeleterose").AddRequiredPermission("Administration.MassDeleteRoSe.View");

            root.Add("Участники процесса").Add("Роли контрагента").Add("Региональные операторы", "regoperator").AddRequiredPermission("Ovrhl.RegOperator.View");
            root.Add("Участники процесса").Add("Контрагенты").Add("Кредитные организации", "creditorg").AddRequiredPermission("Ovrhl.CreditOrg.View");

            root.Add("Справочники").Add("Капитальный ремонт").Add("Операции по счету", "accountoperation").AddRequiredPermission("Ovrhl.Dictionaries.AccountOperation.View");

            var longProgram = root.Add("Капитальный ремонт").Add("Долгосрочная программа");

            longProgram.Add("Первый этап", "programfirststage").AddRequiredPermission("Ovrhl.Program1Stage.View");
            longProgram.Add("Долгосрочная программа", "dpkr").AddRequiredPermission("Ovrhl.LongTermProgram.View").WithIcon("longProgram");
            //longProgram.Add("Тариф по типам домов", "realestatetyperate").AddRequiredPermission("Ovrhl.RealEstateTypeRate.View");
            longProgram.Add("Версии программы", "dpkr_versions").AddRequiredPermission("Ovrhl.LongProgram.ProgramVersion.View").WithIcon("programVersion");
            longProgram.Add("Субсидирование", "subsidy").AddRequiredPermission("Ovrhl.Subcidy.View").WithIcon("subsidy");
            longProgram.Add("Результат корректировки", "correctionresult").AddRequiredPermission("Ovrhl.ProgramCorrection.View");
            longProgram.Add("Опубликованные программы", "publicationprogs").AddRequiredPermission("Ovrhl.PublicationProgs.View").WithIcon("publishPrograms");
            //longProgram.Add("Дефицит по МО", "shortprogramdef");//.AddRequiredPermission("Ovrhl.ShortProgram.View").WithIcon("deficitMo");
            longProgram.Add("Краткосрочная программа", "shortprogram").AddRequiredPermission("Ovrhl.ShortProgram.View").WithIcon("shortProgram");
            longProgram.Add("Документы ДПКР", "dpkrdocument").AddRequiredPermission("Ovrhl.DpkrDocument.View");

            var programParams = root.Add("Капитальный ремонт").Add("Параметры программы капитального ремонта");

            programParams.Add("Параметры очередности", "priorityparam").AddRequiredPermission("Ovrhl.PriorityParam.View").WithIcon("unitMeasure");

            root.Add("Капитальный ремонт").Add("Объекты региональной программы капитального ремонта")
                .Add("Реестр объектов региональной программы", "longtermprobject").AddRequiredPermission("Ovrhl.LongTermProgramObject.View").WithIcon("longProgramRegistry");

            root.Add("Капитальный ремонт").Add("Финансирование").Add("Платежные поручения", "paymentorder").AddRequiredPermission("GkhCr.PaymentOrder.View").WithIcon("paymentOrder");

            root.Add("Жилищная инспекция").Add("Реестр уведомлений").Add("Реестр уведомлений фонда КР", "decisiomnoticeregister").AddRequiredPermission("Ovrhl.DecisionNoticeRegister.View");

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