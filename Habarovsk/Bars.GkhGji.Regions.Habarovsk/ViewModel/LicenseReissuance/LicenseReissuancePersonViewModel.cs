﻿using System.Linq;
using Bars.B4.DataAccess;

namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{

    using B4;
    using Entities;
    using Bars.Gkh.Entities;

    public class LicenseReissuancePersonViewModel : BaseViewModel<LicenseReissuancePerson>
    {

        public IDomainService<PersonPlaceWork> placeWorkDomain { get; set; }

        public IDomainService<LicenseReissuance> requestDomain { get; set; }

        public override IDataResult List(IDomainService<LicenseReissuancePerson> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestId = loadParams.Filter.GetAs("requestId", 0L);

            var request = requestDomain.FirstOrDefault(x => x.Id == requestId);
            var ctrId = request.Contragent != null ? request.Contragent.Id : 0;

            var placeWorkDict = placeWorkDomain.GetAll()
                .Where(x => x.Contragent.Id == ctrId && x.StartDate.HasValue)
                .Select(x => new
                {
                    personId = x.Person.Id,
                    position = x.Position.Name,
                    date = x.StartDate.Value
                })
                .AsEnumerable()
                .GroupBy(x => x.personId)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.date).Select(z => z.position).First());
            
            var data = domain.GetAll()
                .Where(x=>x.LicenseReissuance.Id == requestId)
               .Select(x => new
               {
                   x.Id,
                   Person = x.Person.Id,
                   PersonFullName = x.Person.FullName,
               })
               .AsEnumerable()
               .Select(x => new
               {
                   x.Id,
                   x.Person,
                   x.PersonFullName,
                   Position = placeWorkDict.ContainsKey(x.Person) ? placeWorkDict[x.Person] : string.Empty
               })
               .AsQueryable()
               .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}