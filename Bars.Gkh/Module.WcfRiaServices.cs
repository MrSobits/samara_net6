namespace Bars.Gkh
{
    using B4.IoC;

    using Bars.Gkh.Services.Impl.DataTransfer;
    using Bars.Gkh.Services.ServiceContracts.DataTransfer;
    using Bars.Gkh.SystemDataTransfer.Utils;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Registration;
    using Services.Impl;
    using Services.Impl.GlonassIntegration;
    using Services.Impl.Suggestion;
    using Services.ServiceContracts;
    using Services.ServiceContracts.GlonassIntegration;
    using Services.ServiceContracts.Suggestion;

    public partial class Module
    {
        /// <summary>
        /// Регистрация сервисов
        /// </summary>
        public void RegisterWcfRiaServices()
        {


            // TODO wcf
            // Component.For<IService>().ImplementedBy<Service>().AsWcfSecurityService().RegisterIn(this.Container);
            // Component.For<ISuggestionService>().ImplementedBy<SuggestionService>().AsWcfSecurityService().RegisterIn(this.Container);
            // Component.For<IGlonassIntegService>().ImplementedBy<GlonassIntegService>().AsWcfSecurityService().RegisterIn(this.Container);
            // Component.For<IDataTransferService>().ImplementedBy<DataTransferService>().AsWcfSecurityService().RegisterIn(this.Container);
            
            Component.For<AuthHeaderEndpointBehavior>().RegisterIn(this.Container);
        }
    }
}
