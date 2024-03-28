namespace Bars.Gkh.Overhaul.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    public class RemovePaysizeMuDuplicatesAction : BaseExecutionAction
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        /// <summary>
        /// Тип хранимого параметра
        /// </summary>
        public override string Description => "";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Удаление дублей справочника \"Размер взноса на КР\"";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            try
            {
                var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

                session.CreateSQLQuery(@"
delete from OVRHL_PAYSIZE_REC_RET where record_id in (
	select
		tt.id
	from (
		select
			paysize_id,
			mu_id,
			max(id) as id,
			count(*) as cnt
		from ovrhl_paysize_rec
		group by paysize_id, mu_id
	) tt
		
	where tt.cnt > 1);
").ExecuteUpdate();

                session.CreateSQLQuery(@"
delete from ovrhl_paysize_rec where id in (
	select
		tt.id
	from (
		select
			paysize_id,
			mu_id,
			max(id) as id,
			count(*) as cnt
		from ovrhl_paysize_rec
		group by paysize_id, mu_id
	) tt
		
	where tt.cnt > 1);
").ExecuteUpdate();
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }
    }
}