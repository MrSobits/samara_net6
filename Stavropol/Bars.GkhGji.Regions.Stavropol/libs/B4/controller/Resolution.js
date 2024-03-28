Ext.define('B4.controller.Resolution', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.Resolution',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'Resolution',
        'ProtocolGji',
        'Presentation',
        'resolution.Annex',
        'resolution.Dispute',
        'resolution.PayFine',
        'resolution.Definition'
    ],

    stores: [
        'Resolution',
        'resolution.Annex',
        'resolution.Definition',
        'resolution.Dispute',
        'resolution.PayFine',
        'dict.ExecutantDocGji',
        'dict.Municipality',
        'dict.Inspector',
        'dict.SanctionGji'
    ],

    views: [
        'resolution.EditPanel',
        'resolution.AnnexEditWindow',
        'resolution.AnnexGrid',
        'resolution.DefinitionEditWindow',
        'resolution.DefinitionGrid',
        'resolution.PayFineGrid',
        'resolution.DisputeEditWindow',
        'resolution.DisputeGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'resolution.EditPanel',
    mainViewSelector: '#resolutionEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Постановления
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'resolutionCreateButtonAspect',
            buttonSelector: '#resolutionEditPanel gjidocumentcreatebutton',
            containerSelector: '#resolutionEditPanel',
            typeDocument: 70 // Тип документа Постановление
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'resolutionStateButtonAspect',
            stateButtonSelector: '#resolutionEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('resolutionEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            xtype: 'resolutionperm',
            editFormAspectName: 'resolutionEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'resolutionPrintAspect',
            buttonSelector: '#resolutionEditPanel #btnPrint',
            codeForm: 'Resolution',
            getUserParams: function (reportId) {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'resolutionDefinitionPrintAspect',
            buttonSelector: '#resolutionDefinitionEditWindow #btnPrint',
            codeForm: 'ResolutionDefinition',
            getUserParams: function (reportId) {
                var param = { DefinitionId: this.controller.params.DefinitionId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            * Апект для основной панели постановления
            */
            xtype: 'gjidocumentaspect',
            name: 'resolutionEditPanelAspect',
            editPanelSelector: '#resolutionEditPanel',
            modelName: 'Resolution',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this } };
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this} };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' combobox[name=TypeInitiativeOrg]'] = { 'change': { fn: this.onChangeTypeInitiativeOrg, scope: this } };
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
                    panel.setTitle('Постановление ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Постановление');

                //ставим активной вкладку "реквизиты"
                this.getPanel().down('.tabpanel').setActiveTab(0);

                //Делаем запросы на получение документа основания
                
                me.controller.mask('Загрузка', me.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'Resolution', {
                    documentId: asp.controller.params.documentId
                })).next(function (response) {
                    me.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldBaseName = panel.down('#tfBaseName');
                    fieldBaseName.setValue(obj.baseName);
                    me.disableButtons(false);
                }).error(function () {
                    me.controller.unmask();
                });

                this.setTypeExecutantPermission(rec.get('Executant'));
                
                //Передаем аспекту смены статуса необходимые параметры
                this.controller.getAspect('resolutionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                //загружаем стор для кнопки печати
                this.controller.getAspect('resolutionPrintAspect').loadReportStore();
                
                // обновляем кнопку Сформирвоать
                this.controller.getAspect('resolutionCreateButtonAspect').setData(rec.get('Id'));
            },

            setTypeExecutantPermission: function (typeExec) {
                var me = this,
                    panel = me.getPanel(),
                    permissions = [
                        'GkhGji.DocumentsGji.Resolution.Field.Contragent_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.PhysicalPerson_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.PhysicalPersonInfo_Edit'
                    ];

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('GetObjectSpecificPermissions', 'Permission'),
                        method: 'POST',
                        params: {
                            permissions: Ext.encode(permissions),
                            ids: Ext.encode([me.controller.params.documentId])
                        }
                    }).next(function (response) {
                        var perm = Ext.decode(response.responseText)[0];
                        
                        me.controller.unmask();
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

            onChangeTypeInitiativeOrg: function (field, value) {

                var cmp = field.up('panel').down('#tsfFineMunicipality');

                if (value === 10) {
                    cmp.allowBlank = false;
                    cmp.show();
                } else {
                    cmp.allowBlank = true;
                    cmp.hide();
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

            onBeforeLoadOfficial: function (field, options, store) {
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
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'resolutionAnnexAspect',
            gridSelector: '#resolutionAnnexGrid',
            editFormSelector: '#resolutionAnnexEditWindow',
            storeName: 'resolution.Annex',
            modelName: 'resolution.Annex',
            editWindowView: 'resolution.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /* 
            Аспект взаимодействия Таблицы определений с формой редактирования 
            */
            xtype: 'grideditwindowaspect',
            name: 'resolutionDefinitionAspect',
            gridSelector: '#resolutionDefinitionGrid',
            editFormSelector: '#resolutionDefinitionEditWindow',
            storeName: 'resolution.Definition',
            modelName: 'resolution.Definition',
            editWindowView: 'resolution.DefinitionEditWindow',
            onSaveSuccess: function (asp, record) {
                asp.setDefinitionId(record.getId());
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    asp.setDefinitionId(record.getId());

                    var numTxtFld = form.down('textfield[name=DocumentNum]'),
                        numberTxtFld = form.down('numberfield[name=DocumentNumber]');
                    if (!numTxtFld.isHidden() && !numberTxtFld.isHidden()) {
                        numTxtFld.hide();
                    }
                }
            },
            setDefinitionId: function (id) {
                this.controller.params.DefinitionId = id;
                if (id) {
                    this.controller.getAspect('resolutionDefinitionPrintAspect').loadReportStore();
                }
            }
        },
        {
            /* Аспект взаимодействия Таблицы оплаты штрафов с формой редактирования */
            xtype: 'gkhinlinegridaspect',
            name: 'resolutionPayFineAspect',
            storeName: 'resolution.PayFine',
            modelName: 'resolution.PayFine',
            gridSelector: '#resolutionPayFineGrid',
            saveButtonSelector: '#resolutionPayFineGrid #btnSaveResolutionPayFine',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (record, index) {
                        if (!record.get('Id')) {
                            record.set('Resolution', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            /* Аспект взаимодействия Таблицы оспариваний с формой редактирования */
            xtype: 'grideditwindowaspect',
            name: 'resolutionDisputeAspect',
            gridSelector: '#resolutionDisputeGrid',
            editFormSelector: '#resolutionDisputeEditWindow',
            storeName: 'resolution.Dispute',
            modelName: 'resolution.Dispute',
            editWindowView: 'resolution.DisputeEditWindow',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbDisputeAppeal'] = { 'change': { fn: this.onChangeDisputeAppeal, scope: this} };
            },
            onChangeDisputeAppeal: function (field, newValue, oldValue) {
                var form = this.getForm();
                if (newValue == 40) {
                    form.down('#cbDisputeCourt').setDisabled(false);
                    form.down('#cbDisputeInstance').setDisabled(false);
                } else {
                    form.down('#cbDisputeCourt').setDisabled(true);
                    form.down('#cbDisputeInstance').setDisabled(true);
                }
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution', this.controller.params.documentId);
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('resolution.PayFine').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.Dispute').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.Annex').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.Definition').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('resolutionEditPanelAspect').setData(this.params.documentId);

            //Обновляем стор оплат штрафов
            this.getStore('resolution.PayFine').load();

            //Обновляем стор приложений
            this.getStore('resolution.Annex').load();

            //Обновляем стор оспариваний
            this.getStore('resolution.Dispute').load();

            //Обновляем стор определений
            this.getStore('resolution.Definition').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});