namespace Sobits.RosReg
{
    using Bars.B4;

    /// <summary>
    /// Манифест ресурсов.
    /// Используется для регистрации ресурсов модуля в общем контейере ресурсов.
    /// </summary>
    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {

            this.RegisterResource(container, "libs/B4/aspects/ExtractImportAspect.js");
            this.RegisterResource(container, "libs/B4/controller/Extract.js");
            this.RegisterResource(container, "libs/B4/controller/ExtractEgrn.js");
            this.RegisterResource(container, "libs/B4/controller/ExtractImport.js");
            this.RegisterResource(container, "libs/B4/model/AccountsForComparsionNew.js");
            this.RegisterResource(container, "libs/B4/model/Extract.js");
            this.RegisterResource(container, "libs/B4/model/ExtractEgrn.js");
            this.RegisterResource(container, "libs/B4/model/ExtractEgrnRight.js");
            this.RegisterResource(container, "libs/B4/model/ExtractEgrnRightInd.js");
            this.RegisterResource(container, "libs/B4/store/AccountsForComparsionNew.js");
            this.RegisterResource(container, "libs/B4/store/Extract.js");
            this.RegisterResource(container, "libs/B4/store/ExtractEgrn.js");
            this.RegisterResource(container, "libs/B4/store/ExtractEgrnRight.js");
            this.RegisterResource(container, "libs/B4/store/ExtractEgrnRightInd.js");
            this.RegisterResource(container, "libs/B4/view/ExtractImport.js");
            this.RegisterResource(container, "libs/B4/view/Extract/Grid.js");
            this.RegisterResource(container, "libs/B4/view/ExtractEgrn/EditWindow.js");
            this.RegisterResource(container, "libs/B4/view/ExtractEgrn/Grid.js");
            this.RegisterResource(container, "libs/B4/view/ExtractEgrn/IndGrid.js");

        }
    }
}
