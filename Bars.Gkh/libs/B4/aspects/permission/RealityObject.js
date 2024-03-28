Ext.define('B4.aspects.permission.RealityObject', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.realityobjperm',
    
    init: function() {
        this.permissions = [
            
            { name: 'Gkh.RealityObject.Create', applyTo: 'b4addbutton', selector: 'realityobjGrid' },
            {
                name: 'Gkh.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjGrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            },
            { name: 'Gkh.RealityObject.Edit', applyTo: 'b4savebutton', selector: 'realityobjEditPanel' },

            { name: 'Gkh.UpdateRetPreview_View', applyTo: 'button[action=UpdateRoTypes]', selector: 'realityobjGrid' },
            
            { name: 'Gkh.RealityObject.Edit', applyTo: 'b4editcolumn', selector: 'realityobjGrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        ];

        this.callParent(arguments);
    }
});