namespace Bars.Gkh1468.Domain.PassportImport.Impl
{
    using System.Collections.Generic;
    using B4;

    using Bars.Gkh.Authentification;

    using Castle.Windsor;
    using DataProcessor;
    using DataProvider;
    using Gkh.Import;
    using Interfaces;

    public class DataProcessorFactory : IDataProcessorFactory
    {
        public IWindsorContainer Container { get; set; }
        public IGkhUserManager UserManager { get; set; }

        public IDataProcessor CreateDataProcessor(BaseParams baseParams, IDictionary<string, object> extraArgs)
        {
            var type = baseParams.Params.GetAs<ImportType1468>("import_type");
            var logger = extraArgs.ContainsKey("logger") ? extraArgs["logger"] as ILogImport : null;
            var allowImportWoDigitSign = extraArgs.ContainsKey("allowImportWoDigitSign") && ((extraArgs["allowImportWoDigitSign"] as bool?) ?? false);

            switch (type)
            {
                case ImportType1468.ONLINE:
                {
                    var provider = new OnlineServiceDataProvider(baseParams, logger, Container);

                    return new OnlineServiceDataProcessor(provider, Container);
                }
                case ImportType1468.ARCHIVE:
                return new ArchiveDataProcessor(baseParams, new SignedArchiveDataProvider(baseParams, logger, UserManager, allowImportWoDigitSign), Container, logger);
                default:
                    return new NullProcessor();
            }
        }
    }
}