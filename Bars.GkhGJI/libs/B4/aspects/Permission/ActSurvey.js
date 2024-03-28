Ext.define('B4.aspects.permission.ActSurvey', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.actsurveyperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */

        //поля панели редактирования ActSurvey
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#actSurveyEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActSurvey.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#actSurveyEditPanel' },

        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#actSurveyEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.ActSurvey.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#actSurveyEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: '#actSurveyEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.Inspectors_Edit', applyTo: '#trigfInspectors', selector: '#actSurveyEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.Flat_Edit', applyTo: '#tfFlat', selector: '#actSurveyEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.Area_Edit', applyTo: '#nfArea', selector: '#actSurveyEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.FactSurveyed_Edit', applyTo: '#cbFactSurveyed', selector: '#actSurveyEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.Reason_Edit', applyTo: '#tfReason', selector: '#actSurveyEditPanel' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.Description_Edit', applyTo: '#taDescription', selector: '#actSurveyEditPanel' },
        
        //ActSurveyInspectedPart
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.InspectedPart.Create', applyTo: 'b4addbutton', selector: '#actSurveyInspectedPartGrid' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.InspectedPart.Edit', applyTo: 'b4savebutton', selector: '#actSurveyInspectedPartEditWindow' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.InspectedPart.Delete', applyTo: 'b4deletecolumn', selector: '#actSurveyInspectedPartGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActSurveyPhoto
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Photo.Create', applyTo: 'b4addbutton', selector: '#actSurveyPhotoGrid' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Photo.Edit', applyTo: 'b4savebutton', selector: '#actSurveyPhotoEditWindow' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Photo.Delete', applyTo: 'b4deletecolumn', selector: '#actSurveyPhotoGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActSurveyAnnex
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#actSurveyAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#actSurveyAnnexEditWindow' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#actSurveyAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //ActSurveyOwners
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Owner.Create', applyTo: 'b4addbutton', selector: '#actSurveyOwnerGrid' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Owner.Edit', applyTo: '#actSurveyOwnerSaveButton', selector: '#actSurveyOwnerGrid' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Owner.Delete', applyTo: 'b4deletecolumn', selector: '#actSurveyOwnerGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});