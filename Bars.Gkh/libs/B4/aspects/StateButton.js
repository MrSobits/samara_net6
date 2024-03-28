/*
Данный аспект предназначен для смены статусов из карточки редатирования объекта по кнопке с выпадающим списком
{
    xtype:'statebuttonaspect',
    name:'statebutton',
    stateButtonSelector: '#stateButton123',
}
*/

Ext.define('B4.aspects.StateButton', {
    extend: 'B4.base.Aspect',

    alias: 'widget.statebuttonaspect',

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

    constructor: function(config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents('transfersuccess');
    },

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.stateButtonSelector + ' menuitem'] = { 'click': { fn: this.onStateMenuItemClick, scope: this } };

        this.stateStore = Ext.create('Ext.data.Store', {
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

        //this.stateStore.on('beforeload', this.onStateStoreBeforeLoad, this);
        this.stateStore.on('load', this.onStateStoreLoad, this);

        controller.control(actions);
    },

    getWindowHistory: function() {
        var window = this.componentQuery(this.windowHistorySelector);

        if (!window) {
            window = this.controller.getView(this.windowHistoryView).create();
        }

        return window;
    },

    getStateButton: function() {
        return this.componentQuery(this.stateButtonSelector);
    },

    setStateData: function (entityId, currentState) {
        this.entityId = entityId;
        this.currentState = currentState;
        if (currentState)
            this.getStateButton().setText(currentState.Name);

        if (this.currentState) {
            this.stateStore.load({
                params: { currentStateId: this.currentState.Id }
            });
        }
    },

    onStateStoreBeforeLoad: function (store, operation) {
        if (this.currentState) {
            operation.params = operation.params || {};
            operation.params.currentStateId = this.currentState.Id;
        }
    },

    onStateStoreLoad: function(store) {
        var btnState = this.getStateButton();
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

    onStateMenuItemClick: function(itemMenu) {
        if (itemMenu.actionName == 'history') {
            var historyWin = this.getWindowHistory();

            if (this.entityId > 0 && this.currentState)
                historyWin.updateGrid(this.entityId, this.currentState.TypeId);
            historyWin.show();
        } else {
            this.changeState(itemMenu.actionName);
        }
    },

    changeState: function(newStateId) {
        var asp = this,
            id = asp.entityId;

        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('/StateTransfer/StateChange'),
            params: {
                newStateId: newStateId,
                entityId: id,
                description: asp.emptyDescription
            }
        }).next(function(response) {
            //десериализуем полученную строку
            var obj = Ext.JSON.decode(response.responseText);

            if (obj.success) {
                B4.QuickMsg.msg('Смена статуса', 'Статус переведен успешно', 'success');

                asp.fireEvent('transfersuccess', asp, asp.entityId, obj.newState);
            } else {
                Ext.Msg.alert('Ошибка!', obj.message);
            }
        }).error(function(result) {
            Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
        });
    }
});