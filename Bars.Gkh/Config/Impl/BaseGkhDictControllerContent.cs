namespace Bars.Gkh.ContentResource
{
    using System;
    using System.IO;
    using System.Text;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Автоматически генерируемый js-файл клиентского контроллера справочника <see cref="BaseGkhDict"/>
    /// </summary>
    public class BaseGkhDictControllerContent : ContentResource
    {
        private readonly byte[] content;

        private readonly string _controllerPath;

        public BaseGkhDictControllerContent(
            string controllerName, 
            string controllePath, 
            string title, 
            string permissionPrefix = null, 
            string mainView = null, 
            string mainViewSelector = null)
        {
            this._controllerPath = controllePath;

            controllePath = controllePath ?? $"B4.controller.dict.{controllerName}";

            this.content = Encoding.UTF8.GetBytes(
$@"Ext.define('{controllePath}', {{
    extend: 'B4.controller.dict.BaseGkhDict',
    controllerName: '{controllerName}',
    title: '{title}',
    permissionPrefix: '{permissionPrefix}',
    mainView: '{mainView ?? "dict.BaseGkhDictGrid"}',
    mainViewSelector: '{mainViewSelector ?? "basegkhdictgrid"}'
}});");
        }

        /// <inheritdoc />
        public override Stream CreateReadStream()
        {
            return new MemoryStream(this.content);
        }

        /// <inheritdoc />
        public override bool Exists => true;

        /// <inheritdoc />
        public override bool IsDirectory => false;

        /// <inheritdoc />
        public override DateTimeOffset LastModified => DateTimeOffset.UtcNow;

        /// <inheritdoc />
        public override string Name => $"{this._controllerPath}.js";

        /// <inheritdoc />
        public override string PhysicalPath => null;
    }
}