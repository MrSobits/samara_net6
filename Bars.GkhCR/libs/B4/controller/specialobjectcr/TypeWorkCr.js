Ext.define('B4.controller.specialobjectcr.TypeWorkCr', {
    /*
    * Контроллер раздела видов работ
    */
    extend: 'B4.controller.MenuItemController',

    requires:
    [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.specialobjectcr.TypeWork',
        'B4.view.specialobjectcr.TypeWorkCrMultiSelectWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.enums.TypeChangeProgramCr'
    ],

    models: [
        'specialobjectcr.TypeWorkCr',
        'specialobjectcr.TypeWorkCrHistory',
        'specialobjectcr.TypeWorkCrRemoval'
    ],

    stores: [
        'specialobjectcr.TypeWorkCr',
        'specialobjectcr.TypeWorkCrHistory'
    ],

    views: [
        'specialobjectcr.TypeWorkCrGrid',
        'specialobjectcr.TypeWorkCrPanel',
        'specialobjectcr.TypeWorkCrHistoryGrid',
        'specialobjectcr.TypeWorkCrEditWindow',
        'specialobjectcr.TypeWorkCrRemovalWindow',
        'specialobjectcr.TypeWorkCrMultiSelectWindow',
        'specialobjectcr.TypeWorkCrAddWindow',
    ],

    mainView: 'specialobjectcr.TypeWorkCrPanel',
    mainViewSelector: 'typeworkspecialcrpanel',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    refs: [
        {
            ref: 'typeWorkCrGrid',
            selector: 'typeworkspecialcrgrid'
        },
        {
            ref: 'workCrHistoryGrid',
            selector: 'typeworkspecialcrhistorygrid'
        },
        {
            ref: 'editWindow',
            selector: 'typeworkspecialcreditwindow'
        },
        {
            ref: 'reasonSuspensionChangeValBtn',
            selector: 'typeworkspecialcreditwindow changevalbtn[propertyName=ReasonSuspension]'
        }
    ],

    aspects: [
        {
            xtype: 'typeworkspecialobjectcrperm',
            name: 'typeWorkObjectCrPerm'
        },
        //todo добавить stateperm на вкладку история
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования видов работ
            */
            xtype: 'grideditctxwindowaspect',
            name: 'typeWorkCrGridWindowAspect',
            gridSelector: 'typeworkspecialcrgrid',
            editFormSelector: 'typeworkspecialcreditwindow',
            modelName: 'specialobjectcr.TypeWorkCr',
            editWindowView: 'specialobjectcr.TypeWorkCrEditWindow',
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' [name=FinanceSource]'] =
                {
                    'beforeload': { fn: me.onBeforeLoadFinanceSource, scope: me },
                    'change': { fn: me.onChangeFinanceSource, scope: me }
                };
                actions['typeworkcrmultiselectwindow b4selectfield[name=FinanceSource]'] =
                {
                    'beforeload': { fn: me.onBeforeLoadFinanceSource, scope: me }
                };
                actions[me.gridSelector + ' [name=Work]'] = { 'beforeload': { fn: me.onBeforeLoadWork, scope: me } };
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

            onChangeFinanceSource: function (newValue) {
                var me = this,
                    editWindow = me.controller.getEditWindow();

                if (newValue.value != null) {
                    var financeSourceId = editWindow.down('[name=FinanceSource]').getValue();
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
            listeners: {
                getdata: function (asp, record) {
                    var me = this;

                    if (!record.data.Id) {
                        record.data.ObjectCr = me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId');
                    }
                }
            },
            deleteRecord: function (record) {
                var me = this;

                if (record.getId()) {

                    Ext.Msg.confirm('Удаление записи', 'Существуют связанные записи в разделе Мониторинг СМР. Вы действительно хотите удалить запись и ее историю?', function (result) {
                        if (result == 'yes') {
                            me.showFormRemoval(record);
                        }
                    }, me);

                }
            },

            // показываем форму заполнения Причины удаления
            showFormRemoval: function (record) {
                var me = this,
                    view = 'specialobjectcr.TypeWorkCrRemovalWindow',
                    grid = me.getGrid(),
                    tabPanel = grid.up('tabpanel'),
                    removalWindow,
                    removalView = me.controller.getView(view),
                    removalModel = me.controller.getModel('specialobjectcr.TypeWorkCrRemoval'),
                    removalRecord = new removalModel({ Id: 0 });

                if (!removalView)
                    throw 'Не удалось найти вьюшку контроллера ' + view;

                // Заполняем нужными полями новый объект
                removalRecord.set('TypeWorkCr', record.get('Id'));
                removalRecord.set('WorkName', record.get('WorkName'));
                removalRecord.set('TypeReason', 30);
                removalRecord.set('YearRepair', record.get('YearRepair'));
                
                removalWindow = removalView.create({ constrain: true, renderTo: tabPanel ? tabPanel.getEl() : grid.getEl() });

                tabPanel ? tabPanel.add(removalWindow) : grid.add(removalWindow);

                removalWindow.loadRecord(removalRecord);

                removalWindow.show();
                removalWindow.center();

                removalWindow.down('b4savebutton').on('click', me.saveRequestRemovalHandler, me);

                removalWindow.down('b4closebutton').on('click', function () {
                    removalWindow.close();
                });

                removalWindow.getForm().isValid();
            },

            saveRequestRemovalHandler: function (btn) {
                var me = this,
                    grid = me.getGrid(),
                    rec,
                    from = btn.up('typeworkspecialcrremovalwindow'),
                    historyGrid = me.controller.getWorkCrHistoryGrid(),
                    fields,
                    invalidFields = '',
                    reasonValue;

                from.getForm().updateRecord();
                rec = from.getForm().getRecord();

                if (from.getForm().isValid()) {

                    reasonValue = from.down('combobox[name=TypeReason]').getValue();

                    if (reasonValue === 0) {
                        Ext.Msg.alert('Ошибка заполнения формы', 'Необходимо указать причину');
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
                            Ext.Msg.alert('Ошибка сохранения причины удаления', action.result.message);
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
                    Ext.Msg.alert('Ошибка заполнения формы', 'Не заполнены обязательные поля: ' + invalidFields);
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы журнал изменений
            */
            xtype: 'grideditctxwindowaspect',
            name: 'typeWorkCrPanelWindowAspect',
            gridSelector: 'typeworkspecialcrhistorygrid',
            modelName: 'specialobjectcr.TypeWorkCrHistory',
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
                actions[me.gridSelector + ' b4updatebutton'] =
                {
                    'click': { fn: me.onGridUpdate, scope: me }
                };
            },

            onGridUpdate: function (btn) {
                btn.up('grid').getStore().load();
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

                Ext.Msg.confirm('Восстановление вида работы', 'Восстановить запись во вкладке "Виды работ"?', function (result) {
                    if (result == 'yes') {
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('Restore', 'SpecialTypeWorkCrHistory'),
                            timeout: 9999999,
                            params: {
                                id: record.get('Id')
                            }
                        }).next(function (r) {
                            // обновляем список работ поскольку восстановили запись
                            mainView.down('typeworkspecialcrgrid').getStore().load();
                            mainView.down('typeworkspecialcrhistorygrid').getStore().load();
                            B4.QuickMsg.msg('Успешно', 'Удаленная запись успешно восстановлена', 'success');

                        }).error(function (e) {
                            Ext.Msg.alert('Ошибка при восстановлении', e.message || e);
                        });
                    }
                });
            }
        }
    ],
    
    onStoreBeforeLoad: function (store, operation) {
        var me = this;
        (operation.params || (operation.params = {})).objectCrId = me.getContextValue(me.getMainComponent(), 'objectcrId');
    },

    init: function () {
        var me = this,
            actions = {};

        actions['typeworkspecialcrgrid'] = {
            'store.beforeload': { fn: me.onStoreBeforeLoad, scope: me }
        };

        actions['typeworkspecialcrhistorygrid'] = {
            'store.beforeload': { fn: me.onStoreBeforeLoad, scope: me }
        };

        me.control(actions);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('typeworkspecialcrpanel');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');
        view.down('typeworkspecialcrgrid').getStore().load();

        me.getAspect('typeWorkObjectCrPerm').setPermissionsByRecord({ getId: function () { return id; } });
    }
});