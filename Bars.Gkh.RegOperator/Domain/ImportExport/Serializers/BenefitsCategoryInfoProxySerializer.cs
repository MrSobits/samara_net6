namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using Bars.Gkh.RegOperator.Domain.ProxyEntity;

    using Castle.Windsor;

    public class BenefitsCategoryInfoProxySerializer : DefaultImportExportSerializer<BenefitsCategoryInfoProxy>
    {
        public BenefitsCategoryInfoProxySerializer(IWindsorContainer container)
            : base(container)
        {
        }
    }
}
