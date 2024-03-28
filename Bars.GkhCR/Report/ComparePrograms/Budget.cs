namespace Bars.GkhCr.Report.ComparePrograms
{
    internal class Budget
    {
        public Budget()
        {
        }

        public Budget(decimal budgetRf, decimal budgetRt, decimal budgetMu, decimal budgetOwners)
        {
            this.BudgetRf = budgetRf;
            this.BudgetRt = budgetRt;
            this.BudgetMu = budgetMu;
            this.BudgetOwners = budgetOwners;
        }

        public decimal BudgetRf { get; set; }

        public decimal BudgetRt { get; set; }

        public decimal BudgetMu { get; set; }

        public decimal BudgetOwners { get; set; }

        public Budget Clone()
        {
            return new Budget(this.BudgetRf, this.BudgetRt, this.BudgetMu, this.BudgetOwners);
        }

        public void Add(Budget rec)
        {
            this.BudgetRf += rec.BudgetRf;
            this.BudgetRt += rec.BudgetRt;
            this.BudgetMu += rec.BudgetMu;
            this.BudgetOwners += rec.BudgetOwners;
        }
    }
}
