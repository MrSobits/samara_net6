namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckWitnessViewModel : BaseViewModel<ActCheckWitness>
    {
        public override IDataResult List(IDomainService<ActCheckWitness> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActCheck.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.Position,
                    x.IsFamiliar,
                    x.Fio
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}