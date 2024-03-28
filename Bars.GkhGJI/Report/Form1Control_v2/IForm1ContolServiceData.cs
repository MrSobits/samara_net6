namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;

    // Данный класс служит сборкой для отчета Форма №1 - Контроль
    // Внимание Данная сборкаменяется в Томске 
    public interface IForm1ContolServiceData
    {
        byte[] GetTemplate();

        Form1ContolData GetData(DateTime dateStart, DateTime dateEnd, List<long> municipalityes);
    }

    public class Form1ContolData
    {
        public long cell1 { get; set; }
        public long cell3 { get; set; }
        public long cell4 { get; set; }
        public long cell5 { get; set; }
        public long cell7 { get; set; }
        public long cell9 { get; set; }
        public long cell10 { get; set; }
        public long cell11 { get; set; }
        public long cell12 { get; set; }
        public long cell13 { get; set; }
        public long cell14 { get; set; }
        public long cell15 { get; set; }

        public long cell16_1 { get; set; }
        public long cell16_2 { get; set; }

        public long cell19_1 { get; set; }
        public long cell19_2 { get; set; }

        public long cell21_1 { get; set; }
        public long cell21_2 { get; set; }

        public long cell23_1 { get; set; }
        public long cell23_2 { get; set; }

        public long cell24_1 { get; set; }
        public long cell24_2 { get; set; }

        public long cell25_1 { get; set; }
        public long cell25_2 { get; set; }

        public long cell29_1 { get; set; }
        public long cell29_2 { get; set; }

        public long cell33_1 { get; set; }
        public long cell33_2 { get; set; }

        public long cell35_1 { get; set; }
        public long cell35_2 { get; set; }

        public long cell37_1 { get; set; }
        public long cell37_2 { get; set; }

        public decimal cell39_1 { get; set; }
        public decimal cell39_2 { get; set; }

        public decimal cell41_1 { get; set; }
        public decimal cell41_2 { get; set; }

        public decimal cell42_1 { get; set; }
        public decimal cell42_2 { get; set; }

        public long cell46_1 { get; set; }
        public long cell46_2 { get; set; }

        public long cell47_1 { get; set; }
        public long cell47_2 { get; set; }

        public long cell48_1 { get; set; }
        public long cell48_2 { get; set; }

        public long cell50 { get; set; }
        public long cell51 { get; set; }
        public long cell52 { get; set; }
        public long cell53 { get; set; }
        public long cell54 { get; set; }
        public long cell55 { get; set; }
        public long cell56 { get; set; }
    }
}
