Ext.define('B4.controller.administration.TariffDataIntegrationLog', {
    extend: 'B4.base.Controller',

    models: [
        'administration.TariffDataIntegrationLog'
    ],

    stores: [
        'administration.TariffDataIntegrationLog'
    ],

    views: [
        'administration.TariffDataIntegrationLogGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'administration.TariffDataIntegrationLogGrid',
    mainViewSelector: 'tariffDataIntegrationLogGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'tariffDataIntegrationLogGrid'
        },
        {
            ref: 'BeginDate',
            selector: 'tariffDataIntegrationLogGrid #beginDate'
        },
        {
            ref: 'EndDate',
            selector: 'tariffDataIntegrationLogGrid #endDate'
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'tariffDataIntegrationLogGrid': { 'afterrender': { fn: me.getViewData, scope: me } },
                'tariffDataIntegrationLogGrid b4updatebutton': { 'click': { fn: me.updatetariffDataIntegrationLogGrid, scope: me } }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);
    },

    getViewData: function (view) {
        var me = this,
            store = view.getStore();

        store.on('beforeload', me.onStoreBeforeLoad, me);
        store.load();
    },

    updatetariffDataIntegrationLogGrid: (btn) => btn.up('tariffDataIntegrationLogGrid').getStore().load(),

    onStoreBeforeLoad: function (store, options) {
        options.params.beginDate = this.getBeginDate().getValue();
        options.params.endDate = this.getEndDate().getValue();
    }
});