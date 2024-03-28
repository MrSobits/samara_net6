namespace Sobits.RosReg.ExtractTypes
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;

    public class EgrpReestrExtractBig : ExtractTypeBase
    {
        /// <inheritdoc />
        public override ExtractType Code => ExtractType.EgrnExtractBig;

        /// <inheritdoc />
        public override ExtractCategory Category => ExtractCategory.EgrnRoom;

        /// <inheritdoc />
        public override string Name => "ЕГРН - Помещения - v07";

        /// <inheritdoc />
        public override string Pattern =>
            "<?xml-stylesheet type=\"text/xsl\" href=\"https://portal.rosreestr.ru/xsl/EGRP/Reestr_Extract_Big/ROOM/07/Common.xsl\"?>";

        /// <inheritdoc />
        public override string Xslt => "Sobits.RosReg.resources.xsl_egrp_reestr_extract_big_room_v07.Common.xsl";

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

                var nsmgr = new XmlNamespaceManager(xDoc.NameTable);
                nsmgr.AddNamespace("kpoks", "urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1");
                nsmgr.AddNamespace("adrs", "urn://x-artefacts-rosreestr-ru/commons/complex-types/address-output/4.0.1");


                var xRoot = xDoc.DocumentElement;
                var ro = xRoot.SelectSingleNode("kpoks:Realty/*", nsmgr);
                var extr = xRoot.SelectSingleNode("kpoks:ReestrExtract", nsmgr);
                newExtractEgrn.CadastralNumber = ro.Attributes["CadastralNumber"].Value;
                newExtractEgrn.Area = ro.SelectSingleNode("kpoks:Area", nsmgr)?.InnerText.ToDecimal() ?? 0;
                newExtractEgrn.Type = ParseType(ro.SelectSingleNode("kpoks:ObjectType", nsmgr)?.InnerText);
                newExtractEgrn.Purpose = ParsePurpose(ro.SelectSingleNode("kpoks:Assignation/*", nsmgr)?.InnerText);
                List<string> addrStrings = new List<string>();
                var District =
                    ro.SelectSingleNode("kpoks:Address/adrs:District/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:District/@Name", nsmgr)?.Value;
                if (District != ". ") addrStrings.Add(District);
                var City =
                    ro.SelectSingleNode("kpoks:Address/adrs:City/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:City/@Name", nsmgr)?.Value;
                if (City != ". ") addrStrings.Add(City);
                var UrbanDistrict =
                    ro.SelectSingleNode("kpoks:Address/adrs:UrbanDistrict/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:UrbanDistrict/@Name", nsmgr)?.Value;
                if (UrbanDistrict != ". ") addrStrings.Add(UrbanDistrict);
                var SovietVillage =
                    ro.SelectSingleNode("kpoks:Address/adrs:SovietVillage/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:SovietVillage/@Name", nsmgr)?.Value;
                if (SovietVillage != ". ") addrStrings.Add(SovietVillage);
                var Locality =
                    ro.SelectSingleNode("kpoks:Address/adrs:Locality/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:Locality/@Name", nsmgr)?.Value;
                if (Locality != ". ") addrStrings.Add(Locality);
                var Street =
                    ro.SelectSingleNode("kpoks:Address/adrs:Street/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:Street/@Name", nsmgr)?.Value;
                if (Street != ". ") addrStrings.Add(Street);
                var Level1 =
                    ro.SelectSingleNode("kpoks:Address/adrs:Level1/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:Level1/@Value", nsmgr)?.Value;
                if (Level1 != ". ") addrStrings.Add(Level1);
                var Level2 =
                    ro.SelectSingleNode("kpoks:Address/adrs:Level2/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:Level2/@Value", nsmgr)?.Value;
                if (Level2 != ". ") addrStrings.Add(Level2);
                var Level3 =
                    ro.SelectSingleNode("kpoks:Address/adrs:Level3/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:Level3/@Value", nsmgr)?.Value;
                if (Level3 != ". ") addrStrings.Add(Level3);
                var Apartment =
                    ro.SelectSingleNode("kpoks:Address/adrs:Apartment/@Type", nsmgr)?.Value
                    + ". "
                    + ro.SelectSingleNode("kpoks:Address/adrs:Apartment/@Value", nsmgr)?.Value;
                if (Apartment != ". ") addrStrings.Add(Apartment);
                var address = "";
                foreach (var str in addrStrings)
                {
                    address += str + ", ";
                }
                address = address.Substring(0, address.Length - 2);
                if (newExtractEgrn.Purpose == "Нежилое помещение")
                {
                    address += ", кв. Неж.пом.";
                }
                newExtractEgrn.Address =address.Trim();
                

                var extractDate = new DateTime();
                DateTime.TryParse(extr.SelectSingleNode("kpoks:DeclarAttribute/@ExtractDate", nsmgr)?.Value, out extractDate);
                newExtractEgrn.ExtractDate = extractDate;

                newExtractEgrn.ExtractNumber = extr.SelectSingleNode("kpoks:DeclarAttribute/@ExtractNumber", nsmgr)?.Value;

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

                var rightNodes = extr.SelectNodes("kpoks:ExtractObjectRight/kpoks:ExtractObject/kpoks:ObjectRight/kpoks:Right", nsmgr);
                for (var indexRight = 0; indexRight < rightNodes.Count; indexRight++)
                {
                    var num = rightNodes[indexRight].SelectSingleNode("kpoks:Registration/kpoks:Share/@Numerator", nsmgr)?.InnerText ;
                    var denum = rightNodes[indexRight].SelectSingleNode("kpoks:Registration/kpoks:Share/@Denominator", nsmgr)?.InnerText;
                    var rtShare = rightNodes[indexRight].SelectSingleNode("kpoks:Registration/kpoks:ShareText", nsmgr)?.InnerText;
                    
                    var ShareParced = num != null
                        ? num + "/" + denum
                        : rtShare ?? "1";

                    //Запись о праве собственности
                    var newExtractRight = new ExtractEgrnRight
                    {
                        
                        Type = rightNodes[indexRight].SelectSingleNode("kpoks:Registration/kpoks:Name", nsmgr)?.InnerText,
                        Number = rightNodes[indexRight].SelectSingleNode("kpoks:Registration/kpoks:RegNumber", nsmgr)?.InnerText,
                        Share = ShareParced,
                        EgrnId = newExtractEgrn
                    };

                    extractEgrnRightService.Save(newExtractRight);

                    var ownerNodes = rightNodes[indexRight].SelectNodes("kpoks:Owner/kpoks:Person", nsmgr);
                    for (var indexOwner = 0; indexOwner < ownerNodes.Count; indexOwner++)
                    {
                        // Собственники - физ.лица
                        var newExtractRightInd = new ExtractEgrnRightInd
                        {
                            Surname = ownerNodes[indexOwner].SelectSingleNode("kpoks:FIO/kpoks:Surname", nsmgr)?.InnerText,
                            FirstName = ownerNodes[indexOwner].SelectSingleNode("kpoks:FIO/kpoks:First", nsmgr)?.InnerText,
                            Patronymic = ownerNodes[indexOwner].SelectSingleNode("kpoks:FIO/kpoks:Patronymic", nsmgr)?.InnerText
                        };

                        //var dt = new DateTime();
                        //DateTime.TryParse(ownerNodes[indexOwner].SelectSingleNode("DateBirth")?.InnerText, out dt);
                        //newExtractRightInd.BirthDate = dt;

                        //newExtractRightInd.BirthPlace = ownerNodes[indexOwner].SelectSingleNode("Place_Birth")?.InnerText;
                        //newExtractRightInd.Snils = ownerNodes[indexOwner].SelectSingleNode("SNILS")?.InnerText;
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

        private string ParseType(string code)
        {
            switch (code)
            {
                case "002001002000": return "Здание";
                case "002001004000": return "Сооружение";
                case "002001004001": return "Линейное сооружение";
                case "002001004002": return "Условная часть линейного сооружения";
                case "002001005000": return "Объект незавершенного строительства";
                case "002001003000": return "Помещение";
                case "002002002000": return "Помещение";
                case "002002001000": return "Здание";
                case "002002005000": return "Объект незавершенного строительства";
                case "002002004000": return "Сооружение";
                case "002002000000": return "";
                default: return "";
            }
        }

        private string ParsePurpose(string code)
        {
            switch (code)
            {
                case "206001000000": return "Нежилое помещение";
                case "206002000000": return "Жилое помещение";
                default: return "";
            }
        }
    }
}