namespace Bars.Gkh1468.Wcf
{
    using System.ServiceModel;
    using CoreWCF.Web;

    public partial interface IPassportService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "CreateHousePassports/{year},{month}")]
        string CreateHousePassports(string year, string month);
    }
}