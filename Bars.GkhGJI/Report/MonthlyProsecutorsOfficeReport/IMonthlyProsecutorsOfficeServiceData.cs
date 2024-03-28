namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;

    // Данный класс служит сборкой для Отчет для прокуратуры (ежемесячный)
    // Внимание Данная сборка меняется в Томске 
    public interface IMonthlyProsecutorsOfficeServiceData
    {
        byte[] GetTemplate();

        MonthlyProsecutorsOfficeData GetData(DateTime reportDate, List<long> municipalityes);
    }

    public class MonthlyProsecutorsOfficeData
    {
        public long param1_1 { get; set; }
        public long param1_2 { get; set; }
        public long param2_1 { get; set; }
        public long param2_2 { get; set; }
        public long param3_1 { get; set; }
        public long param3_2 { get; set; }
        public long param3_1_1 { get; set; }
        public long param3_1_2 { get; set; }
        public long param4_1 { get; set; }
        public long param4_2 { get; set; }
        public long param5_1 { get; set; }
        public long param5_2 { get; set; }
        public long param6_1 { get; set; }
        public long param6_2 { get; set; }
        public long param7_1 { get; set; }
        public long param7_2 { get; set; }
        public long param8_1 { get; set; }
        public long param8_2 { get; set; }
        public long param9_1 { get; set; }
        public long param9_2 { get; set; }
        public long param10_1 { get; set; }
        public long param10_2 { get; set; }
        public long param11_1 { get; set; }
        public long param11_2 { get; set; }
    }
}
