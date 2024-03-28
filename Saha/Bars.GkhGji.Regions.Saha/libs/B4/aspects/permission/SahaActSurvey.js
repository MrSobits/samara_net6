Ext.define('B4.aspects.permission.SahaActSurvey', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.sahaactsurveyperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */

        //поля панели редактирования ActSurvey
        //DateOf
        {
            name: 'GkhGji.DocumentsGji.ActSurvey.Field.DateOf_View', applyTo: '[name=DateOf]', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.DateOf_Edit', applyTo: '[name=DateOf]', selector: '#actSurveyEditPanel' },
        
        //TimeStart
        {
            name: 'GkhGji.DocumentsGji.ActSurvey.Field.TimeStart_Edit', applyTo: '[name=TimeStart]', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.TimeStart_Edit', applyTo: '[name=TimeStart]', selector: '#actSurveyEditPanel' },

        //TimeEnd
        {
            name: 'GkhGji.DocumentsGji.ActSurvey.Field.TimeEnd_View', applyTo: '[name=TimeEnd]', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.TimeEnd_Edit', applyTo: '[name=TimeEnd]', selector: '#actSurveyEditPanel' },

        //ConclusionIssued
        {
            name: 'GkhGji.DocumentsGji.ActSurvey.Field.ConclusionIssued_View', applyTo: '[name=ConclusionIssued]', selector: '#actSurveyEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Field.ConclusionIssued_Edit', applyTo: '[name=ConclusionIssued]', selector: '#actSurveyEditPanel' },


        //ActSurveyConclusion
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Conclusion.Create', applyTo: 'b4addbutton', selector: 'actSurveyConclusionGrid' },
        { name: 'GkhGji.DocumentsGji.ActSurvey.Register.Conclusion.Edit', applyTo: 'b4savebutton', selector: 'actSurveyConclusionEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.ActSurvey.Register.Conclusion.Delete', applyTo: 'b4deletecolumn', selector: 'actSurveyConclusionEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});