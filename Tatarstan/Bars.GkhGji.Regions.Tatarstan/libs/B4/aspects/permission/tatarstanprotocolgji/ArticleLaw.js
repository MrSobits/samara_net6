Ext.define('B4.aspects.permission.tatarstanprotocolgji.ArticleLaw', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanprotocolgjiarticlelawperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.ArticleLaw.Create', applyTo: 'b4addbutton', selector: 'tatarstanprotocolgjiarticlelawgrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.ArticleLaw.Delete', applyTo: 'b4deletecolumn', selector: 'tatarstanprotocolgjiarticlelawgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});