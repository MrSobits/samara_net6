Ext.define('B4.controller.Decision', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.aspects.GkhGjiNestedDigitalSignatureGridAspect',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.Disposal',
        'B4.aspects.permission.ChelyabinskDisposal',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GkhBlobText',
        'B4.form.SelectWindow',
        'B4.QuickMsg',
        'B4.enums.TypeCheck'
    ],

    models: [
        'DecisionGji',
        'ActCheck',
        'ActSurvey',
        'decision.Expert',
        'decision.ProvidedDoc',
        'decision.Annex',
        'decision.ControlList',
        'decision.ControlSubjects',
        'decision.InspectionReason',
        'decision.VerificationSubject',
        'decision.VerificationSubjectNormDocItem',
        'decision.AdminRegulation',
        'decision.ControlMeasures',
    ],

    stores: [
        'DecisionGji',
        'decision.Expert',
        'decision.ProvidedDoc',
        'decision.Annex',
        'RealityObjectGjiForSelect',
        'RealityObjectGjiForSelected',
        'dict.TypeSurveyGjiForSelect',
        'dict.Inspector',
        'dict.SurveySubjectForSelect',
        'dict.SurveySubjectForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ExpertGjiForSelect',
        'dict.ExpertGjiForSelected',
        'dict.ControlListForSelect',
        'dict.ControlListForSelected',
        'dict.InspectionReasonForSelect',
        'dict.InspectionReasonForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected',
        'decision.VerificationSubject',
        'decision.VerificationSubjectNormDocItem',
        'dict.Municipality',
        'decision.AdminRegulation',
        'decision.ControlList',
        'decision.InspectionReason',
        'dict.NormativeDocForSelect',
        'dict.NormativeDocForSelected',
        'dict.NormDocItemForSelect',
        'dict.NormDocItemForSelected',
        'dict.SurveySubjectForSelect',
        'dict.SurveySubjectForSelected',
        'decision.ControlSubjects',
        'decision.ControlMeasures'
    ],

    views: [
        'decision.EditPanel',
        'decision.AnnexEditWindow',
        'decision.AnnexGrid',
        'decision.ExpertGrid',
        'decision.ControlListGrid',
        'decision.InspectionReasonGrid',
        'decision.AdminRegulationGrid',
        'decision.DecisionControlMeasuresGrid',
        'decision.VerificationGrid',
        'decision.ProvidedDocGrid',
        'decision.ControlSubjectGrid',
        'decision.ControlSubjectWindow',
        'decision.VerificationSubjectPanel',
        'decision.NormDocItemGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'decision.EditPanel',
    mainViewSelector: '#decisionEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhgjinesteddigitalsignaturegridaspect',
            gridSelector: '#decisionAnnexGrid',
            controllerName: 'DecisionAnnex',
            name: 'decisionAnnexNestedSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            Аспект формирвоания документов для Распоряжения
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'decisionCreateButtonAspect',
            buttonSelector: 'decisioneditpanel gjidocumentcreatebutton',
            containerSelector: 'decisioneditpanel',
            typeDocument: 15, // Тип документа Распоряжение
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId === 'DisposalToActCheckByRoRule' || params.ruleId === 'DisposalToActSurveyRule') {
                    return false;
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'decisionPrintAspect',
            buttonSelector: '#decisionEditPanel #btnPrint',
            codeForm: 'Decision',
            getUserParams: function (reportId) {

                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'decisionBlobTextAspect',
            fieldSelector: '#taAdditionalInfo',
            editPanelAspectName: 'decisionEditPanelAspect',
            controllerName: 'DecisionGji',
            valueFieldName: 'Description',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: 'Description',
            getParentPanel: function () {
                return this.componentQuery('#decisionEditPanel');
            },
            getParentRecordId: function () {
                return this.controller.params.documentId;
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'decisionStateButtonAspect',
            stateButtonSelector: '#decisionEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('decisionEditPanelAspect');

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
            name: 'decisionEditPanelAspect',
            editPanelSelector: '#decisionEditPanel',
            modelName: 'DecisionGji',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #sfIssuredDisposal'] = { 'beforeload': { fn: this.onBeforeLoadInspectorManager, scope: this } };
                actions[this.editPanelSelector + ' #cbTypeAgreementResult'] = { 'change': { fn: this.onChangeTypeAgreementResult, scope: this } };
                actions[this.editPanelSelector + ' button[action=ERKNMRequest]'] = { 'click': { fn: this.ERKNMRequest, scope: this } };
            },

            ERKNMRequest: function (btn) {
                var me = this,
                    panel = btn.up('#decisionEditPanel'),
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
                                typeDoc: 10
                            }
                        }).next(function () {
                            B4.QuickMsg.msg('СМЭВ', 'Запрос на  размещение проверки в ЕРКНМ отправлен', 'success');
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

            onChangeTypeAgreementResult: function (field, newValue) {
                debugger;
                var form = field.up('#decisionEditPanel');
                var approveContainer = form.down('#approveContainer');    
                var approveresContainer = form.down('#approveresContainer');    

                if (newValue == B4.enums.TypeAgreementResult.Agreed ||
                    newValue == B4.enums.TypeAgreementResult.NotAgreed) {
                    approveContainer.show();
                    approveresContainer.show();
                }
                else {               
                    approveContainer.hide();
                    approveresContainer.hide();
                }
            },

            saveRecord: function (rec) {
                var me = this;

                debugger;
                //Ext.Msg.confirm('Внимание!', 'Убедитесь, что вид проверки указан правильно', function (result) {
                //    if (result === 'yes') {
                //        if (me.fireEvent('beforesave', me, rec) !== false) {
                //            if (me.hasUpload()) {
                //                me.saveRecordHasUpload(rec);
                //            } else {
                //                me.saveRecordHasNotUpload(rec);
                //            }
                //        }
                //    }
                //});
                if (me.fireEvent('beforesave', me, rec) !== false) {
                    if (me.hasUpload() && me.isFileLoad()) {
                        me.saveRecordHasUpload(rec);
                    } else {
                        me.saveRecordHasNotUpload(rec);
                    }
                }
            },
            onBeforeLoadInspectorManager: function (field, options, store) {
                options = options || {};
                options.params = options.params || {};

                options.params.headOnly = true;

                return true;
            },
            disableButtons: function (value) {
                debugger;
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
                    panel.setTitle("Решение" + " " + rec.get('DocumentNumber'));
                else
                    panel.setTitle("Решение");

                panel.down('#decisionTabPanel').setActiveTab(0);

                //получаем вид проверки
                if (asp.controller.params) {
                    asp.controller.params.kindCheckId = panel.down('#cbTypeCheck').getValue();
                }

                //Делаем запросы на получение Инспекторов
                //и обновляем соответсвующие Тригер филды
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('GetInfo', 'DecisionGji', { documentId: asp.controller.params.documentId }),
                    //для IE, чтобы не кэшировал GET запрос
                    cache: false
                }).next(function (response) {
                    asp.controller.unmask();
                    debugger;
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldInspectors = panel.down('#trigFInspectors');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    panel.down('#tfBaseName').setValue(obj.baseName);
                    panel.down('#tfPlanName').setValue(obj.planName);

                    debugger;
                    asp.disableButtons(false);
                }).error(function () {
                    asp.controller.unmask();
                });

                me.controller.getStore('decision.Expert').load();
                me.controller.getStore('decision.ControlList').load();
                me.controller.getStore('decision.ControlSubjects').load();
                me.controller.getStore('decision.InspectionReason').load();
                me.controller.getStore('decision.Annex').load();
                me.controller.getStore('decision.ControlMeasures').load();
                me.controller.getStore('decision.AdminRegulation').load();
                me.controller.getStore('decision.ProvidedDoc').load();
                me.controller.getStore('decision.VerificationSubject').load();

                // Значение по умолчанию "Внеплановая документарная" для поля "Вид проверки", если не задано
                var cbTypeCheck = panel.down('#cbTypeCheck');
                if (cbTypeCheck.getValue() == null) {
                    panel.down('#cbTypeCheck').setValue(B4.enums.TypeCheck.NotPlannedDocumentation);
                }

                // Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('decisionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем отчеты
                me.controller.getAspect('decisionPrintAspect').loadReportStore();

                // обновляем кнопку Сформирвоать
                me.controller.getAspect('decisionCreateButtonAspect').setData(rec.get('Id'));

                me.controller.getAspect('decisionBlobTextAspect').doInjection();
                var taDescription = panel.down('#taAdditionalInfo');
                taDescription.setValue(rec.get('Description'));
            },
            onSaveSuccess: function (asp, rec) {
                this.getPanel().setTitle(asp.controller.params.title + " " + rec.get('DocumentNumber'));

                if (asp.controller.params)
                    asp.controller.params.kindCheckId = asp.getPanel().down('#cbTypeCheck').getValue();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'decisioncontrolsubjectGridAspect',
            gridSelector: 'decisioncontrolsubjectgrid',
            editFormSelector: '#decisionControlSubjectWindow',
            storeName: 'decision.ControlSubjects',
            modelName: 'decision.ControlSubjects',
            editWindowView: 'decision.ControlSubjectWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Decision', this.controller.params.documentId);
                    }
                }
            },
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onChangePerson, scope: this } };
            },
            onChangePerson: function (field, newValue, oldValue) {
                var form = this.getForm(),
                    sfContragent = form.down('#sfContragent'),
                    tfPhysicalPerson = form.down('#tfPhysicalPerson'),
                    tfPhysicalPersonINN = form.down('#tfPhysicalPersonINN'),
                    tfPhysicalPersonPosition = form.down('#tfPhysicalPersonPosition');
                sfContragent.setValue(null);
                tfPhysicalPerson.setValue(null);
                tfPhysicalPersonPosition.setValue(null);
                switch (newValue) {
                    case 10://физлицо
                        sfContragent.setDisabled(true);
                        tfPhysicalPerson.setDisabled(false);
                        tfPhysicalPersonINN.setDisabled(false);
                        tfPhysicalPersonPosition.setDisabled(true);
                        break;
                    case 20://организацияы
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(true);
                        tfPhysicalPersonINN.setDisabled(true);
                        tfPhysicalPersonPosition.setDisabled(true);
                        break;
                    case 30://должностное лицо
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(false);
                        tfPhysicalPersonINN.setDisabled(false);
                        tfPhysicalPersonPosition.setDisabled(false);
                        break;
                }
            },
        },
        //{
        //    /* 
        //    Аспект взаимодействия кнопки создания Акт на 1 дом и массовой формы выборка домов
        //    По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
        //    у главного аспекта вызывается метод создания документа createActCheck1House и передаются выбранные Id домов
        //    */
        //    xtype: 'gkhbuttonmultiselectwindowaspect',
        //    name: 'disposalToActCheck1HouseGJIAspect',
        //    buttonSelector: '#disposalEditPanel [ruleId=DisposalToActCheckByRoRule]',
        //    multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        //    multiSelectWindowSelector: '#disposalToActCheckByRoRuleSelectWindow',
        //    storeSelectSelector: '#realityobjForSelectStore',
        //    storeSelect: 'RealityObjectGjiForSelect',
        //    storeSelected: 'RealityObjectGjiForSelected',
        //    selModelMode: 'SINGLE',
        //    columnsGridSelect: [
        //        {
        //            header: 'Муниципальное образование',
        //            xtype: 'gridcolumn',
        //            dataIndex: 'MunicipalityName',
        //            flex: 1,
        //            filter: {
        //                xtype: 'b4combobox',
        //                operand: CondExpr.operands.eq,
        //                storeAutoLoad: false,
        //                hideLabel: true,
        //                editable: false,
        //                valueField: 'Name',
        //                emptyItem: { Name: '-' },
        //                url: '/Municipality/ListWithoutPaging'
        //            }
        //        },
        //        { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
        //    ],
        //    columnsGridSelected: [
        //        { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
        //    ],
        //    titleSelectWindow: 'Выбор дома',
        //    titleGridSelect: 'Дома для отбора',
        //    titleGridSelected: 'Выбранный дом',

        //    onBeforeLoad: function (store, operation) {

        //        //спрятать панель выбранных домов
        //        var wnd = Ext.ComponentQuery.query(this.multiSelectWindowSelector + ' #multiSelectedPanel')[0];
        //        if (wnd) {
        //            wnd.hide();
        //        }

        //        if (this.controller.params && this.controller.params.inspectionId > 0)
        //            operation.params.inspectionId = this.controller.params.inspectionId;
        //    },

        //    onRowSelect: function (rowModel, record, index, opt) {
        //        //Поскольку наша форма множественного выборка должна возвращать только 1 значение
        //        //То Перекрываем метод select
        //        var grid = this.getSelectedGrid();
        //        if (grid) {
        //            var storeSelected = grid.getStore();
        //            storeSelected.removeAll();
        //            storeSelected.add(record);
        //        }
        //    },

        //    listeners: {
        //        getdata: function (asp, records) {
        //            var me = this,
        //                recordIds = [],
        //                btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
        //                creationAspect,
        //                params;

        //            records.each(function (rec, index) { recordIds.push(rec.get('RealityObjectId')); });

        //            if (recordIds[0] > 0) {
        //                creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
        //                // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
        //                params = creationAspect.getParams(btn);
        //                params.realityIds = recordIds;

        //                creationAspect.createDocument(params);
        //            } else {
        //                Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дом');
        //                return false;
        //            }
        //            return true;
        //        }
        //    }
        //},
        //{
        //    /* 
        //    Аспект взаимодействия кнопки создания Акт на обследование и массовой формы выборка домов
        //    По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
        //    у главного аспекта вызывается метод создания документа createActSurvey и передаются выбранные Id домов

        //    метод onRowSelect перекрываем потомучто необходимо изменить поведение выделения строки, чтобы можно было
        //    Выбрать только одну запись а не множество
        //    */
        //    xtype: 'gkhbuttonmultiselectwindowaspect',
        //    name: 'disposalToActSurveyAspect',
        //    buttonSelector: '#disposalEditPanel [ruleId=DisposalToActSurveyRule]',
        //    multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        //    multiSelectWindowSelector: '#disposalToActSurveySelectWindow',
        //    storeSelectSelector: '#realityobjForSelectStore',
        //    storeSelect: 'RealityObjectGjiForSelect',
        //    storeSelected: 'RealityObjectGjiForSelected',
        //    columnsGridSelect: [
        //        {
        //            header: 'Муниципальное образование',
        //            xtype: 'gridcolumn',
        //            dataIndex: 'MunicipalityName',
        //            flex: 1,
        //            filter: {
        //                xtype: 'b4combobox',
        //                operand: CondExpr.operands.eq,
        //                storeAutoLoad: false,
        //                hideLabel: true,
        //                editable: false,
        //                valueField: 'Name',
        //                emptyItem: { Name: '-' },
        //                url: '/Municipality/ListWithoutPaging'
        //            }
        //        },
        //        { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
        //    ],
        //    columnsGridSelected: [
        //        { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
        //    ],
        //    titleSelectWindow: 'Выбор дома',
        //    titleGridSelect: 'Дома для отбора',
        //    titleGridSelected: 'Выбранный дом',

        //    onBeforeLoad: function (store, operation) {
        //        //получаем текущую запись документа Распоряжения
        //        var record = this.controller.getAspect('disposalEditPanelAspect').getRecord();

        //        if (this.controller.params && this.controller.params.inspectionId > 0)
        //            operation.params.inspectionId = this.controller.params.inspectionId;

        //        if (record.get('TypeDisposal') == 20) {
        //            //Если акт обследования делается из Распоряжения на проверку Предписания то
        //            //получаем все дома из предписаний по которым создано это распоряжение
        //            operation.params.documentId = record.get('Id');
        //        }
        //    },

        //    onRowSelect: function (rowModel, record, index, opt) {
        //        //Поскольку наша форма множественного выборка должна возвращать только 1 значение
        //        //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
        //        //куда хотим добавить запись
        //        var grid = this.getSelectedGrid();
        //        if (grid) {
        //            var storeSelected = grid.getStore();
        //            storeSelected.removeAll();
        //            storeSelected.add(record);
        //        }
        //    },

        //    listeners: {
        //        getdata: function (asp, records) {
        //            var me = this,
        //                recordIds = [],
        //                btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
        //                creationAspect,
        //                params;

        //            records.each(function (rec, index) { recordIds.push(rec.get('RealityObjectId')); });

        //            if (recordIds[0] > 0) {
        //                creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
        //                // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
        //                params = creationAspect.getParams(btn);
        //                params.realityIds = recordIds;

        //                creationAspect.createDocument(params);

        //            } else {
        //                Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
        //                return false;
        //            }
        //            return true;
        //        }
        //    }
        //},
        {
            /*
           Переводим грид на мультиселект с инлайн редактированием
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'decisionControlMeasuresAspect',
            modelName: 'decision.ControlMeasures',
            storeName: 'decision.ControlMeasures',
            gridSelector: 'decisioncontrolmeasuresgrid',
            saveButtonSelector: 'decisioncontrolmeasuresgrid #decisionMeasuresGridSaveButton',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisionControlMeasuresSelectWindow',
            storeSelect: 'dict.ControlActivityGji',
            storeSelected: 'dict.ControlActivityGji',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор мероприятий по контролю',
            titleGridSelect: 'Мероприятия по контролю для отбора',
            titleGridSelected: 'Выбранные мероприятия по контролю',

            onBeforeLoad: function (store, operation) {
                var me = this;
                if (me.controller.params) {
                    operation.params.documentId = me.controller.params.kindCheckId;
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddDisposalControlMeasures', 'DecisionGji', {
                        controlMeasuresIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Мероприятия по контролю сохранены успешно');
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
           Переводим грид на мультиселект с инлайн редактированием
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'decisionInspectionReasonAspect',
            modelName: 'decision.InspectionReason',
            storeName: 'decision.InspectionReason',
            gridSelector: 'decisioninspectionreasongrid',
            saveButtonSelector: 'decisioninspectionreasongrid #decisioninspectionreasongridSaveButton',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisioninspectionreasonSelectWindow',
            storeSelect: 'dict.InspectionReasonForSelect',
            storeSelected: 'dict.InspectionReasonForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор причин',
            titleGridSelect: 'Причины вынесения решения для отбора',
            titleGridSelected: 'Выбранные записи',

            onBeforeLoad: function (store, operation) {
                var me = this;
                if (me.controller.params) {
                    operation.params.documentId = me.controller.params.kindCheckId;
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectionReasons', 'DecisionGji', {
                        inspreasonIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Причины вынесения решения сохранены успешно');
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
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            По нажатию на Добавить открывается форма выбора предоставляемых документов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение предоставляемых документов
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'decisionProvidedDocumentAspect',
            gridSelector: 'decisionprovideddocgrid',
            storeName: 'decision.ProvidedDoc',
            modelName: 'decision.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisionProvidedDocMultiSelectWindow',
            saveButtonSelector: 'decisionprovideddocgrid #decisionprovDocGridSaveButton',
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
                    renderer: function (value, metaData) {
                        metaData.style = "white-space: normal;";
                        return value;
                    }
                },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 0.5, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    sortable: false,
                    renderer: function (value, metaData) {
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
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'DecisionGji', {
                            providedDocIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
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
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /Disposal/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'decisionInspectorMultiSelectWindowAspect',
            fieldSelector: '#decisionEditPanel #trigFInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisionInspectorSelectWindow',
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
            Аспект взаимодействия таблицы экспертов с массовой формой выбора экспертов
            По нажатию на Добавить открывается форма выбора экспертов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение экспертов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'decisionExpertAspect',
            gridSelector: '#decisionExpertGrid',
            storeName: 'decision.Expert',
            modelName: 'decision.Expert',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisionExpertMultiSelectWindow',
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
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddExperts', 'DecisionGji', {
                            expertIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
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
            Аспект взаимодействия таблицы экспертов с массовой формой выбора экспертов
            По нажатию на Добавить открывается форма выбора экспертов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение экспертов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'decisionControlListAspect',
            gridSelector: 'decisioncontrollistgrid',
            storeName: 'decision.ControlList',
            modelName: 'decision.ControlList',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisionControlListMultiSelectWindow',
            storeSelect: 'dict.ControlListForSelect',
            storeSelected: 'dict.ControlListForSelected',
            titleSelectWindow: 'Выбор проверочных листов',
            titleGridSelect: 'Проверочные листы для отбора',
            titleGridSelected: 'Выбранные проверочные листы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddControlLists', 'DecisionGji', {
                            contrlistIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
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
             * Аспект взаимодействия Таблицы Приложений с формой редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'decisionAnnexAspect',
            gridSelector: '#decisionAnnexGrid',
            editFormSelector: '#decisionAnnexEditWindow',
            storeName: 'decision.Annex',
            modelName: 'decision.Annex',
            editWindowView: 'decision.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Decision', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'decisionSurveySubjectAspect',
            modelName: 'decision.VerificationSubject',
            storeName: 'decision.VerificationSubject',
            gridSelector: 'decisionverificationgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisionSurveySubjectSelectWindow',
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
                    B4.Ajax.request(B4.Url.action('AddSurveySubjects', 'DecisionGji', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();

                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.getStore('decision.VerificationSubject').load();
                      

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
            name: 'decisionAdminRegulationAspect',
            modelName: 'decision.AdminRegulation',
            storeName: 'decision.AdminRegulation',
            gridSelector: 'decisionadminregulationgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisionAdminRegulationSelectWindow',
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
                    B4.Ajax.request(B4.Url.action('AddAdminRegulations', 'DecisionGji', {
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
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'decisionVerificationSubjectNormativeDocItemAspect',
            gridSelector: 'decisionnormdocitemgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#decisionVerificationSubjectNormativeDocItemSelectWindow',
            storeSelect: 'dict.NormDocItemForSelect',
            storeSelected: 'dict.NormDocItemForSelected',
            columnsGridSelect: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст', xtype: 'gridcolumn', dataIndex: 'Text', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Документ', xtype: 'gridcolumn', dataIndex: 'NormativeDocName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор требований НПА проверки',
            titleGridSelect: 'Требования НПА проверки для отбора',
            titleGridSelected: 'Выбранные требования НПА проверки',
            onBeforeLoad: function (store, operation) {
                operation.params.docId = this.controller.params.normDocId
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainView());
                    B4.Ajax.request(B4.Url.action('AddNormDocItems', 'DecisionGji', {
                        ids: Ext.encode(recordIds),
                        subjId: asp.controller.params.subjId
                    })).next(function (response) {
                        asp.updateGrid();
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Требования НПА проверки сохранены успешно');

                        return true;
                    }).error(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', response.message);
                    });

                    return true;
                }
            }
        },
    ],

    init: function () {
        var me = this,
            actions = {};
        actions['decisionverificationgrid'] = {
            'select': { fn: me.verificationSelect, scope: me },
            'deselect': { fn: me.verificationDeselect, scope: me }
        };
        actions['decisionnormdocitemgrid'] = {
            'store.beforeload': { fn: me.beforeNormDocItemLoad, scope: me }};
 
        me.getStore('decision.Expert').on('beforeload', me.onBeforeLoad, me);
        me.getStore('decision.ControlList').on('beforeload', me.onBeforeLoad, me);
        me.getStore('decision.ControlSubjects').on('beforeload', me.onBeforeLoad, me);
        me.getStore('decision.InspectionReason').on('beforeload', me.onBeforeLoad, me);
        me.getStore('decision.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('decision.VerificationSubject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('decision.AdminRegulation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('decision.ControlMeasures').on('beforeload', me.onBeforeLoad, me);
        me.getStore('decision.ProvidedDoc').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);

        me.control(actions);
    },

    beforeNormDocItemLoad: function (store, operation) {
        if (this.params && this.params.subjId)
        operation.params.subjId = this.params.subjId;
    },


    verificationSelect: function (selModel, record, index, eventOpts) {
        var me = this,
            panel = selModel.view.up('decisionverifsubjpanel'),
            itemGrid = panel.down('decisionnormdocitemgrid'),
            params = me.params || (me.params = {});
        params.subjId = record.get('Id');

        itemGrid.enable();
        itemGrid.getStore().load();
    },

    verificationDeselect: function (selModel, record, index, eventOpts) {
        var panel = selModel.view.up('decisionverifsubjpanel'),
            itemGrid = panel.down('decisionnormdocitemgrid');

        itemGrid.getStore().removeAll();
        itemGrid.disable();
    },

    onLaunch: function () {
        var me = this;
        debugger;
        if (me.params) {
            me.getAspect('decisionEditPanelAspect').setData(me.params.documentId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId)
            operation.params.documentId = this.params.documentId;
    }
});