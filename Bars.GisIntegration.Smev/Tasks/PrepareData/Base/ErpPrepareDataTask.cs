namespace Bars.GisIntegration.Smev.Tasks.PrepareData.Base
{
    using System.Xml.Serialization;

    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public abstract class ErpPrepareDataTask<TContentModel>: SmevPrepareDataTask<TContentModel, TatarstanDisposal>
    {
        /// <inheritdoc />
        protected override XmlSerializerNamespaces GetXmlSerializerNamespaces()
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            namespaces.Add("erp", "urn://ru.gov.proc.erp.communication/5.0.2");
            namespaces.Add("erp_types", "urn://ru.gov.proc.erp.communication/types/5.0.2");
            return namespaces;
        }
    }
}