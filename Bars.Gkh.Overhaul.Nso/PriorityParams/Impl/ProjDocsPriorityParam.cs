namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
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
            return obj.RealityObject.ProjectDocs;
        }
    }
}