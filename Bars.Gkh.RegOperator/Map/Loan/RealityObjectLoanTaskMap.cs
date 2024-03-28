namespace Bars.Gkh.RegOperator.Map.Loan
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Loan;

    public class RealityObjectLoanTaskMap : PersistentObjectMap<RealityObjectLoanTask>
    {
        /// <inheritdoc />
        public RealityObjectLoanTaskMap()
            : base("Bars.Gkh.RegOperator.Entities.Loan.RealityObjectLoanTask", "REGOP_RO_LOAN_TASK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.RealityObject, "Дом").Column("RO_ID").NotNull();
            this.Reference(x => x.Task, "Задача").Column("TASK_ID").NotNull();
        }
    }
}