Ext.define('B4.controller.regop.NotDefinedPayment', {
    extend: 'B4.base.Controller',

    requires: [
    ],

    stores: [
        'regop.ImportedPayment'
    ],

    views: [
        'regop.notdefinedpayment.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'regop.notdefinedpayment.Grid',
    mainViewSelector: 'notdefinedpaymentgrid',

    aspects: [
    ],

    init: function () {
        var me = this;
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('notdefinedpaymentgrid');

        me.bindContext(view);
        me.application.deployView(view);

        var store = view.getStore();
        store.clearFilter(true);
        store.filter([
            { property: 'showNotDefined', value: true },
            { property: 'showNotDeleted', value: true },
            { property: 'showNotDistributed', value: true }
        ]);
    }
});