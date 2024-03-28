namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Dto;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Models;
    using Bars.Gkh.Services.ServiceContracts;

    using Refit;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
    
    /// <inheritdoc />
    public class CryptoService : ICryptoService
    {
        #region Dependency Injection
        private readonly ICryptographyJcpApi _jcpApi;
        private readonly IFileManager _fileManager;

        public CryptoService(ICryptographyJcpApi jcpApi, IFileManager fileManager)
        {
            _jcpApi = jcpApi;
            _fileManager = fileManager;
        }
        #endregion
        
        /// <inheritdoc />
        public async Task<CertificateDto> GetCertificateInfoAsync(string sign)
        {
            using (var content = new MemoryStream(Convert.FromBase64String(sign)))
            {
                CertificateDto response;

                try
                {
                    response = await _jcpApi.GetSignInfoAsync(new StreamPart(content, "content.bin"));
                }
                catch (ApiException ex)
                {
                    throw new Exception(ex.Content);
                }

                return response;
            }
        }

        /// <inheritdoc />
        public async Task<byte[]> EnhanceSignatureAsync(string signature, FileInfo file, CadesType cadesTypeFrom, CadesType cadesTypeTo, bool bDetached = true)
        {
            using (var originalContent = _fileManager.GetFile(file))
            using (var ms = new MemoryStream())
            {
                await originalContent.CopyToAsync(ms);

                return await EnhanceSignatureAsync(signature, ms.ToArray(), cadesTypeFrom, cadesTypeTo, bDetached);
            }
        }
        
        /// <inheritdoc />
        public async Task<byte[]> EnhanceSignatureAsync(string signature, byte[] content, CadesType cadesTypeFrom, CadesType cadesTypeTo, bool bDetached = true)
        {
            var clearSignature = signature.Replace("\n", string.Empty).Replace("\r", string.Empty);
            SignResponse response;
            
            try
            {
                response = await _jcpApi.EnhanceSignatureAsync(((int)cadesTypeTo).ToString(), ((int)cadesTypeFrom).ToString(), clearSignature, bDetached,
                    new StreamPart(new MemoryStream(content), "content.bin"));
            }
            catch (ApiException ex)
            {
                throw new Exception(ex.Content);
            }

            return response.Signature;
        }

        /// <inheritdoc />
        public async  Task<byte[]> CreateHashAsync(string signatureAlgorithm, FileInfo file)
        {
            using (var originalContent = _fileManager.GetFile(file))
            using (var ms = new MemoryStream())
            {
                await originalContent.CopyToAsync(ms);

                return await CreateHashAsync(signatureAlgorithm, ms.ToArray());
            }
        }

        /// <inheritdoc />
        public async Task<byte[]> CreateHashAsync(string signatureAlgorithm, byte[] content)
        {
            SignResponse response;

            try
            {
                response = await this._jcpApi.CreateHashAsync(signatureAlgorithm, new StreamPart(new MemoryStream(content), "content.bin"));
            }
            catch (ApiException ex)
            {
                throw new Exception(ex.Content);
            }

            return response.Hash;
        }
    }
}