Ext.define('B4.aspects.permission.BaseOMSU', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.baseomsuperm',
    init: function () {
        var me = this;

        me.permissions = [
            { name: 'GkhGji.Inspection.BaseOMSU.Field.Plan_Edit', applyTo: '#sflPlan', selector: '#baseOMSUEditPanel', applyBy: me.setVisible },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.UriRegistrationNumber_Edit', applyTo: '[name=UriRegistrationNumber]', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.UriRegistrationDate_Edit', applyTo: '[name=UriRegistrationDate]', selector: '#baseOMSUEditPanel', applyBy: me.setVisible },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.DateStart_Edit', applyTo: '#dfDateStart', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.InsNumber_Edit', applyTo: '#tfInspectionNumber', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.CountDays_Edit', applyTo: '#nfCountDays', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.TypeBaseOMSU_Edit', applyTo: '#cbTypeBaseJuralPerson', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.TypeFact_Edit', applyTo: '#cbTypeFactInspection', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.Reason_Edit', applyTo: '#tfReason', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.TypeForm_Edit', applyTo: '#cbTypeForm', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.Inspectors_Edit', applyTo: '#trigfInspectors', selector: '#baseOMSUEditPanel' },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.ZonalInspections_Edit', applyTo: '#trigfZonalInspections', selector: '#baseOMSUEditPanel' },
            {
                name: 'GkhGji.Inspection.BaseOMSU.Field.ControlType_View', applyTo: 'b4enumcombo[name=ControlType]', selector: '#baseOMSUEditPanel',
                applyBy: function (component, allowed) {
                    component.setVisible(allowed);
                    component.setDisabled(!allowed);
                }
            },
            { name: 'GkhGji.Inspection.BaseOMSU.Field.ReasonErpChecking_Edit', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseOMSUEditPanel' },
            {
                name: 'GkhGji.Inspection.BaseOMSU.Field.ReasonErpChecking_View', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseOMSUEditPanel',
                applyBy: function (component, allowed) {
                    component.setVisible(allowed);
                    component.setDisabled(!allowed);
                }
            },
          
        ];

        me.callParent(arguments);
    }
});