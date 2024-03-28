Ext.define('B4.aspects.permission.inspectionpreventiveaction.EditPanel', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.inspectionpreventiveactioneditpanelperm',

    permissions: [
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.Fields.InspectionNumber_View',
            applyTo: '[name=InspectionNumber]',
            selector: '#inspectionPreventiveActionEditPanel',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed, true);
            }
        },
        { name: 'GkhGji.Inspection.InspectionPreventiveAction.Fields.InspectionNumber_Edit', applyTo: '[name=InspectionNumber]', selector: '#inspectionPreventiveActionEditPanel' },
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.Fields.CheckDate_View',
            applyTo: '[name=CheckDate]',
            selector: '#inspectionPreventiveActionEditPanel',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed, true);
            }
        },
        { name: 'GkhGji.Inspection.InspectionPreventiveAction.Fields.CheckDate_Edit', applyTo: '[name=CheckDate]', selector: '#inspectionPreventiveActionEditPanel' },
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.Fields.RiskCategory_View',
            applyTo: '[name=RiskCategory]',
            selector: '#inspectionPreventiveActionEditPanel',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed, true);
            }
        },
        { name: 'GkhGji.Inspection.InspectionPreventiveAction.Fields.RiskCategory_Edit', applyTo: '[name=RiskCategory]', selector: '#inspectionPreventiveActionEditPanel' },
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.Fields.RiskCategoryStartDate_View',
            applyTo: '[name=RiskCategoryStartDate]',
            selector: '#inspectionPreventiveActionEditPanel',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed, true);
            }
        },
        { name: 'GkhGji.Inspection.InspectionPreventiveAction.Fields.RiskCategoryStartDate_Edit', applyTo: '[name=RiskCategoryStartDate]', selector: '#inspectionPreventiveActionEditPanel' },

        // Проверяемые дома
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.RealityObject.Create',
            applyTo: 'b4addbutton',
            selector: 'inspectionpreventiveactionrealityobjgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.RealityObject.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'inspectionpreventiveactionrealityobjgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        // Органы совместной проверки
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.JointInspection.Create',
            applyTo: 'b4addbutton',
            selector: 'inspectionpreventiveactionjointinspectiongrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Inspection.InspectionPreventiveAction.JointInspection.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'inspectionpreventiveactionjointinspectiongrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});