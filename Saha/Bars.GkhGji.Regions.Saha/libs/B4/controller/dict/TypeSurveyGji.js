Ext.define('B4.controller.dict.TypeSurveyGji', {
    extend: 'B4.base.Controller',
    typeSurveyId: null,
    requires: [
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.dict.TypeSurvey',
        'B4.enums.TypeJurPerson'
    ],

    models: [
        'dict.TypeSurveyGji',
        'dict.TypeSurveyGoalInspGji',
        'dict.TypeSurveyKindInspGji',
        'dict.TypeSurveyInspFoundationGji',
        'dict.TypeSurveyTaskInspGji',
        'dict.TypeSurveyProvidedDocGji',
        'dict.TypeSurveyLegalReason',
        'dict.TypeSurveyContragentType'
    ],
    stores: [
        'dict.TypeSurveyGji',
        'dict.TypeSurveyGoalInspGji',
        'dict.TypeSurveyKindInspGji',
        'dict.TypeSurveyInspFoundationGji',
        'dict.TypeSurveyTaskInspGji',
        'dict.KindCheckGjiForSelect',
        'dict.KindCheckGjiSelected',
        'dict.TypeSurveyProvidedDocGji',
        'dict.TypeSurveyLegalReason',
        'dict.TypeSurveyContragentType',
        'dict.LegalReasonForSelect',
        'dict.LegalReasonForSelected'
    ],
    views: [
        'dict.typesurveygji.Grid',
        'dict.typesurveygji.EditWindow',
        'dict.typesurveygji.GoalInspGjiGrid',
        'dict.typesurveygji.KindInspGjiGrid',
        'dict.typesurveygji.InspFoundationGjiGrid',
        'dict.typesurveygji.TaskInspGjiGrid',
        'dict.typesurveygji.KindInspGjiEditWindow',
        'SelectWindow.MultiSelectWindow',
        'dict.typesurveygji.TypeSurveyProvidedDocGjiGrid',
        'dict.typesurveygji.ContragentTypeGrid'
    ],

    mainView: 'dict.typesurveygji.Grid',
    mainViewSelector: 'typeSurveyGjiGrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    typeSurveyGjiEditWindowSelector: '#typeSurveyGjiEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'typeSurveyGjiGrid'
        }
    ],

    aspects: [
        { xtype: 'typesurveyperm' },
        {
            /*Аспект взаимодействия таблицы справочника Статьи законов и формы редактирования*/
            xtype: 'grideditwindowaspect',
            name: 'typeSurveyGjiGridWindowAspect',
            gridSelector: 'typeSurveyGjiGrid',
            editFormSelector: '#typeSurveyGjiEditWindow',
            storeName: 'dict.TypeSurveyGji',
            modelName: 'dict.TypeSurveyGji',
            editWindowView: 'dict.typesurveygji.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setTypeSurveyId(record.getId());
            },
            listeners: {
                aftersetformdata: function(asp, record) {
                    asp.controller.setTypeSurveyId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'goalInspectionGjiGridAspect',
            saveButtonSelector: '#typeSurveyGjiEditWindow b4savebutton',
            gridSelector: '#goalInspGjiGrid',
            storeName: 'dict.TypeSurveyGoalInspGji',
            modelName: 'dict.TypeSurveyGoalInspGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#goalInspectionGjiMultiSelectWindow',
            storeSelect: 'dict.SurveyPurposeForSelect',
            storeSelected: 'dict.SurveyPurposeForSelected',
            titleSelectWindow: 'Выбор целей проверки',
            titleGridSelect: 'Цели проверки для отбора',
            titleGridSelected: 'Выбранные цели проверки',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (me, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (item) {
                        recordIds.push(item.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddGoals', 'TypeSurveyGji', {
                            objectIds: recordIds,
                            typeSurveyId: me.controller.typeSurveyId
                        })).next(function () {
                            me.controller.unmask();
                            me.controller.getStore(me.storeName).load();
                            return true;
                        }).error(function () {
                            me.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать цели проверки');
                        return false;
                    }
                    return true;
                }
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
            name: 'kindinspectionAspect',
            gridSelector: '#kindInspGjiGrid',
            editFormSelector: 'kindInspGjiEditWindow',
            editWindowView: 'dict.typesurveygji.KindInspGjiEditWindow',
            storeName: 'dict.TypeSurveyKindInspGji',
            modelName: 'dict.TypeSurveyKindInspGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#typesurveyKindInspMultiSelectWindow',
            storeSelect: 'dict.KindCheckGjiForSelect',
            storeSelected: 'dict.KindCheckGjiSelected',
            titleSelectWindow: 'Выбор видов проверки',
            titleGridSelect: 'Виды проверки для отбора',
            titleGridSelected: 'Выбранные виды проверки',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function(me, records) {
                    var recordIds = [];

                    Ext.each(records.items, function(item) {
                        recordIds.push(item.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddKindInsp', 'TypeSurveyKindInspGji', {
                            objectIds: recordIds,
                            typeSurveyId: me.controller.typeSurveyId
                        })).next(function() {
                            me.controller.unmask();
                            me.controller.getStore(me.storeName).load();
                            return true;
                        }).error(function() {
                            me.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды проверки');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'contragentTypeGridAspect',
            gridSelector: '#contragentTypeGrid',
            storeName: 'dict.TypeSurveyContragentType',
            modelName: 'dict.TypeSurveyContragentType',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#typeSurveyContrTypeMultiSelectWindow',
            storeSelect: null,
            storeSelected: null,
            titleSelectWindow: 'Выбор типов контрагента',
            titleGridSelect: 'Типы контрагента',
            titleGridSelected: 'Выбранные типы контрагента',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }
            ],

            otherActions: function() {
                this.storeSelect = B4.enums.TypeJurPerson.getStore();
                this.storeSelected = B4.enums.TypeJurPerson.getStore();
            },

            listeners: {
                getdata: function (asp, records) {
                    var contragentTypes = [];

                    records.each(function (rec) {
                        contragentTypes.push(rec.get('Value'));
                    });

                    if (contragentTypes[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(
                                B4.Url.action('AddContragentTypes', 'TypeSurveyContragentType',
                                {
                                    contragentTypes: contragentTypes,
                                    typeSurveyId: asp.controller.typeSurveyId
                                }))
                            .next(function () {
                                asp.controller.unmask();
                                asp.controller.getStore(asp.storeName).load();
                                return true;
                            }).error(function () {
                                asp.controller.unmask();
                            });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать предоставляемые документы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'inspFoundationGjiGridAspect',
            gridSelector: '#inspFoundationGjiGrid',
            storeName: 'dict.TypeSurveyLegalReason',
            modelName: 'dict.TypeSurveyLegalReason',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#typeSurveyLegalReasonMultiSelectWindow',
            storeSelect: 'dict.LegalReasonForSelect',
            storeSelected: 'dict.LegalReasonForSelected',
            titleSelectWindow: 'Выбор правовых оснований',
            titleGridSelect: 'Правовые основание',
            titleGridSelected: 'Выбранные правовые основания',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', width: 100, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', width: 100, sortable: false }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(
                                B4.Url.action('AddLegalReasons', 'TypeSurveyLegalReason',
                                {
                                    objectIds: recordIds,
                                    typeSurveyId: asp.controller.typeSurveyId
                                }))
                            .next(function() {
                                asp.controller.unmask();
                                asp.controller.getStore(asp.storeName).load();
                                return true;
                            }).error(function() {
                                asp.controller.unmask();
                            });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать предоставляемые документы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'typeSurveyDocAspect',
            gridSelector: '#typeSurveyProvidedDocGjiGrid',
            storeName: 'dict.TypeSurveyProvidedDocGji',
            modelName: 'dict.TypeSurveyProvidedDocGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#typeSurveyDocMultiSelectWindow',
            storeSelect: 'dict.ProvidedDocGjiForSelect',
            storeSelected: 'dict.ProvidedDocGjiForSelected',
            titleSelectWindow: 'Выбор предоставляемых документов',
            titleGridSelect: 'Документы для отбора',
            titleGridSelected: 'Выбранные документы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', width: 100, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', width: 100, sortable: false }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(
                                B4.Url.action('AddProvidedDocuments', 'TypeSurveyProvidedDocumentGji',
                                {
                                    objectIds: recordIds,
                                    typeSurveyId: asp.controller.typeSurveyId
                                }))
                            .next(function() {
                                asp.controller.unmask();
                                asp.controller.getStore(asp.storeName).load();
                                return true;
                            }).error(function() {
                                asp.controller.unmask();
                            });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать предоставляемые документы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'taskInspectionGjiGridAspect',
            saveButtonSelector: '#typeSurveyGjiEditWindow b4savebutton',
            gridSelector: '#taskInspGjiGrid',
            storeName: 'dict.TypeSurveyTaskInspGji',
            modelName: 'dict.TypeSurveyTaskInspGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskInspGjiMultiSelectWindow',
            storeSelect: 'dict.SurveyObjectiveForSelect',
            storeSelected: 'dict.SurveyObjectiveForSelected',
            titleSelectWindow: 'Выбор задач проверки',
            titleGridSelect: 'Задачи проверки для отбора',
            titleGridSelected: 'Выбранные задачи проверки',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (me, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (item) {
                        recordIds.push(item.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddTaskInsp', 'TypeSurveyGji', {
                            objectIds: recordIds,
                            typeSurveyId: me.controller.typeSurveyId
                        })).next(function () {
                            me.controller.unmask();
                            me.controller.getStore(me.storeName).load();
                            return true;
                        }).error(function () {
                            me.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать задачи проверки');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function() {
        this.getStore('dict.TypeSurveyGoalInspGji').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.TypeSurveyKindInspGji').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.TypeSurveyInspFoundationGji').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.TypeSurveyLegalReason').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.TypeSurveyTaskInspGji').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.TypeSurveyProvidedDocGji').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.TypeSurveyContragentType').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView() || Ext.widget('typeSurveyGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TypeSurveyGji').load();
    },

    setTypeSurveyId: function(id) {
        this.typeSurveyId = id;

        var editWindow = Ext.ComponentQuery.query(this.typeSurveyGjiEditWindowSelector)[0];

        var storeGoals = this.getStore('dict.TypeSurveyGoalInspGji');
        var storeKinds = this.getStore('dict.TypeSurveyKindInspGji');
        var storeLegalReasons = this.getStore('dict.TypeSurveyLegalReason');
        var storeTasks = this.getStore('dict.TypeSurveyTaskInspGji');
        var storeDocuments = this.getStore('dict.TypeSurveyProvidedDocGji');
        var storeContrTypes = this.getStore('dict.TypeSurveyContragentType');

        //очищаем сторы
        storeGoals.removeAll();
        storeKinds.removeAll();
        storeLegalReasons.removeAll();
        storeTasks.removeAll();
        storeDocuments.removeAll();
        storeContrTypes.removeAll();
        if (id > 0) {
            editWindow.down('.tabpanel').setDisabled(false);
            storeGoals.load();
            storeKinds.load();
            storeLegalReasons.load();
            storeTasks.load();
            storeDocuments.load();
            storeContrTypes.load();
        } else {
            editWindow.down('.tabpanel').setDisabled(true);
        }
    },

    onBeforeLoad: function(store, operation) {
        operation.params.typeSurveyId = this.typeSurveyId;
    }
});