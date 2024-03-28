namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Administration.SystemDataTransfer;
    using Bars.Gkh.Extensions;

    public static class DataTransferIntegrationSessionExtension
    {
        public static DataTransferIntegrationSession UpdateStateless(this DataTransferIntegrationSession session)
        {
            ApplicationContext.Current.Container.Resolve<ISessionProvider>()
                .InStatelessTransaction(statelessSession =>
                {
                    statelessSession.InsertOrUpdate(session);
                });

            return session;
        }
    }
}