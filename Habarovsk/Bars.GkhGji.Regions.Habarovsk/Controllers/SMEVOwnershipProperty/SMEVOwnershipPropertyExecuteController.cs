namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVOwnershipProperty;
    using Bars.GkhGji.Regions.Habarovsk.Tasks;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.GetSMEVAnswers;
    using Entities;
    using Enums;
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Security.Policy;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System.Xml.Xsl;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class SMEVOwnershipPropertyExecuteController : BaseController
    {
        public IDomainService<SMEVOwnershipProperty> SMEVOwnershipPropertyDomain { get; set; }

        public IDomainService<SMEVOwnershipPropertyFile> SMEVOwnershipPropertyFileDomain { get; set; }

        private static IFileManager _fileManager;

        private readonly ITaskManager _taskManager;

        private IDomainService<FileInfo> _fileDomain;

        public SMEVOwnershipPropertyExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVOwnershipPropertyDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new OwnershipPropertyTaskProvider(Container), baseParams);
                return GetResponse(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных не удалось: " + e.Message);
            }
        }

        public ActionResult GetResponse(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVOwnershipPropertyDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return JsFailure("Ответ уже получен");

            try
            {
                _taskManager.CreateTasks(new GetRPGUAnswersTaskProvider(Container), baseParams);
                return JsSuccess("Задача поставлена в очередь задач");
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }

        public ActionResult GetAnswerFile(BaseParams baseParams, Int64 taskId)
        {
            try
            {
                var a = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                var smevRequestData = SMEVOwnershipPropertyDomain.Get(taskId);

                if (smevRequestData?.AnswerFile != null)
                {
                    var html = CreateHtmlExtractFromXml(smevRequestData.AnswerFile, "Bars.GkhGji.Regions.Voronezh.Resources.test1.xsl");
                    var file = _fileManager.SaveFile(Hash.GetHashString(html), "html", Encoding.UTF8.GetBytes(html));
                    return new JsonNetResult(new BaseDataResult(file));
                }

                return JsFailure("Файл не найден");
            }
            catch(Exception e)
            {
                return JsFailure("Error "+e);
            }
        }

        public static string CreateHtmlExtractFromXml(FileInfo xml, string xsltEmbedded)
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

            XmlDocument xmlDocument = new XmlDocument();
            Stream docStream = _fileManager.GetFile(xml);
            XmlReader xreader = XmlReader.Create(docStream);
            xmlDocument.Load(xreader);

            MemoryStream memStream = new MemoryStream();
            xmlDocument.Save(memStream);
            memStream.Position = 0;
            XPathDocument xpathdocument = new XPathDocument(memStream);

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
    }
}
