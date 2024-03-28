Ext.define('B4.controller.integrations.GisIntegration', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [],

    views: [
        'integrations.gis.PrepareDataResultWindow',
        'integrations.gis.ProtocolWindow',
        'integrations.gis.Panel'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainPanel', selector: 'gisintegrationpanel' }
    ],

    mainView: 'integrations.gis.Panel',
    mainViewSelector: 'gisintegrationpanel',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Administration.OutsideSystemIntegrations.Gis.Dictions.View',
                    applyTo: '[name=dictpanel]',
                    selector: 'gisintegrationpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.tab.show();
                        } else {
                            component.tab.hide();
                        }
                    }
                },
                {
                    name: 'Administration.OutsideSystemIntegrations.Gis.Methods.ExecuteMethod',
                    applyTo: '[name=AddTask]',
                    selector: 'gisintegrationpanel'
                },
                {
                    name: 'Administration.OutsideSystemIntegrations.Gis.Tasks.View',
                    applyTo: '[name=taskpanel]',
                    selector: 'gisintegrationpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.tab.show();
                        } else {
                            component.tab.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'validationResultButtonExportAspect',
            gridSelector: 'validationresultgrid',
            buttonSelector: 'validationresultgrid button[name=Export]',
            controllerName: 'TaskTree',
            actionName: 'ValidationResultForExport'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'uploadAttachmentsResultButtonExportAspect',
            gridSelector: 'uploadresultgrid',
            buttonSelector: 'uploadresultgrid button[name=Export]',
            controllerName: 'TaskTree',
            actionName: 'UploadAttachmentsResultForExport'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'pakcagesButtonExportAspect',
            gridSelector: 'b4grid[name=PackageGrid]',
            buttonSelector: 'b4grid[name=PackageGrid] button[name=Export]',
            controllerName: 'TaskTree',
            actionName: 'PackagesForExport'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'gistasktree b4updatebutton': { 'click': { fn: me.updateTaskTree, scope: me } },
            'gistasktree b4addbutton': { 'click': { fn: me.runPrepareDataWizard, scope: me } },
            'gistasktree actioncolumn[dataIndex=Result]': { 'click': { fn: me.showResult, scope: me } },
            'gistasktree actioncolumn[dataIndex=Protocol]': { 'click': { fn: me.showProtocol, scope: me } },
            // 'preparedataresultwindow b4grid[name=PackageGrid] actioncolumn[dataIndex=NotSignedDataLength]': {
            'preparedataresultwindow b4grid[name=PackageGrid] actioncolumn[name=NotSignedData]': {
                'click': { fn: me.showFormedXml, scope: me }
            },
            'protocolwindow b4grid[name=ProtocolGrid] b4updatebutton': {
                'click': { fn: me.updateProtocol, scope: me }
            },
            'dictionariesgrid': {
                'compareDictionary': { fn: me.compareDictionary, scope: me },
                'compareRecords': { fn: me.compareRecords, scope: me },
                'updateStates': { fn: me.updateDictionaryStates, scope: me },
                'showRecords': { fn: me.showRecords, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainPanel() || Ext.widget('gisintegrationpanel');

        me.bindContext(view);
        me.application.deployView(view);

        view.down('gistasktree').getStore().load();
        view.down('dictionariesgrid').getStore().load();
    },

    showResult: function (gridView, rowIndex, colIndex, el, e, record) {
        //TODO в зависимости от типа узла открывать:
        //для узлов типа Task: визард подписания и отправки данных
        //для узлов типа Trigger: 
        //  для подготовки данных - окно просмотра пакетов и результатов валидации
        //  для отправки данных - окно просмотра результатов обработки пакетов
        // для узлов типа Pachage - окно просмотра результатов обработки пакета

        if (record && record.data && record.getData().Result) {
            var me = this,
                type = record.data.Type,
                id = record.data.Id;

            switch (type) {
                case 'Task':
                    me.showSendDataWizard(id);
                    break;
                case 'PreparingDataTrigger':
                    me.showPrepareDataTriggerResult(id);
                    break;
                case 'SendingDataTrigger':
                    me.showSendDataTriggerResult(id, false);
                    break;
                case 'Package':
                    me.showSendDataTriggerResult(id, true);
                    break;
            }
        }
    },

    showSendDataWizard: function (taskId) {
        var me = this;
        me.mask('Получение параметров подписывания и отправки данных ...');

        B4.Ajax.request({
            url: B4.Url.action('GetSignAndSendDataParams', 'GisIntegration'),
            params: {
                taskId: taskId
            },
            timeout: 9999999
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);

            var prepareDataDescription = json.data.PrepareDataResultDescription,
                needSign = json.data.NeedSign,
                sendDataWizard = Ext.create('B4.view.wizard.senddata.Wizard', {
                    prepareDataDescription: prepareDataDescription,
                    taskId: taskId,
                    needSign: needSign
                });

            sendDataWizard.on('close', function (panel) {

                if (panel.openTaskTree === true) {
                    var me = this,
                        tabPanel = me.getMainPanel().down('tabpanel'),
                        taskTab = tabPanel.down('panel[name=taskpanel]');

                    taskTab.down('gistasktree').getStore().load();
                    tabPanel.setActiveTab(taskTab);
                }
            }, this);

            sendDataWizard.show();

            me.unmask();

        }, me).error(function (e) {

            Ext.Msg.alert('Ошибка!', (e.message || 'Ошибка получения параметров подписывания и отправки данных'));
            me.unmask();

        }, me);
    },

    showProtocol: function (gridView, rowIndex, colIndex, el, e, record) {
        //в зависимости от типа узла открывать:
        //для узлов типа Trigger: 
        //  для подготовки данных - окно просмотра протокола подготовки данных
        //  для отправки данных - окно просмотра протокола отправки данных

        if (record && record.data && record.getData().Protocol) {
            var me = this,
                type = record.data.Type,
                id = record.data.Id;

            switch (type) {
                case 'PreparingDataTrigger':
                    me.showTriggerProtocol(id, false);
                    break;
                case 'SendingDataTrigger':
                    me.showTriggerProtocol(id, true);
                    break;
            }
        }
    },

    showPrepareDataTriggerResult: function (triggerId) {
        var me = this;

        me.mask('Получение результата подготовки данных ...');

        B4.Ajax.request({
            url: B4.Url.action('GetPreparingDataTriggerResult', 'TaskTree'),
            params: {
                triggerId: triggerId
            },
            timeout: 9999999
        }).next(function (response) {

            var json = Ext.JSON.decode(response.responseText),
                validateResults = json.data.ValidateResult,
                packages = json.data.Packages,
                uploadResult = json.data.UploadAttachmentsResult,
                hasPackages = packages && packages.length !== 0,
                hasValidateResults = validateResults && validateResults.length !== 0,
                hasUploadResult = uploadResult && uploadResult.length !== 0;

            if (hasPackages === true || hasValidateResults === true || hasUploadResult === true) {
                var prepareDataResultWindow = Ext.create('B4.view.integrations.gis.PrepareDataResultWindow', {
                    validationResult: validateResults,
                    packages: packages,
                    uploadAttachmentsResult: uploadResult,
                    triggerId: triggerId
                });

                prepareDataResultWindow.show();
            } else {
                Ext.Msg.alert('Ошибка!', 'Нет пакетов, сообщений валидации, результатов загрузки вложений!');
            }

            me.unmask();

        }, me).error(function (e) {

            Ext.Msg.alert('Ошибка!', (e.message || 'Ошибка получения результата подготовки данных.'));
            me.unmask();

        }, me);
    },

    showSendDataTriggerResult: function (nodeId, byPackage) {
        var params = byPackage ? { packageId: nodeId } : { triggerId: nodeId },

        resultWindow = Ext.create('B4.view.integrations.gis.SendDataResultWindow',
        params);

        resultWindow.show();
    },

    showTriggerProtocol: function (triggerId, sendDataProtocol) {
        var protocolWindow = Ext.create('B4.view.integrations.gis.ProtocolWindow',
        {
            triggerId: triggerId,
            sendDataProtocol: sendDataProtocol
        });
        protocolWindow.show();
    },

    updateTaskTree: function (btn) {
        btn.up('gistasktree').getStore().load();
    },

    updateProtocol: function (btn) {
        btn.up('b4grid[name=ProtocolGrid]').getStore().load();
    },

    runPrepareDataWizard: function () {
        var prepareDataWizard = Ext.create('B4.view.wizard.preparedata.Wizard');
        prepareDataWizard.show();

        prepareDataWizard.on('close', function (panel) {
            if (panel.openExtender === true) {
                var extenderWindow = Ext.create(panel.extenderClassName, {
                    exporter_Id: panel.exporter_Id,
                    dataSupplierIsRequired: panel.dataSupplierIsRequired,
                    initialStepId: panel.dataSupplierIsRequired ? 'dataSupplier' : 'pageParameters'
                });

                extenderWindow.show();

                extenderWindow.on('close', arguments.callee, this);
            } else {
                if (panel.openTaskTree === true) {
                    var me = this,
                        tabPanel = me.getMainPanel().down('tabpanel'),
                        taskTab = tabPanel.down('panel[name=taskpanel]');

                    taskTab.down('gistasktree').getStore().load();
                    tabPanel.setActiveTab(taskTab);
                }
            }
        }, this);
    },

    showFormedXml: function (grid, cell, index, hz, event, rec) {
        B4.Ajax.request({
            url: B4.Url.action('GetPackageXmlData', 'Package'),
            params: {
                //package_Ids: rec.get('Id'),
                packageId: rec.get('Id'),
                packageType: rec.get('Type'),
                forPreview: true,
                signed: false
            },
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.decode(response.responseText).data,
                packageName = rec.get('Name') || 'Пакет',
                dataName = 'неподписанные данные',

                xmlPreviewWin = Ext.create('B4.view.integrations.gis.PackageDataPreviewWindow',
                {
                    //xmlData: data[0].Data,
                    xmlData: data,
                    title: packageName + ' - ' + dataName
                });

            xmlPreviewWin.show();

        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || 'Не получены неподписанные данные'));
        });
    },

    compareDictionary: function(gridView, rowIndex, colIndex, el, e, rec) {
        var me = this,
            dictionaryCode = rec.get('Code'),
            dictionaryName = rec.get('Name');

        me.mask('Получение параметров выполнения операции ...');

        B4.Ajax.request({
            url: B4.Url.action('GetCompareDictionaryParams', 'Dictionary'),
            params: {
                dictionaryCode: dictionaryCode
            },
            timeout: 9999999
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);

            var needSign = json.data.NeedSign,
                compareDictionaryWizard = Ext.create('B4.view.wizard.dictionary.comparedictionary.Wizard', {
                    needSign: needSign,
                    dictionaryCode: dictionaryCode,
                    dictionaryName: dictionaryName
                });

            compareDictionaryWizard.on('close', function () {
                var me = this,
                        tabPanel = me.getMainPanel().down('tabpanel'),
                        dictTab = tabPanel.down('panel[name=dictpanel]');

                dictTab.down('dictionariesgrid').getStore().load();
                tabPanel.setActiveTab(dictTab);
            }, this);

            compareDictionaryWizard.show();

            me.unmask();

        }, me).error(function (e) {

            Ext.Msg.alert('Ошибка!', (e.message || 'Ошибка получения параметров выполнения операции'));
            me.unmask();

        }, me);
    },

    compareRecords: function (gridView, rowIndex, colIndex, el, e, rec) {
        var me = this,
            dictionaryCode = rec.get('Code'),
            dictionaryName = rec.get('Name');

        me.mask('Получение параметров выполнения операции ...');

        B4.Ajax.request({
            url: B4.Url.action('GetCompareDictionaryRecordsParams', 'Dictionary'),
            params: {
                dictionaryCode: dictionaryCode
            },
            timeout: 9999999
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);

            var needSign = json.data.NeedSign,
                wizard = Ext.create('B4.view.wizard.dictionary.comparedictionaryrecords.Wizard', {
                    needSign: needSign,
                    dictionaryCode: dictionaryCode,
                    dictionaryName: dictionaryName
                });

            wizard.on('close', function () {
                var me = this,
                        tabPanel = me.getMainPanel().down('tabpanel'),
                        dictTab = tabPanel.down('panel[name=dictpanel]');

                dictTab.down('dictionariesgrid').getStore().load();
                tabPanel.setActiveTab(dictTab);
            }, this);

            wizard.show();

            me.unmask();

        }, me).error(function (e) {

            Ext.Msg.alert('Ошибка!', (e.message || 'Ошибка получения параметров выполнения операции'));
            me.unmask();

        }, me);
    },

    showRecords: function (gridView, rowIndex, colIndex, el, e, rec) {
        var dictionaryCode = rec.get('Code'),
            window = Ext.create('B4.view.dictionaries.RecordWindow');

        window.loadRecords(dictionaryCode);

        window.show();
    },

    updateDictionaryStates: function() {
        var me = this;
        me.mask('Получение параметров выполнения операции ...');

        B4.Ajax.request({
            url: B4.Url.action('GetUpdateStatesParams', 'Dictionary'),
            params: {
            },
            timeout: 9999999
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);

            var needSign = json.data.NeedSign,
                updateStatesWizard = Ext.create('B4.view.wizard.dictionary.udatestates.Wizard', {
                    needSign: needSign
                });

            updateStatesWizard.on('close', function () {
                var me = this,
                        tabPanel = me.getMainPanel().down('tabpanel'),
                        dictTab = tabPanel.down('panel[name=dictpanel]');

                dictTab.down('dictionariesgrid').getStore().load();
                tabPanel.setActiveTab(dictTab);
            }, this);

            updateStatesWizard.show();

            me.unmask();

        }, me).error(function (e) {

            Ext.Msg.alert('Ошибка!', (e.message || 'Ошибка получения параметров выполнения операции'));
            me.unmask();

        }, me);
    }
});