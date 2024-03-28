namespace Sobits.RosReg.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.ExtractTypes;
    using Sobits.RosReg.Helpers;

    public class ExtractActionsController : BaseController
    {
        public ActionResult DownloadExtract(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");
            if (id == 0)
            {
                id = ExtractActionsHelper.GetExtractIdForClw(baseParams);
            }
            var fileManager = this.Container.Resolve<IFileManager>();
            var extractDomain = this.Container.ResolveDomain<Extract>();

            try
            {
                var extract = extractDomain.Get(id);

                //Создаем файл выписки, если его нет и имеется xslt
                // ReSharper disable once InvertIf
                if (extract.File == 0)
                {
                    var extractType = this.Container.ResolveAll<IExtractType>().FirstOrDefault(x => x.Code == extract.Type);
                    // ReSharper disable once InvertIf
                    if (!string.IsNullOrEmpty(extractType.Xslt))
                    {
                        var html = XslTransformer.CreateHtmlExtractFromXml(extract.Xml, extractType.Xslt);
                        var file = fileManager.SaveFile(Hash.GetHashString(html), "html", Encoding.UTF8.GetBytes(html));
                        extract.File = file.Id;
                        extractDomain.Save(extract);
                    }
                }

                return extract.File != 0?new JsonNetResult(new BaseDataResult(extract.File)):new JsonNetResult(new BaseDataResult(false, "Не удалось найти файл выписки или создать его"));
            }
            catch (Exception e)
            {
                return new JsonNetResult(
                    new BaseDataResult { Success = false, Message = "Неизвестная ошибка", Data = new { Exception = e.InnerException } });
            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(extractDomain);
            }
        }


        public ActionResult DownloadPAExtract(BaseParams baseParams)
        {
            //var id = baseParams.Params.GetAs<long>("Id");
            //if (id == 0)
            //{
            //    id = ExtractActionsHelper.GetExtractIdForDebtor(baseParams);
            //}
            var id = ExtractActionsHelper.GetExtractIdForDebtor(baseParams);
            var fileManager = this.Container.Resolve<IFileManager>();
            var extractDomain = this.Container.ResolveDomain<Extract>();

            try
            {
                var extract = extractDomain.Get(id);

                //Создаем файл выписки, если его нет и имеется xslt
                // ReSharper disable once InvertIf
                if (extract.File == 0)
                {
                    var extractType = this.Container.ResolveAll<IExtractType>().FirstOrDefault(x => x.Code == extract.Type);
                    // ReSharper disable once InvertIf
                    if (!string.IsNullOrEmpty(extractType.Xslt))
                    {
                        var html = XslTransformer.CreateHtmlExtractFromXml(extract.Xml, extractType.Xslt);
                        var file = fileManager.SaveFile(Hash.GetHashString(html), "html", Encoding.UTF8.GetBytes(html));
                        extract.File = file.Id;
                        extractDomain.Save(extract);
                    }
                }

                return extract.File != 0 ? new JsonNetResult(new BaseDataResult(extract.File)) : new JsonNetResult(new BaseDataResult(false, "Не удалось найти файл выписки или создать его"));
            }
            catch (Exception e)
            {
                return new JsonNetResult(
                    new BaseDataResult { Success = false, Message = "По данному лицевому счету выписка не найдена", Data = new { Exception = e.InnerException } });
            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(extractDomain);
            }
        }
        // ReSharper disable once UnusedMember.Local
        private FileResult PrintExtractForXml(string xml, string filename, string xslt)
        {
            var str = XslTransformer.CreateHtmlExtractFromXml(xml, xslt);

            var xmlByteArray = Encoding.UTF8.GetBytes(str);

            Stream ms = new MemoryStream(xmlByteArray);
            this.Response.Headers.Add("Content-Disposition", $@"attachment; filename={filename}.html");
            var result = new FileStreamResult(ms, "text/html");

            return result;
        }
    }

    #region XSLTransform
    internal class EmbeddedXsltResolver : XmlResolver
    {
        public override ICredentials Credentials
        {
            set => throw new NotImplementedException();
        }

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            // from RFC 2396, TDB (thing-described-by) URN
            return new Uri("tdb:" + relativeUri);
        }
        
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            foreach (var resourceName in executingAssembly.GetManifestResourceNames())
            {
                if (!resourceName.EndsWith("." + absoluteUri.LocalPath.Replace('/', '.')))
                {
                    continue;
                }

                var xslt = executingAssembly.GetManifestResourceStream(resourceName);
                return XmlReader.Create(new StreamReader(xslt), null, absoluteUri.LocalPath);
            }

            throw new FileNotFoundException("Did not find Xslt as Embedded Resource.", absoluteUri.LocalPath);
        }
    }

    internal class XslTransformer
    {
        public static string CreateHtmlExtractFromXml(string xml, string xsltEmbedded)
        {
            // Load XSLT from resource
            var xsltStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(xsltEmbedded);

            var xsltDoc = new XmlDocument();
            xsltDoc.Load(xsltStream);

            XslCompiledTransform transform = new XslCompiledTransform();

            // Tell Transform to use EmbeddedResourceXsltResolver
            // to resolve xsi:includes.
            var xslResolver = new EmbeddedXsltResolver();
            transform.Load(xsltDoc, new XsltSettings(true, true), xslResolver);

            var xmlByteArray = Encoding.UTF8.GetBytes(xml);
            Stream ms = new MemoryStream(xmlByteArray);

            var xpathdocument = new XPathDocument(ms);

            Stream output = new MemoryStream();
            TextWriter tw = new StreamWriter(output);
            var writer = new XmlTextWriter(tw) { Formatting = Formatting.Indented };

            XmlReader xmlReader = new XmlTextReader(new StringReader(xpathdocument.CreateNavigator().OuterXml));
            // xslResolver для функции xslt document()
            transform.Transform(xmlReader, null, writer, xslResolver);
            var reader = new StreamReader(output);
            output.Seek(0, SeekOrigin.Begin);
            var htmltext = reader.ReadToEnd();

            tw.Close();
            writer.Close();
            return htmltext;
        }
    }
    #endregion

    #region Hash
    internal class Hash
    {
        public static string GetHashString(string inputString)
        {
            var sb = new StringBuilder();
            foreach (var b in Hash.GetHash(inputString))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create(); //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
    #endregion
}