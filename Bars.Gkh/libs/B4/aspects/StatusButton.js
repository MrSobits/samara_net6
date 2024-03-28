/*
Данный аспект предназначен для смены статусов из карточки редатирования объекта по кнопке с выпадающим списком
{
    xtype:'statusbuttonaspect',
    name:'statebutton',
    stateButtonSelector: '#stateButton123',
}
*/

Ext.define('B4.aspects.StatusButton', {
    extend: 'B4.base.Aspect',

    alias: 'widget.statusbuttonaspect',

    requires: [
        'B4.view.StateHistory.Window',
        'B4.QuickMsg'
    ],

    gridSelector: null,
    emptyDescription: 'Перевод статуса из карточки',
    //entityId и currentState проставляются после вызова метода setStateData
    entityId: 0,
    currentState: null,
    //Форма показа истории 
    windowHistorySelector: '#stateHistoryWindow',
    windowHistoryView: 'B4.view.StateHistory.Window',

    stateStore: null,

    stateButtonSelector: null,

    constructor: function (config) {
        var me = this;
        
        Ext.apply(me, config);
        me.callParent(arguments);

        me.addEvents('transfersuccess');
    },

    init: function (controller) {
        var me = this,
            actions = {};
        
        me.callParent(arguments);

        actions[me.stateButtonSelector + ' menuitem'] = {
             'click': { fn: me.onStateMenuItemClick, scope: me }
        };

        me.stateStore = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: ['Id', 'Name'],
            proxy: {
                autoLoad: false,
                type: 'ajax',
                url: B4.Url.action('/StateTransfer/GetStates'),
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        me.stateStore.on('load', me.onStateStoreLoad, me);

        controller.control(actions);
    },

    getWindowHistory: function() {
        var me = this,
            window = me.componentQuery(me.windowHistorySelector);

        if (!window) {
            window = me.controller.getView(me.windowHistoryView).create();
        }

        return window;
    },

    getStateButton: function () {
        var me = this;
        return me.componentQuery(me.stateButtonSelector);
    },

    setStateData: function (entityId, currentState) {
        var me = this;
        
        me.entityId = entityId;
        me.currentState = currentState;

        if (currentState) {
            me.getStateButton().setText(currentState.Name);
        }
        
        if (me.currentState) {
            me.stateStore.load({
                params: { currentStateId: me.currentState.Id }
            });
        }
    },

    onStateStoreBeforeLoad: function (store, operation) {
        var me = this;
        
        if (me.currentState) {
            operation.params = operation.params || {};
            operation.params.currentStateId = me.currentState.Id;
        }
    },

    onStateStoreLoad: function(store) {
        var me = this,
            btnState = me.componentQuery(me.stateButtonSelector);
        if (btnState) {
            btnState.menu.removeAll();

            store.each(function(record) {
                btnState.menu.add({
                    xtype: 'menuitem',
                    text: record.get('Name'),
                    textAlign: 'left',
                    actionName: record.get('Id')
                });
            });

            btnState.menu.add({
                xtype: 'menuitem',
                text: 'История изменений',
                textAlign: 'left',
                actionName: 'history'
            });
        }
    },

    onStateMenuItemClick: function (itemMenu) {
        var me = this;
        if (itemMenu.actionName == 'history') {
            var historyWin = me.getWindowHistory();

            if (me.entityId > 0 && me.currentState)
                historyWin.updateGrid(me.entityId, me.currentState.TypeId);
            historyWin.show();
        } else {
            me.changeState(itemMenu.actionName);
        }
    },

    changeState: function(newStateId) {
        var me = this,
            id = me.entityId;

        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('/StateTransfer/StateChange'),
            params: {
                newStateId: newStateId,
                entityId: id,
                description: me.emptyDescription
            }
        }).next(function(response) {
            var obj = Ext.JSON.decode(response.responseText);

            if (obj.success) {
                B4.QuickMsg.msg('Смена статуса', 'Статус переведен успешно', 'success');

                me.fireEvent('transfersuccess', me, me.entityId, obj.newState);
            } else {
                Ext.Msg.alert('Ошибка!', obj.message);
            }
        }).error(function(result) {
            Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
        });
    }
});