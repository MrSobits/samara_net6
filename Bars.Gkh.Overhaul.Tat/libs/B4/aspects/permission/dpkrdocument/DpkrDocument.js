Ext.define('B4.aspects.permission.dpkrdocument.DpkrDocument', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.dpkrdocumentpermissions',

    permissions: [
        {
            name: 'Ovrhl.DpkrDocument.Create',
            applyTo: 'b4addbutton',
            selector: 'dpkrdocumentgrid'
        },
        {
            name: 'Ovrhl.DpkrDocument.Edit',
            applyTo: 'b4editcolumn',
            selector: 'dpkrdocumentgrid'
        },
        {
            name: 'Ovrhl.DpkrDocument.Edit',
            applyTo: 'b4savebutton',
            selector: 'dpkrdocumenteditwindow'
        },
        {
            name: 'Ovrhl.DpkrDocument.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'dpkrdocumentgrid'
        },
        {
            name: 'Ovrhl.DpkrDocument.DpkrDocumentImport.View',
            applyTo: 'button[itemId="ImportFileButton"]',
            selector: 'dpkrdocumenteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            },
            
        },
        {
            name: 'Ovrhl.DpkrDocument.RealityObject.Included.Create',
            applyTo: 'b4addbutton',
            selector: '#includedDpkrDocumentRealityObject',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            },
        },
        {
            name: 'Ovrhl.DpkrDocument.RealityObject.Included.Create',
            applyTo: 'b4deletecolumn',
            selector: '#includedDpkrDocumentRealityObject',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            },
        },
        {
            name: 'Ovrhl.DpkrDocument.RealityObject.Excluded.Create',
            applyTo: 'b4addbutton',
            selector: '#excludedDpkrDocumentRealityObject',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            },
        },
        {
            name: 'Ovrhl.DpkrDocument.RealityObject.Excluded.Create',
            applyTo: 'b4deletecolumn',
            selector: '#excludedDpkrDocumentRealityObject',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            },
        },
    ]
});