Ext.define('B4.aspects.permission.ObjectCr', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.objectcrstateperm',

    init: function () {
        var me = this;

        me.permissions = [
            { name: 'GkhCr.ObjectCr.Edit', applyTo: 'b4savebutton', selector: 'objectCrEditPanel' },
            { name: 'GkhCr.ObjectCr.Field.RealityObject_Edit', applyTo: '#sfRealityObject', selector: 'objectCrEditPanel' },
            { name: 'GkhCr.ObjectCr.Field.ProgramCr_Edit', applyTo: '#sfProgramCr', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.ProgramNum_View', applyTo: '#tfProgramNum', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.ProgramNum_Edit', applyTo: '#tfProgramNum', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.GjiNum_View', applyTo: '#tfGjiNum', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.GjiNum_Edit', applyTo: '#tfGjiNum', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.FederalNumber_View', applyTo: '#tfFederalNumber', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.FederalNumber_Edit', applyTo: '#tfFederalNumber', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.Description_Edit', applyTo: '#taDescription', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.SumSMR_View', applyTo: '#dcfSumSMR', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.SumSMR_Edit', applyTo: '#dcfSumSMR', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.SumDevolopmentPSD_View', applyTo: '#dcfSumDevolopmentPSD', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.SumDevolopmentPSD_Edit', applyTo: '#dcfSumDevolopmentPSD', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.DateGjiReg_View', applyTo: '#dfDateGjiReg', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.DateGjiReg_Edit', applyTo: '#dfDateGjiReg', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.DateEndBuilder_View', applyTo: '#dfDateEndBuilder', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.DateEndBuilder_Edit', applyTo: '#dfDateEndBuilder', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.DateStartWork_View', applyTo: '#dfDateStartWork', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.DateStartWork_Edit', applyTo: '#dfDateStartWork', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.DateCancelReg_View', applyTo: '#dfDateCancelReg', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.DateCancelReg_Edit', applyTo: '#dfDateCancelReg', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.SumSMRApproved_View', applyTo: '#dcfSumSMRApproved', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.SumSMRApproved_Edit', applyTo: '#dcfSumSMRApproved', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.SumTehInspection_View', applyTo: '#dcfSumTehInspection', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.SumTehInspection_Edit', applyTo: '#dcfSumTehInspection', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.DateStopWorkGji_View', applyTo: '#dfDateStopWorkGji', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.DateStopWorkGji_Edit', applyTo: '#dfDateStopWorkGji', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.DateAcceptCrGji_View', applyTo: '#dfDateAcceptCrGji', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.DateAcceptCrGji_Edit', applyTo: '#dfDateAcceptCrGji', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.DateEndWork_View', applyTo: '#dfDateEndWork', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.DateEndWork_Edit', applyTo: '#dfDateEndWork', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.DateAcceptReg_View', applyTo: '#dfDateAcceptReg', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.DateAcceptReg_Edit', applyTo: '#dfDateAcceptReg', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.MaxKpkrAmount_View', applyTo: '#dcfMaxKpkrAmount', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.MaxKpkrAmount_Edit', applyTo: '#dcfMaxKpkrAmount', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.FactAmountSpent_View', applyTo: '#dcfFactAmountSpent', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.FactAmountSpent_Edit', applyTo: '#dcfFactAmountSpent', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.FactStartDate_View', applyTo: '#dfFactStartDate', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.FactStartDate_Edit', applyTo: '#dfFactStartDate', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.FactEndDate_View', applyTo: '#dfFactEndDate', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.FactEndDate_Edit', applyTo: '#dfFactEndDate', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.WarrantyEndDate_View', applyTo: '#dfWarrantyEndDate', selector: 'objectCrEditPanel', applyBy: me.setVisible },
            { name: 'GkhCr.ObjectCr.Field.WarrantyEndDate_Edit', applyTo: '#dfWarrantyEndDate', selector: 'objectCrEditPanel' },        
            { name: 'GkhCr.ObjectCr.Field.AllowReneg.Edit', applyTo: '#dfAllowReneg', selector: 'objectCrEditPanel' },
            {
                name: 'GkhCr.ObjectCr.Field.AllowReneg.View',
                applyTo: '#dfAllowReneg',
                selector: 'objectCrEditPanel',
                applyBy: function(component, allowed) {
                    if (component)
                        component.setVisible(allowed);
                }
            }
        ];

        this.callParent(arguments);
    }
});