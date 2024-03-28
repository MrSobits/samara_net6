namespace Bars.GkhGji.Domain.SpecialAccountReport.Serialize
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Extensions;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;
    using System;

    /// <summary>Провайдер для формирования Xml по паспорту</summary>
    public class SpecialAccountReportSerializeProvider
    {
        private readonly IDomainService<SpecialAccountRow> domainSpecialAccountReportRow;

        public SpecialAccountReportSerializeProvider(IWindsorContainer container)
        {
            domainSpecialAccountReportRow = container.Resolve<IDomainService<SpecialAccountRow>>();
        }

        /// <summary>Получить Xml</summary>
        /// <param name="houseProviderPassport">Паспорт поставщика дома</param>
        /// <returns>Xml</returns>
        public Stream GetStreamXml(SpecialAccountReport specialAccountReport)
        {
            var values = domainSpecialAccountReportRow.GetAll()
                .Where(x => x.SpecialAccountReport.Id == specialAccountReport.Id)
                .Select(x => new BaseSpecialAccountReportRow
                    {
                        SpecialAccountReport = x.SpecialAccountReport,
                        Incoming = x.Incoming,
                        AmmountDebt = x.AmmountDebt,
                        Transfer = x.Transfer,
                        Municipality = x.Municipality,
                        SpecialAccountNum = x.SpecialAccountNum,
                        RealityObject = x.RealityObject,
                        Ballance = x.Ballance,
                        Id = x.Id,
                        ObjectCreateDate = x.ObjectCreateDate,
                        ObjectEditDate = x.ObjectEditDate,
                        ObjectVersion = x.ObjectVersion
                }
                )
                .ToArray();

            return StreamXml(specialAccountReport, values);
            //return new MemoryStream();
        }

        private Stream StreamXml(SpecialAccountReport specialAccountReport, BaseSpecialAccountReportRow[] values)
        {
            var report =  new Report
                    {
                        ReportYear = specialAccountReport.YearEnums.GetDisplayName(),
                        ReportMonth = specialAccountReport.MonthEnums.GetDisplayName(),
                        Contragent = specialAccountReport.Contragent == null
                            ? null
                            : new ReportContragent()
                            {
                                Name = specialAccountReport.Contragent.Name,
                                Inn = specialAccountReport.Contragent.Inn,
                                Kpp = specialAccountReport.Contragent.Kpp,
                                Ogrn = specialAccountReport.Contragent.Ogrn
                            },

                        Elements = new List<ReportElement>()
                    };


            foreach (var part in values)
            {
                var element = new ReportElement
                {
                    Address = part.RealityObject.Address,
                    AmmountDebt = part.AmmountDebt,
                    Ballance = part.Ballance,
                    Incoming = part.Incoming,
                    MunicipalityName = part.Municipality.Name,
                    SpecialAccountNum = part.SpecialAccountNum,
                    Transfer = part.Transfer
                };
                report.Elements.Add(element);
            }


            var serializer = new XmlSerializer(typeof (Report));

            var result = new MemoryStream();
            var writer = XmlWriter.Create(result);

            serializer.Serialize(writer, report);

            result.Seek(0, SeekOrigin.Begin);

            return result;
        }
    }
}