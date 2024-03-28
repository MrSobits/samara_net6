Ext.define('B4.aspects.permission.specialobjectcr.BuildContract', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.buildcontractspecialobjectcrperm',

    permissions: [
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DocumentName_Edit', applyTo: 'textfield[name=DocumentName]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DocumentNum_Edit', applyTo: 'textfield[name=DocumentNum]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DocumentDateFrom_Edit', applyTo: 'textfield[name=DocumentDateFrom]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DocumentFile_Edit', applyTo: 'b4filefield[name=DocumentFile]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.Builder_Edit', applyTo: 'b4filefield[name=Builder]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TypeContractBuild_Edit', applyTo: 'combobox[name=TypeContractBuild]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.Sum_Edit', applyTo: 'gkhdecimalfield[name=Sum]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.Inspector_Edit', applyTo: 'b4selectfield[name=Inspector]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateStartWork_Edit', applyTo: 'datefield[name=DateStartWork]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateAcceptOnReg_Edit', applyTo: 'datefield[name=DateAcceptOnReg]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateCancelReg_Edit', applyTo: 'datefield[name=DateCancelReg]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateEndWork_Edit', applyTo: 'datefield[name=DateEndWork]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateInGjiRegister_Edit', applyTo: 'datefield[name=DateInGjiRegister]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.Description_Edit', applyTo: 'textarea[name=Description]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TabResultQual_Edit', applyTo: '[name=ResultQual]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.ProtocolName_Edit', applyTo: 'textfield[name=ProtocolName]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.ProtocolNum_Edit', applyTo: 'textfield[name=ProtocolNum]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.ProtocolDateFrom_Edit', applyTo: 'datefield[name=ProtocolDateFrom]', selector: 'specialobjectcrbuildcontracteditwindow' },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.ProtocolFile_Edit', applyTo: 'b4filefield[name=ProtocolFile]', selector: 'specialobjectcrbuildcontracteditwindow' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.BudgetMo_Edit', applyTo: 'textfield[name=BudgetMo]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.BudgetSubject_Edit', applyTo: 'textfield[name=BudgetSubject]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.OwnerMeans_Edit', applyTo: 'textfield[name=OwnerMeans]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.FundMeans_Edit', applyTo: 'textfield[name=FundMeans]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.TypeWork.View', applyTo: 'contracttypeworkspecialcrgrid', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.TypeWork.Create', applyTo: 'b4addbutton', selector: 'contracttypeworkspecialcrgrid' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.TypeWork.Delete', applyTo: 'b4deletecolumn', selector: 'contracttypeworkspecialcrgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Print', applyTo: 'gkhbuttonprint', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractState_Edit', applyTo: 'b4enumcombo[name=BuildContractState]', selector: 'specialobjectcrbuildcontracteditwindow' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractState_View', applyTo: 'b4enumcombo[name=BuildContractState]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.IsLawProvided_Edit', applyTo: 'b4enumcombo[name=IsLawProvided]', selector: 'specialobjectcrbuildcontracteditwindow' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.IsLawProvided_View', applyTo: 'b4enumcombo[name=IsLawProvided]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.WebSite_Edit', applyTo: 'textfield[name=WebSite]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function(component, allowed) {
                var isLawProvidedField = {},
                    commonAllowed = true;
                if (component) {
                    isLawProvidedField = component.up('specialobjectcrbuildcontracteditwindow').down('b4enumcombo[name=IsLawProvided]');
                    if (isLawProvidedField) {
                        commonAllowed = isLawProvidedField.getValue() === B4.enums.YesNo.Yes;
                    }

                    component.setDisabled(!(allowed && commonAllowed));
                    component.manualDisabled = !allowed;
                }
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.WebSite_View', applyTo: 'b4enumcombo[name=WebSite]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        }
    ]
});