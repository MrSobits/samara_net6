namespace Sobits.RosReg.ExtractTypes
{
    using System;

    using Bars.B4.DataAccess;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;

    /// <summary>
    /// Переопределено в модуле регоператор
    /// </summary>
    public class EgrnReestrExtractList07 : ExtractTypeBase
    {
        /// <inheritdoc />
        public override ExtractType Code => ExtractType.EgrnExtractList07;

        /// <inheritdoc />
        public override ExtractCategory Category => ExtractCategory.EgrnRoom;

        /// <inheritdoc />
        public override string Name => "ЕГРН - Помещения - Reestr_Extract_List - v07";

        /// <inheritdoc />
        public override string Pattern =>
            "<?xml-stylesheet type=\"text/xsl\" href=\"https://portal.rosreestr.ru/xsl/EGRP/Reestr_Extract_List/07/Common.xsl\"?>";

        /// <inheritdoc />
        public override string Xslt => null; //Xslt-файлов на данный момент нет и не предвидится для данного типа выписок

        /// <inheritdoc />
        public override void Parse(Extract extract)
        {
            var extractService = this.Container.ResolveDomain<Extract>();
            var extractEgrnService = this.Container.ResolveDomain<ExtractEgrn>();
            var extractEgrnRightService = this.Container.ResolveDomain<ExtractEgrnRight>();
            var extractEgrnRightIndService = this.Container.ResolveDomain<ExtractEgrnRightInd>();
            var residentDomain = this.Container.ResolveDomain<ExtractEgrnRightLegalResident>();
            var notResidentDomain = this.Container.ResolveDomain<ExtractEgrnRightLegalNotResident>();
            var fileInfoService = this.Container.ResolveDomain<Bars.B4.Modules.FileStorage.FileInfo>();

            try
            {
                var newExtractEgrn = new ExtractEgrn();        
                //var xRoot = XElement.Parse(extract.Xml);
                //var extract07 = Deserialize<Sobits.RosReg.Entities.ExtractList07.Extract>(xRoot);
                //if (extract.File > 0)
                //{
                //    var numls = fileInfoService.Get(extract.File).Name;

                //}
                //List< Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightObjectDesc> roomlist = 


                //Создаем запись о выписке
                //newExtractEgrn.CadastralNumber = xRoot.SelectSingleNode("//*/room_record/object/common_data/cad_number")?.InnerText;
                //newExtractEgrn.Address = xRoot.SelectSingleNode("//*/address_room/address/address/readable_address")?.InnerText;
                //newExtractEgrn.Type = xRoot.SelectSingleNode("//*/room_record/params/name")?.InnerText;
                //newExtractEgrn.Purpose = xRoot.SelectSingleNode("//*/room_record/params/purpose/value")?.InnerText;
                //newExtractEgrn.Area = xRoot.SelectSingleNode("//*/room_record/params/area")?.InnerText.ToDecimal() ?? 0;

                //var extractDate = new DateTime();
                ////   DateTime.TryParse(xRoot.SelectSingleNode("//*/date_formation")?.InnerText, out extractDate);
                //DateTime.TryParse(xRoot.SelectSingleNode("//*/group_top_requisites/date_formation")?.InnerText, out extractDate);
                //DateTime.TryParse(xRoot.SelectSingleNode("//*/details_request/date_receipt_request_reg_authority_rights")?.InnerText, out extractDate);
                //newExtractEgrn.ExtractDate = extractDate;

                //newExtractEgrn.ExtractNumber = xRoot.SelectSingleNode("//*/group_top_requisites/registration_number")?.InnerText;

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

                extract.IsParsed = true;
                extractService.Update(extract);
            }
            catch (Exception e)
            {
                string str = e.Message;
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