Ext.define('B4.aspects.permission.BaseProsClaim', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.baseprosclaimperm',
    applyByPostfix: true,
    permissions: [
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.InspectionNumber_Edit', applyTo: '#tfInspectionNumber', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.TypeBaseProsClaim_Edit', applyTo: '#cbTypeBaseProsClaim', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.ProsClaimDateCheck_Edit', applyTo: '#dfProsClaimDateCheck', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.IssuedClaim_Edit', applyTo: '#tfIssuedClaim', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.TypeForm_Edit', applyTo: '#cbTypeForm', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.Inspectors_Edit', applyTo: '#prosClaimInspectorsTrigerField', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.DocumentDescription_Edit', applyTo: '#taDocumentDescription', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.TypeJurPerson_Edit', applyTo: '#cbTypeJurPerson', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.PersonInspection_Edit', applyTo: '#cbPersonInspection', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.Contragent_Edit', applyTo: '#sfContragent', selector: '#baseProsClaimEditPanel' },
        
        { name: 'GkhGji.Inspection.BaseProsClaim.Register.RealityObject.Create', applyTo: 'b4addbutton', selector: '#baseProsClaimRealityObjectGrid' },
        {
            name: 'GkhGji.Inspection.BaseProsClaim.Register.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: '#baseProsClaimRealityObjectGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.CheckDate_View', applyTo: '[name=CheckDate]', selector: '#baseProsClaimEditPanel' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Field.CheckDate_Edit', applyTo: '[name=CheckDate]', selector: '#baseProsClaimEditPanel' },

        { name: 'GkhGji.Inspection.BaseProsClaim.Register.Contragent.Create', applyTo: 'b4addbutton', selector: '#baseProsClaimEditPanel inspectiongjicontragentgrid' },
        { name: 'GkhGji.Inspection.BaseProsClaim.Register.Contragent.Delete', applyTo: 'b4deletecolumn', selector: '#baseProsClaimEditPanel inspectiongjicontragentgrid' }
    ]
});