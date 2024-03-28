namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    // Пустышка если от этого класса наследвоались в регионах
    public class PrescriptionCancelViewModel : PrescriptionCancelViewModel<PrescriptionCancel>
    {
        //Внимание Все методы писать в Generic
    }

    // Generic класс чтобы лучше расширят ьв регионах через subclass
    public class PrescriptionCancelViewModel<T> : BaseViewModel<T>
        where T : PrescriptionCancel
    {
        public override IDataResult List(IDomainService<T> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domain.GetAll()
                .Where(x => x.Prescription.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.DateCancel,
                    x.IsCourt,
                    x.Reason,
                    IssuedCancel = x.IssuedCancel.Fio,
                    x.DateDecisionCourt,
                    DecisionMakingAuthority = x.DecisionMakingAuthority.Name,
                    x.TypeCancel
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}