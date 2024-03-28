Ext.define('B4.controller.actionisolated.taskaction.TaskAction', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.actionisolated.TaskAction',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.StateButton',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.FieldRequirementAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.enums.TypeBaseAction',
        'B4.enums.TypeObjectAction'
    ],

    models: [
        'actionisolated.TaskAction',
        'actionisolated.taskaction.Annex',
        'actionisolated.taskaction.ArticleLaw',
        'actionisolated.taskaction.House',
        'actionisolated.taskaction.Item',
        'RealityObject',
        'actionisolated.taskaction.RealityObjectTask',
        'actionisolated.taskaction.SurveyPurpose'
        ],

    stores: [
        'actionisolated.TaskAction',
        'actionisolated.taskaction.Annex',
        'actionisolated.taskaction.ArticleLaw',
        'actionisolated.taskaction.House',
        'actionisolated.taskaction.Item',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.KnmAction',
        'RealityObject',
        'actionisolated.taskaction.RealityObjectTaskForSelect',
        'actionisolated.taskaction.RealityObjectTaskForSelected',
        'actionisolated.taskaction.SurveyPurpose',
        'actionisolated.taskaction.SurveyPurposeForSelect',
        'actionisolated.taskaction.SurveyPurposeForSelected'
    ],

    views: [
        'actionisolated.taskaction.AnnexEditWindow',
        'actionisolated.taskaction.EditPanel'
    ],

    mainView: 'actionisolated.taskaction.EditPanel',
    mainViewSelector: '#taskactionEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'actionisolatedtaskperm',
            editFormAspectName: 'taskactionEditPanelAspect'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                {
                    name: 'GkhGji.DocumentReestrGji.TaskActionIsolated.Field.PlannedAction', applyTo: '#tfPlannedActions', selector: '#taskactionEditPanel'
                }
            ]
        },
        {
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'taskactionCreateButtonAspect',
            buttonSelector: '#taskactionEditPanel gjidocumentcreatebutton',
            containerSelector: '#taskactionEditPanel',
            typeDocument: 180, //B4.enums.TypeDocumentGji.TaskActionIsolated 
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId === 'TaskActionToActActionRule') {
                    return false;
                }
                return true;
            }
        },
        {
            xtype: 'statebuttonaspect',
            name: 'taskactionStateButtonAspect',
            stateButtonSelector: '#taskactionEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    asp.controller.getAspect('taskactionEditPanelAspect').setData(entityId);
                    asp.controller.getMainView().up('#actionIsolatedNavigationPanel').getComponent('actionisolatedMenuTree').getStore().load();
                }
            }
        },
        {
            xtype: 'gjidocumentaspect',
            name: 'taskactionEditPanelAspect',
            editPanelSelector: '#taskactionEditPanel',
            modelName: 'actionisolated.TaskAction',
            listeners: {
                beforesave: function (asp, rec) {
                    var panel = asp.getPanel(),
                        tfPlannedActions = panel.down('#tfPlannedActions');

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddTaskActionIsolatedPlannedActions', 'KnmAction'),
                        method: 'POST',
                        params: {
                            actionId: tfPlannedActions.getValue(),
                            documentId: asp.controller.params.documentId
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                        return false;
                    });
                }
            },
            otherActions: function(actions) {
            },
            onSaveSuccess: function (asp, rec) {
                //исключение изменения заголовка панели
            },
            onAfterSetPanelData: function(asp, rec, panel) {
                asp.controller.params = asp.controller.params || {};

                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                panel.down('#taskactionTabPanel').setActiveTab(0);

                this.disableButtons(false);
                this.hideComponents(panel, rec);
                this.updateFieldValue(panel, rec);

                this.controller.getAspect('taskactionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                this.controller.getAspect('taskactionCreateButtonAspect').setData(rec.get('Id'));
                this.controller.getAspect('taskPrintAspect').loadReportStore();
            },
            disableButtons: function(value) {
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
            updateFieldValue: function (panel, rec) {
                var fieldInspectors = panel.down('#trigfInspectors'),
                    tfPlannedActions = panel.down('#tfPlannedActions');

                fieldInspectors.updateDisplayedText(rec.data.Inspectors);
                fieldInspectors.setValue(rec.data.InspectorIds);

                tfPlannedActions.updateDisplayedText(rec.data.PlannedActions);
                tfPlannedActions.setValue(rec.data.PlannedActionIds);
            },
            hideComponents: function (panel, rec) {
                var hideComponent = function (cmp, isHide) {
                        cmp.allowBlank = isHide;
                        cmp.setVisible(!isHide);
                    },
                    typeContragentField = panel.down('b4enumcombo[name=TypeJurPerson]'),
                    contragentField = panel.down('b4selectfield[name=Contragent]'),
                    fioField = panel.down('textfield[name=PersonName]'),
                    innField = panel.down('textfield[name=Inn]'),
                    planField = panel.down('b4selectfield[name=PlanAction]'),
                    appealCitsField = panel.down('b4selectfield[name=AppealCits]');

                hideComponent(typeContragentField, rec.data.TypeObject == B4.enums.TypeObjectAction.Individual);
                hideComponent(contragentField, rec.data.TypeObject == B4.enums.TypeObjectAction.Individual);
                hideComponent(fioField, rec.data.TypeObject == B4.enums.TypeObjectAction.Legal);
                hideComponent(innField, rec.data.TypeObject == B4.enums.TypeObjectAction.Legal);
                hideComponent(planField, !(rec.data.TypeBase == B4.enums.TypeBaseAction.Plan));
                hideComponent(appealCitsField, !(rec.data.TypeBase == B4.enums.TypeBaseAction.Appeal));
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'taskInspectorMultiSelectWindowAspect',
            fieldSelector: '#taskactionEditPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskInspectorsSelectWindow',
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

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddInspectors', 'DocumentGjiInspector'),
                        method: 'POST',
                        params: {
                            inspectorIds: (recordIds),
                            documentId: asp.controller.params.documentId
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                        return false;
                    });
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'taskPlannedActionsMultiSelectWindowAspect',
            fieldSelector: '#taskactionEditPanel #tfPlannedActions',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskPlannedActionsMultiSelectWindow',
            storeSelect: 'dict.KnmAction',
            columnsGridSelect: [
                { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Запланированные действия',
            titleGridSelect: 'Запланированные действия',
            titleGridSelected: 'Выбранные запланированные действия',
            onBeforeLoad: function(store, operation) {
                var me = this;

                me.setIgnoreChanges(true);
                me.setKnmActionStoreParams(store, operation);
            },
            onSelectedBeforeLoad: function(store, operation) {
                var me = this,
                    field = me.getSelectField();

                if (field) {
                    operation.params[me.valueProperty] = field.getValue();
                    
                    me.setKnmActionStoreParams(store, operation);
                }
            },
            setKnmActionStoreParams: function(store, operation) {
                var me = this,
                    taskactionEditPanelAspect = me.controller.getAspect('taskactionEditPanelAspect'),
                    panel = taskactionEditPanelAspect.getPanel(),
                    kindActionField = panel.down('b4enumcombo[name=KindAction]');

                operation.params.currentDictOnly = true;
                operation.params.kindAction = kindActionField.getValue();
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskactionRealityObjectAspect',
            gridSelector: '#taskActionIsolatedHouseGrid',
            storeName: 'RealityObject',
            modelName: 'RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskactionRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
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
                            url: B4.Url.action('AddRealityObjects', 'TaskActionIsolatedRealityObject'),
                            method: 'POST',
                            params: {
                                roIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.updateGrid('actionisolated.taskaction.HouseGrid');
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
            xtype: 'gkhbuttonprintaspect',
            name: 'taskPrintAspect',
            buttonSelector: '#taskactionEditPanel #btnPrint',
            codeForm: 'TaskAction',
            getUserParams: function () {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'taskToActHouseAspect',
            buttonSelector: '#taskactionEditPanel [ruleId=TaskActionToActActionRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskToActByRoRuleSelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'actionisolated.taskaction.RealityObjectTaskForSelect',
            storeSelected: 'actionisolated.taskaction.RealityObjectTaskForSelected',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранный дом',

            onBeforeLoad: function (store, operation) {
                var me = this,
                    record = me.controller.getAspect('taskactionEditPanelAspect').getRecord();

                if (record) {
                    operation.params.documentId = record.get('Id');
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('taskactionCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
                        params = creationAspect.getParams(btn);
                        params.realityIds = recordIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'actionIsolatedTaskItemAspect',
            gridSelector: '#taskActionIsolatedItemGrid',
            storeName: 'actionisolated.taskaction.Item',
            modelName: 'actionisolated.taskaction.Item',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actionIsolatedTaskItemMultiSelectWindow',
            storeSelect: 'dict.SurveySubject',
            titleSelectWindow: 'Предметы мероприятия',
            titleGridSelect: 'Предметы для отбора',
            titleGridSelected: 'Выбранные предметы мероприятия',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddItems', 'TaskActionIsolatedItem', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Предметы мероприятия сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'taskActionIsolatedAnnexGridWindowAspect',
            gridSelector: '#taskActionIsolatedAnnexGrid',
            editFormSelector: '#taskActionIsolatedAnnexEditWindow',
            storeName: 'actionisolated.taskaction.Annex',
            modelName: 'actionisolated.taskaction.Annex',
            editWindowView: 'actionisolated.taskaction.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Task', asp.controller.params.documentId);
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskActionIsolatedSurveyPurposeGjiAspect',
            gridSelector: '#taskActionIsolatedSurveyPurposeGrid',
            storeName: 'actionisolated.taskaction.SurveyPurpose',
            modelName: 'actionisolated.taskaction.SurveyPurpose',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskActionIsolatedSurveyPurposeGjiAspectMultiSelectWindow',
            storeSelect: 'actionisolated.taskaction.SurveyPurposeForSelect',
            storeSelected: 'actionisolated.taskaction.SurveyPurposeForSelected',
            titleSelectWindow: 'Выбор целей мероприятия',
            titleGridSelect: 'Цели для отбора',
            titleGridSelected: 'Выбранные цели проверки',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: {xtype: 'textfield'}
                },
                {
                    header: 'Код',
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    filter: {xtype: 'textfield'}
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: {xtype: 'textfield'}
                }],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddPurposes', 'TaskActionIsolatedSurveyPurpose', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Цели мероприятия сохранены успешно');
                        asp.getGrid().getStore().load();
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskActionIsolatedArticleLawAspect',
            gridSelector: '#taskActionIsolatedArticleLawGrid',
            storeName: 'actionisolated.taskaction.ArticleLaw',
            modelName: 'actionisolated.taskaction.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskActionArticleLawMultiSelectWindow',
            storeSelect: 'dict.ArticleLawGjiForSelect',
            storeSelected: 'dict.ArticleLawGjiForSelected',
            titleSelectWindow: 'Выбор статей закона',
            titleGridSelect: 'Статьи для отбора',
            titleGridSelected: 'Выбранные статьи',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield'} }
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

                    if (recordIds.length > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddArticles', 'TaskActionIsolatedArticleLaw'),
                            params: {
                                articleLawIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать статьи закона');
                        return false;
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('actionisolated.TaskAction').on('beforeload', me.onBeforeLoad, me);
        me.getStore('actionisolated.taskaction.House').on('beforeload', me.onBeforeLoad, me);
        me.getStore('actionisolated.taskaction.Item').on('beforeload', me.onBeforeLoad, me);
        me.getStore('actionisolated.taskaction.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('actionisolated.taskaction.ArticleLaw').on('beforeload', me.onBeforeLoad, me);
        me.getStore('actionisolated.taskaction.SurveyPurpose').on('beforeload', me.onBeforeLoad, me);

        me.control({
            '#taskActionIsolatedHouseGrid': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            },
            '#taskActionIsolatedItemGrid': {
                afterrender: function (grid) {
                    grid.getStore().load();
                }
            },
            '#taskActionIsolatedAnnexGrid': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            },
            '#taskActionIsolatedArticleLawGrid': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            },
            '#taskActionIsolatedSurveyPurposeGrid': {
                afterrender: function (grid) {
                    grid.getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('taskactionEditPanelAspect').setData(me.params.documentId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});