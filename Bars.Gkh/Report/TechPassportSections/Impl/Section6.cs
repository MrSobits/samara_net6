namespace Bars.Gkh.Report.TechPassportSections
{
    public class Section6 : BaseTechPassportSectionReport
    {
        protected override void PrepareComponentIds()
        {
            //---6.1. Энергетический аудит---
            GenerateCellCodes("Form_6_1_1", 1, 2);
            GenerateCellCodes("Form_6_1_2", 1, 2);

            //---6.2. Температурные условия---
            GenerateCellCodes("Form_6_1", 1, 6, 4, 4);

            //---6.3. Энергопотребление здания---
            GenerateCellCodes("Form_6_2", 1, 10, 4, 5);

            //---6.4. Удельные расходы энергоносителей---
            GenerateCellCodes("Form_6_3", 1, 7, 4, 5);

            //---6.5. Характеристики максимального энергопотребления зданием---
            GenerateCellCodes("Form_6_4", 1, 20, 4, 5);

            //---6.6. Состояние приборного учета---
            GenerateCellCodes("Form_6_5", 1, 9, 3, 6);
        }
    }
}