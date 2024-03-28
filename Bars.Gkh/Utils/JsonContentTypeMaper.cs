using System.ServiceModel.Channels;

namespace Bars.Gkh.Utils
{
    using CoreWCF.Channels;

    public class JsonContentTypeMaper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Json;
        }
    }
}
