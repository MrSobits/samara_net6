Ext.define('B4.controller.import.chesimport.SaldoCheck', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.view.import.chesimport.SaldoCheckGrid',
        'B4.enums.regop.FileType'
    ],

    mixins: { 
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody' 
    },

    mainView: 'import.chesimport.SaldoCheckGrid',
    mainViewSelector: 'chesimportsaldocheckgrid',

    init: function() {
        var me = this,
            actions = {
                'chesimportsaldocheckgrid': {
                    'beforerender': {
                        fn: me.initEvents,
                        scope: me
                    },
                    'afterrender': {
                        fn: me.getViewData,
                        scope: me
                    }
                },
                'chesimportsaldocheckgrid b4addbutton': {
                    click: {
                        fn: me.tryImportSaldo,
                        scope: me
                    }
                },
                'chesimportsaldocheckgrid button[action=Export]': {
                    click: {
                        fn: me.onExportClick,
                        scope: me
                    }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.setContextValue(view, 'periodId', id);

        me.bindContext(view);
        me.application.deployView(view, 'chesPeriodId_Info');
    },

    initEvents: function (view) {
        var me = this;
        view.getStore()
            .on('beforeload', function(store, operation) {
                operation.params.periodId = me.getContextValue(view, 'periodId');
            }, me);
    },

    getViewData: function (view) {
        view.getStore().load();
    },

    tryImportSaldo: function() {
        var me = this,
            view = me.getMainView(),
            periodId = me.getContextValue(view, 'periodId'),
            selectionModel = view.getSelectionModel(),
            count = selectionModel.selected.length,
            confirmMsg = '';

        if (selectionModel.isSelectedAll) {
            confirmMsg = 'Будет произведена загрузка <b>всех</b> записей в реестре.';
        } else if (count > 0) {
            confirmMsg = 'Будет произведена загрузка <b>' + count + '</b> выбранных записей.';
        } else {
            Ext.Msg.alert('Предупреждение', 'Не выбраны записи для загрузки')
            return;
        }

        Ext.Msg.confirm('Подтвердите действия',
            confirmMsg + ' Продолжить?',
            function (result) {
                var selectedIds = [];
                if (result === 'yes') {
                    Ext.each(selectionModel.getSelection(), function(v) {
                        selectedIds.push(v.get('Id'));
                    });
                    me.importSaldo(periodId, selectionModel.isSelectedAll, selectedIds);
                }
            });
    },

    importSaldo: function (periodId, importAll, selectedIds) {
        var me = this,
            ids = importAll ? [] : selectedIds;
        me.mask('Загрузка сальдо...', me.getMainView());

        B4.Ajax.request({
                url: B4.Url.action('ImportSaldo', 'ChesImport'),
                timeout: 3600000,
                params: {
                    periodId: periodId,
                    importAll: importAll,
                    ids: Ext.encode(ids)
                }
            })
            .next(function (resp) {
                me.unmask();
                me.getMainView().getStore().load();
            })
            .error(function(response) {
                var message = 'Ошибка во время выполнения операции';
                me.unmask();
                if (response) {
                    message = response.message;
                }
                Ext.Msg.alert('Ошибка', message);
            });
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

            var action = B4.Url.action('/ChesImport/ExportSaldo') + '?_dc=' + (new Date().getTime()),
                form,
                r = /"/gm,
                inputs = [];

            Ext.iterate(params, function (key, value) {
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
    }
});