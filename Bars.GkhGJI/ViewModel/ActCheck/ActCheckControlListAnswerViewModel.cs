namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель-представление для работы с периодами проверки
    /// </summary>
    public class ActCheckControlListAnswerViewModel : BaseViewModel<ActCheckControlListAnswer>
    {
        /// <summary>
        /// Метод формирующий список периодов проверки по заданному ID документа
        /// </summary>
        /// <param name="domainService">Домен-сервис периодов</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult List(IDomainService<ActCheckControlListAnswer> domainService, BaseParams baseParams)
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
                   ControlListQuestion = x.ControlListQuestion.ControlList.Name,
                   x.YesNoNotApplicable,
                   x.Description,
                   x.NpdName,
                   x.Question
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }        
    }
}