namespace Bars.Gkh.DomainService.TechPassport.Impl
{
    using System;
    using Domain.TechPassport;
    using Enums.BasePassport;

    /// <summary>
    /// Описатель значений компонентов паспорта дома.
    ///  Т.к все значения всех элементов паспорта жилого дома хранятся в xml,
    ///  был задуман описатель который хранит сопоставления элементов и их коды
    /// </summary>
    public class TechPassportElementDescriptorService : ITechPassportElementDescriptorService
    {
        /// <summary>Код элемента 1:3</summary>
        public readonly string Cell_1_3 = "1:3";

        /// <summary>
        /// Получить метаданные компонента по типу компонента
        /// </summary>
        /// <param name="typeComponent">Тип компонента</param>
        /// <returns>Описатель компонента</returns>
        public TechPassportComponent GetComponent(TypeTechPassportComponent typeComponent)
        {
            // нижеследующий код похож костыль, 
            //  чем и я вляется ибо все описание паспорта находится в одном документе xml
            switch (typeComponent)
            {
                // Тип перекрытий
                case TypeTechPassportComponent.TypeFloorType:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_5_3",
                        CellCode = Cell_1_3
                    };

                // Тип системы отопления
                case TypeTechPassportComponent.TypeHeatingType:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_3_1",
                        CellCode = Cell_1_3
                    };

                // Тип системы горячего водоснабжения
                case TypeTechPassportComponent.TypeHotWaterType:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_3_2",
                        CellCode = Cell_1_3
                    };

                // Газоснабжение
                case TypeTechPassportComponent.TypeGas:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_3_4",
                        CellCode = Cell_1_3
                    };

                // Выходы на чердак
                case TypeTechPassportComponent.TypeRoofEntrance:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_5_6_2",
                        CellCode = "11:1"
                    };

                // Количество точек ввода системы отопления
                case TypeTechPassportComponent.TypeHeatingEntrance:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_3_1_3",
                        CellCode = "20:1"
                    };

                // Количество точек ввода ГВС
                case TypeTechPassportComponent.TypeHotWaterEntrance:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_3_2_3",
                        CellCode = "11:1"
                    };

                // Количество точек ввода ХВС
                case TypeTechPassportComponent.TypeColdWaterEntrance:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_3_2CW_3",
                        CellCode = "3:1"
                    };

                // Газоснабжение
                case TypeTechPassportComponent.TypeElectroEntrance:
                    return new TechPassportComponent
                    {
                        FormCode = "Form_3_3_3",
                        CellCode = "15:1"
                    };

                default:
                    throw new NotImplementedException("Компонент для данного элемента не определен");
            }
        }
    }
}
