namespace Bars.Gkh.Regions.Voronezh.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using Microsoft.AspNetCore.Mvc;
    using Dapper;
    using Modules.ClaimWork.Entities;
    using RegOperator.Entities;
    using System.Xml;
    using System.Reflection;
    using System;
    using System.Xml.Xsl;
    using System.Xml.XPath;

    using Bars.Gkh.Extensions;

    /// <summary>
    /// Контроллер для обработки запросов печати выписок из Росреестра
    /// </summary>
    public class ExtractPrinterController : BaseController
    {
        /// <summary>
        /// Печать выписки по id описания выписки
        /// </summary>
        /// <param name="id">Description id</param>
        /// <returns></returns>
        public FileResult PrintExtractForDescription(long id)
        {        
            var StatelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            var connection = StatelessSession.Connection;
            var sql = $@"SELECT xml,desc_addresscontent,extractnumber from import.rosregextractdesc WHERE ID={id}";
            dynamic res = connection.Query<dynamic>(sql).FirstOrDefault();

            return PrintExtractForXML(res.xml,res.extractnumber);
        }
        /// <summary>
        /// Печать выписки по id документа ПИР
        /// </summary>
        /// ClaimWork Document id
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult PrintExtractForDocumentClw(long id)
        {
            var docDomain = Container.ResolveDomain<DocumentClw>();
            var doc = docDomain.GetAll().Where(x=>x.Id == id).FirstOrDefault();
            var clw_id = doc.ClaimWork.Id;
            var detailDomain = Container.ResolveDomain<ClaimWorkAccountDetail>();
            var detail = detailDomain.GetAll().Where(x => x.ClaimWork.Id == clw_id).FirstOrDefault();
            var room = detail.PersonalAccount.Room.Id;

            var StatelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            var connection = StatelessSession.Connection;
            var sql = $@"SELECT xml,desc_addresscontent,extractnumber from import.rosregextractdesc WHERE ROOM_ID={room}";

            dynamic res = connection.Query<dynamic>(sql).FirstOrDefault();
            Container.Release(docDomain);
            Container.Release(detailDomain);
            return PrintExtractForXML(res.xml, res.extractnumber);
        }

        /// <summary>
        /// Печать выписки по id претензионно-исковой работы
        /// </summary>
        /// <param name="id">ClaimWork Id</param>
        /// <returns></returns>
        public FileResult PrintExtractForClaimWork(long id)
        {
            var docDomain = Container.ResolveDomain<DocumentClw>();
            var doc = docDomain.GetAll().Where(x => x.ClaimWork.Id == id).FirstOrDefault();
            var clw_id = doc.ClaimWork.Id;
            var detailDomain = Container.ResolveDomain<ClaimWorkAccountDetail>();
            var detail = detailDomain.GetAll().Where(x => x.ClaimWork.Id == clw_id).FirstOrDefault();
            var room = detail.PersonalAccount.Room.Id;

            var StatelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            var connection = StatelessSession.Connection;
            var sql = $@"SELECT xml,desc_addresscontent,extractnumber from import.rosregextractdesc WHERE ROOM_ID={room}";

            dynamic res = connection.Query<dynamic>(sql).FirstOrDefault();
            Container.Release(docDomain);
            Container.Release(detailDomain);
            return PrintExtractForXML(res.xml, res.extractnumber);
        }

        private FileResult PrintExtractForXML(string xml, string filename)
        {
            var str = XSLTransformer.CreateHtmlExtractFromXML(xml);

            byte[] xmlByteArray = Encoding.UTF8.GetBytes(str);

            Stream ms = new MemoryStream(xmlByteArray);
            var contentdisposition = new System.Net.Mime.ContentDisposition();
            contentdisposition.Inline = false;
            Response.Headers.Add("Content-Disposition", $@"attachment; filename={filename}.html");
            var result = new FileStreamResult(ms, "text/html");
            return result;
        }

        public JsonResult Test(long id)
        {
            var clwDomain = this.Container.ResolveDomain<IndividualClaimWork>();
            var entity = clwDomain.Get(id);
            var json = new JsonNetResult(entity.ToJson());
            return json;
        }
    }

    #region XSLTransform
    class EmbeddedXSLTResolver : XmlResolver
    {
        public override System.Net.ICredentials Credentials
        {
            set { throw new NotImplementedException(); }
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
                if (resourceName.EndsWith("." + absoluteUri.LocalPath))
                {
                    var xslt = executingAssembly.GetManifestResourceStream(resourceName);
                    return XmlReader.Create(new StreamReader(xslt), null, absoluteUri.LocalPath);
                }
            }

            throw new FileNotFoundException("Did not find Xslt as Embedded Resource.", absoluteUri.LocalPath);
        }
    }

    public class XSLTransformer
    {
        public static string CreateHtmlExtractFromXML(string xml)
        {
            // Load XSLT from resource
            Stream xsltStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bars.Gkh.Regions.Voronezh.resources.xsl.Common.xsl");

            XmlDocument xsltDoc = new XmlDocument();
            xsltDoc.Load(xsltStream);

            XslCompiledTransform transform = new XslCompiledTransform();
            // Tell Transform to use EmbeddedResourceXsltResolver
            // to resolve xsi:includes.
            transform.Load(xsltDoc, null, new EmbeddedXSLTResolver());

            byte[] xmlByteArray = Encoding.UTF8.GetBytes(xml);
            Stream ms = new MemoryStream(xmlByteArray);

            XPathDocument xpathdocument = new XPathDocument(ms);

            Stream output = new MemoryStream();
            TextWriter tw = new StreamWriter(output);
            XmlTextWriter writer = new XmlTextWriter(tw);
            writer.Formatting = Formatting.Indented;

            transform.Transform(xpathdocument, null, writer, null);
            StreamReader reader = new StreamReader(output);
            output.Seek(0, SeekOrigin.Begin);
            string htmltext = reader.ReadToEnd();

            tw.Close();
            writer.Close();
            return htmltext;
        }
    }
    #endregion
}
