Ext.define('B4.controller.manorglicense.EditRequest', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhEditPanel',
        'B4.form.SelectField',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextButton',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.view.manorglicense.ProvDocGrid',
        'B4.view.manorglicense.PersonGrid',
        'B4.view.manorglicense.RequestProvDocEditWindow',
        'B4.QuickMsg',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.enums.LicenseRequestType',
        'B4.aspects.FieldRequirementAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'dict.LicenseProvidedDoc',
        'manorglicense.PersonByContragent',
        'manorglicense.RequestAnnex'
    ],

    models: [
        'Contragent',
        'manorglicense.Request',
        'manorglicense.RequestPerson',
        'manorglicense.RequestProvDoc',
        'manorglicense.RequestAnnex',
        'BaseLicenseApplicants'
    ],

    views: [
        'manorglicense.EditRequestPanel',
        'manorglicense.ProvDocGrid',
        'manorglicense.PersonGrid',
        'manorglicense.RequestAnnexEditWindow',
        'manorglicense.RequestAnnexGrid'
    ],

    mainView: 'manorglicense.EditRequestPanel',
    mainViewSelector: 'manOrgLicenseRequestEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'manOrgLicenseRequestEditPanel'
        },
        {
            ref: 'personGrid',
            selector: 'manorglicensepersongrid'
        },
        {
            ref: 'provDocGrid',
            selector: 'manorglicenseprovdocgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'licenseRequestPrintAspect',
            buttonSelector: 'manOrgLicenseRequestEditPanel #btnPrint',
            codeForm: 'ManOrgLicenseRequest',
            getUserParams: function() {
                var me = this,
                    param = { RequestId: me.controller.getMainView().params.requestId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'manOrgLicenseRequestStatePermAspectView',
            applyByPostfix: true,
            mainViewSelector: 'manOrgLicenseRequestEditPanel',
            afterSetRequirements: function (rec) {
                var me = this;

                me.controller.applyRequestType(me.controller.getMainView(), rec);
            },
            permissions: [
                { name: 'Gkh.ManOrgLicense.Request.FormInsp', applyTo: 'button[action=createInspection]' },
                { name: 'Gkh.ManOrgLicense.Request.SubmittedDocs.View', applyTo: 'tabpanel tab[text=Предоставленные документы]' },
                { name: 'Gkh.ManOrgLicense.Request.SubmittedDocs.Delete', applyTo: 'b4deletecolumn', selector: 'manorglicenseprovdocgrid' },
                { name: 'Gkh.ManOrgLicense.Request.Reports.Print', applyTo: 'gkhbuttonprint' },

                { name: 'Gkh.ManOrgLicense.Request.Field.DateRequest_View', applyTo: '[name=DateRequest]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.RegisterNum_View', applyTo: '[name=RegisterNum]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.ConfirmationOfDuty_View', applyTo: '[name=ConfirmationOfDuty]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.TaxSum_View', applyTo: '[name=TaxSum]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.File_View', applyTo: '[name=File]' },
                {
                    name: 'Gkh.ManOrgLicense.Request.Field.LicenseRegistrationReason_View', applyTo: '[name=LicenseRegistrationReason]',
                    applyBy: function (component, allowed) {
                        component.allowedView = allowed;
                    }
                },
                { name: 'Gkh.ManOrgLicense.Request.Field.LicenseRejectReason_View', applyTo: '[name=LicenseRejectReason]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.Note_View', applyTo: '[name=Note]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.NoticeAcceptanceDate_View', applyTo: '[name=NoticeAcceptanceDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.NoticeViolationDate_View', applyTo: '[name=NoticeViolationDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.ReviewDate_View', applyTo: '[name=ReviewDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.NoticeReturnDate_View', applyTo: '[name=NoticeReturnDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.ReviewDateLk_View', applyTo: '[name=ReviewDateLk]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.PreparationOfferDate_View', applyTo: '[name=PreparationOfferDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.SendResultDate_View', applyTo: '[name=SendResultDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.SendMethod_View', applyTo: '[name=SendMethod]' },

                { name: 'Gkh.ManOrgLicense.Request.Edit', applyTo: 'b4savebutton' },
                { name: 'Gkh.ManOrgLicense.Request.SubmittedDocs.Create', applyTo: 'toolbar buttongroup', selector: 'manorglicenseprovdocgrid' },
                { name: 'Gkh.ManOrgLicense.Request.SubmittedDocs.Edit', applyTo: 'b4savebutton', selector: 'manorgrequestprovdoceditwindow' },

                { name: 'Gkh.ManOrgLicense.Request.Field.DateRequest_Edit', applyTo: '[name=DateRequest]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.RegisterNum_Edit', applyTo: '[name=RegisterNum]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.ConfirmationOfDuty_Edit', applyTo: '[name=ConfirmationOfDuty]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.TaxSum_Edit', applyTo: '[name=TaxSum]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.File_Edit', applyTo: '[name=File]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.LicenseRegistrationReason_Edit', applyTo: '[name=LicenseRegistrationReason]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.LicenseRejectReason_Edit', applyTo: '[name=LicenseRejectReason]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.Note_Edit', applyTo: '[name=Note]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.NoticeAcceptanceDate_Edit', applyTo: '[name=NoticeAcceptanceDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.NoticeViolationDate_Edit', applyTo: '[name=NoticeViolationDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.ReviewDate_Edit', applyTo: '[name=ReviewDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.NoticeReturnDate_Edit', applyTo: '[name=NoticeReturnDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.ReviewDateLk_Edit', applyTo: '[name=ReviewDateLk]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.PreparationOfferDate_Edit', applyTo: '[name=PreparationOfferDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.SendResultDate_Edit', applyTo: '[name=SendResultDate]' },
                { name: 'Gkh.ManOrgLicense.Request.Field.SendMethod_Edit', applyTo: '[name=SendMethod]' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'manOrgLicenseRequestStatePermAspectEdit',
            permissions: [
                
            ]
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.ManOrgLicenseRequest.Field.DateRequest', applyTo: '[name=DateRequest]', selector: 'manOrgLicenseRequestEditPanel' },
                { name: 'GkhGji.ManOrgLicenseRequest.Field.RegisterNum', applyTo: '[name=RegisterNum]', selector: 'manOrgLicenseRequestEditPanel' },
                { name: 'GkhGji.ManOrgLicenseRequest.Field.ConfirmationOfDuty', applyTo: '[name=ConfirmationOfDuty]', selector: 'manOrgLicenseRequestEditPanel' },
                { name: 'GkhGji.ManOrgLicenseRequest.Field.TaxSum', applyTo: '[name=TaxSum]', selector: 'manOrgLicenseRequestEditPanel' },
                { name: 'GkhGji.ManOrgLicenseRequest.Field.File', applyTo: '[name=File]', selector: 'manOrgLicenseRequestEditPanel' },
                { name: 'GkhGji.ManOrgLicenseRequest.Field.LicenseRegistrationReason', applyTo: '[name=LicenseRegistrationReason]', selector: 'manOrgLicenseRequestEditPanel' },
                { name: 'GkhGji.ManOrgLicenseRequest.Field.LicenseRejectReason', applyTo: '[name=LicenseRejectReason]', selector: 'manOrgLicenseRequestEditPanel' },
                { name: 'GkhGji.ManOrgLicenseRequest.Field.Note', applyTo: '[name=Note]', selector: 'manOrgLicenseRequestEditPanel' }
            ]
        },
        {
            xtype: 'backforwardaspect',
            panelSelector: 'personEditPanel',
            backForwardController: 'manorglicense.Request'
        },
        {
            /*
             * Вешаем аспект смены статуса в карточке редактирования
             */
            xtype: 'statecontextbuttonaspect',
            name: 'requestStateButtonAspect',
            stateButtonSelector: 'manOrgLicenseRequestEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId, newState) {
                    var me = this,
                        view = me.controller.getMainView();

                    asp.controller.getAspect('manOrgLicenseRequestEditPanelAspect').setData(entityId);

                    if (newState && newState.FinalState) {
                        //После успешной смены статуса запрашиваем по Id актуальные данные записи
                        //и обновляем панель
                        if (view.params.requestType == B4.enums.LicenseRequestType.RenewalLicense) {
                            Ext.Msg.alert('Внимание!', 'Не забудьте исключить дома из реестра лицензий для ' + view.params.inn + ' - ' + view.params.contragentName);
                        }
                        Ext.History.add(Ext.String.format("manorglicense/{0}/{1}/addlicense", view.params.type, entityId));
                    }
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'manOrgLicenseRequestEditPanelAspect',
            editPanelSelector: 'manOrgLicenseRequestEditPanel',
            modelName: 'manorglicense.Request',
            otherActions: function(actions) {
                var me = this;

                actions['manOrgLicenseRequestEditPanel b4selectfield[name=Contragent]'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
                actions['manOrgLicenseRequestEditPanel button[action=GoToLicense]'] = { 'click': { fn: me.goToLicense, scope: me } };
            },
            onBeforeLoadContragent: function(field, options, store) {
                options = options || {};
                options.params = options.params || {};

                options.params.typeJurOrg = 10; // нужны только управляющие организации

                return true;
            },
            goToLicense: function (btn) {
                var me = this,
                    record = me.getRecord(),
                    license = record.get('ManOrgLicense');

                if (license) {
                    Ext.History.add('manorglicense/request/' + license.Request.Id + '/editlicense/');
                }
            },
            listeners: {
                savesuccess: function(asp, rec) {
                    asp.setData(rec.getId());
                },
                aftersetpaneldata: function(asp, rec, panel) {
                    var me = this,
                        personGrid = panel.down('manorglicensepersongrid'),
                        personStore = personGrid.getStore(),
                        provDocGrid = panel.down('manorglicenseprovdocgrid'),
                        provDocStore = provDocGrid.getStore(),
                        annexGrid = panel.down('manorglicenserequestannexgrid'),
                        annexStore = annexGrid.getStore(),
                        inspGrid = panel.down('manorglicenserequestinspgrid'),
                        inspStore = inspGrid.getStore(),
                        tfContragentName = panel.down('[name=ContragentName]'),
                        tfContragentShortName = panel.down('[name=ContragentShortName]'),
                        tfContragentOrgForm = panel.down('[name=ContragentOrgForm]'),
                        tfContragentJurAddress = panel.down('[name=ContragentJurAddress]'),
                        tfContragentFactAddress = panel.down('[name=ContragentFactAddress]'),
                        tfContragentOgrn = panel.down('[name=ContragentOgrn]'),
                        tfContragentInn = panel.down('[name=ContragentInn]'),
                        tfContragentRegistration = panel.down('[name=ContragentRegistration]'),
                        tfContragentPhone = panel.down('[name=ContragentPhone]'),
                        tfContragentEmail = panel.down('[name=ContragentEmail]'),
                        tfTaxRegistrationSeries = panel.down('[name=TaxRegistrationSeries]'),
                        tfTaxRegistrationNumber = panel.down('[name=TaxRegistrationNumber]'),
                        tfTaxRegistrationIssuedBy = panel.down('[name=TaxRegistrationIssuedBy]'),
                        tfTaxRegistrationDate = panel.down('[name=TaxRegistrationDate]');

                    B4.Ajax.request({
                        url: B4.Url.action('GetContragentInfo', 'ManOrgLicenseRequest'),
                        method: 'POST',
                        params: { requestId: rec.get('Id') || 0 }
                    }).next(function(response) {
                        var data = Ext.decode(response.responseText);

                        tfContragentName.setValue(data.Name);
                        tfContragentShortName.setValue(data.ShortName);
                        tfContragentOrgForm.setValue(data.OrgForm);
                        tfContragentJurAddress.setValue(data.JurAddress);
                        tfContragentFactAddress.setValue(data.FactAddress);
                        tfContragentOgrn.setValue(data.Ogrn);
                        tfContragentInn.setValue(data.Inn);
                        tfContragentRegistration.setValue(data.OgrnRegistration);
                        tfContragentPhone.setValue(data.Phone);
                        tfContragentEmail.setValue(data.Email);
                        tfTaxRegistrationSeries.setValue(data.TaxRegistrationSeries);
                        tfTaxRegistrationNumber.setValue(data.TaxRegistrationNumber);
                        tfTaxRegistrationIssuedBy.setValue(data.TaxRegistrationIssuedBy);
                        tfTaxRegistrationDate.setValue(data.TaxRegistrationDate);

                        if (data && data.Id) {
                            panel.params.contragentId = data.Id;
                            panel.params.contragentName = data.Name;
                            panel.params.inn = data.Inn;
                        }
                    });

                    me.controller.getAspect('requestStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    me.controller.getAspect('manOrgLicenseRequestStatePermAspectView').setPermissionsByRecord(rec);

                    personStore.clearFilter(true);
                    personStore.filter('requestId', rec.getId());

                    provDocStore.clearFilter(true);
                    provDocStore.filter('requestId', rec.getId());

                    annexStore.clearFilter(true);
                    annexStore.filter('requestId', rec.getId());

                    inspStore.clearFilter(true);
                    inspStore.filter('requestId', rec.getId());

                    panel.down('button[name=createInspection]').itemsStore.load();
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'manorglicenseprovdocAspect',
            gridSelector: 'manorglicenseprovdocgrid',
            modelName: 'manorglicense.RequestProvDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#manorglicenseprovdocMultiSelectWindow',
            storeSelect: 'dict.LicenseProvidedDoc',
            storeSelected: 'dict.LicenseProvidedDoc',
            titleSelectWindow: 'Выбор предоставляемых документов',
            titleGridSelect: 'Документы для выбора',
            titleGridSelected: 'Выбранные документы',
            editFormSelector: 'manorgrequestprovdoceditwindow',
            editWindowView: 'manorglicense.RequestProvDocEditWindow',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            saveRecord: function(rec) {
                var me = this,
                    frm = me.getEditForm().getForm();

                rec.set('LicProvidedDoc', me.getEditForm().down('b4selectfield[name=LicProvidedDoc]').getValue());
                rec.set('LicRequest', me.controller.getMainView().params.requestId);

                frm.submit({
                    url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                    params: {
                        records: Ext.encode([rec.getData()])
                    },
                    success: function() {
                        me.updateGrid();
                        me.getEditForm().close();
                    },
                    failure: function(form, action) {
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            },
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [],
                        panel = asp.controller.getMainView(),
                        docsGrid = asp.controller.getProvDocGrid();

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', panel);
                        B4.Ajax.request(B4.Url.action('AddProvDocs', 'ManOrgLicenseRequest', {
                            provdocIds: Ext.encode(recordIds),
                            requestId: panel.params.requestId
                        })).next(function(response) {
                            asp.controller.unmask();
                            docsGrid.getStore().load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать документы');
                        return false;
                    }
                    return true;
                }
            },
            getEditForm: function() {
                var me = this,
                    editWindow;

                if (me.editFormSelector) {

                    if (me.componentQuery) {
                        editWindow = me.componentQuery(me.editFormSelector);
                    }

                    if (!editWindow) {
                        editWindow = Ext.ComponentQuery.query(me.editFormSelector)[0];
                    }

                    if (!editWindow) {

                        editWindow = me.controller.getView(me.editWindowView).create(
                        {
                            constrain: true,
                            autoDestroy: true,
                            closeAction: 'destroy',
                            renderTo: B4.getBody().getActiveTab().getEl(),
                            ctxKey: me.controller.getCurrentContextKey ? me.controller.getCurrentContextKey() : ''
                        });

                        editWindow.show();
                    }

                    return editWindow;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы ДЛ с массовой формой выбора ДЛ
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'manorglicensepersonAspect',
            gridSelector: 'manorglicensepersongrid',
            modelName: 'manorglicense.RequestPerson',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#manorglicensepersonMultiSelectWindow',
            storeSelect: 'manorglicense.PersonByContragent',
            storeSelected: 'manorglicense.PersonByContragent',
            titleSelectWindow: 'Должностные лица',
            titleGridSelect: 'Должностные лица для выбора',
            titleGridSelected: 'Выбранные ДЛ',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'FullName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'FullName', flex: 1, sortable: false }
            ],
            onBeforeLoad: function(store, operation) {
                var me = this,
                    panel = me.controller.getMainView(),
                    dateRequest = panel.down('datefield[name=DateRequest]').getValue();

                operation.params.requestId = panel.params.requestId;
                operation.params.dateRequest = dateRequest;
                operation.params.contragentId = panel.params.contragentId;
            },
            rowDblClick: function(view, record) {
                if (record.get('Person')) {
                    this.controller.application.redirectTo("personedit/" + record.get('Person'));
                }
            },

            rowAction: function(grid, action, record) {
                switch (action.toLowerCase()) {
                    case 'edit':
                        {
                            if (record.get('Person')) {
                                this.controller.application.redirectTo("personedit/" + record.get('Person'));
                            }
                        }
                        break;
                    case 'delete':
                        this.deleteRecord(record);
                        break;
                }
            },

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [],
                        panel = asp.controller.getMainView(),
                        personGrid = asp.controller.getPersonGrid();

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', panel);
                        B4.Ajax.request(B4.Url.action('AddPersons', 'ManOrgLicenseRequest', {
                            personIds: Ext.encode(recordIds),
                            requestId: panel.params.requestId
                        })).next(function(response) {
                            asp.controller.unmask();
                            personGrid.getStore().load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать ДЛ');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
             * Аспект взаимодействия Таблицы Приложений с формой редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'manOrgLicenseRequestAnnexAspect',
            gridSelector: 'manorglicenserequestannexgrid',
            editFormSelector: 'manorglicenserequestannexeditwindow',
            storeName: 'manorglicense.RequestAnnex',
            modelName: 'manorglicense.RequestAnnex',
            editWindowView: 'manorglicense.RequestAnnexEditWindow',
            listeners: {
                beforesave: function(asp, record) {
                    var me = this,
                        mainView = me.controller.getMainView(),
                        requestId = me.controller.getContextValue(mainView, 'requestId');

                    record.set('LicRequest', requestId);
                },
                deletesuccess: function() {
                    this.controller.getMainView().down('manorglicenserequestannexgrid').getStore().load();
                }
            },
            onSaveSuccess: function(aspect) {
                var form = aspect.getForm();
                if (form) {
                    form.close();
                }

                aspect.getGrid().getStore().load();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'manOrgLicenseRequestInspAspect',
            gridSelector: 'manorglicenserequestinspgrid',
            controllerEditName: 'B4.controller.baselicenseapplicants.Navigation',
            updateGrid: function() {
                this.getGrid().getStore().load();
            },
            editRecord: function(record) {
                var me = this,
                    id = record ? record.get('Id') : null;

                if (id) {
                    if (me.controllerEditName) {
                        var portal = me.controller.getController('PortalController');

                        if (!me.controller.hideMask) {
                            me.controller.hideMask = function() { me.controller.unmask(); };
                        }

                        //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                        me.controller.mask('Загрузка', me.controller.getMainComponent());
                        portal.loadController(me.controllerEditName, record, portal.containerSelector, me.controller.hideMask);
                    }
                }
            }
        }
    ],

    index: function(type, id) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('manOrgLicenseRequestEditPanel');

            view.params = {};

            B4.Ajax.request(B4.Url.action('GetInfo', 'ManOrgLicense', {
                type: type,
                id: id
            })).next(function(response) {
                var obj = Ext.JSON.decode(response.responseText);

                view.params.type = type;
                view.params.licenseId = obj.licenseId;
                view.params.requestId = obj.requestId;
                view.params.requestType = obj.requestType;

                if (view.params.requestId) {
                    me.bindContext(view);
                    me.setContextValue(view, 'type', view.params.type);
                    me.setContextValue(view, 'id', id);
                    me.setContextValue(view, 'requestId', view.params.requestId);
                    me.setContextValue(view, 'licenseId', view.params.licenseId);
                    me.application.deployView(view, 'license_info');

                    me.getAspect('manOrgLicenseRequestEditPanelAspect').setData(view.params.requestId);
                    me.getAspect('licenseRequestPrintAspect').loadReportStore();
                }

                return true;
            });
        } else {
            if (view.params) {
                me.bindContext(view);
                me.setContextValue(view, 'type', type);
                me.setContextValue(view, 'id', id);
                me.setContextValue(view, 'requestId', view.params.requestId);
                me.setContextValue(view, 'licenseId', view.params.licenseId);
                me.application.deployView(view, 'license_info');

                me.getAspect('manOrgLicenseRequestEditPanelAspect').setData(view.params.requestId);
                me.getAspect('licenseRequestPrintAspect').loadReportStore();
            }
        }
    },

    init: function() {
        var me = this,
            actions = {};

        actions['manOrgLicenseRequestEditPanel button[action=goToContragent]'] = { 'click': { fn: me.goToContragent, scope: me } };
        actions['manOrgLicenseRequestEditPanel button[name=createInspection] menuitem'] = { 'click': { fn: me.createInspection, scope: me } };

        actions['manOrgLicenseRequestEditPanel manorglicenserequestannexgrid b4updatebutton'] = { 'click': { fn: me.updateAnnexGrid, scope: me } };

        actions['manOrgLicenseRequestEditPanel'] = {
            'afterrender': function(view) {
                var btn = view.down('button[name=createInspection]'),
                    itemsStore = btn.itemsStore;

                itemsStore.on('beforeload', function(store, operation) {
                    if (view.params && view.params.requestId) {
                        operation.params.requestId = view.params.requestId;;
                    }
                });

                itemsStore.on('load', function(store) {
                    if (btn) {
                        btn.setDisabled(true);
            
                        btn.menu.removeAll();

                        store.each(function (rec) {
                
                            btn.setDisabled(false);
                
                            btn.menu.add({
                                xtype: 'menuitem',
                                text: rec.get('Text'),
                                textAlign: 'left',
                                ruleId: rec.get('Id'),
                                extraParams: rec.get('ExtraParams')
                            });
                        });

                        btn.setDisabled(!btn.menu.items.length);
                    }
                });
                
            },
            scope: me
        };

        me.control(actions);

        me.callParent(arguments);
    },

    goToContragent: function(btn) {
        var panel = btn.up('manOrgLicenseRequestEditPanel');

        if (!panel.params || (panel.params && !panel.params.contragentId)) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать УО!');
            return;
        }

        Ext.History.add('contragentedit/' + panel.params.contragentId + '/');
    },

    updateAnnexGrid: function(btn) {
        btn.up('manorglicenserequestannexgrid').getStore().load();
    },

    hasChanges: function() {
        return this.getMainComponent().getForm().isDirty();
    },

    createInspection: function(btn) {
        var me = this,
            view = me.getMainView(),
            requestId = me.getContextValue(view, 'requestId'),
            rec = me.getModel('BaseLicenseApplicants').create(),
            grid = view.down('manorglicenserequestinspgrid'),
            extraParams = btn.extraParams || {};

        rec.set('ManOrgLicenseRequest', requestId);
        Ext.apply(rec.data, extraParams);

        me.mask('Сохранение');
        rec.save({ id: rec.getId() })
            .next(function(result) {
                me.unmask();
                grid.getStore().load();
                me.getAspect('manOrgLicenseRequestInspAspect').editRecord(result.record);
            })
            .error(function(result) {
                me.unmask();
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            });
    },

    applyRequestType: function(panel, record) {
        var licenseInfo = panel.down('fieldset[name=LicenseInfo]'),
            applicantInfo = panel.down('fieldset[name=ApplicantInfo]'),
            reviewInfo = panel.down('fieldset[name=ReviewInfo]'),
            reviewInfoLk = panel.down('fieldset[name=ReviewInfoLk]'),
            taxInfo = panel.down('container[name=TaxInfo]'),
            noticeInfo = panel.down('container[name=Notice]'),
            reviewDateInfo = panel.down('container[name=ReviewDate]'),
            licenseRejectReason = panel.down('b4selectfield[name=LicenseRejectReason]'),
            revokeLicenseInfo = panel.down('fieldset[name=RevokeLicenseInfo]'),
            orderInfo = panel.down('fieldset[name=OrderInfo]'),
            personGrid = panel.down('grid[name=PersonGrid]'),
            provDocFile = panel.down('b4filefield[name=ProvDocFile]'),
            requestType = record.get('Type'),
            revokeLicenseId = record.get('RevokeLicenseId');

        switch (requestType) {
            case B4.enums.LicenseRequestType.GrantingLicense:
                orderInfo.show();
                reviewInfo.show();
                reviewInfoLk.show();

                if (revokeLicenseId) {
                    revokeLicenseInfo.show();
                }
                break;

            case B4.enums.LicenseRequestType.IssuingDuplicateLicense:
            case B4.enums.LicenseRequestType.ProvisionCopiesLicense:
                licenseInfo.show();
                reviewInfo.show();
                licenseRejectReason.setVisible(false);
                break;

            case B4.enums.LicenseRequestType.ExtractFromRegisterLicense:
                applicantInfo.show();
                reviewInfo.show();
                noticeInfo.setVisible(false);
                reviewDateInfo.setVisible(false);
                taxInfo.setVisible(false);
                licenseRejectReason.setVisible(false);
                personGrid.setVisible(false);
                licenseInfo.show();
                provDocFile.setVisible(false);
                break;

            case B4.enums.LicenseRequestType.TerminationActivities:
                licenseInfo.show();
                reviewInfo.show();
                taxInfo.setVisible(false);
                licenseRejectReason.setVisible(false);
                personGrid.setVisible(false);
                break;

            case B4.enums.LicenseRequestType.RenewalLicense:
                orderInfo.show();
                licenseInfo.show();
                reviewInfo.show();
                break;
        }
    }
});