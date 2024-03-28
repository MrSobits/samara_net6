namespace Sobits.RosReg.ExtractTypes
{
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;

    using Bars.B4.Application;

    using Castle.Windsor;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;

    public abstract class ExtractTypeBase : IExtractType
    {
        protected IWindsorContainer Container => ApplicationContext.Current.Container;

        /// <inheritdoc />
        public abstract ExtractType Code { get; }

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract ExtractCategory Category { get; }

        /// <inheritdoc />
        public abstract string Pattern { get; }

        /// <inheritdoc />
        public abstract void Parse(Extract extract);

        /// <inheritdoc />
        public abstract string Xslt { get; }

        /// <inheritdoc />
        public string Print(Extract extract)
        {
            // Load XSLT from resource
            var xsltStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.Xslt);

            var xsltDoc = new XmlDocument();
            xsltDoc.Load(xsltStream);

            var transform = new XslCompiledTransform();

            // Tell Transform to use EmbeddedResourceXsltResolver
            // to resolve xsi:includes.
            transform.Load(xsltDoc, null, new EmbeddedXsltResolver());

            var xmlByteArray = Encoding.UTF8.GetBytes(extract.Xml);
            Stream ms = new MemoryStream(xmlByteArray);

            var xpathdocument = new XPathDocument(ms);

            Stream output = new MemoryStream();
            TextWriter tw = new StreamWriter(output);
            var writer = new XmlTextWriter(tw) { Formatting = Formatting.Indented };

            transform.Transform(xpathdocument, null, writer, null);
            var reader = new StreamReader(output);
            output.Seek(0, SeekOrigin.Begin);
            var htmltext = reader.ReadToEnd();

            tw.Close();
            writer.Close();
            return htmltext;
        }

        #region XSLTransform
        private class EmbeddedXsltResolver : XmlResolver
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
                    if (!resourceName.EndsWith("." + absoluteUri.LocalPath))
                    {
                        continue;
                    }

                    var xslt = executingAssembly.GetManifestResourceStream(resourceName);
                    return XmlReader.Create(new StreamReader(xslt), null, absoluteUri.LocalPath);
                }

                throw new FileNotFoundException("Did not find Xslt as Embedded Resource.", absoluteUri.LocalPath);
            }
        }
        #endregion
    }
}