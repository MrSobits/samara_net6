namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using System;
    using System.Collections.Generic;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    public class StructElementUsageParams : IPriorityParams
    {
        private readonly IWindsorContainer _container;
        public Dictionary<long, decimal> StructElementUsageValues { get; set; }

        private bool _initialized = false;

        #region IPriorityParams members
        public string Id { get { return "StructElementUsage"; } }
        public string Name { get { return "Эксплуатация КЭ (Срок эксплуатации/Межремонтный срок)"; } }
        public TypeParam TypeParam { get{ return TypeParam.Quant; } }
        #endregion

        public StructElementUsageParams(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetValue(IStage3Entity obj)
        {
            return StructElementUsageValues.Get(obj.Id).RoundDecimal(2);
        }
    }
}