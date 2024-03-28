Ext.define('B4.controller.SaldoRefresh', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.saldorefresh.Grid',
        'B4.view.saldorefresh.AddWindow',
        'B4.model.SaldoRefresh',
        'B4.store.SaldoRefresh'
    ],
    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['B4.model.SaldoRefresh'],
    stores: ['B4.store.SaldoRefresh'],
    views: [
        'B4.view.saldorefresh.Grid',
        'B4.view.saldorefresh.AddWindow'
    ],

    mainView: 'saldorefresh.Grid',
    mainViewSelector: 'saldorefreshgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'saldorefreshgrid'
        },
        {
            ref: 'AddWindow',
            selector: 'saldorefreshaddwindow'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'saldoGridWindowAspect',
            gridSelector: 'saldorefreshgrid',
            editFormSelector: 'saldorefreshaddwindow',
            storeName: 'B4.store.SaldoRefresh',
            modelName: 'B4.model.SaldoRefresh',
            editWindowView: 'saldorefresh.AddWindow'
        }
    ],

    btnDeleteAll: function () {
        var me = this;

        Ext.Msg.confirm('Внимание', 'Вы уверены, что хотите удалить записи? ', function (confirmationResult) {
            if (confirmationResult == 'yes') {
                me.getMainView().mask('Удаление...');
                B4.Ajax.request({
                    url: B4.Url.action('RemoveAll', 'SaldoRefreshAction'),
                    timeout: 999999
                })
                    .next(function () {
                        me.getMainView().getStore().load();
                        me.getMainView().unmask();
                        Ext.Msg.alert('Успешно!', 'Записи удалены');
                    })
                    .error(function () {
                        me.getMainView().unmask();
                        Ext.Msg.alert('Ошибка!', 'Не удалось удалить записи');
                    });
            }
        });
    },

    btnSaldoRefresh: function () {
        var me = this;
        Ext.Msg.confirm('Внимание', 'Подтвердите действие. Продолжить?', function (confirmationResult) {
            if (confirmationResult == 'yes') {
                me.getMainView().mask('Обновление...');
                B4.Ajax.request({
                    url: B4.Url.action('RefreshSaldo', 'SaldoRefreshAction'),
                    timeout: 999999
                })
                    .next(function () {
                        me.getMainView().getStore().load();
                        me.getMainView().unmask();
                        Ext.Msg.alert('Выполнено!', 'Успешно');
                    })
                    .error(function () {
                        me.getMainView().unmask();
                        Ext.Msg.alert('Ошибка!', 'Не удалось обновить записи');
                    });
            }
        });
    },

    init: function () {
        var me = this;

        me.control({
            'saldorefreshgrid #deleteAll': { 'click': { fn: me.btnDeleteAll, scope: me } },
            'saldorefreshgrid #updSaldo': { 'click': { fn: me.btnSaldoRefresh, scope: me } },
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('saldorefreshgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});