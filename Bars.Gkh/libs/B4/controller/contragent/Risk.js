Ext.define('B4.controller.contragent.Risk', {
    extend: 'B4.controller.MenuItemController',

    requires: [
    ],

    models: [
        'contragent.Risk'
    ],
    
    views: [
        'contragent.RiskGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentriskgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.contragent.Navi',

    aspects: [
        
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentriskgrid'),
            store = view.getStore();

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        if (store.getCount() === 0) {
            store.on('beforeload', this.onBeforeLoad, this);
            store.load();
        }
    },

    onBeforeLoad: function (_store, operation) {
        var me = this;
        operation.params.contragentId = me.getContextValue(me.getMainComponent(), 'contragentId');
    }
});