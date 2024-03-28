Ext.define('B4.aspects.DocumentClwAccountDetailAspect', {
    extend: 'B4.base.Aspect',

    alias: 'widget.documentclwaccountdetailaspect',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    panelSelector: '',
    autoLoad: true,

    getDocumentId: function() {
    },

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[me.getGridSelector()] = { 'afterrender': { fn: me.onGridRender, scope: me } };

        controller.control(actions);
    },

    getGridSelector: function() {
        var me = this,
            selector = me.panelSelector + ' documentclwaccountdetailgrid';

        return selector;
    },

    load: function() {
        var me = this,
            grid = me.componentQuery(me.getGridSelector()),
            store;
        if (grid) {
            store = grid.getStore();
            if (store && me.isSubscibed(store)) {
                me.onGridRender(grid);
            }
            grid.getStore().load();
        }
    },

    isSubscibed: function(store) {
        var count = store.events.beforeload.listeners.filter(function(listener) {
                return listener.ev.name === 'onGridBeforeLoad';
        }).length;

        return count !== 0;
    },

    onGridRender: function (grid) {
        var me = this;
        if (grid) {
            grid.getStore().on('beforeload', me.onGridBeforeLoad, me);
            if (me.autoLoad) {
                grid.getStore().load();
            }
        }
    },

    onGridBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.id = me.getDocumentId();
    }
});