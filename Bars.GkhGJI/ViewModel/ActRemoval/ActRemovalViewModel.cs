namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Utils;

	/// <summary>
	/// Вью модель для Акт проверки предписания
	/// </summary>
    public class ActRemovalViewModel: ActRemovalViewModel<ActRemoval>
    {
    }

	/// <summary>
	/// Вью модель для Акт проверки предписания
	/// </summary>
	/// <typeparam name="T">Акт проверки предписания</typeparam>
	public class ActRemovalViewModel<T> : BaseViewModel<T>
        where T: ActRemoval
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
		/// <returns>Результат получения списка</returns>
		public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            /*
             В данном методе если передан stageId, то значит требуется получить все документы Этапа Проверки
             
             Если передан documentId то требуется получить все дочерние документы акта
             То есть если Акт проверки с типом Акт проверки предписания
             то получаем все дочерние акты предписаний и по ним вытаскиваем предписания
            */

            var serviceDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            try
            {
                var loadParam = baseParams.GetLoadParam();

                var stageId = baseParams.Params.ContainsKey("stageId")
                                       ? baseParams.Params["stageId"].ToLong()
                                       : 0;

                var dictParentDocuments = new Dictionary<long, string>();
                List<long> listIds = null;
                var documentId = baseParams.Params.ContainsKey("documentId")
                                       ? baseParams.Params["documentId"].ToLong()
                                       : 0;

                if (documentId > 0)
                {
                    // получаем все id актов проверки документов (они же акты устранения нарушений)
                    listIds = new List<long>();

                    listIds.AddRange(serviceDocumentChildren.GetAll()
                        .Where(x => x.Parent.Id == documentId && x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                        .Select(x => x.Children.Id)
                        .Distinct()
                        .ToList());

                    // по полученным ids актов проверки документов получаем предписания
                    var documents = serviceDocumentChildren.GetAll()
                        .Where(x => listIds.Contains(x.Children.Id) && x.Parent.TypeDocumentGji != TypeDocumentGji.ActCheck)
                        .Select(x => new
                        {
                            ChildrenId = x.Children.Id,
                            x.Parent.DocumentDate,
                            x.Parent.DocumentNumber,
                            x.Parent.TypeDocumentGji
                        })
                        .ToList();

                    foreach (var doc in documents)
                    {
                        var docName = string.Format("{0} №{1} от {2}",
                            Utils.GetDocumentName(doc.TypeDocumentGji),
                            doc.DocumentNumber, doc.DocumentDate.ToDateTime().ToString("dd.MM.yyyy"));

                        if (dictParentDocuments.ContainsKey(doc.ChildrenId))
                        {
                            dictParentDocuments[doc.ChildrenId] += ", " + docName;
                        }
                        else
                        {
                            dictParentDocuments.Add(doc.ChildrenId, docName);
                        }
                    }
                }

#warning проверить, действительно ли нужны все поля, которые указаны здесь
                var data = domainService.GetAll()
                    .WhereIf(stageId > 0, x => x.Stage.Id == stageId)
                    .WhereIf(listIds != null, x => listIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        DocumentId = x.Id,
                        x.Inspection,
                        x.Stage,
                        x.TypeDocumentGji,
                        x.TypeRemoval,
                        x.DocumentDate,
                        x.DocumentNumber,
						x.State
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentId,
                        ParentDocumentName = dictParentDocuments.ContainsKey(x.Id) ? dictParentDocuments[x.Id] : null,
                        x.Inspection,
                        x.Stage,
                        x.TypeDocumentGji,
                        x.TypeRemoval,
                        x.DocumentDate,
                        x.DocumentNumber,
						x.State
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(serviceDocumentChildren);
            }
        }
    }
}