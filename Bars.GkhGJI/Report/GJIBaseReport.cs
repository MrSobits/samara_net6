namespace Bars.GkhGji.Report
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public abstract class GjiBaseReport : GkhBaseReport
    {
        protected GjiBaseReport(IReportTemplate reportTemplate) : base(reportTemplate)
        {

        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
        {
            var result = document;

            if (document.TypeDocumentGji != type)
            {
                var docs = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                                    .Where(x => x.Children.Id == document.Id)
                                    .Select(x => x.Parent)
                                    .ToList();

                foreach (var doc in docs)
                {
                    result = GetParentDocument(doc, type);
                }
            }

            if (result != null)
            {
                return result.TypeDocumentGji == type ? result : null;
            }

            return null;
        }

        public DocumentGji GetChildDocument(DocumentGji document, TypeDocumentGji type)
        {
            var result = document;

            if (document.TypeDocumentGji != type)
            {
                var docs = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                                    .Where(x => x.Parent.Id == document.Id)
                                    .Select(x => x.Children)
                                    .ToList();

                foreach (var doc in docs)
                {
                    result = GetChildDocument(doc, type);
                }
            }

            if (result != null)
            {
                return result.TypeDocumentGji == type ? result : null;
            }

            return null;
        }

        protected void FillCommonFields(ReportParams reportParams, DocumentGji doc)
        {
            reportParams.SimpleReportParams["ИдентификаторДокументаГЖИ"] = doc.Id;
            reportParams.SimpleReportParams["СтрокаПодключениякБД"] =
                Container.Resolve<IDbConfigProvider>().ConnectionString;
            // зональную инспекцию получаем через муниципальное образование первого дома
            var firstRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Where(x => x.Inspection.Id == doc.Inspection.Id)
                .Select(x => x.RealityObject)
                .FirstOrDefault();

            var docType = doc.Inspection.TypeBase;

            if (docType == TypeBase.ProsecutorsResolution)
            {
                firstRo = Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
                         .Where(x => x.ResolPros.Inspection.Id == doc.Inspection.Id)
                         .Select(x => x.RealityObject)
                         .FirstOrDefault();
            }

            if (firstRo != null)
            {
                var zonalInspection = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                    .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                    .Select(x => x.ZonalInspection)
                    .FirstOrDefault();

                if (zonalInspection != null)
                {
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                    reportParams.SimpleReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык1"] = zonalInspection.ZoneName;
                    reportParams.SimpleReportParams["Адрес1ГосЯзык"] = zonalInspection.Address;
                    reportParams.SimpleReportParams["Адрес2ГосЯзык"] = zonalInspection.AddressSecond;
                    reportParams.SimpleReportParams["Телефон"] = zonalInspection.Phone;
                    reportParams.SimpleReportParams["Email"] = zonalInspection.Email;

                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык(ТворП)"] = zonalInspection.NameAblative;
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык(РП)"] = zonalInspection.NameGenetive;
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык(ВП)"] = zonalInspection.NameAccusative;
                }

                if (doc.Inspection != null)
                {
                    if (doc.Inspection.Contragent != null)
                    {
                        reportParams.SimpleReportParams["УправОрг(РП)"] = doc.Inspection.Contragent.NameGenitive;
                        reportParams.SimpleReportParams["АдресКонтрагента"] = doc.Inspection.Contragent.FiasJuridicalAddress != null ? doc.Inspection.Contragent.FiasJuridicalAddress.AddressName : "";
                    }

                    if (doc.Inspection.TypeBase == TypeBase.CitizenStatement)
                    {
                        var appeals =
                            Container.Resolve<IDomainService<InspectionAppealCits>>()
                                     .GetAll()
                                     .Where(x => x.Inspection.Id == doc.Inspection.Id).ToList();

                        if (appeals.Count > 0)
                        {
                            reportParams.SimpleReportParams["Корреспондент"] = String.Join(",", appeals.Select(x => x.AppealCits.Correspondent));

                            reportParams.SimpleReportParams["АдресКорреспондента"] = String.Join(",", appeals.Select(x => x.AppealCits.CorrespondentAddress));
                        }
                    }
                }
            }
        }

        protected virtual void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {

        }
    }
}