Ext.define('B4.aspects.permission.BaseStatement', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.basestatementperm',
    applyByPostfix: true,
    permissions: [
        { name: 'GkhGji.Inspection.BaseStatement.Field.InspectionNumber_Edit', applyTo: '#tfInspectionNumber', selector: '#baseStatementEditPanel' },
        { name: 'GkhGji.Inspection.BaseStatement.Field.TypeJurPerson_Edit', applyTo: '#cbTypeJurPerson', selector: '#baseStatementEditPanel' },
        { name: 'GkhGji.Inspection.BaseStatement.Field.PersonInspection_Edit', applyTo: '#cbPersonInspection', selector: '#baseStatementEditPanel' },
        { name: 'GkhGji.Inspection.BaseStatement.Field.Contragent_Edit', applyTo: '#sfContragent', selector: '#baseStatementEditPanel' },

        { name: 'GkhGji.Inspection.BaseStatement.Field.CheckDate_View', applyTo: '[name=CheckDate]', selector: '#baseStatementEditPanel' },
        { name: 'GkhGji.Inspection.BaseStatement.Field.CheckDate_Edit', applyTo: '[name=CheckDate]', selector: '#baseStatementEditPanel' },
        { name: 'GkhGji.Inspection.BaseStatement.Register.Contragent.Create', applyTo: 'b4addbutton', selector: '#baseStatementEditPanel inspectiongjicontragentgrid' },
        { name: 'GkhGji.Inspection.BaseStatement.Register.Contragent.Delete', applyTo: 'b4deletecolumn', selector: '#baseStatementEditPanel inspectiongjicontragentgrid' }
    ]
});