Ext.define('B4.controller.import.chesimport.Analysis', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.enums.regop.FileType',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'import.chesimport.AnalysisPanel',
    mainViewSelector: 'chesimportanalysisgrid',

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'chesPrintAspect',
            buttonSelector: 'chesimportanalysisgrid gkhbuttonprint',
            codeForm: 'ChesReport',
            getUserParams: function(reportId) {
                var me = this,
                    view = me.controller.getMainView();

                var param = { periodId: parseInt(me.controller.getContextValue(view, 'periodId')) };

                me.params.userParams = Ext.JSON.encode(param);
            }
        }
    ],

    deleteRecord: function(grid, action, record) {
        var me = this,
            view = me.getMainView(),
            periodId = me.getContextValue(view, 'periodId'),
            fileType = record.get('FileType');

        if (action.toLowerCase() !== 'delete') {
            return;
        }

        Ext.Msg.confirm('Подтвердите действия',
            'Все связанные данные будут удалены. Удалить выбранный файл?',
            function(result) {
                if (result === 'yes') {
                    me.mask('Удаление...', me.getMainView());
                    B4.Ajax.request({
                            url: B4.Url.action('DeleteSection', 'ChesImport'),
                            params: {
                                type: fileType,
                                periodId: periodId
                            }
                        })
                        .next(function() {
                            me.updateGrid();
                            me.unmask();
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
    },

    onImportClick: function() {
        var me = this,
            view = me.getMainView(),
            periodId = me.getContextValue(view, 'periodId'),
            recordSelection = view.getSelectionModel().getSelection();

        if (!recordSelection || !recordSelection.length) {
            Ext.Msg.alert('Ошибка', 'Выберите секции для импорта');
            return;
        }

        Ext.Msg.confirm('Подтвердите действия',
            'Выбранные секции будут загружены в систему. Продолжить?',
            function(result) {
                if (result === 'yes') {
                    me.mask('Запуск импорта...', me.getMainView());
                    B4.Ajax.request({
                            url: B4.Url.action('Import', 'GkhImport'),
                            params: {
                                fileTypes: Ext.encode(Ext.Array.map(recordSelection, function(el) {
                                    return el.get('FileType');
                                })),
                                periodId: periodId,
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
    },

    init: function() {
        var me = this;

        me.control({
            'chesimportanalysisgrid b4updatebutton': { 'click': { fn: me.updateGrid, scope: me } },
            'chesimportanalysisgrid': { 'rowaction': { fn: me.deleteRecord, scope: me } },
            'chesimportanalysisgrid button[action=import]': { 'click': { fn: me.onImportClick, scope: me } },
            'chesimportanalysisgrid button[action=runCheck]': { 'click': { fn: me.onRunCheckClick, scope: me } }
        });

        me.callParent(arguments);

        me.getAspect('chesPrintAspect').loadReportStore();
    },

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view, 'chesPeriodId_Info');
        me.setContextValue(view, 'periodId', id);

        view.getStore()
            .on('beforeload', function(store, operation) {
                operation.params = operation.params || {};
                operation.params.periodId = id;
            });

        me.updateGrid();
    },

    updateGrid: function() {
        var me = this,
            view = me.getMainView();
        view.getStore().load();
    },

    onRunCheckClick: function() {
        var me = this,
            view = me.getMainView(),
            periodId = me.getContextValue(view, 'periodId'),
            recordSelection = view.getSelectionModel().getSelection();

        if (!recordSelection || !recordSelection.length) {
            Ext.Msg.alert('Ошибка', 'Выберите секции для проверки');
            return;
        }

        me.mask('Подождите...', me.getMainView());
        B4.Ajax.request({
                url: B4.Url.action('RunCheck', 'ChesImport'),
                timeout: 10 * 60 * 1000,
                params: {
                    fileTypes: Ext.encode(Ext.Array.map(recordSelection, function(el) {
                        return el.get('FileType');
                    })),
                    periodId: periodId
                }
            })
            .next(function (resp) {
                me.updateGrid();
                me.unmask();
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