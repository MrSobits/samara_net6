namespace Bars.Gkh.Overhaul.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.Gkh.Overhaul.Properties;
    using Bars.Gkh.Report;
    using Gkh.Entities.CommonEstateObject;
    
    public class StructElementList : GkhBaseReport
    {
        public StructElementList()
            : base(new ReportTemplateBinary(Resources.StructElementList))
        {
        }

        public StructElementList(IReportTemplate reportTemplate) : base(reportTemplate)
        {
        }

        public override string CodeForm
        {
            get { return "StructElementList"; }
        }

        public override string Name
        {
            get { return "Список конструктивных элементов"; }
        }

        public override string Description
        {
            get { return "Список конструктивных элементов"; }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>();
        }

        public override string Id
        {
            get { return "StructElementList"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {

        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var els = Container.Resolve<IDomainService<StructuralElement>>().GetAll().ToList();
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
                i++;
            }
        }
    }
}