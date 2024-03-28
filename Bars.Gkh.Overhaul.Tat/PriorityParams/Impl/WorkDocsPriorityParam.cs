namespace Bars.Gkh.Overhaul.Tat.PriorityParams.Impl
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

        public TypeParam TypeParam
        {
            get { return TypeParam.Qualit; }
        }

        public Type EnumType
        {
            get
            {
                return typeof(TypePresence);
            }
        }

        public object GetValue(RealityObjectStructuralElementInProgrammStage3 obj)
        {
            return obj.RealityObject.ConfirmWorkDocs;
        }
    }
}