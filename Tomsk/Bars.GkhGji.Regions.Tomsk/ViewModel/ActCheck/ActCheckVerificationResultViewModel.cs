namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;
    using Gkh.Domain;

    public class ActCheckVerificationResultViewModel : BaseViewModel<ActCheckVerificationResult>
    {
        public IDomainService<ActCheckVerificationResult> ServiceActCheckVerificationResult { get; set; }

        public override IDataResult List(IDomainService<ActCheckVerificationResult> domainService, BaseParams baseParams)
        {
            var actCheckId = baseParams.Params.GetAsId("documentId");

            var existingValue = ServiceActCheckVerificationResult.GetAll()
                .Where(x => x.ActCheck.Id == actCheckId)
                .Select(x => (TypeVerificationResult?)x.TypeVerificationResult)
                .FirstOrDefault();

            var data = Enum.GetValues(typeof(TypeVerificationResult))
                .Cast<TypeVerificationResult>()
                .Select(x => new { Id = (int)x, Name = x.GetEnumMeta().Display, Active = existingValue == x })
                .ToList();

            return new ListDataResult(data.ToList(), data.Count);
        }
    }
}
    

