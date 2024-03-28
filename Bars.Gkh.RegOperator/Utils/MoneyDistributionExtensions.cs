namespace Bars.Gkh.RegOperator.Utils
{
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Distribution;

    public static class MoneyDistributionExtensions
    {
        public static string GetPermissionId(this IDistribution impl, string ns)
        {
            return "{0}.{1}".FormatUsing(ns, impl.Code);
        }
    }
}