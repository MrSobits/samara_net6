namespace Bars.B4.Modules.ESIA.Auth
{
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {
            AddResource(container, "libs/B4/view/desktop/portlet/LoginForm.js");
            AddResource(container, "libs/B4/view/desktop/portlet/User.js");

            AddResource(container, "esia/oauthlogin.ashx");
            AddResource(container, "esia/oauthlogout.ashx");

            AddResource(container, "Views/Login/esia.cshtml");
            AddResource(container, "Views/OauthLogin/index.cshtml");
        }

        private void AddResource(IResourceManifestContainer container, string path)
        {
            container.Add(path, string.Format("Bars.B4.Modules.ESIA.Auth.dll/Bars.B4.Modules.ESIA.Auth.{0}", path.Replace("/", ".")));
        }
    }
}
