namespace Sobits.RosReg.ExtractTypes
{
    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;

    public class EgrpReestrExtractSubject06 : ExtractTypeBase
    {
        /// <inheritdoc />
        public override ExtractType Code => ExtractType.EgrnExtractSubject06;

        /// <inheritdoc />
        public override ExtractCategory Category => ExtractCategory.EgrnJurRoom;

        /// <inheritdoc />
        public override string Name => "ЕГРН - Помещения юр.лица - v06";

        /// <inheritdoc />
        public override string Pattern =>
            "<?xml version=\"1.0\" encoding=\"utf-8\"?><?xml-stylesheet type=\"text/xsl\" href=\"https://portal.rosreestr.ru/xsl/EGRP/Reestr_Extract_Subject/06/Common.xsl\"?>";

        /// <inheritdoc />
        public override string Xslt => "Sobits.RosReg.resources.xsl_egrp_reestr_extract_subject_v06.Common.xsl";

        /// <inheritdoc />
        public override void Parse(Extract extract)
        {
            // var extractService = this.Container.ResolveDomain<Extract>();
            // var extractEgrnService = this.Container.ResolveDomain<ExtractEgrn>();
            // try
            // {
            //     var newExtractEgrn = new ExtractEgrn();
            //
            //     var xDoc = new XmlDocument();
            //     xDoc.LoadXml(extract.Xml);
            //     XmlElement xRoot = xDoc.DocumentElement;
            //
            //     newExtractEgrn.CadastralNumber = xRoot.SelectSingleNode("//*/CadastralNumber")?.InnerText;
            //     newExtractEgrn.Address = xRoot.SelectSingleNode("//*/Address/Content")?.InnerText;
            //     newExtractEgrn.Area = xRoot.SelectSingleNode("//*/Area/Area")?.InnerText.ToDecimal() ?? 0;
            //     newExtractEgrn.ExtractId = extract;
            //
            //     XmlNodeList rightNodes = xRoot.SelectNodes("//*/Right");
            //     for (var indexRight = 0; indexRight < rightNodes.Count; indexRight++)
            //     {
            //         XmlNodeList ownerNodes = rightNodes[indexRight].SelectNodes("//*/Owner");
            //         for (var indexOwner = 0; indexOwner < ownerNodes.Count; indexOwner++)
            //             ownerNodes[indexOwner].SelectSingleNode("");
            //     }
            //
            //     extractEgrnService.Save(newExtractEgrn);
            //     extract.IsParsed = true;
            //     extractService.Update(extract);
            // }
            // finally
            // {
            //     this.Container.Release(extractService);
            //     this.Container.Release(extractEgrnService);
            // }
        }
    }
}