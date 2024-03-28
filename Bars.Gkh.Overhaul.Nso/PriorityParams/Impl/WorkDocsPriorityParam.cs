namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using System;
    using Bars.Gkh.Enums;
    using Entities;

    public class WorkDocsPriorityParam : IPriorityParams, IQualitPriorityParam
    {
        public string Id
        {
            get { return "WorkDocs"; }
        }

        public string Name
        {
            get { return "Документы по гос. кадастровому учету земельного участка"; }
        }

        public Type EnumType
        {
            get
            {
                return typeof(TypePresence);
            }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Qualit; }
        }

        public object GetValue(IStage3Entity obj)
        {
            return obj.RealityObject.ConfirmWorkDocs;
        }
    }
}