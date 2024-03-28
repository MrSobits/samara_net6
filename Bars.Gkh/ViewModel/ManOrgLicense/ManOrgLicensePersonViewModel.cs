namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class ManOrgLicensePersonViewModel : BaseViewModel<ManOrgLicensePerson>
    {
        public IDomainService<PersonPlaceWork> PlaceWorkDomain { get; set; }

        public IDomainService<ManOrgLicense> LicenseDomain { get; set; }

        public override IDataResult List(IDomainService<ManOrgLicensePerson> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var licenseId = loadParams.Filter.GetAs("licenseId", 0L);

            var license = this.LicenseDomain.FirstOrDefault(x => x.Id == licenseId);
            var contragentId = license.Contragent?.Id ?? 0;

            var placeWorkDict = this.PlaceWorkDomain.GetAll()
                .Where(x => x.Contragent.Id == contragentId && x.StartDate.HasValue)
                .Select(
                    x => new
                    {
                        personId = x.Person.Id,
                        position = x.Position.Name,
                        date = x.StartDate.Value
                    })
                .AsEnumerable()
                .GroupBy(x => x.personId)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.date).Select(z => z.position).First());

            return domain.GetAll()
                .Where(x => x.ManOrgLicense.Id == licenseId)
                .Select(
                    x => new
                    {
                        x.Id,
                        Person = x.Person.Id,
                        PersonFullName = x.Person.FullName,
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Person,
                        x.PersonFullName,
                        Position = placeWorkDict.ContainsKey(x.Person) ? placeWorkDict[x.Person] : string.Empty
                    })
                .AsQueryable()
                .ToListDataResult(loadParams, this.Container);
        }
    }
}