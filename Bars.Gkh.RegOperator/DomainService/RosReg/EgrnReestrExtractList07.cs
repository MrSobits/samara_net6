namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Xml;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using System.Xml.Serialization;
    using NHibernate.Util;
    using System.Xml.Linq;
    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;
    using System.IO;
    using System.Text;
    using Sobits.RosReg.ExtractTypes;
    using Bars.Gkh.RegOperator.Entities;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.Gkh.Entities;

    public class EgrnReestrExtractPersAccList07 : ExtractTypeBase
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
            var persAccService = this.Container.ResolveDomain<BasePersonalAccount>();
            var extractEgrnRightService = this.Container.ResolveDomain<ExtractEgrnRight>();
            var extractEgrnRightIndService = this.Container.ResolveDomain<ExtractEgrnRightInd>();
            var residentDomain = this.Container.ResolveDomain<ExtractEgrnRightLegalResident>();
            var notResidentDomain = this.Container.ResolveDomain<ExtractEgrnRightLegalNotResident>();
            var fileInfoService = this.Container.ResolveDomain<Bars.B4.Modules.FileStorage.FileInfo>();

            try
            {
                var newExtractEgrn = new ExtractEgrn();
                var xRoot = XElement.Parse(extract.Xml);
                var extract07 = Deserialize<Sobits.RosReg.Entities.ExtractList07.Extract>(xRoot);
                if (extract.File > 0)
                {
                    var numls = fileInfoService.Get(extract.File).Name;
                    var persAcc = persAccService.GetAll().FirstOrDefault(x => x.PersonalAccountNum == numls);
                    if (persAcc != null)
                    {
                        newExtractEgrn.RoomId = new Room {Id = persAcc.Room.Id };
                        newExtractEgrn.Room_id = persAcc.Room.Id;
                    }
                   
                }
                if (extract07.ReestrExtract != null)
                {
                    newExtractEgrn.ExtractNumber = extract07.ReestrExtract.DeclarAttribute?.ExtractNumber;
                    var extractDate = new DateTime();
                    DateTime.TryParse(extract07.ReestrExtract.DeclarAttribute?.ExtractDate, out extractDate);
                    newExtractEgrn.ExtractDate = extractDate;
                    Dictionary<string, Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightOwner> owners = new Dictionary<string, Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightOwner>();
                    Dictionary<string, Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightRegistration> regisrets = new Dictionary<string, Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightRegistration>();
                    foreach (var obj in extract07.ReestrExtract.ExtractObjectRight.ExtractObject.ToList())
                    {
                        if (obj is Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightObjectDesc)
                        {
                            var objDesc = obj as Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightObjectDesc;
                            newExtractEgrn.CadastralNumber = objDesc.CadastralNumber;
                            newExtractEgrn.Address = objDesc.Address?.Content;
                            newExtractEgrn.Type = objDesc.ObjectTypeText;
                            newExtractEgrn.Purpose = objDesc.Assignation_Code_Text;
                            newExtractEgrn.Area = objDesc.Area?.Area.ToDecimal()??0;                  
                        }
                        if (obj is Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightOwner)
                        {
                            var ow = obj as Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightOwner;
                            if (ow != null && !owners.ContainsKey(ow.OwnerNumber))
                            {
                                owners.Add(ow.OwnerNumber, ow);
                            }
                        }
                        if (obj is Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightRegistration)
                        {
                            var reg = obj as Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightRegistration;
                            if (reg != null && !regisrets.ContainsKey(reg.RegistrNumber))
                            {
                                regisrets.Add(reg.RegistrNumber, reg);
                            }
                        }

                    }
                    try
                    {
                        newExtractEgrn.ExtractId = extract;
                        extractEgrnService.Save(newExtractEgrn);
                    }
                    catch (Exception e)
                    {
                        extract.Comment = e.Message + " " + e.StackTrace;
                        extract.IsParsed = true;
                        extractService.Update(extract);
                    }
                    foreach (KeyValuePair<string, Sobits.RosReg.Entities.ExtractList07.ExtractReestrExtractExtractObjectRightRegistration> reg in regisrets)
                    {
                        if (string.IsNullOrEmpty(reg.Value.EndDate))
                        {
                            if (owners.ContainsKey(reg.Key))
                            {
                                var ow = owners[reg.Key];
                                string share = string.IsNullOrEmpty(reg.Value.ShareText) ? "1" : reg.Value.ShareText;
                                var newExtractRight = new ExtractEgrnRight
                                {
                                    Type = reg.Value.Name,
                                    Number = reg.Value.RegNumber,
                                    EgrnId = newExtractEgrn,
                                    Share = share
                                };

                                extractEgrnRightService.Save(newExtractRight);

                                if (ow.Person != null)
                                {
                                    var newExtractRightInd = new ExtractEgrnRightInd
                                    {
                                        Surname = ow.Person.FIO?.Surname,
                                        FirstName = ow.Person.FIO?.First,
                                        Patronymic = ow.Person.FIO?.Patronymic
                                    };                                
                                    newExtractRightInd.RightId = newExtractRight;
                                    extractEgrnRightIndService.Save(newExtractRightInd);
                                }
                               
                            }
                        }
                       
                    }
                }                


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

               
                newExtractEgrn.IsMerged = Bars.Gkh.Enums.YesNoNotSet.No;
                try
                {
                    extractEgrnService.Update(newExtractEgrn);
                }
                catch (Exception e)
                {

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
        private XElement ToXElement<T>(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private T Deserialize<T>(XElement element)
        where T : class
        {
            XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(element.ToString()))
                return (T)ser.Deserialize(sr);
        }

    }
}