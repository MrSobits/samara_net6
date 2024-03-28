Ext.define('B4.aspects.permission.objectcr.TypeWork', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.typeworkobjectcrperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Create', applyTo: 'b4addbutton', selector: 'objectcr_type_work_cr_grid' },
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Edit', applyTo: 'b4savebutton', selector: 'typeworkcreditwindow' },
        {
            name: 'GkhCr.ObjectCr.Register.TypeWork.Delete', applyTo: 'b4deletecolumn', selector: 'objectcr_type_work_cr_grid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.TypeWork.DeleteStruclEl', applyTo: 'b4deletecolumn', selector: 'objectcrtypeworkst1grid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.TypeWork.MoveToAnotherPeriod', applyTo: '#sendToOtherPeriodButton', selector: 'typeworkcreditwindow',
            applyBy: function (component, allowed) {
                debugger;
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.FinanceSource_Edit', applyTo: '#sflFinanceSource', selector: 'typeworkcreditwindow' },
        // Закомментировал потому что нельзя правами это поле скрывать или открывать. Оно открывается только не для программы сформирвоанной вручную { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.TypeWork_Edit', applyTo: '#sflWork', selector: 'typeworkcreditwindow' },
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.SumMaterialsRequirement_Edit', applyTo: '#dcfSumMaterialsRequirement', selector: 'typeworkcreditwindow' },
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.HasPsd_Edit', applyTo: '#chbxHasPSD', selector: 'typeworkcreditwindow' },
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.Volume_Edit', applyTo: '#dcfVolume', selector: 'typeworkcreditwindow' },
     //   { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.Sum_Edit', applyTo: '#dcfSum', selector: 'typeworkcreditwindow' },
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.Description_Edit', applyTo: '#taDescription', selector: 'typeworkcreditwindow' },
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.DateStartWork_Edit', applyTo: 'datefield[name=DateStartWork]', selector: 'typeworkcreditwindow' },
        {
            name: 'GkhCr.ObjectCr.Register.TypeWork.Field.DateStartWork_View', applyTo: 'datefield[name=DateStartWork]', selector: 'typeworkcreditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        { name: 'GkhCr.ObjectCr.Register.TypeWork.Field.DateEndWork_Edit', applyTo: 'datefield[name=DateEndWork]', selector: 'typeworkcreditwindow' },
        {
            name: 'GkhCr.ObjectCr.Register.TypeWork.Field.DateEndWork_View', applyTo: 'datefield[name=DateEndWork]', selector: 'typeworkcreditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.TypeWork.Field.GroupDpkr_View', applyTo: '[name=groupDpkr]', selector: 'objectcr_type_work_cr_grid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.TypeWork.Field.GroupCr_View', applyTo: '[name=groupCr]', selector: 'objectcr_type_work_cr_grid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // журнал изменений? кнопка Восстановить
        { name: 'GkhCr.ObjectCr.Register.TypeWorkCrHistory.Restore', applyTo: 'button[name=Restore]', selector: 'objectcr_type_work_cr_history_grid' },
    ]
});