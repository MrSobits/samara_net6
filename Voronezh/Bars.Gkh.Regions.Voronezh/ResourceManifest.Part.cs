namespace Bars.Gkh.Regions.Voronezh
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    // TODO: Расскоментировать после перевода FIAS
    //using Bars.B4.Modules.FIAS.Enums;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Voronezh.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            this.RegisterEnums(container);
        }

        private void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<RoomOwnershipType>();
            container.RegisterExtJsEnum<RepresentativeType>();
            container.Add("libs/B4/enums/RepresentativeType.js", new ExtJsEnumResource<RepresentativeType>("B4.enums.RepresentativeType"));
            //container.Add("libs/B4/enums/FiasEstimateStatusEnum.js", new ExtJsEnumResource<FiasEstimateStatusEnum>("B4.enums.FiasEstimateStatusEnum"));
        }
    }
}