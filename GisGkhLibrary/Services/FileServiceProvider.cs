using GisGkhLibrary.Enums;

namespace GisGkhLibrary.Services
{
    /// <summary>
    /// Service provider для файлового сервиса
    /// </summary>
    public class FileServiceProvider
    {
        static FileServicePortClient service;

        static FileServicePortClient ServiceInstance => service ?? (service = ServiceHelper<FileServicePortClient, FileServicePort>.MakeNew());

        private string serviceAddress;

        /// <summary>
        /// Хранилище ГИС ЖКХ
        /// </summary>
        public GisFileRepository FileStorageType;

        /// <summary>
        /// Адрес сервиса
        /// </summary>
        public string ServiceAddress
        {
            get
            {
                if (string.IsNullOrEmpty(this.serviceAddress))
                {
                    var context = this.FileStorageType.ToString().Replace("_", " - ");
                    var address = ServiceInstance.Endpoint.Address.ToString();
                    this.serviceAddress = $"{address.TrimEnd('/')}/{context}";
                }

                return this.serviceAddress;
            }
        }
    }

    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://dom.gosuslugi.ru/schema/integration/file-store-service/rest/", ConfigurationName = "FileServiceProvider.FileServicePort")]
    public interface FileServicePort
    {
    }

    public partial class FileServicePortClient : System.ServiceModel.ClientBase<GisGkhLibrary.Services.FileServicePort>
    {

        public FileServicePortClient()
        {
        }

        public FileServicePortClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public FileServicePortClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public FileServicePortClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public FileServicePortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }
    }
}