Ext.define('B4.controller.UnconfirmedPayments', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    models: ['B4.model.UnconfirmedPayments'],
    stores: ['B4.store.UnconfirmedPayments'],

    views: [
        'unconfirmedpayments.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'unconfirmedpayments.Grid',
    mainViewSelector: 'unconfirmedpaymentsgrid',

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'unconfirmedpaymentsgrid',
            storeName: 'UnconfirmedPayments',
            modelName: 'UnconfirmedPayments'
        }
    ],

    init: function () {
        var me = this;
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('unconfirmedpaymentsgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});