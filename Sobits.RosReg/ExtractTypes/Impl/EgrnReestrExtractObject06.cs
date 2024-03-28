namespace Sobits.RosReg.ExtractTypes
{
    using System;
    using System.Xml;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;

    public class EgrpReestrExtractObject06 : ExtractTypeBase
    {
        /// <inheritdoc />
        public override ExtractType Code => ExtractType.EgrnExtractObject06;

        /// <inheritdoc />
        public override ExtractCategory Category => ExtractCategory.EgrnRoom;

        /// <inheritdoc />
        public override string Name => "ЕГРН - Помещения - v06";

        /// <inheritdoc />
        public override string Pattern =>
            "<?xml-stylesheet type=\"text/xsl\" href=\"https://portal.rosreestr.ru/xsl/EGRP/Reestr_Extract_Object/06/Common.xsl\"?>";

        /// <inheritdoc />
        public override string Xslt => "Sobits.RosReg.resources.xsl_egrp_reestr_extract_object_v06.Common.xsl";

        /// <inheritdoc />
        public override void Parse(Extract extract)
        {
            var extractService = this.Container.ResolveDomain<Extract>();
            var extractEgrnService = this.Container.ResolveDomain<ExtractEgrn>();
            var extractEgrnRightService = this.Container.ResolveDomain<ExtractEgrnRight>();
            var extractEgrnRightIndService = this.Container.ResolveDomain<ExtractEgrnRightInd>();
            try
            {
                var newExtractEgrn = new ExtractEgrn();

                var xDoc = new XmlDocument();
                xDoc.LoadXml(extract.Xml);
                var xRoot = xDoc.DocumentElement;

                newExtractEgrn.CadastralNumber = xRoot.SelectSingleNode("//*/CadastralNumber")?.InnerText;
                newExtractEgrn.Address = xRoot.SelectSingleNode("//*/Address/Content")?.InnerText;
                newExtractEgrn.Area = xRoot.SelectSingleNode("//*/Area/Area")?.InnerText.ToDecimal() ?? 0;
                newExtractEgrn.Type = xRoot.SelectSingleNode("//*/ObjectDesc/Name")?.InnerText;
                newExtractEgrn.Purpose = xRoot.SelectSingleNode("//*/ObjectDesc/Assignation_Code_Text")?.InnerText;

                var extractDate = new DateTime();
                DateTime.TryParse(xRoot.SelectSingleNode("//*/FootContent/ExtractDate")?.InnerText, out extractDate);
                newExtractEgrn.ExtractDate = extractDate;

                newExtractEgrn.ExtractNumber = xRoot.SelectSingleNode("//DeclarAttribute/@ExtractNumber")?.InnerText;

                newExtractEgrn.ExtractId = extract;

                newExtractEgrn.IsMerged = Bars.Gkh.Enums.YesNoNotSet.No;

                try
                {
                    extractEgrnService.Save(newExtractEgrn);
                }
                catch (Exception e)
                {
                    extract.Comment = e.Message + " " + e.StackTrace;
                    extract.IsParsed = true;
                    extractService.Update(extract);
                }

                var rightNodes = xRoot.SelectNodes("//*/Right");
                for (var indexRight = 0; indexRight < rightNodes.Count; indexRight++)
                {
                    //Запись о праве собственности
                    var newExtractRight = new ExtractEgrnRight
                    {
                        Type = rightNodes[indexRight].SelectSingleNode("Registration/Name")?.InnerText,
                        Number = rightNodes[indexRight].SelectSingleNode("Registration/RegNumber")?.InnerText,
                        Share = rightNodes[indexRight].SelectSingleNode("Registration/ShareText")?.InnerText,
                        EgrnId = newExtractEgrn
                    };

                    extractEgrnRightService.Save(newExtractRight);

                    var ownerNodes = rightNodes[indexRight].SelectNodes("Owner/Person");
                    for (var indexOwner = 0; indexOwner < ownerNodes.Count; indexOwner++)
                    {
                        // Собственники - физ.лица
                        var newExtractRightInd = new ExtractEgrnRightInd
                        {
                            Surname = ownerNodes[indexOwner].SelectSingleNode("FIO/Surname")?.InnerText,
                            FirstName = ownerNodes[indexOwner].SelectSingleNode("FIO/First")?.InnerText,
                            Patronymic = ownerNodes[indexOwner].SelectSingleNode("FIO/Patronymic")?.InnerText
                        };

                        var dt = new DateTime();
                        DateTime.TryParse(ownerNodes[indexOwner].SelectSingleNode("DateBirth")?.InnerText, out dt);
                        newExtractRightInd.BirthDate = dt;

                        newExtractRightInd.BirthPlace = ownerNodes[indexOwner].SelectSingleNode("Place_Birth")?.InnerText;
                        newExtractRightInd.Snils = ownerNodes[indexOwner].SelectSingleNode("SNILS")?.InnerText;
                        newExtractRightInd.RightId = newExtractRight;
                        extractEgrnRightIndService.Save(newExtractRightInd);
                    }
                }

                extract.IsParsed = true;
                extractService.Update(extract);
            }
            finally
            {
                this.Container.Release(extractService);
                this.Container.Release(extractEgrnService);
                this.Container.Release(extractEgrnRightService);
                this.Container.Release(extractEgrnRightIndService);
            }
        }
    }
}