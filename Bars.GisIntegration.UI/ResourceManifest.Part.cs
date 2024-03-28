namespace Bars.GisIntegration.UI
 {
     using Bars.B4;
     using Bars.B4.Modules.ExtJs;
     using Bars.GisIntegration.Base.Enums;
     using Bars.Gkh.Quartz.Scheduler.Log;

     public partial class ResourceManifest
     {
         protected void AdditionalInit(IResourceManifestContainer container)
         {
             container.RegisterExtJsEnum<ObjectValidateState>();
             container.RegisterExtJsEnum<MessageType>();
             container.RegisterExtJsEnum<ObjectProcessingState>();
             container.RegisterExtJsEnum<IntegrationService>();
             container.RegisterExtJsEnum<DictionaryState>();
             container.RegisterExtJsEnum<DictionaryGroup>();
             container.RegisterExtJsEnum<FileStorageName>();
         }
     }
 }