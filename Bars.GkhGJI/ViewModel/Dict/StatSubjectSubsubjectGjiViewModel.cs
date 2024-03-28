namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Entities;

    /// <summary>
    /// Domain service связи тематики и подтематики обращения
    /// </summary>
    public class StatSubjectSubsubjectGjiViewModel : BaseViewModel<StatSubjectSubsubjectGji>
    {
        public override IDataResult List(IDomainService<StatSubjectSubsubjectGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            //идентификатор тематики
            var subjectId = baseParams.Params.ContainsKey("subjectId") ? baseParams.Params["subjectId"].ToLong() : -1;

            //идентификатор подтематики
            var subsubjectId = baseParams.Params.ContainsKey("subsubjectId")
                                   ? baseParams.Params["subsubjectId"].ToLong()
                                   : -1;

            //если нужно фильтровать по тематике
            if (subjectId > -1)
            {
                var data = domainService.GetAll().Where(x => x.Subject.Id == subjectId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Subsubject.Name,
                        x.Subsubject.Code,
                        x.Subsubject.ISSOPR
                    })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            //если нужно фильтровать по подтематике
            else
            {
                var data = domainService.GetAll().Where(x => x.Subsubject.Id == subsubjectId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Subject.Name,
                        x.Subject.Code
                    })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
        }
    }
}