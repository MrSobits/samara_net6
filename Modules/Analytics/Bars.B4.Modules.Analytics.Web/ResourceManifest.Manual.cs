namespace Bars.B4.Modules.Analytics.Web
{
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Modules.Analytics.Filters;
    using Bars.B4.Modules.ExtJs;

    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            var dataSourceModel = new ExtJsModelResource<DataSource>("B4.model.al.DataSource");
            dataSourceModel.GetModelMeta().Exclude(x => x.SystemFilterBytes);
            dataSourceModel.GetModelMeta().Exclude(x => x.DataFiletrBytes);
            dataSourceModel.GetModelMeta().Controller("DataSource");
            container.Add("libs/B4/model/al/DataSource.js", dataSourceModel);

            var filterExprModel = new ExtJsModelResource<FilterExprProvider>("B4.model.al.FilterExpression");
            var filterExprModelMeta = filterExprModel.GetModelMeta();
            filterExprModelMeta.IdProperty("Key");
            filterExprModelMeta.Controller("FilterExpression");
            container.Add("libs/B4/model/al/FilterExpression.js", filterExprModel);

            container.Add("libs/B4/enums/al/ParamType.js", new ExtJsEnumResource<ParamType>("B4.enums.al.ParamType"));
            container.Add("libs/B4/enums/al/OwnerType.js", new ExtJsEnumResource<OwnerType>("B4.enums.al.OwnerType"));
            container.Add("libs/B4/enums/al/SystemFilterGroup.js", new ExtJsEnumResource<SystemFilterGroup>("B4.enums.al.SystemFilterGroup"));
        }
    }
}
