Ext.define('B4.controller.licensereissuance.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhEditPanel',
        'B4.form.SelectField',
        'B4.aspects.StateContextButton',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.view.licensereissuance.RequestProvDocEditWindow',
        'B4.view.licensereissuance.ProvDocGrid',
        'B4.view.licensereissuance.PersonGrid',
        'B4.QuickMsg',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'dict.LicenseProvidedDoc',
    ],

    models: [
        'Contragent',
        'licensereissuance.LicenseReissuance',
        'licensereissuance.LicenseReissuancePerson',
        'licensereissuance.LicenseReissuanceProvDoc',
    ],

    views: [
        'licensereissuance.EditLicenseReissuancePanel',
        'licensereissuance.ProvDocGrid',
        'licensereissuance.PersonGrid',
    ],

    mainView: 'licensereissuance.EditLicenseReissuancePanel',
    mainViewSelector: 'licenseReissuanceEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'licenseReissuanceEditPanel'
        },
        {
            ref: 'personGrid',
            selector: 'licensereissuancepersongrid'
        },
        {
            ref: 'provDocGrid',
            selector: 'licensereissuanceprovdocgrid'
        }
    ],

    aspects: [
        //{
        //    xtype: 'gkhbuttonprintaspect',
        //    name: 'licenseRequestPrintAspect',
        //    buttonSelector: 'licenseReissuanceEditPanel #btnPrint',
        //    codeForm: 'ManOrgLicenseRequest',
        //    getUserParams: function () {
        //        var me = this,
        //            param = { RequestId: me.controller.getContextValue('requestId') };

        //        me.params.userParams = Ext.JSON.encode(param);
        //    }
        //},
        //{
        //    xtype: 'gkhstatepermissionaspect',
        //    name: 'manOrgLicenseRequestStatePermAspect',
        //    permissions: [
        //        { name: 'Gkh.ManOrgLicense.Request.Edit', applyTo: 'b4savebutton', selector: 'licenseReissuanceEditPanel' },
        //        { name: 'Gkh.ManOrgLicense.Request.Field.RegisterNum_Edit', applyTo: '[name=RegisterNum]', selector: 'licenseReissuanceEditPanel' },
        //        {
        //            name: 'Gkh.ManOrgLicense.Request.SubmittedDocs.View', applyTo: 'tabpanel tab[text=Предоставленные документы]', selector: 'licenseReissuanceEditPanel',
        //            applyBy: function (component, allowed) {
        //                if (allowed) component.show();
        //                else component.hide();
        //            }
        //        },
        //        {
        //            name: 'Gkh.ManOrgLicense.Request.SubmittedDocs.Create', applyTo: 'toolbar buttongroup', selector: 'manorglicenseprovdocgrid'
        //        },
        //        {
        //            name: 'Gkh.ManOrgLicense.Request.SubmittedDocs.Edit', applyTo: 'b4savebutton', selector: 'manorgrequestprovdoceditwindow'
        //        },
        //        {
        //            name: 'Gkh.ManOrgLicense.Request.SubmittedDocs.Delete', applyTo: 'b4deletecolumn', selector: 'manorglicenseprovdocgrid',
        //            applyBy: function (component, allowed) {
        //                if (allowed) component.show();
        //                else component.hide();
        //            }
        //        },
        //        {
        //            name: 'Gkh.ManOrgLicense.Request.Reports.Print', applyTo: 'gkhbuttonprint', selector: 'licenseReissuanceEditPanel',
        //            applyBy: function (component, allowed) {
        //                if (allowed) component.show();
        //                else component.hide();
        //            }
        //        }
        //    ]
        //},
        //{
        //    xtype: 'backforwardaspect',
        //    panelSelector: 'personEditPanel',
        //    backForwardController: 'manorglicense.Request'
        //},
        {
            /*
             * Вешаем аспект смены статуса в карточке редактирования
             */
            xtype: 'statecontextbuttonaspect',
            name: 'requestStateButtonAspect',
            stateButtonSelector: 'licenseReissuanceEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var me = this,
                        view = me.controller.getMainView();
                    
                    asp.controller.getAspect('licenseReissuanceEditPanelAspect').setData(entityId);
                    
                    //if (newState && newState.FinalState) {
                    //    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //    //и обновляем панель
                    //    B4.QuickMsg.msg('Создание лицензии', 'Лицензия сформирована успешно', 'success');
                    //    Ext.History.add(Ext.String.format("manorglicense/{0}/{1}/addlicense", view.params.type, entityId));
                    //}
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'licenseReissuanceEditPanelAspect',
            editPanelSelector: 'licenseReissuanceEditPanel',
            modelName: 'licensereissuance.LicenseReissuance',
            otherActions: function (actions) {
                var me = this;
                
                actions['licenseReissuanceEditPanel b4selectfield[name=Contragent]'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
            },
            onBeforeLoadContragent: function (field, options, store) {
                options = options || {};
                options.params = options.params || {};

                options.params.typeJurOrg = 10; // нужны только управляющие организации

                return true;
            },
            listeners: {
                savesuccess: function(asp, rec) {
                    asp.setData(rec.getId());
                },
                aftersetpaneldata: function(asp, rec, panel) {
                    var me = this,
                        personGrid = panel.down('licensereissuancepersongrid'),
                        personStore = personGrid.getStore(),
                        provDocGrid = panel.down('licensereissuanceprovdocgrid'),
                        provDocStore = provDocGrid.getStore(),
                        //annexGrid = panel.down('manorglicenserequestannexgrid'),
                        //annexStore = annexGrid.getStore(),
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
                        url: B4.Url.action('GetContragentInfoById', 'ManOrgLicenseGis'),
                        method: 'POST',
                        params: { requestId: rec.get('Contragent').Id || 0 }
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
                        
                        if (rec.get('Contragent') && rec.get('Contragent').Id) {
                            panel.params.contragentId = rec.get('Contragent').Id;
                            panel.params.contragentName = data.Name;
                        }
                    });

                    me.controller.getAspect('requestStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    //me.controller.getAspect('manOrgLicenseRequestStatePermAspect').setPermissionsByRecord({ getId: function () { return rec.get('Id'); } });
                    
                    personStore.clearFilter(true);
                    personStore.filter('requestId', rec.getId());
                    
                    provDocStore.clearFilter(true);
                    provDocStore.filter('requestId', rec.getId());
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'licensereissuanceprovdocAspect',
            gridSelector: 'licensereissuanceprovdocgrid',
            modelName: 'licensereissuance.LicenseReissuanceProvDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#manorglicenseprovdocMultiSelectWindow',
            storeSelect: 'dict.LicenseProvidedDoc',
            storeSelected: 'dict.LicenseProvidedDoc',
            titleSelectWindow: 'Выбор предоставляемых документов',
            titleGridSelect: 'Документы для выбора',
            titleGridSelected: 'Выбранные документы',
            editFormSelector: 'licensereissuanceprovdoceditwindow',
            editWindowView: 'licensereissuance.RequestProvDocEditWindow',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            saveRecord: function (rec) {
                var me = this,
                    frm = me.getEditForm().getForm();

                rec.set('LicProvidedDoc', me.getEditForm().down('b4selectfield[name=LicProvidedDoc]').getValue());
                rec.set('LicRequest', me.controller.getMainView().params.requestId);

                frm.submit({
                    url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                    params: {
                        records: Ext.encode([rec.getData()])
                    },
                    success: function () {
                        me.updateGrid();
                        me.getEditForm().close();
                    },
                    failure: function (form, action) {
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [],
                        panel = asp.controller.getMainView(),
                        docsGrid = asp.controller.getProvDocGrid();

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', panel);
                        B4.Ajax.request(B4.Url.action('AddProvDocs', 'LicenseReissuancePerson', {
                            provdocIds: Ext.encode(recordIds),
                            requestId: panel.params.lr
                        })).next(function (response) {
                            asp.controller.unmask();
                            docsGrid.getStore().load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать документы');
                        return false;
                    }
                    return true;
                }
            },
            getEditForm: function () {
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
            name: 'licensereissuancepersonAspect',
            gridSelector: 'licensereissuancepersongrid',
            modelName: 'licensereissuance.LicenseReissuancePerson',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#licensereissuancepersonMultiSelectWindow',
            storeSelect: 'licensereissuance.PersonByContragent',
            storeSelected: 'licensereissuance.PersonByContragent',
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
            onBeforeLoad: function (store, operation) {
                var me = this,
                    panel = me.controller.getMainView();
                    //dateRequest = panel.down('datefield[name=DateRequest]').getValue();

                operation.params.contragentId = panel.params.contragentId;
            },
            rowDblClick: function (view, record) {
                if (record.get('Person')) {
                    this.controller.application.redirectTo("personedit/" + record.get('Person'));
                }
            },

            rowAction: function (grid, action, record) {
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
                getdata: function (asp, records) {
                    var recordIds = [],
                        panel = asp.controller.getMainView(),
                        personGrid = asp.controller.getPersonGrid();

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', panel);
                        B4.Ajax.request(B4.Url.action('AddPersons', 'LicenseReissuancePerson', {
                            personIds: Ext.encode(recordIds),
                            requestId: panel.params.lr
                        })).next(function (response) {
                            asp.controller.unmask();
                            personGrid.getStore().load();
                            return true;
                        }).error(function () {
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
                beforesave: function (asp, record) {
                    var me = this,
                        mainView = me.controller.getMainView(),
                        requestId = me.controller.getContextValue(mainView, 'requestId');

                    record.set('LicRequest', requestId);
                },
                deletesuccess: function() {
                    this.controller.getMainView().down('manorglicenserequestannexgrid').getStore().load();
                }
            },
            onSaveSuccess: function (aspect) {
                var form = aspect.getForm();
                if (form) {
                    form.close();
                }

                aspect.getGrid().getStore().load();
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('licenseReissuanceEditPanel');

        view.params = {};
        view.params.lr = id;
        debugger;
        me.bindContext(view);
        me.setContextValue(view, 'id', id);
        me.application.deployView(view);
        me.getAspect('licenseReissuanceEditPanelAspect').setData(id);
    },

    init: function () {
        var me = this;
        me.callParent(arguments);
    },

    goToContragent: function(btn) {
        var panel = btn.up('licenseReissuanceEditPanel');

        if (!panel.params || (panel.params && !panel.params.contragentId)) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать УО!');
            return;
        }
        
        Ext.History.add('contragentedit/' + panel.params.contragentId + '/');
    },

    updateAnnexGrid: function(btn) {
        btn.up('manorglicenserequestannexgrid').getStore().load();
    },
    
    hasChanges: function () {
        return this.getMainComponent().getForm().isDirty();
    }
});