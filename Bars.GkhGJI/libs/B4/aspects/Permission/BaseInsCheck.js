Ext.define('B4.aspects.permission.BaseInsCheck', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.baseinscheckperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.Plan_Edit', applyTo: '#sflPlan', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.InsCheckDate_Edit', applyTo: '#dfInsCheckDate', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.InsNumber_Edit', applyTo: '#tfInspectionNumber', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.DateStart_Edit', applyTo: '#dfDateStart', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.Area_Edit', applyTo: '#insCheckEditPanelAreaNumberField', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.Reason_Edit', applyTo: '#tfReason', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.TypeFact_Edit', applyTo: '#cbTypeCheck', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.RealityObject_Edit', applyTo: '#insCheckRealityObjectsTrigerField', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.Inspectors_Edit', applyTo: '#insCheckInspectorsTrigerField', selector: '#baseInsCheckEditPanel' },
        { name: 'GkhGji.Inspection.BaseInsCheck.Field.ReasonErpChecking_View', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseInsCheckEditPanel' },

        { name: 'GkhGji.Inspection.BaseInsCheck.Field.ReasonErpChecking_Edit', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseInsCheckEditPanel' },
       {
           name: 'GkhGji.Inspection.BaseInsCheck.Field.ReasonErpChecking_View', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseInsCheckEditPanel',
           applyBy: function (component, allowed) {
               component.setVisible(allowed);
               component.setDisabled(!allowed);
           }
       }
    ]
});