Ext.define('B4.aspects.permission.administration.Operator', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.operatoradminperm',

    permissions: [
        { name: 'Administration.Operator.Create', applyTo: 'b4addbutton', selector: '#operatorGrid' },
        { name: 'Administration.Operator.Edit', applyTo: 'b4savebutton', selector: '#operatorEditWindow' },
        {
            name: 'Administration.Operator.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#operatorGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
       
        { name: 'Administration.Operator.Fields.RisToken.Edit', applyTo: '[name=RisToken]', selector: '#operatorEditWindow' },
        {
            name: 'Administration.Operator.Fields.RisToken.View',
            applyTo: '[name=RisToken]',
            selector: '#operatorEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Administration.Operator.Fields.MobileApplicationAccessEnabled',
            applyTo: '[name=MobileApplicationAccessEnabled]',
            selector: '#operatorEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Administration.Operator.Fields.UserPhoto',
            applyTo: '[name=UserPhoto]',
            selector: '#operatorEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});