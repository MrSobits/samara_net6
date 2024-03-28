using System;
using System.Collections.Generic;
using System.Linq;
using Bars.B4;
using Bars.B4.Modules.Reports;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Nso.Entities;
using Slepov.Russian.Morpher;

namespace Bars.GkhGji.Regions.Nso.Report
{
    public class ProtocolGjiReport : GkhGji.Report.ProtocolGjiReport
    {
        protected override void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var protocol = (Protocol) doc;
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            var physicalPersonsCode = new List<string> { "1", "3", "5", "6", "7", "10", "12", "13", "14", "16", "19" };
            var jurPersonsCode = new List<string> { "0", "2", "4", "8", "9", "11", "15", "18" };

            var contragentContactDomain = Container.Resolve<IDomainService<ContragentContact>>();
            var nsoProtocolDomain = Container.Resolve<IDomainService<NsoProtocol>>();

            var nsoProtocol = nsoProtocolDomain.GetAll().FirstOrDefault(x => x.Id == doc.Id);

            var formatDate = string.Empty;
            var notifNumber = string.Empty;
            if (nsoProtocol != null)
            {
                formatDate = nsoProtocol.FormatDate.HasValue ? nsoProtocol.FormatDate.Value.ToShortDateString() : string.Empty;
                notifNumber = nsoProtocol.NotifNumber;
                reportParams.SimpleReportParams["МестоРассмотренияДела"] = nsoProtocol.ProceedingsPlace;
            }

            if (protocol.Executant != null)
            {
                Position position = null;
                var headContragentFioGenetive = string.Empty;
                var headContragentShortFio = string.Empty;
                var сontragentInn = string.Empty;
                var сontragentJurAddress = string.Empty;
                var сontragentFactAddress = string.Empty;

                if (protocol.Contragent != null)
                {
                    var headContragent = contragentContactDomain.GetAll()
                        .Where(x => x.Contragent.Id == protocol.Contragent.Id)
                        .Where(x => x.DateStartWork.HasValue)
                        .Where(x => x.DateStartWork.Value <= DateTime.Now)
                        .Where(x => !x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)
                        .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                    сontragentInn = protocol.Contragent.Inn;
                    сontragentJurAddress = protocol.Contragent.JuridicalAddress;
                    сontragentFactAddress = protocol.Contragent.FactAddress;

                    if (headContragent != null)
                    {
                        headContragentFioGenetive = string.Format("{0} {1}. {2}.",
                            headContragent.SurnameGenitive,
                            headContragent.NameGenitive.Length > 0 ? headContragent.NameGenitive.Substring(0, 1) : null,
                            headContragent.PatronymicGenitive.Length > 0 ? headContragent.PatronymicGenitive.Substring(0, 1) : null);

                        headContragentShortFio = string.Format("{0} {1} {2}",
                            headContragent.Surname,
                            headContragent.Name.Length > 0 ? headContragent.Name.Substring(0, 1) + "." : null,
                            headContragent.Patronymic.Length > 0 ? headContragent.Patronymic.Substring(0, 1) + "." : null);
                        position = headContragent.Position;
                    }
                }

                var contragentShortName = protocol.Contragent != null ? protocol.Contragent.ShortName : string.Empty;

                if (jurPersonsCode.Contains(protocol.Executant.Code) )
                {
                    reportParams.SimpleReportParams["Присутствующий"] = string.Format("представителя {0} {1},  действующего на основании_______________________", contragentShortName, headContragentFioGenetive);
                    reportParams.SimpleReportParams["УведомлениеОВручении"] = string.Format("Уведомление о месте и времени составления протокола вручено через канцелярию {0} {1} №{2}", contragentShortName, formatDate, notifNumber);
                    reportParams.SimpleReportParams["НаКогоСоставлен"] = string.Format("юридическим лицом  {0}  {1}, {2} \n адрес местонахождения {3} {4} {5}",
                        contragentShortName, сontragentInn, сontragentJurAddress, сontragentFactAddress, position != null ? position.Name : string.Empty, headContragentShortFio);
                    reportParams.SimpleReportParams["КемУстановлено"] =  string.Format("установлено, что {0}", contragentShortName);
                    reportParams.SimpleReportParams["нарушитель"] =   contragentShortName;
                    reportParams.SimpleReportParams["ОбъяснениеПрав"] =  contragentShortName;
                }

                var physicalPerson = склонятель.Проанализировать(protocol.PhysicalPerson ?? string.Empty);
                if (physicalPersonsCode.Contains(protocol.Executant.Code))
                {
                    reportParams.SimpleReportParams["Присутствующий"] = string.Format("{0} {1}", physicalPerson != null ? physicalPerson.Родительный : string.Empty, contragentShortName);
                    reportParams.SimpleReportParams["УведомлениеОВручении"] = string.Format("Уведомление о месте и времени составления протокола вручено {0} лично {1}", formatDate, physicalPerson != null ? physicalPerson.Дательный : string.Empty);
                    reportParams.SimpleReportParams["НаКогоСоставлен"] = string.Format("должностным лицом {0}  {1}", physicalPerson != null ? physicalPerson.Творительный : string.Empty, protocol.PhysicalPersonInfo);
                    reportParams.SimpleReportParams["КемУстановлено"] = string.Format("установлено, что  должностным лицом {0} {1}", physicalPerson != null ? physicalPerson.Творительный : string.Empty, protocol.PhysicalPersonInfo);
                    reportParams.SimpleReportParams["нарушитель"] = physicalPerson != null ? physicalPerson.Родительный : string.Empty;
                    reportParams.SimpleReportParams["ОбъяснениеПрав"] = string.Format("Должностному лицу {0}", physicalPerson != null ? physicalPerson.Дательный : string.Empty);

                }
            }
        }
    }
}