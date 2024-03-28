namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Действие "Исправление трансферов обновления баланса кошелька"
    /// </summary>
    public class FixPersonalAccTransfersIsAffectedParameter : BaseExecutionAction
    {
        private readonly ISessionProvider sessionProvider;

        /// <summary>
        /// Коструктор
        /// </summary>
        /// <param name="sessionProvider">Провайдер сессии</param>
        public FixPersonalAccTransfersIsAffectedParameter(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
            =>
                "Исправление трансферов обновления баланса кошелька. При ручном изменении сальдо изменение садилось на кошелёк. Этот скрипт исправляет эту проблему"
            ;

        /// <summary>
        /// Имя действия
        /// </summary>
        public override string Name => "Исправление трансферов обновления баланса кошелька";

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;


        private BaseDataResult Execute()
        {
            var query = "update REGOP_TRANSFER set is_affect = false where reason = 'Установка/изменение сальдо'";
            using (var session = this.sessionProvider.OpenStatelessSession())
            {
                session.CreateSQLQuery(query).ExecuteUpdate();
            }

            return new BaseDataResult(true);
        }
    }
}