Ext.define('B4.aspects.EntityHistoryFieldWindow', {
    extend: 'B4.base.Aspect',

    alias: 'widget.entityhistoryfieldwindowaspect',

    requires: [
        'B4.mixins.MaskBody',
        'B4.ux.grid.EntityHistoryFieldWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    gridSelector: 'entityhistoryinfogrid',
    editWindowView: 'entityhistoryfieldwindow',

    _editWindow: null,

    getParentId: null,
    getEntityId: null,

    init: function (controller) {
        var asp = this,
            actions = {};
        asp.callParent(arguments);

        actions[asp.gridSelector] = {
            'render': function (grid) {
                grid.getStore().on('beforeload', asp.onBeforeHistoryLoad, asp);
            },
            'rowaction': {
                fn: asp.rowAction,
                scope: asp
            }
        };

        asp.otherActions(actions);

        controller.control(actions);
    },

    otherActions: function (action) { },

    onBeforeHistoryLoad: function (store, operation) {
        var asp = this;
        if (asp.getParentId) {
            operation.params.parentId = asp.getParentId();
        }
        if (asp.getEntityId) {
            operation.params.entityId = asp.getEntityId();
        }
    },

    rowAction: function (grid, action, record) {
        var asp = this;
        if (!grid || grid.isDestroyed) return;

        if (action === 'edit') {
            asp.showDetails(record.get('Id'));
        }
    },

    showDetails: function(id) {
        var asp = this,
            store = {};

        if (asp._editWindow && !asp._editWindow.getBox().width) {
            asp._editWindow = asp._editWindow.destroy();
        }

        if (!asp._editWindow) {
            asp._editWindow = Ext.create('B4.ux.grid.EntityHistoryFieldWindow',
                {
                    constrain: true,
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    closeAction: 'destroy',
                    ctxKey: asp.controller.getCurrentContextKey ? asp.controller.getCurrentContextKey() : ''
                });

            store = asp._editWindow.down('entityhistoryfieldgrid').getStore();
            store.on('beforeload', function (store, operation) {
                operation.params.id = id;
            }, asp);

            store.load();
            asp._editWindow.show();
        }
    }
});