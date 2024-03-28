Ext.define('B4.aspects.permission.tatarstanresolutiongji.Eyewitness', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanresolutiongjieyewittnessperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Eyewitness.Create', applyTo: 'b4addbutton', selector: 'tatarstanprotocolgjieyewitnessgrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Eyewitness.Edit', applyTo: '[name=btnSave]', selector: 'tatarstanprotocolgjieyewitnessgrid'
        },
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Eyewitness.Delete', applyTo: 'b4deletecolumn', selector: 'tatarstanprotocolgjieyewitnessgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});