/*
Данный аспект предназначен для смены статусов из карточки редатирования объекта по кнопке с выпадающим списком
{
    xtype:'statebuttonaspect',
    name:'statebutton',
    stateButtonSelector: '#stateButton123',
}
*/

Ext.define('B4.aspects.StateContextButton', {
    extend: 'B4.base.Aspect',

    alias: 'widget.statecontextbuttonaspect',

    requires: [
        'B4.view.StateHistory.Window',
        'B4.QuickMsg'
    ],

    emptyDescription: 'Перевод статуса из карточки',

    //Форма показа истории 
    windowHistorySelector: '#stateHistoryWindow',
    windowHistoryView: 'B4.view.StateHistory.Window',
    customWindowSelector: '',
    customWindowView: '',
    customWindowApplyButtonSelector: '',
    
    stateButtonSelector: null,

    constructor: function(config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents('transfersuccess');
    },

    init: function (controller) {
        var me = this,
            actions = {};
        this.callParent(arguments);

        actions[this.stateButtonSelector + ' menuitem'] = { 'click': { fn: me.onStateMenuItemClick, scope: me } };

        if (me.customWindowSelector && me.customWindowApplyButtonSelector) {
            actions[Ext.String.format('{0} {1}', me.customWindowSelector, me.customWindowApplyButtonSelector)] = { 'click': { fn: me.applyUserParams, scope: me } };
        }

        me.otherActions(actions);

        controller.control(actions);
    },

    otherActions: function () {
        //Данный метод служит для перекрытия в контроллерах где используется данный аспект
        //наслучай если потребуется к данному аспекту добавить дополнительные обработчики
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
        
        return  me.componentQuery(me.stateButtonSelector);
    },

    getEntityId: function () {
        var me = this;
        
        return me.getStateButton().entityId;
    },
    
    getCurrentState: function() {
        var me = this;

        return me.getStateButton().currentState;
    },
    
    getNewStateId: function () {
        var me = this;

        return me.getStateButton().newStateId;
    },

    setStateData: function (entityId, currentState) {
        var me = this,
            btn = me.getStateButton();
        
        if (currentState) {

            btn.entityId = entityId;
            btn.currentState = currentState;

            btn.stateStore = btn.stateStore || Ext.create('Ext.data.Store', {
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

            btn.stateStore.on('load', me.onStateStoreLoad, me, {single:true});
            
            btn.setText(currentState.Name);
            
            btn.stateStore.load({
                params: { currentStateId: me.getCurrentState().Id }
            });
        }
    },

    onStateStoreLoad: function (store) {
        var me = this,
            btnState = me.getStateButton();
        
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
        var me = this,
            curState = me.getCurrentState(),
            entityId = me.getEntityId(),
            stateButton = me.getStateButton(),
            historyWin;
        
        if (itemMenu.actionName == 'history') {
            historyWin = me.getWindowHistory();

            if (entityId > 0 && curState)
                historyWin.updateGrid(entityId, curState.TypeId);
            historyWin.show();
        } else {
            stateButton.newStateId = itemMenu.actionName;
            me.checkHasGkhStateRules();
            
        }
    },
    
    changeState: function (newStateId, userParams) {
        var me = this,
            curState = me.getCurrentState(),
            entityId = me.getEntityId(),         
            customWindow = me.componentQuery(me.customWindowSelector);

        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('StateChange', customWindow ? 'GkhStateTransfer' : 'StateTransfer'),
            params: {
                newStateId: newStateId,
                entityId: entityId,
                typeId: curState.TypeId,
                description: me.emptyDescription,
                userParams: userParams
            }
        }).next(function (response) {
            //десериализуем полученную строку
            var obj = Ext.JSON.decode(response.responseText);

            if (obj.success) {
                B4.QuickMsg.msg('Смена статуса', 'Статус переведен успешно', 'success');

                if (customWindow) {
                    customWindow.close();
                }

                me.fireEvent('transfersuccess', me, entityId, obj.newState);
            } else {
                Ext.Msg.alert('Ошибка!', obj.message);
            }
        }).error(function (result) {
            Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
        });
    },

    checkHasGkhStateRules: function () {
        var me = this,
            newStateId = me.getNewStateId(),
            entityId = me.getEntityId(),
            curState = me.getCurrentState();

        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('HasGkhStateRule', 'GkhStateTransfer'),
            params: {
                newStateId: newStateId,
                entityId: entityId,
                typeId: curState.TypeId
            }
        }).next(function (response) {
            var resp = Ext.JSON.decode(response.responseText);

            if (resp === true) {
                me.getCustomWindow();
            } else {
                me.changeState(newStateId);
            }
        }, me).error(function (result) {
            Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
        });
    },

    getCustomWindow: function () {
        var me = this,
           customWindow;

        if (me.customWindowSelector) {
            customWindow = me.componentQuery(me.customWindowSelector);

            if (customWindow && !customWindow.getBox().width) {
                customWindow = customWindow.destroy();
            }

            if (!customWindow) {

                customWindow = me.controller.getView(me.customWindowView).create(
                    {
                        constrain: true,
                        renderTo: B4.getBody().getActiveTab().getEl(),
                        closeAction: 'destroy',
                        ctxKey: me.controller.getCurrentContextKey()
                    });

                customWindow.show();
            }

            return customWindow;
        }
        
        return null;
    },

    applyUserParams: function () {
        var me = this,
            newStateId = me.getNewStateId(),   
            view = me.getCustomWindow(),
            form = view.getForm(),
            valid = form.isValid(),
            values = form.getValues(false, false, false, true),
            params = Ext.encode(values);

        if (valid) {
            me.changeState(newStateId, params);
        } else {
            Ext.Msg.alert('Ошибка!', 'Проверьте правильность заполнения формы!');
        }
    }
});