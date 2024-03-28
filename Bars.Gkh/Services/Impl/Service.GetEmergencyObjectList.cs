namespace Bars.Gkh.Services.Impl
{
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetEmergencyObjectList;

    public partial class Service
    {
        public GetEmergencyObjectListResponse GetEmergencyObjectList(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GetEmergencyObjectListResponse { Result = Result.DataNotFound };
            }

            var emergencyObjects = EmergencyObjectDomain.GetAll()
                .Where(x => x.RealityObject.FiasAddress.AddressGuid.Contains(id))
                .Select(x => new { x.Id, x.RealityObject.Address, x.RealityObject.FiasAddress.StreetName })
                .ToArray()
                .Select(x => new { x.Id, x.Address, StreetName = !x.StreetName.IsEmpty() && x.StreetName.Contains('.') ? x.StreetName.Remove(0, x.StreetName.IndexOf('.') + 1).TrimStart() : x.StreetName })
                .OrderBy(x => x.StreetName)
                .Select(x => new EmergencyObjectProxy { Id = x.Id, Address = x.Address })
                .ToArray();

            return new GetEmergencyObjectListResponse
            {
                EmergencyObjects = emergencyObjects,
                Result = emergencyObjects.Any() ? Result.NoErrors : Result.DataNotFound
            };
        }
    }
}