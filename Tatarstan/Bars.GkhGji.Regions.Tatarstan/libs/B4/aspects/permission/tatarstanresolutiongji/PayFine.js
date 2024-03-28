Ext.define('B4.aspects.permission.tatarstanresolutiongji.PayFine', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanresolutiongjipayfineperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.PayFine.Create', applyTo: 'b4addbutton', selector: 'resolutionPayFineGrid' },
        { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.PayFine.Edit', applyTo: '#btnSaveResolutionPayFine', selector: 'resolutionPayFineGrid'},
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.PayFine.Delete', applyTo: 'b4deletecolumn', selector: 'resolutionPayFineGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});