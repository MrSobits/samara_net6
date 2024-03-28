namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Удаление дубликатов в разделе Реестр жилых домов включенных в договор
    /// </summary>
    public class DeleteDuplicatesTransferObjects : BaseExecutionAction
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IDomainService<TransferObject> transferObjectDomain;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sessionProvider">Провайдер сессии</param>
        /// <param name="transferObjectDomain">Домен-сервис сущности связи Дом - Договор найма</param>
        public DeleteDuplicatesTransferObjects(
            ISessionProvider sessionProvider,
            IDomainService<TransferObject> transferObjectDomain)
        {
            this.sessionProvider = sessionProvider;
            this.transferObjectDomain = transferObjectDomain;
        }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Удаление дубликатов в разделе Реестр жилых домов включенных в договор\n" +
            "Региональный фонд -> Перечисление средств в фонд -> Договор -> Период -> Реестр жилых домов включенных в договор";

        /// <summary>
        /// Наименование действия
        /// </summary>
        public override string Name => "Удаление дубликатов в разделе Реестр жилых домов включенных в договор";

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var transferHiresDict = this.transferObjectDomain.GetAll()
                .ToList()
                .GroupBy(x => x.TransferRecord.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y
                        .GroupBy(x => x.RealityObject.Id)
                        .Where(x => x.Count() > 1)
                        .ToDictionary(x => x.Key, ro => ro.ToList()));

            var listToDelete =
                transferHiresDict.SelectMany(transferHireDict => transferHireDict.Value).SelectMany(x => x.Value.Skip(1).Select(y => y.Id)).ToList();

            using (var session = this.sessionProvider.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var section in listToDelete.Section(1000))
                        {
                            session.CreateSQLQuery("DELETE FROM REGOP_TRANSFER_OBJ where id in (:ids)")
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