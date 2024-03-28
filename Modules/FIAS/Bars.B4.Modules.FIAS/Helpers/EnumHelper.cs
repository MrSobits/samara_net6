using Bars.B4.Modules.FIAS.Enums;

namespace Bars.B4.Modules.FIAS.Helpers
{
    public static class EnumHelper
    {
        public static string GetPrefix(FiasEstimateStatusEnum estimateType)
        {
            switch (estimateType)
            {
                case FiasEstimateStatusEnum.Building:
                    return "здание";
                case FiasEstimateStatusEnum.Garage:
                    return "гараж";
                case FiasEstimateStatusEnum.House:
                    return "д.";
                case FiasEstimateStatusEnum.HouseOwnership:
                    return "домовладение";
                case FiasEstimateStatusEnum.Mine:
                    return "шахта";
                case FiasEstimateStatusEnum.Ownership:
                    return "владение";
                default:
                    return "д.";
            }
        }
    }
}
