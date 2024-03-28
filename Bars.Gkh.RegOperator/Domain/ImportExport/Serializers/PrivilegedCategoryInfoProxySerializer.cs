namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using Bars.Gkh.RegOperator.Domain.ProxyEntity;

    using Castle.Windsor;

    public class PrivilegedCategoryInfoProxySerializer : DefaultImportExportSerializer<PrivilegedCategoryInfoProxy>
    {
        public PrivilegedCategoryInfoProxySerializer(IWindsorContainer container)
            : base(container)
        {
        }
    }
}
