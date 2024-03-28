Ext.define('B4.aspects.permission.tatarstanprotocolgji.TatarstanProtocolGji', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanprotocolgjiperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Create', applyTo: 'b4addbutton', selector: 'tatarstanprotocolgjigrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Edit', applyTo: 'b4editcolumn', selector: 'tatarstanprotocolgjigrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Delete', applyTo: 'b4deletecolumn', selector: 'tatarstanprotocolgjigrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});