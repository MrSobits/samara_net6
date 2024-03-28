Ext.define('B4.aspects.permission.tatarstanprotocolgji.Eyewitness', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanprotocolgjieyewittnessperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Eyewitness.Create', applyTo: 'b4addbutton', selector: 'tatarstanprotocolgjieyewitnessgrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Eyewitness.Edit', applyTo: '[name=btnSave]', selector: 'tatarstanprotocolgjieyewitnessgrid'
        },
        {
            name: 'GkhGji.DocumentsGji.TatarstanProtocolGji.Eyewitness.Delete', applyTo: 'b4deletecolumn', selector: 'tatarstanprotocolgjieyewitnessgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});