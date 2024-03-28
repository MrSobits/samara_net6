namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckViewModel : ActCheckViewModel<ActCheck>
    {
    }

    public class ActCheckViewModel<T> : BaseViewModel<T>
        where T: ActCheck
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var stageId = baseParams.Params.ContainsKey("stageId")
                                   ? int.Parse(baseParams.Params["stageId"].ToString())
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Stage.Id == stageId)
                .Select(x => new
                {
                    x.Id,
                    DocumentId = x.Id,
                    x.Inspection,
                    x.Stage,
                    x.TypeDocumentGji,
                    x.DocumentDate,
                    x.DocumentNumber,
                    SignatoryInspector = x.SignatoryInspector.Fio 
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

#warning переделал метод принимающий параметры (object) -> (domainService, baseParams)
        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var refService = Container.Resolve<IDomainService<DocumentGjiReference>>();
            var actRealityObjectService = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            try
            {
                var intId = baseParams.Params.GetAs<long>("id", ignoreCase: true);

                var obj = domainService.Get(intId);

                // Тут мы получаем документ переданный в прокуратуру
                if (obj.ToProsecutor == YesNoNotSet.Yes)
                {
                    var docRef = refService.GetAll()
                        .FirstOrDefault(x => x.TypeReference == TypeDocumentReferenceGji.ActCheckToProsecutor
                                        && (x.Document1.Id == intId || x.Document2.Id == intId));

                    if (docRef != null)
                    {
                        obj.ResolutionProsecutor = (docRef.Document1.Id == intId ? docRef.Document2 : docRef.Document1);
                    }
                }

                // Получаем поличество домов в акте
                var cnt = actRealityObjectService.GetAll().Count(x => x.ActCheck.Id == intId);
                if (cnt == 1)
                {
                    obj.ActCheckGjiRealityObject = actRealityObjectService.GetAll().FirstOrDefault(x => x.ActCheck.Id == intId);
                }

                return new BaseDataResult(obj);
            }
            finally 
            {
                Container.Release(refService);
                Container.Release(actRealityObjectService);
            }
            
        }
    }
}