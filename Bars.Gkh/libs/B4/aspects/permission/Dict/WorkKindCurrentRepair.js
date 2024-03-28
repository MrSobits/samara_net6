Ext.define('B4.aspects.permission.dict.WorkKindCurrentRepair', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.workkindcurrentrepairdictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.WorkKindCurrentRepair.Create', applyTo: 'b4addbutton', selector: '#workKindCurrentRepairGrid' },
        { name: 'Gkh.Dictionaries.WorkKindCurrentRepair.Edit', applyTo: 'b4savebutton', selector: '#workKindCurrentRepairEditWindow' },
        { name: 'Gkh.Dictionaries.WorkKindCurrentRepair.Delete', applyTo: 'b4deletecolumn', selector: '#workKindCurrentRepairGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});