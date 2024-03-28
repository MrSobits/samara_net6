namespace Bars.GkhGji.DataProviders
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Meta;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public abstract class DocumentGjiDataProvider<T> : BaseCollectionDataProvider<T> where T : class, new()
    {
        protected DocumentGjiDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        protected virtual void FillCommonFields(DocumentGji doc, DocumentGjiProxy proxy)
        {
            var inspRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var resolProsRoDomain = Container.Resolve<IDomainService<ResolProsRealityObject>>();
            var zonalInspMuDomain = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
            var inspAppealCitsDomain = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var inspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();


            using (Container.Using(inspRoDomain, resolProsRoDomain,
                zonalInspMuDomain, inspAppealCitsDomain, inspectorDomain))
            {
                var docType = doc.Inspection.TypeBase;

                var firstRo = inspRoDomain.GetAll()
                   .Where(x => x.Inspection.Id == doc.Inspection.Id)
                   .Select(x => x.RealityObject)
                   .FirstOrDefault();

                if (docType == TypeBase.ProsecutorsResolution)
                {
                    firstRo = resolProsRoDomain.GetAll()
                             .Where(x => x.ResolPros.Inspection.Id == doc.Inspection.Id)
                             .Select(x => x.RealityObject)
                             .FirstOrDefault();
                }

                if (firstRo != null)
                {
                    var zonalInspection = zonalInspMuDomain.GetAll()
                        .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                        .Select(x => x.ZonalInspection)
                        .FirstOrDefault();

                    if (zonalInspection != null)
                    {
                        proxy.InspectionNameLang1 = zonalInspection.BlankName;
                        proxy.InspectionNameLang2 = zonalInspection.BlankNameSecond;
                        proxy.InspectionNameZoneName = zonalInspection.ZoneName;
                        proxy.InspectionAddressLang1 = zonalInspection.Address;
                        proxy.InspectionAddressLang2 = zonalInspection.AddressSecond;
                        proxy.InspectionPhone = zonalInspection.Phone;
                        proxy.InspectionEmail = zonalInspection.Email;

                        proxy.InspectionNameLang1_ТворПадеж = zonalInspection.NameAblative;
                        proxy.InspectionNameLang1_РодитПадеж = zonalInspection.NameGenetive;
                        proxy.InspectionNameLang1_ВинитПадеж = zonalInspection.NameAccusative;
                    }

                    if (doc.Inspection != null)
                    {
                        if (doc.Inspection.Contragent != null)
                        {
                            proxy.InspectionContragentNameGenitive = doc.Inspection.Contragent.NameGenitive;
                            proxy.InspectionContragentAddress = doc.Inspection.Contragent.FiasJuridicalAddress != null ? doc.Inspection.Contragent.FiasJuridicalAddress.AddressName : string.Empty;
                        }

                        if (doc.Inspection.TypeBase == TypeBase.CitizenStatement)
                        {
                            var appeals =
                                inspAppealCitsDomain.GetAll().Where(x => x.Inspection.Id == doc.Inspection.Id).ToList();

                            if (appeals.Any())
                            {
                                proxy.InspectionAppealCitsCorrespondent = string.Join(",", appeals.Select(x => x.AppealCits.Correspondent));
                                proxy.InspectionAppealCitsCorrespondentAddress = string.Join(",", appeals.Select(x => x.AppealCits.CorrespondentAddress));
                            }
                        }
                    }
                }

                proxy.DocNumber = doc.DocumentNumber;
                if (doc.DocumentDate.HasValue)
                {
                    proxy.Date = doc.DocumentDate.Value;
                }

                if (doc.DocumentYear.HasValue)
                {
                    proxy.DocYear = doc.DocumentYear.Value;
                }

                if (doc.DocumentNum.HasValue)
                {
                    proxy.Number = doc.DocumentNum.Value.ToString();
                }

                if (doc.DocumentSubNum.HasValue)
                {
                    proxy.SubNumber = doc.DocumentSubNum.Value.ToString();
                }

                var inspectors = inspectorDomain.GetAll()
               .Where(x => x.DocumentGji.Id == doc.Id)
               .Select(x => new
               {
                   x.Inspector.Id,
                   x.Inspector.Fio,
                   x.Inspector.Position,
                   x.Inspector.ShortFio,
                   x.Inspector.FioAblative
               })
               .ToArray();
            }
        }

        public override string Name
        {
            get { return "Документ ГЖИ"; }
        }

        public override string Description
        {
            get { return "Базовый документ ГЖИ"; }
        }

        protected DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji type)
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

        protected DocumentGji GetChildDocument(DocumentGji document, TypeDocumentGji type)
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

    }
}
