Ext.define('B4.aspects.permission.Disposal', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.disposalperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        { name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        { name: 'GkhGji.DocumentsGji.Disposal.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#disposalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.Disposal.Field.ObjectVisitStart_Edit', applyTo: '#dfObjectVisitStart', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.ObjectVisitStart_View', applyTo: '#dfObjectVisitStart', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        { name: 'GkhGji.DocumentsGji.Disposal.Field.ObjectVisitEnd_Edit', applyTo: '#dfObjectVisitEnd', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.ObjectVisitEnd_View', applyTo: '#dfObjectVisitEnd', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        { name: 'GkhGji.DocumentsGji.Disposal.Field.OutInspector_Edit', applyTo: '#cbOutInspector', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.OutInspector_View', applyTo: '#cbOutInspector', selector: '#disposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.ResponsibleExecution_Edit', applyTo: '#sflResponsibleExecution', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.DateStart_Edit', applyTo: '#dfDateStart', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.DateEnd_Edit', applyTo: '#dfDateEnd', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.TypeAgreementProsecutor_Edit', applyTo: '#cbTypeAgreementProsecutor', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.TypeAgreementResult_Edit', applyTo: '#cbTypeAgreementResult', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.IssuedDisposal_Edit', applyTo: '#sfIssuredDisposal', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.Inspectors_Edit', applyTo: '#trigFInspectors', selector: '#disposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.Disposal.Field.KindCheck_Edit', applyTo: '#cbTypeCheck', selector: '#disposalEditPanel' },
        
        //DisposalTypeSurvey
        { name: 'GkhGji.DocumentsGji.Disposal.Register.TypeSurvey.Create', applyTo: 'b4addbutton', selector: '#disposalTypeSurveyGrid' },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.TypeSurvey.Delete', applyTo: 'b4deletecolumn', selector: '#disposalTypeSurveyGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //DisposalAnnex
        { name: 'GkhGji.DocumentsGji.Disposal.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#disposalAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#disposalAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#disposalAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        //DisposalControlMeasures
        { name: 'GkhGji.DocumentsGji.Disposal.Register.ControlMeasures.Create', applyTo: 'b4addbutton', selector: 'disposalControlMeasuresGrid' },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.ControlMeasures.Delete', applyTo: 'b4deletecolumn', selector: 'disposalcontrolmeasuresgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //DisposalExpert
        { name: 'GkhGji.DocumentsGji.Disposal.Register.Expert.Create', applyTo: 'b4addbutton', selector: '#disposalExpertGrid' },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.Expert.Delete', applyTo: 'b4deletecolumn', selector: '#disposalExpertGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        //DisposalProvidedDocument
        { name: 'GkhGji.DocumentsGji.Disposal.Register.ProvidedDoc.Create', applyTo: 'b4addbutton', selector: '#disposalProvidedDocGrid' },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.ProvidedDoc.Delete', applyTo: 'b4deletecolumn', selector: '#disposalProvidedDocGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});