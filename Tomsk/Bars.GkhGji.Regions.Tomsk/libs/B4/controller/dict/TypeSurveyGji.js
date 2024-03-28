Ext.define('B4.controller.dict.TypeSurveyGji', {
    extend: 'B4.base.Controller',
    typeSurveyId: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.dict.TypeSurvey'
    ],

    models: [
        'dict.TypeSurveyGji',
        'dict.TypeSurveyGoalInspGji',
        'dict.TypeSurveyKindInspGji',
        'dict.TypeSurveyInspFoundationGji',
        'dict.TypeSurveyTaskInspGji',
        'dict.TypeSurveyGjiIssue',
        'dict.TypeSurveyProvidedDocGji'
    ],
    stores: [
        'dict.TypeSurveyGji',
        'dict.TypeSurveyGoalInspGji',
        'dict.TypeSurveyKindInspGji',
        'dict.TypeSurveyInspFoundationGji',
        'dict.TypeSurveyTaskInspGji',
        'dict.KindCheckGjiForSelect',
        'dict.KindCheckGjiSelected',
        'dict.TypeSurveyGjiIssue',
        'dict.TypeSurveyProvidedDocGji'
    ],
    views: [
        'dict.typesurveygji.Grid',
        'dict.typesurveygji.EditWindow',
        'dict.typesurveygji.GoalInspGjiGrid',
        'dict.typesurveygji.KindInspGjiGrid',
        'dict.typesurveygji.InspFoundationGjiGrid',
        'dict.typesurveygji.TaskInspGjiGrid',
        'dict.typesurveygji.IssueGrid',
        'SelectWindow.MultiSelectWindow',
        'dict.typesurveygji.TypeSurveyProvidedDocGjiGrid'
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
                aftersetformdata: function(asp, record, form) {
                    asp.controller.setTypeSurveyId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'goalInspectionGjiGridAspect',
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
                getdata: function (me, records) {
                    var recordIds = [];

                    Ext.each(records.items, function(item) {
                        recordIds.push(item.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddKindInsp', 'TypeSurveyGji', {
                            objectIds: recordIds,
                            typeSurveyId: me.controller.typeSurveyId
                        })).next(function (response) {
                            me.controller.unmask();
                            me.controller.getStore(me.storeName).load();
                            return true;
                        }).error(function () {
                            me.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды проверки');
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
            storeName: 'dict.TypeSurveyInspFoundationGji',
            modelName: 'dict.TypeSurveyInspFoundationGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#inspFoundationGjiMultiSelectWindow',
            storeSelect: 'dict.NormativeDocForSelect',
            storeSelected: 'dict.NormativeDocForSelected',
            titleSelectWindow: 'Выбор нормативных документов',
            titleGridSelect: 'Нормативные документы для отбора',
            titleGridSelected: 'Выбранные нормативные документы',
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
                        B4.Ajax.request(B4.Url.action('AddInspFoundation', 'TypeSurveyGji', {
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
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нормативные документы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskInspectionGjiGridAspect',
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
        },
        {
            /*Аспект взаимодействия таблицы справочника По вопросу*/
            xtype: 'gkhinlinegridaspect',
            name: 'typesurveygjiIssueGridAspect',
            storeName: 'dict.TypeSurveyGjiIssue',
            modelName: 'dict.TypeSurveyGjiIssue',
            gridSelector: 'typesurveygjiIssueGrid',
            saveButtonSelector: '#typeSurveyGjiEditWindow #typeSurveyGjiIssueSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (record) {
                        if (!record.get('Id')) {
                            record.set('TypeSurvey', asp.controller.typeSurveyId);
                        }
                    });
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
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(
                            B4.Url.action('AddProvidedDocuments', 'TypeSurveyGji',
                            {
                                objectIds: recordIds,
                                typeSurveyId: asp.controller.typeSurveyId
                            }))
                            .next(function () {
                                asp.controller.unmask();
                                asp.controller.getStore(asp.storeName).load();
                                Ext.Msg.alert('Сохранение!', 'Предоставляемые документы сохранены успешно');
                                return true;
                            }).error(function () {
                                asp.controller.unmask();
                            });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать предоставляемые документы');
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
        this.getStore('dict.TypeSurveyTaskInspGji').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.TypeSurveyGjiIssue').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.TypeSurveyProvidedDocGji').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
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
        var storeFoundation = this.getStore('dict.TypeSurveyInspFoundationGji');
        var storeTasks = this.getStore('dict.TypeSurveyTaskInspGji');
        var storeIssue = this.getStore('dict.TypeSurveyGjiIssue');
        var storeDocuments = this.getStore('dict.TypeSurveyProvidedDocGji');

        //очищаем сторы
        storeGoals.removeAll();
        storeKinds.removeAll();
        storeFoundation.removeAll();
        storeTasks.removeAll();
        storeIssue.removeAll();
        storeDocuments.removeAll();

        if (id > 0) {
            editWindow.down('.tabpanel').setDisabled(false);
            storeGoals.load();
            storeKinds.load();
            storeFoundation.load();
            storeTasks.load();
            storeIssue.load();
            storeDocuments.load();
        } else {
            editWindow.down('.tabpanel').setDisabled(true);
        }
    },

    onBeforeLoad: function(store, operation) {
        operation.params.typeSurveyId = this.typeSurveyId;
    }
});