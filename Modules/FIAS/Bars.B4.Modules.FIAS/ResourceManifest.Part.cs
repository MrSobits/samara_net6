namespace Bars.B4.Modules.FIAS
{    
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;

    public partial class ResourceManifest
    {

        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/FiasTypeRecordEnum.js", new ExtJsEnumResource<FiasTypeRecordEnum>("B4.enums.FiasTypeRecordEnum"));
            container.Add("libs/B4/enums/FiasCenterStatusEnum.js", new ExtJsEnumResource<FiasCenterStatusEnum>("B4.enums.FiasCenterStatusEnum"));
            container.Add("libs/B4/enums/FiasLevelEnum.js", new ExtJsEnumResource<FiasLevelEnum>("B4.enums.FiasLevelEnum"));
            container.Add("libs/B4/enums/FiasOperationStatusEnum.js", new ExtJsEnumResource<FiasOperationStatusEnum>("B4.enums.FiasOperationStatusEnum"));
            container.Add("libs/B4/enums/FiasActualStatusEnum.js", new ExtJsEnumResource<FiasActualStatusEnum>("B4.enums.FiasActualStatusEnum"));
        }

    }
}
