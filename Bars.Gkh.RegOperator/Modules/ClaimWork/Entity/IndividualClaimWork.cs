namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Enums.ClaimWork;

    /// <summary>
    /// Основание ПИР для неплательщиков - физ. лиц
    /// </summary>
    public class IndividualClaimWork : DebtorClaimWork
    {
        public IndividualClaimWork()
        {
            this.DebtorType = DebtorType.Individual;
        }
    }
}