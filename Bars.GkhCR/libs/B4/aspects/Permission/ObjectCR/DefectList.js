Ext.define('B4.aspects.permission.objectcr.DefectList', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.defectlistobjectcrperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.DefectList.Edit', applyTo: 'b4savebutton', selector: 'objectcrdefectlistwin' },
        
        { name: 'GkhCr.ObjectCr.Register.DefectList.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: 'objectcrdefectlistwin' },
        { name: 'GkhCr.ObjectCr.Register.DefectList.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: 'objectcrdefectlistwin' },
        { name: 'GkhCr.ObjectCr.Register.DefectList.Field.Work_Edit', applyTo: '#sfWork', selector: 'objectcrdefectlistwin' },
        { name: 'GkhCr.ObjectCr.Register.DefectList.Field.File_Edit', applyTo: '#ffFile', selector: 'objectcrdefectlistwin' },
        { name: 'GkhCr.ObjectCr.Register.DefectList.Field.Sum_Edit', applyTo: 'numberfield[name="Sum"]', selector: 'objectcrdefectlistwin'}
    ]
});