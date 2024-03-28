Ext.define('B4.aspects.permission.specialobjectcr.TypeWork', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.typeworkspecialobjectcrperm',

    permissions: [
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Create', applyTo: 'b4addbutton', selector: 'typeworkspecialcrgrid' },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Edit', applyTo: 'b4savebutton', selector: 'typeworkspecialcreditwindow' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Delete', applyTo: 'b4deletecolumn', selector: 'typeworkspecialcrgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.FinanceSource_Edit', applyTo: 'b4selectfield[name=FinanceSource]', selector: 'typeworkspecialcreditwindow' },
        // Закомментировал потому что нельзя правами это поле скрывать или открывать. Оно открывается только не для программы сформирвоанной вручную { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.TypeWork_Edit', applyTo: '#sflWork', selector: 'typeworkspecialcreditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.SumMaterialsRequirement_Edit', applyTo: 'gkhdecimalfield[name=SumMaterialsRequirement]', selector: 'typeworkspecialcreditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.HasPsd_Edit', applyTo: 'checkbox[name=HasPSD]', selector: 'typeworkspecialcreditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.Volume_Edit', applyTo: 'gkhdecimalfield[name=Volume]', selector: 'typeworkspecialcreditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.Sum_Edit', applyTo: 'gkhdecimalfield[name=Sum]', selector: 'typeworkspecialcreditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.Description_Edit', applyTo: 'textarea[name=Description]', selector: 'typeworkspecialcreditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.DateStartWork_Edit', applyTo: 'datefield[name=DateStartWork]', selector: 'typeworkspecialcreditwindow' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.DateStartWork_View', applyTo: 'datefield[name=DateStartWork]', selector: 'typeworkspecialcreditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.DateEndWork_Edit', applyTo: 'datefield[name=DateEndWork]', selector: 'typeworkspecialcreditwindow' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.TypeWork.Field.DateEndWork_View', applyTo: 'datefield[name=DateEndWork]', selector: 'typeworkspecialcreditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        
        // журнал изменений? кнопка Восстановить
        { name: 'GkhCr.SpecialObjectCr.Register.TypeWorkCrHistory.Restore', applyTo: 'button[name=Restore]', selector: 'typeworkspecialcrhistorygrid' },
    ]
});