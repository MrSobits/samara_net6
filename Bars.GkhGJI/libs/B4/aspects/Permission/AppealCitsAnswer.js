Ext.define('B4.aspects.permission.AppealCitsAnswer', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.appealcitsanswerperm',

    permissions: [
        { name: 'GkhGji.AppealCitizensState.Answer.Create', applyTo: 'b4addbutton', selector: '#appealCitsAnswerGrid' },
        {
            name: 'GkhGji.AppealCitizensState.Answer.Delete', applyTo: 'b4deletecolumn', selector: '#appealCitsAnswerGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        { name: 'GkhGji.AppealCitizensState.Answer.Edit', applyTo: 'b4savebutton', selector: '#appealCitsAnswerEditWindow' },
        { 
            name: 'GkhGji.AppealCitizensState.Answer.Executor_View', 
            applyTo: '[name=Executor]', 
            selector: '#appealCitsAnswerEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            } 
        },
        {
            name: 'GkhGji.AppealCitizensState.Answer.Executor_Edit', 
            applyTo: '[name=Executor]', 
            selector: '#appealCitsAnswerEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setReadOnly(!allowed);
                }
            } 
        }
    ]
});