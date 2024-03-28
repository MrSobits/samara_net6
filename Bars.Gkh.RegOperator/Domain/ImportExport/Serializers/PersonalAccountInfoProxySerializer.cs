namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using Castle.Windsor;
    using ProxyEntity;

    public class PersonalAccountInfoProxySerializer : DefaultImportExportSerializer<PersonalAccountInfoProxy>
    {
        public PersonalAccountInfoProxySerializer(IWindsorContainer container) : base(container)
        {
        }
    }
}
