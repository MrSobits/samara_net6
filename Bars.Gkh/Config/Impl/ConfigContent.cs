namespace Bars.Gkh.Config.Impl
{
    using System;
    using System.IO;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.Properties;

    using Castle.Windsor;

    /// <summary>
    /// Автоматически генерируемый js-файл настроек Gkh.config <see cref="Resources.GkhConfigTemplate"/>
    /// </summary>
    public class ConfigContent : ContentResource
    {
        private readonly IWindsorContainer container = ApplicationContext.Current.Container;

        /// <inheritdoc />
        public override Stream CreateReadStream()
        {
            var configService = this.container.Resolve<IGkhConfigService>();
            var content = Resources.GkhConfigTemplate.Replace("'{CONFIG_TEMPLATE}'", configService.GetSerializedConfig());
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        /// <inheritdoc />
        public override bool Exists => true;

        /// <inheritdoc />
        public override bool IsDirectory => false;

        /// <inheritdoc />
        public override DateTimeOffset LastModified => DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public override string Name => "GkhConfigTemplate.js";

        /// <inheritdoc />
        public override string PhysicalPath => null;
    }
}