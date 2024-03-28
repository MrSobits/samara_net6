namespace Bars.Gkh.Diagnostic
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Diagnostic.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add(
                "libs/B4/enums/CollectedDiagnosticResultState.js",
                new ExtJsEnumResource<CollectedDiagnosticResultState>("B4.enums.CollectedDiagnosticResultState"));
            container.Add(
                "libs/B4/enums/DiagnosticResultState.js",
                new ExtJsEnumResource<DiagnosticResultState>("B4.enums.DiagnosticResultState"));
        }
    }
}