namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using Entities;

    public class YearCommissioningPriorityParam : IPriorityParams
    {
        public string Id
        {
            get { return "YearCommissioning"; }
        }

        public string Name
        {
            get { return "Год ввода в эксплуатацию МКД"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public object GetValue(IStage3Entity obj)
        {
            return obj.RealityObject.DateCommissioning.HasValue
                ? obj.RealityObject.DateCommissioning.Value.Year
                : obj.RealityObject.BuildYear.HasValue
                    ? obj.RealityObject.BuildYear.Value
                    : 0;
        }
    }
}