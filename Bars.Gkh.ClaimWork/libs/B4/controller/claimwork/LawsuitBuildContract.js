Ext.define('B4.controller.claimwork.LawsuitBuildContract', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.ClaimWorkDocument',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocCreateButton',
        'B4.aspects.GkhEditPanel',
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
        'B4.aspects.permission.Lawsuit'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'claimwork.lawsuit.Doc',
        'claimwork.lawsuit.Court'
    ],

    models: [
        'claimwork.lawsuit.Lawsuit',
        'claimwork.lawsuit.Petition',
        'claimwork.lawsuit.CourtOrderClaim',
        'claimwork.lawsuit.Doc',
        'claimwork.lawsuit.Court'
    ],

    views: [
        'claimwork.buildcontract.lawsuit.EditPanel',
        'claimwork.buildcontract.lawsuit.CourtClaimInfoPanel',
        'claimwork.buildcontract.lawsuit.MainInfoPanel',
        'claimwork.lawsuit.DocEditWindow',
        'claimwork.lawsuit.CourtEditWindow',
        'claimwork.lawsuit.CourtGrid',
        'claimwork.lawsuit.DocGrid',
        'claimwork.lawsuit.DocumentationGrid',
        'claimwork.lawsuit.DocumentationEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'clwbclawsuiteditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'lawsuitpermission'
        },
        //#region Court claim
        {
            xtype: 'courtclaimeditpanelaspect',
            name: 'courtClaimBcEditPanelAspect',
            editPanelSelector: 'clwlawsuitbccourtclaiminfopanel',
            modelName: 'claimwork.lawsuit.CourtOrderClaim',
            docCreateAspectName: 'courtClaimBcAcceptMenuAspect',
            listeners: {
                beforesetpaneldata: function (asp, record) {
                    var panel = asp.getPanel();
                    if (!record) {
                        panel.close();
                        return false;
                    }

                    return true;
                },
                aftersetpaneldata: function (asp, record) {
                    if (!record) {
                        return false;
                    }
                    var panel = asp.getPanel(),
                        tabPanel = panel.up('tabpanel'),
                        resultConsideration = panel.down('[name=ResultConsideration]').getValue();
                    panel.setDisabled(false);
                    tabPanel.setActiveTab(panel);

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
            name: 'courtClaimBcAcceptMenuAspect',
            buttonSelector: 'clwlawsuitbccourtclaiminfopanel acceptmenubutton',
            containerSelector: 'clwlawsuitbccourtclaiminfopanel',
            createDocument: function (params) {
                var me = this,
                    data,
                    container = me.componentQuery(me.containerSelector);

                me.controller.mask('Формирование документа', container);

                B4.Ajax.request({
                    url: B4.Url.action('CreateDocument', 'ClaimWorkDocument'),
                    method: 'POST',
                    timeout: 9999999,
                    params: params
                }).next(function (res) {
                    data = Ext.decode(res.responseText);

                    me.fireEvent('createsuccess', me);

                    Ext.History.add(Ext.String.format("claimworkbc/BuildContractClaimWork/{0}/{1}/{2}/aftercreatedoc", params.claimWorkId, data.Id, params.actionUrl));

                    me.controller.unmask();
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка формирования документа!', e.message || e);
                });
            },
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'courtClaimBcPrintAspect',
            buttonSelector: 'clwlawsuitbccourtclaiminfopanel gkhbuttonprint',
            codeForm: 'CourtClaim',
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

                    controller.getAspect('courtClaimBcEditPanelAspect').setData(docId);
                }
            }
        },
        //#endregion
        {
            xtype: 'gkheditpanel',
            name: 'collectionBcEditPanelAspect',
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
            name: 'lawsuitBcDocAspect',
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
            name: 'lawsuitBcEditPanelAspect',
            editPanelSelector: 'clwlawsuitbcmaininfopanel',
            modelName: 'claimwork.lawsuit.Petition',
            docCreateAspectName: 'lawsuitBcCreateButtonAspect',
            otherActions: function (actions) {
                var me = this;
                actions['clwlawsuitbcmaininfopanel [name=Suspended]'] = { 'change': { fn: me.onChangeCheckBox, scope: me } };
            },
            onChangeCheckBox: function (chkbox, newValue) {
                var panel = chkbox.up('clwlawsuitbcmaininfopanel'),
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

                    var panel = asp.getPanel(),
                        tabPanel = panel.up('tabpanel');
                    panel.setDisabled(false);
                    tabPanel.setActiveTab(panel);

                    panel.down('[name=JuridicalSectorMu]').setDisabled(Gkh.config.ClaimWork.Common.General.CourtName === 10);

                    return true;
                }
            },
            recordDestroy: function (record, questionStr) {
                var me = this;
                Ext.Msg.confirm('Удаление записи!', questionStr, function (result) {
                    if (result === 'yes') {
                        me.mask('Удаление', B4.getBody());
                        record.destroy()
                            .next(function () {
                                var view = me.controller.getMainView(),
                                    claimworkId = me.controller.getContextValue(view, 'claimWorkId'),
                                    type = me.controller.getContextValue(view, 'type');
                                B4.QuickMsg.msg('Удаление', 'Документ удален успешно', 'success');
                                if (claimworkId && type) {
                                    Ext.History.add(Ext.String.format('claimworkbc/BuildContractClaimWork/{0}/deletedocument', claimworkId));
                                }
                                me.unmask();
                            }, me)
                            .error(function (result) {
                                me.unmask();
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            }, me);
                    }
                }, me);
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'lawsuitCourtBcAspect',
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
            name: 'lawsuitDocumentationBcAspect',
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
            name: 'lawsuitBcCreateButtonAspect',
            buttonSelector: 'clwlawsuibctmaininfopanel acceptmenubutton',
            containerSelector: 'clwlawsuitbcmaininfopanel'
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'lawsuitBcPrintAspect',
            buttonSelector: 'clwlawsuitbcmaininfopanel gkhbuttonprint',
            codeForm: 'LawSuit',
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

                    controller.getAspect('lawsuitBcEditPanelAspect').setData(docId);
                }
            },
            onBeforeLoadReportStore: function (store, operation) {
                operation.params = {};
                operation.params.codeForm = this.codeForm;
            }
        }
    ],

    getCurrentDoc: function () {
        var me = this;
        return me.getContextValue(me.getMainComponent(), 'docId');
    },

    init: function () {
        var me = this,
            actions = [];

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('clwbclawsuiteditpanel'),
            gridDoc = view.down('claimworklawsuitdocgrid'),
            gridCourt = view.down('claimworklawsuitcourtgrid'),
            gridDocumentation = view.down('claimworklawsuitdocumentationgrid'),
            storeDoc = gridDoc.getStore(),
            storCourt = gridCourt.getStore(),
            storeDocumentation = gridDocumentation.getStore();

        view.ctxKey = Ext.String.format('claimworkbc/BuildContractClaimWork/{0}/{1}/lawsuit', id, docId);
        me.bindContext(view);

        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', 'BuildContractClaimWork');
        me.setContextValue(view, 'docId', docId);
        me.setContextValue(view, 'docType', 'lawsuit');
        me.application.deployView(view, 'claim_work_bc');

        me.getAspect('lawsuitBcEditPanelAspect').setData(docId);

        me.getAspect('collectionBcEditPanelAspect').setData(docId);

        me.getAspect('lawsuitBcCreateButtonAspect').setData(id, 'BuildContractClaimWork');
        me.getAspect('lawsuitBcPrintAspect').loadReportStore();


        //#region Court claim aspects init
        me.getAspect('courtClaimBcEditPanelAspect').setData(docId);
        me.getAspect('courtClaimBcAcceptMenuAspect').setData(id, 'BuildContractClaimWork');
        me.getAspect('courtClaimBcPrintAspect').loadReportStore();
        //#endregion

        storeDoc.clearFilter(true);
        storeDoc.filter('docId', docId);

        storCourt.clearFilter(true);
        storCourt.filter('docId', docId);

        storeDocumentation.clearFilter(true);
        storeDocumentation.filter('docId', docId);
    }
});