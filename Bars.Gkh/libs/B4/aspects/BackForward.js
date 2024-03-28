Ext.define('B4.aspects.BackForward', {
    extend: 'B4.base.Aspect',

    requires: ['B4.Ajax'],

    alias: 'widget.backforwardaspect',

    /**
     * Селектор панели, для которой осуществляется переход вперед-назад
     */
    panelSelector: null,

    /**
     * Серверный контроллер, у которого есть методы Next и Previous, 
     * возвращающие Id предыдущей и следующей сущности, соответственно.
     */
    backForwardController: null,

    init: function (controller) {
        var me = this, actions = {};

        if (!me.panelSelector) {
            Ext.Error.raise("Селектор панели является обязательным параметром.");
        }

        actions[me.panelSelector + " backforwardbutton button[direction]"] = {
            'click': {
                fn: me.onButtonClick,
                scope: me
            }
        };

        controller.control(actions);
        me.callParent(arguments);
    },

    onButtonClick: function (btn) {
        var me = this,
            controller = me.controller;

        if (!Ext.isFunction(controller.getCurrent)) {
            Ext.Error.raise("Функция getCurrent должна быть реализована в контроллере");
        }

        if (!Ext.isFunction(controller.hasChanges)) {
            Ext.Error.raise("Функция hasChanges должна быть реализована в контроллере");
        }

        if (!controller.hasChanges()) {
            me.goTo(btn.direction, controller.getCurrent());
        } else {
            Ext.Msg.confirm('Внимание', 'Некоторые данные не сохранены. Продолжить?', function (btnId) {
                if (btnId === "yes") {
                    me.goTo(btn.direction, controller.getCurrent());
                }
            });
        }
    },

    goTo: function (direction, current) {
        var me = this;

        B4.Ajax.request({
            url: B4.Url.action(direction, me.backForwardController),
            params: {
                current: current
            }
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);
            if (json.success) {
                if (!json.data) {
                    Ext.Msg.alert('Внимание', 'Текущий объект ' + (direction == 'back' ? 'первый' : 'последний'));
                } else {
                    me.disposeCurrent();
                    me.openNew(json.data);
                }

            } else {
                me.showErrorMsg();
            }
        }).error(function (response) {
            me.showErrorMsg();
        });
    },

    showErrorMsg: function () {
        Ext.Msg.alert('Ошибка!', 'Ошибка при получении данных!');
    },

    disposeCurrent: function () {
        Ext.ComponentQuery.query('b4tabpanel')[0].getActiveTab().close();
    },

    openNew: function (newObj) {
        this.controller.application.redirectTo("realityobjectedit/" + newObj.Id);
    }
});