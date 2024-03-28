namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Imports
{
    using System;
    using Gkh.Entities;

    public sealed class DataRecord
    {
        public DataRecord(int rowNumber)
        {
            RowNumber = rowNumber;
        }

        public RealityObject Robject { get; set; }

        public bool IsValidRecord { get; set; }

        public int RowNumber { get; private set; }

        public string AccountNumberExternalSystems { get; set; }

        public decimal Tariff { get; set; }

        public decimal AreaShare { get; set; }

        public decimal Area { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public string Municipality { get; set; }

        public string Settlement { get; set; }

        public string TypeSettlement { get; set; }

        public string Street { get; set; }

        public string TypeStreet { get; set; }

        public string House { get; set; }

        public string Letter { get; set; }

        public string Housing { get; set; }

        public string Building { get; set; }

        public string RoomNum { get; set; }

        public string AccountState { get; set; }

        public string Period { get; set; }

        public decimal SaldoIn { get; set; }

        public decimal SaldoOut { get; set; }

        public string TypeService { get; set; }

        public decimal Charged { get; set; }

        public decimal Paid { get; set; }

        public decimal Recalc { get; set; }
    }
}