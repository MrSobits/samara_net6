Ext.define('B4.controller.import.chesimport.Payments', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.view.import.chesimport.payments.Panel',
        'B4.enums.regop.FileType'
    ],

    mixins: { 
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody' 
    },

    mainView: 'import.chesimport.payments.Panel',
    mainViewSelector: 'chesimportpaymentspanel',

    init: function() {
        var me = this,
            actions = {
                'chesimportpaymentspanel': {
                    'beforerender': {
                        fn: me.initEvents,
                        scope: me
                    }
                },
                'chesimportpaymentsunassignedgrid': {
                    'afterrender': {
                        fn: me.getViewData,
                        scope: me
                    }
                },
                'chesimportpaymentssummarygrid': {
                    'afterrender': {
                        fn: me.getViewData,
                        scope: me
                    }
                },
                'chesimportpaymentsassignedgrid': {
                    'afterrender': {
                        fn: me.getViewData,
                        scope: me
                    }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (id, day) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget(me.mainViewSelector, {
                title: 'Оплаты' + (Ext.isNumber(Number(day)) ? (', день ' + day) : ''),
            });
        }

        me.setContextValue(view, 'periodId', id);
        me.setContextValue(view, 'day', day);

        me.bindContext(view);
        me.application.deployView(view, 'chesPeriodId_Info');
    },

    getViewData: function (view) {
        view.getStore().load();
    },

    initEvents: function (view) {
        var me = this,
            unassignedGrid = view.down('chesimportpaymentsunassignedgrid'),
            assignedGrid = view.down('chesimportpaymentsassignedgrid'),
            summaryGrid = view.down('chesimportpaymentssummarygrid');

        summaryGrid.getStore().on('beforeload', me.onBeforeDataLoad, me);
        unassignedGrid.getStore().on('beforeload', me.onBeforeDataLoad, me);
        assignedGrid.getStore().on('beforeload', me.onBeforeDataLoad, me);

        assignedGrid.down('button[action=import]').on('click', me.onImportClick, me);
        assignedGrid.down('button[action=Export]').on('click', me.onExportClick, me);
        unassignedGrid.down('button[action=Export]').on('click', me.onExportClick, me);
    },

    onBeforeDataLoad: function(store, operation) {
        var me = this,
            view = me.getMainView();

        operation.params.periodId = me.getContextValue(view, 'periodId');
        operation.params.paymentDay = me.getContextValue(view, 'day');
    },

    onExportClick: function (button) {
        var me = this,
            grid = button.up('grid'),
            params = {};

        if (grid) {
            var store = grid.getStore();
            var columns = grid.columns;

            var headers = [];
            var dataIndexes = [];

            Ext.each(columns, function (res) {
                if (!res.hidden && res.header != "&#160;" && (res.dataIndex || res.dataExportAlias)) {
                    var dataIndex = res.dataIndex || res.dataExportAlias,
                        index = dataIndex.indexOf(".");
                    headers.push(res.text);
                    dataIndexes.push(index >= 0 ? dataIndex.substring(0, index) : dataIndex);
                }
            });

            if (headers.length > 0) {
                Ext.apply(params, { headers: headers, dataIndexes: dataIndexes });
            }

            if (store.sortInfo != null) {
                Ext.apply(params, {
                    sort: store.sortInfo.field,
                    dir: store.sortInfo.direction
                });
            }

            Ext.apply(params, store.lastOptions.params);

            var action = B4.Url.action('/ChesImport/ExportPayments') + '?_dc=' + (new Date().getTime()),
                form,
                r = /"/gm,
                inputs = [];

            Ext.iterate(params, function(key, value) {
                if (!Ext.isDefined(value)) {
                    return;
                }

                if (Ext.isArray(value)) {
                    Ext.each(value, function (item) {
                        inputs.push({ tag: 'input', type: 'hidden', name: key, value: item.toString().replace(r, "&quot;") });
                    });
                } else {
                    inputs.push({ tag: 'input', type: 'hidden', name: key, value: value.toString().replace(r, "&quot;") });
                }
            });

            form = Ext.DomHelper.append(document.body, { tag: 'form', action: action, method: 'POST', target: '_blank' });
            Ext.DomHelper.append(form, inputs);

            form.submit();
            form.remove();
        }
    },

    onImportClick: function () {
        var me = this,
            view = me.getMainView(),
            periodId = me.getContextValue(view, 'periodId'),
            paymentDay = me.getContextValue(view, 'day');

        Ext.Msg.confirm('Подтвердите действия',
            'Оплаты будут загружены в систему. Продолжить?',
            function(result) {
                if (result === 'yes') {
                    me.mask('Запуск импорта...', me.getMainView());
                    B4.Ajax.request({
                            url: B4.Url.action('Import', 'GkhImport'),
                            params: {
                                fileTypes: B4.enums.regop.FileType.Pay,
                                periodId: periodId,
                                paymentDay: paymentDay,
                                importId: 'Bars.Gkh.RegOperator.Imports.Ches.ChesImport',
                                importFromTables: true
                            }
                        })
                        .next(function(resp) {
                            var data = Ext.decode(resp.responseText),
                                message;

                            if (!Ext.isEmpty(data.message)) {
                                if (!Ext.isEmpty(data.title)) {
                                    message = data.title + ' <br/>';
                                } else {
                                    message = '';
                                }

                                message += data.message;
                            } else if (!Ext.isEmpty(data.title)) {
                                message = data.title;
                            } else {
                                message = '';
                            }

                            me.unmask();

                            if (data.status == 40) {
                                Ext.Msg.alert(data.title, data.message);
                            } else {
                                Ext.Msg.alert('Успешная загрузка', message);
                            }
                        })
                        .error(function(response) {
                            var message = 'Ошибка во время выполнения операции';
                            if (response) {
                                message = response.message;
                            }
                            me.unmask();
                            Ext.Msg.alert('Ошибка', message);
                        });
                }
            });
    }
});