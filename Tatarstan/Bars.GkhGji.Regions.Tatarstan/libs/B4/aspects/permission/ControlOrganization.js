Ext.define('B4.aspects.permission.ControlOrganization', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.controlorgperm',

    permissions: [
        { name: 'Gkh.Orgs.ControlOrganization.Create', applyTo: 'b4addbutton', selector: 'controlorganizationgrid' },
        {
            name: 'Gkh.Orgs.ControlOrganization.Edit', applyTo: 'b4editcolumn', selector: 'controlorganizationgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.Orgs.ControlOrganization.Delete', applyTo: 'b4deletecolumn', selector: 'controlorganizationgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});