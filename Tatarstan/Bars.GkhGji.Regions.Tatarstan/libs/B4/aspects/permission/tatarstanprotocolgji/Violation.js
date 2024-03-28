Ext.define('B4.aspects.permission.tatarstanprotocolgji.Violation', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanprotocolgjiviolationperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Violation.Create', applyTo: 'b4addbutton', selector: 'tatarstanprotocolgjiviolationgrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Violation.Delete', applyTo: 'b4deletecolumn', selector: 'tatarstanprotocolgjiviolationgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});