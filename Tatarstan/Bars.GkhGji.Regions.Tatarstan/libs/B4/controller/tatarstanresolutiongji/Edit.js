Ext.define('B4.controller.tatarstanresolutiongji.Edit', {
        extend: 'B4.base.Controller',

        params: null,

        requires: [
            'B4.aspects.GjiDocument',
            'B4.aspects.GridEditWindow',
            'B4.aspects.StateContextButton',
            'B4.enums.TypeInitiativeOrgGji',
            'B4.enums.ResolutionAppealed',
            'B4.aspects.GkhInlineGrid',
            'B4.aspects.GkhButtonPrintAspect',
            'B4.aspects.permission.tatarstanresolutiongji.TatarstanResolutionGjiEdit',
            'B4.aspects.permission.tatarstanresolutiongji.FieldRequirement',
            'B4.aspects.permission.tatarstanresolutiongji.Annex',
            'B4.aspects.permission.tatarstanresolutiongji.Eyewitness',
            'B4.aspects.permission.tatarstanresolutiongji.Dispute',
            'B4.aspects.permission.tatarstanresolutiongji.Definition',
            'B4.aspects.permission.tatarstanresolutiongji.PayFine'
        ],

        mixins: {
            controllerLoader: 'B4.mixins.LayoutControllerLoader',
            mask: 'B4.mixins.MaskBody'
        },

        models: [
            'tatarstanresolutiongji.TatarstanResolutionGji',
            'tatarstanprotocolgji.TatarstanProtocolGjiAnnex',
            'tatarstanprotocolgji.TatarstanProtocolGjiEyewitness',
            'resolution.Dispute',
            'resolution.PayFine',
            'resolution.Definition'
        ],

        stores: [
            'tatarstanresolutiongji.TatarstanResolutionGji',
            'tatarstanprotocolgji.TatarstanProtocolGjiAnnex',
            'tatarstanprotocolgji.TatarstanProtocolGjiEyewitness',
            'resolution.Dispute',
            'resolution.PayFine',
            'resolution.Definition'
        ],

        views: [
            'tatarstanresolutiongji.EditPanel',
            'tatarstanprotocolgji.AnnexGrid',
            'tatarstanprotocolgji.AnnexEditWindow',
            'tatarstanresolutiongji.WitnessGrid',
            'resolution.DefinitionEditWindow',
            'resolution.DefinitionGrid',
            'resolution.PayFineGrid',
            'resolution.DisputeEditWindow',
            'resolution.DisputeGrid'
        ],

        mainView: 'tatarstanresolutiongji.EditPanel',
        mainViewSelector: 'tatarstanresolutiongjieditpanel',

        refs: [
            { ref: 'AnnexGrid', selector: '#resolutionAnnexGrid' },
            { ref: 'EditPanel', selector: 'tatarstanresolutiongjieditpanel' }
        ],

    aspects: [
            {
                xtype: 'tatarstanresolutiongjieditperm',
                editFormAspectName: 'tatarstanresolutiongjiEditPanelAspect'
            },
            {
                xtype: 'tatarstanresolutiongjifieldrequirement'
            },
            {
                xtype: 'tatarstanresolutiongjianexperm'
            },
            {
                xtype: 'tatarstanresolutiongjieyewittnessperm'
            },
            {
                xtype: 'tatarstanresolutiongjidisputeperm'
            },
            {
                xtype: 'tatarstanresolutiongjidefinitionperm'
            },
            {
                xtype: 'tatarstanresolutiongjipayfineperm'
            },
            {
                xtype: 'gjidocumentaspect',
                name: 'tatarstanresolutiongjiEditPanelAspect',
                editPanelSelector: 'tatarstanresolutiongjieditpanel',
                modelName: 'tatarstanresolutiongji.TatarstanResolutionGji',
                listeners: {
                    aftersetpaneldata: function(asp, rec, panel) {
                        asp.controller.getAspect('tatarstanresolutionjiStateButtonAspect')
                            .setStateData(rec.get('Id'), rec.get('State'));
                        asp.controller.getAspect('resolutionPrintAspect').loadReportStore();
                        asp.controller.updateCitizenField(panel, false);

                        //Делаем запросы на получение документа основания

                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('GetInfo', 'Resolution', {
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
                            asp.controller.unmask();
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);

                            var fieldBaseName = panel.down('[name=BasisDocumentName]');
                            fieldBaseName.setValue(obj.baseName);
                            asp.disableButtons(false);
                        }).error(function () {
                            asp.controller.unmask();
                        });

                        var citizenShipType = panel.down('[name=CitizenshipType]');
                        //подписка здесь, чтобы избежать обработки события при инициализации
                        citizenShipType.on('change', this.controller.citizenTypeChange, this.controller,
                            citizenShipType);
                    }
                }
            },
            {
                /*
                Вешаем аспект смены статуса в карточке редактирования
                */
                xtype: 'statecontextbuttonaspect',
                name: 'tatarstanresolutionjiStateButtonAspect',
                stateButtonSelector: 'tatarstanresolutiongjieditpanel [name=btnState]',
                listeners: {
                    transfersuccess: function(asp, entityId) {
                        //После успешной смены статуса запрашиваем по Id актуальные данные записи
                        asp.controller.getAspect('tatarstanresolutiongjiEditPanelAspect').setData(entityId);
                    }
                }
            },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'resolutionPrintAspect',
            buttonSelector: 'tatarstanresolutiongjieditpanel #btnPrint',
            codeForm: 'CourtResolution',
            getUserParams: function(reportId) {
                var param = {
                    DocumentId: this.controller.params.documentId
                };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'tatarstanresolutiongjieyewitnessgrid',
            storeName: 'tatarstanprotocolgji.TatarstanProtocolGjiEyewitness',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGjiEyewitness',
            saveButtonSelector: 'tatarstanresolutiongjieyewitnessgrid [name=btnSave]',
            listeners: {
                'beforesave': function(asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        documentId = asp.controller.params.documentId;

                    Ext.each(modifiedRecords, function(rec) {
                        rec.data.DocumentGji = documentId;
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
            name: 'tatarstanresolutiongjiAnnexAspect',
            gridSelector: '#resolutionAnnexGrid',
            editFormSelector: 'tatarstanprotocolgjiannexeditwindow',
            mainViewSelector: 'tatarstanresolutiongjieditpanel',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGjiAnnex',
            editWindowView: 'tatarstanprotocolgji.AnnexEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('DocumentGji', asp.controller.params.documentId);
                    }
                },

                validate: function(asp) {
                    var panel = Ext.ComponentQuery.query(this.mainViewSelector)[0];

                    return panel.hidden ? false : true;
                }
            },
            onSaveSuccess: function(aspect) {
                var form = aspect.getForm();
                if (form) {
                    form.close();
                }
                aspect.controller.getAnnexGrid().getStore().load();
            },
        },
        {
            /* 
            Аспект взаимодействия Таблицы определений с формой редактирования 
            */
            xtype: 'grideditwindowaspect',
            name: 'resolutionDefinitionAspect',
            gridSelector: 'resolutionDefinitionGrid',
            editFormSelector: '#resolutionDefinitionEditWindow',
            storeName: 'resolution.Definition',
            modelName: 'resolution.Definition',
            editWindowView: 'resolution.DefinitionEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution',
                            asp.controller.params.documentId);
                    }
                }
            }
        },
        {
            /* Аспект взаимодействия Таблицы оплаты штрафов с формой редактирования */
            xtype: 'gkhinlinegridaspect',
            name: 'resolutionPayFineAspect',
            storeName: 'resolution.PayFine',
            modelName: 'resolution.PayFine',
            gridSelector: 'resolutionPayFineGrid',
            saveButtonSelector: 'resolutionPayFineGrid #btnSaveResolutionPayFine',
            listeners: {
                beforesave: function(asp, store) {
                    var resolution = asp.controller.params.documentId;
                    store.each(function(record, index) {
                        if (!record.get('Id')) {
                            record.set('Resolution', resolution);
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
            gridSelector: 'resolutionDisputeGrid',
            editFormSelector: '#resolutionDisputeEditWindow',
            storeName: 'resolution.Dispute',
            modelName: 'resolution.Dispute',
            editWindowView: 'resolution.DisputeEditWindow',
            otherActions: function(actions) {
                actions[this.editFormSelector + ' #cbDisputeAppeal'] =
                    { 'change': { fn: this.onChangeDisputeAppeal, scope: this } };
            },
            onChangeDisputeAppeal: function(field, newValue, oldValue) {
                var form = this.getForm();
                if (newValue === B4.enums.ResolutionAppealed.Law) {
                    form.down('#cbDisputeCourt').setDisabled(false);
                    form.down('#cbDisputeInstance').setDisabled(false);
                } else {
                    form.down('#cbDisputeCourt').setDisabled(true);
                    form.down('#cbDisputeInstance').setDisabled(true);
                }
            },
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution',
                            asp.controller.params.documentId);
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'tatarstanresolutiongjieditpanel [name=tatarstanResolutionGjiTabPanel]': { 'tabchange': { fn: me.changeTab, scope: me } },
            'tatarstanresolutiongjieditpanel [name=Executant]': { 'change': { fn: me.executantChange, scope: me } },
            'tatarstanresolutiongjieditpanel [name=TypeInitiativeOrg]': { 'change': { fn: me.typeInitiativeOrgChange, scope: me } },
            'tatarstanresolutiongjieditpanel [name=Contragent]': { 'change': { fn: me.contragentChange, scope: me } },
            '#resolutionAnnexGrid': { 'annexstore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'tatarstanresolutiongjieyewitnessgrid': { 'eyewitnessstore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'tatarstanresolutiongjieditpanel [name=IdentityDocumentType]': { 'change': { fn: me.identityDocumentTypeChange, scope: me } },

        });
        
        me.getStore('resolution.PayFine').on('beforeload', me.onBeforeBaseResolutionStoreLoad, me);
        me.getStore('resolution.Dispute').on('beforeload', me.onBeforeBaseResolutionStoreLoad, me);
        me.getStore('resolution.Definition').on('beforeload', me.onBeforeBaseResolutionStoreLoad, me);

        me.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('tatarstanresolutiongjiEditPanelAspect').setData(this.params.documentId);
        }
    },

    onBeforeLoad: function (store, operation) {
        operation.params.documentId = this.params.documentId;
    },

    onBeforeBaseResolutionStoreLoad: function (store, operation) {
        operation.params.documentId = this.params.documentId;
    },

    changeTab: function (tabPanel, newTab, oldTab) {
        if (newTab.title === 'Реквизиты') {
            return;
        }
        var store = newTab.getStore();
        if (store) {
            store.load();
        }
    },

    identityDocumentTypeChange: function (selectedField, value) {

        var panel = selectedField.up('tatarstanresolutiongjieditpanel'),
            documentField = panel.down('[name=SerialAndNumberDocument]');

        documentField.regex = value && value.Regex ? Ext.decode(value.Regex) : null;
        documentField.regexText = value && value.RegexErrorMessage ? value.RegexErrorMessage : null;
    },

    executantChange: function (combobox, newValue) {
        var panel = combobox.up('tatarstanresolutiongjieditpanel'),
            contragentInfoFieldSet = panel.down('[name=ContragentInfoFieldSet]'),
            personalInfoFieldSet = panel.down('[name=PersonalInfoFieldSet]'),
            isIndividual = newValue === B4.enums.TypeExecutantProtocol.Individual,
            contragentField = panel.down('[name=Contragent]'),
            сitizenshipType = panel.down('[name=CitizenshipType]');

        contragentInfoFieldSet.setVisible(!isIndividual);
        personalInfoFieldSet.setTitle(isIndividual ? 'Протокол составлен в отношении' : 'Должностное/физическое лицо');
        contragentField.allowBlank = isIndividual || newValue === B4.enums.TypeExecutantProtocol.Official;
        сitizenshipType.allowBlank = !isIndividual;
        //обязательность поля контрагент
        this.updateAllowBlank(contragentField);
        //обязательность поля гражданство
        this.updateAllowBlank(сitizenshipType);
    },

    typeInitiativeOrgChange: function (combobox, newValue) {
        var panel = combobox.up('tatarstanresolutiongjieditpanel'),
            fineMunicipality = panel.down('[name=FineMunicipality]');

        fineMunicipality.setVisible(newValue === B4.enums.TypeInitiativeOrgGji.HousingInspection);
    },
    
    updateCitizenField: function (panel, needClearValue) {
        var citizenship = panel.down('[name=Citizenship]'),
            citizenshipType = panel.down('[name=CitizenshipType]');

        var isVisible = citizenshipType.getValue() === B4.enums.CitizenshipType.Other;

        citizenship.setVisible(isVisible);

        if (needClearValue) {
            citizenship.setValue(null);
        }
    },

    citizenTypeChange: function (combobox) {
        var panel = combobox.up('tatarstanresolutiongjieditpanel');
        this.updateCitizenField(panel, true);
    },

    updateAllowBlank: function (component) {
        !component.allowBlank && (component.getValue() == null || component.getValue() == undefined)
            ? component.markInvalid()
            : component.clearInvalid();
    },

    contragentChange: function (selectedField, value) {
        if (value !== null) {
            return;
        }
        var panel = selectedField.up('tatarstanresolutiongjieditpanel'),
            contragentInfoFieldSet = panel.down('[name=ContragentInfoFieldSet]');
        contragentInfoFieldSet.down('[name=Ogrn]').setValue(null);
        contragentInfoFieldSet.down('[name=Inn]').setValue(null);
        contragentInfoFieldSet.down('[name=Kpp]').setValue(null);
        contragentInfoFieldSet.down('[name=SettlementAccount]').setValue(null);
        contragentInfoFieldSet.down('[name=BankName]').setValue(null);
        contragentInfoFieldSet.down('[name=CorrAccount]').setValue(null);
        contragentInfoFieldSet.down('[name=Bik]').setValue(null);
        contragentInfoFieldSet.down('[name=Okpo]').setValue(null);
        contragentInfoFieldSet.down('[name=Okonh]').setValue(null);
        contragentInfoFieldSet.down('[name=Okved]').setValue(null);
    },
});