namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System;
    using System.Linq;
    using B4;
    using B4.Modules.Reports;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Utils;
    using Properties;
    using Overhaul.Entities;

    public class StructElementListNso : Overhaul.Reports.StructElementList
    {
        public StructElementListNso(): base(new ReportTemplateBinary(Resources.StructElementList))
        {
        }

        public override string Id
        {
            get { return "StructElementListNso"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var els = Container.Resolve<IDomainService<StructuralElement>>().GetAll().ToList();

            var works = Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                .AsEnumerable()
                .GroupBy(x => x.StructuralElement.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Job.Work.Name));
            
            var i = 1;
            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            foreach (var structuralElement in els)
            {
                sect.ДобавитьСтроку();

                sect["Num"] = i;
                sect["CeoName"] = structuralElement.Group.CommonEstateObject.Name;
                sect["CeoGroupType"] = structuralElement.Group.CommonEstateObject.GroupType.Name;
                sect["MatchHC"] = structuralElement.Group.CommonEstateObject.IsMatchHc ? "Да" : "Нет";
                sect["IncludedInPrg"] = structuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm ? "Да" : "Нет";
                sect["IsIngNetwork"] = structuralElement.Group.CommonEstateObject.IsEngineeringNetwork ? "Да" : "Нет";
                sect["MultipleObj"] = structuralElement.Group.CommonEstateObject.MultipleObject ? "Да" : "Нет";
                sect["GroupName"] = structuralElement.Group.Name;
                sect["Required"] = structuralElement.Group.Required ? "Да" : "Нет"; 
                sect["Formula"] = structuralElement.Group.Formula;
                sect["StructEl"] = structuralElement.Name;
                sect["StructElCode"] = structuralElement.Code;
                sect["UnitMeasure"] = structuralElement.UnitMeasure.Name;
                sect["LifeTime"] = structuralElement.LifeTime;
                sect["NormDoc"] = structuralElement.NormativeDoc != null ? structuralElement.NormativeDoc.Name : string.Empty;
                sect["Weight"] = structuralElement.Group.CommonEstateObject.Weight;
                sect["Works"] = works.ContainsKey(structuralElement.Id) ? works[structuralElement.Id].AggregateWithSeparator(", ") : "";
                i++;
            }
        }
    }
}