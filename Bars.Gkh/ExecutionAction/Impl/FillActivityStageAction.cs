namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Extensions;
    using Dapper;

    /// <summary>
    /// Заполнение стадий деятельности контрагентов и физ.лиц
    /// </summary>
    public class FillActivityStageAction : BaseMandatoryExecutionAction
    {
        private const int Timeout = 10000;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Заполнение стадий деятельности контрагентов и физ.лиц";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "ЖКХ - Заполнение стадий деятельности";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        public ISessionProvider SessionProvider { get; set; }

        private BaseDataResult Execute()
        {
            this.SessionProvider.InStatelessConnectionTransaction(
                (connection, transaction) => { connection.Execute(this.query, transaction: transaction, commandTimeout: FillActivityStageAction.Timeout); });

            return new BaseDataResult();
        }

        private string query = @"
            truncate GKH_ACTIVITY_STAGE;

            --физики
            insert into GKH_ACTIVITY_STAGE
            (
	            object_version, object_create_date, object_edit_date, 
	            ENTITY_ID, ENYITY_TYPE, DATE_START, DATE_END, 
	            ACTIVITY_STAGE_TYPE, DESCRIPTION, FILE_ID
            )
            select 
	             0, now()::date, now()::date,
	             o.id, 20, o.object_create_date, null,
	             60, null, null
            from regop_pers_acc_owner o
            where owner_type = 0;

            --контрагенты действующие
            insert into GKH_ACTIVITY_STAGE
            (
	            object_version, object_create_date, object_edit_date, 
	            ENTITY_ID, ENYITY_TYPE, DATE_START, DATE_END, 
	            ACTIVITY_STAGE_TYPE, DESCRIPTION, FILE_ID
            )
            select 
	             0, now()::date, now()::date,
	             c.id, 10, c.object_create_date, c.DATE_TERMINATION,
	             contragent_state, null, null
            from GKH_CONTRAGENT c;";
    }
}