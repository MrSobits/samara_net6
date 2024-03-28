namespace Bars.Gkh.RegOperator.Report.PersonalAccountChargeReports
{
    using System.Collections.Generic;

    public interface IChargeReportData
    {
        /// <summary>
        /// получаем список начислений по 1 или множеству выбранных Районов + период
        /// </summary>
        List<RaionChargeRecord> GetRaionChargeRecords(long[] mrIds, long periodId);

        /// <summary>
        /// получаем список начислений по 1 району + 1 или множеству выбранных МО + период
        /// </summary>
        List<MunicipalityChargeRecord> GetMunicipalityChargeRecords(long mrId, long[] moIds, long periodId);

        /// <summary>
        /// получаем список начислений по 1 МО + 1 или множеству выбранных МКД + период
        /// </summary>
        List<RealityObjectChargeRecord> GetRealityObjectChargeRecords(long moId, long[] roIds, long periodId);

        /// <summary>
        /// получаем список начислений по 1 МКД + 1 или множеству выбранных сетов + период
        /// </summary>
        List<AccountChargeRecord> GetOwnerChargeRecords(long roId, long[] accountIds, long periodId);
    }

    // Запись по начислениям для Района
    public class RaionChargeRecord
    {
        public long RaionId { get; set; }

        public string RaionName { get; set; }

        public decimal MinSize { get; set; }

        public decimal OverMinSize { get; set; }

        public decimal Penalty { get; set; }

        public decimal Total { get; set; }
    }

    // Запись по начислениям для МО
    public class MunicipalityChargeRecord
    {
        public long MunicipalityId { get; set; }

        public string MunicipalityName { get; set; }

        public decimal MinSize { get; set; }

        public decimal OverMinSize { get; set; }

        public decimal Penalty { get; set; }

        public decimal Total { get; set; }
    }

    // Запись по начислениям для Дома
    public class RealityObjectChargeRecord
    {
        public long RealityObjectId { get; set; }

        public string Address { get; set; }

        public decimal MinSize { get; set; }

        public decimal OverMinSize { get; set; }

        public decimal Penalty { get; set; }

        public decimal Total { get; set; }
    }

    // Запись по начислениям собственников
    public class AccountChargeRecord
    {
        public long AccountId { get; set; }

        public long RaionId { get; set; }

        public string RaionName { get; set; }

        public long MunicipalityId { get; set; }

        public string MunicipalityName { get; set; }

        public long RealityObjectId { get; set; }
        
        public string Address { get; set; }
        
        public string Room { get; set; }

        public string Owner { get; set; }

        public decimal MinSize { get; set; }

        public decimal OverMinSize { get; set; }

        public decimal Penalty { get; set; }

        public decimal Total { get; set; }
    }
}