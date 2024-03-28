namespace Sobits.RosReg.ExtractTypes
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Xml;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using NHibernate.Util;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;

    public class EgrpReestrExtractAboutPropertyRoom : ExtractTypeBase
    {
        /// <inheritdoc />
        public override ExtractType Code => ExtractType.EgrnExtractAboutPropertyRoom;

        /// <inheritdoc />
        public override ExtractCategory Category => ExtractCategory.EgrnRoom;

        /// <inheritdoc />
        public override string Name => "ЕГРН - Помещения (eapr_v01)";

        /// <inheritdoc />
        public override string Pattern =>
            "<extract_about_property_room";

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
            try
            {
                var newExtractEgrn = new ExtractEgrn();

                var xDoc = new XmlDocument();
                xDoc.LoadXml(extract.Xml);
                var xRoot = xDoc.DocumentElement;

                //Создаем запись о выписке
                newExtractEgrn.CadastralNumber = xRoot.SelectSingleNode("//*/room_record/object/common_data/cad_number")?.InnerText;
                newExtractEgrn.Address = xRoot.SelectSingleNode("//*/address_room/address/address/readable_address")?.InnerText;
                newExtractEgrn.Type = xRoot.SelectSingleNode("//*/room_record/params/name")?.InnerText;
                newExtractEgrn.Purpose = xRoot.SelectSingleNode("//*/room_record/params/purpose/value")?.InnerText;
                newExtractEgrn.Area = xRoot.SelectSingleNode("//*/room_record/params/area")?.InnerText.ToDecimal() ?? 0;

                var extractDate = new DateTime();
                //   DateTime.TryParse(xRoot.SelectSingleNode("//*/date_formation")?.InnerText, out extractDate);
                DateTime.TryParse(xRoot.SelectSingleNode("//*/group_top_requisites/date_formation")?.InnerText, out extractDate);
                DateTime.TryParse(xRoot.SelectSingleNode("//*/details_request/date_receipt_request_reg_authority_rights")?.InnerText, out extractDate);
                newExtractEgrn.ExtractDate = extractDate;

                newExtractEgrn.ExtractNumber = xRoot.SelectSingleNode("//*/group_top_requisites/registration_number")?.InnerText;

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

                var rightNodes = xRoot.SelectNodes("//*/right_record");
                for (var indexRight = 0; indexRight < rightNodes.Count; indexRight++)
                {
                    //Запись о праве собственности
                    
                    var newExtractRight = new ExtractEgrnRight
                    {
                        Type = rightNodes[indexRight].SelectSingleNode("right_data/right_type/value")?.InnerText,
                        Number = rightNodes[indexRight].SelectSingleNode("right_data/right_number")?.InnerText,
                        EgrnId = newExtractEgrn
                    };
                   
                    //пилим сюда логику обработки процентов

                    var simple = rightNodes[indexRight].SelectSingleNode("right_data/shares/share/numerator") != null;
                    
                    ////
                    ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!/////////////////////
                    ///Если доля не найдена - нужно оставить null. Тогда доля считается 1
                    ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!/////////////////////
                    ///
                    
                    if (simple)
                    {
                        newExtractRight.Share = rightNodes[indexRight].SelectSingleNode("right_data/shares/share/numerator") != null ?
                            rightNodes[indexRight].SelectSingleNode("right_data/shares/share/numerator")?.InnerText + "/" +
                            rightNodes[indexRight].SelectSingleNode("right_data/shares/share/denominator")?.InnerText : "1";
                    }
                    else
                    {
                        var share = rightNodes[indexRight].SelectSingleNode("right_data/share_description")?.InnerText?.Replace(',', '.');
                        if (share != null)
                        {
                            var matches = Regex.Matches(share, @"\d+\/\d*");

                            if (matches.Count > 0)
                                newExtractRight.Share = matches[0].Value;
                            else
                            {
                                var matchesPescent = Regex.Matches(share, @"\d+\.*\d*\ ?\%");
                                if (matchesPescent.Count > 0)
                                {
                                    var digits = matchesPescent[0].Value.Replace("%", "").Split(".");
                                    IFormatProvider formatProvider = new CultureInfo("en-US", false);
                                    var num = double.Parse(matchesPescent[0].Value.Replace("%", ""), formatProvider);
                                    var denum = 100.0;
                                    if (digits.Length > 1)
                                    {
                                        num *= Math.Pow(10, digits[1].Length);
                                        denum *= Math.Pow(10, digits[1].Length);

                                    }

                                    var gCd = this.GCD((int) num, (int) denum);
                                    var resNum = num / gCd;
                                    var resDenum = denum / gCd;
                                    newExtractRight.Share = $"{resNum}/{resDenum}";
                                }
                            }
                        }
                    }


                    extractEgrnRightService.Save(newExtractRight);

                    var ownerNodes = rightNodes[indexRight].SelectNodes("right_holders/right_holder/individual");
                    for (var indexOwner = 0; indexOwner < ownerNodes.Count; indexOwner++)
                    {
                        // Собственники - физ.лица
                        var newExtractRightInd = new ExtractEgrnRightInd
                        {
                            Surname = ownerNodes[indexOwner].SelectSingleNode("surname")?.InnerText,
                            FirstName = ownerNodes[indexOwner].SelectSingleNode("name")?.InnerText,
                            Patronymic = ownerNodes[indexOwner].SelectSingleNode("patronymic")?.InnerText
                        };

                        var dt = new DateTime();
                        DateTime.TryParse(ownerNodes[indexOwner].SelectSingleNode("birth_date")?.InnerText, out dt);
                        newExtractRightInd.BirthDate = dt;

                        newExtractRightInd.BirthPlace = ownerNodes[indexOwner].SelectSingleNode("birth_place")?.InnerText;
                        newExtractRightInd.Snils = ownerNodes[indexOwner].SelectSingleNode("snils")?.InnerText;
                        newExtractRightInd.DocIndCode = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_code/code")?.InnerText;
                        newExtractRightInd.DocIndName = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_code/value")?.InnerText;
                        newExtractRightInd.DocIndSerial = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_series")?.InnerText;
                        newExtractRightInd.DocIndNumber = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_number")?.InnerText;
                        DateTime.TryParse(ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_date")?.InnerText, out dt);
                        newExtractRightInd.DocIndDate = dt;
                        newExtractRightInd.DocIndIssue = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_issuer")?.InnerText;
                        newExtractRightInd.RightId = newExtractRight;
                        
                        extractEgrnRightIndService.Save(newExtractRightInd);
                    }

                    var legalNodes = rightNodes[indexRight].SelectNodes("right_holders/right_holder/legal_entity");

                    for (var legalOwnerIndex = 0; legalOwnerIndex < legalNodes.Count; legalOwnerIndex++)
                    {

                        var residentNode = legalNodes[legalOwnerIndex].SelectSingleNode("entity/resident");
                        if (residentNode == null) residentNode = legalNodes[legalOwnerIndex].SelectSingleNode("resident");
                        var isresident = residentNode != null;
                        if(isresident)
                        {

                            var resident = new ExtractEgrnRightLegalResident();

                            resident.OwnerType = LegalOwnerType.Resident;
                            resident.IncorporationForm = legalNodes[legalOwnerIndex].SelectSingleNode("entity/resident/incorporation_form")?.InnerText;
                            resident.Name = legalNodes[legalOwnerIndex].SelectSingleNode("entity/resident/name")?.InnerText.Substring(0, 255);
                            resident.Inn = long.Parse(legalNodes[legalOwnerIndex].SelectSingleNode("entity/resident/inn")?.InnerText);
                            resident.Ogrn = long.Parse(legalNodes[legalOwnerIndex].SelectSingleNode("entity/resident/ogrn")?.InnerText);
                            resident.MailingAddress = legalNodes[legalOwnerIndex].SelectSingleNode("entity/resident/mailing_addess")?.InnerText;
                            resident.Email = legalNodes[legalOwnerIndex].SelectSingleNode("entity/resident/email")?.InnerText;
                            resident.RightId = newExtractRight;
                            
                            residentDomain.Save(resident);
                        }
                        else
                        {
                            var notResident = new ExtractEgrnRightLegalNotResident();

                            notResident.OwnerType = LegalOwnerType.NotResident;
                            notResident.IncorporationForm =
                                legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/incorporation_form")?.InnerText;
                            notResident.Name = legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/name")?.InnerText;
                            notResident.IncorporationCountry =
                                legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/incorporate_country")?.InnerText;
                            notResident.RegistrationNumber =
                                legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/registration_number")?.InnerText;
                            notResident.RegistrationOrgan =
                                legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/registration_organ")?.InnerText;
                            notResident.RegAddresSubject =
                                legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/reg_address_subject")?.InnerText;
                            notResident.MailingAddress = legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/mailing_addess")?.InnerText;
                            notResident.Email = legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/email")?.InnerText;
                            notResident.RightId = newExtractRight;
                            
                            var date = legalNodes[legalOwnerIndex].SelectSingleNode("entity/not_resident/date_state_reg")?.InnerText;
                            if(DateTime.TryParse(date,out DateTime datep)) notResident.DateStateReg = datep;
                            
                            notResidentDomain.Save(notResident);
                        }


                    }
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
        private int GCD(int a, int b)
        {
            while (b > 0)
            {
                int rem = a % b;
                a = b;
                b = rem;
            }
            return a;
        }
    }
}