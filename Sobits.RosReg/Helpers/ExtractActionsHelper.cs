namespace Sobits.RosReg.Helpers
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;

    using Dapper;

    public static class ExtractActionsHelper
    {
        public static long GetExtractIdForClw(BaseParams baseParams)
        {
            var clwId = baseParams.Params.GetAs<long>("clwId");
            var container = ApplicationContext.Current.Container;
            var statelessSession = container.Resolve<ISessionProvider>().OpenStatelessSession();
            var connection = statelessSession.Connection;
            var sql = $@"select ex.id from clw_claim_work_acc_detail ccwad
                        join regop_pers_acc rpa on ccwad.account_id=rpa.id
                        join rosreg.extractegrn egrn on rpa.room_id=egrn.roomid
                        join rosreg.extract ex on egrn.extractid=ex.id
                        where ccwad.claim_work_id={clwId}
                        order by 1 desc";
            var res = connection.Query<long>(sql).FirstOrDefault();
            return res;
        }

        public static long GetExtractIdForDebtor(BaseParams baseParams)
        {
            var debtorId = baseParams.Params.GetAs<long>("Id");
            var container = ApplicationContext.Current.Container;
            var statelessSession = container.Resolve<ISessionProvider>().OpenStatelessSession();
            var connection = statelessSession.Connection;
            var sql = $@"select ex.id from regop_pers_acc rpa
                        join rosreg.extractegrn egrn on rpa.room_id=egrn.roomid
                        join rosreg.extract ex on egrn.extractid=ex.id
                        where rpa.id = {debtorId}
                        order by 1 desc";
            var res = connection.Query<long>(sql).FirstOrDefault();
            return res;
        }
    }
}