Ext.define('B4.controller.protocolmvd.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,

    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ProtocolMvdState',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton'
    ],

    models: [
        'ProtocolMvd',
        'Resolution',
        'protocolmvd.Annex',
        'protocolmvd.ArticleLaw',
        'protocolmvd.RealityObject'
    ],

    stores: [
        'ProtocolMvd',
        'protocolmvd.Annex',
        'protocolmvd.ArticleLaw',
        'protocolmvd.RealityObject',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.ExecutantDocGji',
        'dict.Municipality',
        'Contragent'
    ],

    views: [
        'protocolmvd.EditPanel',
        'protocolmvd.AnnexEditWindow',
        'protocolmvd.AnnexGrid',
        'protocolmvd.ArticleLawGrid',
        'protocolmvd.RealityObjectGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'protocolmvd.EditPanel',
    mainViewSelector: '#protocolMvdEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Протокол МВД
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'protocolMvdCreateButtonAspect',
            buttonSelector: '#protocolMvdEditPanel gjidocumentcreatebutton',
            containerSelector: '#protocolMvdEditPanel',
            typeDocument: 120 // Тип документа Протокол МВД
        },
        {
            xtype: 'protocolmvdstateperm',
            editFormAspectName: 'protocolMvdEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'protocolMvdStateButtonAspect',
            stateButtonSelector: '#protocolMvdEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('protocolMvdEditPanelAspect').setData(entityId);
                }
            }
        },
        {   /*
            Апект для основной панели Протокола МВД
            */
            xtype: 'gjidocumentaspect',
            name: 'protocolMvdEditPanelAspect',
            editPanelSelector: '#protocolMvdEditPanel',
            modelName: 'ProtocolMvd',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this} };
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
                    panel.setTitle('Протокол МВД ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Протокол МВД');

                panel.down('#protocolMvdTabPanel').setActiveTab(0);

                this.disableButtons(false);

                this.setTypeExecutantPermission(rec.data.TypeExecutant);

                //Обновляем статусы
                this.controller.getAspect('protocolMvdStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем кнопку Сформирвоать
                this.controller.getAspect('protocolMvdCreateButtonAspect').setData(rec.get('Id'));
            },

            setTypeExecutantPermission: function (typeExec) {
                var me = this;
                var panel = this.getPanel();
                var permissions = [
                    'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_Edit',
                    'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_Edit'
                ];

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                        permissions: Ext.encode(permissions),
                        ids: Ext.encode([me.controller.params.documentId])
                    })).next(function (response) {
                        me.controller.unmask();
                        var perm = Ext.decode(response.responseText)[0];
                        switch (typeExec) {
                            case 10:
                                panel.down('#tfPhysPerson').setDisabled(!perm[0]);
                                panel.down('#taPhysPersonInfo').setDisabled(!perm[1]);
                                break;
                        }
                    }).error(function () {
                        me.controller.unmask();
                    });
                }
            },

            onChangeTypeExecutant: function (field, value, oldValue) {
                if (value) {
                    if (this.controller.params) {
                        this.controller.params.typeExecutant = value;
                    }
                    this.setTypeExecutantPermission(value);
                }
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
            name: 'protocolMvdAnnexAspect',
            gridSelector: '#protocolMvdAnnexGrid',
            editFormSelector: '#protocolMvdAnnexEditWindow',
            storeName: 'protocolmvd.Annex',
            modelName: 'protocolmvd.Annex',
            editWindowView: 'protocolmvd.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ProtocolMvd', this.controller.params.documentId);
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
            name: 'protocolMvdArticleLawAspect',
            gridSelector: '#protocolMvdArticleLawGrid',
            saveButtonSelector: '#protocolMvdArticleLawGrid #btnSaveArticles',
            storeName: 'protocolmvd.ArticleLaw',
            modelName: 'protocolmvd.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolMvdArticleLawMultiSelectWindow',
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
                            url: B4.Url.action('AddArticles', 'ProtocolMvdArticleLaw'),
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
            name: 'protocolMvdRealityObjectAspect',
            gridSelector: '#protocolMvdRealityObjectGrid',
            storeName: 'protocolmvd.RealityObject',
            modelName: 'protocolmvd.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolMvdRealityObjectMultiSelectWindow',
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
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'ProtocolMvdRealityObject', {
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
        }
    ],

    init: function () {
        this.getStore('protocolmvd.Annex').on('beforeload', this.onBeforeLoad, this);
        this.getStore('protocolmvd.ArticleLaw').on('beforeload', this.onBeforeLoad, this);
        this.getStore('protocolmvd.RealityObject').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('protocolMvdEditPanelAspect').setData(this.params.documentId);

            //Обновляем стор приложений
            this.getStore('protocolmvd.Annex').load();

            //Обновляем стор статьи закона
            this.getStore('protocolmvd.ArticleLaw').load();

            //Обновляем стор домов
            this.getStore('protocolmvd.RealityObject').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});