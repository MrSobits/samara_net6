namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;

    /// <summary>
    /// Действие исправляет ошибки в копиях трансферов на дом
    /// </summary>
    [Obsolete("Не использует новую модель трансферов, исправить перед регистрацией")] // пытался понять, для чего действие, не понял, просто отключил
    public class TransferCorrectionAction : BaseExecutionAction
    {
        /// <summary>
        /// Код действия
        /// </summary>
        public static string Code = "TransferCorrectionAction";

        /// <summary>
        /// Флаг обязательности выполнения
        /// </summary>
        public bool IsMandatory => false;

        /// <summary>
        /// Параметры задачи
        /// </summary>
        public BaseParams ExecutionParams { get; set; }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Исправляет ошибки, связанные с копиями трансферов на дом";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Корректировка трансферов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => TransferCorrectionAction.Execute;

        /// <summary>
        /// IoC
        /// </summary>
        private static BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;

            ISessionProvider sessions = container.Resolve<ISessionProvider>();

            using (var session = sessions.OpenStatelessSession())
            {
                using (var tr = session.BeginTransaction())
                {
                    //1. удаляем is_indirect is_copy_for_source трансферы
                    var query = session.CreateSQLQuery(@"
delete from regop_transfer
where is_copy_for_source and is_indirect;
");
                    query.ExecuteUpdate();

                    //2. если у трансфера отмены был неверный originator_id, проставляем верный
                    query = session.CreateSQLQuery(@"
update 
regop_transfer t2
set originator_id=cancel_orig_id
from
(
    select t1.id as cancel_orig_id, copy_id from 
    (
        select t.id as copy_id,tt.id as orig_id from regop_transfer t 
        join regop_transfer tt on tt.id=t.originator_id
        where t.reason like 'Отмена%'
            and t.is_copy_for_source 
            and tt.originator_id is null
    ) res
    join 
    regop_transfer t1
    on res.orig_id=t1.originator_id and copy_id<>t1.id
    where not t1.is_copy_for_source and reason like 'Отмена%'
) y
where copy_id=t2.id;
");
                    query.ExecuteUpdate();

                    try
                    {
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}