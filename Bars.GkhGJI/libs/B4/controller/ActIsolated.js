Ext.define('B4.controller.ActIsolated', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ActIsolated',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeBase'
    ],

    models: [
        'ActIsolated',
        'actisolated.Annex',
        'actisolated.Period',
        'actisolated.Witness',
        'actisolated.Definition',
        'actisolated.RealityObject',
        'actisolated.InspectedPart',
        'actisolated.ProvidedDoc',
        'ProtocolGji'
    ],

    stores: [
        'ActIsolated',
        'actisolated.Annex',
        'actisolated.Period',
        'actisolated.Witness',
        'actisolated.Violation',
        'actisolated.Event',
        'actisolated.Measure',
        'actisolated.Definition',
        'actisolated.InspectedPart',
        'actisolated.RealityObject',
        'actisolated.ProvidedDoc',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ViolationGjiForSelect',
        'dict.ViolationGjiForSelected',
        'dict.InspectedPartGjiForSelect',
        'dict.InspectedPartGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected'
    ],

    views: [
        'actisolated.EditPanel',
        'actisolated.RealityObjectGrid',
        'actisolated.RealityObjectEditWindow',
        'actisolated.EventGrid',
        'actisolated.EventEditWindow',
        'actisolated.MeasureGrid',
        'actisolated.ViolationGrid',
        'actisolated.AnnexGrid',
        'actisolated.AnnexEditWindow',
        'actisolated.WitnessGrid',
        'actisolated.PeriodGrid',
        'actisolated.PeriodEditWindow',
        'actisolated.DefinitionGrid',
        'actisolated.DefinitionEditWindow',
        'actisolated.InspectedPartGrid',
        'actisolated.InspectedPartEditWindow',
        'actisolated.ProvidedDocGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'actisolated.EditPanel',
    mainViewSelector: 'actisolatededitpanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        actCheck: 'B4.mixins.ActCheck'
    },

    aspects: [
        {
            /*
            Аспект формирования документов для Акта без взаимодействия
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'actIsolatedCreateButtonAspect',
            buttonSelector: 'actisolatededitpanel gjidocumentcreatebutton',
            containerSelector: 'actisolatededitpanel',
            typeDocument: 21
        },
        {
            xtype: 'actisolatedperm',
            editFormAspectName: 'actIsolatedEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'actIsolatedStateButtonAspect',
            stateButtonSelector: 'actisolatededitpanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    var editPanelAspect = asp.controller.getAspect('actIsolatedEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            //Аспект кнопки печати акта проверки
            xtype: 'gkhbuttonprintaspect',
            name: 'actIsolatedPrintAspect',
            buttonSelector: 'actisolatededitpanel #btnPrint',
            codeForm: 'ActIsolated',
            getUserParams: function (reportId) {
                var me = this,
                    param = { DocumentId: me.controller.params.documentId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /* 
            Аспект для Акта без взаимодействия
            */
            xtype: 'gjidocumentaspect',
            name: 'actIsolatedEditPanelAspect',
            editPanelSelector: 'actisolatededitpanel',
            modelName: 'ActIsolated',
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    callbackUnMask,
                    addButton,
                    title,
                    typeBase = rec.get('TypeBase');
                
                asp.controller.params= asp.controller.params || {};

                asp.controller.params.typeBase = typeBase;
                me.controller.params.isExistsWarningDoc = rec.get('IsExistsWarningDoc');
                
                callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                //После проставления данных обновляем title у вкладки
                title = 'Акт без взаимодействия';

                if (rec.get('DocumentNumber'))
                    panel.setTitle(title + ' ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle(title);

                panel.down('#actIsolatedTabPanel').setActiveTab(0);

                me.controller.currentRoId = 0;
                me.controller.params.documentId = rec.getId();
                
                //Обновляем таблицу Проверяемых домов акта
                me.controller.getStore('actisolated.RealityObject').load();

                B4.Ajax.request(B4.Url.action('GetInfo', 'ActIsolated', {
                        documentId: asp.controller.params.documentId
                    }))
                    .next(function(response) {
                        var obj = Ext.JSON.decode(response.responseText),
                            fieldInspectors = panel.down('#trigfInspectors');

                        fieldInspectors.updateDisplayedText(obj.inspectorNames);
                        fieldInspectors.setValue(obj.inspectorIds);

                        me.disableButtons(false);
                        asp.controller.unmask();
                    })
                    .error(function() {
                        asp.controller.unmask();
                    });

                //загружаем стор отчетов
                me.controller.getAspect('actIsolatedPrintAspect').loadReportStore();

                //Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('actIsolatedStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                // обновляем кнопку Сформировать
                me.controller.getAspect('actIsolatedCreateButtonAspect').setData(rec.get('Id'));
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
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы Проверяемых домов акта и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actIsolatedRealityObjectAspect',
            gridSelector: 'actisolatedrealityobjectgrid',
            editFormSelector: 'actisolatedrealityobjecteditwindow',
            storeName: 'actisolated.RealityObject',
            modelName: 'actisolated.RealityObject',
            editWindowView: 'actisolated.RealityObjectEditWindow',

            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var editWindow = asp.getForm(),
                        storeViolation = editWindow.down('actisolatedviolationgrid').getStore(),
                        eventViolation = editWindow.down('actisolatedeventgrid').getStore(),
                        measureViolation = editWindow.down('actisolatedmeasuregrid').getStore();

                    asp.controller.setCurrentRoId(record.getId());

                    storeViolation.load();
                    eventViolation.load();
                    measureViolation.load();
                },
                beforesave: function (asp, rec) {
                    var editWindow = asp.getForm(),
                        storeViolation = editWindow.down('actisolatedviolationgrid').getStore(),
                        haveViolation = editWindow.down('#cbHaveViolation').getValue(),
                        message;

                    message = asp.controller.checkViolationCondition(storeViolation, haveViolation);

                    if (message) {
                        Ext.Msg.alert('Ошибка сохранения!', message);
                        return false;
                    }

                    return true;
                }
            },

            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbHaveViolation'] = { 'change': { fn: this.changeHaveViolation, scope: this } };
            },

            changeHaveViolation: function (combobox, newValue) {
                var me = this,
                    actEventGridAddButton = me.getForm().down('#actEventGridAddButton'),
                    actViolationGridAddButton = me.getForm().down('#actViolationGridAddButton'),
                    eventStore = me.getForm().down('actisolatedeventgrid').getStore();


                actEventGridAddButton.setDisabled(newValue !== B4.enums.YesNoNotSet.Yes);
                actViolationGridAddButton.setDisabled(true);

                if (newValue === B4.enums.YesNoNotSet.Yes && eventStore.count() > 0) {
                    actViolationGridAddButton.setDisabled(false);
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actIsolatedInspectorMultiSelectWindowAspect',
            fieldSelector: 'actisolatededitpanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actIsolatedInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield'} }
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

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    
                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);

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
            name: 'actIsolatedWitnessAspect',
            storeName: 'actisolated.Witness',
            modelName: 'actisolated.Witness',
            gridSelector: 'actisolatedwitnessgrid',
            saveButtonSelector: 'actisolatededitpanel #actIsolatedWitnessSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (rec) {
                        //Для новых  записей присваиваем родительский документ
                        if (!rec.get('Id')) {
                            rec.set('ActIsolated', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы 'Дата и время проведения проверки' с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actIsolatedPeriodAspect',
            gridSelector: 'actisolatedperiodgrid',
            editFormSelector: 'actisolatedperiodeditwindow',
            storeName: 'actisolated.Period',
            modelName: 'actisolated.Period',
            editWindowView: 'actisolated.PeriodEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    //Для новой записи присваиваем родительский документ
                    if (!record.get('Id')) {
                        record.set('ActIsolated', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actIsolatedAnnexAspect',
            gridSelector: 'actisolatedannexgrid',
            editFormSelector: 'actisolatedannexeditwindow',
            storeName: 'actisolated.Annex',
            modelName: 'actisolated.Annex',
            editWindowView: 'actisolated.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    //Для новой записи присваиваем родительский документ
                    if (!record.get('Id')) {
                        record.set('ActIsolated', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы определений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actIsolatedDefinitionAspect',
            gridSelector: 'actisolateddefinitiongrid',
            editFormSelector: 'actisolateddefinitioneditwindow',
            storeName: 'actisolated.Definition',
            modelName: 'actisolated.Definition',
            editWindowView: 'actisolated.DefinitionEditWindow',
            onSaveSuccess: function (asp, record) {
                asp.setDefinitionId(record.getId());
            },
            listeners: {
                getdata: function (asp, record) {
                    //Для новой записи присваиваем родительский документ
                    if (!record.get('Id')) {
                        record.set('ActIsolated', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    asp.setDefinitionId(record.getId());
                }
            },

            setDefinitionId: function (id) {
                this.controller.params.definitionId = id;
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы инспектируемых частей с массовой формой выбора инсп. частей
            По нажатию на Добавить открывается форма выбора инсп. частей.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение инсп. частей
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'actIsolatedInspectedPartAspect',
            gridSelector: 'actisolatedinspectedpartgrid',
            storeName: 'actisolated.InspectedPart',
            modelName: 'actisolated.InspectedPart',
            editFormSelector: 'actisolatedinspectedparteditwindow',
            editWindowView: 'actisolated.InspectedPartEditWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actIsolatedInspectedPartMultiSelectWindow',
            storeSelect: 'dict.InspectedPartGjiForSelect',
            storeSelected: 'dict.InspectedPartGjiForSelected',
            titleSelectWindow: 'Выбор инспектируемых частей',
            titleGridSelect: 'Элементы для отбора',
            titleGridSelected: 'Выбранные элементы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);
                    
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddInspectedParts', 'ActIsolatedInspectedPart', {
                            partIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать инспектируемые части');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            //Аспект взаимодействия инлайн таблицы Предоставляемые документы и массовой формой выбора документов
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actIsolatedProvidedDocAspect',
            gridSelector: 'actisolatedprovideddocgrid',
            saveButtonSelector: 'actisolatedprovideddocgrid #actProvidedDocGridSaveButton',
            storeName: 'actisolated.ProvidedDoc',
            modelName: 'actisolated.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actIsolatedProvidedDocMultiSelectWindow',
            storeSelect: 'dict.ProvidedDocGjiForSelect',
            storeSelected: 'dict.ProvidedDocGjiForSelected',
            titleSelectWindow: 'Выбор дкоументов',
            titleGridSelect: 'Документы для отбора',
            titleGridSelected: 'Выбранные документы',
            isPaginable: false,
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            otherActions: function (actions) {
                var me = this;
                actions['actisolatedprovideddocgrid #actProvidedDocGridUpdateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };
            },
            onBeforeLoad: function (store, operation) {
                operation.start = undefined;
                operation.page = undefined;
                operation.limit = undefined;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'ActIsolatedProvidedDoc', {
                            providedDocIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            Ext.Msg.alert('Сохранено!', 'Документы сохранены успешно');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать документы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            //Аспект взаимодействия таблицы нарушений по дому с массовой формой выбора нарушений
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actIsolatedRealObjViolationAspect',
            gridSelector: 'actisolatedviolationgrid',
            saveButtonSelector: 'actisolatedviolationgrid #realObjViolationSaveButton',

            storeName: 'actisolated.Violation',
            modelName: 'actisolated.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actIsolatedRoViolationMultiSelectWindow',
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'CodePin', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'CodePin', flex: 1, sortable: false },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            otherActions: function (actions) {
                actions[this.gridSelector] = {
                    'store.beforeload': { fn: this.controller.onChildObjectBeforeLoad, scope: this.controller },
                    'store.load': { fn: this.onViolationStoreLoad, scope: this }
                };
            },
            listeners: {
                getdata: function (asp, records) {
                    var currentViolationStore = asp.getGrid().getStore(),
                        range = currentViolationStore.getRange(0, currentViolationStore.getCount());

                    asp.controller.mask('Выбор нарушений', asp.controller.getMainComponent());

                    currentViolationStore.removeAll();

                    //сначала добавлем вверх новые нарушения
                    Ext.Array.each(records.items,
                        function (rec) {
                            //если уже среди существующих записей нет таких записей до добавляем в стор
                            currentViolationStore.add({
                                Id: 0,
                                ActIsolatedRealObj: asp.controller.currentRoId,
                                ViolationGjiPin: rec.get('CodePin'),
                                ViolationGjiName: rec.get('Name'),
                                ViolationGjiId: rec.get('Id'),
                                DatePlanRemoval: null
                            });
                        }, this);

                    Ext.Array.each(range,
                        function (rec) {
                            currentViolationStore.add(rec);
                        }, this);

                    asp.controller.unmask();

                    return true;
                },
                beforesave: function (asp, store) {
                    var me = this,
                        isCorrectDate = true,
                        panel = me.controller.getMainComponent(),
                        actDate = panel.down('datefield[name=DocumentDate]').value,
                        timeLine = me.controller.getTimeline(),
                        actDatePlusTimeLine = new Date(new Date(actDate).setMonth(actDate.getMonth() + timeLine)),
                        message = me.controller.checkViolationCondition(store, haveViolation);

                    if (message) {
                        Ext.Msg.alert('Ошибка сохранения!', message);
                        return false;
                    }

                    store.each(function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('ActIsolatedRealObj', asp.controller.currentRoId);
                        }

                        if (rec.get('DatePlanRemoval')) {
                            var datePlanRemoval;
                            if (rec.get('DatePlanRemoval') instanceof Date) {
                                datePlanRemoval = rec.get('DatePlanRemoval');
                            } else {
                                var dateParts = rec.get('DatePlanRemoval').split("-");
                                datePlanRemoval = new Date(dateParts[0], dateParts[1] - 1, dateParts[2].substring(0, dateParts[2].lenght - 9));
                            }

                            if (datePlanRemoval.getTime() > actDatePlusTimeLine.getTime()) {
                                isCorrectDate = false;
                            }
                        }

                        return isCorrectDate;
                    });

                    if (!isCorrectDate) {
                        Ext.Msg.alert('Ошибка сохранения!', Ext.String.format('Срок устранения нарушения не должен превышать {0} месяцев', timeLine));
                        return false;
                    }

                    return true;
                }
            },
            onViolationStoreLoad: function (store, records) {
                var me = this,
                    realObjEditWindow = me.getGrid().up(),
                    actMeasureGridAddButton = realObjEditWindow.down('#actMeasureGridAddButton');

                actMeasureGridAddButton.setDisabled(store.count() === 0);
            },
        },
        {
            /*
            * Аспект взаимодействия Таблицы мероприятий по дому с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actIsolatedEventAspect',
            gridSelector: 'actisolatedeventgrid',
            editFormSelector: 'actisolatedeventeditwindow',
            modelName: 'actisolated.Event',
            editWindowView: 'actisolated.EventEditWindow',
            otherActions: function (actions) {
                Ext.apply(actions[this.gridSelector], {
                    'store.beforeload': { fn: this.controller.onChildObjectBeforeLoad, scope: this.controller },
                    'store.load': { fn: this.onEventStoreLoad, scope: this }
                });
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ActIsolatedRealObj', asp.controller.currentRoId);
                    }
                }
            },
            onEventStoreLoad: function (store, records) {
                var me = this,
                    realObjEditWindow = me.getGrid().up(),
                    haveViolationField = realObjEditWindow.down('#cbHaveViolation'),
                    actViolationGridAddButton = realObjEditWindow.down('#actViolationGridAddButton');

                actViolationGridAddButton.setDisabled(haveViolationField.getValue() !== B4.enums.YesNoNotSet.Yes || store.count() === 0);
            }
        },
        {
            /*
             Аспект взаимодействия Таблицы мер по дому как инлайн грид
            */
            xtype: 'gkhinlinegridaspect',
            name: 'actIsolatedMeasureAspect',
            modelName: 'actisolated.Measure',
            gridSelector: 'actisolatedrealityobjecteditwindow actisolatedmeasuregrid',
            saveButtonSelector: 'actisolatedmeasuregrid #realObjMeasureSaveButton',
            otherActions: function (actions) {
                actions[this.gridSelector] = {
                    'store.beforeload': { fn: this.controller.onChildObjectBeforeLoad, scope: this.controller }
                };
            },
            listeners: {
                beforesave: function (asp, store) {
                    var res = true;

                    store.each(function (rec) {
                        if (!rec.get('Measure')) {
                            Ext.Msg.alert('Ошибка сохранения!', 'Необходимо заполнить поле "Меры, принятые по пресечению нарушения обязательных требований"');
                            res = false;
                        }
                        if (!rec.get('Id')) {
                            rec.set('ActIsolatedRealObj', asp.controller.currentRoId);
                        }

                        return res;
                    });

                    return res;
                }
            }
        },
    ],

    init: function () {
        var me = this;

        me.getStore('actisolated.RealityObject').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actisolated.Witness').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actisolated.Definition').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actisolated.Annex').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actisolated.Period').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actisolated.InspectedPart').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actisolated.ProvidedDoc').on('beforeload', me.onObjectBeforeLoad, me);

        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('actIsolatedEditPanelAspect').setData(me.params.documentId);

            //Обновляем таблицу Лиц присутсвующих при проверке
            me.getStore('actisolated.Witness').load();

            //Обновляем таблицу Дата и время проведения проверки
            me.getStore('actisolated.Period').load();

            //Обновляем таблицу Приложений
            me.getStore('actisolated.Annex').load();

            //Обновляем таблицу определений
            me.getStore('actisolated.Definition').load();

            //Обновляем таблицу инспектируемых частей
            me.getStore('actisolated.InspectedPart').load();

            //Обновляем таблицу предоставленных документов
            me.getStore('actisolated.ProvidedDoc').load();
        }
    },

    onObjectBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params && me.params.documentId > 0)
            operation.params.documentId = me.params.documentId;
    },

    onChildObjectBeforeLoad: function (store, operation) {
        var me = this;

        if (me.currentRoId > 0) {
            operation.params.objectId = me.currentRoId;
        }
    },

    setCurrentRoId: function (id) {
        this.currentRoId = id;
    },

    checkViolationCondition: function (violationStore, haveViolation) {
        if (violationStore.getCount() == 0 && haveViolation == B4.enums.YesNoNotSet.Yes) {
            return 'Если нарушения выявлены, то необходимо в таблице нарушений добавить записи нарушений';
        }

        if (violationStore.getCount() != 0 && haveViolation != B4.enums.YesNoNotSet.Yes) {
            return 'Записи в таблице нарушений должны быть только если нарушения выявлены';
        }
    }
});