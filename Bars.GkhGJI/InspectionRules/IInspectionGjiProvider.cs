namespace Bars.GkhGji.InspectionRules
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Интерфейс для правил формирвоания документов, а также для формирования самих документов
    /// Либо из основания проверки, либо из другого документа ГЖИ
    /// </summary>
    public interface IInspectionGjiProvider
    {
        string CodeRegion { get; }

        /// <summary>
        /// Получение правил формирования дкоументов на основе ТипаДокументаГЖи
        /// </summary>
        List<IDocumentGjiRule> GetRules(DocumentGji document);

        /// <summary>
        /// Получение правил формирования дкоументов на основе ТипаПроверки
        /// </summary>
        List<IInspectionGjiRule> GetRules(InspectionGji inspection);

        /// <summary>
        /// Получение правил формирования дкоументов на сонове ТипаДокументаГЖи
        /// </summary>
        IDataResult GetRules(BaseParams baseParams);

        /// <summary>
        /// Метод формирвоания документа ГЖИ Либо по Основанию проверки либо по другому дкоументу ГЖИ 
        /// </summary>
        IDataResult CreateDocument(BaseParams baseParams);

        /// <summary>
        /// Метод создания дкоумента по Основанияю проверки
        /// </summary>
        IDataResult CreateDocument(InspectionGji inspection, TypeDocumentGji typeDocument);

    }
}
