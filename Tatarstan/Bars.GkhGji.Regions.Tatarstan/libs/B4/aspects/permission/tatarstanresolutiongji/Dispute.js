Ext.define('B4.aspects.permission.tatarstanresolutiongji.Dispute', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanresolutiongjidisputeperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Dispute.Create', applyTo: 'b4addbutton', selector: 'resolutionDisputeGrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Dispute.Edit', applyTo: 'b4editcolumn', selector: 'resolutionDisputeGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Dispute.Delete', applyTo: 'b4deletecolumn', selector: 'resolutionDisputeGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});