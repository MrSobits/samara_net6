Ext.define('B4.controller.Prescription', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,

    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GjiDocument',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.Prescription',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhBlobText'
    ],

    models: [
        'ProtocolGji',
        'Prescription',
        'prescription.Annex',
        'prescription.Cancel',
        'prescription.Violation',
        'prescription.ArticleLaw',
        'violationgroup.ViolationGroup'
    ],

    stores: [
        'Contragent',
        'Prescription',
        'prescription.Violation',
        'prescription.RealityObjViolation',
        'prescription.ViolationForSelect',
        'prescription.ViolationForSelected',
        'prescription.Annex',
        'prescription.ArticleLaw',
        'prescription.Cancel',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.ExecutantDocGji',
        'dict.Municipality',
        'violationgroup.ViolationGroupForSelect',
        'violationgroup.ViolationGroupForSelected',
        'prescription.ViolationGroup'
    ],

    views: [
        'prescription.EditPanel',
        'prescription.RealityObjViolationGrid',
        'prescription.AnnexEditWindow',
        'prescription.AnnexGrid',
        'prescription.ArticleLawGrid',
        'prescription.CancelEditWindow',
        'prescription.CancelGrid',
        'prescription.RealityObjListPanel',
        'SelectWindow.MultiSelectWindow',
        'prescription.ViolationGrid',
        'prescription.ViolationEditWindow',
        'prescription.ViolationGroupGrid',
        'prescription.ViolationGroupEditWindow'
    ],

    mainView: 'prescription.EditPanel',
    mainViewSelector: '#prescriptionEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Акта проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'prescriptionCreateButtonAspect',
            buttonSelector: '#prescriptionEditPanel gjidocumentcreatebutton',
            containerSelector: '#prescriptionEditPanel',
            typeDocument: 50, // Тип документа предписания
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил для которых необходимы пользовательские параметры
                // при созданни документов из предписания все правила ожидают пользовательские параметры
                if (params.ruleId == 'PrescriptionToProtocolRule') {
                    return false;
                }
            }
        },
        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'prescriptionStateButtonAspect',
            stateButtonSelector: '#prescriptionEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('prescriptionEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            /*
             * аспект кнопки печати предписания
             */
            xtype: 'gkhbuttonprintaspect',
            name: 'prescriptionPrintAspect',
            buttonSelector: '#prescriptionEditPanel #btnPrint',
            codeForm: 'Prescription',
            getUserParams: function(reportId) {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
             * аспект кнопки печати решения об отмене предписания
             */
            xtype: 'gkhbuttonprintaspect',
            name: 'prescriptionCancelPrintAspect',
            buttonSelector: '#prescriptionCancelEditWindow #btnPrint',
            codeForm: 'PrescriptionCancel',
            getUserParams: function(reportId) {
                var param = { CancelId: this.controller.params.CancelId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
             * аспект пермишенов предписания
             */
            xtype: 'prescriptionperm',
            editFormAspectName: 'prescriptionEditPanelAspect'
        },
        {
            /*
            * Апект для основной панели Предписания
            */
            xtype: 'gjidocumentaspect',
            name: 'prescriptionEditPanelAspect',
            editPanelSelector: '#prescriptionEditPanel',
            modelName: 'Prescription',
            otherActions: function(actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions['#prescriptionRealityObjViolationGrid'] = { 'select': { fn: this.onSelectRealityObjViolationGrid, scope: this } };
            },

            onSelectRealityObjViolationGrid: function() {
                this.controller.getStore('prescription.Violation').load();
                this.controller.getStore('prescription.ViolationGroup').load();
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                var contragent = asp.controller.getMainView().down('#sfContragent');
                
                //После проставления данных обновляем title вкладки
                panel.setTitle(asp.controller.params.title);

                if (rec.get('DocumentNumber')) {
                    panel.setTitle('Предписание ' + rec.get('DocumentNumber'));
                } else {
                    panel.setTitle('Предписание');
                }

                if (!Ext.isObject(contragent.value)) {
                    contragent.setDisabled(true);
                }
                
                panel.down('#prescriptionTabPanel').setActiveTab(0);

                //Делаем запросы на получение Инспекторов и документа основания
                //и обновляем соответствующие Тригер филды
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'Prescription', {
                    documentId: asp.controller.params.documentId
                })).next(function(response) {
                    asp.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldInspectors = panel.down('#prescriptionInspectorsTrigerField');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    var fieldBaseName = panel.down('#prescriptionBaseNameTextField');
                    fieldBaseName.setValue(obj.baseName);

                    asp.disableButtons(false);
                }).error(function() {
                    asp.controller.unmask();
                });
                
                //загружаем стор меню кнопки печати
                this.controller.getAspect('prescriptionPrintAspect').loadReportStore();

                //обновляем статусы
                this.controller.getAspect('prescriptionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                // обновляем кнопку Сформирвоать
                this.controller.getAspect('prescriptionCreateButtonAspect').setData(rec.get('Id'));
            },

            setTypeExecutantPermission: function(typeExec) {
                var me = this,
                    panel = this.getPanel(),
                    permissions = [
                        'GkhGji.DocumentsGji.Prescription.Field.Contragent_Edit',
                        'GkhGji.DocumentsGji.Prescription.Field.PhysicalPerson_Edit',
                        'GkhGji.DocumentsGji.Prescription.Field.PhysicalPersonInfo_Edit'
                    ];

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                            permissions: Ext.encode(permissions),
                            ids: Ext.encode([me.controller.params.documentId])
                        })
                    }).next(function(response) {
                        me.controller.unmask();
                        var perm = Ext.decode(response.responseText)[0];
                        switch (typeExec.Code) {
                        //Активны все поля                                                                    
                        case "1":
                        case "5":
                        case "10":
                        case "12":
                        case "13":
                        case "16":
                        case "19":
                            panel.down('#sfContragent').setDisabled(!perm[0]);

                            panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                            panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);

                            panel.down('#sfContragent').allowBlank = false;
                            break;
                        //Активно поле Юр.лицо                                                                    
                        case "0":
                        case "4":
                        case "8":
                        case "9":
                        case "11":
                        case "15":
                        case "18":
                            panel.down('#sfContragent').setDisabled(!perm[0]);

                            panel.down('#tfPhysPerson').setDisabled(true);
                            panel.down('#taPhysPersonInfo').setDisabled(true);

                            panel.down('#sfContragent').allowBlank = false;
                            break;
                        //Активны поля Физ.лица                                                                    
                        case "6":
                        case "7":
                        case "14":
                            panel.down('#sfContragent').setDisabled(true);

                            panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                            panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);

                            panel.down('#sfContragent').allowBlank = true;
                            break;
                        }
                    }).error(function() {
                        me.controller.unmask();
                    });
                }
            },

            onChangeTypeExecutant: function(field, value, oldValue) {
                var me = this,
                    contragentField = field.up(me.editPanelSelector).down('#sfContragent'),
                    data = field.getRecord(value);

                if (!Ext.isEmpty(contragentField) && !Ext.isEmpty(oldValue)) {
                    contragentField.setValue(null);
                }

                if (data) {
                    if (me.controller.params) {
                        me.controller.params.typeExecutant = data.Code;
                    }
                    me.setTypeExecutantPermission(data);
                }
            },
            onBeforeLoadContragent: function(field, options, store) {
                var executantField = this.getPanel().down('#cbExecutant');

                var typeExecutant = executantField.getRecord(executantField.getValue());
                if (!typeExecutant)
                    return true;

                options = options || {};
                options.params = options.params || {};

                options.params.typeExecutant = typeExecutant.Code;

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
            }
        },
        /* Сначала решили что в предпитсании должно нарушение редактироватся по форме редактирования, после переделали чтобы былигруппы описаний
        {
            // Аспект взаимодействия Таблицы приложений с формой редактирования
            
            xtype: 'grideditwindowaspect',
            name: 'prescriptionViolAspect',
            gridSelector: '#prescriptionViolationGrid',
            editFormSelector: 'prescrvioleditwindow',
            storeName: 'prescription.Violation',
            modelName: 'prescription.Violation',
            editWindowView: 'prescription.ViolationEditWindow'
        },
        {
            xtype: 'gkhblobtextaspect',
            fieldSelector: '[name="Description"]',
            editPanelAspectName: 'prescriptionViolAspect',
            controllerName: 'PrescriptionViol',
            valueFieldName: 'Description'
        },
        {
            xtype: 'gkhblobtextaspect',
            fieldSelector: '[name="Action"]',
            editPanelAspectName: 'prescriptionViolAspect',
            controllerName: 'PrescriptionViol',
            valueFieldName: 'Action'
        },*/
        {
            /*
            Аспект инлайн таблицы нарушений
            */
            xtype: 'gkhinlinegridaspect',
            name: 'prescriptionViolationAspect',
            storeName: 'prescription.Violation',
            modelName: 'prescription.Violation',
            gridSelector: '#prescriptionViolationGrid',
            //saveButtonSelector: '#prescriptionRealityObjListPanel #prescriptionViolationSaveButton', В смоленске нельзя сохранять грид нарушений все заполняется через Описания
            otherActions: function (actions) {
                var me = this;
                actions['#prescriptionViolationGrid #updateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };
            }
            /* В Смоленске грид нарушения работает тольк она отображение вся работа по вводу ДАт осущетсвляется в описаниях
            ,
            listeners: {
                beforesave: {
                    fn: function(asp, store) {
                        var result = true,
                            firstDate;

                        Ext.each(store.data.items, function(rec) {
                            if (Ext.isEmpty(rec.get('DatePlanRemoval'))) {
                                result = false;
                                return false;
                            }
                        });

                        if (!result) {
                            Ext.Msg.alert('Ошибка!', 'У всех записей должен быть заполнен срок исполнения!');
                            return false;
                        }
                        
                        return true;
                    }
                }
            }
            */
        },
        {
            /* 
            Аспект взаимодействия кнопки Протокол с массовой формой выборка
            По нажатию на кнопку Протокол открывается форма массовго выбора нарушений
            По нажатию на применить у главного аспекта вызывается метод createProtocol
            в который передаются Id нарушений
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'prescriptionToProtocolAspect',
            buttonSelector: '#prescriptionEditPanel [ruleId=PrescriptionToProtocolRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#prescriptionToProtocolSelectWindow',
            storeSelectSelector: '#prescriptionViolationForSelected',
            storeSelect: 'prescription.ViolationForSelect',
            storeSelected: 'prescription.ViolationForSelected',
            columnsGridSelect: [
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
            ],
            columnsGridSelected: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, sortable: false },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',

            onBeforeLoad: function(store, operation) {
                if (this.controller.params)
                    operation.params.documentId = this.controller.params.documentId;
            },

            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function(rec) {
                        listIds.push(rec.get('InspectionViolationId'));
                    });

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('prescriptionCreateButtonAspect');
                        // еще раз получаем параметры по умолчанию и добавляем к уже созданным еще один (Выбранные пользователем нарушения)
                        params = creationAspect.getParams(btn);
                        params.violationIds = listIds;
                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /PrescriptionGJI/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'prescriptionInspectorMultiSelectWindowAspect',
            fieldSelector: '#prescriptionEditPanel #prescriptionInspectorsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#prescriptionInspectorSelectWindow',
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

                    records.each(function(rec) {
                        recordIds.push(rec.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function() {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'prescriptionAnnexAspect',
            gridSelector: '#prescriptionAnnexGrid',
            editFormSelector: '#prescriptionAnnexEditWindow',
            storeName: 'prescription.Annex',
            modelName: 'prescription.Annex',
            editWindowView: 'prescription.AnnexEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Prescription', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы решений об отмены с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'prescriptionCancelAspect',
            gridSelector: '#prescriptionCancelGrid',
            editFormSelector: '#prescriptionCancelEditWindow',
            storeName: 'prescription.Cancel',
            modelName: 'prescription.Cancel',
            editWindowView: 'prescription.CancelEditWindow',
            otherActions: function (actions) {
                actions['#prescriptionCancelEditWindow b4combobox[name=TypeCancel]'] = {
                    change: function (cmb, newValue) {
                        var win = cmb.up('#prescriptionCancelEditWindow'),
                            handler = function(cmp, show) {
                                cmp.setVisible(show);
                            };
                        if (newValue === 10) {
                            handler(win.down('[name=PetitionNum]'), true);
                            handler(win.down('[name=PetitionDate]'), true);
                            handler(win.down('[name=IsCourt]'), false);
                            handler(win.down('[name=DateCancel]'), false);
                            handler(win.down('[name=Reason]'), false);
                            handler(win.down('[name=DescriptionSet]'), true);
                            handler(win.down('[name=CancelResult]'), true);
                        } else {
                            handler(win.down('[name=PetitionNum]'), false);
                            handler(win.down('[name=PetitionDate]'), false);
                            handler(win.down('[name=IsCourt]'), true);
                            handler(win.down('[name=DateCancel]'), true);
                            handler(win.down('[name=Reason]'), true);
                            handler(win.down('[name=DescriptionSet]'), false);
                            handler(win.down('[name=CancelResult]'), false);
                        }
                    }
                };
            },
            onSaveSuccess: function(asp, record) {
                asp.setCancelId(record.getId());
            },
            setCancelId: function(id) {
                this.controller.params.CancelId = id;
                if (id) {
                    this.controller.getAspect('prescriptionCancelPrintAspect').loadReportStore();
                }
            },
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Prescription', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function(asp, record) {
                    asp.setCancelId(record.getId());
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы статьи закона с массовой формой выбора статей
            По нажатию на Добавить открывается форма выбора статей.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение статей
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'prescriptionArticleLawAspect',
            gridSelector: '#prescriptionArticleLawGrid',
            saveButtonSelector: '#prescriptionArticleLawGrid #prescriptionSaveButton',
            storeName: 'prescription.ArticleLaw',
            modelName: 'prescription.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#prescriptionArticleLawMultiSelectWindow',
            storeSelect: 'dict.ArticleLawGjiForSelect',
            storeSelected: 'dict.ArticleLawGjiForSelected',
            titleSelectWindow: 'Выбор статей закона',
            titleGridSelect: 'Статьи для отбора',
            titleGridSelected: 'Выбранные статьи',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddArticles', 'PrescriptionArticleLaw'),
                            method: 'POST',
                            params: {
                                articleIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать статьи закона');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы Описаний с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'prescriptionViolGroupAspect',
            gridSelector: 'prescriptionViolationGroupGrid',
            editFormSelector: 'prescriptionViolationGroupEditWindow',
            storeName: 'prescription.ViolationGroup',
            modelName: 'violationgroup.ViolationGroup',
            editWindowView: 'prescription.ViolationGroupEditWindow',
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
            name: 'prescriptionBlobDescriptionAspect',
            fieldSelector: '[name=Description]',
            editPanelAspectName: 'prescriptionViolGroupAspect',
            controllerName: 'DocumentViolGroup',
            valueFieldName: 'Description',
            previewLength: 150,
            autoSavePreview: true,
            previewField: 'Description'
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'prescriptionBlobActionAspect',
            fieldSelector: '[name=Action]',
            editPanelAspectName: 'prescriptionViolGroupAspect',
            controllerName: 'DocumentViolGroup',
            valueFieldName: 'Action',
            previewLength: 150,
            autoSavePreview: true,
            previewField: 'Action'
        }
    ],

    init: function () {
        this.getStore('prescription.ViolationGroup').on('beforeload', this.onBeforeLoadViolGroup, this);
        this.getStore('prescription.Violation').on('beforeload', this.onBeforeLoadRealityObjViol, this);
        this.getStore('prescription.Annex').on('beforeload', this.onBeforeLoad, this);
        this.getStore('prescription.ArticleLaw').on('beforeload', this.onBeforeLoad, this);
        this.getStore('prescription.Cancel').on('beforeload', this.onBeforeLoad, this);
        this.getStore('prescription.RealityObjViolation').on('beforeload', this.onBeforeLoad, this);
        this.getStore('prescription.RealityObjViolation').on('load', this.onLoadRealityObjectViolation, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('prescriptionEditPanelAspect').setData(me.params.documentId);

            //Обновляем стор нарушений предписания
            me.getStore('prescription.RealityObjViolation').load();

            //Обновляем стор приложений
            me.getStore('prescription.Annex').load();

            //Обновляем стор статьи закона
            me.getStore('prescription.ArticleLaw').load();

            //Обновляем стор решений об отмене
            me.getStore('prescription.Cancel').load();
        }
    },

    onLoadRealityObjectViolation: function(store) {
        var me = this,
            storeViol = me.getStore('prescription.Violation'),
            storeViolGroup = this.getStore('prescription.ViolationGroup'),
            objGrid,
            countRecords;
        
        if (storeViol.getCount() > 0) {
            storeViol.removeAll();
        }
        
        if (storeViolGroup.getCount() > 0) {
            storeViolGroup.removeAll();
        }
        
        objGrid = Ext.ComponentQuery.query('#prescriptionRealityObjViolationGrid')[0],
        countRecords = store.getCount();
        
        if (countRecords > 0) {
            objGrid.getSelectionModel().select(0);
            if (countRecords == 1) {
                objGrid.up('#prescriptionWestPanel').collapse();
            } else {
                objGrid.up('#prescriptionWestPanel').expand();
            }
        } else {
            me.getStore('prescription.Violation').load();
        }
    },

    onBeforeLoad: function(store, operation) {
        if (this.params)
            operation.params.documentId = this.params.documentId;
    },

    onBeforeLoadRealityObjViol: function (store, operation) {
        var objGrid = Ext.ComponentQuery.query('#prescriptionRealityObjViolationGrid')[0],
            violGrid = Ext.ComponentQuery.query('#prescriptionViolationGrid')[0],
            tabPanel = violGrid.up('tabpanel'),
            rec = objGrid.getSelectionModel().getSelection()[0];

        if (rec) {
            operation.params.documentId = this.params.documentId;
            operation.params.realityObjId = rec.getId();
            tabPanel.setTitle(rec.get('RealityObject'));
        }
    },
    
    onBeforeLoadViolGroup: function (store, operation) {
        var objGrid = Ext.ComponentQuery.query('#prescriptionRealityObjViolationGrid')[0],
            rec = objGrid.getSelectionModel().getSelection()[0];

        if (rec) {
            operation.params.documentId = this.params.documentId;
            operation.params.withRo = true;
            operation.params.roId = rec.getId();
        }
    }
});