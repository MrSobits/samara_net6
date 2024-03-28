namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Voronezh.Entities;

    public class LawsuitOwnerRepresentativeViewModel : BaseViewModel<LawsuitOwnerRepresentative>
    {
        public override IDataResult List(IDomainService<LawsuitOwnerRepresentative> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            long rloi = 0;
            rloi = loadParams.Filter["Rloi"].ToLong();
            
            if (rloi > 0)
            {
                var data = domain.GetAll()
                    .Where(x => x.Rloi.Id == rloi)
                    .Select(x => new
                    {
                        x.Id,
                        x.Rloi,
                        x.RepresentativeType,
                        x.Surname,
                        x.FirstName,
                        x.Patronymic,
                        x.BirthDate,
                        x.BirthPlace,
                        x.LivePlace,
                        x.Note
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var data = domain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Rloi,
                        x.RepresentativeType,
                        x.Surname,
                        x.FirstName,
                        x.Patronymic,
                        x.BirthDate,
                        x.BirthPlace,
                        x.LivePlace,
                        x.Note
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}