namespace Bars.Gkh.RegOperator.Regions.Tatarstan.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    /// <summary>
    /// Действие "Удаление дубликатов в реестре Перечисления по найму"
    /// </summary>
    public class DeleteDuplicatesTransferHireTat : BaseExecutionAction
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IDomainService<TransferHire> transferHireDomain;

        public DeleteDuplicatesTransferHireTat(
            ISessionProvider sessionProvider,
            IDomainService<TransferHire> transferHireDomain)
        {
            this.sessionProvider = sessionProvider;
            this.transferHireDomain = transferHireDomain;
        }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Удаление дубликатов в реестре Перечисления по найму\n" +
            "Региональный фонд -> Перечисление средств в фонд -> Договор -> Период -> Перечисление по найму";

        /// <summary>
        /// Наименование действия
        /// </summary>
        public override string Name => "Удаление дубликатов в реестре Перечисления по найму";

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var transferHiresDict = this.transferHireDomain.GetAll()
                .ToList()
                .GroupBy(x => x.TransferRecord.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y
                        .GroupBy(x => x.Account.Id)
                        .Where(x => x.Count() > 1)
                        .ToDictionary(x => x.Key, hires => hires.ToList()));

            var listToDelete = new List<long>();

            foreach (var transferHireDict in transferHiresDict)
            {
                foreach (var tranferHires in transferHireDict.Value)
                {
                    listToDelete.AddRange(tranferHires.Value.Skip(1).Select(x => x.Id));
                }
            }

            using (var session = this.sessionProvider.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var section in listToDelete.Section(1000))
                        {
                            session.CreateSQLQuery("DELETE FROM REGOP_TRANSFER_HIRE where id in (:ids)")
                                .SetParameterList("ids", section)
                                .ExecuteUpdate();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BaseDataResult.Error(ex.Message);
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}