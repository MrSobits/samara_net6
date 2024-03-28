Ext.define('B4.aspects.permission.tatarstanprotocolgji.Annex', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanprotocolgjianexperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Annex.Create', applyTo: 'b4addbutton', selector: '#protocolAnnexGrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Annex.Edit', applyTo: 'b4editcolumn', selector: '#protocolAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#protocolAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});