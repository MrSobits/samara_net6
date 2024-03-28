Ext.define('B4.controller.al.ReportPanel', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.mixins.LayoutControllerLoader',
        'B4.form.EnumCombo',
        'B4.helpers.al.ReportParamFieldBuilder',
        'B4.enums.al.ReportPrintFormat',
        'B4.store.ReportHistoryParam',
        'B4.enums.TaskStatus',
        'B4.enums.TypeProgramStateCr',
        'B4.enums.TypeVisibilityProgramCr',
        'B4.ux.grid.column.Enum'
    ],

    views: ['al.ReportPanel'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'reportpanel'
        },
        {
            ref: 'tabPanel',
            selector: 'reportpanel tabpanel'
        },
        {
            ref: 'historyGrid',
            selector: 'reportpanel tabpanel reporthistorygrid'
        }
    ],

    init: function () {
        B4.Url.loadCss('Content/ReportPanel.View.css');
        var me = this;

        me.control({
            'reportpanel #panelReports button': {
                click: me.onPrepareReport
            },

            'reportpanel #panelTabs #printReport': {
                click: me.onPrint
            },

            'reportpanel textfield[name=searchfield]': {
                keypress: me.searchReport
            },

            'reporthistorygrid': {
                'store.beforeload': me.beforeHistoryLoad,
                'rowaction': {
                    fn: me.onHistoryRowAction,
                    scope: me
                },
                'gridaction': {
                    fn: me.onHistoryGridAction,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('reportpanel');

        me.bindContext(view);
        me.application.deployView(view);
        me.getHistoryGrid().getStore().load();

        me.buildReportList();
    },

    onPrepareReport: function (button) {
        this.prepareReport(button);
    },

    //окно показывается либо незаполненное, либо на основе записи из истории
    prepareReport: function (button, historyId) {
        var me = this,
            btnCfg = {
                id: button.reportId,
                codeReport: button.codeReport,
                isOld: button.isOld,
                title: button.reportTitle,
                paramsController: button.paramsController,
                description: button.reportDesc,
                isPivot: button.isPivot,
                className: button.className
            },
            tabPanel = Ext.ComponentQuery.query('reportpanel #panelTabs')[0],
            idx = tabPanel.items.findIndexBy(function (tab) {
                return tab.reportId == btnCfg.id;
            }),
            tabId,
            tab;

        if (idx != -1) {
            //если открываем из журнала вкладку, которая уже есть, то удаляем ее и заново создаем уже заполненную
            if (historyId) {
                tabPanel.remove(idx);
            }
            else {
                tabPanel.setActiveTab(idx);
                return;
            }        
        }

        tabId = 'printTab' + (btnCfg.isOld ? '' : '_new_') + btnCfg.id;

        tab = tabPanel.add(Ext.create('Ext.panel.Panel', {
            itemId: 'tab_' + btnCfg.id,
            title: btnCfg.title,
            closable: true,
            reportId: btnCfg.id,
            layout: 'anchor',
            margins: -1,
            autoScroll: true,
            items: [
                {
                    xtype: 'container',
                    html: '<h1 class="descTitle" style="padding: 20px 0 0;">' + btnCfg.title + '</h1>' +
                          '<p class="textblock" style="padding: 20px 0;">' + btnCfg.description + '</p>' +
                          '<hr style="border-top: 1px solid #ccc; border-right: none; border-left: none;">' +
                          '<b style="color: #545454; padding: 0 0 25px 0;">Параметры отчета:</b>',
                    anchor: '100%',
                    padding: '0 35px 15px 35px'
                },
                Ext.create('Ext.container.Container', {
                    border: false,
                    itemId: tabId,
                    anchor: '100%',
                    padding: 10
                }),
                {
                    xtype: 'container',
                    padding: '0 35px',
                    items: [
                        {
                            xtype: 'container',
                            html: '<hr style="border-top: 1px solid #ccc; border-right: none; border-left: none;">'
                        },
                        {
                            xtype: 'button',
                            margin: '10 0',
                            text: 'Печать',
                            itemId: 'printReport',
                            reportId: btnCfg.id,
                            codeReport: btnCfg.codeReport,
                            paramsController: btnCfg.paramsController,
                            isPivot: btnCfg.isPivot,
                            isOld: btnCfg.isOld,
                            className: btnCfg.className,
                            iconCls: 'icon-printer'
                        },
                        {
                            xtype: 'progressbar',
                            text: 'Подготовка отчета...',
                            margin: 20,
                            hidden: true
                        },
                        {
                            xtype: 'container',
                            itemId: 'downloadLink',
                            margin: '20 0 0 0',
                            hidden: true,
                            layout: 'hbox'
                        }
                    ]
                }
            ]
        }));

        if (btnCfg.isOld) {
            me.loadController(btnCfg.paramsController, null, 'reportpanel tabpanel #' + tabId);
        } else {
            me.buildParamsView(btnCfg.id, 'reportpanel tabpanel #' + tabId, historyId);
        }

        tabPanel.setActiveTab(tab);
    },

    fillParams: function (panel, historyId) {
        var me = this,
            data,
            field,
            ar, el;

        B4.Ajax.request({
            url: B4.Url.action('ReportHistoryParamList', 'ReportHistory'),
            params: {
                historyId: historyId
            }
        }).next(function (resp) {
            data = Ext.decode(resp.responseText);

            Ext.each(data.data, function (param) {
                field = panel.down('[name=' + param.Name + ']');
                if (field) {
                    if (field.idProperty) {
                        ar = [];
                        //приходится проставлять так, чтобы потом правильно получать значения при печати
                        Ext.each(param.Value, function(rec) {
                            el = {};
                            el[field.idProperty] = rec;
                            ar.push(el);
                        });

                        field.setValue(ar);
                        field.setRawValue(param.DisplayValue);
                    } else {
                        field.setValue(param.Value);
                    }
                }
            }, me);
        }, me)
        .error(function (err) {
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    searchReport: function (field) {
        var me = this;
        me.buildReportList(field.getValue());
    },

    buildReportList: function (query) {
        var me = this, data,
           panelReports = Ext.ComponentQuery.query('reportpanel #panelReports')[0];
     
        if (!me.searchInProcess) {
            me.searchInProcess = true;

            me.mask('Пожалуйста подождите...', panelReports);
            B4.Ajax.request({
                url: B4.Url.action('Search', 'ReportPanel'),
                params: {
                    query: query || ''
                },
                timeout: 1000 * 60 * 30 //30min
            }).next(function (resp) {
                data = Ext.decode(resp.responseText);
                panelReports.removeAll();

                Ext.each(data.data, function (reportCategory) {
                    var panel = {
                        xtype: 'panel',
                        ui: 'reportlist-item',
                        autoScroll: true,
                        title: reportCategory.Name,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        items: []
                    };

                    Ext.each(reportCategory.Reports, function (report) {
                        panel.items.push({
                            xtype: 'button',
                            ui: 'reportButton',
                            text: report.Name,
                            codeReport: report.Code,
                            isOld: report.IsOld,
                            reportId: report.Id,
                            reportTitle: report.Name,
                            paramsController: report.ParamsController,
                            reportDesc: report.Description,
                            isPivot: report.IsPivot,
                            className: report.ClassName
                        });
                    });

                    panelReports.add(panel);

                }, me);

                panelReports.enable();
                panelReports.doLayout();
                me.isReportListReady = true;
                me.searchInProcess = false;
                me.unmask();
            }, me)
            .error(function (err) {
                me.searchInProcess = false;
                me.unmask();
                Ext.Msg.alert('Ошибка', err.message || err.message || err);
            });
        }  
    },

    buildParamsView: function (id, select, historyId) {
        var me = this,
            builder = B4.helpers.al.ReportParamFieldBuilder,
            container = Ext.ComponentQuery.query(select)[0],
            data, panel,
            defaultsValue = {
                labelWidth: 200,
                labelAlign: 'right',
                reportId: id
            };

        B4.Ajax.request({
            url: B4.Url.action('list', 'ReportParamGkh'),
            params: {
                reportId: id    
            }
        }).next(function (resp) {
            data = Ext.decode(resp.responseText);

            panel = Ext.create('Ext.container.Container', {
                xtype: 'container',
                layout: 'vbox',
                name: 'paramsContainer',
                suspendLayout: true,
                defaults: {
                    width: 550,
                    labelAlign: 'right',
                    labelWidth: 200
                },
                items: [
                    { xtype: 'hiddenfield', name: 'controllerName', value: 'ReportGenerator' },
                    { xtype: 'hiddenfield', name: 'controllerAction', value: 'SaveOnServer' }
                ]
            });

            Ext.each(data.data, function (field) {
                panel.add(builder.getFieldConfig(field, defaultsValue, field.Id));
            }, me);

            var additionalAspects = builder.getAspects();
            if (additionalAspects.length != 0) {
                Ext.each(additionalAspects, function (aspectConfig) {
                    var aspect = Ext.ComponentManager.create(aspectConfig);
                    this.aspectCollection.add(aspect.name = aspect.name || this.aspectCollection.getCount(), aspect);
                }, this);

                this.aspectCollection.each(function (aspect) {
                    aspect.init(this);
                }, this);
            }

            panel.add({
                xtype: 'b4enumcombo',
                fieldLabel: 'Формат отчета',
                name: 'format',
                value: me.getReportFormat(id),
                enumName: 'B4.enums.al.ReportPrintFormat'
            });

            //заполняем поля даныыми, если на основе записи из истории
            if (historyId) {
                me.fillParams(panel, historyId);
            }

            panel.suspendLayout = false;
            panel.doLayout();

            container.add(panel);

        }, me);
    },

    getReportFormat: function(reportId) {
        switch (reportId) {
            case 3690:
            case 3692:
            case 3706:
                return B4.enums.al.ReportPrintFormat.csv;
            default:
                return B4.enums.al.ReportPrintFormat.xlsx;
        }
    },

    onPrint: function (button, event, eOpts) {
        var me = this,
            activeTab = Ext.ComponentQuery.query('reportpanel tabpanel')[0].getActiveTab(),
            controllerNameField = activeTab ? activeTab.down('hiddenfield[name="controllerName"]') : undefined,
            controllerActionField = activeTab ? activeTab.down('hiddenfield[name="controllerAction"]') : undefined,
            controllerName = controllerNameField ? controllerNameField.getValue() : undefined,
            controllerAction = controllerActionField ? controllerActionField.getValue() : undefined;

        if (button.isPivot) {
            me.showOlap(button, event, eOpts);
        } else {
            me.printReport(button, event, { controllerName: controllerName, controllerAction: controllerAction });
        }
    },

    printReport: function (button, event, eOpts) {
        var me = this,
            controllerName = button.paramsController,
            controller = controllerName ? me.application.getController(controllerName) : undefined,
            userValidateResult = controller ? controller.validateParams() : undefined,
            url,
            tab = button.up('#tab_' + button.reportId),
            pb = tab.down('progressbar'),
            linkContainer = tab.down('#downloadLink'),
            serverController = eOpts.controllerName || 'PrintForm',
            serverControllerAction = eOpts.controllerAction || 'GetPrintFormResult',
            validateMessage,
            params,
            paramObj,
            paramsForSave,
            fields,
            fieldisnull = tab.down('[name=paramsContainer]');

        if(fieldisnull) {
            fields = tab.down('[name=paramsContainer]').items.getRange();

        } else {
            fields = [];
        }
           
        if (button.isOld) {
            validateMessage = me.getValidationResult(tab, userValidateResult);
        } else {
            Ext.Array.each(fields, function (f) {
                if (!f.isValid()) {
                    validateMessage = 'Не заполнены обязательные поля';
                    return false;
                }
            });
        }

        if (validateMessage) {
            Ext.Msg.alert('Ошибка валидации!', validateMessage);
            return;
        } else {
            //Если ошибки заполнения полей нет, то печатаем отчет
            if (button.isOld) {
                params = controller.getParams();
            } else {
                params = {};
                paramsForSave = {};
                Ext.Array.each(fields, function (f) {
                    var value = f.getValue();
                    params[f.name] = value;
                    if (!f.isHidden() && f.xtype !== 'hiddenfield' && value) {
                        paramsForSave[f.name] = {
                            Value: value,
                            DisplayValue: f.getRawValue()
                        };
                    }
                });

                params['paramsForSave'] = Ext.encode(paramsForSave);
            }

            params['reportId'] = button.reportId;
            params['codeReport'] = button.codeReport;

            linkContainer.removeAll();
            linkContainer.hide();
            pb.show();

            pb.wait({
                interval: 500,
                text: 'Подготовка отчета...'
            });

            B4.Ajax.request({
                url: B4.Url.action(serverControllerAction, serverController),
                method: 'POST',
                params: params,
                timeout: 30 * 60 * 1000 // 30 минут таймаута
            }).next(function (response) {
                var result = Ext.decode(response.responseText);
                if (!result.taskedReport) {
                    linkContainer.add(
                    [
                        {
                            xtype: 'container',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'label',
                                            html: '<h1 class="descTitle">Отчет сформирован:</h1>'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Скачать',
                                            href: B4.Url.action('/fileupload/download?id=' + result.fileId),
                                            margin: '5 0 0 0'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    html: '<div class="warningXlsxReport">' +
                                            'Если у вас не открываются печатные формы,  необходимо скачать пакет обеспечения совместимости Microsoft Office' +
                                            '  <a href="http://www.microsoft.com/ru-ru/download/details.aspx?id=3" target="_blank">здесь</a>' +
                                          '</div>'
                                }
                            ]
                        }
                    ]);

                    linkContainer.show();

                    pb.hide();
                    button.enable();
                }
            }).error(function (response) {
                linkContainer.hide();
                pb.hide();
                button.enable();
                Ext.Msg.alert('Ошибка!', !Ext.isString(response.message) ? 'При создании отчета произошла ошибка!' : response.message);
            });
        }

    },

    getValidationResult: function (tab, userValidateResult) {
        var panel = tab.down('panel'),
            result = '',
            fields,
            invalidFields;

        if (Ext.isBoolean(userValidateResult) && userValidateResult) {

            // Если userValidateResult вернул true = нефакт что все заполнено правильно
            // Возможно, что просто не обработали в отчете параметры и поумолчанию вернулось true
            // но поскольку на форме могут быть поля allowBlank = false (то есть неразрешать пустое значение)
            // то проверяем сами за того кто писал форму отчета

            if (panel && !panel.getForm().isValid()) {

                // получаем все поля
                fields = panel.getForm().getFields();
                invalidFields = '';

                // проверяем, если поле не валидно, то записиваем fieldLabel в строку невалидных полей
                Ext.each(fields.items, function (field) {
                    if (!field.isValid()) {
                        invalidFields += '<br>' + field.fieldLabel;
                    }
                });

                if (invalidFields) {
                    result = 'Не заполнены обязательные поля:' + invalidFields;
                }
            }

        } else if (Ext.isString(userValidateResult)) {
            result = userValidateResult;
        } else {
            result = 'Параметры введены неверно';
        }

        return result;
    },

    showOlap: function (button) {
        var me = this,
            tab = button.up('#tab_' + button.reportId),
            controllerName = button.paramsController,
            controller = me.application.getController(controllerName),
            params = Ext.Object.toQueryString(controller.getParams()),
            url, frame;

        me.mask("Loading", tab);

        tab.removeAll();

        url = Ext.urlAppend('/pivot?modelName=' + button.className + '&' + params);

        frame = Ext.DomHelper.append(tab.body.dom, {
            tag: 'iframe',
            id: 'iframe_' + button.reportId,
            frameBorder: 0,
            style: 'width:100%; height: 100%;',
            src: B4.Url.action(url)
        });

        Ext.EventManager.on(frame, Ext.isIE ? 'readystatechange' : 'load', function () {
            me.unmask(tab);
        });
    },

    beforeHistoryLoad: function (store, opts) {
        Ext.apply(opts.params, {
            useUserFilter: true
        });
    },

    onHistoryRowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'reprint':
                this.onReportReprint(record);
                break;
            case 'showparams':
                this.onShowParams(record);
                break;
        }
    },

    onReportReprint: function(record) {
        var me = this,
            panelReports = Ext.ComponentQuery.query('reportpanel #panelReports')[0],
            isOld = record.get('ReportType') === 1, // если тип отчета=PrintForm
            button = panelReports.down('[reportId=' + record.get('ReportId') + '][isOld=' + isOld + ']');

        if (button) {
            if (isOld) {
                me.prepareReport(button);
            }
            else {
                me.prepareReport(button, record.get('Id'));
            }
        }
        else {
            Ext.Msg.alert('Ошибка!', 'Отчет для повторной печати не найден');
        }
    },

    onShowParams: function (record) {
        var me = this,
            paramWindow = Ext.ComponentQuery.query('reporthistoryparamwindow')[0];

        if (record.get('ReportType') === 2) { // если тип отчета=StoredReport
            if (paramWindow) {
                paramWindow.show();
            } else {
                paramWindow = Ext.create('B4.view.reportHistory.ParamWindow',
                {
                    constrain: true,
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    closeAction: 'destroy',
                    ctxKey: me.getCurrentContextKey()
                });
                paramWindow.show();

                paramWindow.down('[name=paramGrid]').getStore().filter('historyId', record.get('Id'));
            }
        } else {
            Ext.Msg.alert('Ошибка!', 'Детализация для данного типа отчета недоступна');
        }
    },

    onHistoryGridAction: function (grid, action) {
        switch (action.toLowerCase()) {
            case 'delete':
                this.onDeleteHistory(grid);
                break;
            case 'update':
                this.onUpdateHistoryGrid(grid);
                break;
        }
    },

    onUpdateHistoryGrid: function (grid) {
        grid.getStore().load();
    },

    onDeleteHistory: function (grid) {
        var me = this,
            records = Ext.Array.map(grid.getSelectionModel().getSelection(),
                function (record) { return record.get('Id') });
        records = Ext.Array.from(records);

        Ext.Msg.confirm('Внимание',
            'Вы действительно хотите удалить записи?'
            + (Ext.isEmpty(records) ? '<br>Будут удалены все записи журнала' : ''), function (btn) {
                if (btn !== 'yes') {
                    return false;
                }
                me.mask();
                B4.Ajax.request({
                    url: B4.Url.action('Delete', 'ReportHistory'),
                    timeout: 5 * 60 * 1000,
                    params: {
                        records: Ext.JSON.encode(records)
                    }
                }).next(function () {
                    me.unmask();
                    grid.getStore().load();
                }).error(function (err) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err);
                });
            });
    },

    updateStatus: function (reportId, status, fileId) {
        var tabPanel = Ext.ComponentQuery.query('reportpanel #panelTabs')[0],
            tab = tabPanel ? tabPanel.down('[reportId=' + reportId + ']') : undefined;
        
        if (!tab) {
            return;
        }

        var pb = tab.down('progressbar'),
            linkContainer = tab.down('#downloadLink'),
            printButton = tab.down('#printReport');

        linkContainer.removeAll();

        switch (status) {
            case B4.enums.TaskStatus.InProgress:
                linkContainer.add(
                    [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            anchor: '100%',
                            items: [
                                {
                                    xtype: 'label',
                                    html: '<h1 class="descTitle">Статус задачи:</h1>',
                                    width: 170
                                },
                                {
                                    xtype: 'label',
                                    html: '<h1 class="descSmallTitle">Выполняется</h1>',
                                    padding: '6 5 10 10'
                                }
                            ]
                        }
                    ]);
                printButton.setDisabled(true);
                break;
            case B4.enums.TaskStatus.Succeeded:
                linkContainer.add(
                    [
                        {
                            xtype: 'container',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    anchor: '100%',
                                    items: [
                                        {
                                            xtype: 'label',
                                            html: '<h1 class="descTitle">Статус задачи:</h1>',
                                            width: 170
                                        },
                                        {
                                            xtype: 'label',
                                            html: '<h1 class="descSmallTitle">Успешно выполнена</h1>',
                                            padding: '6 5 10 10'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'label',
                                            html: '<h1 class="descTitle">Отчет сформирован:</h1>'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Скачать',
                                            href: B4.Url.action('/fileupload/download?id=' + fileId),
                                            margin: '5 0 0 0'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    html: '<div class="warningXlsxReport">' +
                                        'Если у вас не открываются печатные формы,  необходимо скачать пакет обеспечения совместимости Microsoft Office' +
                                        '  <a href="http://www.microsoft.com/ru-ru/download/details.aspx?id=3" target="_blank">здесь</a>' +
                                        '</div>'
                                }
                            ]
                        }
                    ]);

                pb.hide();
                printButton.setDisabled(false);
                break;
            case B4.enums.TaskStatus.Error:
                linkContainer.add(
                    [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            anchor: '100%',
                            items: [
                                {
                                    xtype: 'label',
                                    html: '<h1 class="descTitle">Статус задачи:</h1>',
                                    width: 170
                                },
                                {
                                    xtype: 'label',
                                    html: '<h1 class="descSmallTitle">Не удалось сформировать</h1>',
                                    padding: '6 5 10 10'
                                }
                            ]
                        }
                    ]);

                pb.hide();
                printButton.setDisabled(false);
                break;
            default:
                return;
        }

        linkContainer.show();
    }
});