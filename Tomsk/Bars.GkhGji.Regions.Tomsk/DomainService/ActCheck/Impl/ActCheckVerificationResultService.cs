namespace Bars.GkhGji.Regions.Tomsk.DomainService.ActCheck.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    public class ActCheckVerificationResultService : IActCheckVerificationResultService
    {
        public IDomainService<ActCheckVerificationResult> ServiceActVerificationResult { get; set; }

        public IDataResult AddActCheckVerificationResult(BaseParams baseParams)
        {
            var actCheckId = baseParams.Params.GetAs<long>("actId");
            var typeVerificationResult = baseParams.Params.GetAs<TypeVerificationResult>("selectedCode");
            

            if (actCheckId == 0)
            {
                return new BaseDataResult(false, "Нет акта");
                //throw new Exception("Нет акта");
            }

            var existingValue = ServiceActVerificationResult.GetAll().FirstOrDefault(x => x.ActCheck.Id == actCheckId);

            if (existingValue != null)
            {
                existingValue.TypeVerificationResult = typeVerificationResult;
            }
            else
            {
                existingValue = new ActCheckVerificationResult
                {
                    ActCheck = new ActCheck { Id = actCheckId },
                    TypeVerificationResult = typeVerificationResult
                };
            }

            ServiceActVerificationResult.Save(existingValue);

            return new BaseDataResult();
        }
    }
}
