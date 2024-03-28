namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class DecisionControlSubjectsViewModel : BaseViewModel<DecisionControlSubjects>
    {
        public override IDataResult List(IDomainService<DecisionControlSubjects> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.Decision.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.PersonInspection,
                    x.PhysicalPersonINN,
                    Contragent = x.Contragent != null? x.Contragent.Name:"",
                    x.PhysicalPerson
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}