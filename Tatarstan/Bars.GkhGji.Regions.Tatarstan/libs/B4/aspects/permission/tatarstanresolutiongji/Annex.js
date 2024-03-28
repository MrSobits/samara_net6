Ext.define('B4.aspects.permission.tatarstanresolutiongji.Annex', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanresolutiongjianexperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Annex.Create', applyTo: 'b4addbutton', selector: '#resolutionAnnexGrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Annex.Edit', applyTo: 'b4editcolumn', selector: '#resolutionAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#resolutionAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});