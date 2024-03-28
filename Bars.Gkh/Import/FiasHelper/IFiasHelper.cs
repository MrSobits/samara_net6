namespace Bars.Gkh.Import.FiasHelper
{
    using Bars.B4.Modules.FIAS;

    public interface IFiasHelper
    {
        void Initialize();

        bool IncludeInBranch(string guid);

        bool HasValidStreetKladrCode(string streetKladrCode);

        bool FindInBranch(string branchGuid, string placeName, string streetName, ref string faultReason, out DynamicAddress address);

        bool FindInBranchByKladr(string branchGuid, string streetKladrCode, ref string faultReason, out DynamicAddress address);

        FiasAddress CreateFiasAddress(DynamicAddress address, string house, string letter, string housing, string building);
    }
}