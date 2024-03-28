namespace Bars.Gkh.ClaimWork
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Namespace("Clw", "Модуль Претензионная и исковая работа");

            this.Namespace("Clw.FlattenedClaimWork", "Долевые ПИР");
            this.CRUDandViewPermissions("Clw.FlattenedClaimWork");

            // Справочники
            this.Dictionaries();

            this.Namespace("Clw.ClaimWork", "Основания ПИР");

            this.Namespace("Clw.JurJournal", "Журнал судебной практики");
            this.Permission("Clw.JurJournal.View", "Просмотр");

            this.Namespace("Clw.DocumentRegister", "Реестр документов ПИР");
            this.Permission("Clw.DocumentRegister.View", "Просмотр");
            this.Permission("Clw.DocumentRegister.LawsuitOwnerInfo_View", "Сведения о собственниках-Просмотр");

            this.Namespace("Clw.ClaimWork.Debtor", "Основание ПИР - Неплательщики");
            this.Namespace("Clw.ClaimWork.Debtor.Pretension", "Претензия");
            this.Permission("Clw.ClaimWork.Debtor.Pretension.PaymentPlannedPeriodField_View", "Планируемый срок оплаты");
            this.Permission("Clw.ClaimWork.Debtor.Pretension.PaymentPlannedPeriodColumn_View", "Планируемый срок оплаты - Столбец");

            this.Permission("Clw.ClaimWork.Debtor.CourtOrderApplication.Print", "Кнопка отчетов - Показать");
            this.Permission("Clw.ClaimWork.Debtor.CourtOrderApplication.Delete", "Кнопка удаления - Показать");

            this.Namespace("Clw.ClaimWork.AgentPIR", "Агент ПИР");
            this.Permission("Clw.ClaimWork.AgentPIR.View", "Просмотр Агент ПИР");
        }

        private void Dictionaries()
        {
            this.Namespace("Clw.Dictionaries", "Справочники");

            this.Namespace("Clw.Dictionaries.ViolClaimWork", "Виды нарушений договора подряда");
            this.CRUDandViewPermissions("Clw.Dictionaries.ViolClaimWork");

            this.Namespace("Clw.Dictionaries.JurInstitution", "Учреждения в судебной практике");
            this.Permission("Clw.Dictionaries.JurInstitution.View", "Просмотр");

            this.Namespace("Clw.Dictionaries.PetitionToCourt", "Заявления в суд");
            this.Permission("Clw.Dictionaries.PetitionToCourt.View", "Просмотр");

            this.Namespace("Clw.Dictionaries.StateDuty", "Госпошлина");
            this.Permission("Clw.Dictionaries.StateDuty.View", "Просмотр");
        }
    }
}