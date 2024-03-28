namespace Bars.Gkh.Modules.Reforma
{
    using System;

    using Bars.B4;

    public interface IRealityObjectBothProtocolProvider
    {
        IDataResult GetData(long roId);
    }

    public class RealityObjectBothProtocolData
    {
        public string ProviderInn { get; set; }

        public string ProviderName { get; set; }

        public DateTime? CommonMeetingProtocolDate { get; set; }

        public string CommonMeetingProtocolNumber { get; set; }

        public decimal? PaymentAmount { get; set; }
    }
}