namespace Bars.GkhGji.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    public class FillAppealSortGjiNumberValueAction : BaseExecutionAction
    {
        public override string Description => this.Name;

        public override string Name => "ГЖИ значение поля номер гжи копируется в числовую колонку для сортировки в гриде обращений";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            try
            {
                var sessionProvider = this.Container.Resolve<ISessionProvider>();
                var session = sessionProvider.OpenStatelessSession();
                bool isOracle = session.Connection.GetType().Name.ToLower().Contains("oracle");

                try
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            // выбираем из таблицы только те записи, у кого GJI_NUMBER можно преобразовать в число. И для этих записей проставляем GJI_NUM_SORT
                            if (isOracle)
                            {
                                // отдельная логика для Oracle
                                session.CreateSQLQuery(@"UPDATE GJI_APPEAL_CITIZENS 
                                                    SET GJI_NUM_SORT = TO_NUMBER(GJI_NUMBER) 
                                                    WHERE decode(length(trim(translate(GJI_NUMBER, '1234567890', ' '))), null, 't', 'f') = 't'")
                                    .ExecuteUpdate();
                            }
                            else
                            {
                                // отдельная логика для Postgre
                                // to_number мощная штука, поэтому преобразуем только значения, которые обязательно начинаются с числа, а в конце могут иметь буквы
                                session.CreateSQLQuery(@"UPDATE GJI_APPEAL_CITIZENS 
                                                    SET GJI_NUM_SORT = to_number(GJI_NUMBER, '99999999999') 
                                                    WHERE (GJI_NUMBER ~ '^\d+[a-zA-Zа-яА-Я]*$') = 't'").ExecuteUpdate();
                            }

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                finally
                {
                    sessionProvider.CloseCurrentSession();
                }

                return new BaseDataResult(true);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}