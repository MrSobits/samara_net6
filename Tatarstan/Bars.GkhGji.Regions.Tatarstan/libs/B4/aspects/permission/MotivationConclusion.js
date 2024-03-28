Ext.define('B4.aspects.permission.MotivationConclusion', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.motivationconclusionperm',

    permissions: [
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Edit',
            applyTo: 'b4savebutton',
            selector: '#motivationconclusioneditpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Delete',
            applyTo: 'button[action=Delete]',
            selector: '#motivationconclusioneditpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Field.InspectionNumber_View',
            applyTo: 'textfield[name=InspectionNumber]',
            selector: '#motivationconclusioneditpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Field.InspectionNumber_Edit',
            applyTo: 'textfield[name=InspectionNumber]',
            selector: '#motivationconclusioneditpanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setReadOnly(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Field.Autor_View',
            applyTo: '[name=Autor]',
            selector: '#motivationconclusioneditpanel motivationconclusionrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Field.Autor_Edit',
            applyTo: '[name=Autor]',
            selector: '#motivationconclusioneditpanel motivationconclusionrequisitepanel'
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Field.Executant_View',
            applyTo: '[name=Executant]',
            selector: '#motivationconclusioneditpanel motivationconclusionrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Field.Executant_Edit',
            applyTo: '[name=Executant]',
            selector: '#motivationconclusioneditpanel motivationconclusionrequisitepanel'
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Field.Inspectors_View',
            applyTo: '[name=Inspectors]',
            selector: '#motivationconclusioneditpanel motivationconclusionrequisitepanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },


        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Annex.Create',
            applyTo: 'b4addbutton',
            selector: '#motivationconclusioneditpanel motivationconclusionannexgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.MotivationConclusion.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#motivationconclusioneditpanel motivationconclusionannexgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        }
    ]
});