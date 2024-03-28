Ext.define('B4.controller.Disposal', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.form.ComboBox',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.Disposal',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.view.disposal.MultiSelectViolationsForBaseDisposal',
        'B4.DisposalTextValues',
        'B4.enums.TypeDisposalGji'
    ],

    models: [
        'Disposal',
        'ActCheck',
        'ActSurvey',
        'disposal.Violation',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.TypeSurvey',
        'disposal.DisposalVerificationSubjectLicensing',
        'requirement.Requirement',
        'requirement.ArticleLaw'
    ],

    stores: [
        'Disposal',
        'disposal.Violation',
        'disposal.RealityObjViolation',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.TypeSurvey',
        'RealityObjectGjiForSelect',
        'RealityObjectGjiForSelected',
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
        'B4.aspects.StateContextMenu',
        'disposal.DisposalVerificationSubjectLicensing',
        'disposal.DisposalRequirement',
        'requirement.Type',
        'requirement.ArticleLaw',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'prescription.ViolationForSelect',
        'prescription.ViolationForSelected',
        'disposal.ViolationForSelect',
        'disposal.ViolationForSelected'
    ],

    views: [
        'disposal.EditPanel',
        'disposal.RealityObjViolationGrid',
        'disposal.AnnexEditWindow',
        'disposal.AnnexGrid',
        'disposal.ExpertGrid',
        'disposal.TypeSurveyGrid',
        'disposal.ProvidedDocGrid',
        'disposal.SubjectVerificationLicensingGrid',
        'disposal.RequirementGrid',
        'disposal.RequirementEditWindow',
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
                if ( params.ruleId == 'TomskDisposalToPrescriptionByViolationRule') {
                    return false;
                }
            }
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'disposalRequirementStateTransferAspect',
            gridSelector: 'disprequirementgrid',
            menuSelector: 'disposalrequirementgridmenu',
            stateType: 'gji_requirement'
        },
        {
            xtype: 'disposalperm',
            editFormAspectName: 'disposalEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'disposalPrintAspect',
            buttonSelector: '#disposalEditPanel #btnPrint',
            codeForm: 'Disposal',
            getUserParams: function(reportId) {

                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования Распоряжения
            */
            xtype: 'statebuttonaspect',
            name: 'disposalStateButtonAspect',
            stateButtonSelector: '#disposalEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('disposalEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            /**
            * Вешаем аспект смены статуса на форме Требования
            */
            xtype: 'statebuttonaspect',
            name: 'disposalRequirementStateButtonAspect',
            stateButtonSelector: 'disposalrequirementeditwin #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var me = this,
                        model = this.controller.getModel('requirement.Requirement');
                       
                    model.load(entityId, {
                        success: function (rec) {
                            me.controller.getAspect('disposalRequirementAspect').setFormData(rec);
                        },
                        scope: this
                    });
                }
            }
        },
        {
            //Аспект панели распоряжения
            xtype: 'gjidocumentaspect',
            name: 'disposalEditPanelAspect',
            editPanelSelector: '#disposalEditPanel',
            modelName: 'Disposal',
            otherActions: function (actions) {
                var me = this;
                actions[this.editPanelSelector + ' #sfIssuredDisposal'] = { 'beforeload': { fn: this.onBeforeLoadInspectorManager, scope: this } };

                actions[this.editPanelSelector + ' [name=DateStart]'] = {
                    dirtychange: function (field, isDirty) {
                        me.setObjectVisit(field.up(me.editPanelSelector), isDirty);
                    },
                    change: function (field, newValue) {
                        if (field.isDirty()) {
                            var dateEndField = me.componentQuery(me.editPanelSelector + ' [name=DateEnd]');
                            dateEndField.disable();
                            B4.Ajax.request({
                                method: 'GET',
                                url: B4.Url.action('GetDateAfterWorkDays', 'Day'),
                                params: {
                                    date: newValue,
                                    workDaysCount: 20
                                }
                            }).next(function (response) {
                                if (response) {
                                    var newDate = Ext.JSON.decode(response.responseText);
                                    if (newDate) {
                                        dateEndField.enable();
                                        dateEndField.setValue(newDate);
                                    }
                                }
                            }).error(function () {
                                dateEndField.enable();
                            });
                        }
                    }
                };
                actions[this.editPanelSelector + ' [name=DateEnd]'] = {
                    dirtychange: function (field, isDirty) {
                        me.setObjectVisit(field.up(me.editPanelSelector), isDirty);
                    }
                };
                
                actions['#disposalRealityObjViolationGrid'] = { 'select': { fn: this.onSelectRealityObjViolationGrid, scope: this } };
            },
            
            onSelectRealityObjViolationGrid: function () {
                this.controller.getStore('disposal.Violation').load();
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
            onBeforeLoadInspectorManager: function(field, options) {
                options = options || {};
                options.params = options.params || {};

                options.params.headOnly = true;

                return true;
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

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function(asp, rec, panel) {
                var me = this;
                asp.controller.params = asp.controller.params || {};

                asp.controller.params.disposalType = rec.get('TypeDisposal');
                asp.controller.params.parentIds = rec.get('ParentIds');
                
                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                //После проставления данных обновляем title вкладки
                var title = "Приказ ГЖИ";
                panel.down('#disposalTabPanel').child('disposalsubjectverificationlicensinggrid').tab.hide();

                if (rec.get('TypeDisposal') == B4.enums.TypeDisposalGji.DocumentGji) {
                    title = "Приказ на предписание";
                }
                else if (rec.get('TypeDisposal') == B4.enums.TypeDisposalGji.Licensing) {
                    title = "Приказ лицензирование";
                    panel.down('#disposalTabPanel').child('disposalsubjectverificationlicensinggrid').tab.show();

                    asp.controller.getStore('disposal.DisposalVerificationSubjectLicensing').load();
                }

                if (rec.get('DocumentNumber'))
                    panel.setTitle(title + " " + rec.get('DocumentNumber'));
                else
                    panel.setTitle(title);

                panel.down('#disposalTabPanel').setActiveTab(0);

                if (rec.get('KindCheck')) {
                    panel.down('#ProvideDocumentsNum').setVisible(rec.get('KindCheck').Code != '4');
                }
                
                //получаем вид проверки
                if (asp.controller.params) {
                    var cbTypeCheck = panel.down('#cbTypeCheck');
                    asp.controller.params.kindCheckId = cbTypeCheck.getValue();
                    cbTypeCheck.store.reload({ params: { typeBase: rec.get('TypeBase') }});
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

                    asp.disableButtons(false);
                }).error(function() {
                    asp.controller.unmask();
                });

                me.setObjectVisit(panel, false);

                //Обновляем сторы
                me.controller.getStore('disposal.RealityObjViolation').load();
                me.controller.getStore('disposal.Expert').load();
                me.controller.getStore('disposal.ProvidedDoc').load();
                me.controller.getStore('disposal.Annex').load();
                me.controller.getStore('disposal.TypeSurvey').load();
                panel.down('disprequirementgrid').getStore().load();

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

                if (rec.get('KindCheck') && rec.get('KindCheck').Code != '4') {
                    rec.set('ProvideDocumentsNum', this.getPanel().down('#ProvideDocumentsNum').getValue());
                    this.saveProvidedDocNum(rec.getId());
                }
            },
            
            saveProvidedDocNum: function (dispId) {
                var panel = this.getPanel(),
                    provideDocumentsNum = panel.down('#ProvideDocumentsNum').getValue();
                B4.Ajax.request(B4.Url.action('AddProvideDocNum', 'DisposalProvidedDocNum', {
                    dispId: dispId,
                    provideDocumentsNum: provideDocumentsNum
                })).error(function() {
                    Ext.Msg.alert('Сохранение!', 'Возникла ошибка при сохранении значения "Предоставить документы в течение:"');
                });
            },
            setObjectVisit: function (panel, isDirty) {
                if (isDirty) {
                    var dfVisitStart = panel.down('[name=ObjectVisitStart]'),
                    dfVisitEnd = panel.down('[name=ObjectVisitEnd]');
                
                    dfVisitStart.setValue(panel.down('[name=DateStart]').getValue());
                    dfVisitEnd.setValue(panel.down('[name=DateEnd]').getValue());
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Предписания и массовой формы выбора Нарушений (для распоряжения на проверку предписания)
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'disposalToPrescriptionByViolationsAspect',
            buttonSelector: '#disposalEditPanel [ruleId=TomskDisposalToPrescriptionByViolationRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalToPrescriptionByViolationsSelectWindow',
            storeSelectSelector: '#disposalViolationsForSelect',
            storeSelect: 'disposal.ViolationForSelect',
            storeSelected: 'disposal.ViolationForSelected',
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор нарушения',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',

            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0)
                    operation.params.documentId = this.controller.params.documentId;
            },

            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec, index) { recordIds.push(rec.get('InspectionViolationId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
                        
                        params = creationAspect.getParams(btn);
                        params.violationIds = recordIds;

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
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'disposalProvidedDocumentAspect',
            gridSelector: '#disposalProvidedDocGrid',
            saveButtonSelector: '#disposalProvidedDocGrid #disposalProvidedDocSaveButton',
            storeName: 'disposal.ProvidedDoc',
            modelName: 'disposal.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalProvidedDocMultiSelectWindow',
            storeSelect: 'dict.ProvidedDocGjiForSelect',
            storeSelected: 'dict.ProvidedDocGjiForSelected',
            titleSelectWindow: 'Выбор предоставляемых документов',
            titleGridSelect: 'Документы для выбора',
            titleGridSelected: 'Выбранные документы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
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
            name: 'disposalAnnexAspect',
            gridSelector: '#disposalAnnexGrid',
            editFormSelector: '#disposalAnnexEditWindow',
            storeName: 'disposal.Annex',
            modelName: 'disposal.Annex',
            editWindowView: 'disposal.AnnexEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Disposal', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы требований с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'disposalRequirementAspect',
            gridSelector: 'disprequirementgrid',
            editFormSelector: 'disposalrequirementeditwin',
            modelName: 'requirement.Requirement',
            editWindowView: 'disposal.RequirementEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #btnCreateProtocol'] = {
                    click: {
                        fn: function() {
                            me.createProtocol();
                        }
                    }
                };
                actions['disposalrequirementeditwin [name=TypeRequirement]'] = { 'change': {  fn: this.changeRequirement, scope: this}};
            },
            onSaveSuccess: function (asp, record) {
                if (record && record.getId()) {
                    var model = this.getModel(record);

                    model.load(record.getId(), {
                        success: function (rec) {
                            asp.setFormData(rec);
                        },
                        scope: this
                    });
                }
            },
            changeRequirement: function (fld, newValue) {
                var win = fld.up('disposalrequirementeditwin'),
                    matSubDateFld = win.down('[name=MaterialSubmitDate]'),
                    addMaterialsFld = win.down('[name=AddMaterials]'),
                    inspectionDateFld = win.down('[name=InspectionDate]'),
                    inspectionHourFld = win.down('[name=InspectionHour]'),
                    inspectionMinuteFld = win.down('[name=InspectionMinute]');

                matSubDateFld.enable();
                addMaterialsFld.enable();
                inspectionDateFld.enable();
                inspectionHourFld.enable();
                inspectionMinuteFld.enable();

                switch (newValue) {
                    case 10:
                        matSubDateFld.disable();
                        
                        break;
                    case 20:
                        inspectionDateFld.disable();
                        inspectionHourFld.disable();
                        inspectionMinuteFld.disable();
                        
                        break;
                }

            },
            createProtocol: function () {
                var me = this,
                    form = me.getForm(),
                    rec;

                form.getForm().updateRecord();
                rec = form.getRecord();

                me.controller.mask('Формирование протокола', me.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('CreateProtocol', 'RequirementDocument'),
                    timeout: 9999999,
                    method: 'POST',
                    params: {
                        requirementId: rec.getId()
                    }
                }).next(function (res) {
                    form.close();
                    
                    var data = Ext.decode(res.responseText);

                    // Обновляем дерево меню
                    var tree = Ext.ComponentQuery.query(me.controller.params.treeMenuSelector)[0];
                    if (tree) {
                        tree.getStore().load();
                    }

                    var docParams = {};
                    docParams.inspectionId = data.inspectionId;
                    docParams.documentId = data.documentId;
                    docParams.containerSelector = me.controller.params.containerSelector;
                    docParams.treeMenuSelector = me.controller.params.treeMenuSelector;

                    // Для того чтобы маска снялась только после показа новой карточки, формирую функцию обратного вызова
                    if (!me.controller.hideMask) {
                        me.controller.hideMask = function () { me.controller.unmask(); };
                    }

                    me.controller.loadController('B4.controller.ProtocolGji', docParams, me.controller.params.containerSelector, null, me.controller.hideMask);

                    me.controller.unmask();

                    return true;
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка формирования документа!', e.message || e);
                });
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Document', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record) {
                    var form = asp.getForm(),
                        requirementStore = form.down('[name=TypeRequirement]').getStore(),
                        trfArticles = form.down('gkhtriggerfield[name=ArticleLaw]');

                    requirementStore.clearFilter(true);
                    requirementStore.filter('docId', this.controller.params.documentId);
                    
                    //Передаем аспекту смены статуса необходимые параметры
                    this.controller.getAspect('disposalRequirementStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                    this.controller.getAspect('disposalRequirementPrintAspect').loadReportStore();

                    asp.controller.params.reqId = record.getId();

                    trfArticles.setValue('');
                    trfArticles.setDisabled(true);
                    if (record.getId()) {
                        asp.controller.mask('Загрузка', B4.getBody().getActiveTab());
                        trfArticles.setDisabled(false);

                        B4.Ajax.request(B4.Url.action('GetInfo', 'Requirement', {
                            reqId: record.getId()
                        })).next(function (response) {

                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);
                            
                            // если Плановая проверка юр.лица
                            if (obj.data.typeBase == 30) {
                                trfArticles.setDisabled(true);
                            } else {
                                trfArticles.setValue(obj.data.artIds);
                                trfArticles.updateDisplayedText(obj.data.artNames);
                            }
                            
                            asp.controller.unmask();
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'disposalRequirementPrintAspect',
            buttonSelector: 'disposalrequirementeditwin #btnPrint',
            codeForm: 'RequirementGji',
            getUserParams: function () {
                var param = { documentId: this.controller.params.reqId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /ActCheckGJI/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'disprequirementarticlelawMultiSelectWindowAspect',
            fieldSelector: 'disposalrequirementeditwin gkhtriggerfield[name=ArticleLaw]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disprequirementarticlelawSelectWindow',
            storeSelect: 'dict.ArticleLawGjiForSelect',
            storeSelected: 'dict.ArticleLawGjiForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор статей закона',
            titleGridSelect: 'Статьи закона для отбора',
            titleGridSelected: 'Выбранные статьи закона',
            listeners: {
                getdata: function (asp, records) {
                    var objectIds = [];

                    Ext.Array.each(records.items, function (item) {
                        objectIds.push(item.get('Id'));
                    });

                    asp.controller.mask('Сохранение', B4.getBody().getActiveTab());

                    B4.Ajax.request(B4.Url.action('AddArticles', 'RequirementArticleLaw', {
                        objectIds: Ext.encode(objectIds),
                        reqId: asp.controller.params.reqId
                    })).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Статьи закона сохранены успешно');
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
            Аспект взаимодействия таблицы нарушения с массовой формой выбора нарушений
            Тут есть 2 варианта
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalViolationAspect',
            gridSelector: '#disposalViolationGrid',
            storeName: 'disposal.Violation',
            modelName: 'disposal.Violation',
            multiSelectWindowSelector: '#disposalViolationMultiSelectWindow',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            otherActions: function (actions) {
                var me = this;
                actions['#disposalViolationGrid #updateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };

                actions['msviolationsbasedisposal b4selectfield[name=RealityObject]'] = {
                    beforeload: {
                        fn: function (asp, options, store) {
                            // поулчаем дома по основанию проверки
                            options.params.inspectionId = me.controller.params.inspectionId;
                        }
                    }
                };
            },
            onBeforeLoad: function (store, operation) {
                if (this.controller.params.disposalType == 20)
                    operation.params.documentIds = this.controller.params.parentIds;
            },
            listeners: {
                beforegridaction: function (asp, grid, action) {
                    if (action.toLowerCase() == 'add') {
                        /*
                            Если приказ на проверку предписания, то выбираем из нарушений предписания
                            Если приказ не на проверку предписания то выбираем из списка всех нарушений
                        */
                        if (asp.controller.params.disposalType == 20) {
                            asp.multiSelectWindow = 'SelectWindow.MultiSelectWindow';
                            asp.storeSelect = 'prescription.ViolationForSelect';
                            asp.storeSelected = 'prescription.ViolationForSelected';
                            asp.columnsGridSelect = [
                                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
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
                                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
                            ];

                            asp.columnsGridSelected = [
                                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, sortable: false },
                                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, sortable: false },
                                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
                            ];
                        } else {
                            asp.multiSelectWindow = 'disposal.MultiSelectViolationsForBaseDisposal';
                            asp.storeSelect = 'dict.ViolationGjiForSelect';
                            asp.storeSelected = 'dict.ViolationGjiForSelected';
                            asp.columnsGridSelect = [
                                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ];
                            
                            asp.columnsGridSelected = [
                                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ];
                        }
                    }
                },
                
                getdata: function (asp, records) {
                    var recordIds = [],
                        insViolIds = [],
                        roField = asp.getForm().down('b4selectfield[name=RealityObject]'),
                        roId;

                    if (roField != null) {

                        roId = roField.getValue();
                        
                        if (!roId) {
                            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дом');
                            return false;
                        } else {
                            asp.controller.params.realObjId = roId;
                        }
                    }

                    records.each(function (rec) {
                        if (rec.get('InspectionViolationId') > 0) {
                            insViolIds.push(rec.get('InspectionViolationId'));
                        } else {
                            recordIds.push(rec.getId());
                        }
                    });
                    
                    if (recordIds[0] > 0 || insViolIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddViolations', 'DisposalViol'),
                            method: 'POST',
                            params: {
                                violIds: Ext.encode(recordIds),
                                insViolIds: Ext.encode(insViolIds),
                                documentId: asp.controller.params.documentId,
                                realObjId: roId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.getStore('disposal.RealityObjViolation').load();
                            return true;
                        }).error(function (e) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка!', e.message || e);
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        
        me.getStore('disposal.Violation').on('beforeload', me.onBeforeLoadRealityObjViol, me);
        me.getStore('disposal.Expert').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.ProvidedDoc').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.TypeSurvey').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.DisposalVerificationSubjectLicensing').on('beforeload', me.onBeforeLoad, me);
        
        me.getStore('disposal.DisposalVerificationSubjectLicensing').on('load', me.onViewLicensingReady, me);
        
        me.getStore('disposal.RealityObjViolation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.RealityObjViolation').on('load', me.onLoadRealityObjectViolation, me);
        
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            mainView = me.getMainComponent(),
            gridLicensing = mainView.down('disposalsubjectverificationlicensinggrid'),
            requirementgrid = mainView.down('disprequirementgrid');
        
        if (me.params) {
            me.getAspect('disposalEditPanelAspect').setData(me.params.documentId);

            gridLicensing.on('viewready', me.onViewLicensingReady, me);
            gridLicensing.down('button[cmd=saveValidationSubjectLicensing]').on('click', me.saveVerificationSubjectLicensing, me);
            
            requirementgrid.getStore().on('beforeload', me.onBeforeLoad, me);
            
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        
        if (me.params && me.params.documentId)
            operation.params.documentId = me.params.documentId;
    },

    onViewLicensingReady: function () {
        var me = this,
            grid = me.getMainView().down('disposalsubjectverificationlicensinggrid'),
            store = grid.getStore(),
            selectedRecords = [],
            foundIndex = store.find('Active', true);

        while (foundIndex > -1) {
            selectedRecords[selectedRecords.length] = store.findRecord('Active', true, foundIndex);
            foundIndex = store.find('Active', true, foundIndex + 1);
        }

        if (selectedRecords.length > 0) {
            grid.getSelectionModel().select(selectedRecords);
        }
    },
    
    saveVerificationSubjectLicensing: function () {
        var me = this,
            grid = me.getMainView().down('disposalsubjectverificationlicensinggrid'),
            sm = grid.getSelectionModel(),
            store = grid.getStore(),
            selectedCodes = [],
            selectedRecords,
            i;

        if (!me.params || !me.params.documentId || me.params.documentId <= 0) {
            return;
        }

        if (sm.selected.length == 0) {
            return;
        }

        selectedRecords = sm.getSelection();
        for (i = 0; i < selectedRecords.length; i++) {
            selectedCodes[selectedCodes.length] = selectedRecords[i].getId();
        }

        me.mask('Сохранение', grid);

        B4.Ajax.request({
            method: 'POST',
            url: B4.Url.action('AddDisposalVerificationSubjectLicensing', 'DisposalVerificationSubjectLicensing'),
            params: {
                documentId: me.params.documentId,
                ids: selectedCodes
            }
        }).next(function (result) {
            me.unmask();
            store.reload();
        }).error(function (result) {
            me.unmask();

            Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.message);
        });
    },

    onLoadRealityObjectViolation: function (store) {
        var me = this,
            storeViol = me.getStore('disposal.Violation'),
            objGrid,
            countRecords,
            selectIndex = 0;
        
        if (storeViol.getCount() > 0) {
            storeViol.removeAll();
        }

        objGrid = Ext.ComponentQuery.query('#disposalRealityObjViolationGrid')[0];
        countRecords = store.getCount();
        if (countRecords > 0) {

            var findIdx = -1;
            if (me.params.realObjId > 0) {
                findIdx = store.findBy(function (record, id) {
                    if (record.get('RealityObjectId') == me.params.realObjId) {
                        return id;
                    }
                    else return -1;
                });
            }

            if (findIdx != -1) {
                selectIndex = findIdx;
            }
            
            objGrid.getSelectionModel().select(selectIndex);
        }
    },
    
    onBeforeLoadRealityObjViol: function (store, operation) {
        var me = this,
            objGrid = Ext.ComponentQuery.query('#disposalRealityObjViolationGrid')[0],
            violGrid = Ext.ComponentQuery.query('#disposalViolationGrid')[0],
            rec = objGrid.getSelectionModel().getSelection()[0];

        if (rec) {
            operation.params.documentId = me.params.documentId;
            operation.params.realityObjId = rec.get('Id');
            me.params.realObjId = rec.get('Id');
            violGrid.setTitle(rec.get('RealityObject'));
        }
    }
});