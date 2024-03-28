Ext.define('B4.controller.preventiveaction.task.Task', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.StateButton',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.preventiveaction.Task',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.enums.PreventiveActionType'
    ],
    
    models: [
        'preventiveaction.PreventiveActionTask',
        'preventiveaction.PreventiveActionTaskPlannedAction',
        'preventiveaction.PreventiveActionTaskConsultingQuestion',
        'preventiveaction.PreventiveActionTaskRegulation',
        'preventiveaction.PreventiveActionTaskItem',
        'preventiveaction.TaskOfPreventiveActionTask',
        'preventiveaction.PreventiveActionTaskConsultingQuestion',
        'preventiveaction.task.Objective'
    ],
    
    stores: [
        'preventiveaction.PreventiveActionTaskPlannedAction',
        'preventiveaction.PreventiveActionTaskConsultingQuestion',
        'preventiveaction.PreventiveActionTaskItem',
        'preventiveaction.TaskOfPreventiveActionTask',
        'preventiveaction.TaskOfPreventiveActionTaskForSelect',
        'preventiveaction.PreventiveActionTaskRegulation',
        'dict.NormativeDocForSelect',
        'dict.NormativeDocForSelected',
        'preventiveaction.TaskOfPreventiveActionTaskForSelected',
        'preventiveaction.PreventiveActionTaskConsultingQuestion',
        'preventiveaction.task.Objective',
        'preventiveaction.task.ObjectiveForSelect',
        'preventiveaction.task.ObjectiveForSelected'
    ],
    
    views: [
        'preventiveaction.task.EditPanel',
        'preventiveaction.task.MainInfoTabPanel',
        'preventiveaction.task.PlannedActionGrid',
        'preventiveaction.task.TaskOfPreventiveActionTaskGrid',
        'preventiveaction.task.RegulationsTabPanel',
        'preventiveaction.task.ObjectiveTabPanel'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'preventiveaction.task.EditPanel',
    mainViewSelector: '#preventiveActionTaskEditPanel',
    
    aspects: [
        {
            xtype: 'preventiveactiontaskpermissions',
            editFormAspectName: 'taskEditPanelAspect'
        },
        {
            xtype: 'gjidocumentaspect',
            name: 'taskEditPanelAspect',
            editPanelSelector: '#preventiveActionTaskEditPanel',
            modelName: 'preventiveaction.PreventiveActionTask',
            otherActions: function (actions) {
            },
            onSaveSuccess: function (asp, rec) {
                //исключение изменения заголовка панели
            },
            onAfterSetPanelData: function (asp, rec, panel) {
                asp.controller.params = asp.controller.params || {};

                panel.down('#taskTabPanel').setActiveTab(0);
                panel.down('#plannedActionGrid').getStore().load();

                asp.disableButtons(false);

                asp.controller.getAspect('taskStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                asp.controller.getAspect('taskCreateButtonAspect').setData(rec.get('Id'));

                var printAspect = asp.controller.getAspect('taskPrintAspect'),
                    visitType = panel.down('[name=ActionType]').value;

                printAspect.codeForm = asp.controller.getCodeForm(visitType);
                printAspect.loadReportStore();

                var callbackUnMask = asp.controller.params?.callbackUnMask,
                    objectiveStore = panel.down('objectivetabpanel').getStore();

                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                objectiveStore.on('beforeload', asp.controller.onBeforeLoad, asp.controller);
                objectiveStore.load();
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
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'plannedActionAspect',
            storeName: 'preventiveaction.PreventiveActionTaskPlannedAction',
            modelName: 'preventiveaction.PreventiveActionTaskPlannedAction',
            gridSelector: '#plannedActionGrid',
            saveButtonSelector: '#plannedActionGrid #plannedActionSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    var data = store.getRange();

                    // Удаляем из стора записи с незаполненными действиями
                    data.forEach(function(rec){
                        if(rec.get('Action') === ''){
                            store.remove(rec);
                        }
                    });

                    return asp.controller.storeModifiedRowsSave(store);
                }
            }
        },
        {
            /*
            Аспект взаимодействия таблицы Лица присутсвующие при проверке, как инлайн грид
            */
            xtype: 'gkhinlinegridaspect',
            name: 'consultingQuestionAspect',
            storeName: 'preventiveaction.PreventiveActionTaskConsultingQuestion',
            modelName: 'preventiveaction.PreventiveActionTaskConsultingQuestion',
            gridSelector: '#consultingQuestionGrid',
            saveButtonSelector: '#consultingQuestionGrid #consultingQuestionSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    var data = store.getRange();

                    // Удаляем из стора записи с незаполненными полями
                    data.forEach(function(rec){
                        if (rec.get('Question') === '' &&
                            rec.get('Answer') === '' &&
                            rec.get('ControlledPerson') === '') {
                            store.remove(rec);
                        }
                    });

                    return asp.controller.storeModifiedRowsSave(store);
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskRegulationsAspect',
            gridSelector: '#preventiveActionTaskRegulationsTabPanel',
            storeName: 'preventiveaction.PreventiveActionTaskRegulation',
            modelName: 'preventiveaction.PreventiveActionTaskRegulation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#preventiveActionTaskRegulationsMultiSelectWindow',
            storeSelect: 'dict.NormativeDocForSelect',
            storeSelected: 'dict.NormativeDocForSelected',
            titleSelectWindow: 'Выбор нормативных документов',
            titleGridSelect: 'Нормативные документы для отбора',
            titleGridSelected: 'Выбранные нормативные документы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddNormativeDocs', 'PreventiveActionTaskRegulation', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Нормативные документы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'taskCreateButtonAspect',
            buttonSelector: '#preventiveActionTaskEditPanel gjidocumentcreatebutton',
            containerSelector: '#preventiveActionTaskEditPanel',
            typeDocument: 220 //B4.enums.TypeDocumentGji.PreventiveActionTask
        },
        {
            xtype: 'statebuttonaspect',
            name: 'taskStateButtonAspect',
            stateButtonSelector: '#preventiveActionTaskEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    asp.controller.getAspect('taskEditPanelAspect').setData(entityId);
                    asp.controller.getMainView().up('#preventiveActionNavigationPanel').getComponent('preventiveActionMenuTree').getStore().load();
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'taskPrintAspect',
            buttonSelector: '#preventiveActionTaskEditPanel #btnPrint',
            codeForm: '', // Форма задается в аспекте 'taskEditPanelAspect'
            getUserParams: function () {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskOfPreventiveActionTaskGjiAspect',
            gridSelector: '#taskOfPreventiveActionTaskGrid',
            storeName: 'preventiveaction.TaskOfPreventiveActionTask',
            modelName: 'preventiveaction.TaskOfPreventiveActionTask',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskOfPreventiveActionTaskGjiAspectMultiSelectWindow',
            storeSelect: 'preventiveaction.TaskOfPreventiveActionTaskForSelect',
            storeSelected: 'preventiveaction.TaskOfPreventiveActionTaskForSelected',
            titleSelectWindow: 'Выбор задач',
            titleGridSelect: 'Задачи для отбора',
            titleGridSelected: 'Выбранные задачи',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
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
                    B4.Ajax.request(B4.Url.action('AddTasks', 'TaskOfPreventiveActionTask', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Задачи мероприятия сохранены успешно');
                        asp.getGrid().getStore().load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                    
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'objectivePreventiveMeasuresAspect',
            gridSelector: 'objectivetabpanel',
            storeName: 'preventiveaction.task.Objective',
            modelName: 'preventiveaction.task.Objective',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#preventiveActionTaskObjectiveMultiSelectWindow',
            storeSelect: 'preventiveaction.task.ObjectiveForSelect',
            storeSelected: 'preventiveaction.task.ObjectiveForSelected',
            titleSelectWindow: 'Выбор целей',
            titleGridSelect: 'Цели для отбора',
            titleGridSelected: 'Выбранные цели',
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

                    records.each(function (rec, index) {
                        recordIds.push(rec.data.Id);
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddObjective', 'PreventiveActionTaskObjective'),
                            method: 'POST',
                            params: {
                                objectiveIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.updateGrid('preventiveaction.task.ObjectiveTabPanel');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать цели');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'preventiveActionTaskItemAspect',
            gridSelector: '#preventiveActionTaskItemGrid',
            storeName: 'preventiveaction.PreventiveActionTaskItem',
            modelName: 'preventiveaction.PreventiveActionTaskItem',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#preventiveActionTaskItemMultiSelectWindow',
            storeSelect: 'dict.PreventiveActionItems',
            titleSelectWindow: 'Предметы профилактических мероприятий',
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
                    B4.Ajax.request(B4.Url.action('AddItems', 'PreventiveActionTaskItem', {
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

                    return true;
                }
            }
        }
    ],
    
    init: function(){
        var me = this;

        me.control({
            '#consultingQuestionGrid': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            },

            '#taskOfPreventiveActionTaskGrid': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            },

            '#preventiveActionTaskRegulationsTabPanel': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            },

            '#preventiveActionTaskItemGrid': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            },
            
            'preventiveactiontaskmaininfotabpanel [name=ActionType]': {
                change: me.onActionTypeChanged
            }
        });
        
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('taskEditPanelAspect').setData(me.params.documentId);
            me.getAspect('taskOfPreventiveActionTaskGjiAspect').getGrid().getStore().on('beforeload', me.onBeforeLoad, me);
            me.getAspect('taskRegulationsAspect').getGrid().getStore().on('beforeload', me.onBeforeLoad, me);
            me.getAspect('plannedActionAspect').getGrid().getStore().on('beforeload', me.onBeforeLoad, me);
            me.getAspect('consultingQuestionAspect').getGrid().getStore().on('beforeload', me.onBeforeLoad, me);
            me.getAspect('preventiveActionTaskItemAspect').getGrid().getStore().on('beforeload', me.onBeforeLoad, me);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    },

    onActionTypeChanged: function(cmp, newValue){
        var visitTypeField = cmp.up('preventiveactiontaskmaininfotabpanel').down('[name=VisitType]'), 
            counselingTypeField = cmp.up('preventiveactiontaskmaininfotabpanel').down('[name=CounselingType]');
        
        if(newValue === B4.enums.PreventiveActionType.Consultation){
            visitTypeField.hide();
            counselingTypeField.show();
        } else {
            visitTypeField.show();
            counselingTypeField.hide();
        }
    },

    storeModifiedRowsSave: function (store) {
        var me = this,
            modifiedRowsCount = store.getModifiedRecords().length +
            store.getRemovedRecords().length;

        if (modifiedRowsCount === 0) {
            return false;
        }

        store.each(function (rec) {
            //Для новых записей присваиваем родительский документ
            if (!rec.get('Id')) {
                rec.set('Task', me.params.documentId);
            }
        });

        return true;
    },

    getCodeForm: function (visitType) {
        switch (visitType) {
            case B4.enums.PreventiveActionType.Visit:
                return 'PreventiveActionTaskSheet,PreventiveActionVisitNotification';
            case B4.enums.PreventiveActionType.Consultation:
                return 'PreventiveActionTaskConsultation';
            default: return '';
        }
    }
});