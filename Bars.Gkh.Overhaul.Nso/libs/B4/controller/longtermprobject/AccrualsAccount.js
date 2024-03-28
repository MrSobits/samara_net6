Ext.define('B4.controller.longtermprobject.AccrualsAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.enums.AccountType'
    ],
    models: ['account.Accruals', 'account.operation.Accruals'],
    stores: [
        'account.Accruals',
        'dict.AccountOperationNoPaging',
        'longtermprobject.AccountCharge'
    ],

    views: [
        'longtermprobject.accrualsaccount.Grid',
        'longtermprobject.accrualsaccount.EditWindow',
        'longtermprobject.accrualsaccount.AccountsAggregationGrid'
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
                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.params.realityObjectId);
                        obj.set('AccountType', B4.enums.AccountType.getMeta('Accruals').Value);
                    }
                    return true;
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.setAccountId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.Accounts.Accruals.Create', applyTo: 'b4addbutton', selector: 'accrualsaccountgrid' },
                { name: 'Gkh.RealityObject.Register.Accounts.Accruals.Edit', applyTo: 'b4savebutton', selector: 'accrualsaccounteditwin' },
                { name: 'Gkh.RealityObject.Register.Accounts.Accruals.Delete', applyTo: 'b4deletecolumn', selector: 'accrualsaccountgrid',
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
        this.getStore('longtermprobject.AccountCharge').on('beforeload', this.onBeforeLoad, this);
        
        this.control({
            'accountsaggregationgrid b4updatebutton': { click: { fn: this.updateGrid, scope: this } }
        });

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('account.Accruals').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.roId = this.params.realityObjectId;
    },
    
    updateGrid: function () {
        this.getStore('longtermprobject.AccountCharge').load();
    },
    
    setAccountId: function (id) {
        var grid = this.getEditWindow().down('accountsaggregationgrid'),
            store = grid.getStore();
        
        this.params.accountId = id;
        store.removeAll();
        grid.setDisabled(!id);
        
        if (id) {
            store.load();
        }
    }
});