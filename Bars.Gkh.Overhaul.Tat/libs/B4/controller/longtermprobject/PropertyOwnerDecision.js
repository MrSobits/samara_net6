Ext.define('B4.controller.longtermprobject.PropertyOwnerDecision', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.StateButton',
        'B4.view.manorg.Grid',
        'B4.view.regoperator.Grid',
        'B4.controller.realityobj.Navigation',
        'B4.controller.realityobj.ManagOrg',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],


    models: [
        'BasePropertyOwnerDecision',
        'PropertyOwnerDecisionWork',
        'RegOpAccountDecision',
        'SpecialAccountDecision',
        'SpecialAccountDecisionNotice',
        'MinAmountDecision',
        'dict.Work',
        'RealityObject'
    ],
    stores: [
        'BasePropertyOwnerDecision',
        'PropertyOwnerDecisionWork',
        'MinAmountDecision',
        'dict.WorkSelect',
        'dict.WorkSelected'
    ],
    views: [
        'longtermprobject.propertyownerdecision.work.Grid',
        'longtermprobject.propertyownerdecision.Grid',
        'longtermprobject.propertyownerdecision.SpecAccEditWindow',
        'longtermprobject.propertyownerdecision.RegOpEditWindow',
        'longtermprobject.propertyownerdecision.SpecAccNoticePanel',
        'longtermprobject.propertyownerdecision.MinAmountEditWindow',
        'longtermprobject.propertyownerdecision.SpecAccNoticePanel',
        'longtermprobject.propertyownerdecision.AddWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'longtermprobject.propertyownerdecision.Grid',
    mainViewSelector: '#propertyownerdecisionGrid',
    
    refs: [
        {
            ref: 'WorksGrid',
            selector: '#propertyOwnerDecisionWorkGrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'propertyOwnerDecisionGridWindowAspect',
            gridSelector: '#propertyownerdecisionGrid',
            storeName: 'BasePropertyOwnerDecision',
            
            otherActions: function (actions) {
                //добавляем экшены для наших форм
                
                //экшены на форму 
                actions['longtermdecisionspecaccwindow b4savebutton[type="Decision"]'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['longtermdecisionspecaccwindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['longtermdecisionspecaccwindow #sfPropertyOwnerProtocol'] = { 'beforeload': { fn: this.beforeLoadPropertyOwnerProtocol, scope: this } };
                actions['longtermdecisionspecaccwindow #cbTypeOrganization'] = { 'change': { fn: this.onTypeOrganizationChange, scope: this } };
                actions['longtermdecisionspecaccwindow #sfManagingOrganization'] = { 'beforeload': { fn: this.beforeLoadManOrg, scope: this } };
                actions['longtermdecisionspecaccwindow b4selectfield[name="CreditOrg"]'] = { 'change': { fn: this.onChangeCreditOrg, scope: this } };
                actions['longtermdecisionspecaccwindow button[cmd="ToRealObj"]'] = { 'click': { fn: this.onClickToRealObj, scope: this } };
                //экшены на форму 
                actions['longtermdecisionregopwindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['longtermdecisionregopwindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['longtermdecisionregopwindow #sfPropertyOwnerProtocol'] = { 'beforeload': { fn: this.beforeLoadPropertyOwnerProtocol, scope: this } };
                actions['longtermdecisionregopwindow button[cmd="ToRealObj"]'] = { 'click': { fn: this.onClickToRealObj, scope: this } };
                
                actions['longtermdecisionaddwindow combobox[name="MoOrganizationForm"]'] = { 'change': { fn: this.onChangeOrgForm, scope: this } };
                actions['longtermdecisionaddwindow combobox[name="PropertyOwnerDecisionType"]'] = { 'change': { fn: this.onChangeDecType, scope: this } };
                actions['longtermdecisionaddwindow b4savebutton'] = { 'click': { fn: this.saveDecision, scope: this } };
                actions['longtermdecisionaddwindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['longtermdecisionaddwindow button[cmd="ToRealObj"]'] = { 'click': { fn: this.onClickToRealObj, scope: this } };
                
                //экшены на форму 
                actions['longtermdecisionminamountwindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['longtermdecisionminamountwindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['longtermdecisionminamountwindow b4selectfield[name="PropertyOwnerProtocol"]'] = { 'beforeload': { fn: this.beforeLoadPropertyOwnerProtocol, scope: this } };
                actions['longtermdecisionminamountwindow button[cmd="ToRealObj"]'] = { 'click': { fn: this.onClickToRealObj, scope: this } };
            },
            onChangeCreditOrg: function (fld, newValue) {
                
                var form = fld.up(),
                    mailAddrField = form.down('textfield[name="MailingAddress"]'),
                    innField = form.down('textfield[name="Inn"]'),
                    kppField = form.down('textfield[name="Kpp"]'),
                    ogrnField = form.down('textfield[name="Ogrn"]'),
                    okpoField = form.down('textfield[name="Okpo"]'),
                    bikField = form.down('textfield[name="Bik"]'),
                    corrAccountField = form.down('textfield[name="CorrAccount"]');

                if (newValue != null) {
                    mailAddrField.setValue(newValue.MailingAddress);
                    innField.setValue(newValue.Inn);
                    kppField.setValue(newValue.Kpp);
                    ogrnField.setValue(newValue.Ogrn);
                    okpoField.setValue(newValue.Okpo);
                    bikField.setValue(newValue.Bik);
                    corrAccountField.setValue(newValue.CorrAccount);
                } else {
                    mailAddrField.setValue(null);
                    innField.setValue(null);
                    kppField.setValue(null);
                    ogrnField.setValue(null);
                    okpoField.setValue(null);
                    bikField.setValue(null);
                    corrAccountField.setValue(null);
                }

            },

            onClickToRealObj: function () {
                var me = this,
                    portal,
                    params,
                    realObjId = me.controller.params.record.data.RealObjId,
                    realObjModel = me.controller.getModel('RealityObject');

                if (realObjId) {
                    realObjModel.load(realObjId, {
                        success: function(rec) {

                            if (rec) {
                                portal = me.controller.getController('PortalController');

                                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                                if (!me.controller.hideMask) {
                                    me.controller.hideMask = function () { me.controller.unmask(); };
                                }

                                params = new realObjModel({ Id: realObjId, RealityObjName: rec.get('Address') });
                                
                                params.defaultController = 'B4.controller.realityobj.ManagOrg';
                                params.defaultParams = { realityObjectId: realObjId };

                                me.controller.mask('Загрузка', me.controller.getMainComponent());
                                portal.loadController('B4.controller.realityobj.Navigation', params, portal.containerSelector, me.controller.hideMask);
                            }
                        },
                        scope: this
                    });
                }
            },

            onChangeOrgForm: function (field, newValue) {
                var formFundField = field.up('window').down('combobox[name="MethodFormFund"]');

                if (newValue == 10 || newValue == 20) {
                    formFundField.setValue(10);
                    formFundField.setReadOnly(true);
                }
            },
            onChangeDecType: function (field, newValue) {
                var form = field.up(),
                    formFundField = form.down('combobox[name="MethodFormFund"]'),
                    orgForm = form.down('combobox[name="MoOrganizationForm"]').getValue();

                if (newValue == 20) {
                    formFundField.setValue(null);
                    formFundField.setReadOnly(true);
                    formFundField.allowBlank = true;
                } else if (orgForm == 10 || orgForm == 20) {
                    formFundField.setValue(10);
                    formFundField.setReadOnly(true);
                } else {
                    formFundField.setReadOnly(false);
                    formFundField.allowBlank = false;
                }
                
                formFundField.validate();
            },
            saveDecision: function () {
                var me = this, rec, form = this.getForm();
                if (form.getForm().isValid()) {
                    form.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(form.getRecord());
                    me.controller.mask('Загрузка...', form);

                    B4.Ajax.request({
                        url: B4.Url.action('SaveDecision', 'LongTermPrObject'),
                        params: {
                            record: Ext.encode(rec.data),
                            longTermObjId: me.controller.params.longTermObjId
                        }
                    }).next(function(resp) {
                        me.controller.unmask();
                        me.getGrid().getStore().load();
                        form.close();
                    }).error(function() {
                        me.controller.unmask();
                    });
                }
            },

            beforeLoadPropertyOwnerProtocol: function(sender,options) {
                options.params = {};
                options.params.ltpObjectId = this.controller.params.longTermObjId;
            },
            
            beforeLoadManOrg: function (sender, options) {
                var typeManOrg = this.getForm().down('#cbTypeOrganization').getValue();

                options.params = {};
                options.params.tsjOnly = typeManOrg == 20;
                options.params.jskOnly = typeManOrg == 40;
            },
            
            onTypeOrganizationChange: function (sender, newVal) {
                if (Ext.isEmpty(newVal)) {
                    return;
                }
                
                var editWindow = sender.up();

                this.editWindowElementsMovement(editWindow, newVal);
            },
            
            getModel: function (record) {
                //поскольку наш аспект работает с разными моделями мы подсовываем модель в зависимости от типа

                if (record) {
                    var typeDec = record.get('Decision'),
                        methodFormFund = record.get('MethodFormFund');

                    this.setParams(typeDec, methodFormFund);
                } else {
                    this.modelName = 'BasePropertyOwnerDecision';
                    this.editFormSelector = 'longtermdecisionaddwindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.AddWindow';
                }

                return this.controller.getModel(this.modelName);
            },

            setParams: function (typeDec, methodFormFund) {
                if (typeDec == 20) {
                    this.modelName = 'MinAmountDecision';
                    this.editFormSelector = 'longtermdecisionminamountwindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.MinAmountEditWindow';
                } else if (methodFormFund == 10) {
                    this.modelName = 'RegOpAccountDecision';
                    this.editFormSelector = 'longtermdecisionregopwindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.RegOpEditWindow';
                } else {
                    this.modelName = 'SpecialAccountDecision';
                    this.editFormSelector = 'longtermdecisionspecaccwindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.SpecAccEditWindow';
                }
            },
            
            editWindowElementsMovement: function(editWindow, typeOrganization) {
                if (editWindow) {
                    var sfRegOperator = editWindow.down('#sfRegOperator');
                    var sfManagingOrganization = editWindow.down('#sfManagingOrganization');

                    if (sfRegOperator && sfManagingOrganization) {
                        sfRegOperator.setDisabled(false);

                        sfRegOperator.setValue(0);
                        sfManagingOrganization.setValue(0);

                        if (typeOrganization == 20 || typeOrganization == 40) {
                            sfRegOperator.hide();
                            sfRegOperator.allowBlank = true;
                            sfManagingOrganization.show();
                            sfManagingOrganization.allowBlank = false;
                        } else if (typeOrganization == 60) {
                            sfRegOperator.show();
                            sfRegOperator.allowBlank = false;
                            sfRegOperator.setReadOnly(false);
                            sfManagingOrganization.hide();
                            sfManagingOrganization.allowBlank = true;
                        } else if (typeOrganization == 50) {
                            sfRegOperator.show();
                            sfRegOperator.allowBlank = true;
                            sfRegOperator.setValue(null);
                            sfRegOperator.setReadOnly(true);
                            sfManagingOrganization.hide();
                            sfManagingOrganization.allowBlank = true;
                        }
                    }
                }
            },
            
            setOrgFormData: function (editWindow) {
                var me = this;
                if (editWindow) {
                    var orgFormField = editWindow.down('combobox[name="MoOrganizationForm"]');
                    me.controller.mask('Загрузка...', editWindow);

                    B4.Ajax.request({
                        url: B4.Url.action('GetOrgForm', 'LongTermPrObject'),
                        params: {
                            realObjId: me.controller.params.record.data.RealObjId
                        }
                    }).next(function (resp) {
                        var res = Ext.decode(resp.responseText);
                        orgFormField.setValue(res);
                        me.controller.unmask();
                    }).error(function (e) {
                        B4.QuickMsg.msg('Предупреждение', e.message, 'warning');
                        me.controller.unmask();
                    });
                }
            },

            listeners: {
                beforesetformdata: function (asp, record) {
                    if (record.phantom) {
                        this.setOrgFormData(asp.getForm());
                    } else {
                        asp.controller.params.decisionId = record.getId();
                        
                        if (record.get('MethodFormFund') == 20) {
                            var aspNotice = this.controller.getAspect('longTermSpecAccNoticePanelAspect');

                            var model = aspNotice.getModel();
                            model.load(record.getId(), {
                                success: function(rec) {
                                    aspNotice.setPanelData(rec);
                                },
                                scope: this
                            });
                        }
                    }
                },
                beforesave: function (asp, obj) {
                    obj.set('LongTermPrObject', asp.controller.params.longTermObjId);
                    return true;
                }
            }
        },
        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'longTermSpecAccNoticeAspect',
            stateButtonSelector: 'longtermdecisionspecaccnoticepanel #btnState',
            listeners: {
                transfersuccess: function (asp) {
                    asp.controller.getAspect('longTermSpecAccNoticePanelAspect').setData(asp.controller.params.decisionId);                   
                }
            }
        },
        {
            // Аспект уведомления
            xtype: 'gkheditpanel',
            name: 'longTermSpecAccNoticePanelAspect',
            editPanelSelector: 'longtermdecisionspecaccnoticepanel',
            modelName: 'SpecialAccountDecisionNotice',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    this.controller.getAspect('longTermSpecAccNoticeAspect').setStateData(rec.get('Id'), rec.get('State'));
                },
                beforesave: function(asp, rec) {
                    if (Ext.isEmpty(rec.get('SpecialAccountDecision'))) {
                        rec.set('SpecialAccountDecision', asp.controller.params.decisionId);
                    }

                    return true;
                }
            },
            saveRecordHasUpload: function (rec) {
                var me = this,
                    panel = me.getPanel();

                me.mask('Сохранение', panel);
                panel.submit({
                    url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                    params: {
                        records: Ext.encode([rec.getData()])
                    },
                    success: function () {
                        me.unmask();
                        var model = me.getModel();
  
                        model.load(me.controller.params.decisionId, {
                            success: function (newRec) {
                                me.onPreSaveSuccess(me, newRec);
                                me.setPanelData(newRec);
                            }
                        });
                    },
                    failure: function (form, action) {
                        me.unmask();
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'propertyOwnerDecisionWorksGridAspect',
            gridSelector: '#propertyOwnerDecisionWorkGrid',
            storeName: 'PropertyOwnerDecisionWork',
            modelName: 'PropertyOwnerDecisionWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#propertyOwnerDecisionWorksMultiSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddWorks', 'PropertyOwnerDecisionWork'),
                            method: 'POST',
                            params: {
                                objectIds: recordIds,
                                propertyOwnerDecisionId: asp.controller.propertyOwnerDecisionId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Create', applyTo: 'b4addbutton', selector: '#propertyownerdecisionGrid' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit', applyTo: 'b4savebutton', selector: 'longtermdecisionspecaccwindow' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit', applyTo: 'b4savebutton', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit', applyTo: 'b4savebutton', selector: 'longtermdecisionregopwindow' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit', applyTo: 'b4addbutton', selector: '#propertyOwnerDecisionWorkGrid' },
                {name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit', applyTo: 'b4deletecolumn', selector: '#propertyOwnerDecisionWorkGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Delete', applyTo: 'b4deletecolumn', selector: '#propertyownerdecisionGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }      
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Save', applyTo: 'b4savebutton', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.File', applyTo: 'b4filefield[name="File"]', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.NoticeNumber', applyTo: 'datefield[name="NoticeDate"]', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.RegDate', applyTo: 'datefield[name="RegDate"]', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.GjiNumber', applyTo: 'textfield[name="GjiNumber"]', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.HasOriginal', applyTo: 'checkbox[name="HasOriginal"]', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.HasCopyCertificate', applyTo: 'checkbox[name="HasCopyCertificate"]', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.HasCopyProtocol', applyTo: 'checkbox[name="HasCopyProtocol"]', selector: 'longtermdecisionspecaccnoticepanel' }
            ],
            name: 'decisionNoticeStatePerm',
            editFormAspectName: 'longTermSpecAccNoticePanelAspect'
        }
    ],

    init: function () {
        this.getStore('BasePropertyOwnerDecision').on('beforeload', this.onBeforeLoad, this);
        this.getStore('PropertyOwnerDecisionWork').on('beforeload', this.onBeforeLoadWorkStore, this);
        
        this.callParent(arguments);
    },

    onLaunch: function () {
        var me= this, model, asp;
        
        me.getStore('BasePropertyOwnerDecision').load();

        if (me.params.decision) {
            model = me.getModel('SpecialAccountDecision'),
            model.load(me.params.decision, {
                success: function (rec) {
                    asp = me.getAspect('propertyOwnerDecisionGridWindowAspect');
                    asp.getModel(rec);
                    asp.setFormData(rec);
                }
            });
        }

        this.testForMethodFormFundCr();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.longTermObjId) {
            operation.params.ltpObjectId = this.params.longTermObjId;
        }
    },
    
    onBeforeLoadWorkStore: function (store, operation) {
        operation.params.propertyOwnerDecisionId = this.propertyOwnerDecisionId;
    },
   
    testForMethodFormFundCr: function () {
        if (this.params && this.params.record && this.params.record.get('MethodFormFundCr')) {

            var methodFormFundCr = this.params.record.get('MethodFormFundCr');

            var addButton = this.getMainView().down('b4addbutton');
            
            if (addButton) {
                addButton.setDisabled(methodFormFundCr != 10 && methodFormFundCr != 20);
            } 
        }
    },
    
    setCurrentId: function (id) {
        this.propertyOwnerDecisionId = id;
        var grid = this.getWorksGrid(),
            store = this.getStore('PropertyOwnerDecisionWork');
        store.removeAll();

        if (grid) {
            grid.setDisabled(id == 0);
        }

        if (id > 0) {
            store.load();
        }
    }
});