Ext.define('B4.aspects.permission.objectcr.BuildContractView', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.buildcontractobjectcrviewperm',

    permissions: [
        //права на просмотр
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.Inspector_View', applyTo: '#sfInspector', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DateInGjiRegister_View', applyTo: '#dfDateInGjiRegister', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DateCancelReg_View', applyTo: '#dfDateCancelReg', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.DateAcceptOnReg_View', applyTo: '#dfDateAcceptOnReg', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.Description_View', applyTo: '#taDescription', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TabResultQual_View', applyTo: '#tabResultQual', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.tab.show();
                else component.tab.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TerminationContractTab_View', applyTo: '[name=TerminationContractTab]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.tab.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TerminationDate_View', applyTo: '[name=TerminationDate]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TerminationDate_Edit', applyTo: '[name=TerminationDate]', selector: 'buildcontracteditwindow',
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TerminationDocumentFile_View', applyTo: '[name=TerminationDocumentFile]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TerminationDocumentFile_Edit', applyTo: '[name=TerminationDocumentFile]', selector: 'buildcontracteditwindow',
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TerminationReason_View', applyTo: '[name=TerminationReason]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.TerminationReason_Edit', applyTo: '[name=TerminationReason]', selector: 'buildcontracteditwindow',
        },

        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.GuaranteePeriod_View', applyTo: '[name=GuaranteePeriod]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.GuaranteePeriod_Edit', applyTo: '[name=GuaranteePeriod]', selector: 'buildcontracteditwindow',
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.UrlResultTrading_View', applyTo: '[name=UrlResultTrading]', selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.UrlResultTrading_Edit', applyTo: '[name=UrlResultTrading]', selector: 'buildcontracteditwindow',
        },
    ]
});