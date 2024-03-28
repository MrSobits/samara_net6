Ext.define('B4.controller.ActRemoval', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.InlineGrid',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.aspects.permission.ActRemoval',
        'B4.Ajax',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Url',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhBlobText',
        'B4.aspects.StateContextMenu',
        'B4.aspects.StateGridWindowColumn'
    ],

    models: [
        'ActRemoval',
        'actremoval.Violation',
        'actremoval.Annex',
        'actremoval.Period',
        'actremoval.Witness',
        'actremoval.Definition',
        'actremoval.InspectedPart',
        'actremoval.ProvidedDoc',
        'Prescription',
        'ProtocolGji'
    ],

    stores: [
        'ActRemoval',
        'actremoval.Violation',
        'actremoval.ViolationForSelect',
        'actremoval.ViolationForSelected',
        'actremoval.Annex',
        'actremoval.Period',
        'actremoval.Witness',
        'actremoval.Definition',
        'actremoval.InspectedPart',
        'actremoval.ProvidedDoc',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.InspectedPartGjiForSelect',
        'dict.InspectedPartGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected'
    ],

    views: [
        'actremoval.EditPanel',
        'actremoval.ViolationGrid',
        'actremoval.ViolationEditWindow',
        'actremoval.AnnexGrid',
        'actremoval.AnnexEditWindow',
        'actremoval.WitnessGrid',
        'actremoval.PeriodGrid',
        'actremoval.PeriodEditWindow',
        'actremoval.DefinitionGrid',
        'actremoval.DefinitionEditWindow',
        'actremoval.InspectedPartGrid',
        'actremoval.InspectedPartEditWindow',
        'actremoval.ProvidedDocGrid',
        'SelectWindow.MultiSelectWindow',
        'actremoval.Grid'
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
            xtype: 'b4_state_contextmenu',
            name: 'actRemovalStateTransferAspect',
            gridSelector: '#actRemovalGrid',
            stateType: 'gji_document_actrem',
            menuSelector: 'actRemovalStateTransferStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    var model = asp.controller.getModel('ActRemoval');
                    model.load(record.getId());
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
                var me = this,
                    param = { DocumentId: me.controller.params.documentId };
                me.params.userParams = Ext.JSON.encode(param);
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
                    modifiedRecs,
                    removedRecs,
                    typeRemoval,
                    asp = me.controller.getAspect('actRemovalViolationAspect'),
                    store = asp.getGrid().getStore();

                modifiedRecs = store.getModifiedRecords();
                removedRecs = store.getRemovedRecords();
                
                typeRemoval = me.getPanel().down('#cbTypeRemoval').getValue();

                if (!me.isTypeRemovalValid(store, typeRemoval)) {
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
                var result = true,
                    allFilled,
                    anyEmpty;

                if (value == 10) {
                    // Нарушения устранены == 'Да'  =>  У всех записей должна быть проставлена дата фактического исполнения
                    allFilled = true;
                    store.each(function(record) {
                        allFilled = allFilled && !Ext.isEmpty(record.get('DateFactRemoval'));
                    });

                    result = allFilled;
                } else if (value == 20) {
                    // Нарушения устранены == 'Нет' =>  Хотя бы у одной записи должна быть пустая дата фактического исполнения
                    anyEmpty = false;
                    store.each(function (record) {
                        anyEmpty = anyEmpty || Ext.isEmpty(record.get('DateFactRemoval'));
                    });
                    
                    result = anyEmpty;
                }

                return result;
            },
            
            otherActions: function (actions) {
                var me = this;

                actions[me.editPanelSelector + ' #cbTypeRemoval'] = { 'change': { fn: me.setDateFactRemoval, scope: me } };
            },

            //Если выставили дату акта проверки нарушения и нарушения устранены == да то проставляем дату в дату факт устранения если там пусто
            setDateFactRemoval: function (field, newValue) {
                var me = this,
                    storeRemovalViolation,
                    documentDate = me.getPanel().down('#dfDocumentDate').getValue();


                if (!Ext.isEmpty(documentDate) && newValue == 10) {
                    storeRemovalViolation = me.getPanel().down('#actRemovalViolationGrid').getStore();

                    storeRemovalViolation.each(function (record) {
                        if (Ext.isEmpty(record.get('DateFactRemoval'))) {
                            record.set('DateFactRemoval', documentDate);
                        }
                    });
                }
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

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    callbackUnMask;

                asp.controller.params = asp.controller.params || {};
                
                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;
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
                    var obj = Ext.JSON.decode(response.responseText),
                        fieldInspectors,
                        fieldBase;

                    fieldInspectors = panel.down('#actRemovalInspectorsTrigerField');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    fieldBase = panel.down('#actRemovalBaseTextField');
                    fieldBase.setValue(obj.baseName);

                    me.disableButtons();
                }).error(function () {
                    me.controller.unmask();
                });
                
                //Обновляем таблицу Лиц присутсвующих при проверке
                me.controller.getStore('actremoval.Witness').load();

                //Обновляем таблицу Дата и время проведения проверки
                me.controller.getStore('actremoval.Period').load();

                //Обновляем таблицу Приложений
                me.controller.getStore('actremoval.Annex').load();

                //Обновляем таблицу определений
                me.controller.getStore('actremoval.Definition').load();

                //Обновляем таблицу инспектируемых частей
                me.controller.getStore('actremoval.InspectedPart').load();

                //Обновляем таблицу предосталвяемые докуенты 
                me.controller.getStore('actremoval.ProvidedDoc').load();

                //Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('actRemovalStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // заполняем печать
                me.controller.getAspect('actRemovalPrintAspect').loadReportStore();
                
                // обновляем кнопку Сформирвоать
                me.controller.getAspect('actRemovalCreateButtonAspect').setData(rec.get('Id'));

                me.controller.getAspect('actRemovalPersonViolationInfoEditWindowBlobTextAspect').doInjection();
                me.controller.getAspect('actRemovalPersonViolationActionInfoEditWindowBlobTextAspect').doInjection();
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
               Аспект взаимодействия кнопки Предписание с массовой формой выбора нарушений
               По нажатию на кнопку Предписание будет открыта форма массовго выбора нарушений
               а после отбора будет вызван метд у главного аспекта createPrescription с передачей выбранных Ids нарушений
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actRemovalToPrescriptionAspect',
            buttonSelector: '#actRemovalEditPanel [ruleId=ActRemovalToPrescriptionRule]',
            multiSelectWindowSelector: '#actRemovalToPrescriptionMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'actremoval.ViolationForSelect',
            storeSelected: 'actremoval.ViolationForSelected',
            columnsGridSelect: [
                { header: 'Пункты НПД', dataIndex: 'CodesPin', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Текст нарушения', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq} },
                { header: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Адрес', dataIndex: 'RealityObject', flex: 1, sortable: false },
                { header: 'Пункты НПД', dataIndex: 'CodesPin', flex: 1, sortable: false },
                { header: 'Нарушение', dataIndex: 'ViolationGji', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            onBeforeLoad: function (store, operation) {
                var me = this;
                if (me.controller.params && me.controller.params.documentId > 0) {
                    operation.params.documentId = me.controller.params.documentId;
                    operation.params.forSelect = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    records.each(function(rec, index) {
                         listIds.push(rec.get('InspectionViolationId'));
                    });

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actRemovalCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем нарушения)
                        params = creationAspect.getParams(btn);
                        params.violationIds = listIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {   /* 
               Аспект взаимодействия кнопки Протокол с массовой формой выбора нарушений
               По нажатию на кнопку Протокол будет открыта форма массовго выбора нарушений
               а после отбора будет вызван метд у главного аспекта createProtocol с передачей выбранных Ids нарушений
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actRemovalToProtocolAspect',
            buttonSelector: '#actRemovalEditPanel [ruleId=ActRemovalToProtocolRule]',
            multiSelectWindowSelector: '#actRemovalToProtocolMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'actremoval.ViolationForSelect',
            storeSelected: 'actremoval.ViolationForSelected',
            columnsGridSelect: [
                { header: 'Пункты НПД', dataIndex: 'CodesPin', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Текст нарушения', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq} },
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
                { header: 'Адрес', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Адрес', dataIndex: 'RealityObject', flex: 1, sortable: false },
                { header: 'Пункты НПД', dataIndex: 'CodesPin', flex: 1, sortable: false },
                { header: 'Нарушение', dataIndex: 'ViolationGji', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            onBeforeLoad: function (store, operation) {
                var me = this;

                if (me.controller.params && me.controller.params.documentId > 0) {
                    operation.params.documentId = me.controller.params.documentId;
                    operation.params.forSelect = true;
                }

            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    records.each(function(rec, index) {
                        listIds.push(rec.get('InspectionViolationId'));
                    });

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actRemovalCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем нарушения)
                        params = creationAspect.getParams(btn);
                        params.violationIds = listIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
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
            xtype: 'gkhblobtextaspect',
            name: 'actRemovalPersonViolationInfoEditWindowBlobTextAspect',
            fieldSelector: 'textarea[name=PersonViolationInfo]',
            editPanelAspectName: 'actRemovalEditPanelAspect',
            controllerName: 'NsoDocumentGji',
            valueFieldName: 'PersonViolationInfo',
            getAction: 'GetPersonViolationInfo',
            saveAction: 'SavePersonViolationInfo',
            previewLength: 200,
            autoSavePreview: true,
            previewField: false,
            getParentRecordId: function () {
                return this.controller.params.documentId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actRemovalPersonViolationActionInfoEditWindowBlobTextAspect',
            fieldSelector: 'textarea[name=PersonViolationActionInfo]',
            editPanelAspectName: 'actRemovalEditPanelAspect',
            controllerName: 'NsoDocumentGji',
            valueFieldName: 'PersonViolationActionInfo',
            getAction: 'GetPersonViolationActionInfo',
            saveAction: 'SavePersonViolationActionInfo',
            previewLength: 200,
            autoSavePreview: true,
            previewField: false,
            getParentRecordId: function () {
                return this.controller.params.documentId;
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'actRemovalWitnessAspect',
            storeName: 'actremoval.Witness',
            modelName: 'actremoval.Witness',
            gridSelector: '#actRemovalWitnessGrid',
            saveButtonSelector: '#actRemovalEditPanel #actRemovalWitnessSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('ActRemoval', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'actRemovalPeriodAspect',
            gridSelector: '#actRemovalPeriodGrid',
            editFormSelector: '#actRemovalPeriodEditWindow',
            storeName: 'actremoval.Period',
            modelName: 'actremoval.Period',
            editWindowView: 'actremoval.PeriodEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ActRemoval', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'actRemovalAnnexAspect',
            gridSelector: '#actRemovalAnnexGrid',
            editFormSelector: '#actRemovalAnnexEditWindow',
            storeName: 'actremoval.Annex',
            modelName: 'actremoval.Annex',
            editWindowView: 'actremoval.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ActRemoval', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'actRemovalDefinitionAspect',
            gridSelector: '#actRemovalDefinitionGrid',
            editFormSelector: '#actRemovalDefinitionEditWindow',
            storeName: 'actremoval.Definition',
            modelName: 'actremoval.Definition',
            editWindowView: 'actremoval.DefinitionEditWindow',
            onSaveSuccess: function (asp, record) {
                asp.setDefinitionId(record.getId());
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ActRemoval', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record) {
                    asp.setDefinitionId(record.getId());
                }
            },
            setDefinitionId: function (id) {
                this.controller.params.definitionId = id;
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'actRemovalInspectedPartAspect',
            gridSelector: '#actRemovalInspectedPartGrid',
            storeName: 'actremoval.InspectedPart',
            modelName: 'actremoval.InspectedPart',
            editFormSelector: '#actRemovalInspectedPartEditWindow',
            editWindowView: 'actremoval.InspectedPartEditWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actRemovalInspectedPartMultiSelectWindow',
            storeSelect: 'dict.InspectedPartGjiForSelect',
            storeSelected: 'dict.InspectedPartGjiForSelected',
            titleSelectWindow: 'Выбор инспектируемых частей',
            titleGridSelect: 'Элементы для отбора',
            titleGridSelected: 'Выбранные элементы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
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
                        });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddInspectedParts', 'ActRemovalInspectedPart', {
                            partIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать инспектируемые части');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actRemovalProvidedDocAspect',
            gridSelector: '#actRemovalProvidedDocGrid',
            saveButtonSelector: '#actRemovalProvidedDocGrid #actProvidedDocGridSaveButton',
            storeName: 'actremoval.ProvidedDoc',
            modelName: 'actremoval.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actRemovalProvidedDocMultiSelectWindow',
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
                actions['#actRemovalProvidedDocGrid #actProvidedDocGridUpdateButton'] = {
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
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'ActRemovalProvidedDoc', {
                            providedDocIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
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
        }
    ],

    init: function () {
        var me = this;

        me.getStore('actremoval.Violation').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actremoval.Witness').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actremoval.Definition').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actremoval.Annex').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actremoval.Period').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actremoval.InspectedPart').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actremoval.ProvidedDoc').on('beforeload', me.onObjectBeforeLoad, me);

        me.control({
            '#actRemovalEditPanel #btnMerge': { click: { fn: me.mergeActs, scope: me } }
        });

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('actRemovalEditPanelAspect').setData(me.params.documentId);

            //загружаем стор нарушений акта проверки предписания
            me.getStore('actremoval.Violation').load();
        }
    },

    onObjectBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params && me.params.documentId > 0) {
            operation.params.documentId = me.params.documentId;
        }
    },

    mergeActs: function(btn) {
        var me = this,
            panel = btn.up('actRemovalEditPanel');

        me.mask('Объединение актов...', panel);

        B4.Ajax.request({
            url: B4.Url.action('MergeActs', 'ActRemoval'),
            timeout: 9999999,
            params: {
                documentId: me.params.documentId
            }
        }).next(function(resp) {
            var obj = Ext.decode(resp.responseText);
            var editPanelAspect = me.getAspect('actRemovalEditPanelAspect');

            editPanelAspect.setData(me.params.documentId);
            editPanelAspect.reloadTreePanel();

            if (obj && obj.message) {
                B4.QuickMsg.msg('Объединение актов', obj.message, 'success');
            }

            me.unmask();
        }).error(function(error) {
            me.unmask();
            Ext.Msg.alert('Ошибка', error.message || error.message || error);
        });
    }
});