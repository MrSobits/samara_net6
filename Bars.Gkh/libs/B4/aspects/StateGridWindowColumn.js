/*
Данный аспект предназначен для смены статусов в гриде через Сокращенную или Стандартную форму
{
    xtype:'stategridwindowcolumnaspect',
    name:'transfer',
    gridSelector: '#grid',
    stateType: 'documents',
    isShortWindow: true //или false
}
*/

Ext.define('B4.aspects.StateGridWindowColumn', {
    extend: 'B4.base.Aspect',

    alias: 'widget.stategridwindowcolumnaspect',

    requires: [
        'B4.view.StateTransfer.ShortWindow',
        'B4.view.StateTransfer.StandartWindow',
        'B4.view.StateTransfer.StandartWindow',
        'B4.view.StateHistory.Window',
        'B4.QuickMsg'
    ],

    gridSelector: null,
    //указываем поле Id сущности со статусом
    entityIdProperty: 'Id',
    //указываем само поле Статуса 
    stateProperty: 'State',
    stateType: null,
    //Признак показывать стандартную большую форму перехода или маленькую
    //Окно для смены статуса
    isShortWindow: true,
    windowSelector: null,
    windowView: null,
    //Форма показа истории 
    windowHistorySelector: '#stateHistoryWindow',
    windowHistoryView: 'B4.view.StateHistory.Window',
    stateStore: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'transfersuccess'
        );
    },

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.gridSelector] = {
            'rowaction': { fn: this.rowAction, scope: this }
        };

        //Если при использовании аспекта не указали форму то
        //будет показана Коротка форма смены (без истории) и полная форма смены (с историей)
        if (!this.windowSelector && !this.windowView) {
            
            if (this.isShortWindow) {
                //Если показывать сокращенную форму смены статуса
                this.windowSelector = '#stateTransferShortWindow';
                this.windowView = 'B4.view.StateTransfer.ShortWindow';
            } else {
                this.windowSelector = '#stateTransferStandartWindow';
                this.windowView = 'B4.view.StateTransfer.StandartWindow';
            }    
        }
        
        //actions[this.windowSelector + ' b4savebutton'] = { 'click': { fn: this.onSaveTransfer, scope: this} };

        actions[this.windowSelector + ' b4closebutton'] = { 'click': { fn: this.onClose, scope: this} };

        actions[this.windowSelector + ' button'] = { 'click': { fn: this.onBtnClick, scope: this} };

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

        this.stateStore.on('beforeload', this.onStateStoreBeforeLoad, this);
        this.stateStore.on('load', this.onStateStoreLoad, this);

        controller.control(actions);
    },

    getGrid: function () {
        return Ext.ComponentQuery.query(this.gridSelector)[0];
    },

    getWindowHistory: function () {
        var window = Ext.ComponentQuery.query(this.windowHistorySelector)[0];

        if (!window) {
            window = this.controller.getView(this.windowHistoryView).create();
        }

        return window;
    },

    getWindow: function () {
        var window = Ext.ComponentQuery.query(this.windowSelector)[0];

        if (!window) {
            window = this.controller.getView(this.windowView).create();
        }

        return window;
    },

    rowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'statechange':
                this.updateData(record);
                break;
        }
    },

    updateData: function (record) {
        this.currentRecord = record;
        this.getWindow().setDisabled(true);
        this.stateStore.load();
    },

    onStateStoreBeforeLoad: function (store, operation) {
        if (this.currentRecord) {
            operation.params = {};
            var state = this.currentRecord.get(this.stateProperty);
            if (state)
                operation.params.currentStateId = state.Id;
        }
    },

    onStateStoreLoad: function (store, records) {
        var win = this.getWindow();
        win.updateControls(this.stateType, store);
        win.doLayout();
        win.setDisabled(false);
        win.show();
    },

    //Если на сокращенной форме (без истории) нажали на какую то кнопку то попадаем суда 
    onBtnClick: function (btn) {
        if (btn.stateType == this.stateType) {
            if (btn.actionName == 'history') {
                var historyWin = this.getWindowHistory();

                if (this.currentRecord) {
                    historyWin.updateGrid(this.currentRecord.get(this.entityIdProperty), this.currentRecord.get(this.stateProperty).TypeId);
                    historyWin.show();
                }
            }
            else {
                var description = this.getWindow().getDescription();
                this.changeState(btn.actionName, description);
            }
        }
    },

    changeState: function (newStateId, description) {
        var asp = this;

        asp.getWindow().setDisabled(true);
        
        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action('/StateTransfer/StateChange'),
            params: {
                newStateId: newStateId,
                entityId: asp.currentRecord.get(asp.entityIdProperty),
                description: description
            },
            success: function (response) {
                //десериализуем полученную строку
                var obj = Ext.JSON.decode(response.responseText);

                if (obj.success) {
                    B4.QuickMsg.msg('Смена статуса', 'Статус переведен успешно', 'success');

                    asp.currentRecord.set(asp.stateProperty, obj.newState);
                    asp.getWindow().close();
                    asp.fireEvent('transfersuccess', asp, asp.currentRecord);
                } else {
                    Ext.Msg.alert('Ошибка!', obj.message);
                    asp.getWindow().setDisabled(false);
                }
            },
            failure: function (result) {
                Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }
        });
    },

    onSaveTransfer: function () {
        this.getwindow().close();
    },

    onClose: function () {
        this.getwindow().close();
    }
});