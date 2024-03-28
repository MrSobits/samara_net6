namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System;

    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public interface ITomskDisposalReportData
    {
        //о Приказу получить источник данных для сборки отчета
        DisposalReportData GetData(Bars.GkhGji.Entities.Disposal disposal);

        // по приказу получить значение Основания приказа
        string GetBaseSurvey(DisposalReportData reportData);
    }

    public class DisposalReportData
    {
        public Disposal Disposal { get; set; }
        public long AppealCitsId { get; set; }
        public string AppealCitsNumber { get; set; }
        public DateTime? AppealCitsDate { get; set; }
        public string AppealCitsDateStr { get; set; }
        public string AppealCitsCorrespondent { get; set; }
        public string AppealCitsTypeCorrespondent { get; set; }
        public string AppealCitsQuestion { get; set; }
        public int JurPersonPlanYear { get; set; }
        public string AppealSourceName { get; set; }
        public string AppealSourceNameGenetive { get; set; }
        public string HouseAndAdrress { get; set; }
        public TypeBase InspectionTypeBase { get; set; }
        public string InspectionTypeBaseName { get; set; }
        public string InspectionNumber { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string InspectionDateStr { get; set; }
    }
}
