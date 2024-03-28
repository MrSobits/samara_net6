Ext.define('B4.aspects.permission.specialobjectcr.DefectList', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.defectlistspecialobjectcrperm',

    permissions: [
        { name: 'GkhCr.SpecialObjectCr.Register.DefectList.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrdefectlistwin' },

        { name: 'GkhCr.SpecialObjectCr.Register.DefectList.Field.DocumentName_Edit', applyTo: 'textfield[name=DocumentName]', selector: 'specialobjectcrdefectlistwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.DefectList.Field.DocumentDate_Edit', applyTo: 'datefield[name=DocumentDate]', selector: 'specialobjectcrdefectlistwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.DefectList.Field.Work_Edit', applyTo: 'b4selectfield[name=Work]', selector: 'specialobjectcrdefectlistwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.DefectList.Field.File_Edit', applyTo: 'b4filefield[name=File]', selector: 'specialobjectcrdefectlistwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.DefectList.Field.Sum_Edit', applyTo: 'numberfield[name="Sum"]', selector: 'specialobjectcrdefectlistwin'}
    ]
});