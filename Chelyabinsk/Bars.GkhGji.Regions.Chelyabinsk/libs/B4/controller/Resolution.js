Ext.define('B4.controller.Resolution', {
    extend: 'B4.base.Controller',
    params: null,
    resolutionDecision: null,
    objectId: 0,
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.view.resolution.TabTextEditWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.permission.Resolution',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.enums.TypeExecutantProtocolMvd',
        'B4.Ajax',
        'B4.aspects.GkhBlobText',
        'B4.Url',
        'B4.enums.YesNoNotSet',
        'B4.enums.ResolutionPaymentStatus'
    ],

    models: [
        'Resolution',
        'ProtocolGji',
        'Presentation',
        'resolution.ArtLaw',
        'resolution.Fiz',
        'resolution.Annex',
        'resolution.Decision',
        'resolution.Dispute',
        'resolution.PayFine',
        'resolution.Definition',
        'dict.PhysicalPersonDocType'
    ],

    stores: [
        'Resolution',
        'resolution.Annex',
        'resolution.ArtLaw',
        'resolution.Fiz',
        'resolution.Definition',
        'resolution.Dispute',
        'resolution.Decision',
        'resolution.PayFine',
        'dict.ExecutantDocGji',
        'dict.Municipality',
        'dict.Inspector',
        'dict.SanctionGji',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.PhysicalPersonDocType'
    ],

    views: [
        'resolution.EditPanel',
        'resolution.AnnexEditWindow',
        'resolution.AnnexGrid',
        'resolution.FizGrid',
        'resolution.FizEditWindow',
        'resolution.DefinitionEditWindow',
        'resolution.DefinitionGrid',
        'resolution.DecisionEditWindow',
        'resolution.PayFineGrid',
        'resolution.PayFineEditWindow',
        'resolution.TabTextEditWindow',
        'resolution.DisputeEditWindow',
        'resolution.DisputeGrid',
        'resolution.DecisionGrid',
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
            xtype: 'gkhblobtextaspect',
            name: 'resolutionEstablishedAspect',
            fieldSelector: '[name=Established]',
            editPanelAspectName: 'resolutionEditPanelAspect',
            controllerName: 'Resolution',
            valueFieldName: 'Established',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
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
                //actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this } };
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' combobox[name=TypeInitiativeOrg]'] = { 'change': { fn: this.onChangeTypeInitiativeOrg, scope: this } };
                actions[this.editPanelSelector + ' #cbWrittenOff'] = { 'change': { fn: this.onChangeWrittenOff, scope: this } };

            },

            onChangeWrittenOff: function (field, value) {

                var taWrittenOffComment = field.up('panel').down('#taWrittenOffComment');            

                if (value === true) {
                    taWrittenOffComment.show();
                }
                else {
                    taWrittenOffComment.hide();
                }
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this;
                this.controller.getAspect('resolutionEstablishedAspect').doInjection();
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
                this.setProtocolMvdPermission(rec.get('ProtocolMvdId'));

                //Передаем аспекту смены статуса необходимые параметры
                this.controller.getAspect('resolutionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                //загружаем стор для кнопки печати
                this.controller.getAspect('resolutionPrintAspect').loadReportStore();

                // обновляем кнопку Сформирвоать
                this.controller.getAspect('resolutionCreateButtonAspect').setData(rec.get('Id'));

           
            },

            setTypeExecutantPermission: function (typeExec) {
                var me = this,
                    panel = me.getPanel().down('resolutionRequisitePanel'),
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

                            //Активны все поля (ДЛ)
                            case "1":
                            case "3":
                            case "5":
                            case "11":
                            case "13":
                            case "16":
                            case "18":
                            case "19":
                                panel.down('#sfContragent').setDisabled(!perm[0]);
                                panel.down('#tfSurname').setDisabled(!perm[1]);
                                panel.down('#tfFirstName').setDisabled(!perm[1]);
                                panel.down('#tfPatronymic').setDisabled(!perm[1]);
                                panel.down('#tfPosition').setDisabled(!perm[0]);
                                panel.down('#dfPhysicalPersonDocType').setDisabled(!perm[1]);
                                panel.down('#dfPhysicalPersonDocumentSerial').setDisabled(!perm[1]);
                                panel.down('#dfPhysicalPersonDocumentNumber').setDisabled(!perm[1]);
                                panel.down('#dfPhysicalPersonIsNotRF').setDisabled(!perm[1]);

                                panel.down('#dfPersonBirthDate').setDisabled(false);
                                panel.down('#tfPersonBirthPlace').setDisabled(false);
                                panel.down('#tfRegistrationAddress').setDisabled(false);
                                panel.down('#tfFactAddress').setDisabled(false);

                                panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                                panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);
                                panel.down('#sfContragent').allowBlank = false;
                                panel.down('#tfSurname').allowBlank = false;
                                panel.down('#tfFirstName').allowBlank = false;
                                panel.down('#tfPatronymic').allowBlank = false;
                                panel.down('#tfPosition').allowBlank = false;
                                panel.down('#dfPhysicalPersonDocumentSerial').allowBlank = true;
                                panel.down('#dfPhysicalPersonDocumentNumber').allowBlank = true;
                                panel.down('#dfPhysicalPersonDocType').allowBlank = true;

                                panel.down('#dfPersonBirthDate').allowBlank = true;
                                panel.down('#tfPersonBirthPlace').allowBlank = true;
                                panel.down('#tfRegistrationAddress').allowBlank = true;
                                panel.down('#tfFactAddress').allowBlank = true;

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
                                panel.down('#tfSurname').setDisabled(true);
                                panel.down('#tfFirstName').setDisabled(true);
                                panel.down('#tfPatronymic').setDisabled(true);
                                panel.down('#tfPosition').setDisabled(true);
                                panel.down('#dfPhysicalPersonDocType').setDisabled(true);
                                panel.down('#dfPhysicalPersonDocumentSerial').setDisabled(true);
                                panel.down('#dfPhysicalPersonDocumentNumber').setDisabled(true);
                                panel.down('#dfPhysicalPersonIsNotRF').setDisabled(true);

                                panel.down('#dfPersonBirthDate').setDisabled(true);
                                panel.down('#tfPersonBirthPlace').setDisabled(true);
                                panel.down('#tfRegistrationAddress').setDisabled(true);
                                panel.down('#tfFactAddress').setDisabled(true);

                                panel.down('#tfPhysPerson').setDisabled(true);
                                panel.down('#taPhysPersonInfo').setDisabled(true);
                                panel.down('#tfSurname').setValue(null);
                                panel.down('#tfFirstName').setValue(null);
                                panel.down('#tfPatronymic').setValue(null);
                                panel.down('#tfPosition').setValue(null);
                                panel.down('#dfPhysicalPersonDocumentSerial').setValue(null);
                                panel.down('#dfPhysicalPersonDocumentNumber').setValue(null);
                                panel.down('#dfPhysicalPersonDocType').setValue(null);
                                panel.down('#sfContragent').allowBlank = false;
                                panel.down('#tfSurname').allowBlank = true;
                                panel.down('#tfFirstName').allowBlank = true;
                                panel.down('#tfPatronymic').allowBlank = true;
                                panel.down('#tfPosition').allowBlank = true;
                                panel.down('#dfPhysicalPersonDocumentSerial').allowBlank = true;
                                panel.down('#dfPhysicalPersonDocumentNumber').allowBlank = true;
                                panel.down('#dfPhysicalPersonDocType').allowBlank = true;

                                panel.down('#dfPersonBirthDate').allowBlank = true;
                                panel.down('#tfPersonBirthPlace').allowBlank = true;
                                panel.down('#tfRegistrationAddress').allowBlank = true;
                                panel.down('#tfFactAddress').allowBlank = true;
                                break;
                                //Активны поля Физ.лица   
                            case "8":
                            case "9":
                            case "14":      
                                panel.down('#sfContragent').setDisabled(true);
                                panel.down('#sfContragent').setValue(null);
                                panel.down('#tfSurname').setDisabled(!perm[1]);
                                panel.down('#tfFirstName').setDisabled(!perm[1]);
                                panel.down('#tfPatronymic').setDisabled(!perm[1]);
                                panel.down('#tfPosition').setDisabled(true);
                                panel.down('#dfPhysicalPersonDocType').setDisabled(!perm[1]);
                                panel.down('#dfPhysicalPersonDocumentSerial').setDisabled(!perm[1]);
                                panel.down('#dfPhysicalPersonDocumentNumber').setDisabled(!perm[1]);
                                panel.down('#dfPhysicalPersonIsNotRF').setDisabled(!perm[1]);

                                panel.down('#dfPersonBirthDate').setDisabled(false);
                                panel.down('#tfPersonBirthPlace').setDisabled(false);
                                panel.down('#tfRegistrationAddress').setDisabled(false);
                                panel.down('#tfFactAddress').setDisabled(false);

                                panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                                panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);

                                panel.down('#sfContragent').allowBlank = true;
                                panel.down('#tfSurname').allowBlank = false;
                                panel.down('#tfFirstName').allowBlank = false;
                                panel.down('#tfPatronymic').allowBlank = false;
                                panel.down('#tfPosition').allowBlank = false;
                                panel.down('#dfPhysicalPersonDocumentSerial').allowBlank = true;
                                panel.down('#dfPhysicalPersonDocumentNumber').allowBlank = true;
                                panel.down('#dfPhysicalPersonDocType').allowBlank = true;

                                panel.down('#dfPersonBirthDate').allowBlank = true;
                                panel.down('#tfPersonBirthPlace').allowBlank = true;
                                panel.down('#tfRegistrationAddress').allowBlank = true;
                                panel.down('#tfFactAddress').allowBlank = true;
                                break;
                        }
                    }).error(function() {
                        me.controller.unmask();
                    });
                }
            },

            setProtocolMvdPermission: function(protocolMvdId) {
                var me = this,
                    panel = me.getPanel(),
                    permissions = [
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View'
                    ];

                me.controller.mask('Загрузка', me.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('GetObjectSpecificPermissions', 'Permission'),
                    method: 'POST',
                    params: {
                        permissions: Ext.encode(permissions),
                        ids: Ext.encode([protocolMvdId])
                    }
                }).next(function(response) {
                    var perm = Ext.decode(response.responseText)[0];

                    me.controller.unmask();

                    if (!perm[0]) {
                        panel.down('#company').hide();
                    } else {
                        panel.down('#company').show();
                    }
                    if (!perm[1]) {
                        panel.down('#protocolMvdPhysicalPersonInfo').hide();
                    } else {
                        panel.down('#protocolMvdPhysicalPersonInfo').show();
                    }
                    if (!perm[2]) {
                        panel.down('#issuingAuthority').hide();
                    } else {
                        panel.down('#issuingAuthority').show();
                    }
                    if (!perm[3]) {
                        panel.down('#issueDate').hide();
                    } else {
                        panel.down('#issueDate').show();
                    }
                    if (!perm[4]) {
                        panel.down('#serialAndNumber').hide();
                    } else {
                        panel.down('#serialAndNumber').show();
                    }
                    if (!perm[5]) {
                        panel.down('#birthPlace').hide();
                    } else {
                        panel.down('#birthPlace').show();
                    }
                    if (!perm[6]) {
                        panel.down('#birthDate').hide();
                    } else {
                        panel.down('#birthDate').show();
                    }
                    if (!perm[7]) {
                        panel.down('#protocolMvdPhysicalPerson').hide();
                    } else {
                        panel.down('#protocolMvdPhysicalPerson').show();
                    }

                }).error(function() {
                    me.controller.unmask();
                });

            },

            onChangeTypeInitiativeOrg: function (field, value) {

                var cmp = field.up('panel').down('#tsfFineMunicipality');
                var judpanel = field.up('panel').down('#fsJudicalOffice');
                var desNum = field.up('panel').down('#tfDecisionNumber'); 
                var judicalOffice = field.up('panel').down('#sfJudicalOffice');
                var desDate = field.up('panel').down('#dfDecisionDate');

                if (value === 10) {
                    cmp.allowBlank = false;
                    cmp.show();
                    judpanel.allowBlank = true;
                    desNum.allowBlank = true;
                    judicalOffice.allowBlank = true;
                    desDate.allowBlank = true;
                    judpanel.hide();
                } else {
                    if (value === 20) {
                        judpanel.allowBlank = false;
                        desNum.allowBlank = false;
                        judicalOffice.allowBlank = false;
                        desDate.allowBlank = false;
                        judpanel.show();
                    }
                    cmp.allowBlank = true;
                    cmp.hide();
                }
            },

            onChangeTypeExecutant: function (field, value, oldValue) {

                var data = field.getRecord(value);

                //var contragentField = field.up(this.editPanelSelector).down('#sfContragent');

                //if (!Ext.isEmpty(contragentField) && !Ext.isEmpty(oldValue)) {
                //    contragentField.setValue(null);
                //}

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
            xtype: 'gkhbuttonprintaspect',
            name: 'resolutionDecisionPrintAspect',
            buttonSelector: '#resolutionDecisionEditWindow #btnPrint',
            codeForm: 'ResolutionDecision',
            getUserParams: function () {
                debugger;
                var param = { Id: this.controller.resolutionDecision };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'resolutionDecisionGridWindowAspect',
            gridSelector: 'resolutionDecisionGrid',
            editFormSelector: '#resolutionDecisionEditWindow',
            storeName: 'resolution.Decision',
            modelName: 'resolution.Decision',
            editWindowView: 'resolution.DecisionEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                this.controller.afterset = false;
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Resolution', this.controller.params.documentId);
                    }
                },

                aftersetformdata: function (asp, record) {
                    var me = this,
                        form = me.getForm();
                    asp.controller.resolutionDecision = record.get('Id');
                    this.controller.getAspect('resolutionDecisionPrintAspect').loadReportStore();
                    this.controller.getAspect('resolutionDecisionEstablishedAspect').doInjection();
                    this.controller.getAspect('resolutionDecisionDecidedAspect').doInjection();

                },
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'resolutionDecisionEstablishedAspect',
            fieldSelector: '[name=Established]',
            editPanelAspectName: 'resolutionDecisionGridWindowAspect',
            controllerName: 'ResolutionDefinition',
            valueFieldName: 'Established',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'resolutionDecisionDecidedAspect',
            fieldSelector: '[name=Decided]',
            editPanelAspectName: 'resolutionDecisionGridWindowAspect',
            controllerName: 'ResolutionDefinition',
            valueFieldName: 'Decided',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
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
            xtype: 'grideditwindowaspect',
            name: 'resolutionFizGridWindowAspect',
            gridSelector: '#resolutionFizGrid',
            editFormSelector: '#resolutionFizEditWindow',
            storeName: 'resolution.Fiz',
            modelName: 'resolution.Fiz',
            editWindowView: 'resolution.FizEditWindow',
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
            otherActions: function (actions) {
                actions['#resolutionPayFineGrid #btnAddPayFinePayReg'] = { 'click': { fn: this.onAddPayFine, scope: this } };
                actions['#resolutionPayFineEditWindow #btnSavePayFine'] = { 'click': { fn: this.onSavePayFine, scope: this } };
                actions['#resolutionPayFineEditWindow #btnClose'] = { 'click': { fn: this.onClosePayFineWin, scope: this } }
            },
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
            },
            onAddPayFine: function () {
                addWin = Ext.create('B4.view.resolution.PayFineEditWindow');
                addWin.show();
                //this.getStore('smev.PayRegRequests').load();
            },
            onSavePayFine: function (btn) {
                var me = this; // аспект
                var win = btn.up('#resolutionPayFineEditWindow'); // окно
                var resolutionId = me.controller.params.documentId;
                var payRegId = win.down('#dfPayReg').getValue();
                me.mask('Добавление оплаты', win);
                var result = B4.Ajax.request(B4.Url.action('AddPayFine', 'PAYREGExecute', {
                    resolutionId: resolutionId,
                    payRegId: payRegId
                }
                )).next(function (response) {
                    debugger;

                    var grid = me.getGrid(),
                        store = grid.getStore();
                    store.load();
                    //ar data = Ext.decode(response.responseText);
                    addWin = btn.up('#resolutionPayFineEditWindow');
                    addWin.close();
                    B4.QuickMsg.msg('Сообщение', "Оплата добавлена", 'success');

                    me.unmask();

                    return true;
                })
                    .error(function (resp) {
                        addWin = btn.up('#resolutionPayFineEditWindow');
                        addWin.close();
                        B4.QuickMsg.msg('Ошибка', resp.message, 'error');
                        me.unmask();
                    });
            },
            onClosePayFineWin: function (btn) {
                addWin = btn.up('#resolutionPayFineEditWindow');
                addWin.close();
            },
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
        },
          {
              /* 
              * Аспект взаимодействия таблицы статьи закона с массовой формой выбора статей
              * По нажатию на Добавить открывается форма выбора статей.
              * По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
              * И сохранение статей
              */
              xtype: 'gkhinlinegridmultiselectwindowaspect',
              name: 'resolutionArticleLawAspect',
              gridSelector: '#resolutionArtLawGrid',
              saveButtonSelector: '#resolutionArtLawGrid #resolutionSaveButton',
              storeName: 'resolution.ArtLaw',
              modelName: 'resolution.ArtLaw',
              multiSelectWindow: 'SelectWindow.MultiSelectWindow',
              multiSelectWindowSelector: '#resolutionArticleLawMultiSelectWindow',
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
                              url: B4.Url.action('AddArticles', 'ResolutionArticleLaw'),
                              method: 'POST',
                              params: {
                                  articleIds: Ext.encode(recordIds),
                                  documentId: asp.controller.params.documentId
                              }
                          }).next(function (response) {
                              asp.controller.unmask();
                              asp.controller.getStore(asp.storeName).load();
                              return true;
                          }).error(function (e) {
                              asp.controller.unmask();
                              Ext.Msg.alert('Ошибка!', e.message || e);
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
                 xtype: 'grideditwindowaspect',
                 name: 'admoVoilationGridWindowAspect',
                 gridSelector: '#admonVoilationGrid',
                 editFormSelector: '#admonVoilationEditWindow',
                 storeName: 'appealcits.AppCitAdmonVoilation',
                 modelName: 'appealcits.AppCitAdmonVoilation',
                 editWindowView: 'appealcits.AdmonVoilationEditWindow',
                 listeners: {
                     getdata: function (asp, record) {
                         if (!record.get('Id')) {
                             record.set('AppealCitsAdmonition', appealCitsAdmonition);
                         }
                     }
                 }
             }
    ],

    init: function () {
        this.getStore('resolution.PayFine').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.Dispute').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.Decision').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.Annex').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.Definition').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.ArtLaw').on('beforeload', this.onBeforeLoad, this);
        this.getStore('resolution.Fiz').on('beforeload', this.onBeforeLoad, this);

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

            //Обновляем стор оспариваний
            this.getStore('resolution.Decision').load();

            //Обновляем стор определений
            this.getStore('resolution.Definition').load();

            //Обновляем стор статей закона
            this.getStore('resolution.ArtLaw').load();
            //Обновляем стор статей закона
            this.getStore('resolution.Fiz').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});