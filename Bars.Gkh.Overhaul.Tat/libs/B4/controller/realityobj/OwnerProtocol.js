Ext.define('B4.controller.realityobj.OwnerProtocol', {
    extend: 'B4.controller.MenuItemController',

    params: {},

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GridEditWindow',
        'B4.enums.TypeOrganization',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.aspects.permission.realityobj.OwnerProtocol'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'BasePropertyOwnerDecision',
        'PropertyOwnerProtocols',
        'PropertyOwnerDecisionWork',
        'MinAmountDecision',
        'dict.WorkSelect',
        'dict.WorkSelected',
        'OwnerAccountContragent'
    ],

    models: [
        'PropertyOwnerProtocols',
        'BasePropertyOwnerDecision',
        'PropertyOwnerDecisionWork',
        'RegOpAccountDecision',
        'SpecialAccountDecision',
        'SpecialAccountDecisionNotice',
        'MinAmountDecision',
        'dict.Work',
        'RealityObject',
        'ListServicesDecision',
        'MinFundSizeDecision',
        'PrevAccumulatedAmountDecision',
        'CreditOrganizationDecision',
        'OwnerAccountDecision'
    ],

    views: [
        'realityobj.protocol.Grid',
        'realityobj.protocol.Panel',
        'realityobj.protocol.DecisionGrid',
        'longtermprobject.propertyownerprotocols.EditWindow',
        'longtermprobject.propertyownerdecision.work.Grid',
        'longtermprobject.propertyownerdecision.Grid',
        'longtermprobject.propertyownerdecision.SpecAccEditWindow',
        'longtermprobject.propertyownerdecision.RegOpEditWindow',
        'longtermprobject.propertyownerdecision.MinAmountEditWindow',
        'longtermprobject.propertyownerdecision.ListServicesEditWindow',
        'longtermprobject.propertyownerdecision.OwnerAccountEditWindow',
        'longtermprobject.propertyownerdecision.CreditOrgEditWindow',
        'longtermprobject.propertyownerdecision.MinFundSizeEditWindow',
        'longtermprobject.propertyownerdecision.PreAmountEditWindow',
        'longtermprobject.propertyownerdecision.SpecAccNoticePanel',
        'longtermprobject.propertyownerdecision.AddWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    refs: [
        {
            ref: 'mainView',
            selector: 'roprotpanel'
        },
        {
            ref: 'grid',
            selector: 'roprotocolgrid'
        },
        {
            ref: 'decisionGrid',
            selector: 'roprotdecisiongrid'
        },
        {
            ref: 'decisionAddWindow',
            selector: 'longtermdecisionaddwindow'
        }
    ],

    aspects: [
        {
            xtype: 'ownerprotocolperm',
            name: 'ownerProtocolPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'protocolaspect',
            gridSelector: 'roprotocolgrid',
            editFormSelector: '#propertyownerprotocolsEditWindow',
            storeName: 'PropertyOwnerProtocols',
            modelName: 'PropertyOwnerProtocols',
            editWindowView: 'longtermprobject.propertyownerprotocols.EditWindow',
            otherActions: function(actions) {
                var me = this;

                actions['propertyownerprotocolseditwin [name=TypeProtocol]'] = { change: { fn: me.onChangeTypeProtocol, scope: me } };
                actions['propertyownerprotocolseditwin [name=FormVoting]'] = { change: { fn: me.onChangeFormVoting, scope: me } };
            },
            listeners: {
                beforesave: function (asp, obj) {
                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.getContextValue(asp.controller.getMainView(), 'realityObjectId'));
                    }
                    return true;
                }
            },
            onChangeTypeProtocol: function (fld, newValue) {
                var me = this,
                    win = fld.up('window'),
                    isBoardProtocol = newValue == B4.enums.PropertyOwnerProtocolType.ResolutionOfTheBoard,
                    isLoanProtocol = newValue == B4.enums.PropertyOwnerProtocolType.Loan,
                    isFundProtocol = newValue == B4.enums.PropertyOwnerProtocolType.FormationFund,
                    votingDetails = win.down('[name=VotingDetails]'),
                    npaPanel = win.down('protocolnpapanel'),
                    propertyPerm = me.controller.getAspect('propertyOwnerProtocolPerm'),
                    record = win.getRecord();

                if (propertyPerm) {
                    propertyPerm.loadPermissions(record)
                        .next(function (response) {
                            var grants = Ext.decode(response.responseText);
                            npaPanel.tab.setVisible(isBoardProtocol && grants[0] === 1);
                            npaPanel.setDisabled(!isBoardProtocol && grants[0] === 1);
                            win.getForm().isValid();
                        }, me);
                }

                votingDetails.setVisible(isFundProtocol);
                votingDetails.setDisabled(!isFundProtocol);


                win.down('[name=NumberOfVotes]').setDisabled(isBoardProtocol || isLoanProtocol);
                win.down('[name=TotalNumberOfVotes]').setDisabled(isBoardProtocol || isLoanProtocol);
                win.down('[name=PercentOfParticipating]').setDisabled(isBoardProtocol || isLoanProtocol);

                win.down('[name=DocumentNumber]').allowBlank = isLoanProtocol;
                win.down('[name=DocumentDate]').allowBlank = isLoanProtocol;
                win.down('[name=DocumentFile]').allowBlank = isLoanProtocol;

                win.down('[name=LoanAmount]').setVisible(isLoanProtocol);
                win.down('[name=Borrower]').setVisible(isLoanProtocol);
                win.down('[name=Lender]').setVisible(isLoanProtocol);

                win.down('[name=Description]').setVisible(!isLoanProtocol);
                win.down('[name=Parameters]').setVisible(!isLoanProtocol);

                win.getForm().isValid();
            },
            onChangeFormVoting: function (combo, newVal, oldVal) {
                if (oldVal == newVal || !newVal) {
                    return;
                }

                var win = combo.up('window'),
                    notUseSystemFieldSet = win.down('fieldset[votingUseSystem=false]'),
                    useSystemFieldSet = win.down('fieldset[votingUseSystem=true]'),
                    notUse = newVal === B4.enums.FormVoting.Extramural ||
                        newVal === B4.enums.FormVoting.Intramural ||
                        newVal === B4.enums.FormVoting.FullTime;

                if (notUse) {
                    notUseSystemFieldSet.setVisible(true);
                    notUseSystemFieldSet.setDisabled(false);

                    useSystemFieldSet.setVisible(false);
                    useSystemFieldSet.setDisabled(true);
                } else {
                    notUseSystemFieldSet.setVisible(false);
                    notUseSystemFieldSet.setDisabled(true);

                    useSystemFieldSet.setVisible(true);
                    useSystemFieldSet.setDisabled(false);
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'decisionaspect',
            gridSelector: 'roprotdecisiongrid',
            storeName: 'BasePropertyOwnerDecision',
            otherActions: function (actions) {
                var me = this;

                me.controller.setContextValue(me.controller.getMainView(), 'initialScope', me);

                actions['longtermdecisionaddwindow b4savebutton'] = { click: { fn: me.saveDecision, scope: me } };
                actions['longtermdecisionaddwindow b4closebutton'] = { click: { fn: me.closeWindowHandler, scope: me } };
                actions['longtermdecisionaddwindow combobox[name="MoOrganizationForm"]'] = { 'change': { fn: me.onChangeOrgForm, scope: me } };
                actions['longtermdecisionaddwindow combobox[name="PropertyOwnerDecisionType"]'] = { 'change': { fn: me.onChangeDecType, scope: me } };

                actions['longtermdecisionspecaccwindow #cbTypeOrganization'] = { 'change': { fn: me.onTypeOrganizationChange, scope: me } };
                actions['longtermdecisionspecaccwindow #sfManagingOrganization'] = { 'beforeload': { fn: me.beforeLoadManOrg, scope: me } };
                //actions['longtermdecisionspecaccwindow b4selectfield[name="CreditOrg"]'] = { 'change': { fn: me.onChangeCreditOrg, scope: me } };
                actions['longtermdecisionspecaccwindow b4savebutton[type="Decision"]'] = { 'click': { fn: me.saveRequestHandler, scope: me } };
                actions['longtermdecisionspecaccwindow b4closebutton'] = { 'click': { fn: me.closeWindowHandler, scope: me } };
                
                actions['longtermdecisionowneraccountwindow'] = { 'show': { fn: me.onContragentAllowBlank, scope: me } };
                actions['longtermdecisionowneraccountwindow combobox[name="OwnerAccountType"]'] = { 'change': { fn: me.onChangeOwnerAccountType, scope: me } };
                actions['longtermdecisionowneraccountwindow b4selectfield[name="Contragent"]'] = { 'beforeload': { fn: me.beforeLoadPropertyOwnerAccountContragent, scope: me } };

                actions['longtermdecisionlistserviceswindow combobox'] = {
                    'render': {
                        fn: function (grid) {
                            grid.getStore().on('beforeload', me.onWorkStoreBeforeLoad, me);
                        },
                        scope: this
                    }
                };
                actions['longtermdecisionlistserviceswindow listservicesworksgrid'] = {
                    'render': {
                        fn: function (grid) {
                            grid.getStore().on('beforeload', me.onBeforeLoadListServiceWorksGrid, me);
                        },
                        scope: this
                    }
                };

                Ext.each(me.getWindowSelectors(), function (selector) {
                    actions[selector + ' b4savebutton'] = { click: { fn: me.saveRequestHandler, scope: me } };
                    actions[selector + ' b4closebutton'] = { click: { fn: me.closeWindowHandler, scope: me } };
                });
            },
            listeners: {
                beforesetformdata: function (asp, record, form) {
                    var aspNotice, model;
                    if (record.phantom) {
                        this.setOrgFormData(form);
                    } else {
                        asp.controller.params.decisionId = record.getId();

                        if (record.get('MethodFormFund') == 20) {
                            aspNotice = asp.controller.getAspect('longTermSpecAccNoticePanelAspect');

                            var moOrganizationForm = record.get('MoOrganizationForm');
                            var cbTypeOrganization = form.down('#cbTypeOrganization');
                            
                            if (cbTypeOrganization && cbTypeOrganization.store) {
                                cbTypeOrganization.store.removeAll();
                                
                                cbTypeOrganization.store.add(B4.enums.TypeOrganization.getStore().findRecord('Value', 60));

                                var item = B4.enums.TypeOrganization.getStore().findRecord('Value', moOrganizationForm == 30 ? 20 : moOrganizationForm);
                                
                                if (item) {
                                    cbTypeOrganization.store.add(item);
                                }
                            }

                            model = aspNotice.getModel();
                            model.load(record.getId(), {
                                success: function(rec) {
                                    aspNotice.setPanelData(rec);
                                },
                                scope: this
                            });
                        }

                        form.setTitle(B4.enums.PropertyOwnerDecisionType.displayRenderer(record.get('PropertyOwnerDecisionType')));
                    }
                },
                aftersetformdata: function (asp, rec, form) {
                    var decisionType = rec.get('PropertyOwnerDecisionType');

                    if(asp.controller.params.decisId) {
                        form.down('.tabpanel').setActiveTab(1);
                        delete asp.controller.params.decisId;
                    }

                    asp.controller.params.decisionId = rec.getId();
                    if (decisionType === 30) {
                        var grid = form.down('listservicesworksgrid');
                        grid.getStore().load();
                    }

                    if (rec.get('MethodFormFund') == 20) {
                        this.setManagingOrganization(form, rec.get('TypeOrganization'));
                    }
                },
                beforesave: function (asp, obj) {
                    var result = true,
                        store;
                    
                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.getContextValue(asp.controller.getMainView(), 'realityObjectId'));
                        obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                    }
                    
                    if (obj.get('PropertyOwnerDecisionType') == 60) {
                        if (obj.get('SubjectMinFundSize') && obj.get('OwnerMinFundSize') < obj.get('SubjectMinFundSize')) {
                            Ext.Msg.alert('Ошибка сохранения', 'Размер фонда на КР не может быть меньше минимального размера фонда КР, установленного субъектом');
                            return false;
                        }
                    }
                    else if (obj.get('PropertyOwnerDecisionType') == 30) {
                        store = asp.getForm().down('listservicesworksgrid').getStore();

                        Ext.each(store.data.items, function (item) {
                            var planYear = item.get('PlanYear'),
                                factYear = item.get('FactYear');

                            if (planYear != '-' && factYear && planYear < factYear) {
                                result = false;
                            }

                            return result;
                        });

                        if (!result) {
                            Ext.Msg.alert('Ошибка сохранения', 'Фактический срок проведения работ не может быть позднее планового');
                            return false;
                        }
                    }
                    return true;
                },
                savesuccess: function (asp, rec) {
                    var decisionType = rec.get('PropertyOwnerDecisionType');
                    if (decisionType === 30) {
                        asp.getForm().down('listservicesworksgrid').getStore().sync();
                    }
                }
            },
            onBeforeLoadListServiceWorksGrid: function (sender, options) {
                options.params = {};
                options.params.decisionId = this.controller.params.decisionId;
            },
            /*onChangeCreditOrg: function (fld, newValue) {

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
            },*/
            beforeLoadManOrg: function (sender, options) {
                var typeManOrg = this.getForm().down('#cbTypeOrganization').getValue();

                options.params = {};
                options.params.fromDecision = true;
                options.params.tsjOnly = typeManOrg == 20;
                options.params.jskOnly = typeManOrg == 40;
            },
            editWindowElementsMovement: function (editWindow, typeOrganization) {
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
                        else if (typeOrganization == 10) {
                            sfRegOperator.hide();
                            sfRegOperator.allowBlank = true;
                            sfManagingOrganization.show();
                            sfManagingOrganization.allowBlank = false;
                        }
                    }
                }
            },
            setManagingOrganization: function (editWindow, typeOrganization) {
                if (!editWindow) {
                    return;
                }
                
                var me = this,
                    sfManagingOrganization = editWindow.down('#sfManagingOrganization');
              
                if (!sfManagingOrganization || sfManagingOrganization.getValue()) {
                    return;
                }
                
                B4.Ajax.request({
                    url: B4.Url.action('GetManagingOrganization', 'LongTermPrObject'),
                    params: {
                        realObjId: me.controller.params.realityObjectId,
                        typeOrganization: typeOrganization
                    }
                }).next(function (resp) {
                    var res = Ext.decode(resp.responseText);
                    sfManagingOrganization.setValue(res);
                });
            },
            onTypeOrganizationChange: function (sender, newVal, oldVal) {
                if (Ext.isEmpty(newVal)) {
                    return;
                }

                var editWindow = sender.up();

                this.editWindowElementsMovement(editWindow, newVal);
                
                if (oldVal && (newVal == 20 || newVal == 40)) {
                    this.setManagingOrganization(editWindow, newVal);
                }                    
            },
            onChangeOwnerAccountType: function (field, newValue) {
                var form = field.up(),
                    sfContragent = form.down('b4selectfield[name="Contragent"]');

                sfContragent.allowBlank = newValue == 50;
                sfContragent.validate();
            },
            onContragentAllowBlank: function () {
                this.getForm().down('#sfContragent').allowBlank = true;
            },
            beforeLoadPropertyOwnerAccountContragent: function (field, options) {
                var form = field.up(),
                    valueType = form.down('combobox[name="OwnerAccountType"]').getValue();

                options.params = {};
                options.params.typeOwner = valueType;
            },
            onChangeOrgForm: function (field, newValue) {
                var formFundField = field.up('window').down('combobox[name="MethodFormFund"]');

                if (newValue == 10 || newValue == 20) {
                    formFundField.setValue(10);
                    formFundField.setReadOnly(true);
                }
            },
            onWorkStoreBeforeLoad: function (store) {
                var id = this.getForm().getRecord().getId();

                Ext.apply(store.getProxy().extraParams, { decision_id: id });
            },
            saveDecision: function () {
                var me = this, rec, form = this.getForm();
                if (form.getForm().isValid()) {
                    form.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(form.getRecord());
                    me.controller.mask('Сохранение...', me.controller.getMainView());
                    rec.set('PropertyOwnerProtocol', me.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                    rec.set('RealityObject', me.controller.params.realityObjectId);

                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('SaveDecision', 'LongTermPrObject'),
                        params: {
                            records: Ext.encode([rec.data])
                        }
                    }).next(function () {
                        me.controller.unmask();
                        me.getGrid().getStore().load();
                        form.close();
                    }).error(function () {
                        me.controller.unmask();
                    });
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
                            realObjId: me.controller.params.realityObjectId
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
            onChangeDecType: function (field, newValue) {
                var me = this,
                    form = field.up(),
                    formFundField = form.down('combobox[name="MethodFormFund"]'),
                    orgForm = form.down('combobox[name="MoOrganizationForm"]').getValue();

                if (newValue == 20 || newValue == 30 || newValue == 40
                   || newValue == 50 || newValue == 60 || newValue == 70) {
                    //formFundField.setValue(null);
                    formFundField.setReadOnly(true);
                    formFundField.allowBlank = true;
                } else {
                    formFundField.setReadOnly(false);
                    formFundField.allowBlank = false;
                }

                formFundField.validate();
            },
            getModel: function (record) {
                var me = this,
                    initialScope = me.controller.getContextValue(me.controller.getMainView(), 'initialScope');
                    
                //поскольку наш аспект работает с разными моделями мы подсовываем модель в зависимости от типа
                if (record) {
                    var typeDec = record.get('Decision'),
                        methodFormFund = record.get('MethodFormFund');

                    this.setParams(typeDec, methodFormFund);
                } 
                else
                {
                    var form = me.editFormSelector ? me.componentQuery(me.editFormSelector) : null;

                    //заходим сюда при сохранении записей в гриде
                    if (form) {
                        var rec = form.getRecord();
                        return me.controller.getModel(rec.modelName);
                    }
                    else {
                        this.modelName = 'BasePropertyOwnerDecision';
                        this.editFormSelector = 'longtermdecisionaddwindow';
                        this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.AddWindow';
                    }
                }

                if (initialScope) {
                    initialScope.modelName = this.modelName;
                    initialScope.editFormSelector = this.editFormSelector;
                    initialScope.editWindowView = this.editWindowView;
                }

                return this.controller.getModel(this.modelName);     
            },
            setParams: function (typeDec, methodFormFund) {
                if (typeDec == 70) {
                    this.modelName = 'PrevAccumulatedAmountDecision';
                    this.editFormSelector = 'longtermdecisionpreamountwindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.PreAmountEditWindow';
                } else if (typeDec == 60) {
                    this.modelName = 'MinFundSizeDecision';
                    this.editFormSelector = 'longtermdecisionminfundsizewindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.MinFundSizeEditWindow';
                } else if (typeDec == 50) {
                    this.modelName = 'CreditOrganizationDecision';
                    this.editFormSelector = 'longtermdecisioncreditorgwindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.CreditOrgEditWindow';
                } else if (typeDec == 40) {
                    this.modelName = 'OwnerAccountDecision';
                    this.editFormSelector = 'longtermdecisionowneraccountwindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.OwnerAccountEditWindow';
                } else if (typeDec == 30) {
                    this.modelName = 'ListServicesDecision';
                    this.editFormSelector = 'longtermdecisionlistserviceswindow';
                    this.editWindowView = 'B4.view.longtermprobject.propertyownerdecision.ListServicesEditWindow';
                } else if (typeDec == 20) {
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
            getWindowSelectors: function() {
                return [
                    'longtermdecisionregopwindow',
                    'longtermdecisionminamountwindow',
                    'longtermdecisionlistserviceswindow',
                    'longtermdecisionowneraccountwindow',
                    'longtermdecisionminfundsizewindow',
                    'longtermdecisionpreamountwindow',
                    'longtermdecisioncreditorgwindow'
                ];
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
                beforesave: function (asp, rec) {
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
            name: 'propertyOwnerProtocolPerm',
            permissions: [
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Create', applyTo: 'b4addbutton', selector: 'roprotocolgrid' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Edit', applyTo: 'b4savebutton', selector: '#propertyownerprotocolsEditWindow' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Delete', applyTo: 'b4deletecolumn', selector: 'roprotocolgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Create', applyTo: 'b4addbutton', selector: 'roprotdecisiongrid' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit', applyTo: 'b4savebutton', selector: 'longtermdecisionspecaccwindow' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit', applyTo: 'b4savebutton', selector: 'longtermdecisionspecaccnoticepanel' },
                { name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit', applyTo: 'b4savebutton', selector: 'longtermdecisionregopwindow' },
                {
                    name: 'Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Delete', applyTo: 'b4deletecolumn', selector: 'roprotdecisiongrid',
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
        var me = this;
        me.control({
            'roprotocolgrid': {
                render: function(grid) {
                    grid.getStore().on('beforeload', me.onBeforeLoadDecision, me);
                },
                select: {
                    fn: me.onSelectProtocol,
                    scope: me
                },
                deselect: {
                    fn: function() {
                        var decisionGrid = me.getDecisionGrid();
                        decisionGrid.setTitle('Решения протокола');
                        decisionGrid.getStore().removeAll();
                        decisionGrid.setDisabled(true);
                    },
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view, model,
            realityObjectId = me.params.realityObjectId = id;

        view = me.getMainView() || Ext.widget('roprotpanel');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.getAspect('ownerProtocolPerm').setPermissionsByRecord({ getId: function () { return realityObjectId; } });

        if (me.params.decisId) {
            model = me.getModel('SpecialAccountDecisionNotice');
            me.getAspect('decisionaspect').editRecord(new model({ Id: me.params.decisId }));
        }

        me.getGrid().getStore().load();
        me.getDecisionGrid().getStore().removeAll();
    },

    onBeforeLoadDecision: function (store, operation) {
        var me = this;
        operation.params.roId = me.getContextValue(me.getMainView(), 'realityObjectId');
    },

    onSelectProtocol: function(grid, record) {
        var me = this,
            decisionGrid = me.getDecisionGrid(),
            decisionStore = decisionGrid.getStore(),
            docNumber = record.get('DocumentNumber'),
            docDate = record.get('DocumentDate'),
            typeProtocol = record.get('TypeProtocol'),
            title = 'Решения протокола';

        if (!Ext.isEmpty(docNumber)) {
            title += ' №' + docNumber;
        }

        if (!Ext.isEmpty(docDate)) {
            title += ' от ' + Ext.util.Format.dateRenderer('d.m.Y')(docDate);
        }
        
        decisionGrid.setTitle(title);
        if (typeProtocol != 10 && typeProtocol != 30) {
            decisionGrid.setDisabled(false);
            decisionStore.clearFilter(true);
            decisionStore.filter('protocolId', record.getId());
        } else {
            decisionGrid.setDisabled(true);
            decisionStore.removeAll();
        }
    }
});