// #warning ВЫПИЛИТЬ после обновления в B4

Ext.define('B4.aspects.StateContextMenu', {
    extend: 'B4.base.Aspect',

    alias: 'widget.b4_state_contextmenu',

    requires: [
        'B4.view.StateHistory.Window',
        'B4.view.State.StateMenu'
    ],

    stateType: null,
    gridSelector: null,
    entityIdProperty: 'Id',
    stateProperty: 'State',
    windowHistorySelector: '#stateHistoryWindow',
    windowHistoryView: 'B4.view.StateHistory.Window',
    contextMenuView: 'B4.view.State.StateMenu',
    menuSelector: undefined,

    init: function (controller) {
        var actions = {},
            me = this;
        me.callParent(arguments);

        actions[me.gridSelector] = {
            'cellclickaction': { fn: me.cellClickAction, scope: me }
        };

        actions['#' + me.menuSelector + ' button'] = {
            'click': { fn: me.onSaveBtnClick, scope: me }
        };

        actions['#' + me.menuSelector + ' [actionName=history]'] = {
            'click': { fn: me.onHistoryBtnClick, scope: me }
        };

        controller.control(actions);
    },

    getGrid: function () {
        return Ext.ComponentQuery.query(this.gridSelector)[0];
    },

    getContextMenu: function () {
        var me = this,
            contextMenu = Ext.ComponentQuery.query('#' + me.menuSelector)[0];

        if (!contextMenu) {
            contextMenu = me.controller.getView(me.contextMenuView).create({ store: 'B4.store.StateContext', itemId: me.menuSelector });
        }

        return contextMenu;
    },

    getWindowHistory: function () {
        var me = this,
            window = Ext.ComponentQuery.query(me.windowHistorySelector)[0];

        if (!window) {
            window = me.controller.getView(me.windowHistoryView).create();
        }

        return window;
    },

    cellClickAction: function (grid, e, action, record, store) {
        switch (action.toLowerCase()) {
            case 'statechange':
                e.stopEvent();
                var menu = this.getContextMenu();
                this.currentRecord = record;
                menu.updateData(record, e.xy, this.stateType, this.stateProperty);
                break;
        }
    },

    onSaveBtnClick: function (btn, e) {
        var me = this,
            menu = me.getContextMenu(),
            desc = menu.getFocusEl().menu.down('textareafield').getValue(),
            stateId = menu.getFocusEl().did,
            entityId = menu.currentRecord.get(me.entityIdProperty);

        btn.setDisabled(true);
        me.changeState(stateId, desc, entityId, btn);
    },

    onHistoryBtnClick: function (btn, e) {
        var me = this,
            historyWin = me.getWindowHistory(),
            currentRecord = me.getContextMenu().currentRecord;

        if (currentRecord) {
            historyWin.updateGrid(currentRecord.get(me.entityIdProperty), (currentRecord.get(me.stateProperty) ? currentRecord.get(me.stateProperty).TypeId : undefined));
            historyWin.show();
        }
    },

    changeState: function (newStateId, description, entityId, btn, timeout) {
        var me = this,
            obj,
            menu = me.getContextMenu();

        timeout = timeout || 3 * 60 * 1000;

        me.controller.mask("Смена статуса", me.getGrid());

        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action('/StateTransfer/StateChange'),
            params: {
                newStateId: newStateId,
                entityId: entityId,
                description: description
            },
            timeout: timeout,
            success: function (response) {
                me.controller.unmask();
                //десериализуем полученную строку
                obj = Ext.JSON.decode(response.responseText);
                if (obj.success) {
                    menu.hide();
                    menu.currentRecord.set(me.stateProperty, obj.newState);
                    me.getGrid().getStore().load();
                    Ext.Msg.alert('Смена статуса', obj.message);
                    me.fireEvent('transfersuccess', me, menu.currentRecord);
                } else {
                    Ext.Msg.alert('Ошибка', obj.message);
                }
                btn.setDisabled(false);
            },
            failure: function (response) {
                me.controller.unmask();

                if (response.timedout) {
                    Ext.Msg.alert('Ошибка', 'Превышено время ожидания ответа от сервера');
                } else {
                    Ext.Msg.alert('Ошибка', 'Не удалось сменить статус из-за непредвиденной ошибки');
                }

                btn.setDisabled(false);
            }
        });
    }

});