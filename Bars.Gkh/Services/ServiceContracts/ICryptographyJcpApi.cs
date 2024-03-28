namespace Bars.Gkh.Services.ServiceContracts
{
    using System.Threading.Tasks;

    using Bars.Gkh.Dto;
    using Bars.Gkh.Models;

    using Refit;

    /// <summary>
    /// API сервиса криптографии на JCP
    /// </summary>
    public interface ICryptographyJcpApi
    {
        /// <summary>
        /// Получить хэш по содержимому
        /// </summary>
        /// <param name="algorithms"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Multipart]
        [Post("/v1/CryptoService/CreateHash")]
        Task<SignResponse> CreateHashAsync(string algorithms, StreamPart content);

        /// <summary>
        /// Создать открепленную подпись
        /// </summary>
        /// <param name="thumbprint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Multipart]
        [Post("/v1/CryptoService/CreateSignature")]
        Task<SignResponse> CreateSignatureAsync(string thumbprint, StreamPart content);

        /// <summary>
        /// Создать XAdES подпись
        /// </summary>
        /// <param name="thumbprint"></param>
        /// <returns></returns>
        [Post("/v1/CryptoService/CreateXAdESSignature")]
        Task<SignResponse> CreateXAdESSignatureAsync([Query] string thumbprint, [Body(BodySerializationMethod.Json)] XmlHash references);

        /// <summary>
        /// Подписать сообщение электронной подписью по стандарту XMLDsig
        /// </summary>
        /// <param name="thumbprint"></param>
        /// <param name="references"></param>
        /// <returns></returns>
        [Post("/v1/CryptoService/CreateXmlSignature")]
        Task<string> CreateXmlSignatureAsync([Query] string thumbprint, [Body(BodySerializationMethod.Json)] XmlHash references);

        /// <summary>
        /// Усовершенствование подписи семейства CAdES
        /// </summary>
        [Multipart]
        [Post("/v1/CryptoService/EnhanceSignature")]
        Task<SignResponse> EnhanceSignatureAsync(string cadesTypeTo, string cadesTypeFrom, string signature, bool bDetached, StreamPart content);

        /// <summary>
        /// Получить информацию из файла sig о сертификате
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [Multipart]
        [Post("/v1/CryptoService/GetSignInfo")]
        Task<CertificateDto> GetSignInfoAsync(StreamPart content);
    }
}