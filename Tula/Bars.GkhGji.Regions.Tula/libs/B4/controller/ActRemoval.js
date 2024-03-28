Ext.define('B4.controller.ActRemoval', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.InlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.aspects.permission.ActRemoval',
        'B4.Ajax',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Url',
        'B4.aspects.GkhBlobText',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    models: [
        'ActRemoval',
        'actremoval.Violation',
        'Prescription',
        'ProtocolGji',
        'violationgroup.ViolationGroup'
    ],

    stores: [
        'ActRemoval',
        'actremoval.Violation',
        'actremoval.ViolationForSelect',
        'actremoval.ViolationForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'actremoval.ViolationGroup',
        'violationgroup.ViolationGroupForSelect',
        'violationgroup.ViolationGroupForSelected'
    ],

    views: [
        'actremoval.EditPanel',
        'actremoval.ViolationGrid',
        'actremoval.ViolationEditWindow',
        'SelectWindow.MultiSelectWindow',
        'actremoval.ViolationGroupGrid',
        'actremoval.ViolationGroupEditWindow'
    ],

    mainView: 'actremoval.EditPanel',
    mainViewSelector: '#actRemovalEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Акта устранения нарушений (Акт проверки предписания)
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'actRemovalCreateButtonAspect',
            buttonSelector: '#actRemovalEditPanel gjidocumentcreatebutton',
            containerSelector: '#actRemovalEditPanel',
            typeDocument: 30, // Тип документа Акта устранения нарушений (Акт проверки предписания)
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил для которых необходимы пользовательские параметры
                // при созданни документов из акта проверки предписания все правила ожидают пользовательские параметры
                if (params.ruleId == 'ActRemovalToProtocolRule'
                    || params.ruleId == 'ActRemovalToPrescriptionRule') {
                    return false;
                }
            }
        },
        {
            /**
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'actRemovalStateButtonAspect',
            stateButtonSelector: '#actRemovalEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('actRemovalEditPanelAspect');

                    editPanelAspect.setData(entityId);
                }
            }
        },
        {
            //аспект пермишенов, /libs/B4/aspect/permission/
            xtype: 'actremovalperm',
            editFormAspectName: 'actRemovalEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'actRemovalPrintAspect',
            buttonSelector: '#actRemovalEditPanel #btnPrint',
            codeForm: 'ActRemoval',
            getUserParams: function (reportId) {
                var param = { DocumentId: this.controller.params.documentId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /*
            * Аспект для основной панели Акта предписания (он же Акт устранения нарушений)
            */
            xtype: 'gjidocumentaspect',
            name: 'actRemovalEditPanelAspect',
            editPanelSelector: '#actRemovalEditPanel',
            modelName: 'ActRemoval',
            
            saveRecord: function (rec) {
                
                var me = this,
                    asp = this.controller.getAspect('actRemovalViolationAspect');
                store = asp.getGrid().getStore();

                var modifiedRecs = store.getModifiedRecords();
                var removedRecs = store.getRemovedRecords();
                
                var typeRemoval = this.getPanel().down('#cbTypeRemoval').getValue();

                if (!this.isTypeRemovalValid(store, typeRemoval)) {
                    Ext.Msg.alert('Ошибка!', 'Неправильно установлено значение в поле "Нарушения устранены", либо неверно заполнены даты фактического исполнения нарушений.');
                    return;
                }
                
                //Если есть записи нарушений на сохранение то сначала сохраняем стор потом только запись ActRemoval
                if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                    store.sync({
                        callback: function () {
                            if (me.hasUpload()) {
                                me.saveRecordHasUpload(rec);
                            } else {
                                me.saveRecordHasNotUpload(rec);
                            }
                            store.load();
                        },
                        // выводим сообщение при ошибке сохранения
                        failure: function (result) {
                            if (result && result.exceptions[0] && result.exceptions[0].response) {
                                Ext.Msg.alert('Ошибка!', Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                            }
                        }
                    });
                } else {
                    if (me.hasUpload()) {
                        me.saveRecordHasUpload(rec);
                    } else {
                        me.saveRecordHasNotUpload(rec);
                    }
                }
            },
            
            isTypeRemovalValid: function (store, value) {
                var result = true;

                if (value == 10) {
                    // Нарушения устранены == 'Да'  =>  У всех записей должна быть проставлена дата фактического исполнения
                    var allFilled = true;
                    store.each(function(record) {
                        var dateFactRemoval = record.get('DateFactRemoval');
                        allFilled = allFilled && !Ext.isEmpty(dateFactRemoval);
                    });

                    result = allFilled;
                } else if (value == 20) {
                    // Нарушения устранены == 'Нет' =>  Хотя бы у одной записи должна быть пустая дата фактического исполнения
                    var anyEmpty = false;
                    store.each(function (record) {
                        var dateFactRemoval = record.get('DateFactRemoval');
                        anyEmpty = anyEmpty || Ext.isEmpty(dateFactRemoval);
                    });
                    
                    result = anyEmpty;
                }

                return result;
            },
            
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbTypeRemoval'] = { 'change': { fn: this.setDateFactRemoval, scope: this} };
            },

            //Если выставили дату акта проверки нарушения и нарушения устранены == да то проставляем дату в дату факт устранения если там пусто
            setDateFactRemoval: function (field, newValue) {
                var documentDate = this.getPanel().down('#dfDocumentDate').getValue();

                if (!Ext.isEmpty(documentDate) && newValue == 10) {
                    var storeRemovalViolation = this.getPanel().down('#actRemovalViolationGrid').getStore();

                    storeRemovalViolation.each(function (record) {
                        var dateFactRemoval = record.get('DateFactRemoval');
                        if (Ext.isEmpty(dateFactRemoval)) {
                            record.set('DateFactRemoval', documentDate);
                        }
                    });
                }
            },

            disableButtons: function (value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
                var idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this;
                
                asp.controller.params = asp.controller.params || {};
                
                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber'))
                    panel.setTitle('Акт проверки предписания ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Акт проверки предписания');
                
                //Делаем запросы на получение Инспекторов м документа основания
                //и обновляем соответсвующие Тригер филды
                me.controller.mask('загрузка', me.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'ActRemoval', {
                    documentId: asp.controller.params.documentId
                })).next(function (response) {
                    me.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldInspectors = panel.down('#actRemovalInspectorsTrigerField');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    var fieldBase = panel.down('#actRemovalBaseTextField');
                    fieldBase.setValue(obj.baseName);

                    me.disableButtons();
                }).error(function () {
                    me.controller.unmask();
                });
                
                //Передаем аспекту смены статуса необходимые параметры
                this.controller.getAspect('actRemovalStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // заполняем печать
                this.controller.getAspect('actRemovalPrintAspect').loadReportStore();
                
                // обновляем кнопку Сформирвоать
                this.controller.getAspect('actRemovalCreateButtonAspect').setData(rec.get('Id'));
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'actRemovalViolationAspect',
            storeName: 'actremoval.Violation',
            modelName: 'actremoval.Violation',
            gridSelector: '#actRemovalViolationGrid'
        },
        {   /* 
               Аспект взаимодействия кнопки Предписание с массовой формой выбора описаний
               По нажатию на кнопку Предписание будет открыта форма массовго выбора описаний
               а после отбора будет вызван метд у главного аспекта createPrescription с передачей выбранных Ids описаний
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actRemovalToPrescriptionAspect',
            buttonSelector: '#actRemovalEditPanel [ruleId=ActRemovalToPrescriptionRule]',
            multiSelectWindowSelector: '#actRemovalToPrescriptionMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'violationgroup.ViolationGroupForSelect',
            storeSelected: 'violationgroup.ViolationGroupForSelected',
            columnsGridSelect: [
                { header: 'Пункты нормативных документов', xtype: 'gridcolumn', dataIndex: 'PointCodes', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Описание', xtype: 'gridcolumn', dataIndex: 'Description', flex: 2, filter: { xtype: 'textfield' } },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Дата факт. устранения', xtype: 'datecolumn', dataIndex: 'DateFactRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Пункты', xtype: 'gridcolumn', dataIndex: 'PointCodes', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор описаний',
            titleGridSelect: 'Описания для отбора',
            titleGridSelected: 'Выбранные описания',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    Ext.Array.each(records.items,
                        function (item) {
                            listIds.push(item.get('Id'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actRemovalCreateButtonAspect');
                        // еще раз получаем параметры по умолчанию и добавляем к уже созданным еще один (Выбранные пользователем описания)
                        params = creationAspect.getParams(btn);
                        params.violationGroupIds = listIds;
                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать описания');
                        return false;
                    }
                    return true;
                }
            }
        },
        {   /* 
               Аспект взаимодействия кнопки Протокол с массовой формой выбора описаний
               По нажатию на кнопку Протокол будет открыта форма массовго выбора описаний
               а после отбора будет вызван метд у главного аспекта createProtocol с передачей выбранных Ids описаний
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actRemovalToProtocolAspect',
            buttonSelector: '#actRemovalEditPanel [ruleId=ActRemovalToProtocolRule]',
            multiSelectWindowSelector: '#actRemovalToProtocolMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'violationgroup.ViolationGroupForSelect',
            storeSelected: 'violationgroup.ViolationGroupForSelected',
            columnsGridSelect: [
                { header: 'Пункты нормативных документов', xtype: 'gridcolumn', dataIndex: 'PointCodes', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Описание', xtype: 'gridcolumn', dataIndex: 'Description', flex: 2, filter: { xtype: 'textfield' } },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Дата факт. устранения', xtype: 'datecolumn', dataIndex: 'DateFactRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Пункты', xtype: 'gridcolumn', dataIndex: 'PointCodes', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор описаний',
            titleGridSelect: 'Описания для отбора',
            titleGridSelected: 'Выбранные описания',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                }
            },
            listeners: {
                    getdata: function (asp, records) {
                        var me = this,
                            listIds = [],
                            btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                            creationAspect,
                            params;

                        Ext.Array.each(records.items,
                            function (item) {
                                listIds.push(item.get('Id'));
                            }, this);

                        if (listIds[0] > 0) {
                            creationAspect = asp.controller.getAspect('actRemovalCreateButtonAspect');
                            // еще раз получаем параметры по умолчанию и добавляем к уже созданным еще один (Выбранные пользователем описания)
                            params = creationAspect.getParams(btn);
                            params.violationGroupIds = listIds;

                            creationAspect.createDocument(params);
                        } else {
                            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать описания');
                            return false;
                        }
                        return true;
                    }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /ActRemoval/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actRemovalInspectorMultiSelectWindowAspect',
            fieldSelector: '#actRemovalEditPanel #actRemovalInspectorsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actRemovalInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
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
            * Аспект взаимодействия Таблицы Описаний с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actremovalViolGroupAspect',
            gridSelector: 'actremovalViolationGroupGrid',
            editFormSelector: 'actremovalViolationGroupEditWindow',
            storeName: 'actremoval.ViolationGroup',
            modelName: 'violationgroup.ViolationGroup',
            editWindowView: 'actremoval.ViolationGroupEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            closeWindowHandler: function () {
                this.getForm().close();
                this.updateGrid();
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actremovalBlobDescriptionAspect',
            fieldSelector: '[name=Description]',
            editPanelAspectName: 'actremovalViolGroupAspect',
            controllerName: 'DocumentViolGroup',
            valueFieldName: 'Description',
            previewLength: 150,
            autoSavePreview: true,
            previewField: 'Description'
        }
    ],

    init: function () {
        this.getStore('actremoval.Violation').on('beforeload', this.onViolationBeforeLoad, this);

        this.getStore('actremoval.ViolationGroup').on('beforeload', this.onViolationBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('actRemovalEditPanelAspect').setData(this.params.documentId);

            //загружаем стор нарушений акта проверки предписания
            this.getStore('actremoval.Violation').load();
            this.getStore('actremoval.ViolationGroup').load();
        }
    },

    onViolationBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});