namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class SubsidyCopyToVersionAction : BaseExecutionAction
    {
        public IDomainService<SubsidyRecordVersion> SubsidyRecordVersDomain { get; set; }

        public override string Name => "Перенос данных из субсидирования МО по версиям субсидирования";

        public override string Description => @"Перенос данных из субсидирования МО по версиям субсидирования";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var subsidyVersRecs = this.SubsidyRecordVersDomain.GetAll()
                .ToList();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    subsidyVersRecs.ForEach(
                        x =>
                        {
                            x.SubsidyYear = x.SubsidyRecord.SubsidyYear;
                            x.BudgetRegion = x.SubsidyRecord.BudgetRegion;
                            x.BudgetMunicipality = x.SubsidyRecord.BudgetMunicipality;
                            x.BudgetFcr = x.SubsidyRecord.BudgetFcr;
                            x.BudgetOtherSource = x.SubsidyRecord.BudgetOtherSource;
                            x.PlanOwnerCollection = x.SubsidyRecord.PlanOwnerCollection;
                            x.PlanOwnerPercent = x.SubsidyRecord.PlanOwnerPercent;
                            x.NotReduceSizePercent = x.SubsidyRecord.NotReduceSizePercent;
                            x.OwnerSumForCr = x.SubsidyRecord.OwnerSumForCr;
                            x.DateCalcOwnerCollection = x.SubsidyRecord.DateCalcOwnerCollection;

                            this.SubsidyRecordVersDomain.Update(x);
                        });

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();

                        return new BaseDataResult
                        {
                            Success = false,
                            Message = exc.Message
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            return new BaseDataResult {Success = true};
        }
    }
}