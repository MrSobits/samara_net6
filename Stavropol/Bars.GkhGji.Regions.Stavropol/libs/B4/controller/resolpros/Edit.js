Ext.define('B4.controller.resolpros.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,

    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ResolProsState',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    models: [
        'ResolPros',
        'Resolution',
        'resolpros.Annex',
        'resolpros.ArticleLaw',
        'resolpros.RealityObject',
        'resolpros.Definition'
    ],

    stores: [
        'ResolPros',
        'resolpros.Annex',
        'resolpros.ArticleLaw',
        'resolpros.RealityObject',
        'resolpros.Definition',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.ExecutantDocGji',
        'dict.Municipality',
        'Contragent'
    ],

    views: [
        'resolpros.EditPanel',
        'resolpros.AnnexEditWindow',
        'resolpros.AnnexGrid',
        'resolpros.ArticleLawGrid',
        'resolpros.RealityObjectGrid',
        'resolpros.DefinitionEditWindow',
        'resolpros.DefinitionGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'resolpros.EditPanel',
    mainViewSelector: '#resolProsEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Постановление прокуратуры
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'resolProsCreateButtonAspect',
            buttonSelector: '#resolProsEditPanel gjidocumentcreatebutton',
            containerSelector: '#resolProsEditPanel',
            typeDocument: 80 // Тип документа Постановление прокуратуры
        },
        {
            xtype: 'resolprosstateperm',
            editFormAspectName: 'resolProsEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'resolProsStateButtonAspect',
            stateButtonSelector: '#resolProsEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('resolProsEditPanelAspect').setData(entityId);
                }
            }
        },
        {   /*
            Апект для основной панели Постановления прокуратуры
            */
            xtype: 'gjidocumentaspect',
            name: 'resolProsEditPanelAspect',
            editPanelSelector: '#resolProsEditPanel',
            modelName: 'ResolPros',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this} };
                actions[this.editPanelSelector + ' #sfContragent'] = {
                    'beforeload': { fn: this.onBeforeLoadContragent, scope: this },
                    'change': { fn: this.onChangeContragent, scope: this }
                };
                actions[this.editPanelSelector + ' #actCheckSelectField'] = {
                    'beforeload': { fn: this.onBeforeLoadActCheck, scope: this },
                    'change': { fn: this.onChangeActCheck, scope: this }
                };
            },

            //Данный метод перекрываем для того чтобы вместо целого объекта ActCheck
            // передать только Id на сохранение, поскольку если на сохранение уйдет ActCheck целиком,
            //то это поле тоже сохраниться и поля для записи ActCheck будут потеряны
            getRecordBeforeSave: function (record) {

                var act = record.get('ActCheck');
                if (act && act.Id > 0) {
                    record.set('ActCheck', act.Id);
                }

                return record;
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
                    panel.setTitle('Постановление прокуратуры ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Постановление прокуратуры');

                panel.down('#resolprosTabPanel').setActiveTab(0);
                
                this.disableButtons(false);

                this.setTypeExecutantPermission(rec.get('Executant'));
                
                //Обновляем статусы
                this.controller.getAspect('resolProsStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                // обновляем кнопку Сформирвоать
                this.controller.getAspect('resolProsCreateButtonAspect').setData(rec.get('Id'));
            },

            setTypeExecutantPermission: function (typeExec) {
                var me = this;
                var panel = this.getPanel();
                var permissions = [
                    'GkhGji.DocumentsGji.ResolPros.Field.Contragent_Edit',
                    'GkhGji.DocumentsGji.ResolPros.Field.PhysicalPerson_Edit',
                    'GkhGji.DocumentsGji.ResolPros.Field.PhysicalPersonInfo_Edit'
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

            onChangeContragent: function (field, newValue, oldValue) {
                var actCheckSf = this.getPanel().down('#actCheckSelectField');
                if (newValue) {
                    actCheckSf.readOnly = false;
                    this.controller.params.contragentId = newValue.Id;
                } else {
                    actCheckSf.readOnly = true;
                    this.controller.params.contragentId = 0;
                }
                actCheckSf.reset();
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

            onChangeActCheck: function (field, data) {
                if (data) {
                    var dateStr = '';
                    if (data.DocumentDate) {
                        var date = new Date(data.DocumentDate);
                        dateStr = date.toLocaleDateString();
                    }

                    field.updateDisplayedText('№' + data.DocumentNumber + ' от ' + dateStr);
                } else {
                    field.updateDisplayedText('');
                }
            },

            onBeforeLoadActCheck: function (field, options, store) {

                if (this.controller.params) {
                    options = options || {};
                    options.params = options.params || {};

                    options.params.actCheckForContragentId = this.controller.params.contragentId;
                }
                else {
                    Ext.Msg.alert('Внимание!', 'Необходимо выбрать контрагента');
                    return false;
                }

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
            name: 'resolProsAnnexAspect',
            gridSelector: '#resolProsAnnexGrid',
            editFormSelector: '#resolProsAnnexEditWindow',
            storeName: 'resolpros.Annex',
            modelName: 'resolpros.Annex',
            editWindowView: 'resolpros.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ResolPros', this.controller.params.documentId);
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
            name: 'resolProsArticleLawAspect',
            gridSelector: '#resolProsArticleLawGrid',
            saveButtonSelector: '#protocolgjiArticleLawGrid #btnSaveArticles',
            storeName: 'resolpros.ArticleLaw',
            modelName: 'resolpros.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#resolProsArticleLawMultiSelectWindow',
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
                            url: B4.Url.action('AddArticles', 'ResolProsArticleLaw'),
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
            name: 'resolProsRealityObjectAspect',
            gridSelector: '#resolProsRealityObjectGrid',
            storeName: 'resolpros.RealityObject',
            modelName: 'resolpros.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#resolProsRealityObjectMultiSelectWindow',
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
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'ResolProsRealityObject', {
                            objectIds: recordIds,
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
            xtype: 'gkhbuttonprintaspect',
            name: 'resolprosDefinitionPrintAspect',
            buttonSelector: '#resolprosDefinitionEditWindow #btnPrint',
            codeForm: 'ResolProsDefinition',
            getUserParams: function (reportId) {
                var me = this,
                    param = { DefinitionId: me.controller.params.DefinitionId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /**
            * Аспект взаимодействия Таблицы определений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'resolprosDefinitionAspect',
            gridSelector: '#resolprosDefinitionGrid',
            editFormSelector: '#resolprosDefinitionEditWindow',
            storeName: 'resolpros.Definition',
            modelName: 'resolpros.Definition',
            editWindowView: 'resolpros.DefinitionEditWindow',
            onSaveSuccess: function (asp, record) {
                this.setDefinitionId(record.getId());
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ResolPros', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    this.setDefinitionId(record.getId());

                    var numTxtFld = form.down('textfield[name=DocumentNum]'),
                        numberTxtFld = form.down('numberfield[name=DocumentNumber]');
                    if (!numTxtFld.isHidden() && !numberTxtFld.isHidden()) {
                        numTxtFld.hide();
                    }
                }
            },
            setDefinitionId: function (id) {
                var me = this;

                me.controller.params.DefinitionId = id;
                if (id) {
                    me.controller.getAspect('resolprosDefinitionPrintAspect').loadReportStore();
                }
            }
        }
    ],

    init: function () {
        this.getStore('resolpros.Annex').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolpros.ArticleLaw').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolpros.RealityObject').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolpros.Definition').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('resolProsEditPanelAspect').setData(this.params.documentId);

            //Обновляем стор приложений
            this.getStore('resolpros.Annex').load();

            //Обновляем стор статьи закона
            this.getStore('resolpros.ArticleLaw').load();

            //Обновляем стор домов
            this.getStore('resolpros.RealityObject').load();

            //Обновляем стор определений
            this.getStore('resolpros.Definition').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});