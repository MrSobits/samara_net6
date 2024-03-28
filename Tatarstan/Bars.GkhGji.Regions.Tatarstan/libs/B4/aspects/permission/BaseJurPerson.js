Ext.define('B4.aspects.permission.BaseJurPerson', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.basejurpersonperm',

    permissions: [
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.Plan_Edit', applyTo: '#sflPlan', selector: '#baseJurPersonEditPanel', applyBy: this.setVisible },
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.DateStart_Edit', applyTo: '#dfDateStart', selector: '#baseJurPersonEditPanel' },
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.InsNumber_Edit', applyTo: '#tfInspectionNumber', selector: '#baseJurPersonEditPanel' },
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.TypeBaseJuralPerson_Edit', applyTo: '[name=InspectionBaseType]', selector: '#baseJurPersonEditPanel' },
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.TypeFact_Edit', applyTo: '#cbTypeFactInspection', selector: '#baseJurPersonEditPanel' },
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.Reason_Edit', applyTo: '#tfReason', selector: '#baseJurPersonEditPanel' },
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.TypeForm_Edit', applyTo: '#cbTypeForm', selector: '#baseJurPersonEditPanel' },
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.Inspectors_Edit', applyTo: '#trigfInspectors', selector: '#baseJurPersonEditPanel' },
        { name: 'GkhGji.Inspection.BaseJurPerson.Field.ZonalInspections_Edit', applyTo: '#trigfZonalInspections', selector: '#baseJurPersonEditPanel' },
        {
            name: 'GkhGji.Inspection.BaseJurPerson.Field.ReasonErpChecking_Edit', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseJurPersonEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.readOnly = !allowed;
                }
            }
        },
        {
            name: 'GkhGji.Inspection.BaseJurPerson.Field.ReasonErpChecking_View', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseJurPersonEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                    component.setDisabled(!allowed);
                }
            }
        },
        { name: 'GkhGji.Inspection.BaseJurPerson.Register.RealityObject.Create', applyTo: 'b4addbutton', selector: '#baseJurPersonRealityObjectGrid' },
        { name: 'GkhGji.Inspection.BaseJurPerson.Register.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: '#baseJurPersonRealityObjectGrid', applyBy: this.setVisible }
    ]
});