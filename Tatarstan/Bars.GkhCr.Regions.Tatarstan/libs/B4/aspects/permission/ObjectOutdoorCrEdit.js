Ext.define('B4.aspects.permission.ObjectOutdoorCrEdit', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.objectoutdoorcreditstateperm',
    applyByPostfix: true,
    
    init: function () {
        var me = this;

        me.permissions = [
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Edit', applyTo: 'b4savebutton', selector: 'objectoutdoorcreditpanel' },

            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.RealityObjectOutdoor_Edit', applyTo: '[name=RealityObjectOutdoor]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.RealityObjectOutdoorProgram_Edit', applyTo: '[name=RealityObjectOutdoorProgram]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.WarrantyEndDate_Edit', applyTo: '[name=WarrantyEndDate]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.GjiNum_Edit', applyTo: '[name=GjiNum]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.Description_Edit', applyTo: '[name=Description]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.MaxAmount_Edit', applyTo: '[name=MaxAmount]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactStartDate_Edit', applyTo: '[name=FactStartDate]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.SumSmr_Edit', applyTo: '[name=SumSmr]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateGjiReg_Edit', applyTo: '[name=DateGjiReg]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateEndBuilder_Edit', applyTo: '[name=DateEndBuilder]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateStartWork_Edit', applyTo: '[name=DateStartWork]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactAmountSpent_Edit', applyTo: '[name=FactAmountSpent]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactEndDate_Edit', applyTo: '[name=FactEndDate]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.SumSmrApproved_Edit', applyTo: '[name=SumSmrApproved]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateStopWorkGji_Edit', applyTo: '[name=DateStopWorkGji]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateAcceptGji_Edit', applyTo: '[name=DateAcceptGji]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateEndWork_Edit', applyTo: '[name=DateEndWork]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.СommissioningDate_Edit', applyTo: '[name=CommissioningDate]', selector: 'objectoutdoorcreditpanel' },

            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.RealityObjectOutdoor_View', applyTo: '[name=RealityObjectOutdoor]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.RealityObjectOutdoorProgram_View', applyTo: '[name=RealityObjectOutdoorProgram]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.WarrantyEndDate_View', applyTo: '[name=WarrantyEndDate]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.GjiNum_View', applyTo: '[name=GjiNum]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.Description_View', applyTo: '[name=Description]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.MaxAmount_View', applyTo: '[name=MaxAmount]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactStartDate_View', applyTo: '[name=FactStartDate]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.SumSmr_View', applyTo: '[name=SumSmr]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateGjiReg_View', applyTo: '[name=DateGjiReg]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateEndBuilder_View', applyTo: '[name=DateEndBuilder]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateStartWork_View', applyTo: '[name=DateStartWork]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactAmountSpent_View', applyTo: '[name=FactAmountSpent]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.FactEndDate_View', applyTo: '[name=FactEndDate]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.SumSmrApproved_View', applyTo: '[name=SumSmrApproved]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateStopWorkGji_View', applyTo: '[name=DateStopWorkGji]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateAcceptGji_View', applyTo: '[name=DateAcceptGji]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.DateEndWork_View', applyTo: '[name=DateEndWork]', selector: 'objectoutdoorcreditpanel' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.Fields.СommissioningDate_View', applyTo: '[name=CommissioningDate]', selector: 'objectoutdoorcreditpanel' }
        ];

        me.callParent(arguments);
    }
});