Ext.define('B4.aspects.permission.DpkrDocument', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.dpkrdocumentperm',

    permissions: [
        { name: 'Ovrhl.DpkrDocument.Create', applyTo: 'b4addbutton', selector: 'dpkrdocumentgrid' },
        {
            name: 'Ovrhl.DpkrDocument.Edit', applyTo: 'b4editcolumn', selector: 'dpkrdocumentgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Ovrhl.DpkrDocument.Delete', applyTo: 'b4deletecolumn', selector: 'dpkrdocumentgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        // Версии ДПКР
        {
            name: 'Ovrhl.DpkrDocument.ProgramVersion.Create', applyTo: 'b4addbutton', selector: 'dpkrdocumentversiongrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Ovrhl.DpkrDocument.ProgramVersion.Delete', applyTo: 'b4deletecolumn', selector: 'dpkrdocumentversiongrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Ovrhl.DpkrDocument.ProgramVersion.AddRealityObjects', applyTo: '#btnAddRealityObjects', selector: 'dpkrdocumentversiongrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});