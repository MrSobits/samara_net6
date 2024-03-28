namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolProsViewModel: ResolProsViewModel<ResolPros>
    {
    }

    public class ResolProsViewModel<T> : BaseViewModel<T>
        where T : ResolPros
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            /*
             * параметры:
             * dateStart - период с
             * dateEnd - период по
             * realityObjectId - жилой дом
             */

            var resolProsRoService = Container.Resolve<IDomainService<ResolProsRealityObject>>();
            
            try
            {
                var loadParam = baseParams.GetLoadParam();

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var stageId = baseParams.Params.GetAs<long>("stageId");

                var userManager = Container.Resolve<IGkhUserManager>();
                var municipalityList = userManager.GetMunicipalityIds();

                //словарь id пост.прокуратуры - наименование мо
                var resolProsMuDict = resolProsRoService.GetAll()
                    .WhereIf(dateStart != DateTime.MinValue, x => x.ResolPros.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.ResolPros.DocumentDate <= dateEnd)
                    .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.RealityObject.Municipality.Id))
                    .Select(x => new
                    {
                        x.ResolPros.Id,
                        x.RealityObject.Municipality.Name
                    })
                    .Where(x => x.Name != null)
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).First());

                var data = domainService.GetAll()

                    .WhereIf(municipalityList.Count > 0,
                        y => resolProsRoService.GetAll()
                            .Where(x => municipalityList.Contains(x.RealityObject.Municipality.Id))
                            .Any(x => x.ResolPros.Id == y.Id))

                    .WhereIf(realityObjectId > 0,
                        y => resolProsRoService.GetAll()
                            .Where(x => x.RealityObject.Id == realityObjectId)
                            .Any(x => x.ResolPros.Id == y.Id))

                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(stageId > 0, x => x.Stage.Id == stageId)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        DocumentNumber = x.DocumentNumber ?? "",
                        x.DocumentDate,
                        Municipality = x.Municipality != null ? x.Municipality.Name : "",
                        ProsecutorOffice = x.ProsecutorOffice != null ? x.ProsecutorOffice.Name : "",
                        Executant = x.Executant != null ? x.Executant.Name : "",
                        InspectionId = x.Inspection != null ? x.Inspection.Id : 0,
                        Contragent = x.Contragent != null ? x.Contragent.Name : "",
                        PhysicalPerson = x.PhysicalPerson ?? "",
                        x.UIN
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.DocumentNumber,
                        x.DocumentDate,
                        Municipality = x.Municipality.Length == 0 ? resolProsMuDict.Get(x.Id) : x.Municipality,
                        x.Executant,
                        x.InspectionId,
                        x.Contragent,
                        x.PhysicalPerson
                    })
                    .AsQueryable()
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParam, Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
            }
            finally 
            {
                Container.Release(resolProsRoService);
            }
        }

#warning переделать какая то хрень тут происходит. Такк не должно быть
        
        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
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