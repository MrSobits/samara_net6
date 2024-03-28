namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using Entities;

    public class BuildYearPriorityParam : IPriorityParams
    {
        public string Id 
        {
            get { return "BuildYear"; }
        }

        public string Name
        {
            get { return "Год постройки"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public  object GetValue(IStage3Entity obj)
        {
            return obj.RealityObject.BuildYear;
        }
    }
}