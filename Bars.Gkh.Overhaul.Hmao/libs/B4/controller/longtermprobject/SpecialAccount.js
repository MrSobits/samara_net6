Ext.define('B4.controller.longtermprobject.SpecialAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    models: [
        'account.Special',
        'account.operation.Special'
    ],
    stores: [
        'account.Special',
        'account.operation.Special',
        'dict.AccountOperationNoPaging'
    ],
    views: [
        'longtermprobject.specialaccount.Grid',
        'longtermprobject.specialaccount.EditWindow',
        'longtermprobject.specialaccount.OperationGrid'
    ],

    mainView: 'longtermprobject.specialaccount.Grid',
    mainViewSelector: 'specialaccountgrid',

    refs: [
        { ref: 'editWindow', selector: 'specialaccounteditwin' }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'specialAccountGridWindowAspect',
            gridSelector: 'specialaccountgrid',
            editFormSelector: 'specialaccounteditwin',
            storeName: 'account.Special',
            modelName: 'account.Special',
            editWindowView: 'longtermprobject.specialaccount.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setAccountId(record.getId());
            },
            listeners: {
                beforesave: function (asp, obj) {
                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.params.realityObjectId);
                        obj.set('AccountType', B4.enums.AccountType.getMeta('Special').Value);
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
            gridSelector: 'specaccountopergrid',
            saveButtonSelector: 'specaccountopergrid #btnSave',
            storeName: 'account.operation.Special',
            modelName: 'account.operation.Special',
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
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.Accounts.Special.Create', applyTo: 'b4addbutton', selector: 'specialaccountgrid' },
                { name: 'Gkh.RealityObject.Register.Accounts.Special.Edit', applyTo: 'b4savebutton', selector: 'specialaccounteditwin' },
                { name: 'Gkh.RealityObject.Register.Accounts.Special.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'specialaccountgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function() {
        this.getStore('account.Special').on('beforeload', this.onBeforeLoad, this);
        this.getStore('account.operation.Special').on('beforeload', this.onBeforeLoadOperations, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('account.Special').load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.roId = this.params.realityObjectId;
    },

    onBeforeLoadOperations: function(store, operation) {
        operation.params.accountId = this.params.accountId;
    },

    setAccountId: function(id) {
        var grid = this.getEditWindow().down('specaccountopergrid'),
            store = grid.getStore();

        this.params.accountId = id;
        store.removeAll();
        grid.setDisabled(!id);

        if (id) {
            store.load();
        }
    }
});