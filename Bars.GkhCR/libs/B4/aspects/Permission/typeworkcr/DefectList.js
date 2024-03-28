Ext.define('B4.aspects.permission.typeworkcr.DefectList', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.defectlisttypeworkcrperm',

    permissions: [
        { name: 'GkhCr.TypeWorkCr.Register.DefectList.Edit', applyTo: 'b4savebutton', selector: 'objectcrdefectlistwin' },
        
        { name: 'GkhCr.TypeWorkCr.Register.DefectList.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: 'objectcrdefectlistwin' },
        { name: 'GkhCr.TypeWorkCr.Register.DefectList.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: 'objectcrdefectlistwin' },
        { name: 'GkhCr.TypeWorkCr.Register.DefectList.Field.Work_Edit', applyTo: '#sfWork', selector: 'objectcrdefectlistwin' },
        { name: 'GkhCr.TypeWorkCr.Register.DefectList.Field.File_Edit', applyTo: '#ffFile', selector: 'objectcrdefectlistwin' },
        { name: 'GkhCr.TypeWorkCr.Register.DefectList.Field.Sum_Edit', applyTo: 'numberfield[name="Sum"]', selector: 'objectcrdefectlistwin'}
    ]
});