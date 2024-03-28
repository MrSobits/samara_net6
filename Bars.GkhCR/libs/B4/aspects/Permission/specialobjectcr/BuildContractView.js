Ext.define('B4.aspects.permission.specialobjectcr.BuildContractView', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.buildcontractspecialobjectcrviewperm',

    permissions: [
        //права на просмотр
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.Inspector_View', applyTo: 'b4selectfield[name=Inspector]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateInGjiRegister_View', applyTo: 'datefield[name=DateInGjiRegister]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateCancelReg_View', applyTo: 'datefield[name=DateCancelReg]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateAcceptOnReg_View', applyTo: 'datefield[name=DateAcceptOnReg]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.Description_View', applyTo: 'textarea[name=Description]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TabResultQual_View', applyTo: '[name=ResultQual]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.tab.show();
                else component.tab.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationContractTab_View', applyTo: '[name=TerminationContractTab]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.tab.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationDate_View', applyTo: 'datefield[name=TerminationDate]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationDate_Edit', applyTo: 'datefield[name=TerminationDate]', selector: 'specialobjectcrbuildcontracteditwindow',
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationDocumentFile_View', applyTo: 'b4filefield[name=TerminationDocumentFile]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationDocumentFile_Edit', applyTo: 'b4filefield[name=TerminationDocumentFile]', selector: 'specialobjectcrbuildcontracteditwindow',
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationReason_View', applyTo: 'textarea[name=TerminationReason]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationReason_Edit', applyTo: 'textarea[name=TerminationReason]', selector: 'specialobjectcrbuildcontracteditwindow',
        },

        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.GuaranteePeriod_View', applyTo: 'textfield[name=GuaranteePeriod]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.GuaranteePeriod_Edit', applyTo: 'textfield[name=GuaranteePeriod]', selector: 'specialobjectcrbuildcontracteditwindow',
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.UrlResultTrading_View', applyTo: 'textfield[name=UrlResultTrading]', selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.UrlResultTrading_Edit', applyTo: 'textfield[name=UrlResultTrading]', selector: 'specialobjectcrbuildcontracteditwindow',
        },
    ]
});