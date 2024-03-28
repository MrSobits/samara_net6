namespace Bars.GkhGji.Regions.Tatarstan.Extensions
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.ExplanationAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InspectionAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Extensions для удобной работы с <see cref="ActCheckAction"/> и его наследниками
    /// </summary>
    public static class ActCheckActionExtensions
    {
        /// <summary>
        /// Получить тип класса сущности по <see cref="ActCheckActionType"/>
        /// </summary>
        public static Type GetEntityClassType(this ActCheckActionType actCheckActionType)
        {
            var dict = new Dictionary<ActCheckActionType, Type>
            {
                { ActCheckActionType.Inspection, typeof(InspectionAction) },
                { ActCheckActionType.Survey, typeof(SurveyAction) },
                { ActCheckActionType.InstrumentalExamination, typeof(InstrExamAction) },
                { ActCheckActionType.RequestingDocuments, typeof(DocRequestAction) },
                { ActCheckActionType.GettingWrittenExplanations, typeof(ExplanationAction) }
            };

            return dict.Get(actCheckActionType);
        }
    }
}