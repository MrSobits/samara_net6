namespace Bars.GkhGji.DataProviders
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Contracts.Meta;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;

    public class ProtocolGjiDataProvider : DocumentGjiDataProvider<ProtocolGjiProxy>
    {
        public ProtocolGjiDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        #region DomainServices

        #endregion

        protected override IQueryable<ProtocolGjiProxy> GetDataInternal(BaseParams baseParams)
        {
            var protocolDomain = Container.ResolveDomain<Protocol>();
            var protocolViolDomain = Container.ResolveDomain<ProtocolViolation>();
            var protocolArticleLawDomain = Container.ResolveDomain<ProtocolArticleLaw>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();

            try
            {
                var documentId = baseParams.Params.GetAsId("DocumentId");
                var protocol = protocolDomain.FirstOrDefault(x => x.Id == documentId);

                if (protocol == null)
                {
                    throw new ReportProviderException("Не удалось получить протокол");
                }

                var protocolProxy = new ProtocolGjiProxy();
                FillCommonFields(protocol, protocolProxy);

                if (protocol.Contragent != null)
                {
                    protocolProxy.Contragent = new Contragent
                    {
                        FactAddress = protocol.Contragent.FactAddress,
                        JurAddress = protocol.Contragent.JuridicalAddress,
                        Name = protocol.Contragent.Name,
                        ShortName = protocol.Contragent.ShortName,
                        Name_РодитПадеж = protocol.Contragent.NameGenitive,
                        Inn = protocol.Contragent.Inn,
                        Ogrn = protocol.Contragent.Ogrn,
                        RegistrationDate = protocol.Contragent.DateRegistration.ToDateTime()
                    };
                }

                var actCheck = GetParentDocument(protocol, TypeDocumentGji.ActCheck);

                if (actCheck != null)
                {
                    var actCheckProxy = new ActCheckProxy();
                    FillCommonFields(actCheck, actCheckProxy);

                    protocolProxy.ActCheck = actCheckProxy;
                }

                var roInfos = protocolViolDomain.GetAll()
                    .Where(x => x.Document.Id == documentId)
                    .Select(x => new
                    {
                        Municipality =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Municipality.Name
                                : "",
                        PlaceName =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.FiasAddress.PlaceName
                                : "",
                        RealityObject =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Address
                                : "",
                        Id = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                    })
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.RealityObject)
                    .ToList()
                    .Select(x => new Realty
                    {
                        Address = x.RealityObject,
                        SettlementName = x.PlaceName
                    }).ToList();

                protocolProxy.RealtyObjs = roInfos;

                var articles = protocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == documentId)
                    .Select(x => new ArticleLaw
                    {
                        Name = x.ArticleLaw.Name
                    })
                    .ToList();

                protocolProxy.ArticleLaws = articles;

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == documentId)
                    .Select(x => new Inspector
                    {
                        Code = x.Inspector.Code,
                        FullName = x.Inspector.Fio,
                        ShortName = x.Inspector.ShortFio,
                        FullName_ТворПадеж = x.Inspector.FioAblative,
                        Position = x.Inspector.Position
                    })
                    .ToList();

                protocolProxy.Inspectors = inspectors;

                protocolProxy.DateOfProceedings = protocol.DateOfProceedings.ToDateTime();
                protocolProxy.HourOfProceedings = protocol.HourOfProceedings;
                protocolProxy.MinuteOfProceedings = protocol.MinuteOfProceedings;

                return new[] { protocolProxy }.AsQueryable();
            }
            finally
            {
                Container.Release(protocolDomain);
            }
        }

        public override string Name
        {
            get { return "Протокол"; }
        }

        public override string Description
        {
            get { return "Протокол(базовый)"; }
        }

        protected virtual void FillRegionFields(Disposal disposal, DisposalProxy disposalProxy)
        {
        }
    }
}
