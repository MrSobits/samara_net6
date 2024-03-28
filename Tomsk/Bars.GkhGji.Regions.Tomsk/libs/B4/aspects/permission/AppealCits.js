Ext.define('B4.aspects.permission.AppealCits', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.appealcitsperm',

    permissions: [
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.Executant_Edit', applyTo: '#appealCitsExecutantSelectField', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.Tester_Edit', applyTo: '#sflTester', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.ExecuteDate_Edit', applyTo: '#dfExecuteDate', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.ZonalInspection_Edit', applyTo: '#sflZonalInspection', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.TypeCorrespondent_View', applyTo: '#cbTypeCorrespondent', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.DocumentNumber_View', applyTo: '[name=NumberGji]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.DocumentNumber_Edit', applyTo: '[name=NumberGji]', selector: '#appealCitsEditWindow' },
        {
            name: 'GkhGji.AppealCitizensState.Field.SpecialControl_View', applyTo: '[name=SpecialControl]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.SpecialControl_Edit', applyTo: '[name=SpecialControl]', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Year_View', applyTo: '[name=Year]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.Field.Year_Edit', applyTo: '[name=Year]', selector: '#appealCitsEditWindow' },
        {name: 'GkhGji.AppealCitizensState.Field.SocialStatus_View', applyTo: '#sfSocialStatus', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.AppealCitizensState.Edit', applyTo: 'b4savebutton', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Answer.Create', applyTo: 'b4addbutton', selector: '#appealCitsAnswerGrid' },
        {
            name: 'GkhGji.AppealCitizensState.Answer.Delete', applyTo: 'b4deletecolumn', selector: '#appealCitsAnswerGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Request.Create', applyTo: 'b4addbutton', selector: '#appealCitsRequestGrid' },
        {
            name: 'GkhGji.AppealCitizensState.Request.Delete', applyTo: 'b4deletecolumn', selector: '#appealCitsRequestGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Consideration.Executants.Create', applyTo: 'b4addbutton', selector: '#appealCitsEditWindow appealcitsexecutantgrid' },
        { name: 'GkhGji.AppealCitizensState.Consideration.Executants.Edit', applyTo: 'b4savebutton', selector: '#appealCitsExecutantEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Consideration.Executants.Edit', applyTo: '#btnRedirect', selector: '#appealCitsExecutantEditWindow' },
        {
            name: 'GkhGji.AppealCitizensState.Consideration.Executants.Delete', applyTo: 'b4deletecolumn', selector: '#appealCitsEditWindow appealcitsexecutantgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.SuretyResolve_Edit', applyTo: '[name = SuretyResolve]', selector: '#appealCitsEditWindow' },
        {
            name: 'GkhGji.AppealCitizensState.Field.Consideration.SuretyResolve_View', applyTo: '[name = SuretyResolve]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.Field.Department_View', applyTo: '[name=ZonalInspection]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.Department_Edit', applyTo: '[name=ZonalInspection]', selector: '#appealCitsEditWindow' },
    ]
}); 