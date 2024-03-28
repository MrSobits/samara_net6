Ext.define('B4.controller.regoperator.AccountHistory', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.view.regoperator.accounthistory.Main',
        'B4.enums.regop.TypeCalcAccount'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'regoperator.accounthistory.Main',
    mainViewSelector: 'accounthistoryMain',

    init: function () {
        var me = this,
            actions = {};

        actions['accounthistorySpecialAccountGrid'] = {
            'render': function (grid) {
                var store = grid.getStore();
                store.on('beforeload', me.beforeSpecialStoreLoad, me);
                store.load();
            }
        }
        actions['accounthistoryRegopAccountGrid'] = {
            'render': function (grid) {
                var store = grid.getStore();
                store.on('beforeload', me.beforeRegopStoreLoad, me);
                store.load();
            }
        }
        actions['b4selectfield[name=CalcAccount]'] = {
            'beforeload': function (field, operation, store) {
                operation.params.regopId = me.params.get('Id');
            },
            'change': function() {
                me.getMainView().down('accounthistoryRegopAccountGrid').getStore().load();
            }
        }

        me.control(actions);
        me.callParent(arguments);
    },

    beforeSpecialStoreLoad: function (store, operation) {
        var me = this;
        operation.params.regopId = me.params.get('Id');
        operation.params.typeAccount = B4.enums.regop.TypeCalcAccount.Special;
    },

    beforeRegopStoreLoad: function (store, operation) {
        var me = this;
        operation.params.regopId = me.params.get('Id');
        operation.params.accId = me.getMainView().down('b4selectfield[name=CalcAccount]').getValue();
        operation.params.typeAccount = B4.enums.regop.TypeCalcAccount.Regoperator;
    }
});