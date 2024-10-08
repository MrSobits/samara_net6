﻿Ext.define('B4.controller.claimwork.IndividualClaimWork', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.view.claimwork.IndividualGrid',
        'B4.aspects.ButtonDataExport',
        'B4.controller.claimwork.Navi',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkButtonPrintAspect',
        'B4.aspects.DebtorClaimWorkAspect',
        'B4.enums.DocumentFormationKind',
        'B4.enums.DebtorType',
        'B4.enums.ClaimWorkTypeBase'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'claimwork.IndividualClaimWork'
    ],

    stores: [
         'claimwork.IndividualClaimWork'
    ],

    views: [
        'claimwork.IndividualGrid',
        'claimwork.IndividualPrintWindow'
    ],

    refs: [
       {
           ref: 'mainView',
           selector: 'individualclaimworkgrid'
       }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.Individual.Update', applyTo: 'button[actionName=updateState]', selector: 'individualclaimworkgrid'
                },
                {
                    name: 'Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate', applyTo: 'button[actionName=btnCalcDebtDate]', selector: 'individualclaimworkgrid'
                },
                {
                    name: 'Clw.ClaimWork.Individual.Print', applyTo: 'gkhbuttonprint', selector: 'individualclaimworkgrid'
                },
                {
                    name: 'Clw.ClaimWork.Individual.Columns.BaseTariffDebtSum', applyTo: '[dataIndex=CurrChargeBaseTariffDebt]', selector: 'individualclaimworkgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Individual.Columns.DecisionTariffDebtSum', applyTo: '[dataIndex=CurrChargeDecisionTariffDebt]', selector: 'individualclaimworkgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Individual.Columns.DebtSum', applyTo: '[dataIndex=CurrChargeDebt]', selector: 'individualclaimworkgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Individual.Delete', applyTo: 'b4deletecolumn', selector: 'individualclaimworkgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
                
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'individualClaimWorkButtonExportAspect',
            gridSelector: 'individualclaimworkgrid',
            buttonSelector: 'individualclaimworkgrid #btnExport',
            controllerName: 'IndividualClaimWork',
            actionName: 'Export',
            usePost: true
        },
        {
            xtype: 'debtorclaimworkaspect',
            name: 'individualClaimworkAspect',
            gridSelector: 'individualclaimworkgrid',
            storeName: 'claimwork.IndividualClaimWork',
            modelName: 'claimwork.IndividualClaimWork',
            controllerEditName: 'B4.controller.claimwork.Navi',
            entityName: 'IndividualClaimWork',
            getDebtorType: function () {
                return B4.enums.DebtorType.Individual;
            }
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'individualPrintAspect',
            buttonSelector: 'individualclaimworkgrid gkhbuttonprint',
            codeForm: 'AccountNotification, CourtClaimAccount, LawSuitClaimAccount, LawSuitAccount, PretensionAccount',
            onBeforeLoadReportStore: function (store, operation) {
                operation.params = {};
                operation.params.codeForm = this.codeForm;
                operation.params.type = B4.enums.DebtorType.Individual;
            },
            onMenuItemClick: function (itemMenu) {
                var me = this,
                    PrintAccountGrid,
                    store,
                    claimWorkIds = [],
                    buttonPrint,
                    isMany = false,
                    claimworkGrid = me.controller.getMainView(),
                    claimworks = claimworkGrid.getSelectionModel().getSelection(),
                    printWin = Ext.create('B4.view.claimwork.IndividualPrintWindow',
                        {
                            renderTo: B4.getBody().getActiveTab().getEl(),
                            actionName: itemMenu.actionName
                        });

                isMany = Ext.Array.some(claimworks, function(claimwork) {
                    if (claimwork.get('AccountsNumber') > 1) {
                        return true;
                    }
                });

                if (isMany) {
                    Ext.each(claimworks, function(claimwork) {
                        claimWorkIds.push(claimwork.getId());
                    });

                    PrintAccountGrid = printWin.down('grid');
                    store = PrintAccountGrid.getStore();
                    buttonPrint = PrintAccountGrid.down('button[action=print]');

                    buttonPrint.on('click', me.printReport, me);

                    store.on('beforeload', function(store, operation) {
                        operation.params.claimWorkIds = Ext.encode(claimWorkIds);
                    }, me);

                    store.load();
                    printWin.show();
                } else {
                    me.massPrintReport(itemMenu);
                }
            },
            massPrintReport: function (itemMenu) {
                var me = this,
                    grid = me.controller.getMainView(),
                    sm = grid.getSelectionModel(),
                    selected = sm.getSelection(),
                    selectedIds = Ext.Array.map(selected, function (el) { return el.get('Id'); }),
                    param = {
                        claimworkIds: Ext.JSON.encode(selectedIds),
                        reportId: itemMenu.actionName,
                        isMassPrint: true
                    };

                Ext.apply(me.params, param);

                if (me.getClaimWorkType()) {
                    Ext.apply(me.params, {
                        claimWorkType: me.getClaimWorkType()
                    });
                }

                if (selected.length === 0) {
                    Ext.Msg.alert('Внимание', 'Необходимо выбрать записи для печати');
                } else {
                    Ext.Msg.prompt({
                        title: 'Формирование документов на печать',
                        msg: 'Сформировать документы для выбранных записей?',
                        buttons: Ext.Msg.OKCANCEL,
                        fn: function (btnId, text) {
                            if (btnId === 'ok') {
                                me.mask('Формирование документов...');
                                B4.Ajax.request({
                                    url: B4.Url.action('MassReportAccountPrint', 'ClaimWorkReport'),
                                    params: me.params,
                                    timeout: 60 * 60 * 1000
                                }).next(function (resp) {
                                    var tryDecoded;

                                    me.unmask();
                                    try {
                                        tryDecoded = Ext.JSON.decode(resp.responseText);
                                    } catch (e) {
                                        tryDecoded = {};
                                    }

                                    var id = resp.data ? resp.data : tryDecoded.data;

                                    if (id > 0) {
                                        Ext.DomHelper.append(document.body, {
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
                                }).error(function (err) {
                                    me.unmask();
                                    Ext.Msg.alert('Ошибка', err.message || err.message || err);
                                });
                            }
                        }
                    });
                }
            },
            printReport: function (button) {
                var me = this,
                    view = me.controller.getMainView(),
                    window = button.up('window'),
                    grid = button.up('grid'),
                    controller = me.controller,
                    records = grid.getSelectionModel().getSelection(),
                    recIds = [],
                    frame = Ext.get('downloadIframe');

                if (frame != null) {
                    Ext.destroy(frame);
                }

                Ext.each(records,
                    function (rec) {
                        recIds.push(rec.get('Id'));
                    });

                me.params = { recIds: Ext.JSON.encode(recIds) };

                if (recIds.length == 0) {
                    Ext.Msg.alert('Ошибка', 'Необходимо выбрать хотя бы одну запись для печати');
                    return;
                }

                Ext.apply(me.params, { reportId: window.actionName });

                controller.mask('Обработка...', window);
                B4.Ajax.request({
                    url: B4.Url.action('MassReportAccountPrint', 'ClaimWorkReport'),
                    params: me.params,
                    timeout: 60 * 60 * 1000
                })
                    .next(function (resp) {
                        var tryDecoded;

                        controller.unmask();
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
                    .error(function (err) {
                        controller.unmask();
                        Ext.Msg.alert('Ошибка', err.message || err);
                    });
            }
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'individualclaimworkgrid button[actionName=btnCalcDebtDate]': { click: { fn: me.calcDebtDate, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('individualclaimworkgrid');
        me.bindContext(view);
        me.application.deployView(view);

        me.getAspect('individualPrintAspect').loadReportStore();
        me.getAspect('individualClaimworkAspect').getDocsTypeToCreate();

        view.getStore().load();
    },

    calcDebtDate: function (btn) {
        var me = this,
            claimWorkIds = [],
            grid = btn.up('individualclaimworkgrid'),
            claimworks = grid.getSelectionModel().getSelection();
        Ext.each(claimworks, function (claimwork) {
            claimWorkIds.push(claimwork.getId());
        });
       
        me.mask('Постановка задачи расчёта эталонных оплат', this.getMainComponent());

        B4.Ajax.request({
            url: B4.Url.action('DebtStartDateCalculateMass', 'LawsuitOwnerInfo'),
            params: {
                clwIds: Ext.encode(claimWorkIds)
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            var data = Ext.decode(response.responseText);
            B4.QuickMsg.msg('Сообщение', data.data, 'success');
            return true;
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Сообщение', e.message, 'error');
            return false;
        });
    }
});