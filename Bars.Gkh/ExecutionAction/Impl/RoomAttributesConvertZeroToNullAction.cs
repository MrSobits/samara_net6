namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;

    public class RoomAttributesConvertZeroToNullAction : BaseExecutionAction
    {
        public override string Description => @"Заменяет все значения 0 на пусто для полей Жилая площадь, Номер подъезда, Этаж, Количество комнат";

        public override string Name => "Заменить 0 на пустое значение для сведений о помещениях";

        public override Func<IDataResult> Action => this.ConevrtZeroToNull;

        private BaseDataResult ConevrtZeroToNull()
        {
            try
            {
                var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

                session.CreateSQLQuery(@"
                    update GKH_ROOM set LAREA = NULL where LAREA = 0;
                    update GKH_ROOM set ENTRANCE_NUM = NULL where ENTRANCE_NUM = 0;
                    update GKH_ROOM set ROOMS_COUNT = NULL where ROOMS_COUNT = 0;
                    update GKH_ROOM set FLOOR = NULL where FLOOR = 0;")
                    .ExecuteUpdate();
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }
    }
}