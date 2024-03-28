namespace Bars.Gkh.RegOperator.Export.ExportToEbir
{
    using System.IO;
    using System.Xml.Serialization;

    public class XmlExport: BaseExport, IEbirExport
    {
        public string Format { get { return "xml"; } }

        protected override B4.Modules.FileStorage.FileInfo SaveFile()
        {
            var serializer = new XmlSerializer(typeof(EbirResponse));

            var response = new EbirResponse { Records = this.records.ToArray() };

            var stream = new MemoryStream();

            serializer.Serialize(stream, response);

            var fileInfo = FileManager.SaveFile(stream, "ebirExport." + this.Format);

            return fileInfo;
        }
    }
}