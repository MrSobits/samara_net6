using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Utils.Attributes;
using Bars.GkhGji.Entities;
using Bars.GkhCr.Entities;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict
{
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Тип справочника ГИС МЖФ РТ
    /// </summary>
    public enum DictCode
    {
        /// <summary>
        /// Нормативные документы
        /// </summary>
        [Display("Нормативные документы")]
        [Type(typeof(NormativeDoc))]
        NormativeDoc = 1,
        
        /// <summary>
        /// Муниципальные образования
        /// </summary>
        [Display("Муниципальные образования")]
        [Type(typeof(Municipality))]
        Municipality = 2,
        
        /// <summary>
        /// Виды проверок
        /// </summary>
        [Display("Виды проверок")]
        [Type(typeof(KindCheckGji))]
        KindCheck = 3,
        
        /// <summary>
        /// Инспекторы
        /// </summary>
        [Display("Инспекторы")]
        [Type(typeof(Inspector))]
        Inspector = 4,
        
        /// <summary>
        /// Предоставляемые документы
        /// </summary>
        [Display("Предоставляемые документы")]
        [Type(typeof(ProvidedDocGji))]
        ProvidedDoc = 5,
        
        /// <summary>
        /// Инспектируемые части
        /// </summary>
        [Display("Инспектируемые части")]
        [Type(typeof(InspectedPartGji))]
        InspectedPart = 6,
        
        /// <summary>
        /// Статьи закона
        /// </summary>
        [Display("Статьи закона")]
        [Type(typeof(ArticleLawGji))]
        ArticleLaw = 7,
        
        /// <summary>
        /// Периоды отопительного сезона
        /// </summary>
        [Display("Периоды отопительного сезона")]
        [Type(typeof(HeatSeasonPeriodGji))]
        HeatSeasonPeriod = 8,
        
        /// <summary>
        /// Типы исполнителей
        /// </summary>
        [Display("Типы исполнителей")]
        [Type(typeof(ExecutantDocGji))]
        ExecutantDoc = 9,
        
        /// <summary>
        /// Программы капитального ремонта
        /// </summary>
        [Display("Программы капитального ремонта")]
        [Type(typeof(ProgramCr))]
        ProgramCr = 10,
        
        /// <summary>
        /// Виды контроля
        /// </summary>
        [Display("Виды контроля")]
        [Type(typeof(ControlType))]
        ControlType = 11,
    }
}