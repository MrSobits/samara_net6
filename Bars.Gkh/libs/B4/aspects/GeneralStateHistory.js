Ext.define('B4.aspects.GeneralStateHistory', {
    extend: 'B4.base.Aspect',

    alias: 'widget.generalstatehistory',

    requires: [
        'B4.view.GeneralStateHistory.Window'
    ],

    refs: [
        {
            ref: 'historyGrid',
            selector: 'generalstatehistorywindow generalstatehistorygrid'
        }
    ],

    stateCode: null,
    gridSelector: null,
    entityIdProperty: 'Id',

    windowHistorySelector: 'generalstatehistorywentityIdindow',
    windowHistoryView: 'B4.view.GeneralStateHistory.Window',
    gridHistorySelector: 'generalstatehistorygrid',

    init: function (controller) {
        var actions = {},
            me = this;
        me.callParent(arguments);

        actions[me.gridSelector] = {
            'cellclickaction': { fn: me.onHistoryBtnClick, scope: me }
        };

        controller.control(actions);
    },

    onHistoryBtnClick: function (grid, e, action, record, store) {
        var me = this,
            historyWin = me.getWindowHistory();

        if (record) {
            historyWin.entityId = record.get(me.entityIdProperty);
            historyWin.stateCode = me.stateCode;
            historyWin.show();
            historyWin.down(me.gridHistorySelector).getStore().load();
        }
    },

    getGrid: function () {
        return Ext.ComponentQuery.query(this.gridSelector)[0];
    },

    getWindowHistory: function () {
        var me = this,
            window = Ext.ComponentQuery.query(me.windowHistorySelector)[0];

        if (!window) {
            window = me.controller.getView(me.windowHistoryView).create();
        }

        return window;
    }
});