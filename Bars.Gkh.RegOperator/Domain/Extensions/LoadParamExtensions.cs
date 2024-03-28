namespace Bars.Gkh.RegOperator.Domain.Extensions
{
    using B4;

    public static class LoadParamExtensions
    {
        public static LoadParam ReplaceOrder(this LoadParam loadParam, string from, string to)
        {
            if (loadParam.Order.Length == 0)
            {
                return loadParam;
            }

            foreach (var orderField in loadParam.Order)
            {
                if (orderField.Name == from)
                {
                    orderField.Name = to;
                    break;
                }
            }

            return loadParam;
        }
    }
}