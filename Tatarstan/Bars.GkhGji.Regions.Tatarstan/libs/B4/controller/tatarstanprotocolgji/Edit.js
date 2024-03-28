Ext.define('B4.controller.tatarstanprotocolgji.Edit', {
    extend: 'B4.base.Controller',

    params: null,

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.enums.TypeExecutantProtocol',
        'B4.enums.CitizenshipType',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.tatarstanprotocolgji.TatarstanProtocolGjiEdit',
        'B4.aspects.permission.tatarstanprotocolgji.Annex',
        'B4.aspects.permission.tatarstanprotocolgji.ArticleLaw',
        'B4.aspects.permission.tatarstanprotocolgji.Eyewitness',
        'B4.aspects.permission.tatarstanprotocolgji.RealityObject',
        'B4.aspects.permission.tatarstanprotocolgji.Violation',
        'B4.aspects.permission.tatarstanprotocolgji.FieldRequirement',
        'B4.aspects.GjiDocumentCreateButton'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    models: [
        'tatarstanprotocolgji.TatarstanProtocolGji',
        'tatarstanprotocolgji.TatarstanProtocolGjiAnnex',
        'tatarstanprotocolgji.TatarstanProtocolGjiArticleLaw',
        'tatarstanprotocolgji.TatarstanProtocolGjiRealityObject',
        'tatarstanprotocolgji.TatarstanProtocolGjiEyewitness'
    ],

    stores: [
        'tatarstanprotocolgji.TatarstanProtocolGji',
        'tatarstanprotocolgji.TatarstanProtocolGjiAnnex',
        'tatarstanprotocolgji.TatarstanProtocolGjiArticleLaw',
        'tatarstanprotocolgji.TatarstanProtocolGjiRealityObject',
        'tatarstanprotocolgji.TatarstanProtocolGjiEyewitness'
        ],

    views: [
        'tatarstanprotocolgji.EditPanel',
        'tatarstanprotocolgji.AnnexGrid',
        'tatarstanprotocolgji.ArticleLawGrid',
        'tatarstanprotocolgji.RealityObjectGrid',
        'tatarstanprotocolgji.AnnexEditWindow',
        'tatarstanprotocolgji.ViolationGrid',
        'tatarstanprotocolgji.EyewitnessGrid'
    ],


    mainView: 'tatarstanprotocolgji.EditPanel',
    mainViewSelector: 'tatarstanprotocolgjieditpanel',

    refs: [
        { ref: 'ArticleLawGrid', selector: 'tatarstanprotocolgjiarticlelawgrid' },
        { ref: 'RealityObjectGrid', selector: 'tatarstanprotocolgjirealityobjectgrid' },
        { ref: 'AnnexGrid', selector: '#protocolAnnexGrid' },
        { ref: 'ViolationGrid', selector: 'tatarstanprotocolgjiviolationgrid' },
        { ref: 'EditPanel', selector: 'tatarstanprotocolgjieditpanel' }
    ],

    aspects: [
        {
            /*
            Аспект формирвоания документов для Протокола ГЖИ РТ
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'protocolGjiCreateButtonAspect',
            buttonSelector: 'tatarstanprotocolgjieditpanel gjidocumentcreatebutton',
            containerSelector: 'tatarstanprotocolgjieditpanel',
            typeDocument: 160, // Протокол по ст.20.6.1 КоАП РФ
        },
        {
            xtype: 'tatarstanprotocolgjieditstateperm',
            editFormAspectName: 'tatarstanprotocolgjiEditPanelAspect'
        },
        {
            xtype: 'tatarstanprotocolgjiarticlelawperm'
        },
        {
            xtype: 'tatarstanprotocolgjianexperm'
        },
        {
            xtype: 'tatarstanprotocolgjieyewittnessperm'
        },
        {
            xtype: 'tatarstanprotocolgjirealityobjperm'
        },
        {
            xtype: 'tatarstanprotocolgjiviolationperm'
        },
        {
            xtype: 'tatarstanprotocolgjifieldrequirement'
        },
        {
            xtype: 'gkheditpanel',
            name: 'tatarstanprotocolgjiEditPanelAspect',
            editPanelSelector: 'tatarstanprotocolgjieditpanel',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGji',
            listeners: {
                aftersetpaneldata: function(asp, rec, panel) {
                    this.controller.getAspect('tatarstanprotocolgjiStateButtonAspect')
                        .setStateData(rec.get('Id'), rec.get('State'));
                    this.controller.updateCitizenField(panel, false);
                    
                    this.controller.getAspect('protocolPrintAspect').loadReportStore();

                    var inTribunalCheckBox = panel.down('[name=IsInTribunal]'),
                        isInTribunal = inTribunalCheckBox.getValue(),
                        residencePetitionCheckBox = panel.down('[name=ResidencePetition]'),
                        residencePetition = residencePetitionCheckBox.getValue(),
                        citizenShipType = panel.down('[name=CitizenshipType]');
                    this.controller.isInTribunalChange(inTribunalCheckBox, isInTribunal);
                    this.controller.residencePetitionChange(residencePetitionCheckBox, residencePetition);

                    //подписка здесь, чтобы избежать обработки события при инициализации
                    citizenShipType.on('change', this.controller.citizenTypeChange, this.controller, citizenShipType);

                    B4.Ajax.request(B4.Url.action('GetInfo', 'TatarstanProtocolGji', {
                        protocolId: asp.controller.params.documentId
                        }))
                        .next(function(response) {
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText).data;
                            if (obj) {
                                var fieldInspectors = panel.down('[name=CheckInspectors]');
                                fieldInspectors.updateDisplayedText(obj.inspectorNames);
                                fieldInspectors.setValue(obj.inspectorIds);
                            }
                            asp.controller.unmask();
                        })
                        .error(function() {
                            asp.controller.unmask();
                        });

                    // обновляем кнопку Сформировать
                    this.controller.getAspect('protocolGjiCreateButtonAspect').setData(rec.get('Id'));
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'protocolPrintAspect',
            buttonSelector: 'tatarstanprotocolgjieditpanel #btnPrint',
            codeForm: 'TatProtocolGji',
            getUserParams: function(reportId) {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'tatarstanprotocolgjiStateButtonAspect',
            stateButtonSelector: 'tatarstanprotocolgjieditpanel [name=btnState]',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    asp.controller.getAspect('tatarstanprotocolgjiEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'tatarstanprotocolgjiarticlelawaspect',
            gridSelector: 'tatarstanprotocolgjiarticlelawgrid',
            storeName: 'tatarstanprotocolgji.TatarstanProtocolGjiArticleLaw',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGjiArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#tatarstanprotocolgjiarticlelawgridMultiSelectWindow',
            storeSelect: 'dict.ArticleLawGjiForSelect',
            storeSelected: 'dict.ArticleLawGjiForSelected',
            titleSelectWindow: 'Выбор статей закона',
            titleGridSelect: 'Статьи для отбора',
            titleGridSelected: 'Выбранные элементы',
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

                    if (recordIds.length === 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать статьи закона');
                        return false;
                    }

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('SaveArticles', 'TatarstanProtocolGjiArticleLaw'),
                        method: 'POST',
                        params: {
                            articleIds: Ext.encode(recordIds),
                            protocolId: asp.controller.params.documentId
                        }
                    }).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getArticleLawGrid().getStore().load();
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
            Аспект взаимодействия таблицы домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'tatarstanprotocolgjirealityobjectaspect',
            gridSelector: 'tatarstanprotocolgjirealityobjectgrid',
            storeName: 'tatarstanprotocolgji.TatarstanProtocolGjiRealityObject',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGjiRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#tatarstanProtocolGjiRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds.length === 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('SaveRealityObjects', 'TatarstanProtocolGjiRealityObject'),
                        method: 'POST',
                        params: {
                            roIds: Ext.encode(recordIds),
                            protocolId: asp.controller.params.documentId
                        }
                    }).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getRealityObjectGrid().getStore().load();
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
            Аспект взаимодействия таблицы домов с массовой формой выбора нарушений
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'tatarstanprotocolgjiviolationaspect',
            gridSelector: 'tatarstanprotocolgjiviolationgrid',
            storeName: 'tatarstanprotocolgji.TatarstanProtocolGjiViolation',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGjiViolation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#tatarstanProtocolGjiViolationMultiSelectWindow',
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'Пункт нормативно-правового документа', xtype: 'gridcolumn', dataIndex: 'NormDocNum', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Пункт нормативно-правового документа', xtype: 'gridcolumn', dataIndex: 'NormDocNum', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds.length === 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('SaveViolations', 'TatarstanProtocolGjiViolation'),
                        method: 'POST',
                        params: {
                            violationIds: Ext.encode(recordIds),
                            protocolId: asp.controller.params.documentId
                        }
                    }).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getViolationGrid().getStore().load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            },

            onBeforeLoad: function(store, operation) {
                operation.params.isForTatarstanGjiSelect = true;
            },
        },
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'tatarstanprotocolgjiAnnexAspect',
            gridSelector: '#protocolAnnexGrid',
            editFormSelector: 'tatarstanprotocolgjiannexeditwindow',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGjiAnnex',
            mainViewSelector: 'tatarstanprotocolgjieditpanel',
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
            onSaveSuccess: function (aspect) {
                var form = aspect.getForm();
                if (form) {
                    form.close();
                }
                aspect.controller.getAnnexGrid().getStore().load();
            },
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'tatarstanProtocolGjiInspectorMultiSelectWindowAspect',
            fieldSelector: 'tatarstanprotocolgjieditpanel [name=CheckInspectors]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#tatarstanProtocolGjiCheckInspectorSelectWindow',
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

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);
                    var protocolId = asp.controller.params.documentId;
                    
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: protocolId
                    })).next(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        asp.controller.getAspect('tatarstanprotocolgjiEditPanelAspect').setData(protocolId);
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'tatarstanprotocolgjieyewitnessgrid',
            storeName: 'tatarstanprotocolgji.TatarstanProtocolGjiEyewitness',
            modelName: 'tatarstanprotocolgji.TatarstanProtocolGjiEyewitness',
            saveButtonSelector: 'tatarstanprotocolgjieyewitnessgrid [name=btnSave]',
            listeners: {
                'beforesave': function (asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        protocolId = asp.controller.params.documentId;

                    Ext.each(modifiedRecords, function (rec) {
                        rec.data.DocumentGji = protocolId;
                    });

                    return true;
                }
            },
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'tatarstanprotocolgjiarticlelawgrid': { 'articlelawstore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'tatarstanprotocolgjirealityobjectgrid': { 'realityobjectstore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'tatarstanprotocolgjiviolationgrid': { 'violationstore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            '#protocolAnnexGrid': { 'annexstore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'tatarstanprotocolgjieyewitnessgrid': { 'eyewitnessstore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'tatarstanprotocolgjieditpanel [name=tatarstanProtocolGjiTabPanel]': { 'tabchange': { fn: me.changeTab, scope: me } },
            'tatarstanprotocolgjieditpanel [name=Executant]': { 'change': { fn: me.executantChange, scope: me } },
            'tatarstanprotocolgjieditpanel [name=IsInTribunal]': { 'change': { fn: me.isInTribunalChange, scope: me } },
            'tatarstanprotocolgjieditpanel [name=ResidencePetition]': { 'change': { fn: me.residencePetitionChange, scope: me } },
            'tatarstanprotocolgjieditpanel [name=Contragent]': { 'change': { fn: me.contragentChange, scope: me } },
            'tatarstanprotocolgjieditpanel [name=btnCancel]': { 'click': { fn: me.btnCancelClick, scope: me } },
            'tatarstanprotocolgjieditpanel [name=IdentityDocumentType]': { 'change': { fn: me.identityDocumentTypeChange, scope: me } },

        });
        
        me.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('tatarstanprotocolgjiEditPanelAspect').setData(this.params.documentId);
        }
    },

    onBeforeLoad: function (store, operation) {
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

    btnCancelClick: function () {
        this.getAspect('tatarstanprotocolgjiEditPanelAspect')
            .setData(this.params.documentId);
    },

    contragentChange: function (selectedField, value) {
        if (value !== null) {
            return;
        }
        var panel = selectedField.up('tatarstanprotocolgjieditpanel'),
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

    identityDocumentTypeChange: function (selectedField, value) {
       
        var panel = selectedField.up('tatarstanprotocolgjieditpanel'),
            documentField = panel.down('[name=SerialAndNumberDocument]');

        documentField.regex = value && value.Regex ? Ext.decode(value.Regex) : null;
        documentField.regexText = value && value.RegexErrorMessage ? value.RegexErrorMessage : null;
    },

    executantChange: function (combobox, newValue) {
        var panel = combobox.up('tatarstanprotocolgjieditpanel'),
            contragentInfoFieldSet = panel.down('[name=ContragentInfoFieldSet]'),
            personalInfoFieldSet = panel.down('[name=PersonalInfoFieldSet]'),
            isIndividual = newValue === B4.enums.TypeExecutantProtocol.Individual,
            contragentField = panel.down('[name=Contragent]'),
            сitizenshipType = panel.down('[name=CitizenshipType]'),
            violationTab = panel.down('tatarstanprotocolgjiviolationgrid');

        contragentInfoFieldSet.setVisible(!isIndividual);
        violationTab.setDisabled(isIndividual);
        personalInfoFieldSet.setTitle(isIndividual ? 'Протокол составлен в отношении' : 'Должностное/физическое лицо');
        contragentField.allowBlank = isIndividual || newValue === B4.enums.TypeExecutantProtocol.Official;
        сitizenshipType.allowBlank = !isIndividual;
        //обязательность поля контрагент
        this.updateAllowBlank(contragentField);
        //обязательность поля гражданство
        this.updateAllowBlank(сitizenshipType);
    },

    updateAllowBlank: function(component) {
        !component.allowBlank && (component.getValue() == null || component.getValue() == undefined)
            ? component.markInvalid()
            : component.clearInvalid();
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
        var panel = combobox.up('tatarstanprotocolgjieditpanel');
        this.updateCitizenField(panel, true);
    },

    isInTribunalChange: function(checkbox, newValue) {
        var panel = checkbox.up('tatarstanprotocolgjieditpanel'),
            tribunalName = panel.down('[name=TribunalName]');
        tribunalName.setVisible(newValue);
    },

    residencePetitionChange: function (checkbox, newValue) {
        var panel = checkbox.up('tatarstanprotocolgjieditpanel'),
            offenseAddress = panel.down('[name=OffenseAddress]');
        offenseAddress.setVisible(newValue);
    },
});