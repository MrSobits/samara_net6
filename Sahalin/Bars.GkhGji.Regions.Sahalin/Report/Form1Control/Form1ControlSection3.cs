namespace Bars.GkhGji.Regions.Sahalin.Report.Form1Control
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Enums;

    public class Form1ControlSection3 : BaseReportSection
    {
        public Form1ControlSection3(
            long[] inspections,
            DateTime dateStart,
            DateTime dateEnd,
            IWindsorContainer container,
            long[] municipalityIds)
            : base(inspections, dateStart, dateEnd)
        {
            _municipalityIds = municipalityIds;
            Container = container;
        }

        public IWindsorContainer Container { get; set; }

        private readonly long[] _municipalityIds;

        private readonly HashSet<long> _listContagetn = new HashSet<long>();
        private List<DisposalTypeAgreement> listDisp = new List<DisposalTypeAgreement>();

        private int GetCell54_5()
        {
            return
                Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                    .WhereIf(_municipalityIds.IsNotEmpty(), x => _municipalityIds.Contains(x.Contragent.Municipality.Id))
                    .Where(x => x.TypeManagement == TypeManagementManOrg.UK || x.TypeManagement == TypeManagementManOrg.TSJ)
                    .Count(x => x.OrgStateRole == OrgStateRole.Active);
        }

        private int GetCell55_5()
        {
            var listDisposal = new List<long>();

            foreach (var sectionIds in InspectionsIds.Section(1000))
            {
                listDisposal.AddRange(
                    GetDocuments<Disposal>()
                        .WhereIf(DateStart != DateTime.MinValue, x => x.DocumentDate >= DateStart)
                        .WhereIf(DateEnd != DateTime.MinValue, x => x.DocumentDate <= DateEnd)
                        .Where(x => sectionIds.Contains(x.Id))
                        .Select(x => x.Id)
                        .ToList());
            }

            var childrenDomain = Container.ResolveDomain<DocumentGjiChildren>();

            foreach (var sectionIds in listDisposal.Section(1000))
            {
                var ids = sectionIds;
                childrenDomain.GetAll()
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => x.Parent.Inspection.Contragent != null)
                    .Where(x => ids.Contains(x.Parent.Id))
                    .Select(x => x.Parent.Inspection.Contragent.Id)
                    .ForEach(x => _listContagetn.Add(x));
            }

            return _listContagetn.Count;
        }

        private int GetCell56_5()
        {
            var listContrId = _listContagetn;

            var listNameManOrg = new List<string>();
            var start = 0;
            while (start < listContrId.Count)
            {
                var tmpIds = listContrId.Skip(start).Take(1000).ToArray();
                var tmpData =
                    Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                        .Where(x => tmpIds.Contains(x.Contragent.Id) && x.TypeManagement == TypeManagementManOrg.TSJ)
                        .Select(x => x.Contragent.Name)
                        .ToArray();
                listNameManOrg.AddRange(tmpData);
                start += 1000;
            }

            return listNameManOrg.Distinct().Count();
        }

        private int GetCell57_5()
        {
            var result = 0;
            var start = 0;
            while (start < InspectionsIds.Length)
            {
                var tmpId = InspectionsIds.Skip(start).Take(1000).ToArray();
                result +=
                    Container.Resolve<IDomainService<BaseJurPerson>>().GetAll()
                        .Count(x => x.DateStart.Value.Year == DateStart.Year && tmpId.Contains(x.Id));
                start += 1000;
            }

            return result;
        }

        private int GetCell58_5()
        {
            var result = 0;
            var start = 0;
            while (start < InspectionsIds.Length)
            {
                var tmpId = InspectionsIds.Skip(start).Take(1000).ToArray();
                result +=
                    Container.Resolve<IDomainService<BaseJurPerson>>().GetAll()
                        .WhereIf(DateStart != DateTime.MinValue, x => x.DateStart >= DateStart)
                        .WhereIf(DateEnd != DateTime.MinValue, x => x.DateStart <= DateEnd)
                        .Count(x => tmpId.Contains(x.Id));
                start += 1000;
            }

            return result;
        }

        private double GetCell60_5()
        {
            var listBaseJurPerson = new List<long>();
            var start = 0;

            while (start < InspectionsIds.Length)
            {
                var tmpId = InspectionsIds.Skip(start).Take(1000).ToArray();
                listBaseJurPerson.AddRange(
                    Container.Resolve<IDomainService<BaseJurPerson>>().GetAll()
                        .WhereIf(DateStart != DateTime.MinValue, x => x.DateStart >= DateStart)
                        .WhereIf(DateEnd != DateTime.MinValue, x => x.DateStart <= DateEnd)
                        .Where(x => tmpId.Contains(x.Id))
                        .Select(x => x.Id)
                        .ToList());
                start += 1000;
            }

            var listBaseJurPersonDisp = new List<long>();
            start = 0;
            while (start < listBaseJurPerson.Count)
            {
                var tmpIds = listBaseJurPerson.Skip(start).Take(1000).ToArray();
                var tmpData =
                    GetDocuments<Disposal>()
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal && tmpIds.Contains(x.Inspection.Id))
                        .WhereIf(_municipalityIds.IsNotEmpty(), x => _municipalityIds.Contains(x.Inspection.Contragent.Municipality.Id))
                        .Select(x => x.Inspection.Id)
                        .ToArray();
                listBaseJurPersonDisp.AddRange(tmpData);
                start += 1000;
            }

            var num60 = 0.00;
            if (listBaseJurPerson.Any())
            {
                num60 = ((listBaseJurPersonDisp.Distinct().Count().ToDouble()) / (listBaseJurPerson.Distinct().Count().ToDouble())) * 100;
            }
            else
            {
                num60 = 0;
            }

            return (int)num60;
        }

        private int GetCell61_5()
        {
            var start = 0;
            while (start < InspectionsIds.Count())
            {
                listDisp.AddRange(
                    GetDocuments<Disposal>()
                        .WhereIf(DateStart != DateTime.MinValue, x => x.DocumentDate >= DateStart)
                        .WhereIf(DateEnd != DateTime.MinValue, x => x.DocumentDate <= DateEnd)
                        .Where(x => x.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
                        .Where(x => InspectionsIds.Skip(start).Take(1000).ToArray().Contains(x.Inspection.Id))
                        .Select(x => new DisposalTypeAgreement {Id = x.Id, TypeAgreementResult = x.TypeAgreementResult})
                        .ToList());
                start += 1000;
            }
            
            return listDisp.Distinct().Count();
        }

        private int GetCell62_5()
        {
            return
                listDisp
                    .Where(x => x.TypeAgreementResult == TypeAgreementResult.NotAgreed)
                    .Select(x => x.Id)
                    .Distinct()
                    .Count();
        }

        private int GetCell63_5()
        {
            return
                listDisp
                    .Where(x => x.TypeAgreementResult == TypeAgreementResult.Agreed)
                    .Select(x => x.Id)
                    .Distinct()
                    .Count();
        }

        private int GetCell64_5()
        {
            var start = 0;

            var listDisp64 = new List<long>();
            while(start<InspectionsIds.Length)
            {
                listDisp64.AddRange(
                    GetDocuments<Disposal>()
                        .WhereIf(DateStart != DateTime.MinValue, x => x.DocumentDate >= DateStart)
                        .WhereIf(DateEnd != DateTime.MinValue, x => x.DocumentDate <= DateEnd)
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => InspectionsIds.Skip(start).Take(1000).ToArray().Contains(x.Inspection.Id))
                        .Select(x => x.Id)
                        .Distinct()
                        .ToArray());
                start += 1000;
            }

            start = 0;
            var listExpertId = new List<long>();
            while (start < listDisp64.Count())
            {
                var tmpIds = listDisp64.Skip(start).Take(1000).ToArray();
                var tmpData =
                    Container.Resolve<IDomainService<DisposalExpert>>().GetAll()
                        .Where(x => tmpIds.Contains(x.Disposal.Id))
                        .Select(x => x.Expert.Id)
                        .ToArray();

                listExpertId.AddRange(tmpData);
                start += 1000;
            }

            return listExpertId.Count;
        }

        private class DisposalTypeAgreement
        {
            public long Id;

            public TypeAgreementResult TypeAgreementResult;
        }
    }
}
