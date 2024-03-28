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
        'B4.aspects.FieldRequirementAspect',
        'B4.enums.CitizenshipType',
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
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.FormatPlace', applyTo: '[name=FormatPlace]', selector: '#protocolgjiEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.FormatDate', applyTo: '[name=FormatDate]', selector: '#protocolgjiEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.NotifNumber', applyTo: '[name=NotifNumber]', selector: '#protocolgjiEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.DateOfProceedings', applyTo: '[name=DateOfProceedings]', selector: '#protocolgjiEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.DateOfProceedings', applyTo: '[name=HourOfProceedings]', selector: '#protocolgjiEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.DateOfProceedings', applyTo: '[name=MinuteOfProceedings]', selector: '#protocolgjiEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.ProceedingCopyNum', applyTo: '[name=ProceedingCopyNum]', selector: '#protocolgjiEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.ProceedingsPlace', applyTo: '[name=ProceedingsPlace]', selector: '#protocolgjiEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.Remarks', applyTo: '[name=Remarks]', selector: '#protocolgjiEditPanel' }
            ]
        },
        {
            // Обязательность полей protocolgjiRequisitePanel
            xtype: 'requirementaspect',
            applyOn: { event: 'show', selector: '#protocolgjiEditPanel' },
            name: 'protocolRequisiteRequirementAspect',
            requirements: [
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.Surname', applyTo: '[name=SurName]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.Name', applyTo: '[name=Name]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.Patronymic', applyTo: '[name=Patronymic]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.BirthDate', applyTo: '[name=BirthDate]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.BirthPlace', applyTo: '[name=BirthPlace]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.FactAddress', applyTo: '[name=FactAddress]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.SerialAndNumber', applyTo: '[name=SerialAndNumber]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.IssueDate', applyTo: '[name=IssueDate]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.IssuingAuthority', applyTo: '[name=IssuingAuthority]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.Company', applyTo: '[name=Company]', selector: '#protocolgjiRequisitePanel' },
                { name: 'GkhGji.DocumentReestrGji.Protocol.Field.Snils', applyTo: '[name=Snils]', selector: '#protocolgjiRequisitePanel' }
            ],
            afterSetRequirements: function() {
                var controller = this.controller,
                    protocolEditPanelAsp = controller.getAspect('protocolEditPanelAspect'),
                    typeExecutant = controller.getMainView().down('[name=Executant]'),
                    data = typeExecutant.getRecord(typeExecutant.getValue());

                protocolEditPanelAsp.setTypeExecutantPermission(data);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'protocolPrintAspect',
            buttonSelector: '#protocolgjiEditPanel #btnPrint',
            codeForm: 'Protocol',
            getUserParams: function (reportId) {
                var me = this,
                    param = { DocumentId: me.controller.params.documentId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'protocolDefinitionPrintAspect',
            buttonSelector: '#protocolgjiDefinitionEditWindow #btnPrint',
            codeForm: 'ProtocolDefinition',
            getUserParams: function (reportId) {
                var me = this,
                    param = { DefinitionId: me.controller.params.DefinitionId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /*
            Апект для основной панели Протокола
            */
            xtype: 'gjidocumentaspect',
            name: 'protocolEditPanelAspect',
            editPanelSelector: '#protocolgjiEditPanel',
            modelName: 'ProtocolGji',

            /*
             * Переопределяем метод проверки полей формы перед сохранением,
             * чтобы также проверить отсутствие элементов в гриде Статьи законов
             */
            checkFields: function () {
                var me = this,
                    panel = me.getPanel(),
                    form = panel.getForm(),
                    grid = panel.down('#protocolgjiArticleLawGrid'),
                    invalidFields = [],
                    fields;

                if (!form.isValid()) {

                    fields = form.getFields();

                    Ext.each(fields.items, function (field) {
                        if (!field.isValid()) {
                            invalidFields.push(field.fieldLabel);
                        }
                    });
                }

                if (grid && grid.getStore().getRange().length === 0) {
                    invalidFields.push('Статья закона');
                }

                if (!Ext.isEmpty(invalidFields)) {
                    Ext.Msg.alert('Ошибка', 'Не заполнены, или заполнены неверно, обязательные поля: <b><br>' + invalidFields.join('<br>') + '</b>');
                    return false;
                }

                return true;
            },

            otherActions: function (actions) {
                var me = this;

                actions[me.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: me.onChangeTypeExecutant, scope: me } };
                actions[me.editPanelSelector + ' #citizenshipType'] = { 'change': { fn: me.onChangeCitizenshipType, scope: me } };
                actions[me.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
                actions[me.editPanelSelector + ' #cbToCourt'] = { 'change': { fn: me.onChangeToCourt, scope: me } };
                actions[me.editPanelSelector + ' tabpanel #protocolgjiArticleLawGrid'] = { 'activate': { fn: me.onArticleLawTabActivate, scope: me } };
                actions['#protocolgjiRealityObjViolationGrid'] = { 'select': { fn: me.onSelectRealityObjViolationGrid, scope: me } };
            },
            
            onSelectRealityObjViolationGrid: function (rowModel, record, index, opt) {
                this.controller.getStore('protocolgji.Violation').load();
            },

            onArticleLawTabActivate: function (component) {
                component.getStore().load();
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    callbackUnMask,
                    dfToCourt;
                
                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;

                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                panel.down('#protocolTabPanel').setActiveTab(0);
                
                //включаем/выключаем поле "Дата передачи документов"
                dfToCourt = panel.down('#dfDateToCourt');

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
                    var obj = Ext.JSON.decode(response.responseText),
                        fieldBaseName,
                        fieldInspectors = panel.down('#trigfInspector');

                    fieldInspectors.setValue(obj.InspectorIds);
                    fieldInspectors.updateDisplayedText(obj.InspectorNames);

                    if (obj.InspectorNames)
                        fieldInspectors.clearInvalid();
                    else
                        fieldInspectors.markInvalid();

                    fieldBaseName = panel.down('#protocolBaseNameTextField');
                    fieldBaseName.setValue(obj.BaseName);

                    me.disableButtons(false);
                    me.controller.unmask();
                }).error(function () {
                    me.controller.unmask();
                });

                //Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('protocolStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                //обновляем отчеты
                me.controller.getAspect('protocolPrintAspect').loadReportStore();
                
                me.setTypeExecutantPermission(rec.get('Executant'));
                
                // обновляем кнопку Сформирвоать
                me.controller.getAspect('protocolCreateButtonAspect').setData(rec.get('Id'));
            },
            onChangeToCourt: function (field, data) {
                var me = this;
                if (data == true) {
                    me.getPanel().down('#dfDateToCourt').setDisabled(false);
                } else {
                    me.getPanel().down('#dfDateToCourt').setDisabled(true);
                }
            },

            setTypeExecutantPermission: function (typeExec) {
                var me = this,
                    panel = me.getPanel(),
                    permissions = [
                        'GkhGji.DocumentsGji.Protocol.Field.Contragent_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.PhysicalPerson_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.PhysicalPersonInfo_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.BirthDate_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.BirthPlace_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.FactAddress_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.CitizenshipType_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.Citizenship_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.SerialAndNumber_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.IssueDate_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.IssuingAuthority_Edit',
                        'GkhGji.DocumentsGji.Protocol.Field.Company_Edit'
                    ];

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('GetPermissions', 'Permission'),
                        method: 'POST',
                        params: {
                            permissions: Ext.encode(permissions)
                        }
                    }).next(function (response) {
                        me.controller.unmask();
                        var perm = Ext.decode(response.responseText),
                            sfContragent = panel.down('#sfContragent'),
                            dfDateWriteOut = panel.down('#dfDateWriteOut'),
                            fsReceiverInfo = panel.down('#fsReceiverInfo'),
                            fsReceiverReq = panel.down('#fsReceiverReq'),
                            physicalPersonInfo = panel.down('#physicalPersonInfo'),
                            surname = panel.down('#surname'),
                            name = panel.down('#name'),
                            patronymic = panel.down('#patronymic'),
                            birthDate = panel.down('#birthDate'),
                            birthPlace = panel.down('#birthPlace'),
                            factAddress = panel.down('#factAddress'),
                            citizenshipType = panel.down('#citizenshipType'),
                            citizenship = panel.down('#citizenship'),
                            serialAndNumber = panel.down('#serialAndNumber'),
                            issueDate = panel.down('#issueDate'),
                            issuingAuthority = panel.down('#issuingAuthority'),
                            tfSnils = panel.down('#tfSnils'),
                            company = panel.down('#company'),
                            //Должностные лица
                            officials = ['1', '3', '5', '10', '12', '13', '16', '19'];

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

                                fsReceiverInfo.setVisible(false);
                                fsReceiverReq.setVisible(false);
                                physicalPersonInfo.setVisible(false);

                                surname.allowBlank = true;
                                name.allowBlank = true;
                                patronymic.allowBlank = true;
                                birthDate.allowBlank = true;
                                birthPlace.allowBlank = true;
                                factAddress.allowBlank = true;

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

                                surname.setVisible(perm[1]);
                                name.setVisible(perm[1]);
                                patronymic.setVisible(perm[1]);
                                birthDate.setVisible(perm[3]);
                                birthPlace.setVisible(perm[4]);
                                factAddress.setVisible(perm[5]);

                                citizenshipType.setVisible(perm[6]);
                                citizenship.setVisible(perm[7] &&
                                    citizenshipType.getValue() !== B4.enums.CitizenshipType.RussianFederation);
                                serialAndNumber.setVisible(perm[8]);
                                issueDate.setVisible(perm[9]);
                                issuingAuthority.setVisible(perm[10]);
                                company.setVisible(perm[11]);
                                physicalPersonInfo.setVisible(perm[2]);

                                fsReceiverInfo.setVisible(true);
                                fsReceiverReq.setVisible(true);

                                surname.allowBlank = surname.allowBlank || !perm[1];
                                name.allowBlank = name.allowBlank || !perm[1];
                                birthDate.allowBlank = birthDate.allowBlank || !perm[3];
                                factAddress.allowBlank = factAddress.allowBlank || !perm[5];

                                citizenshipType.allowBlank = citizenshipType.allowBlank || !perm[6];
                                citizenship.allowBlank = citizenship.allowBlank || !perm[7] ||
                                    citizenshipType.getValue() === B4.enums.CitizenshipType.RussianFederation;
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

            onChangeTypeExecutant: function (field, value, oldValue) {
                var me = this,
                    data = field.getRecord(value),
                    protocolRequisiteAspect = me.controller.getAspect('protocolRequisiteRequirementAspect'),
                    contragentField = field.up(me.editPanelSelector).down('#sfContragent');

                if (!Ext.isEmpty(contragentField) && !Ext.isEmpty(oldValue)) {
                    contragentField.setValue(null);
                }

                if (data) {
                    if (me.controller.params) {
                        me.controller.params.typeExecutant = data.Code;
                    }

                    protocolRequisiteAspect.onAfterRender();
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
            },
            onChangeCitizenshipType: function(field, newValue) {
                var panel = field.up('#protocolgjiEditPanel'),
                    citizenship = panel.down('#citizenship');

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

                panel.getForm().isValid();
            },
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
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспектора',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            },
            onRowSelect: function (rowModel, record) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
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
                },
                beforesave: function (me, rec) {
                    if (rec.get('SendFileToErknm') === B4.enums.YesNoNotSet.Yes) {
                        var store = me.getGrid().getStore(),
                            foundRecord = store.findRecord('SendFileToErknm', B4.enums.YesNoNotSet.Yes);
                        if (foundRecord && foundRecord.get('Id') !== rec.get('Id')) {
                            Ext.Msg.alert('Ошибка сохранения!', 'Уже добавлены приложения с признаком "Передавать файл в ФГИС ЕРКНМ = Да". В ФГИС ЕРКНМ допускается передавать только один файл о решениях, принятых по результатам КНМ. Просьба скорректировать значение текущей или ранее добавленной записи.');
                            return false;
                        }
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
                var me = this;

                me.controller.params.DefinitionId = id;
                if (id) {
                    me.controller.getAspect('protocolDefinitionPrintAspect').loadReportStore();
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
            selModelMode: 'SINGLE',
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
                        fn: function() {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('protocolgji.Violation').on('beforeload', me.onBeforeLoadRealityObjViol, me);
        me.getStore('protocolgji.ArticleLaw').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocolgji.ArticleLaw').on('load', me.updateArticleLawGridAddButton, me);
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
        var me = this;

        if (me.params && me.params.documentId > 0)
            operation.params.documentId = me.params.documentId;
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
    },

    updateArticleLawGridAddButton: function(store, operation) {
        var me = this,
            addButton = me.getMainView().down('#protocolgjiArticleLawGrid b4addbutton'),
            records = store.getRange(),
            disable = records && records.length > 0;

        if (addButton) {
            addButton.setDisabled(disable)
        }
    }
});