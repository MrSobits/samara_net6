using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.GkhGji.Report.Form1Controll
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    class Form1ControlSection3 : BaseReportSection
    {
        public Form1ControlSection3(List<long> inspections, DateTime dateStart, DateTime dateEnd, IWindsorContainer container, List<long> municipalityIds)
            : base(inspections, dateStart, dateEnd)
        {
            this.municipalityListId = municipalityIds;
            this.Container = container;
        }

        public IWindsorContainer Container { get; set; }

        private List<long> municipalityListId;

        private List<long> listContagetn = new List<long>();
        private List<DisposalTypeAgreement> listDisp = new List<DisposalTypeAgreement>();

        private int GetCell54_5()
        {
            return
                Container.Resolve<IDomainService<ManagingOrganization>>()
                         .GetAll()
                         .WhereIf(municipalityListId.Count > 0, x => municipalityListId.Contains(x.Contragent.Municipality.Id))
                         .Count(x => (x.TypeManagement == TypeManagementManOrg.UK || x.TypeManagement == TypeManagementManOrg.TSJ)
                             && x.OrgStateRole == OrgStateRole.Active);
        }

        private int GetCell55_5()
        {
            var start = 0;

            var listDisposal = new List<long>();
            while (start<inspectionsIds.Length)
            {
                var tmpIds = inspectionsIds.Skip(start).Take(1000).ToArray();
                listDisposal.AddRange(
                Container.Resolve<IDomainService<DocumentGji>>()
                         .GetAll()
                         .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                         .Where(x => tmpIds.Contains(x.Inspection.Id))
                         .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                         .Select(x => x.Id)
                         .ToList());
                start += 1000;
            }            

          start = 0;

            while (start < listDisposal.Count)
            {
                var tmpId = listDisposal.Skip(start).Take(1000).ToArray();
                var tmpData = Container.Resolve<IDomainService<DocumentGjiChildren>>()
                         .GetAll()
                         .Where(x => tmpId.Contains(x.Parent.Id) && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck
                             && x.Parent.Inspection.Contragent != null)
                         .Select(x => x.Parent.Inspection.Contragent.Id)
                         .ToList();
                listContagetn.AddRange(tmpData);
                start += 1000;
            }

            return listContagetn.Select(x => x).Distinct().Count();
        }

        private int GetCell56_5()
        {
            var listContrId = listContagetn.Select(x => x).Distinct().ToArray();

            var listNameManOrg = new List<string>();
            var start = 0;
            while (start < listContrId.Length)
            {
                var tmpIds = listContrId.Skip(start).Take(1000).ToArray();
                var tmpData =
                    Container.Resolve<IDomainService<ManagingOrganization>>()
                             .GetAll()
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
            while (start < inspectionsIds.Length)
            {
                var tmpId = inspectionsIds.Skip(start).Take(1000).ToArray();
                result +=
                    Container.Resolve<IDomainService<BaseJurPerson>>()
                             .GetAll()
                             .Count(x => x.DateStart.Value.Year == dateStart.Year && tmpId.Contains(x.Id));
                start += 1000;
            }

            return result;
        }

        private int GetCell58_5()
        {
            var result = 0;
            var start = 0;
            while (start < inspectionsIds.Length)
            {
                var tmpId = inspectionsIds.Skip(start).Take(1000).ToArray();
                result +=
                    Container.Resolve<IDomainService<BaseJurPerson>>()
                             .GetAll()
                             .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                             .WhereIf(dateEnd != DateTime.MinValue, x => x.DateStart <= dateEnd)
                             .Count(x => tmpId.Contains(x.Id));
                start += 1000;
            }

            return result;
        }

        private double GetCell60_5()
        {
            var listBaseJurPerson = new List<long>();
            var start = 0;

            while (start<inspectionsIds.Length)
            {
                var tmpId = inspectionsIds.Skip(start).Take(1000).ToArray();
                listBaseJurPerson.AddRange(
                    Container.Resolve<IDomainService<BaseJurPerson>>()
                             .GetAll()
                             .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                             .WhereIf(dateEnd != DateTime.MinValue, x => x.DateStart <= dateEnd)
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
                    Container.Resolve<IDomainService<DocumentGji>>()
                             .GetAll()
                             .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal && tmpIds.Contains(x.Inspection.Id))
                             .WhereIf(municipalityListId.Count > 0, x => municipalityListId.Contains(x.Inspection.Contragent.Municipality.Id))
                             .Select(x => x.Inspection.Id)
                             .ToArray();
                listBaseJurPersonDisp.AddRange(tmpData);
                start += 1000;
            }

            var num60 = 0.00;
            if (listBaseJurPerson.Distinct().Count() > 0)
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
            while (start < inspectionsIds.Count())
            {
                listDisp.AddRange(
                Container.Resolve<IDomainService<Disposal>>()
                         .GetAll()
                         .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                         .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                         .Where(x => x.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
                         .Where(x => inspectionsIds.Skip(start).Take(1000).ToArray().Contains(x.Inspection.Id))
                         .Select(x => new DisposalTypeAgreement { Id = x.Id, TypeAgreementResult = x.TypeAgreementResult })
                         .ToList());
                start += 1000;
            }
            
            return listDisp.Distinct().Count();
        }

        private int GetCell62_5()
        {
            return
                listDisp.Where(x => x.TypeAgreementResult == TypeAgreementResult.NotAgreed)
                        .Select(x => x.Id)
                        .Distinct()
                        .Count();
        }

        private int GetCell63_5()
        {
            return
                listDisp.Where(x => x.TypeAgreementResult == TypeAgreementResult.Agreed)
                        .Select(x => x.Id)
                        .Distinct()
                        .Count();
        }

        private int GetCell64_5()
        {
            var start = 0;

            var listDisp64 = new List<long>();
            while(start<inspectionsIds.Length)
            {
                listDisp64.AddRange(
                    Container.Resolve<IDomainService<DocumentGji>>()
                             .GetAll()
                             .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                             .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                             .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal)
                             .Where(x => inspectionsIds.Skip(start).Take(1000).ToArray().Contains(x.Inspection.Id))
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
                    Container.Resolve<IDomainService<DisposalExpert>>()
                             .GetAll()
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
