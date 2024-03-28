namespace Bars.Gkh.Report.TechPassportSections
{
    using System;
    using B4;
    using B4.Utils;
    using Gkh.Entities;

    public class Section1: BaseTechPassportSectionReport
    {
        protected override void PrepareComponentIds()
        {
            //---1. Общие сведения о жилом доме---
            GenerateCellCodes("Form_1", 1, 26);

            //---1.2. Характеристика жилых помещений и их заселения---
            GenerateCellCodes("Form_1_2", 1, 7, 3, 5);
            GenerateCellCodes("Form_1_2_2", 1, 6, 3, 6);
            GenerateCellCodes("Form_1_2_3", 1, 3);

            //---1.3. Характеристика нежилых помещений---
            GenerateCellCodes("Form_1_3", 1, 4);
            GenerateCellCodes("Form_1_3_2", 1, 5, 3, 4);
            GenerateCellCodes("Form_1_3_3", 1, 5);

            //---1.4. Эксплуатационные показатели общего имущества---
            GenerateCellCodes("Form_1_4", 1, 11, 4, 4);
        }
        
        protected override void AfterPlaceData()
        {
            var realtyObject = Container.Resolve<IDomainService<RealityObject>>().Load(RealtyObjectId);
            if (realtyObject == null) return;
            ReportParams.SimpleReportParams["Address"] = realtyObject.FiasAddress.AddressName;
            ReportParams.SimpleReportParams["TypeHouse"] = enumDislayText(realtyObject.TypeHouse);
            ReportParams.SimpleReportParams["ConditionHouse"] = enumDislayText(realtyObject.ConditionHouse);
            ReportParams.SimpleReportParams["DateCommissioning"] = realtyObject.DateCommissioning.HasValue ? realtyObject.DateCommissioning.Value.ToShortDateString() : string.Empty;
            ReportParams.SimpleReportParams["DateLastOverhaul"] = realtyObject.DateLastOverhaul.HasValue ? realtyObject.DateLastOverhaul.Value.ToShortDateString() : string.Empty;
        }

        private string enumDislayText(Enum enumValue)
        {
            var text = string.Empty;
            var memInfo = enumValue.GetType().GetMember(enumValue.ToString());
            if (memInfo.Length > 0)
            {
                var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                if (attributes.Length > 0)
                {
                    text = ((DisplayAttribute)attributes[0]).Value;
                }
            }

            return text;
        }
    }
}