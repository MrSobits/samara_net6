namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;


    /// <summary>
    /// Правило создание документа 'Акт обследования' из документа 'Распоряжение' (по выбранным домам)
    /// </summary>
    public class StavrDisposalToActSurveyRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public virtual string CodeRegion
        {
            get { return "Tat"; }
        }

        public virtual string Id
        {
            get { return "StavrDisposalToActSurveyRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Акт обследования' из документа 'Распоряжение' (по выбранным домам) Ставрополь"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActSurvey"; }
        }

        public virtual string ResultName
        {
            get { return "Акт обследования"; }
        }

        public virtual TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Disposal; }
        }

        public virtual TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.ActSurvey; }
        }

        public virtual void SetParams(BaseParams baseParams)
        {
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            return new BaseDataResult();
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В Ставрополи нельзя формировать акт обследования от распоряжения");
        }
    }
}
