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
        'B4.aspects.FieldRequirementAspect',
        'B4.enums.TypeExecutantProtocolMvd',
        'B4.enums.AdmissionType',
        'B4.Ajax',
        'B4.Url',
        'B4.enums.YesNoNotSet',
        'B4.enums.CitizenshipType'
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
            /*
            Аспект обязательности полей
            */
            xtype: 'requirementaspect',
            applyOn: { event: 'show', selector: '#resolutionRequisitePanel' },
            name: 'resolutionRequirementAspect',
            requirements: [
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.PatternDict',
                    applyTo: '[name=PatternDict]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.Municipality',
                    applyTo: '[name=Municipality]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.PenaltyAmount',
                    applyTo: '[name=PenaltyAmount]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.SurName',
                    applyTo: '[name=SurName]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.Name',
                    applyTo: '[name=Name]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.Patronymic',
                    applyTo: '[name=Patronymic]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.BirthDate',
                    applyTo: '[name=BirthDate]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.BirthPlace',
                    applyTo: '[name=BirthPlace]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.Address',
                    applyTo: '[name=Address]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.SerialAndNumber',
                    applyTo: '[name=SerialAndNumber]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.IssueDate',
                    applyTo: '[name=IssueDate]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.IssuingAuthority',
                    applyTo: '[name=IssuingAuthority]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.Company',
                    applyTo: '[name=Company]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.Snils',
                    applyTo: '[name=Snils]',
                    selector: '#resolutionRequisitePanel'
                },
                {
                    name: 'GkhGji.DocumentReestrGji.Resolution.Field.SanctionsDuration',
                    applyTo: '[name=SanctionsDuration]',
                    selector: '#resolutionRequisitePanel'
                },
            ],
            afterSetRequirements: function() {
                var ctrl = this.controller,
                    resEditPanelAsp = ctrl.getAspect('resolutionEditPanelAspect'),
                    typeExecutant = ctrl.getMainView().down('[name=Executant]'),
                    data = typeExecutant.getRecord(typeExecutant.getValue());

                resEditPanelAsp.setTypeExecutantPermission(data);
            }
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
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' combobox[name=TypeInitiativeOrg]'] = { 'change': { fn: this.onChangeTypeInitiativeOrg, scope: this } };
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this } };
                actions[this.editPanelSelector + ' #citizenshipType'] = { 'change': { fn: this.onChangeCitizenshipType, scope: this } };
                actions[this.editPanelSelector + ' #cbSanction'] = { 'change': { fn: this.onChangeSanctionType, scope: this } };
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    protocolMvdId = rec.get('ProtocolMvdId');

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

                if (protocolMvdId == 0) {
                    this.setTypeExecutantPermission(rec.get('Executant'));

                    panel.down('fieldset[name=fsPerson]').hide();
                    panel.down('fieldset[name=fsCopy]').hide();
                    panel.down('combobox[name=OffenderWas]').fromMvd = false;
                    panel.down('fieldset[name=fsReciever]').show();
                } else {
                    panel.down('fieldset[name=fsPerson]').show();
                    panel.down('fieldset[name=fsCopy]').show();
                    panel.down('combobox[name=OffenderWas]').fromMvd = true;
                    panel.down('fieldset[name=fsReciever]').hide();

                    panel.down('#surName').allowBlank = true;
                    panel.down('#firstName').allowBlank = true;
                    panel.down('#patronymic').allowBlank = true;
                    panel.down('#birthDate').allowBlank = true;
                    panel.down('#birthPlace').allowBlank = true;
                    panel.down('#address').allowBlank = true;

                    panel.down('#citizenshipType').allowBlank = true;
                    panel.down('#citizenship').allowBlank = true;
                    panel.down('#serialAndNumber').allowBlank = true;
                    panel.down('#issueDate').allowBlank = true;
                    panel.down('#issuingAuthority').allowBlank = true;
                    panel.down('#company').allowBlank = true;

                    panel.down('#sfContragent').allowBlank = true;
                }

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
                    panel = me.getPanel(),
                    cbSanction = panel.down('#cbSanction'),
                    permissions = [
                        'GkhGji.DocumentsGji.Resolution.Field.Contragent_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.PhysicalPerson_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.PhysicalPersonInfo_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.BirthDate_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.BirthPlace_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.Address_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.CitizenshipType_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.Citizenship_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.SerialAndNumber_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.IssueDate_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.IssuingAuthority_Edit',
                        'GkhGji.DocumentsGji.Resolution.Field.Company_Edit'
                    ];

                // Корректно отобразить 'tfSanctionsDuration'
                me.onChangeSanctionType(cbSanction, cbSanction.getValue());

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('GetPermissions', 'Permission'),
                        method: 'POST',
                        params: {
                            permissions: Ext.encode(permissions)
                        }
                    }).next(function (response) {
                        var perm = Ext.decode(response.responseText),
                            sfContragent = panel.down('#sfContragent'),
                            dfDateWriteOut = panel.down('#dfDateWriteOut'),
                            fsRecieverInfo = panel.down('#fsRecieverInfo'),
                            fsRecieverReq = panel.down('#fsRecieverReq'),
                            physicalPersonInfo = panel.down('#physicalPersonInfo'),
                            surName = panel.down('#surName'),
                            firstName = panel.down('#firstName'),
                            patronymic = panel.down('#patronymic'),
                            birthPlace = panel.down('#birthPlace'),
                            birthDate = panel.down('#birthDate'),
                            address = panel.down('#address'),
                            citizenshipType = panel.down('#citizenshipType'),
                            citizenship = panel.down('#citizenship'),
                            serialAndNumber = panel.down('#serialAndNumber'),
                            issueDate = panel.down('#issueDate'),
                            issuingAuthority = panel.down('#issuingAuthority'),
                            company = panel.down('#company'),
                            tfSnils = panel.down('#tfSnils'),
                            //Должностные лица
                            officials = ['1', '3', '5', '10', '12', '13', '16', '19'];

                        me.controller.unmask();
                        switch (typeExec.Code) {
                            //Активны поля Юр.лица
                            case '0':
                            case '2':
                            case '4':
                            case '8':
                            case '9':
                            case '11':
                            case '15':
                            case '17':
                            case '18':
                            case '21':
                                sfContragent.setVisible(perm[0]);
                                dfDateWriteOut.setVisible(perm[0]);

                                fsRecieverInfo.setVisible(false);
                                fsRecieverReq.setVisible(false);
                                physicalPersonInfo.setVisible(false);

                                surName.allowBlank = true;
                                firstName.allowBlank = true;
                                patronymic.allowBlank = true;
                                birthDate.allowBlank = true;
                                birthPlace.allowBlank = true;
                                address.allowBlank = true;

                                citizenshipType.allowBlank = true;
                                citizenship.allowBlank = true;
                                serialAndNumber.allowBlank = true;
                                issueDate.allowBlank = true;
                                issuingAuthority.allowBlank = true;
                                company.allowBlank = true;
                                tfSnils.allowBlank = true;

                                sfContragent.allowBlank = !perm[0];
                                break;
                            //Активны поля Физ.лица
                            case '1':
                            case '3':
                            case '6':
                            case '5':
                            case '7':
                            case '10':
                            case '12':
                            case '13':
                            case '14':
                            case '16':
                            case '19':
                            case '20':
                            case '22':
                                sfContragent.setVisible(officials.includes(typeExec.Code));
                                dfDateWriteOut.setVisible(officials.includes(typeExec.Code));

                                surName.setVisible(perm[1]);
                                firstName.setVisible(perm[1]);
                                patronymic.setVisible(perm[1]);
                                birthDate.setVisible(perm[3]);
                                birthPlace.setVisible(perm[4]);
                                address.setVisible(perm[5]);

                                citizenshipType.setVisible(perm[6]);
                                citizenship.setVisible(perm[7] && 
                                    citizenshipType.getValue() != B4.enums.CitizenshipType.RussianFederation);
                                serialAndNumber.setVisible(perm[8]);
                                issueDate.setVisible(perm[9]);
                                issuingAuthority.setVisible(perm[10]);
                                company.setVisible(perm[11]);
                                physicalPersonInfo.setVisible(perm[2]);

                                fsRecieverInfo.setVisible(true);
                                fsRecieverReq.setVisible(true);

                                surName.allowBlank = surName.allowBlank || !perm[1];
                                firstName.allowBlank = firstName.allowBlank || !perm[1];
                                birthDate.allowBlank = birthDate.allowBlank || !perm[3];
                                address.allowBlank = address.allowBlank || !perm[5];

                                citizenshipType.allowBlank = citizenshipType.allowBlank || !perm[6];
                                citizenship.allowBlank = citizenship.allowBlank || !perm[7] || 
                                    citizenshipType.getValue() == B4.enums.CitizenshipType.RussianFederation;
                                serialAndNumber.allowBlank = serialAndNumber.allowBlank || !perm[8];
                                issueDate.allowBlank = issueDate.allowBlank || !perm[9];
                                issuingAuthority.allowBlank = issuingAuthority.allowBlank || !perm[10];
                                company.allowBlank = company.allowBlank || !perm[11];

                                sfContragent.allowBlank = true;

                                tfSnils.setVisible(tfSnils.viewPermissionAllowed);
                                tfSnils.setDisabled(!tfSnils.editPermissionAllowed);
                                tfSnils.allowBlank = tfSnils.hidden || !tfSnils.permissionRequired;
                                break;
                        }

                        panel.getForm().isValid();
                    }).error(function () {
                        me.controller.unmask();
                    });
                }
            },

            setProtocolMvdPermission: function (protocolMvdId) {
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
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.CitizenshipType_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.Citizenship_View',
                        'GkhGji.DocumentsGji.ProtocolMvd.Field.Commentary_View'
                    ];

                me.controller.mask('Загрузка', me.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('GetPermissions', 'Permission'),
                    method: 'POST',
                    params: {
                        permissions: Ext.encode(permissions),
                        //ids: Ext.encode([protocolMvdId])
                    }
                }).next(function(response) {
                    var perm = Ext.decode(response.responseText);

                    me.controller.unmask();

                    panel.down('#protocolMvdCompany').setVisible(perm[0]);
                    panel.down('#taPhysPersonInfo').setVisible(perm[1]);
                    panel.down('#protocolMvdIssuingAuthority').setVisible(perm[2]);
                    panel.down('#protocolMvdIssueDate').setVisible(perm[3]);
                    panel.down('#protocolMvdSerialAndNumber').setVisible(perm[4]);
                    panel.down('#protocolMvdBirthPlace').setVisible(perm[5]);
                    panel.down('#protocolMvdBirthDate').setVisible(perm[6]);
                    panel.down('#protocolMvdSurName').setVisible(perm[7]);
                    panel.down('#protocolMvdName').setVisible(perm[7]);
                    panel.down('#protocolMvdPatronymic').setVisible(perm[7]);
                    var citizenshipType = panel.down('#protocolMvdCitizenshipType');
                    citizenshipType.setVisible(perm[8]);
                    panel.down('#protocolMvdCitizenship').setVisible(perm[9] &&
                        citizenshipType.getValue() != B4.enums.CitizenshipType.RussianFederation);
                    panel.down('#protocolMvdCommentary').setVisible(perm[10]);

                }).error(function () {
                    me.controller.unmask();
                });
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
                var data = field.getRecord(value),
                    contragentField = field.up(this.editPanelSelector).down('#sfContragent'),
                    reqAspect = this.controller.getAspect('resolutionRequirementAspect'),
                    protocolMvdId = field.up('#resolutionEditPanel').getRecord().get('ProtocolMvdId');

                if (!Ext.isEmpty(contragentField) && !Ext.isEmpty(oldValue)) {
                    contragentField.setValue(null);
                }

                if (data) {
                    if (this.controller.params) {
                        this.controller.params.typeExecutant = data.Code;
                    }

                    if (protocolMvdId == 0) {
                        reqAspect.onAfterRender();
                    }
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
            },

            onChangeCitizenshipType: function(field, newValue) {
                var reqPanel = field.up('#resolutionEditPanel'),
                    citizenship = reqPanel.down('#citizenship');

                switch (newValue) {
                    case B4.enums.CitizenshipType.RussianFederation:
                        citizenship.hide();
                        citizenship.allowBlank = true;
                        break;
                
                    default:
                        citizenship.show();
                        citizenship.allowBlank = false;
                        break;
                }
                reqPanel.getForm().isValid();
            },

            onChangeSanctionType: function(combo, newValue){
                var tfSanctionsDuration = combo.up('#resolutionRequisitePanel').down('#tfSanctionsDuration'),
                    code = combo.getRecord(newValue)?.Code;

                switch (code) {
                    //Дисквалификация
                    case '5':
                    //Административное приостановление деятельности
                    case '6':
                    //Административный арест
                    case '7':
                        tfSanctionsDuration.setVisible(tfSanctionsDuration.viewPermissionAllowed);
                        tfSanctionsDuration.setDisabled(!tfSanctionsDuration.editPermissionAllowed);
                        tfSanctionsDuration.allowBlank = tfSanctionsDuration.hidden || !tfSanctionsDuration.permissionRequired;
                        break;
                    default:
                        tfSanctionsDuration.hide();
                        tfSanctionsDuration.allowBlank = true;
                }
            },

            btnDeleteClick: function() {
                var me = this,
                    panel = this.getPanel(),
                    record = panel.getForm().getRecord(),
                    gisUin = record.get('GisUin'),
                    message;
                
                if(!gisUin) {
                    message = 'Вы действительно хотите удалить документ?';
                }
                else{
                    message = 'Начисление было отправлено в ГИС ГМП.</br><p style="text-align:center">Удалить документ?</p>'
                }

                Ext.Msg.confirm('Удаление записи!', message, function(result) {
                    if (result == 'yes') {
                        this.mask('Удаление', B4.getBody());
                        record.destroy()
                            .next(function() {

                                //Обновляем дерево меню
                                var tree = me.getTreePanel();
                                tree.getStore().load();

                                Ext.Msg.alert('Удаление!', 'Документ успешно удален');

                                panel.close();
                                this.unmask();
                            }, this)
                            .error(function(result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                this.unmask();
                            }, this);

                    }
                }, this);
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
                actions[this.editFormSelector + ' #cbDisputeAppeal'] = { 'change': { fn: this.onChangeDisputeAppeal, scope: this } };
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