namespace Bars.Gkh.Repair
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Repair.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeProgramRepairState.js", new ExtJsEnumResource<TypeProgramRepairState>("B4.enums.TypeProgramRepairState"));
            container.Add("libs/B4/enums/TypeVisibilityProgramRepair.js", new ExtJsEnumResource<TypeVisibilityProgramRepair>("B4.enums.TypeVisibilityProgramRepair"));
        }
    }
}