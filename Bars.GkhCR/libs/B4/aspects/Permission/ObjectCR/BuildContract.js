Ext.define('B4.aspects.permission.objectcr.BuildContract', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.buildcontractobjectcrperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Edit', applyTo: 'b4savebutton', selector: 'buildcontracteditwindow' },

        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DocumentName_Edit', applyTo: '#tfDocumentName', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DocumentNum_Edit', applyTo: '#tfDocumentNum', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DocumentDateFrom_Edit', applyTo: '#tfDocumentDateFrom', selector: 'buildcontracteditwindow' },
      //  { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DocumentFile_Edit', applyTo: '#ffDocumentFile', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.Builder_Edit', applyTo: '#sfBuilder', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TypeContractBuild_Edit', applyTo: '#cbbxTypeContractBuild', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.Sum_Edit', applyTo: '#dcfSum', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.Inspector_Edit', applyTo: '#sfInspector', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DateStartWork_Edit', applyTo: '#dfDateStartWork', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DateAcceptOnReg_Edit', applyTo: '#dfDateAcceptOnReg', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DateCancelReg_Edit', applyTo: '#dfDateCancelReg', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DateEndWork_Edit', applyTo: '#dfDateEndWork', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DateInGjiRegister_Edit', applyTo: '#dfDateInGjiRegister', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.Description_Edit', applyTo: '#taDescription', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TabResultQual_Edit', applyTo: '#tabResultQual', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.ProtocolName_Edit', applyTo: '#tfProtocolName', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.ProtocolNum_Edit', applyTo: '#tfProtocolNum', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.ProtocolDateFrom_Edit', applyTo: '#tfProtocolDateFrom', selector: 'buildcontracteditwindow' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.ProtocolFile_Edit', applyTo: '#tfProtocolFile', selector: 'buildcontracteditwindow' },

        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DocumentFile_Edit', applyTo: '#ffDocumentFile', selector: 'buildcontracteditwindow',
           applyBy: function (component, allowed) {
            if(!component)
            {
            return;
            }
                        component.onApplyBy(allowed, component.trigger1Cls);
                        component.onApplyBy(allowed, component.trigger3Cls);
            }},
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.BudgetMo_Edit', applyTo: '#tfBudgetMo', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.BudgetSubject_Edit', applyTo: '#tfBudgetSubject', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.OwnerMeans_Edit', applyTo: '#tfOwnerMeans', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.FundMeans_Edit', applyTo: '#tfFundMeans', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.TypeWork.View', applyTo: 'buildcontrtypewrkgrid', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.TypeWork.Create', applyTo: 'b4addbutton', selector: 'buildcontrtypewrkgrid' },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.TypeWork.Delete', applyTo: 'b4deletecolumn', selector: 'buildcontrtypewrkgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Print', applyTo: '#btnPrint', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractState_Edit', applyTo: 'field[name=BuildContractState]', selector: 'buildcontracteditwindow' },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractState_View', applyTo: 'field[name=BuildContractState]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        { name: 'GkhCr.ObjectCr.Register.BuildContract.Field.IsLawProvided_Edit', applyTo: 'b4enumcombo[name=IsLawProvided]', selector: 'buildcontracteditwindow' },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.IsLawProvided_View', applyTo: 'b4enumcombo[name=IsLawProvided]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.WebSite_Edit', applyTo: 'field[name=WebSite]', selector: 'buildcontracteditwindow',
            applyBy: function(component, allowed) {
                var isLawProvidedField = {},
                    commonAllowed = true;
                if (component) {
                    isLawProvidedField = component.up('buildcontracteditwindow').down('field[name=IsLawProvided]');
                    if (isLawProvidedField) {
                        commonAllowed = isLawProvidedField.getValue() === B4.enums.YesNo.Yes;
                    }

                    component.setDisabled(!(allowed && commonAllowed));
                    component.manualDisabled = !allowed;
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.WebSite_View', applyTo: 'field[name=WebSite]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        }
    ]
});