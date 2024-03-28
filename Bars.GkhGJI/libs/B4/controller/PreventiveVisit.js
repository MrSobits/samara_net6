Ext.define('B4.controller.PreventiveVisit', {
    extend: 'B4.base.Controller',
    params: null,
    currentResultId: null,
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.permission.Disposal',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GkhInlineGrid',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'PreventiveVisit',
        'RealityObject',
        'preventivevisit.Witness',
        'preventivevisit.RealityObject',
        'preventivevisit.Period',
        'preventivevisit.ResultViolation',
        'preventivevisit.Result',
        'preventivevisit.Annex'
    ],

    stores: [
        'PreventiveVisit',
        'preventivevisit.Witness',
        'preventivevisit.RealityObject',
        'preventivevisit.Annex',
        'dict.Inspector',
        'dict.ViolationGjiForSelect',
        'dict.ViolationGjiForSelected',
        'preventivevisit.Result',
        'preventivevisit.ResultViolation',
        'preventivevisit.ListRoForResultPV',
        'preventivevisit.Period',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'dict.Municipality'
    ],

    views: [
        'preventivevisit.EditPanel',
        'preventivevisit.AnnexEditWindow',
        'preventivevisit.AnnexGrid',
        'preventivevisit.PeriodEditWindow',
        'preventivevisit.PeriodGrid',
        'preventivevisit.ResultEditWindow',
        'preventivevisit.ResultGrid',
        'preventivevisit.WitnessGrid',
        'preventivevisit.ResultViolationGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'preventivevisit.EditPanel',
    mainViewSelector: '#preventivevisitEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        //{
        //    xtype: 'gkhgjidigitalsignaturegridaspect',
        //    gridSelector: 'preventivevisitannexgrid',
        //    controllerName: 'PreventiveVisitAnnex',
        //    name: 'preventivevisitannexSignatureAspect',
        //    signedFileField: 'SignedFile'
        //},
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'preventivevisitPrintAspect',
            buttonSelector: '#preventivevisitEditPanel #btnPrint',
            codeForm: 'PreventiveVisit',
            getUserParams: function (reportId) {
                var me = this,
                    param = { Id: me.controller.params.documentId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'preventivevisitStateButtonAspect',
            stateButtonSelector: '#preventivevisitEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('preventivevisitEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            /*
            Аспект основной панели Карточки распоряжения
            В нем вешаемся на событие aftersetpaneldata, чтобы загрузить подчиенные сторы
            А также проставить дополнительные значения
            Вешаемся на savesuccess чтобы после сохранения сразу получить Номер и обновить Вкладку
            */
            xtype: 'gjidocumentaspect',
            name: 'preventivevisitEditPanelAspect',
            editPanelSelector: '#preventivevisitEditPanel',
            modelName: 'PreventiveVisit',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onChangePerson, scope: this } };
                actions[this.editPanelSelector + ' button[action=ViewVideo]'] = { 'click': { fn: this.ViewVideo, scope: this } };
                actions[this.editPanelSelector + ' button[action=ERKNMRequest]'] = { 'click': { fn: this.ERKNMRequest, scope: this } };
            },
            ERKNMRequest: function (btn) {
                var me = this,
                    panel = btn.up('#preventivevisitEditPanel'),
                    record = panel.getForm().getRecord();
                debugger;
                var recId = record.getId();
                Ext.Msg.confirm('Запрос в ЕРКНМ', 'Подтвердите размещение проверки в ЕРКНМ', function (result) {
                    if (result == 'yes') {
                        me.mask('Отправка запроса', B4.getBody());
                        B4.Ajax.request({
                            url: B4.Url.action('SendERKNMRequest', 'ERKNMExecute'),
                            method: 'POST',
                            timeout: 100 * 60 * 60 * 3,
                            params: {
                                docId: recId,
                                typeDoc: 30
                            }
                        }).next(function () {
                            B4.QuickMsg.msg('СМЭВ', 'Запрос на  размещение ПМ в ЕРКНМ отправлен', 'success');
                            me.unmask();
                        }, me)
                            .error(function (result) {
                                if (result.responseData || result.message) {
                                    Ext.Msg.alert('Ошибка отправки запроса!', Ext.isString(result.responseData) ? result.responseData : result.message);
                                }
                                me.unmask();
                            }, me);

                    }
                }, this);
            },
            ViewVideo: function (btn) {
                var me = this,
                    panel = btn.up('#preventivevisitEditPanel'),
                    record = panel.getForm().getRecord();
                debugger;
                var recId = record.get('VideoLink');
                new Ext.Window({
                    title: 'Проcмотр',
                    layout: { align: 'stretch' },
                    renderTo: B4.getBody().getActiveTab().getEl(), //pnlView, //view.getEl(),
                    constrain: true,
                    autoScroll: true,
                    html: recId,
                    maximizable: true
                }).show();

            },
            onChangePerson: function (field, newValue) {
                var panel = this.getPanel(),
                    sfContragent = panel.down('#sfContragent'),
                    tfPhysicalPerson = panel.down('#tfPhysicalPerson'),
                    tfPhysicalPersonInfo = panel.down('#tfPhysicalPersonInfo'),
                    tfPhysicalPersonINN = panel.down('#tfPhysicalPersonINN'),
                    tfPhysicalPersonAddress = panel.down('#tfPhysicalPersonAddress');

                switch (newValue) {
                    case 10://физлицо
                        sfContragent.hide();
                        tfPhysicalPerson.show();
                        tfPhysicalPersonInfo.show();
                        tfPhysicalPersonINN.show();
                        tfPhysicalPersonAddress.show();
                        break;
                    case 20://организация
                        sfContragent.show();
                        tfPhysicalPerson.hide();
                        tfPhysicalPersonInfo.hide();
                        tfPhysicalPersonINN.hide();
                        tfPhysicalPersonAddress.hide();
                        break;
                    case 30://должностное лицо
                        sfContragent.show();
                        tfPhysicalPerson.show();
                        tfPhysicalPersonInfo.show();
                        tfPhysicalPersonINN.show();
                        tfPhysicalPersonAddress.show();
                        break;
                }
            },
            saveRecord: function (rec) {
                var me = this;

                Ext.Msg.confirm('Внимание!', 'Инспектор!!! Убедитесь, что вид контроля/надзора указан правильно!!!!', function (result) {
                    if (result == 'yes') {
                        var kindknd = rec.get('KindKND');
                        if (kindknd != B4.enums.KindKND.NotSet) {
                            if (me.fireEvent('beforesave', me, rec) !== false) {
                                if (me.hasUpload()) {
                                    me.saveRecordHasUpload(rec);
                                } else {
                                    me.saveRecordHasNotUpload(rec);
                                }
                            }
                        }
                        else {
                            Ext.Msg.alert('Внимание!', 'Почему вы нажали ДА? Вид контроля/надзора же не указан!!!');
                        }
                    }
                });
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    callbackUnMask;
                asp.controller.params = asp.controller.params || {};
                //      asp.controller.documentId = rec.getId();
                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber'))
                    panel.setTitle("Акт профилактического визита" + " " + rec.get('DocumentNumber'));
                else
                    panel.setTitle("Акт профилактического визита");

                panel.down('#preventivevisitTabPanel').setActiveTab(0);
                //var tabpanel = panel.down('#preventivevisitTabPanel');
                //var requsitePanel = tabpanel.down('#requsitePanel');
                //получаем вид проверки

                //Делаем запросы на получение Инспекторов
                //и обновляем соответсвующие Тригер филды
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('GetInfo', 'PreventiveVisitOperations', { documentId: asp.controller.params.documentId }),
                    //для IE, чтобы не кэшировал GET запрос
                    cache: false
                }).next(function (response) {
                    asp.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText),
                        fieldInspectors = panel.down('#trigFInspectors');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);
                    asp.disableButtons(false);
                }).error(function () {
                    asp.controller.unmask();
                });

                // Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('preventivevisitStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем отчеты
                me.controller.getAspect('preventivevisitPrintAspect').loadReportStore();

            },
            onSaveSuccess: function (asp, rec) {
                this.getPanel().setTitle(asp.controller.params.title + " " + rec.get('DocumentNumber'));
            },
            disableButtons: function (value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup'),
                    idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /Disposal/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'preventivevisitInspectorMultiSelectWindowAspect',
            fieldSelector: '#preventivevisitEditPanel #trigFInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#preventivevisitInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия таблицы Лица присутсвующие при проверке, как инлайн грид
            */
            xtype: 'gkhinlinegridaspect',
            name: 'preventivevisitWitnessAspect',
            storeName: 'preventivevisit.Witness',
            modelName: 'preventivevisit.Witness',
            gridSelector: 'preventivevisitwitnessgrid',
            saveButtonSelector: 'preventivevisitwitnessgrid #preventivevisitWitnessSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (rec) {
                        //Для новых  записей присваиваем родительский документ
                        if (!rec.get('Id')) {
                            rec.set('PreventiveVisit', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        //{
        //    /*
        //    Аспект взаимодействия таблицы Лица присутсвующие при проверке, как инлайн грид
        //    */
        //    xtype: 'gkhinlinegridaspect',
        //    name: 'preventivevisitRealityObjectAspect',
        //    storeName: 'preventivevisit.RealityObject',
        //    modelName: 'preventivevisit.RealityObject',
        //    gridSelector: 'preventivevisitrealityobjectgrid'
        //},
        {
            /* 
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'preventivevisitRealityObjectAspect',
            gridSelector: 'preventivevisitrealityobjectgrid',
            storeName: 'preventivevisit.RealityObject',
            modelName: 'preventivevisit.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#preventivevisitRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальный район', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddRealityObjects', 'PreventiveVisitOperations'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы 'Дата и время проведения проверки' с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'preventivevisitPeriodAspect',
            gridSelector: 'preventivevisitperiodgrid',
            editFormSelector: '#preventivevisitPeriodEditWindow',
            storeName: 'preventivevisit.Period',
            modelName: 'preventivevisit.Period',
            editWindowView: 'preventivevisit.PeriodEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    //Для новой записи присваиваем родительский документ
                    if (!record.get('Id')) {
                        record.set('PreventiveVisit', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
             * Аспект взаимодействия Таблицы Приложений с формой редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'preventivevisitAnnexAspect',
            gridSelector: 'preventivevisitannexgrid',
            editFormSelector: '#preventivevisitAnnexEditWindow',
            storeName: 'preventivevisit.Annex',
            modelName: 'preventivevisit.Annex',
            editWindowView: 'preventivevisit.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('PreventiveVisit', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
             * Аспект взаимодействия Таблицы Приложений с формой редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'preventivevisitResultAspect',
            resultId: null,
            gridSelector: 'preventivevisitresultgrid',
            editFormSelector: '#preventivevisitResultEditWindow',
            storeName: 'preventivevisit.Result',
            modelName: 'preventivevisit.Result',
            editWindowView: 'preventivevisit.ResultEditWindow',
            otherActions: function (actions) {
                actions['#preventivevisitResultEditWindow #sfRealityObject'] = { 'beforeload': { fn: this.onBeforeLoadRealityObject, scope: this } };
            },
            onBeforeLoadRealityObject: function (store, operation) {
                var me = this;
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.documentId = me.controller.params.documentId;
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('PreventiveVisit', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        grid = form.down('preventivevisitresultviolationgrid'),
                        store = grid.getStore();

                    me.controller.currentResultId = 0;
                    if (record.get('Id')) {
                        asp.resultId = record.get('Id');
                        debugger;
                        me.controller.currentResultId = record.get('Id');
                    }
                    store.on('beforeload', me.onBeforeLoadViolStore, me);
                    store.load();

                },
            },
            onBeforeLoadViolStore: function (store, operation) {
                var me = this;
                operation.params.resultId = me.resultId;
            },
        },
        {
            //Аспект взаимодействия таблицы нарушений по дому с массовой формой выбора нарушений
            //При добавлении открывается форма массового выбора нарушений. После выбора список получается через подписку 
            //на событие getdata идет добавление записей в сторе
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'preventivevisitResultViolationAspect',
            gridSelector: 'preventivevisitresultviolationgrid',
            saveButtonSelector: 'preventivevisitresultviolationgrid #preventivevisitViolatioSaveButton',
            storeName: 'preventivevisit.ResultViolation',
            modelName: 'preventivevisit.ResultViolation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#preventivevisitResulViolationMultiSelectWindow',
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'НПД', xtype: 'gridcolumn', dataIndex: 'NormDocNum', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'НПД', xtype: 'gridcolumn', dataIndex: 'NormDocNum', flex: 1, sortable: false },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида нарушений без сохранения
                getdata: function (asp, records) {
                    var currentViolationStore = asp.controller.getStore(asp.storeName),
                        range = currentViolationStore.getRange(0, currentViolationStore.getCount());

                    asp.controller.mask('Выбор нарушений', asp.controller.getMainComponent());

                    //Очищаем стор потомучто там буд
                    currentViolationStore.removeAll();
                    debugger;
                    //сначала добавлем вверх новые нарушения
                    Ext.Array.each(records.items,
                        function (rec) {
                            //Tесли уже среди существующих записей нет таких записей до добавляем в стор
                            //if (voilationIds.indexOf(rec.get('Id')) == -1) {
                            currentViolationStore.add({
                                Id: 0,
                                PreventiveVisitResult: asp.controller.currentResultId,
                                CodePin: rec.get('CodePin'),
                                Pprf: rec.get('NormativeDocNames'),
                                ViolationGjiName: rec.get('Name'),
                                ViolationGji: rec.get('Id'),
                                DatePlanRemoval: null
                            });
                            //}
                        }, this);
                    debugger;
                    //теперь добавляем старые вконец
                    Ext.Array.each(range,
                        function (rec) {
                            currentViolationStore.add(rec);
                        }, this);

                    asp.controller.unmask();
                    Ext.Msg.alert('Внимание!', 'Не забудьте сохранить добавленные нарушения');
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('preventivevisit.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('preventivevisit.Witness').on('beforeload', me.onBeforeLoad, me);
        me.getStore('preventivevisit.RealityObject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('preventivevisit.Period').on('beforeload', me.onBeforeLoad, me);
        me.getStore('preventivevisit.Result').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('preventivevisitEditPanelAspect').setData(me.params.documentId);

            //Обновляем таблицу Экспертов
            me.getStore('preventivevisit.Witness').load();

            //Обновляем таблицу Экспертов
            me.getStore('preventivevisit.Annex').load();

            //Обновляем таблицу Экспертов
            me.getStore('preventivevisit.Period').load();
            //Обновляем таблицу Экспертов
            me.getStore('preventivevisit.Result').load();


            //Обновляем таблицу Предоставляемых документов
            me.getStore('preventivevisit.RealityObject').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params && me.params.documentId)
            operation.params.documentId = me.params.documentId;
    },
});