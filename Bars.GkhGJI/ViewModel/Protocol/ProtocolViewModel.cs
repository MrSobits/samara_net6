namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    // пустышка на тот случай если от этого класса наследовались в других регионах
    public class ProtocolViewModel : ProtocolViewModel<Protocol>
    {
        // Внимание!! Код override писать в Generic калссе
    }

    // Genric Класс для тго чтобы сущность Protocol расширять через subclass В других регионах
    public class ProtocolViewModel<T> : BaseViewModel<T>
        where T: Protocol
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var stageId = baseParams.Params.GetAs<long>("stageId");

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
                        x.DocumentNumber
                    })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}