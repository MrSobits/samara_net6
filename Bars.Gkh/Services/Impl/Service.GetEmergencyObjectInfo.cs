namespace Bars.Gkh.Services.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.EmergencyObjectInfo;

    public partial class Service
    {
        public IDomainService<EmergencyObject> EmergencyObjectDomain { get; set; }
        
        public GetEmergencyObjectInfoResponse GetEmergencyObjectInfo(string id)
        {
            var emergencyObjectInfo = this.GetEmergencyObjInfo(id.ToInt());

            return new GetEmergencyObjectInfoResponse
            {
                EmergencyObjectInfo = emergencyObjectInfo,
                Result = emergencyObjectInfo.Any() ? Result.NoErrors : Result.DataNotFound
            };
        }

        private EmergencyObjectInfoProxy[] GetEmergencyObjInfo(int id)
        {
            return EmergencyObjectDomain.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address,
                    x.CadastralNumber,
                    x.DemolitionDate,
                    x.IsRepairExpedient,
                    x.LandArea,
                    x.ResettlementFlatAmount,
                    x.ResettlementFlatArea,
                    ResettlementProgram = x.ResettlementProgram.Name,
                    x.ActualInfoDate
                })
                .AsEnumerable()
                .Select(x => new EmergencyObjectInfoProxy
                {
                    Id = x.Id,
                    Address = x.Address.ToStr(),
                    CadastralNumber = x.CadastralNumber.ToStr(),
                    DemolitionDate = x.DemolitionDate.HasValue ? x.DemolitionDate.Value.ToString("dd.MM.yyyy") : string.Empty,
                    IsRepairExpedient = x.IsRepairExpedient.ToStr(),
                    LandArea = x.LandArea.HasValue ? x.LandArea.Value.ToStr() : string.Empty,
                    ResettlementFlatAmount = x.ResettlementFlatAmount.HasValue ? x.ResettlementFlatAmount.Value.ToStr() : string.Empty,
                    ResettlementFlatArea = x.ResettlementFlatArea.HasValue ? x.ResettlementFlatArea.Value.ToStr() : string.Empty,
                    ResettlementProgram = x.ResettlementProgram.ToStr(),
                    ActualityDate = x.ActualInfoDate.HasValue ? x.ActualInfoDate.Value.ToString("dd.MM.yyyy") : string.Empty,
                })
                .ToArray();
        }
    }
}