Ext.define('B4.aspects.permission.rapidresponsesystem.AppealPermission', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',

    alias: 'widget.appealperm',

    permissions: [
        { name: 'CitizenAppealModule.RapidResponseSystem.Filter', applyTo: '#mainFilter', selector: '#appealFilterPanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        { name: 'CitizenAppealModule.RapidResponseSystem.View', applyTo: 'b4editcolumn', selector: 'rapidResponseSystemAppealGrid'},
        { name: 'CitizenAppealModule.RapidResponseSystem.Edit', applyTo: 'b4savebutton', selector: '#soprAppealEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }

                component.allowed = allowed;
            }
        },
        { name: 'CitizenAppealModule.RapidResponseSystem.Fields.ResponseDate_Edit', applyTo: '[name=ResponseDate]', selector: '#appealResponsePanel', applyBy: function(component, allowed){component.setReadOnly(!allowed);}},
        { name: 'CitizenAppealModule.RapidResponseSystem.Fields.Theme_Edit', applyTo: '[name=Theme]', selector: '#appealResponsePanel', applyBy: function(component, allowed){component.setReadOnly(!allowed);}},
        { name: 'CitizenAppealModule.RapidResponseSystem.Fields.Response_Edit', applyTo: '[name=Response]', selector: '#appealResponsePanel', applyBy: function(component, allowed){component.setReadOnly(!allowed);}},
        { name: 'CitizenAppealModule.RapidResponseSystem.Fields.CarriedOutWork_Edit', applyTo: '[name=CarriedOutWork]', selector: '#appealResponsePanel', applyBy: function(component, allowed){component.setReadOnly(!allowed);}},
        { name: 'CitizenAppealModule.RapidResponseSystem.Fields.ResponseFile_Edit', applyTo: '[name=ResponseFile]', selector: '#appealResponsePanel', applyBy: function(component, allowed){component.setReadOnly(!allowed);}},
    ]
});