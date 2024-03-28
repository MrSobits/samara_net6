namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class SetContragentMoSettlementAction : BaseExecutionAction
    {
        public IRepository<Contragent> ContragentRepository { get; set; }

        public override string Description => "Проставление муниципальных образований для уже созданных контрагентов";

        public override string Name => "Проставление муниципальных образований для уже созданных контрагентов";

        public override Func<IDataResult> Action => this.SetMoSettlement;

        public BaseDataResult SetMoSettlement()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var contragents = this.ContragentRepository.GetAll()
                        .Where(x => x.MoSettlement == null)
                        .ToArray();

                    foreach (var agent in contragents)
                    {
                        if (agent.FiasJuridicalAddress != null)
                        {
                            var mo = Utils.GetMoSettlement(this.Container, agent.FiasJuridicalAddress);
                            if (mo != null)
                            {
                                agent.MoSettlement = mo;
                                this.ContragentRepository.Update(agent);
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }
    }
}