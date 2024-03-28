Ext.define('B4.controller.administration.EmailMessage', {
    extend: 'B4.base.Controller',

    models: [
        'administration.EmailMessage'
    ],

    stores: [
        'administration.EmailMessage'
    ],

    views: [
        'administration.EmailMessageGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'administration.EmailMessageGrid',
    mainViewSelector: 'emailMessageGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'emailMessageGrid'
        },
        {
            ref: 'BeginDate',
            selector: 'emailMessageGrid #beginDate'
        },
        {
            ref: 'EndDate',
            selector: 'emailMessageGrid #endDate'
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'emailMessageGrid': { 'afterrender': { fn: me.getViewData, scope: me } },
                'emailMessageGrid b4updatebutton': { 'click': { fn: me.updateMessageGrid, scope: me } }
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

    updateMessageGrid: (btn) => btn.up('emailMessageGrid').getStore().load(),

    onStoreBeforeLoad: function (store, options) {
        options.params.beginDate = this.getBeginDate().getValue();
        options.params.endDate = this.getEndDate().getValue();
    }
});