// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Меню, навигация
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhCr
{
    using Bars.B4;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
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

        public void Init(MenuItem root)
        {
            var programCr = root.Add("Капитальный ремонт").Add("Программы капитального ремонта");

            programCr.Add("Программы капитального ремонта", "programcr").AddRequiredPermission("GkhCr.ProgramCr.View").WithIcon("programmCr");
            programCr.Add("Контрольные сроки видов работ", "controldate").AddRequiredPermission("GkhCr.ControlDate.View");
            programCr.Add("Объекты капитального ремонта", "objectcr").AddRequiredPermission("GkhCr.ObjectCrViewCreate.View").WithIcon("objectCr");
            programCr.Add("Реестр смет", "estimateregister").AddRequiredPermission("GkhCr.Estimate.View").WithIcon("estimate");
            programCr.Add("Реестр актов выполненных работ", "wrkactregister").AddRequiredPermission("GkhCr.WorkAct.View").WithIcon("performedWorkAct");
            programCr.Add("Массовая смена статусов объектов КР", "objcrmasschangestate").AddRequiredPermission("GkhCr.ObjectCrMassStateChange.View").WithIcon("objCrMassChangeStatus");
            programCr.Add("Квалификационный отбор", "qualificationregister").AddRequiredPermission("GkhCr.QualificationMember.View").WithIcon("qualification");
            programCr.Add("Конкурсы", "competition").WithIcon("competition").AddRequiredPermission("GkhCr.Competition.View");
            programCr.Add("Объекты капитального ремонта (работы)", "workscr").AddRequiredPermission("GkhCr.TypeWorkCr.View");
            programCr.Add("Объекты капитального ремонта для владельцев спец. счетов", "specialobjectcr").AddRequiredPermission("GkhCr.SpecialObjectCrViewCreate.View").WithIcon("objectCr");

            programCr.Add("Предложения по капремонту", "overhaulpropose").AddRequiredPermission("GkhCr.OverhaulProposal.View");
            programCr.Add("Массовые договора", "massbuildcontract").AddRequiredPermission("GkhCr.OverhaulProposal.View");

            root.Add("Капитальный ремонт").Add("Финансирование").Add("Банковские выписки", "bankstatement").AddRequiredPermission("GkhCr.BankStatement.View").WithIcon("bankStatement");
            
            var dictsCr = root.Add("Справочники").Add("Капитальный ремонт");
            dictsCr.Add("Разрезы финансирования", "financesource").AddRequiredPermission("GkhCr.Dict.FinanceSource.View");
            dictsCr.Add("Этапы работ", "stageworkcr").AddRequiredPermission("GkhCr.Dict.StageWorkCr.View");
            dictsCr.Add("Участники квалификационного отбора", "qualificationmember").AddRequiredPermission("GkhCr.Dict.QualMember.View");
            dictsCr.Add("Должностные лица", "official").AddRequiredPermission("GkhCr.Dict.Official.View");
            dictsCr.Add("Причины расторжения договора", "terminationreason").AddRequiredPermission("GkhCr.Dict.TerminationReason.View");
            dictsCr.Add("Вид документа основания ДПКР", "basisoverhauldockind").AddRequiredPermission("GkhCr.Dict.BasisOverhaulDocKind.View");

            programCr.Add("Подрядчики, нарушившие условия договора", "builderviolator").AddRequiredPermission("GkhCr.BuilderViolator.View");

            root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт выполненных работ", "performedworkimport")
                .AddRequiredPermission("Import.PerformedWork.View");

            root.Add("Капитальный ремонт")
                .Add("Архив файлов")
                .Add("Архив файлов ОКР", "crfileregister")
                .AddRequiredPermission("GkhCr.CrFileRegister.View");
        }
    }
}