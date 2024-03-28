Ext.define('B4.aspects.permission.SpecialObjectCr', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.specialobjectcrstateperm',

    init: function () {
        var me = this;

        me.permissions = [
            { name: 'GkhCr.SpecialObjectCr.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcreditpanel' },
            { name: 'GkhCr.SpecialObjectCr.Field.RealityObject_Edit', applyTo: 'b4selectfield[name=RealityObject]', selector: 'specialobjectcreditpanel' },
            { name: 'GkhCr.SpecialObjectCr.Field.ProgramCr_Edit', applyTo: 'b4selectfield[name=ProgramCr]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.ProgramNum_View', applyTo: 'textfield[name=ProgramNum]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.ProgramNum_Edit', applyTo: 'textfield[name=ProgramNum]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.GjiNum_View', applyTo: 'textfield[name=GjiNum]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.GjiNum_Edit', applyTo: 'textfield[name=GjiNum]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.FederalNumber_View', applyTo: 'textfield[name=FederalNumber]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.FederalNumber_Edit', applyTo: 'textfield[name=FederalNumber]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.Description_Edit', applyTo: 'textarea[name=Description]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.SumSMR_View', applyTo: 'gkhdecimalfield[name=SumSMR]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.SumSMR_Edit', applyTo: 'gkhdecimalfield[name=SumSMR]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.SumDevolopmentPSD_View', applyTo: 'gkhdecimalfield[name=SumDevolopmentPSD]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.SumDevolopmentPSD_Edit', applyTo: 'gkhdecimalfield[name=SumDevolopmentPSD]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.DateGjiReg_View', applyTo: 'datefield[name=DateGjiReg]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.DateGjiReg_Edit', applyTo: 'datefield[name=DateGjiReg]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.DateEndBuilder_View', applyTo: 'datefield[name=DateEndBuilder]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.DateEndBuilder_Edit', applyTo: 'datefield[name=DateEndBuilder]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.DateStartWork_View', applyTo: 'datefield[name=DateStartWork]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.DateStartWork_Edit', applyTo: 'datefield[name=DateStartWork]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.DateCancelReg_View', applyTo: 'datefield[name=DateCancelReg]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.DateCancelReg_Edit', applyTo: 'datefield[name=DateCancelReg]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.SumSMRApproved_View', applyTo: 'gkhdecimalfield[name=SumSMRApproved]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.SumSMRApproved_Edit', applyTo: 'gkhdecimalfield[name=SumSMRApproved]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.SumTehInspection_View', applyTo: 'gkhdecimalfield[name=SumTehInspection]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.SumTehInspection_Edit', applyTo: 'gkhdecimalfield[name=SumTehInspection]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.DateStopWorkGji_View', applyTo: 'datefield[name=DateStopWorkGji]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.DateStopWorkGji_Edit', applyTo: 'datefield[name=DateStopWorkGji]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.DateAcceptCrGji_View', applyTo: 'datefield[name=DateAcceptCrGji]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.DateAcceptCrGji_Edit', applyTo: 'datefield[name=DateAcceptCrGji]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.DateEndWork_View', applyTo: 'datefield[name=DateEndWork]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.DateEndWork_Edit', applyTo: 'datefield[name=DateEndWork]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.DateAcceptReg_View', applyTo: 'datefield[name=DateAcceptReg]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.DateAcceptReg_Edit', applyTo: 'datefield[name=DateAcceptReg]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.MaxKpkrAmount_View', applyTo: 'gkhdecimalfield[name=MaxKpkrAmount]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.MaxKpkrAmount_Edit', applyTo: 'gkhdecimalfield[name=MaxKpkrAmount]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.FactAmountSpent_View', applyTo: 'gkhdecimalfield[name=FactAmountSpent]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.FactAmountSpent_Edit', applyTo: 'gkhdecimalfield[name=FactAmountSpent]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.FactStartDate_View', applyTo: 'datefield[name=FactStartDate]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.FactStartDate_Edit', applyTo: 'datefield[name=FactStartDate]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.FactEndDate_View', applyTo: 'datefield[name=FactEndDate]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.FactEndDate_Edit', applyTo: 'datefield[name=FactEndDate]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.WarrantyEndDate_View', applyTo: 'datefield[name=WarrantyEndDate]', selector: 'specialobjectcreditpanel', applyBy: me.setVisible },
            { name: 'GkhCr.SpecialObjectCr.Field.WarrantyEndDate_Edit', applyTo: 'datefield[name=WarrantyEndDate]', selector: 'specialobjectcreditpanel' },        
            { name: 'GkhCr.SpecialObjectCr.Field.AllowReneg.Edit', applyTo: '#dfAllowReneg', selector: 'specialobjectcreditpanel' },
            {
                name: 'GkhCr.SpecialObjectCr.Field.AllowReneg.View',
                applyTo: '#dfAllowReneg',
                selector: 'specialobjectcreditpanel',
                applyBy: function(component, allowed) {
                    if (component)
                        component.setVisible(allowed);
                }
            }
        ];

        this.callParent(arguments);
    }
});