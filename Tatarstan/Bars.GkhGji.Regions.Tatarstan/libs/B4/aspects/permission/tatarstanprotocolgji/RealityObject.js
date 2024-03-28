Ext.define('B4.aspects.permission.tatarstanprotocolgji.RealityObject', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanprotocolgjirealityobjperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.RealityObject.Create', applyTo: 'b4addbutton', selector: 'tatarstanprotocolgjirealityobjectgrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: 'tatarstanprotocolgjirealityobjectgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});