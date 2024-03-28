Ext.define('B4.controller.claimwork.IndividualClaimWork', {
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
            codeForm: 'AccountNotification, LawSuitClaimAccount, LawSuitAccount, AccountClaim185, AccountClaim512, PretensionAccount',
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
                                    timeout: 5 * 60 * 1000
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
                        timeout: 5 * 60 * 1000
                    })
                    .next(function(resp) {
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
                    .error(function(err) {
                        controller.unmask();
                        Ext.Msg.alert('Ошибка', err.message || err);
                    });
            }
        }
    ],

    init: function () {
        var me = this;
        this.control({

            'individualclaimworkgrid #dfDateStart': { change: { fn: this.onChangeDateStart, scope: this } },
            'individualclaimworkgrid #dfDateEnd': { change: { fn: this.onChangeDateEnd, scope: this } },
            'individualclaimworkgrid #cbShowPausedClw': { change: { fn: this.onChangeCheckbox, scope: this } },
            'individualclaimworkgrid #btupdateState': { click: { fn: this.updateGrid1, scope: this } },

        });
        me.params = {};
        this.getStore('claimwork.IndividualClaimWork').on('beforeload', this.onBeforeLoadIndividualClaimWork, this);
        me.callParent(arguments);
    },

    updateGrid1: function () {
        this.getStore('claimwork.IndividualClaimWork').load();
    },
    onBeforeLoadIndividualClaimWork: function (store, operation) {
        var me = this,
            view = me.getMainView() || Ext.widget('individualclaimworkgrid');
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;

        }
    },

    onChangeDateStart: function (field, newValue, oldValue) {
        if (this.params) {
            this.params.dateStart = newValue;
        }
    },
    onChangeDateEnd: function (field, newValue, oldValue) {
        if (this.params) {
            this.params.dateEnd = newValue;
        }
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('individualclaimworkgrid');
        me.bindContext(view);
        me.application.deployView(view);

        me.getAspect('individualPrintAspect').loadReportStore();
        me.getAspect('individualClaimworkAspect').getDocsTypeToCreate();
        this.params = {};
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.params.showPaused = false;
        this.getStore('claimwork.IndividualClaimWork').load();
    }
});