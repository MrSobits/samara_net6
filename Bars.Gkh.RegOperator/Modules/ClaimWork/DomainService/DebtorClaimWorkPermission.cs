namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using B4;

    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Маппинг прав доступа ПИР
    /// </summary>
    public class DebtorClaimWorkPermission : IClaimWorkPermission
    {
        private class DebtorPermission : PermissionMap
        {
            public DebtorPermission()
            {
                this.Namespace("Clw.ClaimWork.Legal", "Основание ПИР - Неплательщики-юр.лица");
                this.Permission("Clw.ClaimWork.Legal.View", "Просмотр");
                this.Permission("Clw.ClaimWork.Legal.Update", "Обновление");
                this.Permission("Clw.ClaimWork.Legal.Save", "Сохранение");
                this.Permission("Clw.ClaimWork.Legal.Print", "Массовая печать");

                this.Namespace("Clw.ClaimWork.Legal.Columns", "Столбцы");
                this.Permission("Clw.ClaimWork.Legal.Columns.DebtSum", "Общая сумма задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Legal.Columns.BaseTariffDebtSum", "Сумма задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Legal.Columns.DecisionTariffDebtSum", "Сумма задолженности по тарифу решения - Просмотр");

                this.Namespace("Clw.ClaimWork.Legal.CrDebt", "Задолженность по оплате КР");
                this.Permission("Clw.ClaimWork.Legal.CrDebt.CurrDebtSum", "Общая сумма текущей задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Legal.CrDebt.CurrBaseTariffDebtSum", "Сумма текущей задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Legal.CrDebt.CurrDecisionTariffDebtSum", "Сумма текущей задолженности по тарифу решения - Просмотр");
                this.Permission("Clw.ClaimWork.Legal.CrDebt.OrigDebtSum", "Общая сумма исходной задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Legal.CrDebt.OrigBaseTariffDebtSum", "Сумма исходной задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Legal.CrDebt.OrigDecisionTariffDebtSum", "Сумма исходной задолженности по тарифу решения - Просмотр");

                this.Namespace("Clw.ClaimWork.Individual", "Основание ПИР - Неплательщики-физ.лица");
                this.Permission("Clw.ClaimWork.Individual.View", "Просмотр");
                this.Permission("Clw.ClaimWork.Individual.Update", "Обновление");
                this.Permission("Clw.ClaimWork.Individual.Save", "Сохранение");
                this.Permission("Clw.ClaimWork.Individual.Delete", "Удаление");
                this.Permission("Clw.ClaimWork.Individual.Print", "Массовая печать");

                this.Namespace("Clw.ClaimWork.Individual.Columns", "Столбцы");
                this.Permission("Clw.ClaimWork.Individual.Columns.DebtSum", "Общая сумма задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.Columns.BaseTariffDebtSum", "Сумма задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.Columns.DecisionTariffDebtSum", "Сумма задолженности по тарифу решения - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.Columns.PrintAccountReport", "Отчет по ЛС - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.Columns.PrintAccountClaimworkReport", "Отчет по ЛС (ПИР) - Просмотр");

                this.Namespace("Clw.ClaimWork.Individual.CrDebt", "Задолженность по оплате КР");
                this.Permission("Clw.ClaimWork.Individual.CrDebt.CurrDebtSum", "Общая сумма текущей задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.CrDebt.CurrBaseTariffDebtSum", "Сумма текущей задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.CrDebt.CurrDecisionTariffDebtSum", "Сумма текущей задолженности по тарифу решения - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.CrDebt.OrigDebtSum", "Общая сумма исходной задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.CrDebt.OrigBaseTariffDebtSum", "Сумма исходной задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Individual.CrDebt.OrigDecisionTariffDebtSum", "Сумма исходной задолженности по тарифу решения - Просмотр");

                this.Namespace("Clw.ClaimWork.Debtor", "Основание ПИР - Неплательщики");
                this.Namespace("Clw.ClaimWork.Debtor.Pretension", "Претензия");
                this.Permission("Clw.ClaimWork.Debtor.Pretension.DebtPayment", "Оплата задолженности");
                this.Permission("Clw.ClaimWork.Debtor.Pretension.DebtSum", "Общая сумма задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.Pretension.BaseTariffDebtSum", "Сумма задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.Pretension.DecisionTariffDebtSum", "Сумма задолженности по тарифу решения - Просмотр");

                this.Namespace("Clw.ClaimWork.Debtor.LawsuitOwnerInfo", "Сведения о собственниках");
                this.Permission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate", "Расчитать долг - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.CalcPeriod", "Периоды расчета задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.BaseTariffDebtSum", "Новая задолженность по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DecisionTariffDebtSum", "Новая задолженность по тарифу решения - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.PenaltyDebt", "Новая задолженность по пени - Просмотр");

                this.Namespace("Clw.ClaimWork.Debtor.CourtOrderApplication", "Заявление о выдаче судебного приказа");
                this.Permission("Clw.ClaimWork.Debtor.CourtOrderApplication.DebtSum", "Общая сумма задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.CourtOrderApplication.BaseTariffDebtSum", "Сумма задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.CourtOrderApplication.DecisionTariffDebtSum", "Сумма задолженности по тарифу решения - Просмотр");

                this.Namespace("Clw.ClaimWork.Debtor.ClaimStatement", "Исковое заявление");
                this.Permission("Clw.ClaimWork.Debtor.ClaimStatement.DebtSum", "Общая сумма задолженности - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.ClaimStatement.BaseTariffDebtSum", "Сумма задолженности по базовому тарифу - Просмотр");
                this.Permission("Clw.ClaimWork.Debtor.ClaimStatement.DecisionTariffDebtSum", "Сумма задолженности по тарифу решения - Просмотр");

                this.Namespace<DebtorClaimWork>("Clw.ClaimWork.Debtor.RestructDebt", "Реструктуризация долга");
                this.Permission("Clw.ClaimWork.Debtor.RestructDebt.Save", "Сохранение");
                this.Permission("Clw.ClaimWork.Debtor.RestructDebt.Update", "Обновление");

                this.Namespace("Clw.ClaimWork.Debtor.RestructDebt.PaymentSchedule", "График реструктуризации");
                this.Permission("Clw.ClaimWork.Debtor.RestructDebt.PaymentSchedule.Add", "Добавление");
                this.Permission("Clw.ClaimWork.Debtor.RestructDebt.PaymentSchedule.Save", "Сохранение");

                this.Namespace("Clw.ClaimWork.Debtor.RestructDebtAmicAgr", "Реструктуризация по мировому соглашению");
                this.Permission("Clw.ClaimWork.Debtor.RestructDebtAmicAgr.Save", "Сохранение");
                this.Permission("Clw.ClaimWork.Debtor.RestructDebtAmicAgr.Update", "Обновление");

                this.Namespace("Clw.ClaimWork.Debtor.RestructDebtAmicAgr.PaymentSchedule", "График реструктуризации по мировому соглашению");
                this.Permission("Clw.ClaimWork.Debtor.RestructDebtAmicAgr.PaymentSchedule.Add", "Добавление");
                this.Permission("Clw.ClaimWork.Debtor.RestructDebtAmicAgr.PaymentSchedule.Save", "Сохранение");

                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.ClaimWorkForm", "Начать претензионную работу");
                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.UpdateJurInstitution", "Обновить судебные учреждения");

                this.Namespace("GkhRegOp.PersonalAccountOwner.Debtor.Column", "Столбцы");
                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.Column.MonthCount", "Количество месяцев просрочки оплаты");
                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.Column.HasClaimWork", "Претензионная работа");
                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.Column.CourtType", "Тип учреждения");
                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.Column.JurInstitution", "Краткое наименование учреждения");
                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.Columns.DebtSum", "Общая сумма задолженности - Просмотр");
                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.Columns.BaseTariffDebtSum", "Сумма задолженности по базовому тарифу - Просмотр");
                this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.Columns.DecisionTariffDebtSum", "Сумма задолженности по тарифу решения - Просмотр");
            }
        }

        public PermissionMap GetPermissionMap()
        {
            return new DebtorPermission();
        }
    }
}