Ext.define('B4.aspects.permission.AppealCits', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.appealcitsperm',

    permissions: [
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.Surety_Edit', applyTo: '#appealCitsSuretySelectField', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.SuretyResolve_Edit', applyTo: '[name =SuretyResolve]', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.SuretyDate_Edit', applyTo: '#dfSuretyDate', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.Executant_Edit', applyTo: '#appealCitsExecutantSelectField', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.Tester_Edit', applyTo: '#sflTester', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.ExecuteDate_Edit', applyTo: '#dfExecuteDate', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.ZonalInspection_Edit', applyTo: '#sflZonalInspection', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Consideration.ApprovalContragent_View', applyTo: '[name = ApprovalContragent]', selector: '#appealCitsEditWindow' },
        
        { name: 'GkhGji.AppealCitizensState.Field.TypeCorrespondent_View', applyTo: '#cbTypeCorrespondent', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.DocumentNumber_View', applyTo: '[name=DocumentNumber]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.DocumentNumber_Edit', applyTo: '[name=DocumentNumber]', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Number_View', applyTo: '[name=Number]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.Number_Edit', applyTo: '[name=Number]', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Year_View', applyTo: '[name=Year]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.Year_Edit', applyTo: '[name=Year]', selector: '#appealCitsEditWindow' },
        
        { name: 'GkhGji.AppealCitizensState.Edit', applyTo: 'b4savebutton', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Answer.Create', applyTo: 'b4addbutton', selector: '#appealCitsAnswerGrid' },
        {
            name: 'GkhGji.AppealCitizensState.Answer.Delete', applyTo: 'b4deletecolumn', selector: '#appealCitsAnswerGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.AppealCitizensState.Request.Create', applyTo: 'b4addbutton', selector: '#appealCitsRequestGrid' },
        {
            name: 'GkhGji.AppealCitizensState.Request.Delete', applyTo: 'b4deletecolumn', selector: '#appealCitsRequestGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.Request.View', applyTo: 'tab[text=Запросы]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
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