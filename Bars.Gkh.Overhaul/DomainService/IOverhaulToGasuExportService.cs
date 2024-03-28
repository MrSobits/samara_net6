namespace Bars.Gkh.Overhaul.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public interface IOverhaulToGasuExportService
    {
        List<OverhaulToGasuProxy> GetData(DateTime startDate, long programId);
    }

    [XmlType(AnonymousType = true)]
    public class OverhaulToGasuProxy
    {
        public OverhaulToGasuProxy()
        {
            LevelCalendar = (int)CalendarLevel.Year;
            Info = "4";
        }

        [XmlElement(ElementName = "N_CALLVL")]
        public int LevelCalendar { get; set; }

        [XmlElement(ElementName = "D_CALEN")]
        public string DateStart { get; set; }

        [XmlElement(ElementName = "ID_INFO")]
        public int FactOrPlan { get; set; }

        [XmlElement(ElementName = "ID_SINFO")]
        public string Info { get; set; }

        [XmlElement(ElementName = "ID_TER")]
        public string Okato { get; set; }

        [XmlElement(ElementName = "N_VAL")]
        public string Value { get; set; }

        [XmlElement(ElementName = "ID_POK")]
        public string IndexId { get; set; }

        [XmlElement(ElementName = "ID_UNITS")]
        public string UnitId { get; set; }
    }

    public enum CalendarLevel
    {
        Year = 1,
        Quarter = 2,
        Month = 3,
        Day = 4,
    }

    public enum FactOrPlan
    {
        Fact = 1,
        Plan = 2,
    }
}