Ext.define('B4.controller.objectcr.TypeWorkCr', {
    /*
    * Контроллер раздела видов работ
    */
    extend: 'B4.controller.MenuItemController',

    requires:
    [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.objectcr.TypeWork',
        'B4.view.objectcr.TypeWorkCrMultiSelectWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.Ajax',
        'B4.Url',
        'B4.enums.TypeChangeProgramCr',
        'B4.aspects.TypeWorkCrAddButton'
    ],

    models: [
        'objectcr.TypeWorkCr',
        'objectcr.TypeWorkCrHistory',
        'objectcr.TypeWorkCrRemoval',
        'objectcr.TypeWorkCrWorks',
        'objectcr.TypeWorkCrPotentialWorks',
    ],

    stores: [
        'objectcr.TypeWorkCr',
        'objectcr.TypeWorkCrHistory',
        'objectcr.TypeWorkCrWorks',
        'objectcr.TypeWorkCrPotentialWorks',
    ],

    views: [
        'objectcr.TypeWorkCrGrid',
        'objectcr.TypeWorkCrPanel',
        'objectcr.TypeWorkCrHistoryGrid',
        'objectcr.TypeWorkCrEditWindow',
        'objectcr.TypeWorkCrRemovalWindow',
        'B4.view.objectcr.TypeWorkSendWindow',
        'objectcr.TypeWorkCrMultiSelectWindow',
        'objectcr.TypeWorkCrAddWindow',
        'objectcr.TypeWorkChangeYearEditWindow',
        'objectcr.TypeWorkCrStage1RemovalWindow'
    ],

    mainView: 'objectcr.TypeWorkCrPanel',
    mainViewSelector: 'objectcr_type_work_cr_panel',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.objectcr.Navi',
    typeworkToMoveId: null,

    refs: [
        { ref: 'workCrHistoryGrid', selector: 'objectcr_type_work_cr_history_grid'},
        { ref: 'editWindow', selector: 'typeworkcreditwindow'},
        { ref: 'reasonSuspensionChangeValBtn', selector: 'typeworkcreditwindow changevalbtn[propertyName=ReasonSuspension]'},
        { ref: 'worksGrids', selector: '#workgridstoolbar'},
        { ref: 'worksGrid', selector: 'typeworkcrworksgrid'},
        { ref: 'potentialworksGrid', selector: 'typeworkcrpotentialworksgrid'}
    ],
    aspects: [
        {
            xtype: 'typeworkobjectcrperm',
            name: 'typeWorkObjectCrPerm'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'typeWorkCrWorksGridAspect',
            storeName: 'objectcr.TypeWorkCrWorks',
            modelName: 'objectcr.TypeWorkCrWorks',
            gridSelector: 'typeworkcrworksgrid'
        },
        /*
           аспект добавления видов работ если программа создаана на основе ДПКР, то виды работ добавляются из ДПКР
           если создана программа в ручную, то виды работ выбираются из спраовнчика
        */
        {
            xtype: 'typeworkcraddbuttonaspect',
            name: 'TypeWorkCrAddButtonAspect',
            showAddHistoryForm: function (record, finSourceValue) {
                var me = this,
                    view = 'objectcr.TypeWorkCrAddWindow',
                    mainView = me.controller.getMainView(),
                    grid = mainView.down('objectcr_type_work_cr_grid[_active]'),
                    tabPanel = mainView.up('tabpanel'),
                    addWindow,
                    addView = me.controller.getView(view),
                    addModel = me.controller.getModel('objectcr.TypeWorkCrRemoval'),
                    addRecord = new addModel({ Id: 0 });

                if (!addView)
                    throw 'Не удалось найти вьюшку контроллера ' + view;

                addRecord.set('WorkName', record.get('WorkName'));
                addRecord.set('TypeReason', 0);
                addRecord.set('YearRepair', record.get('CorrectionYear'));

                addWindow = addView.create({ constrain: true, renderTo: tabPanel ? tabPanel.getEl() : grid.getEl() });

                tabPanel ? tabPanel.add(addWindow) : grid.add(addWindow);

                addWindow.loadRecord(addRecord);

                addWindow.show();
                addWindow.center();

                addWindow.down('b4savebutton').on('click', me.addTypeWorkHandler, me);

                addWindow.down('b4closebutton').on('click', function () {
                    addWindow.close();
                });

                addWindow.params = {};

                addWindow.params.record = record;
                addWindow.params.finSourceValue = finSourceValue;

                addWindow.getForm().isValid();
            },

            addTypeWorkHandler: function (btn) {
                var me = this,
                    mainView = me.controller.getMainView(),
                    grid = mainView.down('objectcr_type_work_cr_grid[_active]'),
                    rec,
                    from = btn.up('objectcrtypeworkcraddwindow'),
                    fields,
                    invalidFields = '',
                    model = me.controller.getModel('objectcr.TypeWorkCr'),
                    typeWorkRecord = from.params.record,
                    finSourceValue = from.params.finSourceValue,
                    objectCrId = me.controller.getContextValue(mainView, 'objectcrId');

                from.getForm().updateRecord();
                rec = from.getForm().getRecord();

                if (from.getForm().isValid()) {

                    me.controller.mask('Сохранение работы', from);

                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('AddWorks', 'Dpkr'),
                        timeout: 9999999,
                        params: {
                            workId: typeWorkRecord.get('WorkId'),
                            objectCrId: objectCrId,
                            stage1Id: typeWorkRecord.get('Stage1Id'),
                            finSrcId: finSourceValue,
                            year: typeWorkRecord.get('CorrectionYear'),
                            newYear: rec.get('NewYearRepair'),
                            isSaveHistory: false
                        }
                    }).next(function (response) {
                        var resp = Ext.JSON.decode(response.responseText);

                        grid.getStore().load();

                        rec.set('TypeWorkCr', resp.Id);

                        from.submit({
                            url: rec.getProxy().getUrl({ action: 'create' }),
                            params: {
                                records: Ext.encode([rec.getData()])
                            },
                            success: function () {

                                me.controller.getStore('objectcr.TypeWorkCrHistory').load();
                                from.close();
                            },
                            failure: function (form, action) {
                                Ext.Msg.alert('Ошибка сохранения работы!', action.result.message);
                            }
                        });

                        model.load(resp.Id, {
                            success: function (rec) {

                                me.controller.unmask();
                                grid.fireEvent('rowaction', grid, 'edit', rec);

                            },
                            failure: function () {
                                me.controller.unmask();
                            },
                            scope: me
                        });

                    }).error(function (e) {
                        me.controller.unmask();
                        Ext.Msg.alert('Ошибка при добавлении', e.message || e);
                    });



                } else {
                    //получаем все поля формы
                    fields = from.getForm().getFields();

                    //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
                    Ext.each(fields.items, function (field) {
                        if (!field.isValid()) {
                            invalidFields += '<br>' + field.fieldLabel;
                        }
                    });

                    //выводим сообщение
                    Ext.Msg.alert('Ошибка заполнения формы!', 'Не заполнены обязательные поля: ' + invalidFields);
                }
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhCr.ObjectCr.Register.TypeWorkCrHistory.View', applyTo: 'objectcr_type_work_cr_history_grid', selector: 'objectcr_type_work_cr_panel' }],
            name: 'viewTypeWorkCrHistoryPerm',
            applyBy: function (component, allowed) {
                var me = this,
                    panel = component ? component.up('objectcr_type_work_cr_panel') : null,
                    typeWorkCrid = panel ? panel.down('objectcr_type_work_cr_grid[name=OnlyTypeWorks]') : null,
                    tabPanel = panel ? panel.down('tabpanel') : null,
                    tpTypeWorkCrid = tabPanel ? tabPanel.down('objectcr_type_work_cr_grid') : null,
                    objectCrId = me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId');

                if (tabPanel && allowed) {

                    // если права есть - это не значит, что вкладку еще можно показывать
                    // вкладка журнал изменений показывается во-первых по правам
                    // во-вторых только, если программе разрешено добаять работы из ДПКР

                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('UseAddWorkFromLongProgram', 'ObjectCr'),
                        params: {
                            objectCrId: objectCrId
                        }
                    }).next(function (response) {

                        var resp = Ext.JSON.decode(response.responseText);

                        if (resp) {
                            tabPanel.show();

                            if (typeWorkCrid) {
                                delete typeWorkCrid._active;
                                typeWorkCrid.hide();
                            }

                            if (tpTypeWorkCrid) {
                                tpTypeWorkCrid._active = true;
                            }
                        } else {
                            tabPanel.hide();

                            if (tpTypeWorkCrid) {
                                delete tpTypeWorkCrid._active;
                            }

                            if (typeWorkCrid) {
                                typeWorkCrid._active = true;
                                typeWorkCrid.show();
                            }
                        }

                        me.doLoadTypeWorCrGrid();
                    }).error(function () {
                        tabPanel.hide();

                        if (tpTypeWorkCrid) {
                            delete tpTypeWorkCrid._active;
                        }

                        if (typeWorkCrid) {
                            typeWorkCrid._active = true;
                            typeWorkCrid.show();
                        }
                    });

                }
                else if (tabPanel) {
                    tabPanel.hide();

                    if (tpTypeWorkCrid) {
                        delete tpTypeWorkCrid._active;
                    }

                    if (typeWorkCrid) {
                        typeWorkCrid._active = true;
                        typeWorkCrid.show();
                    }

                    me.doLoadTypeWorCrGrid();
                }

            },

            doLoadTypeWorCrGrid: function() {
                var grid = this.componentQuery('objectcr_type_work_cr_grid[_active]'),
                    store = grid.getStore(),
                    id = this.controller.getContextValue(this.controller.getMainComponent(), 'objectcrId');

                store.clearFilter(true);
                store.filter('objectCrId', id);
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования видов работ
            */
            xtype: 'grideditctxwindowaspect',
            name: 'typeWorkCrGridWindowAspect',
            gridSelector: 'objectcr_type_work_cr_grid[_active]',
            editFormSelector: 'typeworkcreditwindow',
            modelName: 'objectcr.TypeWorkCr',
            editWindowView: 'objectcr.TypeWorkCrEditWindow',
            changeYearWinSelector: 'typeworkchangeyeareditwin',
            changeYearWindow: 'objectcr.TypeWorkChangeYearEditWindow',
            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                        case 'changeyear':
                            this.changeYear(record);
                            break;
                    }
                }
            },
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' #sflFinanceSource'] =
                {
                    'beforeload': { fn: me.onBeforeLoadFinanceSource, scope: me },
                    'change': { fn: me.onChangeFinanceSource, scope: me }
                };
                actions['typeworkcrmultiselectwindow b4selectfield[name=FinanceSource]'] =
                {
                    'beforeload': { fn: me.onBeforeLoadFinanceSource, scope: me }
                };
                actions['typeworkcreditwindow'] =
                {
                    'afterrender': { fn: me.onEditWindowAfterRender, scope: me }
                };
                actions[me.editFormSelector + ' #sflWork'] = { 'beforeload': { fn: me.onBeforeLoadWork, scope: me } };
                actions[me.editFormSelector + ' #sflWork'] = { 'beforeload': { fn: me.onBeforeLoadWork, scope: me } };
                actions['typeworkcrsendwindow b4savebutton'] = { 'click': { fn: this.pleaseDoMove, scope: this } };
                actions['typeworkcreditwindow #sendToOtherPeriodButton'] = { 'click': { fn: this.moveToAnotherProgram, scope: this } };
                actions[me.changeYearWinSelector + ' b4savebutton'] = { 'click': { fn: me.onSaveChangeYear, scope: me } };
            },
            moveToAnotherProgram: function (btn) {
                typeworkToMoveId = 0;
                var me = this,
                    win = btn.up('window');
                var sflWork = win.down('#sflWork');
                var workProxy = sflWork.value;
                var workName = workProxy['Name'];
                var record = win.getRecord();
                var twId = record.get('Id');
                typeworkToMoveId = twId;
                win.close();
                gisinfo = Ext.create('B4.view.objectcr.TypeWorkSendWindow');
                var taDescription = gisinfo.down('#taDescription');
                taDescription.setValue('Работа/Услуга ' + workName + ' будет перенесена в выбранную программу. Обязательно выберите программу в окне выбора программ КПР.'
                    + 'Если в выбранной программе нет зобъекта КР по данному дому, он будет создан автоматически. Для завершения переноса нажмите Сохранить');
                gisinfo.show();
            },
            pleaseDoMove: function (btn) {
                var me = this,
                    win = btn.up('window');
                var sfProgramCrObj = win.down('#sfProgramCrObj');
                var programProxy = sfProgramCrObj.value;
                var programId = programProxy['Id'];
                me.mask('Перенос работы', win);
                var result = B4.Ajax.request(B4.Url.action('MoveTypeWork', 'ProgramVersion', {
                    programId: programId,
                    typeworkToMoveId: typeworkToMoveId
                }
                )).next(function (response) {
                    me.unmask();
                    return true;
                })
                    .error(function (resp) {
                        Ext.Msg.alert('Ошибка', resp.message);
                        me.unmask();
                    });
                win.close();
                debugger;
                var grid = this.componentQuery('objectcr_type_work_cr_grid[_active]'),
                    store = grid.getStore(),
                    id = this.controller.getContextValue(this.controller.getMainComponent(), 'objectcrId');
                store.clearFilter(true);
                store.filter('objectCrId', id);

            },
            onBeforeLoadFinanceSource: function (store, operation) {
                var me = this,
                    objectCrId = me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId');
                if (objectCrId) {
                    operation.params = operation.params || {};
                    operation.params.objectCrId = objectCrId;
                }
            },
            onBeforeLoadWork: function (store, operation) {
                var me = this,
                    editWindow = me.controller.getEditWindow();

                if (editWindow.onlyByWorkId) {
                    operation.params = operation.params || {};
                    operation.params.onlyByWorkId = editWindow.onlyByWorkId;
                    operation.params.ids = editWindow.ids;
                }
            },
            onEditWindowAfterRender: function (view) {
                var me = this,
                    sfWork,
                    tfYearRepair,
                    resp;
              var m = me.controller;
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('UseAddWorkFromLongProgram', 'ObjectCr'),
                    params: {
                        objectCrId: me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId')
                    }
                }).next(function (response) {
                    sfWork = view.down('b4selectfield[name=Work]');
                    tfYearRepair = view.down('numberfield[name=YearRepair]');

                    resp = Ext.JSON.decode(response.responseText);
                    if (resp) {
                        sfWork.setDisabled(true);
                        tfYearRepair.allowBlank = false,
                        tfYearRepair.show();
                    } else {
                        sfWork.setDisabled(false);
                        tfYearRepair.allowBlank = true,
                        tfYearRepair.hide();
                    }
                    //view.isValid();
                    //view.doLayout();

                }).error(function (response) {
                    Ext.Msg.alert('Ошибка', response.message);
                });
            },

            onChangeFinanceSource: function (newValue) {
                var me = this,
                    editWindow = me.controller.getEditWindow();

                if (newValue.value != null) {
                    var financeSourceId = editWindow.down('#sflFinanceSource').getValue();
                    me.controller.mask('Загрузка', me.controller.getMainComponent());

                    B4.Ajax.request(B4.Url.action('ListWorksByFinSource', 'FinanceSourceWork', {
                        financeSourceId: financeSourceId
                    })).next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText);
                        if (obj.ids) {
                            obj.ids = obj.ids.join();

                            editWindow.ids = obj.ids;
                            editWindow.onlyByWorkId = true; //флаг получения работ по переданным Id
                        }
                        me.controller.unmask();
                        return true;
                    }).error(function () {
                        me.controller.unmask();
                    });
                }
            },
            onSaveSuccess: function (asp, rec) {
                debugger;
                // перекрываем чтобы окно не закрывалось после сохранения
                if (rec.data.MaxCost && rec.data.MaxCost < rec.data.Sum) 
                    B4.QuickMsg.msg('Внимание!', 'Стоимость больше максимальной стоимости для работы', 'warning', 5000);
                else
                    B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form)
                {
                    var me = this,
                        worksgrid = me.controller.getWorksGrid(),
                        worksstore = worksgrid.getStore(),
                        pworksgrid = me.controller.getPotentialworksGrid(),
                        pworksstore = pworksgrid.getStore(),
                        workspanel = me.controller.getWorksGrids();

                        workspanel.setVisible(false);

                        worksstore.filter('typeWorkId', record.getId());
                        pworksstore.filter('typeWorkId', record.getId());

                        B4.Ajax.request({
                            url: B4.Url.action('IsPSD', 'TypeWorkCrWorks'),
                            params: {
                                typeWorkId: record.getId()
                            }
                        }).next(function (response) {
                            if (workspanel.body)
                            {
                                ispsd = Ext.JSON.decode(response.responseText).data;
                                workspanel.setDisabled(!ispsd);
                                workspanel.setVisible(ispsd);
                                me.controller.updateMaxCost();
                            }
                        }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                        });
                },
                getdata: function (asp, record) {
                    var me = this;

                    if (!record.data.Id) {
                        record.data.ObjectCr = me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId');
                    }
                },
            },
            deleteRecord: function (record) {
                var me = this;

                if (record.getId()) {

                    Ext.Msg.confirm('Удаление записи!', 'Существуют связанные записи в разделе Мониторинг СМР. Вы действительно хотите удалить запись и ее историю?', function (result) {
                        if (result == 'yes') {
                            me.showFormRemoval(record);
                        }
                    }, me);

                }
            },

            // показываем форму заполнения Причины удаления
            showFormRemoval: function (record) {
                var me = this,
                    view = 'objectcr.TypeWorkCrRemovalWindow',
                    grid = me.getGrid(),
                    tabPanel = grid.up('tabpanel'),
                    removalWindow,
                    removalView = me.controller.getView(view),
                    removalModel = me.controller.getModel('objectcr.TypeWorkCrRemoval'),
                    removalRecord = new removalModel({ Id: 0 });

                if (!removalView)
                    throw 'Не удалось найти вьюшку контроллера ' + view;

                // Заполняем нужными полями новый объект
                removalRecord.set('TypeWorkCr', record.get('Id'));
                removalRecord.set('WorkName', record.get('WorkName'));
                removalRecord.set('TypeReason', 30);
                removalRecord.set('YearRepair', record.get('YearRepair'));

                // ра
                removalWindow = removalView.create({ constrain: true, renderTo: tabPanel ? tabPanel.getEl() : grid.getEl() });

                tabPanel ? tabPanel.add(removalWindow) : grid.add(removalWindow);

                removalWindow.loadRecord(removalRecord);

                removalWindow.show();
                removalWindow.center();

                removalWindow.down('b4savebutton').on('click', me.saveRequestRemovalHandler, me);

                removalWindow.down('combobox[name=TypeReason]').on('change', me.comboboxTypeReasonChange, me);

                removalWindow.down('b4closebutton').on('click', function () {
                    removalWindow.close();
                });

                removalWindow.getForm().isValid();
            },

            comboboxTypeReasonChange: function (combobox, newValue, oldValue) {
                var form = combobox.up('objectcr_type_work_cr_removal_window'),
                    newYearCmp = form.down('numberfield[name=NewYearRepair]');

                newYearCmp.setValue(null);
                if (newValue === 20) {
                    newYearCmp.readOnly = false;
                    newYearCmp.allowBlank = false;
                    newYearCmp.setDisabled(false);
                } else if (newValue === 30) {
                    newYearCmp.readOnly = true;
                    newYearCmp.allowBlank = true;
                    newYearCmp.setDisabled(true);
                }
                else {
                    newYearCmp.readOnly = true;
                    newYearCmp.allowBlank = true;
                    newYearCmp.setDisabled(true);
                }

                form.getForm().isValid();
            },

            saveRequestRemovalHandler: function (btn) {
                var me = this,
                    grid = me.getGrid(),
                    rec,
                    from = btn.up('objectcr_type_work_cr_removal_window'),
                    historyGrid = me.controller.getWorkCrHistoryGrid(),
                    fields,
                    invalidFields = '',
                    reasonValue;

                from.getForm().updateRecord();
                rec = from.getForm().getRecord();

                if (from.getForm().isValid()) {

                    reasonValue = from.down('combobox[name=TypeReason]').getValue();

                    if (reasonValue === 0) {
                        Ext.Msg.alert('Ошибка заполнения формы!', 'Необходимо указать причину');
                        return;
                    }

                    me.mask('Сохранение причины удаления работы', from);
                    from.submit({
                        url: rec.getProxy().getUrl({ action: 'create' }),
                        params: {
                            records: Ext.encode([rec.getData()])
                        },
                        success: function () {
                            me.unmask();
                            grid.getStore().load();
                            historyGrid.getStore().load();
                            from.close();
                        },
                        failure: function (form, action) {
                            me.unmask();
                            Ext.Msg.alert('Ошибка сохранения причины удаления!', action.result.message);
                        }
                    });

                } else {
                    //получаем все поля формы
                    fields = from.getForm().getFields();

                    //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
                    Ext.each(fields.items, function (field) {
                        if (!field.isValid()) {
                            invalidFields += '<br>' + field.fieldLabel;
                        }
                    });

                    //выводим сообщение
                    Ext.Msg.alert('Ошибка заполнения формы!', 'Не заполнены обязательные поля: ' + invalidFields);
                }
            },
            changeYear: function (record) {
                var me = this,
                    window = me.getChangeYearWindow(),
                    workField = window.down('[name=Work]'),
                    store = window.down('objectcrtypeworkst1grid').getStore();

                workField.setValue(record.get('WorkName'));

                store.clearFilter(true);
                store.filter('typeWorkId', record.get('Id'));
                window.typeWorkId = record.get('Id');
                window.show();
            },
            getChangeYearWindow: function () {
                var me = this;

                if (me.changeYearWinSelector) {
                    var changeYearWin = Ext.ComponentQuery.query(me.changeYearWinSelector)[0];

                    if (changeYearWin && changeYearWin.isHidden() && changeYearWin.rendered) {
                        changeYearWin = changeYearWin.destroy();
                    }

                    if (!changeYearWin) {
                        changeYearWin = me.controller.getView(me.changeYearWindow).create({ constrain: true, autoDestroy: true });
                        if (B4.getBody().getActiveTab()) {
                            B4.getBody().getActiveTab().add(changeYearWin);
                        } else {
                            B4.getBody().add(changeYearWin);
                        }
                    }
                    return changeYearWin;
                }

                return null;
            },
            onSaveChangeYear: function (btn) {
                var me = this,
                    win = btn.up('window'),
                    recs = win.down('objectcrtypeworkst1grid').getStore().getRange(),
                    records;

                records = Ext.Array.map(recs, function (rec) {
                    return {
                        Id: rec.get('Id'),
                        Year: rec.get('Year')
                    };
                });

                me.controller.mask('Изменение года', win);

                B4.Ajax.request(B4.Url.action('ChangeYear', 'TypeWorkCr', {
                    records: Ext.encode(records),
                    typeWorkId: win.typeWorkId
                })).next(function () {
                    win.close();
                    me.updateGrid();
                    me.controller.unmask();
                    Ext.Msg.alert('Изменение года', 'Выполнено успешно');
                    return true;
                }).error(function (err) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err);
                });

            }
        },
        {
            /*
            * Аспект взаимодействия таблицы журнал изменений
            */
            xtype: 'grideditctxwindowaspect',
            name: 'typeWorkCrPanelWindowAspect',
            gridSelector: 'objectcr_type_work_cr_history_grid',
            modelName: 'objectcr.TypeWorkCrHistory',
            otherActions: function (actions) {
                var me = this;

                actions[me.gridSelector + ' button[name=Restore]'] =
                {
                    'click': { fn: me.onClickRestore, scope: me }
                };
                actions[me.gridSelector] =
                {
                    'activate': { fn: me.onActivate, scope: me }
                };
            },
            onActivate: function () {
                var grid = this.getGrid();
                grid.getStore().load();
            },
            onClickRestore: function (btn) {
                var me = this,
                    mainView = me.controller.getMainView(),
                    grid = btn.up('grid'),
                    recs = grid.getSelectionModel().getSelection(),
                    record;

                if (!recs || recs.length != 1) {
                    Ext.Msg.alert('Восстановление вида работы', 'Необходимо выбрать одну запись с признаком "Действие" = "Удаление"!');
                    return false;
                }

                record = recs[0];

                if (record.get('TypeAction') != 30) {
                    Ext.Msg.alert('Восстановление вида работы', 'Необходимо выбрать одну запись с признаком "Действие" = "Удаление"!');
                    return false;
                }

                Ext.Msg.confirm('Восстановление вида работы!', 'Восстановить запись во вкладке "Виды работ"?', function (result) {
                    if (result == 'yes') {
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('Restore', 'TypeWorkCrHistory'),
                            timeout: 9999999,
                            params: {
                                id: record.get('Id')
                            }
                        }).next(function (r) {
                            // обновляем список работ поскольку восстановили запись
                            mainView.down('objectcr_type_work_cr_grid[_active]').getStore().load();
                            mainView.down('objectcr_type_work_cr_history_grid').getStore().load();
                            B4.QuickMsg.msg('Успешно', 'Удаленная запись успешно восстановлена', 'success');

                        }).error(function (e) {
                            Ext.Msg.alert('Ошибка при восстановлении', e.message || e);
                        });
                    }
                });
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['objectcr_type_work_cr_grid'] = {
            afterrender: { fn: me.onAfterRenderMainView, scope: me }
        };

        me.control({
            'objectcr_type_work_cr_grid[_active] #updateGrid': { afterrender: { fn: me.onAfterRenderMainView, scope: me } },
            'objectcrtypeworkst1grid': { rowaction: { fn: me.st1GridRowAction, scope: me } },
            'typeworkcrpotentialworksgrid actioncolumn[action="addWork"]': { click: { fn: this.addWork, scope: this } },
            'typeworkcrworksgrid actioncolumn[action="deleteWork"]': { click: { fn: this.deleteWork, scope: this } },
            'typeworkcreditwindow #sflWork': { 'change': { fn: me.updateMaxCost, scope: me } },
            'typeworkcreditwindow #nfYearRepair': { 'change': { fn: me.updateMaxCost, scope: me } },
            'typeworkcreditwindow #dfVolume': { 'change': { fn: me.updateMaxCost, scope: me } },
        });

        me.control(actions);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('objectcr_type_work_cr_panel'),
            storeHistory;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.setContextValue(view, 'typeWorkKind', Gkh.config.GkhCr.DpkrConfig.AddTypeWorkKind);
        me.application.deployView(view, 'objectcr_info');

        storeHistory = view.down('objectcr_type_work_cr_history_grid').getStore();
        storeHistory.clearFilter(true);
        storeHistory.filter('objectCrId', id);

        me.getAspect('typeWorkObjectCrPerm').setPermissionsByRecord({ getId: function () { return id; } });
        me.getAspect('viewTypeWorkCrHistoryPerm').setPermissionsByRecord({ getId: function () { return id; } });
        me.getAspect('TypeWorkCrAddButtonAspect').on('savetypework', function () {
            var gridStore = view.down('objectcr_type_work_cr_grid[_active]').getStore();
            gridStore.clearFilter(true);
            gridStore.filter('objectCrId', id);
        });
    },

    onAfterRenderMainView: function (view) {
        var me = this,
            groupColumn = view.down('[name=groupDpkr]'),
            changeYearColumn = view.down('actioncolumn[type=changeyear]'),
            addButton = view.down('b4addbutton'),
            addDpkrButton = view.down('button[name=AddButtonDpkr]'),
            objectCrId = me.getContextValue(me.getMainComponent(), 'objectcrId');

        me.mask('Загрузка', view);

        B4.Ajax.request({
            method: 'POST',
            url: B4.Url.action('UseAddWorkFromLongProgram', 'ObjectCr'),
            params: {
                objectCrId: objectCrId
            }
        }).next(function (response) {
            var resp = Ext.JSON.decode(response.responseText),
                isShow = false;

            if (resp) {
                isShow = true;
            }

            B4.Ajax.request({
                url: B4.Url.action('GetObjectSpecificPermissions', 'Permission'),
                params: {
                    ids: Ext.encode([objectCrId]),
                    permissions: Ext.encode(['GkhCr.ObjectCr.Register.TypeWork.Create'])
                },
                timeout: 999999
            }).next(function (responsePermission) {
                var grants = Ext.JSON.decode(responsePermission.responseText)[0];

                if (resp && grants[0]) {
                    addButton.hide();
                    addDpkrButton.show();
                } else {
                    addButton.show();
                    addDpkrButton.hide();
                }
                view.doLayout();
            });

            if (groupColumn) {

                groupColumn.setVisible(isShow);

                Ext.each(groupColumn.items.items, function (col) {
                    col.setVisible(isShow);
                });

                view.getView().refresh();
            }

            if (isShow && Gkh.config.GkhCr.DpkrConfig.TypeWorkTransferType == 10) {
                changeYearColumn.show();
            } else {
                changeYearColumn.hide();
            }

            me.unmask();
        }).error(function () {
            me.unmask();
        });

    },

    st1GridRowAction: function (grid, action, record) {
        var me = this;
        if (!grid || grid.isDestroyed) return;

        if (action.toLowerCase() === 'delete') {

            if (grid.getStore().count() === 1) {

                Ext.Msg.confirm('Удаление записи!', 'Существуют связанные записи в разделе Мониторинг СМР. Вы действительно хотите удалить запись и ее историю?', function (result) {
                    if (result === 'yes') {
                        me.showSt1FormRemoval(grid, record);
                    }
                }, me);

            } else {
                me.showSt1FormRemoval(grid, record);
            }
        }
    },

    showSt1FormRemoval: function (grid, record) {
        var me = this,
            view = 'objectcr.TypeWorkCrStage1RemovalWindow',
            removalWindow,
            removalView = me.getView(view),
            removalModel = me.getModel('objectcr.TypeWorkCrRemoval'),
            removalRecord = new removalModel({ Id: 0 });

        if (!removalView)
            throw 'Не удалось найти вьюшку контроллера ' + view;

        removalRecord.set('TypeWorkCr', record.get('TypeWorkCr'));
        removalRecord.set('WorkName', record.get('WorkName'));
        removalRecord.set('TypeReason', 30);
        removalRecord.set('YearRepair', record.get('Year'));
        removalRecord.set('StructElement', record.get('StructuralElement'));
        removalRecord.set('TypeWorkSt1', record.get('Id'));

        removalWindow = removalView.create({ constrain: true, renderTo: B4.getBody().getActiveTab().getEl() });

        removalWindow.loadRecord(removalRecord);

        removalWindow.show();
        removalWindow.center();

        removalWindow.down('b4savebutton').on('click', me.saveRequestRemovalHandler, me);

        removalWindow.down('combobox[name=TypeReason]').on('change', me.comboboxTypeReasonChange, me);

        removalWindow.down('b4closebutton').on('click', function () {
            removalWindow.close();
        });

        removalWindow.getForm().isValid();
    },

    comboboxTypeReasonChange: function (combobox, newValue, oldValue) {
        var form = combobox.up('typeworkcrst1removalwin'),
            newYearCmp = form.down('numberfield[name=NewYearRepair]');

        newYearCmp.setValue(null);
        if (newValue === 20) {
            newYearCmp.readOnly = false;
            newYearCmp.allowBlank = false;
            newYearCmp.setDisabled(false);
        } else if (newValue === 30) {
            newYearCmp.readOnly = true;
            newYearCmp.allowBlank = true;
            newYearCmp.setDisabled(true);
        }
        else {
            newYearCmp.readOnly = true;
            newYearCmp.allowBlank = true;
            newYearCmp.setDisabled(true);
        }

        form.getForm().isValid();
    },

    saveRequestRemovalHandler: function (btn) {
        var me = this,
            rec,
            isSameId,
            from = btn.up('typeworkcrst1removalwin'),
            fields,
            invalidFields = '',
            reasonValue;

        from.getForm().updateRecord();
        rec = from.getForm().getRecord();

        if (from.getForm().isValid()) {

            reasonValue = from.down('combobox[name=TypeReason]').getValue();

            if (reasonValue === 0) {
                Ext.Msg.alert('Ошибка заполнения формы!', 'Необходимо указать причину');
                return;
            }

            me.mask('Сохранение причины удаления работы', from);

            B4.Ajax.request({
                method: 'POST',
                url: B4.Url.action('SplitStructElementInTypeWork', 'TypeWorkCr'),
                timeout: 9999999,
                params: {
                    typeWorkId: rec.get('TypeWorkCr'),
                    st1Id: rec.get('TypeWorkSt1')
                }
            }).next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                isSameId = obj.TypeWork == rec.get('TypeWorkCr');

                rec.set('TypeWorkCr', obj.TypeWork);
                me.internalSaveRemoval(from, rec, isSameId);
            }).error(function (e) {
                Ext.Msg.alert('Ошибка', e.message || e);
            });

        } else {
            //получаем все поля формы
            fields = from.getForm().getFields();

            //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
            Ext.each(fields.items, function (field) {
                if (!field.isValid()) {
                    invalidFields += '<br>' + field.fieldLabel;
                }
            });

            //выводим сообщение
            Ext.Msg.alert('Ошибка заполнения формы!', 'Не заполнены обязательные поля: ' + invalidFields);
        }
    },

    internalSaveRemoval: function (from, rec, isSameId) {
        var me = this,
            changeYearWindow = me.getChangeYearWindow(),
            grid = changeYearWindow.down('objectcrtypeworkst1grid'),
            typeWorkCrGrid = me.getMainView().down('objectcr_type_work_cr_grid[_active]');

        from.submit({
            url: rec.getProxy().getUrl({ action: 'create' }),
            params: {
                records: Ext.encode([rec.getData()])
            },
            success: function () {
                me.unmask();
                grid.getStore().load();
                me.getStore('objectcr.TypeWorkCrHistory').load();
                from.close();

                if (isSameId && changeYearWindow) {
                    changeYearWindow.close();
                    typeWorkCrGrid.getStore().load();
                }
            },
            failure: function (form, action) {
                me.unmask();
                Ext.Msg.alert('Ошибка сохранения причины удаления!', action.result.message);
            }
        });
    },

    addWork: function (grid, rowIndex, colIndex, param, param2, rec, asp) 
    {
        var me = this,
            edit = me.getEditWindow(),
            grid = me.getWorksGrid(),
            pgrid = me.getPotentialworksGrid(),
            psdtypeWork = edit.getRecord(),
            psdtypeWorkId = psdtypeWork.getId();
             
        me.mask('Добавление...');

        B4.Ajax.request({
                url: B4.Url.action('AddWork', 'TypeWorkCrWorks'),
                params: 
                {
                    psdtypeWorkId: psdtypeWorkId,
                    typeWorkId: rec.getId()
                }
                }).next(function () {
                    grid.getStore().load();
                    pgrid.getStore().load();
                    me.unmask();
                }).error(function (err) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err);
                });
    },
    deleteWork: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this,
            edit = me.getEditWindow(),
            grid = me.getWorksGrid(),
            pgrid = me.getPotentialworksGrid(),
            psdtypeWork = edit.getRecord(),
            psdtypeWorkId = psdtypeWork.getId();

        me.mask('Удаление...');

        B4.Ajax.request({
            url: B4.Url.action('DeleteWork', 'TypeWorkCrWorks'),
            params:
            {
                psdtypeWorkId: psdtypeWorkId,
                typeWorkId: rec.getId()
            }
        }).next(function () {
            grid.getStore().load();
            pgrid.getStore().load();
            me.unmask();
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    updateMaxCost: function()
    {
        var edit = this.getEditWindow(),
            rec = edit.getRecord(),
            work = edit.down('[name=Work]').getValue(),
            yearrepair = edit.down('[name=YearRepair]').getValue(),
            volume = edit.down('[name=Volume]').getValue(),
            maxcostedit = edit.down('[name=MaxCost]');

        B4.Ajax.request({
            url: B4.Url.action('GetCostKpkr', 'CostLimit'),
            params:
            {
                TypeWorkCrId: rec.getId(),
                WorkId: work,
                YearRepair: yearrepair,
                Volume: volume
            }
        }).next(function (resp) {
            var response = JSON.parse(resp.responseText);
            maxcostedit.setValue(response); 
            rec.data.MaxCost = response;
        }).error(function (resp) {
            //maxcostedit.setValue(0); 
        }); 
    },
});