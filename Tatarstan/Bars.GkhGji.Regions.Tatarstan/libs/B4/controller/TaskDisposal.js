Ext.define('B4.controller.TaskDisposal', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.Disposal',
        'B4.aspects.permission.TaskDisposal',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.FieldRequirementAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.DisposalTextValues',
        'B4.aspects.GkhBlobText',
        'B4.form.SelectWindow',
        'B4.QuickMsg',
        'B4.enums.TypeCheck',
        'B4.enums.TypeAgreementResult',
        'B4.enums.TypeAgreementProsecutor',
        'B4.enums.TypeDocumentGji'
    ],

    models: [
        'Disposal',
        'ActCheck',
        'ActSurvey',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.DisposalVerificationSubject',
        'disposal.TypeSurvey',
        'disposal.SurveyPurpose',
        'disposal.InspFoundationCheck',
        'disposal.SurveyObjective',
    ],

    stores: [
        'RealityObjectGjiForSelect',
        'RealityObjectGjiForSelected',
        'Disposal',
        'taskdisposal.Expert',
        'taskdisposal.ProvidedDoc',
        'taskdisposal.Annex',
        'taskdisposal.TypeSurvey',
        'taskdisposal.DisposalVerificationSubject',
        'taskdisposal.SurveyPurpose',
        'taskdisposal.InspFoundationCheck',
        'taskdisposal.SurveyObjective',
        'dict.SurveyPurposeForSelect',
        'dict.SurveyPurposeForSelected',
        'dict.SurveyObjectiveForSelect',
        'dict.SurveyObjectiveForSelected',
        'dict.NormativeDocForSelect',
        'dict.NormativeDocForSelected',
        'dict.SurveySubjectForSelect',
        'dict.SurveySubjectForSelected',
        'dict.TypeSurveyGjiForSelect',
        'dict.TypeSurveyGjiForSelected',
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ExpertGjiForSelect',
        'dict.ExpertGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected',
        'dict.Municipality',
    ],

    views: [
        'taskdisposal.EditPanel',
        'taskdisposal.AnnexEditWindow',
        'taskdisposal.AnnexGrid',
        'taskdisposal.ExpertGrid',
        'taskdisposal.InspFoundationCheckGrid',
        'taskdisposal.ProvidedDocGrid',
        'taskdisposal.SubjectVerificationGrid',
        'taskdisposal.SurveyObjectiveGrid',
        'taskdisposal.SurveyPurposeGrid',
        'taskdisposal.TypeSurveyGrid',

        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'taskdisposal.EditPanel',
    mainViewSelector: '#taskdisposalEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Задачи
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'taskDisposalCreateButtonAspect',
            buttonSelector: 'taskdisposaleditpanel #btnCreateTaskDisposal',
            containerSelector: 'taskdisposaleditpanel',
            typeDocument: 13, // Тип документа Задание
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId == 'TaskDisposalToActIsolatedRule') {
                    return false;
                }
            }
        },
        {
            xtype: 'taskdisposalperm',
            editFormAspectName: 'taskdisposalEditPanelAspect',
            name: 'TaskDisposalPermissionAspect'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitStart', applyTo: '[name=TimeVisitStart]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitEnd', applyTo: '[name=TimeVisitEnd]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcDate', applyTo: '[name=NcDate]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcNum', applyTo: '[name=NcNum]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcDateLatter', applyTo: '[name=NcDateLatter]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcNumLatter', applyTo: '[name=NcNumLatter]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcSent', applyTo: '[name=NcSent]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcObtained', applyTo: '[name=NcObtained]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.CountDays', applyTo: '[name=CountDays]', selector: '#taskdisposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.CountHours', applyTo: '[name=CountHours]', selector: '#taskdisposalEditPanel' }
            ]
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'taskdisposalStateButtonAspect',
            stateButtonSelector: '#taskdisposalEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('taskdisposalEditPanelAspect');

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
            name: 'taskdisposalEditPanelAspect',
            editPanelSelector: '#taskdisposalEditPanel',
            modelName: 'Disposal',
            otherActions: function(actions) {
                actions[this.editPanelSelector + ' #sfIssuredDisposal'] = { 'beforeload': { fn: this.onBeforeLoadInspectorManager, scope: this } };
            },
            saveRecord: function(rec) {
                var me = this;

                Ext.Msg.confirm('Внимание!', 'Убедитесь, что вид проверки указан правильно', function(result) {
                    if (result == 'yes') {
                        if (me.fireEvent('beforesave', me, rec) !== false) {
                            if (me.hasUpload()) {
                                me.saveRecordHasUpload(rec);
                            } else {
                                me.saveRecordHasNotUpload(rec);
                            }
                        }
                    }
                });
            },
            onBeforeLoadInspectorManager: function(field, options, store) {
                options = options || {};
                options.params = options.params || {};

                options.params.headOnly = true;

                return true;
            },
            disableButtons: function(value) {
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
            onAfterSetPanelData: function(asp, rec, panel) {
                var me = this,
                    title = 'Задание',
                    callbackUnMask,
                    cbTypeCheck = panel.down('#cbTypeCheck');

                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber'))
                    panel.setTitle(title+ " " + rec.get('DocumentNumber'));
                else
                    panel.setTitle(title);

                //получаем вид проверки
                if (asp.controller.params) {
                    asp.controller.params.kindCheckId = panel.down('#cbTypeCheck').getValue();
                }

                //Делаем запросы на получение Инспекторов
                //и обновляем соответсвующие Тригер филды
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('GetInfo', 'Disposal', { documentId: asp.controller.params.documentId }),
                    //для IE, чтобы не кэшировал GET запрос
                    cache: false
                }).next(function (response) {
                    asp.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText),
                        fieldInspectors = panel.down('#trigFInspectors');

                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    panel.down('#tfBaseName').setValue(obj.baseName);
                    panel.down('#tfPlanName').setValue(obj.planName);

                    asp.disableButtons(false);
                }).error(function() {
                    asp.controller.unmask();
                });

                me.controller.getStore('taskdisposal.Expert').load();
                me.controller.getStore('taskdisposal.ProvidedDoc').load();
                me.controller.getStore('taskdisposal.Annex').load();
                me.controller.getStore('taskdisposal.TypeSurvey').load();
                me.controller.getStore('taskdisposal.DisposalVerificationSubject').load();

                me.controller.getStore('taskdisposal.SurveyPurpose').load();
                me.controller.getStore('taskdisposal.SurveyObjective').load();
                me.controller.getStore('taskdisposal.InspFoundationCheck').load();

                me.controller.getAspect('taskdisposalNoticeDescriptionAspect').doInjection();

                // Значение по умолчанию "Внеплановая документарная" для поля "Вид проверки", если не задано
                if (cbTypeCheck.getValue() == null) {
                    panel.down('#cbTypeCheck').setValue(B4.enums.TypeCheck.NotPlannedDocumentation);
                }

                // Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('taskdisposalStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем отчеты
               // me.controller.getAspect('disposalPrintAspect').loadReportStore();

                me.controller.getAspect('taskDisposalCreateButtonAspect').setData(rec.get('Id'));
            },
            onSaveSuccess: function(asp, rec) {
                this.getPanel().setTitle('Задание ' + rec.get('DocumentNumber'));

                if (asp.controller.params)
                    asp.controller.params.kindCheckId = asp.getPanel().down('#cbTypeCheck').getValue();
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Акт на 1 дом и массовой формы выборка домов
            По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
            у главного аспекта вызывается метод создания документа createActCheck1House и передаются выбранные Id домов
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'taskdisposalToActCheck1HouseGJIAspect',
            buttonSelector: '#taskdisposalEditPanel [ruleId=DisposalToActCheckByRoRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalToActCheckByRoRuleSelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'RealityObjectGjiForSelect',
            storeSelected: 'RealityObjectGjiForSelected',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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

                //спрятать панель выбранных домов
                var wnd = Ext.ComponentQuery.query(this.multiSelectWindowSelector + ' #multiSelectedPanel')[0];
                if (wnd) {
                    wnd.hide();
                }

                if (this.controller.params && this.controller.params.inspectionId > 0)
                    operation.params.inspectionId = this.controller.params.inspectionId;
            },

            onRowSelect: function(rowModel, record, index, opt) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },

            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    records.each(function(rec, index) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('taskDisposalCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
                        params = creationAspect.getParams(btn);
                        params.realityIds = recordIds;
                        
                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дом');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Акт на обследование и массовой формы выборка домов
            По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
            у главного аспекта вызывается метод создания документа createActSurvey и передаются выбранные Id домов

            метод onRowSelect перекрываем потомучто необходимо изменить поведение выделения строки, чтобы можно было
            Выбрать только одну запись а не множество
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'taskdisposalToActSurveyAspect',
            buttonSelector: '#taskdisposalEditPanel [ruleId=DisposalToActSurveyRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskisposalToActSurveySelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'RealityObjectGjiForSelect',
            storeSelected: 'RealityObjectGjiForSelected',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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

            onBeforeLoad: function(store, operation) {
                //получаем текущую запись документа Распоряжения
                var record = this.controller.getAspect('taskdisposalEditPanelAspect').getRecord();

                if (this.controller.params && this.controller.params.inspectionId > 0)
                    operation.params.inspectionId = this.controller.params.inspectionId;

                if (record.get('TypeDisposal') == 20) {
                    //Если акт обследования делается из Распоряжения на проверку Предписания то
                    //получаем все дома из предписаний по которым создано это распоряжение
                    operation.params.documentId = record.get('Id');
                }
            },

            onRowSelect: function(rowModel, record, index, opt) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },

            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    records.each(function(rec, index) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('taskDisposalCreateButtonAspect');
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
            /* 
            Аспект взаимодействия кнопки создания Акта без взаимодействия и массовой формы выборка домов
            По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
            у главного аспекта вызывается метод создания документа createActCheck1House и передаются выбранные Id домов
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'taskDisposalToActIsolatedGJIAspect',
            buttonSelector: '#taskdisposalEditPanel [ruleId=TaskDisposalToActIsolatedRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskDisposalToActIsolatedRuleSelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'RealityObjectGjiForSelect',
            storeSelected: 'RealityObjectGjiForSelected',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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

                //спрятать панель выбранных домов
                var wnd = Ext.ComponentQuery.query(this.multiSelectWindowSelector + ' #multiSelectedPanel')[0];
                if (wnd) {
                    wnd.hide();
                }

                if (this.controller.params && this.controller.params.inspectionId > 0)
                    operation.params.inspectionId = this.controller.params.inspectionId;
                    operation.params.fromActIsolate = true;
            },

            onRowSelect: function (rowModel, record, index, opt) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec, index) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('taskDisposalCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
                        params = creationAspect.getParams(btn);
                        params.realityIds = recordIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дом');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            аспект взаимодействия грида типов обследований с массовой формой выбора типов обслуживания
            по нажатию на кнопку добавления показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем Типы обслуживания через серверный метод /DisposalTypeSurvey/AddTypeSurveys
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskdisposalTypeSurveyAspect',
            modelName: 'disposal.TypeSurvey',
            storeName: 'taskdisposal.TypeSurvey',
            gridSelector: 'taskdisposaltypesurveygrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalTypeSurveySelectWindow',
            storeSelect: 'dict.TypeSurveyGjiForSelect',
            storeSelected: 'dict.TypeSurveyGjiForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор типов обследования',
            titleGridSelect: 'Типы обследования для отбора',
            titleGridSelected: 'Выбранные типы обследования',

            onBeforeLoad: function(store, operation) {
                if (this.controller.params) {
                    operation.params.kindCheckId = this.controller.params.kindCheckId;
                }
            },

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddTypeSurveys', 'DisposalTypeSurvey', {
                        typeIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function(response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Типы обследований сохранены успешно');
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /Disposal/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'taskdisposalInspectorMultiSelectWindowAspect',
            fieldSelector: '#taskdisposalEditPanel #trigFInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalInspectorSelectWindow',
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
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec, index) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function(response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы экспертов с массовой формой выбора экспертов
            По нажатию на Добавить открывается форма выбора экспертов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение экспертов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskdisposalExpertAspect',
            gridSelector: 'taskdisposalexpertgrid',
            storeName: 'taskdisposal.Expert',
            modelName: 'disposal.Expert',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalExpertMultiSelectWindow',
            storeSelect: 'dict.ExpertGjiForSelect',
            storeSelected: 'dict.ExpertGjiForSelected',
            titleSelectWindow: 'Выбор экспертов',
            titleGridSelect: 'Эксперты для отбора',
            titleGridSelected: 'Выбранные эксперты',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function(asp, records) {

                    var recordIds = [];

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddExperts', 'DisposalExpert', {
                            expertIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать экспертов');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            По нажатию на Добавить открывается форма выбора предоставляемых документов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение предоставляемых документов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'taskdisposalProvidedDocumentAspect',
            gridSelector: 'taskdisposalprovideddocgrid',
            storeName: 'taskdisposal.ProvidedDoc',
            modelName: 'disposal.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalProvidedDocMultiSelectWindow',
            storeSelect: 'dict.ProvidedDocGjiForSelect',
            storeSelected: 'dict.ProvidedDocGjiForSelected',
            titleSelectWindow: 'Выбор предоставляемых документов',
            titleGridSelect: 'Документы для выбора',
            titleGridSelected: 'Выбранные документы',
            isPaginable: false,
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    renderer: function(value, metaData) {
                        metaData.style = "white-space: normal;";
                        return value;
                    }
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    sortable: false,
                    renderer: function(value, metaData) {
                        metaData.style = "white-space: normal;";
                        return value;
                    }
                }
            ],
            onBeforeLoad: function (store, operation) {
                operation.start = undefined;
                operation.page = undefined;
                operation.limit = undefined;
                operation.params.disposalId = this.controller.params.documentId;
            },
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'DisposalProvidedDoc', {
                            providedDocIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
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
            /*
             * Аспект взаимодействия Таблицы Приложений с формой редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'taskdisposalAnnexAspect',
            gridSelector: 'taskdisposalannexgrid',
            editFormSelector: 'taskdisposalannexeditwindow',
            storeName: 'taskdisposal.Annex',
            modelName: 'disposal.Annex',
            editWindowView: 'taskdisposal.AnnexEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Disposal', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'taskdisposalNoticeDescriptionAspect',
            fieldSelector: '[name=NoticeDescription]',
            editPanelAspectName: 'taskdisposalEditPanelAspect',
            getAction: 'GetNoticeDescription',
            saveAction: 'SaveNoticeDescription',
            controllerName: 'Disposal',
            valueFieldName: 'NoticeDescription',
            previewLength: 200,
            autoSavePreview: true,
            previewField: 'NoticeDescription'
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalSurveySubjectAspect',
            modelName: 'disposal.DisposalVerificationSubject',
            storeName: 'taskdisposal.DisposalVerificationSubject',
            gridSelector: 'taskdisposalsubjectverificationgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalSurveySubjectSelectWindow',
            storeSelect: 'dict.SurveySubjectForSelect',
            storeSelected: 'dict.SurveySubjectForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Предметы проверки',
            titleGridSelect: 'Предметы проверки для отбора',
            titleGridSelected: 'Выбранные предметы проверки',
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation = operation || {};
                operation.params = operation.params || {};
                if (me.controller.params) {
                    var existingIds = me.getGrid().getStore().collect('SurveySubjectId', false, true);
                    operation.params.ids = Ext.encode(existingIds);
                    operation.params.forSelect = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveySubjects', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();

                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.getStore('taskdisposal.TypeSurvey').load();
                        asp.controller.getStore('taskdisposal.SurveyPurpose').load();
                        asp.controller.getStore('taskdisposal.SurveyObjective').load();
                        asp.controller.getStore('taskdisposal.InspFoundationCheck').load();
                        asp.controller.getStore('taskdisposal.ProvidedDoc').load();

                        Ext.Msg.alert('Сохранение!', 'Предметы проверки сохранены успешно');
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
            name: 'taskdisposalSurveyPurposeAspect',
            modelName: 'disposal.SurveyPurpose',
            storeName: 'taskdisposal.SurveyPurpose',
            gridSelector: 'taskdisposalsurveypurposegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalSurveyPurposeSelectWindow',
            storeSelect: 'dict.SurveyPurposeForSelect',
            storeSelected: 'dict.SurveyPurposeForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор целей проверки',
            titleGridSelect: 'Цели проверки для отбора',
            titleGridSelected: 'Выбранные цели проверки',

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveyPurposes', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Цели проверки сохранены успешно');
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
            name: 'taskdisposalSurveyObjectiveAspect',
            modelName: 'disposal.SurveyObjective',
            storeName: 'taskdisposal.SurveyObjective',
            gridSelector: 'taskdisposalsurveyobjectivegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalSurveyObjectiveSelectWindow',
            storeSelect: 'dict.SurveyObjectiveForSelect',
            storeSelected: 'dict.SurveyObjectiveForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор задач проверки',
            titleGridSelect: 'Задачи проверки для отбора',
            titleGridSelected: 'Выбранные задачи проверки',

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveyObjectives', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Задачи проверки сохранены успешно');
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
            name: 'taskdisposalInspFoundationCheckAspect',
            modelName: 'disposal.InspFoundationCheck',
            storeName: 'taskdisposal.InspFoundationCheck',
            gridSelector: 'taskdisposalinspfoundationcheckgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskdisposalInspFoundationCheckSelectWindow',
            storeSelect: 'dict.NormativeDocForSelect',
            storeSelected: 'dict.NormativeDocForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нормативных документов',
            titleGridSelect: 'Нормативные документы для отбора',
            titleGridSelected: 'Выбранные нормативные документы',

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspFoundationChecks', 'Disposal', {
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
    ],

    init: function () {
        var me = this,
            editPanelAspect = me.getAspect('taskdisposalEditPanelAspect'),
            actions = {};

        me.getStore('taskdisposal.Expert').on('beforeload', me.onBeforeLoad, me);
        me.getStore('taskdisposal.ProvidedDoc').on('beforeload', me.onBeforeLoad, me);
        me.getStore('taskdisposal.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('taskdisposal.TypeSurvey').on('beforeload', me.onBeforeLoad, me);
        me.getStore('taskdisposal.DisposalVerificationSubject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('taskdisposal.SurveyObjective').on('beforeload', me.onBeforeLoad, me);
        me.getStore('taskdisposal.SurveyPurpose').on('beforeload', me.onBeforeLoad, me);
        me.getStore('taskdisposal.InspFoundationCheck').on('beforeload', me.onBeforeLoad, me);

        me.control({
            '#taskdisposalEditPanel  #cbTypeAgreementProsecutor, #cbTypeAgreementResult': { 'change': me.onChangeTypeAgreement }
        });

        me.callParent(arguments);

        me.control(actions);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('taskdisposalEditPanelAspect').setData(me.params.documentId);
        }
    },

    onBeforeLoad: function(store, operation) {
        if (this.params && this.params.documentId)
            operation.params.documentId = this.params.documentId;
    },

    onChangeTypeAgreement: function () {
        var me = this,
            view = me.getMainView(),
            docDate = view.down('#cbDocumentDateWithResultAgreement'),
            docNum = view.down('#cbDocumentNumberWithResultAgreement'),
            typeAgreementProsecutor = view.down('#cbTypeAgreementProsecutor').getValue(),
            typeAgreementResult = view.down('#cbTypeAgreementResult').getValue(),
            isEnabledocdate = ((typeAgreementResult === B4.enums.TypeAgreementResult.Agreed || typeAgreementResult === B4.enums.TypeAgreementResult.NotAgreed) && typeAgreementProsecutor === B4.enums.TypeAgreementProsecutor.RequiresAgreement) && docDate.allowedEdit,
            isEnabledocnum = ((typeAgreementResult === B4.enums.TypeAgreementResult.Agreed || typeAgreementResult === B4.enums.TypeAgreementResult.NotAgreed) && typeAgreementProsecutor === B4.enums.TypeAgreementProsecutor.RequiresAgreement) && docNum.allowedEdit;

        docDate.setDisabled(!isEnabledocdate);
        docNum.setDisabled(!isEnabledocnum);
    }
});