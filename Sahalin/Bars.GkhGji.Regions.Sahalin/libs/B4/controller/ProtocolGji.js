Ext.define('B4.controller.ProtocolGji', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.ProtocolGji',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GkhBlobText',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'Resolution',
        'ProtocolGji',
        'protocolgji.Annex',
        'protocolgji.Violation',
        'protocolgji.ArticleLaw',
        'protocolgji.Definition'
    ],

    stores: [
        'ProtocolGji',
        'protocolgji.Violation',
        'protocolgji.RealityObjViolation',
        'protocolgji.Annex',
        'protocolgji.ArticleLaw',
        'protocolgji.Definition',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.ExecutantDocGji',
        'Contragent'
    ],

    views: [
        'protocolgji.EditPanel',
        'protocolgji.RealityObjViolationGrid',
        'protocolgji.AnnexEditWindow',
        'protocolgji.AnnexGrid',
        'protocolgji.ArticleLawGrid',
        'protocolgji.DefinitionEditWindow',
        'protocolgji.DefinitionGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'protocolgji.EditPanel',
    mainViewSelector: '#protocolgjiEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Протокола
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'protocolCreateButtonAspect',
            buttonSelector: '#protocolgjiEditPanel gjidocumentcreatebutton',
            containerSelector: '#protocolgjiEditPanel',
            typeDocument: 60 // Тип документа Протокол
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'protocolStateButtonAspect',
            stateButtonSelector: '#protocolgjiEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('protocolEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            xtype: 'protocolgjiperm',
            editFormAspectName: 'protocolEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'protocolPrintAspect',
            buttonSelector: '#protocolgjiEditPanel #btnPrint',
            codeForm: 'Protocol',
            getUserParams: function (reportId) {

                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'protocolDefinitionPrintAspect',
            buttonSelector: '#protocolgjiDefinitionEditWindow #btnPrint',
            codeForm: 'ProtocolDefinition',
            getUserParams: function () {

                var param = { DefinitionId: this.controller.params.DefinitionId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /*
            Апект для основной панели Протокола
            */
            xtype: 'gjidocumentaspect',
            name: 'protocolEditPanelAspect',
            editPanelSelector: '#protocolgjiEditPanel',
            modelName: 'ProtocolGji',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' #cbToCourt'] = { 'change': { fn: this.onChangeToCourt, scope: this } };
                actions['#protocolgjiRealityObjViolationGrid'] = { 'select': { fn: this.onSelectRealityObjViolationGrid, scope: this } };
            },

            onSelectRealityObjViolationGrid: function () {
                this.controller.getStore('protocolgji.Violation').load();
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

                panel.down('#protocolTabPanel').setActiveTab(0);

                //включаем/выключаем поле "Дата передачи документов"
                var dfToCourt = panel.down('#dfDateToCourt');

                dfToCourt.setDisabled(true);
                if (rec.get('ToCourt')) {
                    dfToCourt.setDisabled(false);
                }

                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber'))
                    panel.setTitle('Протокол ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Протокол');

                //Делаем запросы на получение Инспекторов и документа основания
                //и обновляем соответсвующие Тригер филды

                me.controller.mask('Загрузка', me.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'Protocol', {
                    documentId: asp.controller.params.documentId
                })).next(function (response) {
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldInspectors = panel.down('#trigfInspector');
                    fieldInspectors.setValue(obj.InspectorIds);
                    fieldInspectors.updateDisplayedText(obj.InspectorNames);

                    if (obj.InspectorNames)
                        fieldInspectors.clearInvalid();
                    else
                        fieldInspectors.markInvalid();

                    var fieldBaseName = panel.down('#protocolBaseNameTextField');
                    fieldBaseName.setValue(obj.BaseName);

                    me.disableButtons(false);
                    me.controller.unmask();
                }).error(function () {
                    me.controller.unmask();
                });

                //Передаем аспекту смены статуса необходимые параметры
                this.controller.getAspect('protocolStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                //обновляем отчеты
                this.controller.getAspect('protocolPrintAspect').loadReportStore();

                this.controller.getAspect('protocolDescriptionAspect').doInjection();

                this.setTypeExecutantPermission(rec.get('Executant'));

                // обновляем кнопку Сформирвоать
                this.controller.getAspect('protocolCreateButtonAspect').setData(rec.get('Id'));
            },
            onChangeToCourt: function (field, data) {
                if (data == true) {
                    this.getPanel().down('#dfDateToCourt').setDisabled(false);
                } else {
                    this.getPanel().down('#dfDateToCourt').setDisabled(true);
                }
            },

            setTypeExecutantPermission: function (typeExec) {
                function applyPermissionsForPersonFields(pan, perms) {
                    pan.down('#personFields [name=PersonAddress]').setDisabled(!perms[3]);
                    pan.down('#personFields [name=PersonJob]').setDisabled(!perms[4]);
                    pan.down('#personFields [name=PersonPosition]').setDisabled(!perms[5]);
                    pan.down('#personFields [name=PersonBirthDatePlace]').setDisabled(!perms[6]);
                    pan.down('#personFields [name=PersonDoc]').setDisabled(!perms[7]);
                    pan.down('#personFields [name=PersonSalary]').setDisabled(!perms[8]);
                    pan.down('#personFields [name=PersonRelationship]').setDisabled(!perms[9]);
                }


                var me = this;
                var panel = this.getPanel();
                var permissions = [
                    'GkhGji.DocumentsGji.Protocol.Field.Contragent_Edit',
                    'GkhGji.DocumentsGji.Protocol.Field.PhysicalPerson_Edit',
                    'GkhGji.DocumentsGji.Protocol.Field.PhysicalPersonInfo_Edit',
                    // Person fields
                    'GkhGji.DocumentsGji.Protocol.Field.PersonAddress_Edit',
                    'GkhGji.DocumentsGji.Protocol.Field.PersonJob_Edit',
                    'GkhGji.DocumentsGji.Protocol.Field.PersonPosition_Edit',
                    'GkhGji.DocumentsGji.Protocol.Field.PersonBirthDatePlace_Edit',
                    'GkhGji.DocumentsGji.Protocol.Field.PersonDoc_Edit',
                    'GkhGji.DocumentsGji.Protocol.Field.PersonSalary_Edit',
                    'GkhGji.DocumentsGji.Protocol.Field.PersonRelationship_Edit'
                ];

                panel.down('#sfContragent').setDisabled(true);
                panel.down('#tfPhysPerson').setDisabled(true);
                panel.down('#taPhysPersonInfo').setDisabled(true);
                panel.down('#sfContragent').allowBlank = true;
                applyPermissionsForPersonFields(panel, []); // disable all
                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                            permissions: Ext.encode(permissions),
                            ids: Ext.encode([me.controller.params.documentId])
                        })
                    }).next(function (response) {
                        me.controller.unmask();
                        var perm = Ext.decode(response.responseText)[0];
                        panel.down('#personFields').hide();
                        switch (typeExec.Code) {
                            //Активны все поля                
                            case "13":
                                panel.down('#personFields').show();
                                applyPermissionsForPersonFields(panel, perm);
                                break;
                            case "1":
                            case "5":
                            case "10":
                            case "12":
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
                            case "14":
                                panel.down('#personFields').show();
                                applyPermissionsForPersonFields(panel, perm);
                                break;
                            case "6":
                            case "7":
                                panel.down('#sfContragent').setDisabled(true);
                                panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                                panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);
                                panel.down('#sfContragent').allowBlank = true;
                                break;
                            default:
                                panel.down('#personFields').show();
                                applyPermissionsForPersonFields(panel, perm);

                        }
                    }).error(function () {
                        me.controller.unmask();
                    });
                }
            },

            onChangeTypeExecutant: function (field, value) {

                var data = field.getRecord(value);

                if (data) {
                    if (this.controller.params) {
                        this.controller.params.typeExecutant = data.Code;
                    }
                    this.setTypeExecutantPermission(data);
                }
            },
            onBeforeLoadContragent: function (field, options, store) {
                var executantField = this.controller.getMainView().down('#cbExecutant');

                var typeExecutant = executantField.getRecord(executantField.getValue());
                if (!typeExecutant)
                    return true;

                options = options || {};
                options.params = options.params || {};

                options.params.typeExecutant = typeExecutant.Code;

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
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /ProtocolGJI/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'protocolInspectorMultiSelectWindowAspect',
            fieldSelector: '#protocolgjiEditPanel #trigfInspector',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolInspectorSelectWindow',
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
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'protocolAnnexAspect',
            gridSelector: '#protocolgjiAnnexGrid',
            editFormSelector: '#protocolgjiAnnexEditWindow',
            storeName: 'protocolgji.Annex',
            modelName: 'protocolgji.Annex',
            editWindowView: 'protocolgji.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Protocol', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /**
            * Аспект взаимодействия Таблицы определений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'protocolDefinitionAspect',
            gridSelector: '#protocolgjiDefinitionGrid',
            editFormSelector: '#protocolgjiDefinitionEditWindow',
            storeName: 'protocolgji.Definition',
            modelName: 'protocolgji.Definition',
            editWindowView: 'protocolgji.DefinitionEditWindow',
            onSaveSuccess: function (asp, record) {
                this.setDefinitionId(record.getId());
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Protocol', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    this.setDefinitionId(record.getId());
                }
            },
            setDefinitionId: function (id) {
                this.controller.params.DefinitionId = id;
                if (id) {
                    this.controller.getAspect('protocolDefinitionPrintAspect').loadReportStore();
                }
            }
        },
        {
            /* 
            * Аспект взаимодействия таблицы статьи закона с массовой формой выбора статей
            * По нажатию на Добавить открывается форма выбора статей.
            * По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            * И сохранение статей
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'protocolArticleLawAspect',
            gridSelector: '#protocolgjiArticleLawGrid',
            saveButtonSelector: '#protocolgjiArticleLawGrid #protocolSaveButton',
            storeName: 'protocolgji.ArticleLaw',
            modelName: 'protocolgji.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolArticleLawMultiSelectWindow',
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
                        B4.Ajax.request({
                            url: B4.Url.action('AddArticles', 'ProtocolArticleLaw'),
                            method: 'POST',
                            params: {
                                articleIds: Ext.encode(recordIds),
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
                    return true;
                }
            }
        },
        {
            /**
            * Аспект инлайн таблицы нарушений
            */
            xtype: 'gkhinlinegridaspect',
            name: 'protocolViolationAspect',
            storeName: 'protocolgji.Violation',
            modelName: 'protocolgji.Violation',
            gridSelector: '#protocolgjiViolationGrid',
            saveButtonSelector: '#protocolgjiViolationGrid #protocolViolationSaveButton',
            otherActions: function (actions) {
                var me = this;
                actions['#protocolgjiViolationGrid #updateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'protocolDescriptionAspect',
            fieldSelector: '[name=Description]',
            editPanelAspectName: 'protocolEditPanelAspect',
            controllerName: 'Protocol',
            valueFieldName: 'Description',
            previewLength: 200,
            autoSavePreview: true,
            previewField: 'Description'
        }
    ],

    init: function () {
        var me = this;

        me.getStore('protocolgji.Violation').on('beforeload', me.onBeforeLoadRealityObjViol, me);
        me.getStore('protocolgji.ArticleLaw').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.Definition').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.RealityObjViolation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.RealityObjViolation').on('load', me.onLoadRealityObjectViolation, me);

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('protocolEditPanelAspect').setData(me.params.documentId);

            //Обновляем стор нарушений
            me.getStore('protocolgji.RealityObjViolation').load();

            //Обновляем стор приложений
            me.getStore('protocolgji.Annex').load();

            //Обновляем стор статьей закона
            me.getStore('protocolgji.ArticleLaw').load();

            //Обновляем стор определений
            me.getStore('protocolgji.Definition').load();
        }
    },

    onLoadRealityObjectViolation: function (store) {
        var me = this,
            storeViol = me.getStore('protocolgji.Violation'),
            objGrid = Ext.ComponentQuery.query('#protocolgjiRealityObjViolationGrid')[0],
            countRecords = store.getCount();

        if (storeViol.getCount() > 0) {
            storeViol.removeAll();
        }

        if (countRecords > 0) {
            objGrid.getSelectionModel().select(0);
            if (countRecords == 1) {
                objGrid.up('#protocolWestPanel').collapse();
            } else {
                objGrid.up('#protocolWestPanel').expand();
            }
        } else {
            me.getStore('protocolgji.Violation').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    },

    onBeforeLoadRealityObjViol: function (store, operation) {
        var objGrid = Ext.ComponentQuery.query('#protocolgjiRealityObjViolationGrid')[0],
            violGrid = Ext.ComponentQuery.query('#protocolgjiViolationGrid')[0],
            rec = objGrid.getSelectionModel().getSelection()[0];

        operation.params.documentId = this.params.documentId;
        if (rec) {
            operation.params.realityObjId = rec.getId();
            violGrid.setTitle(rec.get('RealityObject'));
        }
    }
});