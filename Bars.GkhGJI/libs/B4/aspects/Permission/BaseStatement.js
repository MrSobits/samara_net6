Ext.define('B4.aspects.permission.BaseStatement', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.basestatementperm',

    permissions: [
                /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        { name: 'GkhGji.Inspection.BaseStatement.Field.InspectionNumber_Edit', applyTo: '#tfInspectionNumber', selector: '#baseStatementEditPanel' },
        { name: 'GkhGji.Inspection.BaseStatement.Field.TypeJurPerson_Edit', applyTo: '#cbTypeJurPerson', selector: '#baseStatementEditPanel' },
        { name: 'GkhGji.Inspection.BaseStatement.Field.PersonInspection_Edit', applyTo: '#cbPersonInspection', selector: '#baseStatementEditPanel' },
        { name: 'GkhGji.Inspection.BaseStatement.Field.Contragent_Edit', applyTo: '#sfContragent', selector: '#baseStatementEditPanel' },
        {
            name: 'GkhGji.Inspection.BaseStatement.Field.ControlType_View',
            applyTo: 'b4enumcombo[name=ControlType]',
            selector: '#baseStatementEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
                component.setDisabled(!allowed);
            }
        }
        //{ name: 'GkhGji.Inspection.BaseStatement.Field.ReasonErpChecking_Edit', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseStatementEditPanel' },
        //{
        //    name: 'GkhGji.Inspection.BaseStatement.Field.ReasonErpChecking_View', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseStatementEditPanel',
        //    applyBy: function (component, allowed) {
        //        component.setVisible(allowed);
        //        component.setDisabled(!allowed);
        //    }
        //}
    ]
});