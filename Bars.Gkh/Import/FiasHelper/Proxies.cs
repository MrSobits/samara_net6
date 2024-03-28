namespace Bars.Gkh.Import.FiasHelper
{
    using System.Collections.Generic;

    using Bars.B4.Modules.FIAS;

    public class DynamicAddress : DinamicAddress
    {
        public string PlaceCode { get; set; }
    }

    public class FiasProxy
    {
        public string AoGuid;

        public string MirrorGuid;

        public string ParentGuid;

        public FiasActualStatusEnum ActStatus;

        public List<FiasProxy> Childen;

        public string CodeRecord;
    }

    public class FiasFullProxy
    {
        public string AoGuid;

        public string MirrorGuid;

        public string ParentGuid;

        public FiasLevelEnum AOLevel;

        public FiasActualStatusEnum ActStatus;

        public string OffName;

        public string ShortName;

        public string CodeRecord;
    }

    public class FiasProxyWithKladr
    {
        public string AoGuid;

        public string ParentGuid;

        public string OffName;

        public string ShortName;

        public string KladrCode;

        public string CodeRecord;

        public string PostalCode;

        public FiasLevelEnum AOLevel;

        public int? KladrCurrStatus;
    }

    public class ReverseFiasProxy
    {
        public string AoGuid;

        public ReverseFiasProxy Parent;
    }

    public enum ErrorType
    {
        NoError,
        MultipleExistance,
        Absence
    }

    public class SearchResult
    {
        public DynamicAddress Address;

        public ErrorType Error;
    }
}