Ext.define('B4.controller.SuspenseAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.enums.AccountType'
    ],

    models: [
        'account.Operation',
        'account.BankStatement'
    ],
    
    stores: [
        'account.Operation',
        'account.BankStatement',
        'dict.AccountOperationNoPaging'
    ],
    views: [
        'suspenseaccount.OperationGrid',
        'suspenseaccount.OperationEditWindow',
        'suspenseaccount.BankStatGrid',
        'suspenseaccount.BankStatEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        mixins: 'B4.mixins.Context'
    },

    mainView: 'suspenseaccount.BankStatGrid',
    mainViewSelector: 'suspenseaccountbankstatgrid',

    refs: [
        { ref: 'mainView', selector: 'suspenseaccountbankstatgrid' }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'suspenseAccBankStatGridWindow',
            gridSelector: 'suspenseaccountbankstatgrid',
            editFormSelector: 'suspenseaccountbankstateditwin',
            storeName: 'account.BankStatement',
            modelName: 'account.BankStatement',
            editWindowView: 'suspenseaccount.BankStatEditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            otherActions: function (actions) {
                actions['suspenseaccountbankstateditwin b4selectfield[name="BankAccount"]'] = { 'beforeload': { fn: this.beforeLoadAccount, scope: this } };
                actions['suspenseaccountbankstateditwin b4selectfield[name="LongTermPrObject"]'] = { 'change': { fn: this.onChangeLongTermObj, scope: this } };
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    var store,
                        grid = asp.getForm().down('suspenseaccountoperationgrid');

                    if (record.getId() > 0) {
                        grid.setDisabled(false);

                        store = grid.getStore();

                        store.clearFilter(true);
                        store.filter('bankStatId', record.getId());

                        asp.controller.bankStatId = record.getId();
                    } else {
                        grid.setDisabled(true);
                    }
                },
                savesuccess: function (asp) {
                    var grid = asp.getForm().down('suspenseaccountoperationgrid');
                    grid.setDisabled(false);

                }
            },
            onChangeLongTermObj: function (fld) {
                var form = fld.up(),
                    accountField = form.down('b4selectfield[name="BankAccount"]');
                accountField.setValue(null);
            },
            beforeLoadAccount: function (store, operation) {
                var form = this.getForm(),
                    longTermObjField = form.down('b4selectfield[name="LongTermPrObject"]');
                
                operation.params = {};
                operation.params.longTermObjId = longTermObjField.getValue();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'suspenseAccOperationGridWindow',
            gridSelector: 'suspenseaccountoperationgrid',
            editFormSelector: 'suspenseaccountoperationeditwin',
            storeName: 'account.Operation',
            modelName: 'account.Operation',
            editWindowView: 'suspenseaccount.OperationEditWindow',
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                beforesave: function (asp, rec) {
                    if (asp.controller.bankStatId > 0) {
                        rec.set('BankStatement', asp.controller.bankStatId);
                    }
                    
                    return true;
                },
                savesuccess: function () {
                    var me = this,
                        model = me.controller.getModel('account.BankStatement');

                    model.load(me.controller.bankStatId, {
                        success: function (rec) {
                            me.controller.getAspect('suspenseAccBankStatGridWindow').setFormData(rec);
                        },
                        scope: this
                    });
                }
            }
        }
    ],
    
    index: function () {
        var view = this.getMainView() || Ext.widget('suspenseaccountbankstatgrid'),
            store = view.getStore();
        this.bindContext(view);
        this.application.deployView(view);

        store.clearFilter(true);
        store.filter('IsSuspenseAcc', true);
    }
});