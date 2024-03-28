Ext.define('B4.aspects.Import', {
    extend: 'B4.base.Aspect',

    alias: 'widget.importaspect',
    requires: ['B4.view.import.BaseWindow'],

    importBtnSelector: null,

    controllerName: null,

    actionName: null,

    importWindowCls: null,

    importWindowSelector: null,

    _importWin: null,

    init: function(controller) {
        var actions = {};

        this.importWindowCls = this.importWindowCls || 'B4.view.import.BaseWindow';
        this.importWindowSelector = this.importWindowSelector || 'baseimportwin';
        this.actionName = this.actionName || 'Import';

        this.callParent(arguments);

        actions[this.importBtnSelector] = {
            'click': {
                fn: this.onImportBtnClick,
                scope: this
            }
        };

        actions[Ext.String.format("{0} button[actionName=submit]", this.importWindowSelector)] = {
            'click': {
                fn: this.onSubmitBtnClick,
                scope: this
            }
        };

        controller.control(actions);
    },

    onImportBtnClick: function(btn) {
        var me = this;

        if (!me._importWin) {
            me._importWin = Ext.create(me.importWindowCls, {
                closeAction: 'hide'
            });
        }

        me._importWin.show();
    },

    onSubmitBtnClick: function (btn) {
        var me = this,
            win = btn.up('window'),
            form = win.down('form');
        //TODO: loading feedback
        form.submit({
            url: B4.Url.action(me.actionName, me.controllerName),
            success: function () {
                B4.QuickMsg.msg('Импорт', 'Импорт прошел успешно', 'success');
                win.close();
            },
            failure: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText);
                B4.QuickMsg.msg('Импорт', json.message, 'error');
            }
        });
    }

});