namespace Bars.GkhGji.Regions.Chelyabinsk.Entities.SMEV
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Cryptography.Xml;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using CryptoPro.Sharpei.Xml;
    using SmevRef;
    using System.Xml.Serialization;
    using System.IO;
    using System.Net;

    public static class SmevWebRequest
    {
        private const String destinationUrl = "http://10.0.40.213/remote_dis_proxy/ws/SMEVMessageExchangePortTypeImpl";
        private const String destinationUrlGmp = "http://smev-mvf.test.gosuslugi.ru:7777/gateway/services/SID0003663?wsdl";

        private const String testDestinationUrl = "http://smev3-n0.test.gosuslugi.ru:7500/smev/v1.1/ws";
      
        public static HttpWebResponse SendRequest(String action, XmlDocument soapEnvelopeXmlBase, out Boolean isError)
        {
            isError = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers.Add("SOAPAction", action);
            request.Headers.Add("Type", "Request-Response");
            //request.Headers.Add("Receives Attachments", "false");
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Method = "POST";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(soapEnvelopeXmlBase.OuterXml);

            writer.Close();
            request.GetRequestStream().Flush();

            string errDescription = string.Empty;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                errDescription = wex.Message;
                response = (HttpWebResponse)wex.Response;
                isError = false;
            }

            return response;
        }

        public static HttpWebResponse SendRequestGMP(String action, XmlDocument soapEnvelopeXmlBase, out Boolean isError)
        {
            isError = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrlGmp);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers.Add("SOAPAction", action);
            request.Headers.Add("Type", "Request-Response");
            //request.Headers.Add("Receives Attachments", "false");
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Method = "POST";
            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.Write(soapEnvelopeXmlBase.OuterXml);

            writer.Close();
            request.GetRequestStream().Flush();

            string errDescription = string.Empty;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                errDescription = wex.Message;
                response = (HttpWebResponse)wex.Response;
                isError = false;
            }

            return response;
        }

        public static XmlDocument GetResponseXML(HttpWebResponse response)
        {
            string responseString = "";
            StreamWriter sw = new StreamWriter(@"C:\FileStore\Attach\Response.txt");
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string line;
                bool isWrite = false;
                while ((line = streamReader.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                    if (line.Contains("/soap:Envelope") || line.Contains("/S:Envelope") || line.Contains("/soapenv:Envelope"))
                    {
                        responseString += line;
                        isWrite = false;
                    }
                    else if (line.Contains("soap:Envelope") || line.Contains("S:Envelope") || line.Contains("soapenv:Envelope"))
                    {
                        responseString += line;
                        isWrite = true;
                    }
                    else if (isWrite)
                    {
                        responseString += line;
                    }
                }
            }

            XmlDocument sendRequestResponseXml = new XmlDocument();
            sendRequestResponseXml.LoadXml(responseString);
            sw.Close();
            return sendRequestResponseXml;
        }

        public static String GetResponseError(HttpWebResponse response)
        {
            string responseString = "";
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    responseString += line;
                }
            }

            return responseString;
        }
    }
}
