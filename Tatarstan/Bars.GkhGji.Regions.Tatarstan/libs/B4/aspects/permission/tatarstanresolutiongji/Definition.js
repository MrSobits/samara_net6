Ext.define('B4.aspects.permission.tatarstanresolutiongji.Definition', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.tatarstanresolutiongjidefinitionperm',

    permissions: [
        { name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Definition.Create', applyTo: 'b4addbutton', selector: 'resolutionDefinitionGrid' },
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Definition.Edit', applyTo: 'b4editcolumn', selector: 'resolutionDefinitionGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TatarstanResolutionGji.Definition.Delete', applyTo: 'b4deletecolumn', selector: 'resolutionDefinitionGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});