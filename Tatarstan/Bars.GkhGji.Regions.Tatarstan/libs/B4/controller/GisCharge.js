Ext.define('B4.controller.GisCharge', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['GisCharge'],
    stores: ['GisCharge'],

    views: ['gischarge.Grid', 'gischarge.JsonWindow', 'gischarge.LogWindow'],

    aspects: [],

    mainView: 'gischarge.Grid',
    mainViewSelector: 'gischargegrid',

    init: function() {
        var me = this;

        me.control({
            'gischargegrid button[action=SendNow]': {
                'click': function () {
                    Ext.Msg.confirm('Отправка!', 'Выполнить отправку начислений?', function (result) {
                        if (result == 'yes') {
                            me.mask('Отправка...', me.getMainView());
                            B4.Ajax.request({
                                url: B4.Url.action('Send', 'GisCharge'),
                                timeout: 2 * 60 * 60 * 1000
                            }).next(function (response) {
                                var obj = Ext.decode(response.responseText);

                                if (Ext.isString(obj)) {
                                    Ext.Msg.alert('Успешно', obj);
                                } else {
                                    Ext.Msg.alert('Успешно', 'Отправка начислений выполнена успешно');
                                }

                                me.unmask();
                            }).error(function (e) {
                                Ext.Msg.alert('Результат', e.message);
                                me.unmask();
                            });
                        }
                    }, me);
                }
            },
            'gischargegrid button[action=UploadNow]': {
                'click': function () {
                    Ext.Msg.confirm('Загрузка!', 'Выполнить загрузку начислений?', function (result) {
                        if (result == 'yes') {
                            me.mask('Загрузка...', me.getMainView());
                            B4.Ajax.request({
                                url: B4.Url.action('Upload', 'GisCharge'),
                                timeout: 2 * 60 * 60 * 1000
                            }).next(function (response) {
                                var obj = Ext.decode(response.responseText);

                                if (Ext.isString(obj)) {
                                    Ext.Msg.alert('Успешно', obj);
                                } else {
                                    Ext.Msg.alert('Успешно', 'Загрузка начислений выполнена успешно');
                                }

                                me.unmask();
                            }).error(function (e) {
                                Ext.Msg.alert('Результат', e.message);
                                me.unmask();
                            });
                        }
                    }, me);
                }
            },
            'gischargegrid actioncolumn[name=showJson]': {
                'click': function (gridView, rowIndex, colIndex, el, e, rec) {
                    var json = JSON.stringify(rec.get('JsonObject')),
                        win;

                    if (!Ext.isEmpty(json)) {
                        win = Ext.widget('gischargejsonwindow', {
                            renderTo: B4.getBody().getActiveTab().getEl()
                        });
                        win.setTitle('Json-объект отправки документа № ' + rec.get('DocumentNumber')
                            + ' от ' + rec.get('DocumentDate').toLocaleString().split(', ')[0]);
                        win.down('[name=JsonObject]').setValue(json);
                        win.show();
                    }
                }
            },
            'gischargegrid actioncolumn[name=showLog]': {
                'click': function (gridView, rowIndex, colIndex, el, e, rec) {
                    var log = rec.get('SendLog'),
                        win;

                    if (!Ext.isEmpty(log)) {
                        win = Ext.widget('gischargelogwindow', {
                            renderTo: B4.getBody().getActiveTab().getEl()
                        });

                        win.setTitle('Лог отправки документа № ' + rec.get('DocumentNumber')
                            + ' от ' + rec.get('DocumentDate').toLocaleString().split(', ')[0]);
                        win.down('[name=Log]').setValue(log);
                        win.show();
                    }
                }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('gischargegrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});