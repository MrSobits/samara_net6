Ext.define('B4.aspects.permission.realityobj.Intercom', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjintercompermissionaspect',

    permissions: [
        {
            name: 'Gkh.RealityObject.Register.Intercom.Create', applyTo: 'b4addbutton', selector: 'intercomgrid' ,
            applyBy: function (component, allowed) {
                if (allowed) {
                    var grid = component.up('intercomgrid');
                    if (grid.getStore().data.items.length == 0) {
                        component.setDisabled(false);
                    } else {
                        component.setDisabled(true);
                    }
                } else {
                    component.setDisabled(true);
                }
            }
        },
        { name: 'Gkh.RealityObject.Register.Intercom.Edit', applyTo: 'b4savebutton', selector: 'intercomwindow' },
        {
            name: 'Gkh.RealityObject.Register.Intercom.Delete', applyTo: 'b4deletecolumn', selector: 'intercomgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});