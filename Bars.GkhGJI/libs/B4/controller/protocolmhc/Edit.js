Ext.define('B4.controller.protocolmhc.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,

    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ProtocolMhcState',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton'
    ],

    models: [
        'ProtocolMhc',
        'Resolution',
        'protocolmhc.Annex',
        'protocolmhc.ArticleLaw',
        'protocolmhc.RealityObject',
        'protocolmhc.Definition'
    ],

    stores: [
        'ProtocolMhc',
        'protocolmhc.Annex',
        'protocolmhc.ArticleLaw',
        'protocolmhc.RealityObject',
        'protocolmhc.Definition',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.ExecutantDocGji',
        'dict.Municipality',
        'Contragent'
    ],

    views: [
        'protocolmhc.EditPanel',
        'protocolmhc.AnnexEditWindow',
        'protocolmhc.AnnexGrid',
        'protocolmhc.ArticleLawGrid',
        'protocolmhc.RealityObjectGrid',
        'protocolmhc.DefinitionGrid',
        'protocolmhc.DefinitionEditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'protocolmhc.EditPanel',
    mainViewSelector: '#protocolMhcEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'protocolMhcCreateButtonAspect',
            buttonSelector: '#protocolMhcEditPanel gjidocumentcreatebutton',
            containerSelector: '#protocolMhcEditPanel',
            typeDocument: 130 // Тип документа Протокол МЖК
        },
        {
            xtype: 'protocolmhcstateperm',
            editFormAspectName: 'protocolMhcEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'protocolMhcStateButtonAspect',
            stateButtonSelector: '#protocolMhcEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('protocolMhcEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'protocolMhcDefinitionPrintAspect',
            buttonSelector: '#protocolMhcDefinitionEditWindow #btnPrint',
            codeForm: 'ProtocolMhcDefinition',
            getUserParams: function (reportId) {

                var param = { DefinitionId: this.controller.params.DefinitionId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /*
            Апект для основной панели Протокола МЖК
            */
            xtype: 'gjidocumentaspect',
            name: 'protocolMhcEditPanelAspect',
            editPanelSelector: '#protocolMhcEditPanel',
            modelName: 'ProtocolMhc',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this} };
                actions[this.editPanelSelector + ' #sfContragent'] = {
                    'beforeload': { fn: this.onBeforeLoadContragent, scope: this }
                };
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
                
                //После проставления данных обновляем title вкладки
                
                if (rec.get('DocumentNumber')) 
                    panel.setTitle('Протокол МЖК ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Протокол МЖК');

                panel.down('#protocolMhcTabPanel').setActiveTab(0);
                
                this.disableButtons(false);

                this.setTypeExecutantPermission(rec.get('Executant'));
                
                //Обновляем статусы
                this.controller.getAspect('protocolMhcStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                // обновляем кнопку Сформирвоать
                this.controller.getAspect('protocolMhcCreateButtonAspect').setData(rec.get('Id'));
            },

            setTypeExecutantPermission: function (typeExec) {
                var me = this;
                var panel = this.getPanel();
                var permissions = [
                    'GkhGji.DocumentsGji.ProtocolMhc.Field.Contragent_Edit',
                    'GkhGji.DocumentsGji.ProtocolMhc.Field.PhysicalPerson_Edit',
                    'GkhGji.DocumentsGji.ProtocolMhc.Field.PhysicalPersonInfo_Edit'
                ];

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                        permissions: Ext.encode(permissions),
                        ids: Ext.encode([me.controller.params.documentId])
                    })).next(function (response) {
                        me.controller.unmask();
                        var perm = Ext.decode(response.responseText)[0];
                        switch (typeExec.Code) {
                            //Активны все поля                                                                        
                            case "1":
                            case "3":
                            case "5":
                            case "11":
                            case "13":
                            case "16":
                            case "18":
                            case "19":
                                panel.down('#sfContragent').setDisabled(!perm[0]);

                                panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                                panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);

                                panel.down('#sfContragent').allowBlank = false;
                                break;

                                //Активно поле Юр.лицо                                                                        
                            case "0":
                            case "2":
                            case "4":
                            case "10":
                            case "12":
                            case "6":
                            case "7":
                            case "15":
                            case "21": //ИП
                                panel.down('#sfContragent').setDisabled(!perm[0]);

                                panel.down('#tfPhysPerson').setDisabled(true);
                                panel.down('#taPhysPersonInfo').setDisabled(true);

                                panel.down('#sfContragent').allowBlank = false;
                                break;

                                //Активны поля Физ.лица                                                                        
                            case "8":
                            case "9":
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

                var data = field.getRecord(value);
                
                var contragentField = field.up(this.editPanelSelector).down('#sfContragent');

                if (!Ext.isEmpty(contragentField) && !Ext.isEmpty(oldValue)) {
                    contragentField.setValue(null);
                }

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
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'protocolMhcAnnexAspect',
            gridSelector: '#protocolMhcAnnexGrid',
            editFormSelector: '#protocolMhcAnnexEditWindow',
            storeName: 'protocolmhc.Annex',
            modelName: 'protocolmhc.Annex',
            editWindowView: 'protocolmhc.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ProtocolMhc', this.controller.params.documentId);
                    }
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
            name: 'protocolMhcArticleLawAspect',
            gridSelector: '#protocolMhcArticleLawGrid',
            saveButtonSelector: '#protocolMhcArticleLawGrid #btnSaveArticles',
            storeName: 'protocolmhc.ArticleLaw',
            modelName: 'protocolmhc.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolMhcArticleLawMultiSelectWindow',
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
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable:false }
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
                            url: B4.Url.action('AddArticles', 'ProtocolMhcArticleLaw'),
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
            Аспект взаимодействия таблицы домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'protocolMhcRealityObjectAspect',
            gridSelector: '#protocolMhcRealityObjectGrid',
            storeName: 'protocolmhc.RealityObject',
            modelName: 'protocolmhc.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolMhcRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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

            onBeforeLoad: function (store, operation) {
                if (this.controller.params) {
                    operation.params.contragentId = this.controller.params.contragentId;
                    //если тип не относится к управляющим организациям, то ставим тип юр.лица 0 (получаем все дома)
                    switch (this.controller.params.typeExecutant) {
                        case "0":
                        case "1":
                        case "9":
                        case "10":
                        case "11":
                        case "12":
                            operation.params.typeJurOrg = 10;
                            break;
                        default:
                            operation.params.typeJurOrg = 0;
                            break;
                    }
                }
            },
            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'ProtocolMhcRealityObject', {
                            objectIds: Ext.encode(recordIds),
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /**
            * Аспект взаимодействия Таблицы определений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'protocolMhcDefinitionAspect',
            gridSelector: '#protocolMhcDefinitionGrid',
            editFormSelector: '#protocolMhcDefinitionEditWindow',
            storeName: 'protocolmhc.Definition',
            modelName: 'protocolmhc.Definition',
            editWindowView: 'protocolmhc.DefinitionEditWindow',
            onSaveSuccess: function (asp, record) {
                this.setDefinitionId(record.getId());
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ProtocolMhc', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    this.setDefinitionId(record.getId());
                }
            },
            setDefinitionId: function (id) {
                this.controller.params.DefinitionId = id;
                if (id) {
                    this.controller.getAspect('protocolMhcDefinitionPrintAspect').loadReportStore();
                }
            }
        },
    ],

    init: function () {
        this.getStore('protocolmhc.Annex').on('beforeload', this.onBeforeLoad, this);
        this.getStore('protocolmhc.ArticleLaw').on('beforeload', this.onBeforeLoad, this);
        this.getStore('protocolmhc.RealityObject').on('beforeload', this.onBeforeLoad, this);
        this.getStore('protocolmhc.Definition').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('protocolMhcEditPanelAspect').setData(this.params.documentId);

            //Обновляем стор приложений
            this.getStore('protocolmhc.Annex').load();

            //Обновляем стор статьи закона
            this.getStore('protocolmhc.ArticleLaw').load();

            //Обновляем стор домов
            this.getStore('protocolmhc.RealityObject').load();
            
            //Обновляем стор определений
            this.getStore('protocolmhc.Definition').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});