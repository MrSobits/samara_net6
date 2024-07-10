Ext.define('B4.controller.Disposal', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
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
        'B4.DisposalTextValues',
        'B4.aspects.GkhBlobText',
        'B4.form.SelectWindow',
        'B4.QuickMsg',
        'B4.enums.TypeCheck'
    ],

    models: [
        'Disposal',
        'ActCheck',
        'ActSurvey',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.DisposalAdditionalDoc',
        'disposal.Annex',
        'disposal.DisposalVerificationSubject',
        'disposal.TypeSurvey',
        'disposal.DisposalDocConfirm',
        'disposal.AdminRegulation',
        'disposal.SurveyPurpose',
        'disposal.InspFoundation',
        'disposal.InspFoundationCheck',
        'disposal.SurveyObjective',
        'disposal.DisposalControlMeasures',
        'disposal.InspFoundCheckNormDocItem'
    ],

    stores: [
        'Disposal',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.TypeSurvey',
        'RealityObjectGjiForSelect',
        'RealityObjectGjiForSelected',
        'dict.TypeSurveyGjiForSelect',
        'disposal.DisposalAdditionalDoc',
        'dict.TypeSurveyGjiForSelected',
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ExpertGjiForSelect',
        'dict.ExpertGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected',
        'disposal.DisposalVerificationSubject',
        'disposal.DisposalDocConfirm',
        'dict.Municipality',
        'dict.TypeFactViolation',
        'dict.TypeFactViolationSelected',
        'disposal.AdminRegulation',
        'disposal.SurveyPurpose',
        'disposal.InspFoundation',
        'disposal.InspFoundationCheck',
        'disposal.SurveyObjective',
        'dict.SurveyPurposeForSelect',
        'dict.SurveyPurposeForSelected',
        'dict.SurveyObjectiveForSelect',
        'dict.SurveyObjectiveForSelected',
        'dict.NormativeDocForSelect',
        'dict.NormativeDocForSelected',
        'dict.SurveySubjectForSelect',
        'dict.SurveySubjectForSelected',
        'disposal.DisposalControlMeasures',
        'disposal.InspFoundCheckNormDocItem'
    ],

    views: [
        'disposal.EditPanel',
        'disposal.AnnexEditWindow',
        'disposal.AnnexGrid',
        'disposal.ExpertGrid',
        'disposal.TypeSurveyGrid',
        'disposal.ProvidedDocGrid',
        'disposal.SubjectVerificationGrid',
        'disposal.DocConfirmGrid',
        'disposal.DisposalAdditionalDocGrid',
        'disposal.AdminRegulationGrid',
        'disposal.SurveyPurposeGrid',
        'disposal.InspFoundationGrid',
        'disposal.InspFoundationCheckGrid',
        'disposal.NormDocItemGrid',
       'disposal.SurveyObjectiveGrid',
        'disposal.DisposalControlMeasuresGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'disposal.EditPanel',
    mainViewSelector: '#disposalEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#disposalAnnexGrid',
            controllerName: 'DisposalAnnex',
            name: 'disposalAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            Аспект формирвоания документов для Распоряжения
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'disposalCreateButtonAspect',
            buttonSelector: 'disposaleditpanel gjidocumentcreatebutton',
            containerSelector: 'disposaleditpanel',
            typeDocument: 10, // Тип документа Распоряжение
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId === 'DisposalToActCheckByRoRule' || params.ruleId === 'DisposalToActSurveyRule') {
                    return false;
                }
            }
        },
        {
            xtype: 'disposalperm',
            editFormAspectName: 'disposalEditPanelAspect',
            afterSetRequirements: function (rec) {
                var requisitePanel = this.controller.getMainView().down('form[name=Requisite]');
                if (requisitePanel) {
                    requisitePanel.getForm().isValid();
                }
            }
        },
        {
            xtype: 'nsodisposalperm',
            editFormAspectName: 'disposalEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'disposalPrintAspect',
            buttonSelector: '#disposalEditPanel #btnPrint',
            codeForm: 'Disposal',
            getUserParams: function (reportId) {

                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'disposalStateButtonAspect',
            stateButtonSelector: '#disposalEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('disposalEditPanelAspect');

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
            name: 'disposalEditPanelAspect',
            editPanelSelector: '#disposalEditPanel',
            modelName: 'Disposal',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #sfIssuredDisposal'] = { 'beforeload': { fn: this.onBeforeLoadInspectorManager, scope: this } };
                actions[this.editPanelSelector + ' #cbTypeAgreementResult'] = { 'change': { fn: this.onChangeTypeAgreementResult, scope: this } };
            },

            onChangeTypeAgreementResult: function (field, newValue) {
                var form = field.up('#disposalEditPanel');
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
                var me = this,
                    factViolField = panel.down('[name=FactViols]'),
                    itemGrid = panel.down('normdocitemgrid');

                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber'))
                    panel.setTitle(B4.DisposalTextValues.getSubjectiveCase() + " " + rec.get('DocumentNumber'));
                else
                    panel.setTitle(B4.DisposalTextValues.getSubjectiveCase());

                panel.down('#disposalTabPanel').setActiveTab(0);

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
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldInspectors = panel.down('#trigFInspectors');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    panel.down('#tfBaseName').setValue(obj.baseName);
                    panel.down('#tfPlanName').setValue(obj.planName);


                    if (!factViolField.isHidden() && obj.baseName != 'Обращение граждан') {
                        factViolField.hide();
                    }


                    asp.disableButtons(false);
                }).error(function () {
                    asp.controller.unmask();
                });

                factViolField.updateDisplayedText(rec.get('FactViols'));
                factViolField.setValue(rec.get('FactViolIds'));
                me.controller.getStore('disposal.Expert').load();
                me.controller.getStore('disposal.ProvidedDoc').load();
                me.controller.getStore('disposal.Annex').load();
                me.controller.getStore('disposal.TypeSurvey').load();
                me.controller.getStore('disposal.DisposalVerificationSubject').load();
                me.controller.getStore('disposal.DisposalDocConfirm').load();
                me.controller.getStore('disposal.DisposalControlMeasures').load();
                me.controller.getStore('disposal.SurveyPurpose').load();
                me.controller.getStore('disposal.SurveyObjective').load();
                me.controller.getStore('disposal.InspFoundation').load();
                me.controller.getStore('disposal.InspFoundationCheck').load();
                me.controller.getStore('disposal.AdminRegulation').load();
                me.controller.getAspect('disposalNoticeDescriptionAspect').doInjection();

                itemGrid.getStore().load();

                // Значение по умолчанию "Внеплановая документарная" для поля "Вид проверки", если не задано
                var cbTypeCheck = panel.down('#cbTypeCheck');
                if (cbTypeCheck.getValue() == null) {
                    panel.down('#cbTypeCheck').setValue(B4.enums.TypeCheck.NotPlannedDocumentation);
                }

                // Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('disposalStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем отчеты
                me.controller.getAspect('disposalPrintAspect').loadReportStore();

                // обновляем кнопку Сформирвоать
                me.controller.getAspect('disposalCreateButtonAspect').setData(rec.get('Id'));
            },
            onSaveSuccess: function (asp, rec) {
                this.getPanel().setTitle(asp.controller.params.title + " " + rec.get('DocumentNumber'));

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
            name: 'disposalToActCheck1HouseGJIAspect',
            buttonSelector: '#disposalEditPanel [ruleId=DisposalToActCheckByRoRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalToActCheckByRoRuleSelectWindow',
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
                        creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
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
            name: 'disposalToActSurveyAspect',
            buttonSelector: '#disposalEditPanel [ruleId=DisposalToActSurveyRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalToActSurveySelectWindow',
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

            onBeforeLoad: function (store, operation) {
                //получаем текущую запись документа Распоряжения
                var record = this.controller.getAspect('disposalEditPanelAspect').getRecord();

                if (this.controller.params && this.controller.params.inspectionId > 0)
                    operation.params.inspectionId = this.controller.params.inspectionId;

                if (record.get('TypeDisposal') == 20) {
                    //Если акт обследования делается из Распоряжения на проверку Предписания то
                    //получаем все дома из предписаний по которым создано это распоряжение
                    operation.params.documentId = record.get('Id');
                }
            },

            onRowSelect: function (rowModel, record, index, opt) {
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
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec, index) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
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
           Переводим грид на мультиселект с инлайн редактированием
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'disposalControlMeasuresAspect',
            modelName: 'disposal.DisposalControlMeasures',
            storeName: 'disposal.DisposalControlMeasures',
            gridSelector: 'disposalcontrolmeasuresgrid',
            saveButtonSelector: 'disposalcontrolmeasuresgrid #disposalMeasuresGridSaveButton',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalControlMeasuresSelectWindow',
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
                    B4.Ajax.request(B4.Url.action('AddDisposalControlMeasures', 'DisposalControlMeasures', {
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
        //{
        //    /*
        //    аспект взаимодействия грида мероприятий по контролю с массовой формой выбора мероприятий по контролю
        //    по нажатию на кнопку добавления показывается форма массового выбора после чего идет отбор
        //    По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
        //    и сохраняем Типы обслуживания через серверный метод /DisposalTypeSurvey/AddTypeSurveys
        //    */
        //    xtype: 'gkhgridmultiselectwindowaspect',
        //    name: 'disposalControlMeasuresAspect',
        //    modelName: 'disposal.DisposalControlMeasures',
        //    storeName: 'disposal.DisposalControlMeasures',
        //    gridSelector: 'disposalcontrolmeasuresgrid',
        //    multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        //    multiSelectWindowSelector: '#disposalControlMeasuresSelectWindow',
        //    storeSelect: 'dict.ControlActivityGji',
        //    storeSelected: 'dict.ControlActivityGji',
        //    columnsGridSelect: [
        //        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
        //        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
        //    ],
        //    columnsGridSelected: [
        //        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
        //    ],
        //    titleSelectWindow: 'Выбор мероприятий по контролю',
        //    titleGridSelect: 'Мероприятия по контролю для отбора',
        //    titleGridSelected: 'Выбранные мероприятия по контролю',

        //    onBeforeLoad: function (store, operation) {
        //        var me = this;
        //        if (me.controller.params) {
        //            operation.params.documentId = me.controller.params.kindCheckId;
        //        }
        //    },

        //    listeners: {
        //        getdata: function (asp, records) {
        //            var recordIds = [];

        //            Ext.each(records.items, function (rec) {
        //                recordIds.push(rec.get('Id'));
        //            });

        //            asp.controller.mask('Сохранение', asp.controller.getMainComponent());
        //            B4.Ajax.request(B4.Url.action('AddDisposalControlMeasures', 'DisposalControlMeasures', {
        //                controlMeasuresIds: recordIds,
        //                documentId: asp.controller.params.documentId
        //            })).next(function (response) {
        //                asp.controller.unmask();
        //                asp.controller.getStore(asp.storeName).load();
        //                Ext.Msg.alert('Сохранение!', 'Мероприятия по контролю сохранены успешно');
        //                return true;
        //            }).error(function () {
        //                asp.controller.unmask();
        //            });

        //            return true;
        //        }
        //    }
        //},
        {
            /*
            аспект взаимодействия грида типов обследований с массовой формой выбора типов обслуживания
            по нажатию на кнопку добавления показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем Типы обслуживания через серверный метод /DisposalTypeSurvey/AddTypeSurveys
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalTypeSurveyAspect',
            modelName: 'disposal.TypeSurvey',
            storeName: 'disposal.TypeSurvey',
            gridSelector: '#disposalTypeSurveyGrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalTypeSurveySelectWindow',
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

            onBeforeLoad: function (store, operation) {
                if (this.controller.params) {
                    operation.params.kindCheckId = this.controller.params.kindCheckId;
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddTypeSurveys', 'DisposalTypeSurvey', {
                        typeIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Типы обследований сохранены успешно');
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
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /Disposal/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'disposalInspectorMultiSelectWindowAspect',
            fieldSelector: '#disposalEditPanel #trigFInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalInspectorSelectWindow',
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
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора фактов нарушения
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем факты нарушения через серверный метод /DisposalFactViolation/AddFactViolation
            */

            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'disposalFactViolationMultiSelectWindowAspect',
            fieldSelector: '#disposalEditPanel gkhtriggerfield[name=FactViols]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalFactViolationSelectWindow',
            storeSelect: 'dict.TypeFactViolation',
            storeSelected: 'dict.TypeFactViolationSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор фактов нарушения',
            titleGridSelect: 'Факты нарушения для отбора',
            titleGridSelected: 'Выбранные факты нарушения',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddFactViolation', 'ChelyabinskDisposal', {
                        factViolIds: recordIds,
                        disposalId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Факты нарушения сохранены успешно');
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
            name: 'disposalExpertAspect',
            gridSelector: '#disposalExpertGrid',
            storeName: 'disposal.Expert',
            modelName: 'disposal.Expert',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalExpertMultiSelectWindow',
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
                        B4.Ajax.request(B4.Url.action('AddExperts', 'DisposalExpert', {
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
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            По нажатию на Добавить открывается форма выбора предоставляемых документов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение предоставляемых документов
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'disposalProvidedDocumentAspect',
            gridSelector: '#disposalProvidedDocGrid',
            storeName: 'disposal.ProvidedDoc',
            modelName: 'disposal.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalProvidedDocMultiSelectWindow',
            saveButtonSelector: 'disposalprovideddocgrid #disposalprovDocGridSaveButton',
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
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'DisposalProvidedDoc', {
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
             * Аспект взаимодействия Таблицы Приложений с формой редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'disposalAnnexAspect',
            gridSelector: '#disposalAnnexGrid',
            editFormSelector: '#disposalAnnexEditWindow',
            storeName: 'disposal.Annex',
            modelName: 'disposal.Annex',
            editWindowView: 'disposal.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Disposal', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            Аспект взаимодействия таблицы Документы на согласование, как инлайн грид
            */
            xtype: 'gkhinlinegridaspect',
            name: 'disposalDocConfirmAspect',
            storeName: 'disposal.DisposalDocConfirm',
            modelName: 'disposal.DisposalDocConfirm',
            gridSelector: 'disposaldocconfirm',
            saveButtonSelector: 'disposaldocconfirm button[name=btnSaveDocConfirm]',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (rec) {
                        //Для новых  записей присваиваем родительский документ
                        if (!rec.get('Id')) {
                            rec.set('Disposal', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия таблицы дополнительных документов, как инлайн грид
            */
            xtype: 'gkhinlinegridaspect',
            name: 'disposalAdditionalDocAspect',
            storeName: 'disposal.DisposalAdditionalDoc',
            modelName: 'disposal.DisposalAdditionalDoc',
            gridSelector: 'disposaladditionaldoc',
            saveButtonSelector: 'disposaladditionaldoc button[name=btnSaveAdditionalDoc]',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (rec) {
                        //Для новых  записей присваиваем родительский документ
                        if (!rec.get('Id')) {
                            rec.set('Disposal', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'disposalNoticeDescriptionAspect',
            fieldSelector: '[name=NoticeDescription]',
            editPanelAspectName: 'disposalEditPanelAspect',
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
            storeName: 'disposal.DisposalVerificationSubject',
            gridSelector: 'disposalsubjectverificationgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveySubjectSelectWindow',
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
                        asp.controller.getStore('disposal.TypeSurvey').load();
                        asp.controller.getStore('disposal.SurveyPurpose').load();
                        asp.controller.getStore('disposal.SurveyObjective').load();
                        asp.controller.getStore('disposal.InspFoundation').load();
                        asp.controller.getStore('disposal.InspFoundationCheck').load();
                        asp.controller.getStore('disposal.AdminRegulation').load();
                        asp.controller.getStore('disposal.ProvidedDoc').load();

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
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'disposalSurveyPurposeAspect',
            modelName: 'disposal.SurveyPurpose',
            storeName: 'disposal.SurveyPurpose',
            gridSelector: 'disposalsurveypurposegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveyPurposeSelectWindow',
            saveButtonSelector: 'disposalsurveypurposegrid #disposalSurveyPurposeGridSaveButton',
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
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'disposalSurveyObjectiveAspect',
            modelName: 'disposal.SurveyObjective',
            storeName: 'disposal.SurveyObjective',
            gridSelector: 'disposalsurveyobjectivegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveyObjectiveSelectWindow',
            saveButtonSelector: 'disposalsurveyobjectivegrid #disposalSurveyObjectiveGridSaveButton',
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
            name: 'disposalInspFoundationAspect',
            modelName: 'disposal.InspFoundation',
            storeName: 'disposal.InspFoundation',
            gridSelector: 'disposalinspfoundationgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalInspFoundationSelectWindow',
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
                    B4.Ajax.request(B4.Url.action('AddInspFoundations', 'Disposal', {
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
            name: 'disposalInspFoundationNormativeDocItemAspect',
            gridSelector: 'normdocitemgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalInspFoundNormDocItemSelectWindow',
            storeSelect: 'dict.NormDocItemForSelect',
            storeSelected: 'dict.NormDocItemForSelected',
            columnsGridSelect: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст', xtype: 'gridcolumn', dataIndex: 'Text', flex: 1, filter: { xtype: 'textfield' } }
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
                    B4.Ajax.request(B4.Url.action('AddNormDocItems', 'Disposal', {
                        ids: Ext.encode(recordIds),
						documentId: asp.controller.params.documentId,
						foundCheckId : asp.controller.params.foundCheckId
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
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalInspFoundationCheckAspect',
            modelName: 'disposal.InspFoundationCheck',
            storeName: 'disposal.InspFoundationCheck',
            gridSelector: 'disposalinspfoundationcheckgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalInspFoundationCheckSelectWindow',
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
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalAdminRegulationAspect',
            modelName: 'disposal.AdminRegulation',
            storeName: 'disposal.AdminRegulation',
            gridSelector: 'disposaladminregulationgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalAdminRegulationSelectWindow',
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
                    B4.Ajax.request(B4.Url.action('AddAdminRegulations', 'Disposal', {
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
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['disposalinspfoundationcheckgrid'] = {
            'select': { fn: me.foundationCheckSelect, scope: me },
            'deselect': { fn: me.foundationCheckDeselect, scope: me }
        };
        actions['normdocitemgrid'] = {
            'store.beforeload': { fn: me.beforeNormDocItemLoad, scope: me },
            'beforeshow': { fn: me.applyPermissions, scope: me }
        };

        me.getStore('disposal.Expert').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.ProvidedDoc').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.TypeSurvey').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.DisposalVerificationSubject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.DisposalDocConfirm').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.DisposalAdditionalDoc').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.SurveyObjective').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.SurveyPurpose').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.InspFoundation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.InspFoundationCheck').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.AdminRegulation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.DisposalControlMeasures').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);

        me.control(actions);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('disposalEditPanelAspect').setData(me.params.documentId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId)
            operation.params.documentId = this.params.documentId;
    },

    foundationCheckSelect: function (selModel, record, index, eventOpts) {
        var me = this,
            panel = selModel.view.up('disposalinspfoundationcheckpanel'),
            itemGrid = panel.down('normdocitemgrid'),
			params = me.params || (me.params = {});
        params.foundCheckId = record.get('Id');
        params.normDocId = record.get('NormDocId');

        itemGrid.enable();
        itemGrid.getStore().load();
    },

    foundationCheckDeselect: function (selModel, record, index, eventOpts) {
        var panel = selModel.view.up('disposalinspfoundationcheckpanel'),
            itemGrid = panel.down('normdocitemgrid');

        itemGrid.getStore().removeAll();
        itemGrid.disable();
    },

    beforeNormDocItemLoad: function (store, operation) {
        if (this.params && this.params.documentId)
			operation.params.documentId = this.params.documentId;
		operation.params.foundCheckId = this.params.foundCheckId;
    },

    applyPermissions: function (grid, eventOpts) {
        var panel = grid.up('disposalinspfoundationcheckpanel'),
            foundCheckGrid = panel.down('disposalinspfoundationcheckgrid'),
            addButton = foundCheckGrid.down('b4addbutton'),
            deleteColumn = foundCheckGrid.down('b4deletecolumn');

        grid.down('b4addbutton').setDisabled(addButton.disabled);
        grid.down('b4deletecolumn').setVisible(!deleteColumn.hidden);
    }
});