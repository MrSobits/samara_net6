Ext.define('B4.controller.claimwork.RestructDebt', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.ClaimWorkButtonPrintAspect',
        'B4.model.claimwork.RestructDebt',
        'B4.enums.RestructDebtDocumentState',
        'B4.enums.DebtorType',
        'B4.model.claimwork.restructdebt.ScheduleParam',
        'B4.view.claimwork.restructdebt.MainPanel',
        'B4.view.claimwork.restructdebt.Edit',
        'B4.view.claimwork.restructdebt.AddWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'restructdebtpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'restructDebtEditPanelAspect',
            editPanelSelector: 'restructdebteditpanel',
            modelName: 'claimwork.RestructDebt',
            otherActions: function (actions) {
                var asp = this;
                actions[asp.editPanelSelector + ' b4enumcombo[name=DocumentState]'] = {
                    change: {
                        fn: function(field, newValue, oldValue) {
                            var fields = field.up('panel[name=RevokeContract]')
                                    .query('textfield:not([name=DocumentState])') ||
                                    [],
                                form = field.up('form').getForm() || {};

                            Ext.Array.each(fields, function(f) {
                                f.setDisabled(newValue == B4.enums.RestructDebtDocumentState.Active);
                            });

                            form.isValid();
                        },
                        scope: asp
                    }
                };

                actions[asp.editPanelSelector + ' b4deletebutton'] = {
                    click: {
                        fn: asp.deleteDocument,
                        scope: asp
                    }
                };
            },

            deleteDocument: function() {
                var asp = this,
                    me = asp.controller,
                    view = me.getMainView(),
                    panel = asp.getPanel(),
                    record = panel.getForm().getRecord(),
                    claimworkId = me.getContextValue(view, 'claimWorkId'),
                    type = me.getContextValue(view, 'type'),
                    docId = me.getContextValue(view, 'docId');

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ',
                    function(result) {
                        if (result === 'yes') {
                            me.mask('Удаление');
                            record.destroy()
                                .next(function() {
                                    B4.QuickMsg.msg('Удаление', 'Документ удален успешно', 'success');
                                    if (claimworkId && type) {
                                        Ext.History.add(Ext.String.format(
                                            "claimwork/{0}/{1}/deletedocument", type, claimworkId));
                                    }
                                    me.unmask();
                                })
                                .error(function(result) {
                                    me.unmask();
                                    Ext.Msg.alert('Ошибка удаления!',
                                        Ext.isString(result.responseData)
                                        ? result.responseData
                                        : result.responseData.message);
                                });
                        }
                    });
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'restructDebtSheduleGridAspect',
            gridSelector: 'restructdebtpanel restructschedulegrid',
            modelName: 'claimwork.restructdebt.ScheduleParam',
            editFormSelector: 'restructdebtaddwindow',
            editWindowView: 'claimwork.restructdebt.AddWindow',
            rowAction: Function.prototype,
            deleteRecord: Function.prototype,
            listeners: {
                beforegridaction: function(asp, grid, action) {
                    var me = asp.controller;
                    if (action === 'add') {
                        return me.getAspect('restructDebtEditPanelAspect').checkFields();
                    }

                    return true;
                },
                beforesetformdata: function (asp, record, window) {
                    var me = asp.controller,
                        mainView = me.getMainView(),
                        claimWorkId = me.getContextValue(mainView, 'claimWorkId'),
                        debtRecord = mainView.down('restructdebteditpanel').getRecord(),
                        persAccSelectField = window.down('b4selectfield[name=PersonalAccount]');

                    persAccSelectField.getStore().on('beforeload', function (store, operation) {
                        Ext.applyIf(operation.params, {
                            claimWorkId: claimWorkId
                        });
                    }, asp);

                    record.set('RestructDebt', debtRecord.getId());
                },
                beforesave: function (asp, record) {
                    record.phantom = false;
                }
            },
            otherActions: function (actions) {
                var asp = this;
                actions[asp.editFormSelector + ' b4selectfield[name=PersonalAccount]'] = {
                    change: {
                        fn: function (field, value) {
                            var totalDebtSum = asp.getForm().down('numberfield[name=TotalDebtSum]'),
                                debtSum = (value && value.CurrChargeBaseTariffDebt) || 0,
                                penalty = (value && value.CurrPenaltyDebt) || 0,
                                sum = debtSum + penalty;
                            if (sum > 0) {
                                totalDebtSum.setValue(sum);
                            }
                        },
                        scope: asp
                    },
                    beforeload: {
                        fn: function (field, operation, store) {
                            var me = asp.controller,
                                mainView = me.getMainView(),
                                restructDebtId = me.getContextValue(mainView, 'docId');
                            Ext.apply(operation.params, {
                                restructDebtId: restructDebtId
                            })
                        },
                        scope: asp
                    }
                }
            },
        },

        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'restructDebtPrintAspect',
            buttonSelector: 'restructdebtpanel gkhbuttonprint',
            codeForm: 'RestructDebt, RestructDebtAmicAgr',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = { DocumentId: me.controller.getContextValue(view, 'docId') };

                me.params.userParams = Ext.JSON.encode(param);
            },
            listeners: {
                onprintsucess: function (asp) {
                    var controller = asp.controller,
                        view = controller.getMainView(),
                        docId = controller.getContextValue(view, 'docId');

                    controller.getAspect('restructDebtEditPanelAspect').setData(docId);
                }
            },
            onBeforeLoadReportStore: function(store, operation) {
                var asp = this;
                Ext.apply(operation,
                    {
                        params: {
                            codeForm: asp.codeForm,
                            type: asp.debtorType
                        }
                    });
            },
            setCodeForm: function(type, docType) {
                var asp = this;
                debugger;
                asp.codeForm = docType === 'restructdebtamicagr'
                    ? 'RestructDebtAmicAgr'
                    : 'RestructDebt';

                asp.debtorType = type === 'LegalClaimWork'
                    ? B4.enums.DebtorType.Legal
                    : type === 'IndividualClaimWork'
                        ? B4.enums.DebtorType.Individual
                        : B4.enums.DebtorType.NotSet;

                asp.loadReportStore();
            }
        },
    ],

    index: function (type, id, docId, docType) {
        var me = this,
            view = me.getMainView() ||
                Ext.widget('restructdebtpanel', {
                    title: docType === 'restructdebtamicagr'
                        ? 'Реструктуризация по мировому соглашению'
                        : 'Реструктуризация долга',
                }),
            scheduleStore = view.down('restructschedulegrid').getStore();

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}/{2}/{3}', type, id, docId, docType);

        me.bindContext(view);
        
        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.setContextValue(view, 'docId', docId);
        me.setContextValue(view, 'docType', docType);
        me.application.deployView(view, 'claim_work');

        scheduleStore.on('beforeload', function (store, operation) {
            Ext.applyIf(operation.params, {
                restructDebtId: docId
            });
            if (operation.sorters.length > 1) {
                operation.sorters.splice(0, 1);
            }
        }, me);

        me.getAspect('restructDebtEditPanelAspect').setData(docId);
        me.getAspect('restructDebtSheduleGridAspect').updateGrid();
      //  me.getAspect('restructDebtPrintAspect').setCodeForm(type, docType);
        me.getAspect('restructDebtPrintAspect').loadReportStore();
    }
});