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
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.ProtocolGji',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhBlobText'
    ],

    models: [
        'Resolution',
        'ProtocolGji',
        'protocolgji.Annex',
        'protocolgji.Violation',
        'protocolgji.ArticleLaw',
        'protocolgji.Definition',
        'requirement.Requirement',
        'DocumentPhysInfo'
    ],

    stores: [
        'protocol.ProtocolRequirement',
        'ProtocolGji',
        'protocolgji.Violation',
        'protocolgji.RealityObjViolation',
        'protocolgji.Annex',
        'protocolgji.ArticleLaw',
        'protocolgji.Definition',
        'inspectiongji.ViolStageForSelect',
        'inspectiongji.ViolStageForSelected',
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
        'protocolgji.RequirementGrid',
        'protocolgji.RequirementEditWindow',
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
            xtype: 'b4_state_contextmenu',
            name: 'protocolRequirementStateTransferAspect',
            gridSelector: 'protocolrequirementgrid',
            menuSelector: 'protocolRequirementStateMenu',
            stateType: 'gji_requirement'
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
            /**
            * Вешаем аспект смены статуса на форме Требования
            */
            xtype: 'statebuttonaspect',
            name: 'protocolRequirementStateButtonAspect',
            stateButtonSelector: 'protocolgjirequirementeditwin #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var me = this,
                        model = this.controller.getModel('requirement.Requirement');

                    model.load(entityId, {
                        success: function (rec) {
                            me.controller.getAspect('protocolRequirementAspect').setFormData(rec);
                        },
                        scope: this
                    });
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
            getUserParams: function (reportId) {

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
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this} };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' #cbToCourt'] = { 'change': { fn: this.onChangeToCourt, scope: this } };
                actions['#protocolgjiRealityObjViolationGrid'] = { 'select': { fn: this.onSelectRealityObjViolationGrid, scope: this} };
            },

            onSelectRealityObjViolationGrid: function (rowModel, record, index, opt) {
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
                    var fieldReq = panel.down('#protocolReqNameTextField');
                    fieldInspectors.setValue(obj.inspectorIds);
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);

                    if (obj.inspectorNames)
                        fieldInspectors.clearInvalid();
                    else
                        fieldInspectors.markInvalid();

                    if (obj.reqName) {
                        fieldReq.setValue(obj.reqName);
                        fieldReq.show();
                    } else {
                        fieldReq.setValue(null);
                        fieldReq.hide();
                    }

                    var fieldBaseName = panel.down('#protocolBaseNameTextField');
                    fieldBaseName.setValue(obj.baseName);

                    asp.controller.params.parentId = obj.parentId;

                    me.disableButtons(false);
                    me.controller.unmask();
                }).error(function () {
                    me.controller.unmask();
                });

                //Обновляем сторы
                me.controller.getStore('protocolgji.RealityObjViolation').load();
                me.controller.getStore('protocolgji.Annex').load();
                me.controller.getStore('protocolgji.ArticleLaw').load();
                me.controller.getStore('protocolgji.Definition').load();
                panel.down('protocolrequirementgrid').getStore().load();

                //Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('protocolStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                //обновляем отчеты
                me.controller.getAspect('protocolPrintAspect').loadReportStore();
                
                me.setTypeExecutantPermission(rec.get('Executant'));
                
                // обновляем кнопку Сформирвоать
                me.controller.getAspect('protocolCreateButtonAspect').setData(rec.get('Id'));

                me.controller.getAspect('protocolDescrBlobAspect').doInjection();
                me.controller.getAspect('protocolDescrSetBlobAspect').doInjection();
            },

            onChangeToCourt: function (field, data) {
                if (data == true) {
                    this.getPanel().down('#dfDateToCourt').setDisabled(false);
                } else {
                    this.getPanel().down('#dfDateToCourt').setDisabled(true);
                }
            },

            setTypeExecutantPermission: function (typeExec) {
                var me = this,
                    panel = me.getPanel(),
                    permissions = [
                        'GkhGji.DocumentsGji.Protocol.Field.Contragent_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.PhysicalPerson_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.PhysicalPersonInfo_Edit'
                    ];

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('GetObjectSpecificPermissions', 'Permission'),
                        params: {
                            permissions: Ext.encode(permissions),
                            ids: Ext.encode([me.controller.params.documentId])
                        }
                    }).next(function (response) {
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
                    }).error(function () {
                        me.controller.unmask();
                    });
                }
            },

            onChangeTypeExecutant: function (field, value, oldValue) {
                var me = this,
                    data = field.getRecord(value),
                    contragentField = field.up(me.editPanelSelector).down('#sfContragent');

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
            onBeforeLoadContragent: function (field, options, store) {
                var executantField = this.controller.getMainView().down('#cbExecutant'),
                    typeExecutant = executantField.getRecord(executantField.getValue());
                if (!typeExecutant)
                    return true;

                options = options || {};
                options.params = options.params || {};

                options.params.typeExecutant = typeExecutant.Code;

                return true;
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
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'protocolDescrBlobAspect',
            fieldSelector: 'tabtextarea[name=Description]',
            editPanelAspectName: 'protocolEditPanelAspect',
            controllerName: 'Protocol',
            valueFieldName: 'Description',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: 'Description',

            getParentRecordId: function () {
                return this.editPanelAspect.getRecord().getId();
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'protocolDescrSetBlobAspect',
            fieldSelector: 'tabtextarea[name=DescriptionSet]',
            editPanelAspectName: 'protocolEditPanelAspect',
            controllerName: 'Protocol',
            valueFieldName: 'DescriptionSet',
            previewLength: 2000,
            previewField: false,

            getParentRecordId: function () {
                return this.editPanelAspect.getRecord().getId();
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
                    var me = this,
                        cmpDateOfProc,
                        cmpTimeDifinition,
                        cmpExecDate;
                    
                    me.setDefinitionId(record.getId());

                    cmpDateOfProc = form.down('[name=DateOfProceedings]');
                    cmpTimeDifinition = form.down('[name=TimeDefinition]');
                    cmpExecDate = form.down('[name=ExecutionDate]');
                    
                    //Если ТипОпределения == 'О назначении времени и места рассмотрения дела'
                    // то тогда показываем поля ввода Даты и времени
                    if (record.get('TypeDefinition') == 10) {
                        cmpDateOfProc.show();
                        cmpTimeDifinition.show();
                        cmpExecDate.setVisible(false);
                        
                        // Если значения еще пустые то по умолчанию заполняем их Датой и временем Родительского протокола
                        if (!record.get('DateOfProceedings') && !record.get('TimeDefinition')) {
                            //То запускаем B4.Ajax Который будет проставлять по умолчанию значение в поле Дата и Время
                            
                            B4.Ajax.request({
                                url: B4.Url.action('GetDefaultParams', 'ProtocolDefinition'),
                                timeout: 9999999,
                                method: 'POST',
                                params: {
                                    protocolId: me.controller.params ? me.controller.params.documentId : 0
                                }
                            }).next(function (res) {

                                var data = Ext.decode(res.responseText);

                                cmpDateOfProc.setValue(data.dateOfProc);
                                cmpTimeDifinition.setValue(data.timeDef);

                                return true;
                            }).error(function (e) {
                                Ext.Msg.alert('Ошибка получения параметров по умолчанию!', e.message || e);
                            });
                        }
                        
                    } else {
                        //иначе их скрываем
                        cmpDateOfProc.hide();
                        cmpTimeDifinition.hide();
                        cmpExecDate.setVisible(true);
                    }
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
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield'} }
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
            /*
            Аспект взаимодействия Таблицы требований с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'protocolRequirementAspect',
            gridSelector: 'protocolrequirementgrid',
            editFormSelector: 'protocolgjirequirementeditwin',
            modelName: 'requirement.Requirement',
            editWindowView: 'protocolgji.RequirementEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #btnCreateProtocol'] = {
                    click: {
                        fn: function() {
                            me.createProtocol();
                        }
                    }
                };
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
            createProtocol: function() {
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
                getdata: function(asp, record) {
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
                    this.controller.getAspect('protocolRequirementStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                    this.controller.getAspect('protocolRequirementPrintAspect').loadReportStore();

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

                            trfArticles.setValue(obj.data.artIds);
                            trfArticles.updateDisplayedText(obj.data.artNames);
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
            name: 'protocolRequirementPrintAspect',
            buttonSelector: 'protocolgjirequirementeditwin #btnPrint',
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
            name: 'protocolrequirementarticlelawMultiSelectWindowAspect',
            fieldSelector: 'protocolgjirequirementeditwin gkhtriggerfield[name=ArticleLaw]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protrequirementarticlelawSelectWindow',
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
            /* 
            Аспект взаимодействия таблицы нарушения с массовой формой выбора нарушений
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'protocolViolationAspect',
            gridSelector: '#protocolgjiViolationGrid',
            saveButtonSelector: '#protocolgjiViolationGrid #protocolViolationSaveButton',
            storeName: 'protocolgji.Violation',
            modelName: 'protocolgji.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolViolationMultiSelectWindow',
            storeSelect: 'inspectiongji.ViolStageForSelect',
            storeSelected: 'inspectiongji.ViolStageForSelected',
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
            otherActions: function (actions) {
                var me = this;
                actions['#protocolgjiViolationGrid #updateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };
            },
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.parentId > 0)
                    operation.params.documentId = this.controller.params.parentId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('InspectionViolationId')); });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddViolations', 'ProtocolViolation'),
                            method: 'POST',
                            params: {
                                insViolIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.getStore('protocolgji.RealityObjViolation').load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
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
        me.getStore('protocolgji.Violation').on('beforeload', me.onBeforeLoadRealityObjViol, me);
        me.getStore('protocolgji.ArticleLaw').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.Definition').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.RealityObjViolation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.RealityObjViolation').on('load', me.onLoadRealityObjectViolation, me);

        me.control({
            '#protocolgjiDefinitionEditWindow combobox[name=TypeDefinition]' : {
                change: me.onTypeDefinitionChange
            },
            '#protocolgjirequirementEditWindow gkhtriggerfield[name=ArticleLaw]': {
                triggerClear: me.onArticleLawClear
            }
        });
        
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            requirementgrid;
        if (me.params) {
            me.getAspect('protocolEditPanelAspect').setData(me.params.documentId);
            
            requirementgrid = me.getMainComponent().down('protocolrequirementgrid');
            requirementgrid.getStore().on('beforeload', me.onBeforeLoad, me);
        }
    },
    
    onLoadRealityObjectViolation: function (store) {
        var storeViol = this.getStore('protocolgji.Violation');
        if (storeViol.getCount() > 0) {
            storeViol.removeAll();
        }

        var objGrid = Ext.ComponentQuery.query('#protocolgjiRealityObjViolationGrid')[0];
        var countRecords = store.getCount();
        if (countRecords > 0) {
            objGrid.getSelectionModel().select(0);
            if (countRecords == 1) {
                objGrid.up('#protocolWestPanel').collapse();
            } else {
                objGrid.up('#protocolWestPanel').expand();
            }
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

        if (rec) {
            operation.params.documentId = this.params.documentId;
            operation.params.realityObjId = rec.getId();
            violGrid.setTitle(rec.get('RealityObject'));
        }
    },

    onTypeDefinitionChange: function (chbx, newValue) {
        var editForm = chbx.up('#protocolgjiDefinitionEditWindow'),
            cmpExecDate = editForm.down('[name=ExecutionDate]');

        cmpExecDate.setVisible(newValue !== 10);
    },

    onArticleLawClear: function (obj) {
        var me = this;

        if (obj.rawValue) {
            me.mask();

            B4.Ajax.request(B4.Url.action('AddArticles', 'RequirementArticleLaw', {
                objectIds: Ext.encode([]),
                reqId: me.params.reqId
            })).next(function() {
                me.unmask();
                return true;
            }).error(function() {
                me.unmask();
            });
        }
    }
});