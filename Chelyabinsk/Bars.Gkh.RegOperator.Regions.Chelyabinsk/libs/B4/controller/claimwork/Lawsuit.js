Ext.define('B4.controller.claimwork.Lawsuit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.ClaimWorkDocument',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocCreateButton',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.ClaimWorkButtonPrintAspect',
        'B4.aspects.DocumentClwAccountDetailAspect',
        'B4.enums.LawsuitDocumentType',
        'B4.enums.LawsuitFactInitiationType',
        'B4.enums.ClaimWork.TypeLawsuitDocument',
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.enums.DebtorType',
        'B4.form.EnumCombo',
        'B4.aspects.CourtClaimEditPanelAspect',
        'B4.store.regop.owner.IndividualAccountOwner',
        'B4.store.regop.owner.LegalAccountOwner',
        'B4.aspects.permission.Lawsuit'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'claimwork.lawsuit.Doc',
        'claimwork.lawsuit.Court',
        'claimwork.lawsuit.LawsuitIndividualOwnerInfo',
        'claimwork.lawsuit.LawsuitLegalOwnerInfo',
        'claimwork.LawSuitDebtWorkSSPDocument',
        'claimwork.LawSuitDebtWorkSSP',
        'LawsuitReferenceCalculation'
    ],

    parentDoc: null,
    parentDocSSP: null,

    models: [
        'claimwork.lawsuit.Lawsuit',
        'claimwork.lawsuit.Petition',
        'claimwork.lawsuit.CourtOrderClaim',
        'claimwork.lawsuit.Doc',
        'claimwork.lawsuit.Court',
        'claimwork.lawsuit.LawsuitIndividualOwnerInfo',
        'claimwork.lawsuit.LawsuitLegalOwnerInfo',
        'claimwork.LawSuitDebtWorkSSPDocument',
        'claimwork.LawSuitDebtWorkSSP',
        'LawsuitReferenceCalculation'
    ],

    views: [
        'claimwork.lawsuit.EditPanel',
        'claimwork.lawsuit.DocEditWindow',
        'claimwork.lawsuit.CourtEditWindow',
        'claimwork.lawsuit.CourtGrid',
        'claimwork.lawsuit.DocGrid',
        'claimwork.lawsuit.DocumentationGrid',
        'claimwork.lawsuit.DocumentationEditWindow',
        'claimwork.lawsuit.LawsuitOwnerInfoWindow',
        'claimwork.lawsuit.LawsuitOwnerInfoGrid',
        'claimwork.LawSuitDebtWorkSSPGrid',
        'claimwork.LawSuitDebtWorkSSPEditWindow',
        'claimwork.LawSuitDebtWorkSSPDocumentGrid',
        'claimwork.LawSuitDebtWorkSSPDocumentEditWindow',
        'claimwork.lawsuit.LawsuitReferenceCalculationGrid',
        'claimwork.DebtorPaymentsWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'clwlawsuiteditpanel'
        },
        {
            ref: 'ownerInfoView',
            selector: 'lawsuitownerinfogrid'
        },
        {
            ref: 'lawsuitsspView',
            selector: 'lawsuitsspgrid'
        },
        {
            ref: 'referenceCalculationView',
            selector: 'lawsuitreferencecalculationgrid'
        },
    ],

    aspects: [
        {
            xtype: 'lawsuitpermission'
        },
        //#region Court claim
        {
            xtype: 'courtclaimeditpanelaspect',
            name: 'courtClaimEditPanelAspect',
            editPanelSelector: 'clwlawsuitcourtclaiminfopanel',
            modelName: 'claimwork.lawsuit.CourtOrderClaim',
            docCreateAspectName: 'courtClaimAcceptMenuAspect',
            listeners: {
                beforesetpaneldata: function (asp, record) {
                    var panel = asp.getPanel();
                    if (!record) {
                        asp.controller.getAspect('lawsuitOwnerPrintAspect').loadReportStore();
                        asp.controller.getMainView().down('lawsuitownerinfogrid gkhbuttonprint[name=PrintOwner]').setVisible(false);
                        panel.close();
                        return false;
                    }
                    asp.controller.getAspect('courtOwnerPrintAspect').loadReportStore();
                    asp.controller.getMainView().down('lawsuitownerinfogrid gkhbuttonprint[name=LawsuitPrintOwner]').setVisible(false);
                    return true;
                },
                aftersetpaneldata: function (asp, record) {
                    if (!record) {
                        return false;
                    }
                    var panel = asp.getPanel(),
                        resultConsideration = panel.down('[name=ResultConsideration]').getValue();
                    panel.setDisabled(false);

                    panel.down('[name=JuridicalSectorMu]').setDisabled(Gkh.config.ClaimWork.Common.General.CourtName == B4.enums.FillType.Automatic);

                    if (resultConsideration == B4.enums.LawsuitResultConsideration.CourtOrderIssued) {
                        panel.down('[xtype=acceptmenubutton]').setDisabled(true);
                    } else {
                        panel.down('[xtype=acceptmenubutton]').setDisabled(false);
                    }

                    return true;
                }
            }
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'courtClaimAcceptMenuAspect',
            buttonSelector: 'clwlawsuitcourtclaiminfopanel acceptmenubutton',
            containerSelector: 'clwlawsuitcourtclaiminfopanel'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'lawsuitreferencecalculationGridAspect',
            storeName: 'LawsuitReferenceCalculation',
            modelName: 'LawsuitReferenceCalculation',
            gridSelector: 'lawsuitreferencecalculationgrid'
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'courtClaimPrintAspect',
            buttonSelector: 'clwlawsuitcourtclaiminfopanel gkhbuttonprint',
            codeForm: 'CourtClaim, AccountClaim185, AccountClaim512, PetitionClw',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = { DocumentId: me.controller.getContextValue(view, 'docId') };

                me.params.userParams = Ext.JSON.encode(param);
            },
            listeners: {
                onprintsucess: function(asp) {
                    var controller = asp.controller,
                        view = controller.getMainView(),
                        docId = controller.getContextValue(view, 'docId');

                    controller.getAspect('courtClaimEditPanelAspect').setData(docId);
                }
            }
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'courtOwnerPrintAspect',
            buttonSelector: 'lawsuitownerinfogrid gkhbuttonprint[action=PrintOwner]',
            codeForm: 'CourtClaimOwner, CourtClaimShared, CourtClaimOwner185, CourtClaimOwner512, LawSuitOwnersPetition, ExecutiveClaim, ExecutiveClaimRepeat',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    grid = me.controller.getOwnerInfoView(),
                    records = grid.getSelectionModel().getSelection(),
                    recIds = [],
                    param = { DocumentId: me.controller.getContextValue(view, 'docId') };

                Ext.each(records,
                    function (rec) {
                        recIds.push(rec.get('Id'));
                    });

                Ext.apply(me.params, { recIds: Ext.JSON.encode(recIds) });
                me.params.userParams = Ext.JSON.encode(param);
            },
            printReport: function(itemMenu) {
                var me = this,
                    frame = Ext.get('downloadIframe');
                if (frame != null) {
                    Ext.destroy(frame);
                }

                me.getUserParams(itemMenu.actionName);

                if (Ext.JSON.decode(me.params.recIds).length == 0) {
                    Ext.Msg.alert('Ошибка', 'Необходимо выбрать хотя бы одну запись для печати');
                    return;
                }

                Ext.apply(me.params, { reportId: itemMenu.actionName });

                me.mask('Обработка...');
                B4.Ajax.request({
                        url: B4.Url.action('ReportLawsuitOnwerPrint', 'ClaimWorkReport'),
                        params: me.params,
                        timeout: 9999999
                    })
                    .next(function(resp) {
                        var tryDecoded;

                        me.unmask();
                        try {
                            tryDecoded = Ext.JSON.decode(resp.responseText);
                        } catch (e) {
                            tryDecoded = {};
                        }

                        var id = resp.data ? resp.data : tryDecoded.data;

                        if (id > 0) {
                            Ext.DomHelper.append(document.body,
                                {
                                    tag: 'iframe',
                                    id: 'downloadIframe',
                                    frameBorder: 0,
                                    width: 0,
                                    height: 0,
                                    css: 'display:none;visibility:hidden;height:0px;',
                                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                                });

                            me.fireEvent('onprintsucess', me);
                        }
                    })
                    .error(function(err) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка', err.message || err);
                    });
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'awSuitDebtWorkSSPGridAspect',
            gridSelector: 'lawsuitsspgrid',
            editFormSelector: 'lawsuitdebtworksspeditwindow',
            editWindowView: 'claimwork.LawSuitDebtWorkSSPEditWindow',
            //xtype: 'grideditwindowaspect',
            //name: 'awSuitDebtWorkSSPGridAspect',
            //gridSelector: 'lawsuitsspgrid',
            //editFormSelector: 'lawsuitdebtworksspeditwindow',
            storeName: 'claimwork.LawSuitDebtWorkSSP',
            modelName: 'claimwork.LawSuitDebtWorkSSP',
            //editWindowView: 'claimwork.LawSuitDebtWorkSSPEditWindow',
            onSaveSuccess: function (aspect, rec) {
                var me = this,
                    roleGrid = me.getGrid(),
                    storeSSP = roleGrid.getStore();
                storeSSP.clearFilter(true);
                storeSSP.filter('Lawsuit', parentDoc);
            },
            otherActions: function (actions) {

                actions['lawsuitdebtworksspeditwindow #sfLawOI'] = { 'beforeload': { fn: this.onBeforeLoadOwners, scope: this } };
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var typeDebtWork = record.data.DebtWorkType;
                    var TypeDWField = form.down('#debtWorkType');
                    TypeDWField.value = typeDebtWork == 0 ? 10 : typeDebtWork;
                    parentDoc = asp.controller.getCurrentDoc();
                    parentDocSSP = record.getId();
                    if (!record.data.Id) {
                        debugger;
                        record.data.Lawsuit = asp.controller.getCurrentDoc();
                    }
                    debugger;
                    var grid = form.down('lawsuitsspdocgrid'),
                        store = grid.getStore();
                    store.filter('docId', record.getId());
                }
            },
            onBeforeLoadOwners: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.Lawsuit = parentDoc;
            },
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'awSuitDebtWorkSSPDocGridAspect',
            gridSelector: 'lawsuitsspdocgrid',
            editFormSelector: 'lawsuitdebtworksspdoceditwindow',
            editWindowView: 'claimwork.LawSuitDebtWorkSSPDocumentEditWindow',
            storeName: 'claimwork.LawSuitDebtWorkSSPDocument',
            modelName: 'claimwork.LawSuitDebtWorkSSPDocument',
            onSaveSuccess: function (aspect, rec) {
                var me = this,
                    roleGrid = me.getGrid(),
                    storeSSP = roleGrid.getStore();
                storeSSP.clearFilter(true);
                storeSSP.filter('docId', parentDocSSP);
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    if (!record.data.Id) {
                        debugger;
                        record.data.LawSuitDebtWorkSSP = parentDocSSP;
                    }


                }
            }
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'lawsuitOwnerPrintAspect',
            buttonSelector: 'lawsuitownerinfogrid gkhbuttonprint[action=LawsuitPrintOwner]',
            codeForm: 'LawSuitOwner,LawSuitOwnersPetition, ExecutiveClaim, ExecutiveClaimRepeat',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    grid = me.controller.getOwnerInfoView(),
                    records = grid.getSelectionModel().getSelection(),
                    recIds = [],
                    param = { DocumentId: me.controller.getContextValue(view, 'docId') };

                Ext.each(records,
                    function (rec) {
                        recIds.push(rec.get('Id'));
                    });

                Ext.apply(me.params, { recIds: Ext.JSON.encode(recIds) });
                me.params.userParams = Ext.JSON.encode(param);
            },
            printReport: function(itemMenu) {
                var me = this,
                    frame = Ext.get('downloadIframe');
                if (frame != null) {
                    Ext.destroy(frame);
                }

                me.getUserParams(itemMenu.actionName);

                if (Ext.JSON.decode(me.params.recIds).length == 0) {
                    Ext.Msg.alert('Ошибка', 'Необходимо выбрать хотя бы одну запись для печати');
                    return;
                }

                Ext.apply(me.params, { reportId: itemMenu.actionName });

                me.mask('Обработка...');
                B4.Ajax.request({
                    url: B4.Url.action('ReportLawsuitOnwerPrint', 'ClaimWorkReport'),
                    params: me.params,
                    timeout: 9999999
                }).next(function(resp) {
                    var tryDecoded;

                    me.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }

                    var id = resp.data ? resp.data : tryDecoded.data;

                    if (id > 0) {
                        Ext.DomHelper.append(document.body,
                            {
                                tag: 'iframe',
                                id: 'downloadIframe',
                                frameBorder: 0,
                                width: 0,
                                height: 0,
                                css: 'display:none;visibility:hidden;height:0px;',
                                src: B4.Url.action('Download', 'FileUpload', { Id: id })
                            });

                        me.fireEvent('onprintsucess', me);
                    }
                }).error(function(err) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err);
                });
            }
        },
        //#endregion
        {
            xtype: 'gkheditpanel',
            name: 'collectionEditPanelAspect',
            editPanelSelector: 'clwlawsuitcollectionpanel',
            modelName: 'claimwork.lawsuit.Lawsuit',
            otherActions: function (actions) {
                var me = this;
                actions['clwlawsuitcollectionpanel [name=CbIsStopped]'] = { 'change': { fn: me.onChangeCheckBox, scope: me } };
                actions['clwlawsuitcollectionpanel [name=CbFactInitiated]'] = { 'change': { fn: me.onCbFactInitiatedChanged, scope: me } };
            },
            listeners: {
                savesuccess: function (asp, rec) {
                    asp.setData(rec.getId());
                }
            },
            onCbFactInitiatedChanged: function (cbox, newValue) {
                var panel = cbox.up('clwlawsuitcollectionpanel'),
                    dateField = panel.down('[name=CbDateInitiated]');

                dateField.allowBlank = newValue === B4.enums.LawsuitFactInitiationType.NotInitiated;
            },
            onChangeCheckBox: function (chkbox, newValue) {
                var panel = chkbox.up('clwlawsuitcollectionpanel'),
                    descrField = panel.down('[name=CbReasonStoppedDescription]'),
                    typeField = panel.down('[name=CbReasonStoppedType]'),
                    dateField = panel.down('[name=CbDateStopped]'),
                    actDateField = panel.down('[name=LackOfPropertyActDate]'),
                    actField = panel.down('[name=LackOfPropertyAct]'),
                    CbDocReturnedField = panel.down('[name=CbDocReturned]');

                descrField.setReadOnly(!newValue);
                typeField.setReadOnly(!newValue);
                dateField.setReadOnly(!newValue);
                actDateField.setReadOnly(!newValue);
                actField.setReadOnly(!newValue);
                CbDocReturnedField.setReadOnly(!newValue);
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'lawsuitDocAspect',
            gridSelector: 'claimworklawsuitdocgrid',
            modelName: 'claimwork.lawsuit.Doc',
            editFormSelector: 'claimworklawsuitdoceditwindow',
            editWindowView: 'claimwork.lawsuit.DocEditWindow',
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.DocumentClw = me.controller.getCurrentDoc();
                    }
                }
            }
        },
        //#region Petition
        {
            xtype: 'claimworkdocumentaspect',
            name: 'lawsuitEditPanelAspect',
            editPanelSelector: 'clwlawsuitmaininfopanel',
            modelName: 'claimwork.lawsuit.Petition',
            docCreateAspectName: 'lawsuitCreateButtonAspect',
            otherActions: function (actions) {
                var me = this;
                actions['clwlawsuitmaininfopanel [name=Suspended]'] = { 'change': { fn: me.onChangeCheckBox, scope: me } };
            },
            onChangeCheckBox: function (chkbox, newValue) {
                var panel = chkbox.up('clwlawsuitmaininfopanel'),
                    numberField = panel.down('[name=DeterminationNumber]'),
                    dateField = panel.down('[name=DeterminationDate]');

                numberField.setReadOnly(!newValue);
                dateField.setReadOnly(!newValue);
            },
            listeners: {
                beforesetpaneldata: function (asp, record) {
                    if (!record) {
                        asp.getPanel().close();
                        return false;
                    }

                    return true;
                },
                aftersetpaneldata: function (asp, record) {
                    if (!record) {
                        return false;
                    }

                    var panel = asp.getPanel();
                    panel.setDisabled(false);

                    panel.down('[name=JuridicalSectorMu]').setDisabled(Gkh.config.ClaimWork.Common.General.CourtName === 10);

                    return true;
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'lawsuitCourtAspect',
            gridSelector: 'claimworklawsuitcourtgrid',
            modelName: 'claimwork.lawsuit.Court',
            editFormSelector: 'claimworklawsuitcourteditwindow',
            editWindowView: 'claimwork.lawsuit.CourtEditWindow',
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.DocumentClw = me.controller.getCurrentDoc();
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'lawsuitDocumentationAspect',
            gridSelector: 'claimworklawsuitdocumentationgrid',
            modelName: 'claimwork.lawsuit.LawsuitDocument',
            editFormSelector: 'claimworklawsuitdocumentationeditwindow',
            editWindowView: 'claimwork.lawsuit.DocumentationEditWindow',
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.Lawsuit = me.controller.getCurrentDoc();
                    }
                }
            }
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'lawsuitCreateButtonAspect',
            buttonSelector: 'clwlawsuitmaininfopanel acceptmenubutton',
            containerSelector: 'clwlawsuitmaininfopanel'
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'lawsuitPrintAspect',
            buttonSelector: 'clwlawsuitmaininfopanel gkhbuttonprint',
            codeForm: 'LawSuit,RefusalLawsuit,PetitionClw',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = { DocumentId: me.controller.getContextValue(view, 'docId') };

                me.params.userParams = Ext.JSON.encode(param);
            },
            listeners: {
                onprintsucess: function(asp) {
                    var controller = asp.controller,
                        view = controller.getMainView(),
                        docId = controller.getContextValue(view, 'docId');

                    controller.getAspect('lawsuitEditPanelAspect').setData(docId);
                }
            },
            onBeforeLoadReportStore: function (store, operation) {
                operation.params = {};
                operation.params.codeForm = this.codeForm;
                operation.params.type = this.controller.getDebtorType();
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'LawsuitOwnerInfoGrid',
            gridSelector: 'lawsuitownerinfogrid',
            editFormSelector: 'lawsuitownerinfowindow',
            editWindowView: 'claimwork.lawsuit.LawsuitOwnerInfoWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.getForm().showNameFields(asp.controller.getOwnerType());
                    if (!record.data.Id) {
                        record.data.Lawsuit = asp.controller.getCurrentDoc();
                    }
                },
                validate: function(asp) {
                    var areaShareNumerator = asp.getForm().getRecord().get('AreaShareNumerator'),
                        areaShareDenominator = asp.getForm().getRecord().get('AreaShareDenominator');

                    if (areaShareNumerator > areaShareDenominator) {
                        Ext.Msg.alert('Ошибка!', 'Значение числителя не может превышать значение знаменателя');
                        return false;
                    }

                    return true;
                }
            },
            getModel: function () {
                var asp = this,
                    ownerType = asp.controller.getOwnerType();

                if (ownerType === B4.enums.regop.PersonalAccountOwnerType.Individual) {
                    asp.modelName = 'claimwork.lawsuit.LawsuitIndividualOwnerInfo';
                }
                if (ownerType === B4.enums.regop.PersonalAccountOwnerType.Legal) {
                    asp.modelName = 'claimwork.lawsuit.LawsuitLegalOwnerInfo';
                }
                return asp.controller.getModel(asp.modelName);
            },
            otherActions: function (actions) {
                var me = this;
                actions['lawsuitownerinfowindow b4selectfield[name=PersonalAccount]'] = {
                    'beforeload': {
                        fn: me.onPersonalAccountBeforeLoad,
                        scope: me
                    },
                    'change': {
                        fn: me.onPersonalAccountChange,
                        scope: me
                    }
                };
                actions['lawsuitownerinfowindow b4selectfield[name=StartPeriod]'] = {
                    'beforeload': {
                        fn: me.onPeriodBeforeLoad,
                        scope: me
                    }
                };
                actions['lawsuitownerinfowindow b4selectfield[name=EndPeriod]'] = {
                    'beforeload': {
                        fn: me.onPeriodBeforeLoad,
                        scope: me
                    }
                };
                actions['lawsuitownerinfowindow b4addbutton[name=FillFromAccount]'] = {
                    'click': {
                        fn: me.onFillFromAccount,
                        scope: me
                    }
                };
            },

            onFillFromAccount: function() {
                var asp = this,
                    me = asp.controller,
                    ownerType = me.getOwnerType(),
                    storeName = '',
                    form = asp.getForm(),
                    surname = form.down('[name=Surname]'),
                    firsName = form.down('[name=FirstName]'),
                    secondName = form.down('[name=SecondName]'),
                    name = form.down('[name=ContragentName]'),
                    inn = form.down('[name=Inn]'),
                    kpp = form.down('[name=Kpp]'),
                    setNameFields = function() {},
                    columns = [];

                if (ownerType === B4.enums.regop.PersonalAccountOwnerType.Individual) {
                    storeName = 'B4.store.regop.owner.IndividualAccountOwner';
                    columns = [
                        { header: 'Наименование', dataIndex: 'Name', filter: { xtype: 'textfield' }, flex: 1 }
                    ];
                    setNameFields = function(data) {
                        surname.setValue(data.Surname);
                        firsName.setValue(data.FirstName);
                        secondName.setValue(data.SecondName);
                    }
                }
                if (ownerType === B4.enums.regop.PersonalAccountOwnerType.Legal) {
                    storeName = 'B4.store.regop.owner.LegalAccountOwner';
                    columns = [
                        { header: 'Наименование', dataIndex: 'Name', filter: { xtype: 'textfield' }, flex: 1 },
                        { header: 'ИНН', dataIndex: 'Inn', filter: { xtype: 'textfield' }, flex: 1 },
                        { header: 'КПП', dataIndex: 'Kpp', filter: { xtype: 'textfield' }, flex: 1 }
                    ];
                    setNameFields = function (data) {
                        name.setValue(data.Name);
                        inn.setValue(data.Inn);
                        kpp.setValue(data.Kpp);
                    }
                }

                me.addView = Ext.create('B4.form.SelectWindow', {
                    modal: true,
                    store: storeName,
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    loadDataOnShow: true,
                    columns: columns,
                    selectWindowCallback: function (data) {
                        if (data) {
                            setNameFields(data);
                        }
                    }
                });

                me.addView.show();
            },

            onPersonalAccountBeforeLoad: function (store, operation) {
                var me = this.controller;

                operation = operation || {};

                Ext.apply(operation.params, {
                    claimWorkId: me.getClaimWorkId()
                });
            },

            onPersonalAccountChange: function (el, newValue) {
                this.setAllowPeriodFields(newValue ? true : false);
            },

            onPeriodBeforeLoad: function (store, operation) {
                var asp = this,
                    form = asp.getForm(),
                    personalAccount = form.down('[name=PersonalAccount]');

                operation = operation || {};

                Ext.apply(operation.params, {
                    id: personalAccount.getValue()
                });
            },

            setAllowPeriodFields: function(allow) {
                var asp = this,
                    me = asp.controller,
                    form = asp.getForm(),
                    id = form.getRecord().getId(),
                    isModified = form.getRecord().isModified('PersonalAccount') || id === 0,
                    startPeriod = form.down('[name=StartPeriod]'),
                    endPeriod = form.down('[name=EndPeriod]'),
                    startPeriodStore = startPeriod.getStore();

                startPeriod.setValue({});
                endPeriod.setValue({});
                startPeriod.setDisabled(!allow);
                endPeriod.setDisabled(!allow);

                if (allow && isModified) {
                    startPeriodStore.load({
                        scope: asp,
                        callback: function (records) {
                            me.unmask();
                            if (records && records.length === 2) {
                                startPeriod.setValue(records[1].getData());
                                endPeriod.setValue(records[0].getData());
                            }
                        },
                        params: {
                            limit: 2,
                            sort: Ext.encode([
                                { property: 'IsClosed', direction: 'ASC' },
                                { property: 'Id', direction: 'ASC' }
                            ])
                        }
                    });

                    me.mask('Пожалуйста, подождите...', form);
                }
            }
        },
        {
            xtype: 'documentclwaccountdetailaspect',
            name: 'courtClwAccountDetail',
            panelSelector: 'clwlawsuitcourtclaiminfopanel',
            getDocumentId: function () {
                var me = this.controller;
                return me.getCurrentDoc();
            }
        },
        {
            xtype: 'documentclwaccountdetailaspect',
            name: 'lawsuitClwAccountDetail',
            panelSelector: 'clwlawsuitmaininfopanel',
            getDocumentId: function () {
                var me = this.controller;
                return me.getCurrentDoc();
            }
        }
        //#endregion
    ],

    getCurrentDoc: function () {
        var me = this;
        return me.getContextValue(me.getMainComponent(), 'docId');
    },

    getClaimWorkId: function () {
        var me = this;
        return me.getContextValue(me.getMainComponent(), 'claimWorkId');
    },

    getOwnerType: function() {
        var me = this,
            type = me.getContextValue(me.getMainComponent(), 'type');

        if (type === 'IndividualClaimWork') {
            return B4.enums.regop.PersonalAccountOwnerType.Individual;
        }
        if (type === 'LegalClaimWork') {
            return B4.enums.regop.PersonalAccountOwnerType.Legal;
        }

        return null;
    },

    getDebtorType: function () {
        var me = this,
            type = me.getContextValue(me.getMainComponent(), 'type');

        if (type === 'IndividualClaimWork') {
            return B4.enums.DebtorType.Individual;
        }
        if (type === 'LegalClaimWork') {
            return B4.enums.DebtorType.Legal;
        }

        return null;
    },

    init: function () {
        var me = this,
            actions = [];

        actions['lawsuitownerinfogrid'] = { render: me.onRenderLawsuitownerInfoGrid, scope: me }
        actions['lawsuitownerinfogrid button[action=DebtCalculate]'] = { click: me.debtCalculate, scope: me }
        actions['lawsuitownerinfogrid button[action=GetRosRegOwners]'] = { click: me.getRosRegOwners, scope: me }
        actions['lawsuitreferencecalculationgrid'] = { render: me.onRenderRefCalcGrid, scope: me }

        actions['clwlawsuitcourtclaiminfopanel button[action=DebtStartCalculate]'] =
            { click: me.calculateDebtStartDate, scope: me }


        actions['clwlawsuitmaininfopanel button[action=DebtStartCalculate]'] =
            { click: me.calculateDebtStartDateLawsuit, scope: me }

        actions['lawsuitsspgrid b4updatebutton'] = { click: me.loadStore, scope: me }
        actions['lawsuitsspdocgrid b4updatebutton'] = { click: me.loadDocStore, scope: me }
        actions['lawsuitownerinfogrid button[action=CreateArchive]'] = { click: me.createArchive, scope: me }
        actions['claimworkoperationwin button[action=Accept]'] = { click: me.calculateAccept, scope: me }   
        me.control(actions);
        me.callParent(arguments);
    },

    loadStore: function (component) {
        var me = this,
            view = me.getMainView() || Ext.widget('clwlawsuiteditpanel');
        var thisGrid = view.down('lawsuitsspgrid');
        var storeSSP = thisGrid.getStore();
        storeSSP.clearFilter(true);
        storeSSP.filter('Lawsuit', parentDoc);

    },

    loadDocStore: function (component) {
        var me = this;
        debugger;
        var thisGrid = component.up('lawsuitsspdocgrid');
        var storeSSP = thisGrid.getStore();
        storeSSP.clearFilter(true);
        storeSSP.filter('docId', parentDocSSP);

    },

    index: function (type, id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('clwlawsuiteditpanel');

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}/{2}/lawsuit', type, id, docId);
        me.bindContext(view);

        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.setContextValue(view, 'docId', docId);
        me.setContextValue(view, 'docType', 'lawsuit');
        me.application.deployView(view, 'claim_work');

        if (type === 'BuildContractClaimWork') {
            view.down('button[action=form]').hide();
        }

        var lawsuitEditPanelAspect = me.getAspect('lawsuitEditPanelAspect');
        if (lawsuitEditPanelAspect.getPanel()) {
            lawsuitEditPanelAspect.setData(docId);
        }

        me.getAspect('collectionEditPanelAspect').setData(docId);

        me.getAspect('lawsuitCreateButtonAspect').setData(id, type);
        me.getAspect('lawsuitPrintAspect').loadReportStore();


        //#region Court claim aspects init
        var courtClaimEditPanelAspect = me.getAspect('courtClaimEditPanelAspect');
        if (courtClaimEditPanelAspect.getPanel()) {
            courtClaimEditPanelAspect.setData(docId);
        }

        me.getAspect('courtClaimAcceptMenuAspect').setData(id, type);
        me.getAspect('courtClaimPrintAspect').loadReportStore();
        //#endregion

        var gridDoc = view.down('claimworklawsuitdocgrid'),
            gridCourt = view.down('claimworklawsuitcourtgrid'),
            gridDocumentation = view.down('claimworklawsuitdocumentationgrid'),
            storeDoc = gridDoc.getStore(),
            storCourt = gridCourt.getStore(),
            storeDocumentation = gridDocumentation.getStore();

        var gridSSP = view.down('lawsuitsspgrid'),
            storeSSP = gridSSP.getStore();

        storeSSP.clearFilter(true);
        storeSSP.filter('Lawsuit', docId);

        storeDoc.clearFilter(true);
        storeDoc.filter('docId', docId);

        storCourt.clearFilter(true);
        storCourt.filter('docId', docId);

        storeDocumentation.clearFilter(true);
        storeDocumentation.filter('docId', docId);
    },

    getRosRegOwners: function (btn) {
        var me = this,
            view = me.getMainView(),
            docId = me.getContextValue(view, 'docId'),
            lawgrig = btn.up('lawsuitownerinfogrid');
        var panel = btn.up('clwlawsuitcourtclaiminfopanel');
        me.mask('Получение собственников...', Ext.getBody());

        B4.Ajax.request({
            url: B4.Url.action('GetOwners', 'RosRegExtractOperations'),
            method: 'POST',
            timeout: 100 * 60 * 60 * 3,
            params: {
                docId: docId
            }
        })
            .next(function (response) {
                //debugger;
                var obj = Ext.JSON.decode(response.responseText);
                //debugger;
                me.unmask();
                lawgrig.getStore().load();
                var alertWindow = Ext.Msg.alert('Результаты добавления', obj.message);
                if (obj.areaWarning == true) {
                    var component = alertWindow.down('displayfield').inputEl;

                    component.addCls('warning-message-red');
                }
            })
            .error(function (error) {
                me.unmask();
                Ext.Msg.alert('Ошибка', 'Выписка не найдена');
            });
    },

    onBeforeLoad: function (store, operation) {
        operation = operation || {};
        operation.params = operation.params || {};
        operation.params.Lawsuit = this.getCurrentDoc();
    },

    onRenderLawsuitownerInfoGrid: function (grid) {
        var me = this;

        grid.getStore().on('beforeload', me.onBeforeLoad, me);
        grid.getStore().load();
    },

    onRenderLSDebtSSPGrid: function (grid) {
        var me = this;
        debugger;
        grid.getStore().on('beforeload', me.onBeforeLoad, me);
        grid.getStore().load();
    },
    onRenderRefCalcGrid: function (grid) {
        var me = this;
        var stor = grid.getStore();
        grid.getStore().on('beforeload', me.onBeforeLoad, me);
        grid.getStore().load();
    },

    calculateAccept: function (btn) {
        var me = this,
            win = btn.up('claimworkoperationwin'),
            grid = win.down('grid'),
            records = grid.getSelectionModel().getSelection(),
            recIds = [],
            view = me.getMainView();
        protstore = grid.getStore();
        var panel = view.down('clwlawsuitcourtclaiminfopanel');
        if (panel == null) {
            panel = view.down('clwlawsuitmaininfopanel');
        }
        Ext.each(records,
            function (rec) {
                recIds.push(rec.get('TransferId'));
            });
        var docId = me.getContextValue(view, 'docId');
        win.close();

        if (docId) {
            me.mask('Расчет даты...', Ext.getBody());

            B4.Ajax.request({
                url: B4.Url.action('DebtStartDateCalculate', 'LawsuitOwnerInfo'),
                method: 'POST',
                timeout: 100 * 60 * 60 * 3,
                params: {
                    docId: docId,
                    recIds: recIds
                }
            })
            .next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                me.unmask();
                Ext.Msg.alert('Результаты расчета', obj.message);
                var datefieldDSD = panel.down('#sfDebtStartDate');
                var datefieldDED = panel.down('#sfDebtEndDate');
                var debtCalcMethod = panel.down('#sfDebtCalcMethod');
                datefieldDSD.setValue(obj.dateStartDebt);
                datefieldDED.setValue(obj.DebtEndDate);
                debtCalcMethod.setValue(obj.DebtCalcMethod);
                var debtSumField = panel.down('#fDebtSum');
                var debtBaseField = panel.down('#fDebtBaseTariffSum');
                var debtDecisionField = panel.down('#fDebtDecisionTariffSum');
                var penaltyDebtField = panel.down('#fPenaltyDebt');
                penaltyDebtField.setValue(obj.PenaltyDebt ? obj.PenaltyDebt : 0);
                debtSumField.setValue(obj.DebtSum);
                debtBaseField.setValue(obj.DebtBaseTariffSum);
                debtDecisionField.setValue(obj.DebtDecisionTariffSum ? obj.DebtDecisionTariffSum : 0);
            })
            .error(function (error) {
                me.unmask();
                Ext.Msg.alert('Ошибка расчета', error.message || 'Ошибка при расчете даты');
            });
        }

    },

    calculateDebtStartDate: function (btn) {
        var me = this,
            win = me.getCmpInContext('claimworkoperationwin'),
            view = me.getMainView();

        var docId = me.getContextValue(view, 'docId');
        var panel = btn.up('clwlawsuitcourtclaiminfopanel');
        var recordLsw = panel.getRecord();
        if (recordLsw.get('IsLimitationOfActions')) {
            if (!win) {

                activeTab = B4.getBody().getActiveTab();

                win = Ext.create('B4.view.claimwork.DebtorPaymentsWindow', {
                    constrain: true,
                    //renderTo: activeTab.getEl(),
                    closeAction: 'destroy',
                    title: 'Операции за период ',
                    ctxKey: me.getCurrentContextKey(),
                    modal: true
                });

                activeTab.add(win);
            }
            if (win) {
                var protstore = win.down('grid').getStore();
                protstore.removeAll();
                protstore.on('beforeload',
                    function (store, operation) {
                        operation.params.docId = docId;
                    },
                    me);
                protstore.load();
                win.show();
            }
        }
        else {
            me.mask('Расчет даты...', Ext.getBody());

            B4.Ajax.request({
                url: B4.Url.action('DebtStartDateCalculate', 'LawsuitOwnerInfo'),
                method: 'POST',
                timeout: 100 * 60 * 60 * 3,
                params: {
                    docId: docId
                }
            })
            .next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                me.unmask();
                Ext.Msg.alert('Результаты расчета', obj.message);
                var datefieldDSD = panel.down('#sfDebtStartDate');
                var datefieldDED = panel.down('#sfDebtEndDate');
                var debtCalcMethod = panel.down('#sfDebtCalcMethod');
                datefieldDSD.setValue(obj.dateStartDebt);
                datefieldDED.setValue(obj.DebtEndDate);
                debtCalcMethod.setValue(obj.DebtCalcMethod);
                var debtSumField = panel.down('#fDebtSum');
                var debtBaseField = panel.down('#fDebtBaseTariffSum');
                var debtDecisionField = panel.down('#fDebtDecisionTariffSum');
                var penaltyDebtField = panel.down('#fPenaltyDebt');
                penaltyDebtField.setValue(obj.PenaltyDebt ? obj.PenaltyDebt : 0);
                debtSumField.setValue(obj.DebtSum);
                debtBaseField.setValue(obj.DebtBaseTariffSum);
                debtDecisionField.setValue(obj.DebtDecisionTariffSum ? obj.DebtDecisionTariffSum : 0);
            })
            .error(function (error) {
                me.unmask();
                Ext.Msg.alert('Ошибка расчета', error.message || 'Ошибка при расчете даты');
            });
        }
    },

    createArchive: function (btn) {
        var me = this
        view = me.getMainView(),
            clw = this.getClaimWorkId(),
            grid = me.getOwnerInfoView(),
            records = grid.getSelectionModel().getSelection(),
            recIds = [];
        Ext.each(records,
            function (rec) {
                recIds.push(rec.get('Id'));
            });
        me.mask('Выгрузка...', Ext.getBody());
        B4.Ajax.request({
            url: B4.Url.action('CreateArchive', 'ArchivedClaimWork'),
            method: 'POST',
            timeout: 100 * 60 * 60 * 3,
            params: {
                ClwId: clw,
                RloiId: recIds
            }
        })
            .next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                me.unmask();
                var alertWindow = Ext.Msg.alert('Результат выполнения', 'Выгрузка в реестр долевых ПИР успешна');
            })
            .error(function (error) {
                me.unmask();
                Ext.Msg.alert('Ошибка', error.message);
            });
    },

    debtCalculate: function (button) {
        var me = this,
            grid = button.up('lawsuitownerinfogrid'),
            selectedItems = grid.getSelectionModel().getSelection() || [],
            selectedIds = [];

        if (selectedItems.length === 0) {
            Ext.Msg.alert('Ошибка расчета', 'Для расчета задолженности необходимо выбрать хотя бы одну запись');
            return;
        }

        Ext.each(selectedItems, function (item) { selectedIds.push(item.getId()) });

        me.mask('Расчет долга...', Ext.getBody());

        B4.Ajax.request({
            url: B4.Url.action('DebtCalculate', 'LawsuitOwnerInfo'),
            method: 'POST',
            timeout: 100 * 60 * 60 * 3,
            params: {
                ids: Ext.encode(selectedIds)
    }
        }).next(function () {
            me.unmask();
            grid.getStore().load();
        }).error(function (error) {
            me.unmask();
            Ext.Msg.alert('Ошибка расчета', error.message || 'Ошибка при расчете задолженности');
        });
    },

    calculateDebtStartDateLawsuit: function(btn) {
        var me = this,
            win = me.getCmpInContext('claimworkoperationwin'),
            view = me.getMainView();
        var docId = me.getContextValue(view, 'docId');
        var panel = btn.up('clwlawsuitmaininfopanel');
        var recordLsw = panel.getRecord();
        if (recordLsw.get('IsLimitationOfActions')) {
            if (!win) {

                activeTab = B4.getBody().getActiveTab();

                win = Ext.create('B4.view.claimwork.DebtorPaymentsWindow', {
                    constrain: true,
                    //renderTo: activeTab.getEl(),
                    closeAction: 'destroy',
                    title: 'Операции за период ',
                    ctxKey: me.getCurrentContextKey(),
                    modal: true
                });

                activeTab.add(win);
            }
            if (win) {
                var protstore = win.down('grid').getStore();
                protstore.removeAll();
                protstore.on('beforeload',
                    function (store, operation) {
                        operation.params.docId = docId;
                    },
                    me);
                protstore.load();
                win.show();
            }
        }
        else {
            me.mask('Расчет даты...', Ext.getBody());

            B4.Ajax.request({
                url: B4.Url.action('DebtStartDateCalculate', 'LawsuitOwnerInfo'),
                method: 'POST',
                timeout: 100 * 60 * 60 * 3,
                params: {
                    docId: docId
                }
            })
                .next(function (response) {
                    var obj = Ext.JSON.decode(response.responseText);
                    me.unmask();
                    Ext.Msg.alert('Результаты расчета', obj.message);
                    var datefieldDSD = panel.down('#sfDebtStartDate');
                    var datefieldDED = panel.down('#sfDebtEndDate');
                    var debtCalcMethod = panel.down('#sfDebtCalcMethod');
                    datefieldDSD.setValue(obj.dateStartDebt);
                    datefieldDED.setValue(obj.DebtEndDate);
                    debtCalcMethod.setValue(obj.DebtCalcMethod);
                    var debtSumField = panel.down('#fDebtSum');
                    var debtBaseField = panel.down('#fDebtBaseTariffSum');
                    var debtDecisionField = panel.down('#fDebtDecisionTariffSum');
                    var penaltyDebtField = panel.down('#fPenaltyDebt');
                    penaltyDebtField.setValue(obj.PenaltyDebt ? obj.PenaltyDebt : 0);
                    debtSumField.setValue(obj.DebtSum);
                    debtBaseField.setValue(obj.DebtBaseTariffSum);
                    debtDecisionField.setValue(obj.DebtDecisionTariffSum ? obj.DebtDecisionTariffSum : 0);
                })
                .error(function (error) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка расчета', error.message || 'Ошибка при расчете даты');
                });
        }
    },

});