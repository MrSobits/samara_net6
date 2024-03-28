namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4.Modules.States;
    using Bars.Gkh.RegOperator.Enums;

    internal class RealityObjectBothProtocolProxy
    {
        public long Id { get; set; }

        public string ProtocolNumber { get; set; }

        public DateTime ProtocolDate { get; set; }

        public CoreDecisionType ProtocolType { get; set; }

        public State State { get; set; }
    }
}
