Ext.define('B4.aspects.permission.AppealCits', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.appealcitsperm',

    permissions: [
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
                this.setVisible(component, allowed);appealCitsEditWindow
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.Number_Edit', applyTo: '[name=Number]', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Field.Year_View', applyTo: '[name=Year]', selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.AppealCitizensState.Field.Year_Edit', applyTo: '[name=Year]', selector: '#appealCitsEditWindow' },
        //{
        //    name: 'GkhGji.AppealCitizensState.Field.DateFrom_Edit', applyTo: '[name=DateFrom]', selector: '#appealCitsEditWindow',
        //    applyBy: function (component, allowed) {
        //        debugger;
        //        if (component) {
        //            if (allowed) component.setDisabled(true);
        //            else component.setDisabled(false);
        //        }
        //    } },
        
      //  { name: 'GkhGji.AppealCitizensState.Edit', applyTo: 'b4savebutton', selector: '#appealCitsEditWindow' },
        { name: 'GkhGji.AppealCitizensState.Answer.Create', applyTo: 'b4addbutton', selector: '#appealCitsAnswerGrid' },
        {
            name: 'GkhGji.AppealCitizensState.Answer.Delete', applyTo: 'b4deletecolumn', selector: '#appealCitsAnswerGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        //Удаление рассмотрения обащений
        //{
        //    name: 'GkhGji.AppealCitizensState.Field.Consideration.Delete', applyTo: 'b4deletecolumn', selector: '#appealCitsExecutantGrid',
        //    applyBy: function (component, allowed) {
        //        var v = allowed;
        //        debugger;
        //        this.setVisible(component, allowed);
        //    }
        //},
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