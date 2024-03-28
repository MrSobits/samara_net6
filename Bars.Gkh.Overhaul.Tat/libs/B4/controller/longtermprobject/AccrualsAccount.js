Ext.define('B4.controller.longtermprobject.AccrualsAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.enums.AccountType'
    ],
    models: ['account.Accruals'],
    stores: ['account.Accruals', 'account.operation.Accruals'],
    views: [
        'longtermprobject.accrualsaccount.Grid',
        'longtermprobject.accrualsaccount.EditWindow',
        'longtermprobject.accrualsaccount.OperationEditWindow'
    ],

    mainView: 'longtermprobject.accrualsaccount.Grid',
    mainViewSelector: 'accrualsaccountgrid',

    refs: [
        { ref: 'editWindow', selector: 'accrualsaccounteditwin' }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'accrualsAccountGridWindowAspect',
            gridSelector: 'accrualsaccountgrid',
            editFormSelector: 'accrualsaccounteditwin',
            storeName: 'account.Accruals',
            modelName: 'account.Accruals',
            editWindowView: 'longtermprobject.accrualsaccount.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setAccountId(record.getId());
            },
            listeners: {
                beforesave: function (asp, obj) {
                    obj.set('RealityObject', asp.controller.params.realityObjectId);
                    return true;
                },
                aftersetformdata: function(asp, record) {
                    asp.controller.setAccountId(record.getId());
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'accrualsAccountOperationGridWindowAspect',
            gridSelector: 'accrualaccountopergrid',
            editFormSelector: 'accrualsaccountopereditwin',
            storeName: 'account.operation.Accruals',
            modelName: 'account.operation.Accruals',
            editWindowView: 'longtermprobject.accrualsaccount.OperationEditWindow',
            listeners: {
                beforesave: function (asp, obj) {
                    obj.set('AccountType', B4.enums.AccountType.getMeta('Accruals').Value);
                    obj.set('Account', asp.controller.params.accountId);
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.Accounts.Accurals.Create', applyTo: 'b4addbutton', selector: 'accrualsaccountgrid' },
                { name: 'Gkh.RealityObject.Register.Accounts.Accurals.Edit', applyTo: 'b4savebutton', selector: 'accrualsaccounteditwin' },
                { name: 'Gkh.RealityObject.Register.Accounts.Accurals.Delete', applyTo: 'b4deletecolumn', selector: 'accrualsaccountgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        this.getStore('account.Accruals').on('beforeload', this.onBeforeLoad, this);
        
        this.getStore('account.operation.Accruals').on('beforeload', this.onBeforeLoadOperations, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('account.Accruals').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.roId = this.params.realityObjectId;
    },

    onBeforeLoadOperations: function (store, operation) {
        operation.params.accountId = this.params.accountId;
    },
    
    setAccountId: function (id) {
        var grid = this.getEditWindow().down('accrualaccountopergrid'),
            store = grid.getStore();
        
        this.params.accountId = id;
        store.removeAll();
        grid.setDisabled(!id);
        
        if (id) {
            store.load();
        }
    }
});