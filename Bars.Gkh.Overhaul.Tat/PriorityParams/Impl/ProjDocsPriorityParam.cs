namespace Bars.Gkh.Overhaul.Tat.PriorityParams.Impl
{
    using System;
    using Bars.Gkh.Enums;
    using Entities;

    public class ProjDocsPriorityParam : IPriorityParams, IQualitPriorityParam
    {
        public string Id 
        {
            get { return "ProjDocs"; }
        }

        public string Name
        {
            get { return "Наличие проектной документации"; }
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
            return obj.RealityObject.ProjectDocs;
        }
    }
}