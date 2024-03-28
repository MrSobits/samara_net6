namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ResolPros
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;

    public class ResolProsTatarstanViewModel : BaseViewModel<ResolPros>
    {
        public override IDataResult List(IDomainService<ResolPros> domainService, BaseParams baseParams)
        {
            var resolProsAndResolution = this.Container.Resolve<IResolProsAndResolutionService>();

            try
            {
                var list = resolProsAndResolution.GetList(baseParams, domainService, false, true);

                return new ListDataResult(list.Paging(baseParams.GetLoadParam()).ToList(), list.Count);
            }
            finally
            {
                this.Container.Release(resolProsAndResolution);
            }
        }

#warning переделать какая то хрень тут происходит. Такк не должно быть

        public override IDataResult Get(IDomainService<ResolPros> domainService, BaseParams baseParams)
        {
            var servDocRef = Container.Resolve<IDomainService<DocumentGjiReference>>();

            try
            {
                var intId = baseParams.Params.GetAs("id", 0L);
                var obj = domainService.GetAll().FirstOrDefault(x => x.Id == intId);

                // Тут мы получаем документ акта проверки который был передан в прокуратуру
                // Через таблицу референсов документов (Предполагается что референсы могут быть самые разные в будущем)
                var docRef = servDocRef.GetAll()
                    .FirstOrDefault(x => x.TypeReference == TypeDocumentReferenceGji.ActCheckToProsecutor
                        && (x.Document1.Id == intId || x.Document2.Id == intId));

                if (docRef != null)
                {
                    obj.ActCheck = docRef.Document1.Id == intId ? docRef.Document2 : docRef.Document1;
                }

                //ToDo ГЖИ непомню зачем это нужно после прехода на правила выпилить
                obj.InspectionId = obj.Inspection.Id;

                return new BaseDataResult(obj);
            }
            finally
            {
                Container.Release(servDocRef);
            }
        }
    }
}