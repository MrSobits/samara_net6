namespace Bars.GisIntegration.Base
 {
     using Bars.B4;
     using Bars.B4.Modules.ExtJs;
     using Bars.GisIntegration.Base.Enums;

     public partial class ResourceManifest : ResourceManifestBase
    {
         protected override void AdditionalInit(IResourceManifestContainer container)
         {
            container.RegisterExtJsEnum<HouseType>();
            container.RegisterExtJsEnum<MeteringDeviceValueType>();
            container.RegisterExtJsEnum<MeteringDeviceType>();
            container.RegisterExtJsEnum<TaskState>();
        }
     }
 }