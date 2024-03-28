namespace Bars.Gkh.DomainService.Documentation.Impl
{
    using Bars.B4.Config;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с документацией на проекте
    /// </summary>
    public class DocumentationService : IDocumentationService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Вовзаращет url-адрес документации
        /// </summary>
        /// <returns>Url-адрес документации</returns>
        public string GetDocumentationUrl()
        {
            var config = Container.Resolve<IConfigProvider>().GetConfig();

            return config.ModulesConfig.ContainsKey("Bars.GkhGji.Modules.Instructions") ? config.ModulesConfig["Bars.GkhGji.Modules.Instructions"].GetAs("UrlInstruction", string.Empty) : null;
        }
    }
}
