Ext.define('B4.controller.longtermprobject.RealAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.enums.AccountType'
    ],

    models: [
        'account.Real',
        'account.operation.Real'
    ],
    
    stores: [
        'account.Real',
        'account.operation.Real',
        'dict.AccountOperationNoPaging'
    ],
    views: [
        'longtermprobject.realaccount.Grid',
        'longtermprobject.realaccount.EditWindow',
        'longtermprobject.realaccount.OperationGrid'
    ],

    mainView: 'longtermprobject.realaccount.Grid',
    mainViewSelector: 'realaccountgrid',

    refs: [
        { ref: 'editWindow', selector: 'realaccounteditwin' }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'realAccountGridWindowAspect',
            gridSelector: 'realaccountgrid',
            editFormSelector: 'realaccounteditwin',
            storeName: 'account.Real',
            modelName: 'account.Real',
            editWindowView: 'longtermprobject.realaccount.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setAccountId(record.getId());
            },
            listeners: {
                beforesave: function (asp, obj) {
                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.params.realityObjectId);
                        obj.set('AccountType', B4.enums.AccountType.getMeta('Real').Value);
                    }
                    return true;
                },
                aftersetformdata: function(asp, record) {
                    asp.controller.setAccountId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'accountoperationinlinegridaspect',
            gridSelector: 'realaccountopergrid',
            saveButtonSelector: 'realaccountopergrid #btnSave',
            storeName: 'account.operation.Real',
            modelName: 'account.operation.Real',
            listeners: {
                beforesave: function (asp, store) {
                    var modifRecords = store.getModifiedRecords(),
                        result = true;

                    Ext.each(modifRecords, function (rec) {
                        return result =
                            !Ext.isEmpty(rec.get('Operation'))
                                && !Ext.isEmpty(rec.get('OperationDate'))
                                && !Ext.isEmpty(rec.get('Sum'))
                                && !Ext.isEmpty(rec.get('Receiver'))
                                && !Ext.isEmpty(rec.get('Payer'))
                                && !Ext.isEmpty(rec.get('Purpose'));
                    });

                    if (!result) {
                        B4.QuickMsg.msg('Предупреждение', 'Необходимо заполнить все поля', 'warning');
                        return false;
                    }

                    Ext.each(store.data.items, function (rec) {
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
        }
    ],

    init: function() {
        this.getStore('account.Real').on('beforeload', this.onBeforeLoad, this);
        this.getStore('account.operation.Real').on('beforeload', this.onBeforeLoadOperations, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('account.Real').load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.roId = this.params.realityObjectId;
    },
    
    onBeforeLoadOperations: function(store, operation) {
        operation.params.accountId = this.params.accountId;
    },
    
    setAccountId: function(id) {
        var grid = this.getEditWindow().down('realaccountopergrid'),
            store = grid.getStore();

        this.params.accountId = id;
        store.removeAll();
        grid.setDisabled(!id);
        
        if (id) {
            store.load();
        }
    }
});