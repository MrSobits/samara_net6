using GisGkhLibrary.Crypto;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace GisGkhLibrary
{
    internal static class ServiceHelper<T, Q> where Q : class
                                             where T : System.ServiceModel.ClientBase<Q>
                                             , new()
    {

        internal static T MakeNew()
        {
            var service = new T();
            //service.ClientCredentials.UserName.UserName = Params.UserName;
            //service.ClientCredentials.UserName.Password = Params.Password;
            service.ClientCredentials.UserName.UserName = "sit";
            service.ClientCredentials.UserName.Password = "xw{p&&Ee3b9r8?amJv*]";

            //Убрать, когда ГИС ЖКХ переедет на сертфикат с неустаревшим шифрованием
            //ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            //{
            //    return (true);
            //};

            return service;
        }
    }
}
