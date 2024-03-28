namespace Bars.Gkh.Report.TechPassportSections
{
    public class Section5 : BaseTechPassportSectionReport
    {
        protected override void PrepareComponentIds()
        {
            //---5.1. Фундаменты---
            GenerateCellCodes("Form_5_1", 1, 5, 3, 3);
            GenerateCellCodes("Form_5_1_1", 1, 3, 4, 4);

            //---5.2. Стены и перегородки---
            GenerateCellCodes("Form_5_2", 1, 1, 3, 3);
            GenerateCellCodes("Form_5_2_2", 1, 2, 4, 4);
            GenerateCellCodes("Form_5_2_3", 1, 1);

            //---5.3. Перекрытия---
            GenerateCellCodes("Form_5_3", 1, 1, 3, 3);
            GenerateCellCodes("Form_5_3_2", 1, 3, 4, 4);

            //---5.4. Полы---
            GenerateCellCodes("Form_5_4", 1, 8, 4, 4);

            //---5.5. Проемы---
            GenerateCellCodes("Form_5_5", 1, 14);

            //---5.6. Крыша, кровля---
            GenerateCellCodes("Form_5_6", 1, 1, 3, 3);
            GenerateCellCodes("Form_5_6_2", 1, 27);

            //---5.7. Отделка внутренняя---
            GenerateCellCodes("Form_5_7", 1, 11, 4, 4);

            //---5.8. Фасады---
            GenerateCellCodes("Form_5_8", 1, 39);

            //---5.9. Благоустройство---
            GenerateCellCodes("Form_5_9", 1, 7);
            GenerateCellCodes("Form_5_9_1", 1, 8);

            //---5.10. Стоимостные характеристики---
            GenerateCellCodes("Form_5_10", 1, 1, 4, 4);
        }
    }
}