Ext.define('B4.aspects.StateContextButtonMenu', {
    extend: 'B4.base.Aspect',

    alias: 'widget.statecontextbuttonmenu',

    requires: [
        'B4.view.StateHistory.Window',
        'B4.view.State.StateMenu'
    ],

    stateType: null,
    buttonSelector: null,
    entityIdProperty: 'Id',
    stateProperty: 'State',
    windowHistorySelector: 'stateHistoryWindow',
    windowHistoryView: 'B4.view.StateHistory.Window',
    contextMenuView: 'B4.view.State.StateMenu',
    menuSelector: 'realityobjStateContextButtonMenu',

    init: function (controller) {
        var actions = {},
            me = this;
        me.callParent(arguments);

        actions[me.buttonSelector] = {
            'click': { fn: me.onbuttonClickAction, scope: me }
        };

        actions['#' + me.menuSelector + ' button'] = {
            'click': { fn: me.onSaveBtnClick, scope: me }
        };

        actions['#' + me.menuSelector + ' [actionName=history]'] = {
            'click': { fn: me.onHistoryBtnClick, scope: me }
        };

        controller.control(actions);
    },

    getButton: function () {
        var me = this;

        return me.buttonSelector ? me.componentQuery(me.buttonSelector) : null;
    },

    getContextMenu: function () {
        var me = this,
            contextMenu = me.componentQuery('#' + me.menuSelector);

        if (!contextMenu) {
            contextMenu = me.controller.getView(me.contextMenuView).create({ store: 'B4.store.StateContext', itemId: me.menuSelector, ctxKey: me.controller.getCurrentContextKey() });
        }

        return contextMenu;
    },

    getWindowHistory: function () {
        var me = this,
            window = me.componentQuery('#' + me.windowHistorySelector);

        if (!window) {
            window = me.controller.getView(me.windowHistoryView).create({ itemId: me.windowHistorySelector, ctxKey: me.controller.getCurrentContextKey() });
        }

        return window;
    },

    onbuttonClickAction: function (button, e) {
        var me = this,
            menu = me.getContextMenu(),
            record = me.controller.getMainView().getRecord(),
            position = button.getPosition();

        // Смещение по Y на высоту кнопки
        position[1] += button.getHeight();

        e.stopEvent();
        menu.updateData(record, position, me.stateType, me.stateProperty);
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
            historyWin.updateGrid(currentRecord.get(me.entityIdProperty),
                (currentRecord.get(me.stateProperty)
                    ? currentRecord.get(me.stateProperty).TypeId
                    : undefined));

            historyWin.show();
        }
    },

    changeState: function (newStateId, description, entityId, btn, timeout) {
        var me = this,
            obj,
            menu = me.getContextMenu();

        timeout = timeout || 3 * 60 * 1000;

        me.controller.mask('Смена статуса', me.controller.getMainComponent());

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

                obj = Ext.JSON.decode(response.responseText);
                if (obj.success) {
                    menu.hide();
                    menu.currentRecord.set(me.stateProperty, obj.newState);

                    me.setStateData(entityId, obj.newState);

                    Ext.Msg.alert('Смена статуса', obj.message);
                    me.fireEvent('transfersuccess', me, entityId, obj.newState);
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
    },

    setStateData: function (entityId, newState) {
        var me = this,
            btn = me.getButton();

        if (btn && newState) {

            btn.entityId = entityId;
            btn.currentState = newState;

            btn.setText(newState.Name);
        }
    }

});