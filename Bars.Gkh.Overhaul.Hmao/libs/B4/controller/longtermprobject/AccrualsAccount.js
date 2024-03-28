Ext.define('B4.controller.longtermprobject.AccrualsAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.enums.AccountType'
    ],
    
    models: [
        'account.Accruals',
        'account.operation.Accruals'
    ],
    
    stores: [
        'account.Accruals',
        'account.operation.Accruals',
        'dict.AccountOperationNoPaging'
    ],

    views: [
        'longtermprobject.accrualsaccount.Grid',
        'longtermprobject.accrualsaccount.EditWindow',
        'longtermprobject.accrualsaccount.OperationGrid'
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
            xtype: 'gkhinlinegridaspect',
            name: 'accountoperationinlinegridaspect',
            gridSelector: 'accrualaccountopergrid',
            saveButtonSelector: 'accrualaccountopergrid #btnSave',
            storeName: 'account.operation.Accruals',
            modelName: 'account.operation.Accruals',
            listeners: {
                beforesave: function (asp, store) {
                    var modifRecords = store.getModifiedRecords(),
                        result = true;

                    Ext.each(modifRecords, function(rec) {
                        return result =
                            !Ext.isEmpty(rec.get('AccrualDate'))
                                && !Ext.isEmpty(rec.get('TotalDebit'))
                                && !Ext.isEmpty(rec.get('TotalCredit'))
                                && !Ext.isEmpty(rec.get('OpeningBalance'))
                                && !Ext.isEmpty(rec.get('ClosingBalance'));
                    });

                    if (!result) {
                        B4.QuickMsg.msg('Предупреждение', 'Необходимо заполнить все поля', 'warning');
                        return false;
                    }

                    Ext.each(store.data.items, function(rec) {
                        if (!rec.getId()) {
                            rec.set('Account', asp.controller.params.accountId);
                        }
                    });
                    return true;
                }
            },
            deleteRecord: function (record) {
                this.getGrid().getStore().remove(record);
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