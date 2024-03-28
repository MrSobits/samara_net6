Ext.define('B4.controller.import.DecisionProtocolImport', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['import.DecisionProtocolImportPanel'],

    mainView: 'import.DecisionProtocolImportPanel',
    mainViewSelector: 'decisionprotocolimportpanel',

    aspects: [
        {
            xtype: 'gkhimportaspect',
            viewSelector: 'decisionprotocolimportpanel',
            importId: 'Bars.Gkh.RegOperator.Imports.DecisionProtocol.DecisionProtocolImport',
            maxFileSize: 52428800,
            getUserParams: function () {
                var me = this;
                me.params = me.params || {};

                me.params['TransferToDefaultState'] = me.controller.getMainView().down('[name=TransferToDefaultState]').getValue();
            },
            loadButtonClick: function () {
                var me = this;
                me.importPanel = me.controller.getMainComponent();

                var fileImport = me.importPanel.down('#fileImport'),
                    transferToDefaultState = me.importPanel.down('[name=TransferToDefaultState]').getValue();

                if (!fileImport.isValid()) {
                    B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл для импорта!', 'warning');
                    return;
                }

                if (!fileImport.isFileExtensionOK()) {
                    B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл с допустимым расширением: ' + fileImport.possibleFileExtensions, 'warning');
                    return;
                }

                if (!Ext.isEmpty(me.maxFileSize) && fileImport.isFileLoad() && fileImport.getSize() > me.maxFileSize) {
                    Ext.Msg.alert('Импорт', 'Необходимо выбрать файл допустимого размера');
                    return;
                }

                if (transferToDefaultState) {
                    Ext.Msg.confirm('Загрузка файла', 'Вы действительно хотите осуществить импорт протоколов, который переведет в начальный статус уже имеющийся(имеющиеся) протокол(ы) на доме?', function (result) {
                        if (result === 'yes') {
                            me.loadFile();
                        }
                    });
                } else {
                    Ext.Msg.confirm('Загрузка файла', 'Вы действительно хотите осуществить импорт протоколов, который позволит сформировать на доме несколько протоколов решения в конечном статусе?', function (result) {
                        if (result === 'yes') {
                            me.loadFile();
                        }
                    });
                }
            },
            loadFile: function() {
                var me = this,
                    formImport = me.importPanel.down('#importForm');

                me.getUserParams();
                me.params.importId = me.importId;

                me.mask('Загрузка данных', me.controller.getMainComponent());

                formImport.submit({
                    url: B4.Url.action('/GkhImport/Import'),
                    params: me.params,
                    success: function(form, action) {
                        me.unmask();
                        var message;
                        if (!Ext.isEmpty(action.result.message)) {
                            message = action.result.title + ' <br/>' + action.result.message;
                        } else {
                            message = action.result.title;
                        }

                        Ext.Msg.show({
                            title: 'Успешная загрузка',
                            msg: message,
                            width: 300,
                            buttons: Ext.Msg.OK,
                            icon: Ext.window.MessageBox.INFO
                        });

                        var log = me.importPanel.down('#log');
                        if (log) {
                            log.setValue(me.createLink(action.result.logFileId));
                        }
                    },
                    failure: function(form, action) {
                        me.unmask();
                        var message;
                        if (!Ext.isEmpty(action.result.message)) {
                            message = (action.result.title ? (action.result.title + ' <br/>') : '') + action.result.message;
                        } else {
                            message = action.result.title;
                        }

                        Ext.Msg.alert('Ошибка загрузки', message, function() {
                            if (action.result.logFileId > 0) {
                                var log = me.importPanel.down('#log');
                                if (log) {
                                    log.setValue(me.createLink(action.result.logFileId));
                                }
                            }
                        });
                    }
                }, this);
            }
        }
    ],

    index: function() {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});