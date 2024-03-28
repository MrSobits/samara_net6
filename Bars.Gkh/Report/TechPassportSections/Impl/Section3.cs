namespace Bars.Gkh.Report.TechPassportSections
{
    public class Section3 : BaseTechPassportSectionReport
    {
        protected override void PrepareComponentIds()
        {
            //---3.1. Отопление (теплоснобжение)---
            GenerateCellCodes("Form_3_1", 1, 1, 3, 3);
            GenerateCellCodes("Form_3_1_2", 1, 2, 3, 4);
            GenerateCellCodes("Form_3_1_3", 1, 21);

            //---3.2. Горячее водоснобжение (ГВС)---
            GenerateCellCodes("Form_3_2", 1, 1, 3, 3);
            GenerateCellCodes("Form_3_2_2", 1, 2, 3, 4);
            GenerateCellCodes("Form_3_2_3", 1, 12);

            //---3.3. Холодное водоснобжение (ХВС)---
            GenerateCellCodes("Form_3_2_CW", 1, 1, 3, 3);
            GenerateCellCodes("Form_3_2CW_2", 1, 1, 3, 4);
            GenerateCellCodes("Form_3_2CW_3", 1, 4);

            //---3.4. Водоотведение (канализация)---
            GenerateCellCodes("Form_3_3_Water", 1, 1, 3, 3);
            GenerateCellCodes("Form_3_3_Water_2", 1, 2);

            //---3.5. Электроснабжение---
            GenerateCellCodes("Form_3_3", 1, 1, 3, 3);
            GenerateCellCodes("Form_3_3_2", 1, 1, 3, 4);
            GenerateCellCodes("Form_3_3_3", 1, 16);

            //---3.6. Газоснабжение---
            GenerateCellCodes("Form_3_4", 1, 1, 3, 3);
            GenerateCellCodes("Form_3_4_3", 1, 1, 3, 4);
            GenerateCellCodes("Form_3_4_2", 1, 7);

            //---3.7. Вентиляция---
            GenerateCellCodes("Form_3_5", 1, 1, 3, 3);

            //---3.8. Водостоки---
            GenerateCellCodes("Form_3_6", 1, 1, 3, 3);

            //---3.9. Мусоропроводы---
            GenerateCellCodes("Form_3_7", 1, 1, 3, 3);
            GenerateCellCodes("Form_3_7_2", 1, 1, 3, 3);
            GenerateCellCodes("Form_3_7_3", 1, 6);
        }
    }
}