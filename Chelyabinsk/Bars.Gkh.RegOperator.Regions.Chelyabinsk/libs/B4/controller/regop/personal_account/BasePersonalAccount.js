Ext.define('B4.controller.regop.personal_account.BasePersonalAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'Ext.tree.Panel',
        'B4.form.Window',
        'B4.form.FileField',
        'B4.aspects.regop.PersAccImportAspect',
        'B4.aspects.GridEditCtxWindow',
        'B4.mixins.Context',
        'B4.aspects.GridEditForm',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ButtonDataExport',
        'Ext.ux.data.PagingMemoryProxy',
        'Ext.ux.IFrame',
        'B4.enums.PerformedWorkFundsDistributionType',
        'B4.enums.regop.WalletType',
        'B4.enums.regop.SplitAccountDistributionType',
        'B4.enums.PerfWorkChargeType',
        'B4.enums.SaldoChangeOperationType',
        'B4.enums.SaldoChangeSaldoFromType',
        'B4.enums.SaldoChangeSaldoToType',
        'B4.enums.RoomOwnershipType',

        'B4.aspects.regop.personal_account.action.SplitAccount',
        'B4.aspects.regop.personal_account.action.ReopenAccount',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    stores: [
        'regop.personal_account.BasePersonalAccount',
        'regop.personal_account.PersonalAccountCharge',
        'dict.PrivilegedCategory'
    ],

    views: [
        'regop.personal_account.PersonalAccountGrid',
        'regop.personal_account.PersonalAccountChargeWindow',
        'regop.personal_account.action.SetBalance',
        'regop.personal_account.action.SetPenalty',
        'regop.personal_account.action.PaymentDocumentType',
        'regop.personal_account.ExportSberWindow',
        'regop.personal_account.action.CancelPenalty.MainWindow',
        'regop.personal_account.action.CancelPenalty.AccountsGrid',
        'regop.personal_account.action.DistributeFundsForPerformedWorkWindow',
        'regop.personal_account.action.PersAccGroup.MassOperationsPersAccGroupWindow',
        'regop.personal_account.action.PersAccGroup.AddGroupGrid',
        'regop.personal_account.action.CorrectPaymentsWin',
        'regop.personal_account.persaccgroup.AddGroupFromMOWindow',
        'regop.personal_account.action.BanRecalcWin',
        'regop.personal_account.action.BanRecalcGrid',
        'regop.personal_account.action.CorrectPaymentsGrid',
        'regop.personal_account.action.RepaymentWindow',
        'regop.personal_account.action.PersonalAccountSplit.MainWindow',
        'regop.personal_account.action.CalcDebtWin',
        'regop.personal_account.action.CalcDebtGrid',
        'regop.personal_account.action.SetSignsWin'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'paccountgrid'
        },
        {
            ref: 'editView',
            selector: 'paccountedit'
        },
        {
            ref: 'period',
            selector: 'paccountgrid [name="ChargePeriod"]'
        },
        {
            ref: 'cashPaymentCenter',
            selector: 'paccountgrid [name="CashPaymentCenter"]'
        },
        {
            ref: 'providerCode',
            selector: 'paccountgrid [name="providerCode"]'
        }
    ],
        
    models: [
        'regop.personal_account.PersAccGroup', 
        'regop.personal_account.SplitAccountInfo',
        'Room'
    ],

    openPeriod: null,

    aspects: [
        {
            xtype: 'persaccimportaspect'
        },
        {
            xtype: 'grideditformaspect',
            name: 'BasePersonalAccountEditFormAspect',
            gridSelector: 'paccountgrid',
            storeName: 'regop.personal_account.BasePersonalAccount',
            modelName: 'regop.personal_account.BasePersonalAccount',
            editRecord: function(record) {
                Ext.History.add('personal_acc_details/' + record.get('Id'));
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            },
            permissions: [
                { name: 'GkhRegOp.PersonalAccount.Registry.ChargePeriod_View', applyTo: 'b4selectfield[name=ChargePeriod]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.UpdateCache_View', applyTo: 'button[action=UpdateCache]', selector: 'paccountgrid' },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.ExportCalculation_View',
                    applyTo: 'b4combobox[name=providerCode]',
                    selector: 'paccountgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.ExportCalculation_View',
                    applyTo: 'buttongroup [action=ExportCalculation]',
                    selector: 'paccountgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.Action.ExportSaldo',
                    applyTo: 'buttongroup [action=ExportSaldo]',
                    selector: 'paccountgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.Settings.PersAccGroup.Create',
                    applyTo: 'b4addbutton',
                    selector: 'masspaccountaddgroupgrid'
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.Action.MassPersAccGroupOperation.Create',
                    applyTo: 'button[itemId=btnAddToGroup]',
                    selector: 'masspaccountaddgroupgrid'
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.Action.MassPersAccGroupOperation.Delete',
                    applyTo: 'b4addbutton',
                    selector: 'masspaccountaddgroupgrid'
                },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action_View', applyTo: 'buttongroup', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.Charge', applyTo: 'buttongroup button[action=Charge]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.Others', applyTo: 'buttongroup button[name=accountoperation]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.PaymentDoc', applyTo: 'buttongroup [action=GetPaymentDocuments]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.PartiallyPaymentDoc', applyTo: 'buttongroup [action=GetPartialPaymentDocuments]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.ZeroPaymentDocs', applyTo: 'buttongroup [action=GetZeroPaymentDocs]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.ExportToVtscp', applyTo: 'buttongroup [action=ExportToVtscp]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.ExportPersonalAccounts', applyTo: 'buttongroup [action=ExportPersonalAccounts]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.ExportPenalty', applyTo: 'buttongroup [action=ExportPenalty]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.ExportPenaltyExcel', applyTo: 'buttongroup [action=ExportPenaltyExcel]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Action.DeleteAccount', applyTo: 'buttongroup button[action=RemovePersonalAccounts]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Mode.View', applyTo: '[name=ModeGroup]', selector: 'paccountgrid' },

                { name: 'GkhRegOp.PersonalAccount.Registry.Field.AccountOwner_View', applyTo: 'gridcolumn[dataIndex=AccountOwner]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Field.OwnerType_View', applyTo: 'gridcolumn[dataIndex=OwnerType]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Field.HasCharges_View', applyTo: 'gridcolumn[dataIndex=HasCharges]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Field.PersAccNumExternalSystems_View', applyTo: 'gridcolumn[dataIndex=PersAccNumExternalSystems]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Field.PrivilegedCategory_View', applyTo: '[name=PrivilegedCategory]', selector: 'paccountgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.Field.CashPaymentCenter_View', applyTo: '[name=CashPaymentCenter]', selector: 'paccountgrid' },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.Field.PrivilegedCategory_View',
                    applyTo: '[name=CheckShowAll]',
                    selector: 'paccountgrid',
                    applyBy: function(component, allowed) {
                        component.colspan = allowed ? 1 : 2;
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.Field.CashPaymentCenter_View',
                    applyTo: '[name=CheckShowSums]',
                    selector: 'paccountgrid',
                    applyBy: function(component, allowed) {
                        component.colspan = allowed ? 1 : 2;
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.Action.ReOpenAccountOperation.OpenBeforeClose_View',
                    applyTo: '[name=OpenBeforeClose]',
                    selector: '[name=reopenaccountoperationwin]'
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'BasePersonalAccountButtonExportExcelAspect',
            gridSelector: 'paccountgrid',
            buttonSelector: 'paccountgrid [action="ExportSaldo"]',
            controllerName: 'PersonAccountOperation',
            actionName: 'ExportExcelSaldo',
            usePost: true,

            // переопределяем, т.к. необходим фильтр по выделенным ЛС (просто скопировал реализацию)
            btnAction: function () {
                var grid = this.getGrid();

                if (grid) {
                    var store = grid.getStore();
                    var columns = grid.columns,
                        form = grid.up(),
                        sm = grid.getSelectionModel(),
                        selected = Ext.Array.map(sm.getSelection(), function(el) { return el.get('Id'); });

                    var headers = [];
                    var dataIndexes = [];

                    Ext.each(columns, function (res) {
                        if (!res.hidden && res.header != "&#160;" && (res.dataIndex || res.dataExportAlias)) {
                            var dataIndex = res.dataIndex || res.dataExportAlias,
                                index = dataIndex.indexOf(".");
                            headers.push(res.text);
                            dataIndexes.push(index >= 0 ? dataIndex.substring(0, index) : dataIndex);
                        }
                    });

                    var params = {};

                    if (headers.length > 0) {
                        Ext.apply(params, { headers: headers, dataIndexes: dataIndexes });
                    }

                    if (store.sortInfo != null) {
                        Ext.apply(params, {
                            sort: store.sortInfo.field,
                            dir: store.sortInfo.direction
                        });
                    }

                    // добавляем в параметры выделденные ЛС
                    params.accIds = Ext.JSON.encode(selected);

                    Ext.apply(params, store.lastOptions.params);

                    if (this.usePost) {
                        this.downloadViaPost(params);
                    } else {
                        this.downloadViaGet(params);
                    }
                }
            },
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'BasePersonalAccountButtonExportAspect',
            gridSelector: 'paccountgrid',
            buttonSelector: 'paccountgrid [action="ExportToExcel"]',
            controllerName: 'BasePersonalAccount',
            actionName: 'Export',
            usePost: true,

            // все нижеследующее есть копия правок внесенных в аспект
            // это нужно убрать, как только будут распространены
            // обновленные пакеты B4
            btnAction: function() {
                var me = this,
                    grid = me.getGrid(),
                    store,
                    columns,
                    headers = [],
                    dataIndexes = [],
                    params = {};

                if (grid) {
                    store = grid.getStore();
                    columns = grid.columns;

                    Ext.each(columns, function(res) {
                        if (!res.hidden && res.header != "&#160;" && res.dataIndex) {
                            var index = res.dataIndex.indexOf(".");
                            headers.push(res.text);
                            dataIndexes.push(index >= 0 ? res.dataIdnex.substring(0, index) : res.dataIndex);
                        }
                    });

                    if (headers.length > 0) {
                        Ext.apply(params, { headers: headers, dataIndexes: dataIndexes });
                    }

                    if (store.sortInfo != null) {
                        Ext.apply(params, {
                            sort: store.sortInfo.field,
                            dir: store.sortInfo.direction
                        });
                    }

                    Ext.apply(params, store.lastOptions.params);

                    if (me.usePost) {
                        me.downloadViaPost(params);
                    } else {
                        me.downloadViaGet(params);
                    }
                }
            },

            downloadViaGet: function(params) {
                var me = this,
                    urlParams = Ext.urlEncode(params),
                    newUrl = Ext.urlAppend('/' + me.controllerName + '/' + me.actionName + '/?' + urlParams, '_dc=' + (new Date().getTime()));
                window.open(B4.Url.action(newUrl));
            },

            downloadViaPost: function(params) {
                var me = this,
                    action = B4.Url.action('/' + me.controllerName + '/' + me.actionName) + '?_dc=' + (new Date().getTime()),
                    form,
                    inputs = [];

                Ext.iterate(params, function(key, value) {
                    if (!value) {
                        return;
                    }

                    if (Ext.isArray(value)) {
                        Ext.each(value, function(item) {
                            inputs.push({ tag: 'input', type: 'hidden', name: key, value: Ext.String.htmlEncode(item.toString()) });
                        });
                    } else {
                        inputs.push({ tag: 'input', type: 'hidden', name: key, value: Ext.String.htmlEncode(value.toString()) });
                    }
                });

                form = Ext.DomHelper.append(document.body, { tag: 'form', action: action, method: 'POST', target: '_blank' });
                Ext.DomHelper.append(form, inputs);

                form.submit();
                form.remove();
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'PersonalAccountMassGroupOperationsWindowAspect',
            gridSelector: 'masspaccountaddgroupgrid',
            editFormSelector: 'persaccgroupfrommoaddwindow', // Своя форма. Чтобы селектор отличался от той, которая используется в карточке ЛС.
            modelName: 'regop.personal_account.PersAccGroup',
            editWindowView: 'regop.personal_account.persaccgroup.AddGroupFromMOWindow',
            operationWindowSelector: 'massoperationspersaccgroupwindow',

            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model = me.getModel(null);

                if (!id) {
                    me.setFormData(new model({ Id: 0 }));
                }
            },

            otherActions: function (actions) {
                Ext.apply(actions[this.gridSelector] || (actions[this.gridSelector] = {}),
                    {
                        'store.beforeload': function (st, operation) {
                            operation.params.isSystem = B4.enums.YesNo.No;
                        },

                        'render': function (grid) {
                            var store = grid.getStore();
                            store.load();
                        }
                    }
                );

                actions[this.gridSelector + ' b4closebutton'] = {
                    'click': function (grid) {
                        var form = grid.up('massoperationspersaccgroupwindow');
                        if (form) {
                            form.close();
                        }
                    }
                };
            }
        },
        {
            xtype: 'splitaccountaspect',
            name: 'SplitAccountAspect'
        },
        {
            xtype: 'reopenaccountaspect',
            name: 'ReopenAccountAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            buttonSelector: 'calcdebtwin button[action=Export]',
            codeForm: '',
            name: 'calcdebtexportaspect',
            printController: 'PersonalAccountCalcDebt',
            printAction: 'Export',
            getUserParams: function () {
                var me = this,
                    calcDebtId = me.controller.getContextValue(me.controller.getMainView(), 'calcDebtId');
                me.params.calcDebtId = calcDebtId;
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            buttonSelector: 'calcdebtwin button[action=Print]',
            codeForm: '',
            name: 'calcdebtaspect',
            printController: 'PersonalAccountCalcDebt',
            printAction: 'Print',
            getUserParams: function () {
                var me = this,
                    calcDebtId = me.controller.getContextValue(me.controller.getMainView(), 'calcDebtId');
                me.params.calcDebtId = calcDebtId;
            }
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'paccountgrid': { 'render': { fn: me.onMainViewRender, scope: me } },
            'paccountgrid button[action="Charge"]': { 'click': { fn: me.makeCharge } },
            'paccountgrid button[action="UpdateCache"]': { 'click': { fn: me.updateCache } },
            'paccountgrid button[name=accountoperation] menuitem': { click: { fn: me.onAccountOperationClick, scope: me } },
            'changeownerwin b4savebutton': { click: { fn: me.changeOwner, scope: me } },
            'setbalancewin b4savebutton': { click: { fn: me.setBalance, scope: me } },
            'setpenaltywin b4savebutton': { click: { fn: me.setPenalty, scope: me } },
            'paymentdocumenttypewin button[action=prepare]': { click: { fn: me.selectPaymentDocumentType, scope: me } },
            'mergeaccountwin b4savebutton': { click: { fn: me.mergeAccounts, scope: me } },
            'paccountgrid b4selectfield[name=ChargePeriod]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid [action=GetPaymentDocuments]': { click: { fn: me.getPaymentDocuments, scope: me } },
            'paccountgrid [action=GetPartialPaymentDocuments]': { click: { fn: me.getPartialPaymentDocuments, scope: me } },
            'paccountgrid [action=PaymentDocumentsPreview]': { click: { fn: me.paymentDocumentPreview, scope: me } },
            'paccountgrid b4selectfield[name=crFoundType]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid b4selectfield[name=PrivilegedCategory]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid b4selectfield[name=CashPaymentCenter]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid b4selectfield[name=DeliveryAgent]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid b4selectfield[name=HasCharges]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid checkbox[action=ShowAll]': { change: { fn: me.showAll, scope: me } },
            'paccountgrid checkbox[action=ShowSums]': { change: { fn: me.showSums, scope: me } },
            'paccountgrid [action=GetZeroPaymentDocs]': { click: { fn: me.getPaymentDocuments, scope: me } },
            'paccountgrid [action=ExportPersonalAccounts]': { click: { fn: me.exportPersonalAccounts, scope: me } },
            'paccountgrid [action=ExportToVtscp]': { click: { fn: me.exportToVtscp, scope: me } },
            'paccountgrid [action=ExportPenalty]': { click: { fn: me.exportPenalty, scope: me } },
            'paccountgrid [action=ExportPenaltyExcel]': { click: { fn: me.exportPenaltyExcel, scope: me } },
            'paccountgrid button[action=RemovePersonalAccounts]': { click: { fn: me.removeAccounts, scope: me } },
            'paccountgrid [action=expcalculation] menuitem': { click: { fn: me.getCalculationDocuments, scope: me } },
            'paccountgrid button[action=ExportSber]': { click: { fn: me.sberExport, scope: me } },
            'personalaccountimportstatuswindow b4closebutton': { click: { fn: me.closeImportWindow, scope: me } },
            'persaccpaymentagentgrid b4closebutton': { click: { fn: me.closeExportSberWindow, scope: me } },
            'persaccpaymentagentgrid button[action=select]': { click: { fn: me.onSelectPaymentAgent, scope: me } },
            'persaccpaymentagentgrid b4updatebutton': { click: { fn: me.onUpdatePaymentAgentGrid, scope: me } },
            'cancelchargewin b4savebutton': { click: { fn: me.cancelCharge, scope: me } },
            'repaymentwindow b4savebutton': { click: { fn: me.onApplyRepaymentOperation, scope: me } },
            'cancelpenaltywin b4savebutton': { click: { fn: me.cancelPenalty, scope: me } },
            'correctpaymentswin b4savebutton': { click: { fn: me.correctPayments, scope: me } },
            'paccountgrid #btnClearAllFilters': { click: { fn: me.goToPAccountsGrid, scope: me } },
            'paccountgrid b4selectfield[name=crOwnerType]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid b4selectfield[name=OwnershipType]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid b4selectfield[name=persAccGroup]': { change: { fn: me.updateGrid, scope: me } },
            'paccountgrid radiogroup[name=Mode]': { change: { fn: me.updateGrid, scope: me } },
            'distributefundsforperformedworkgrid': { rowaction: { fn: me.distributeFundsForperformedWorkGridRowAction, scope: me } },
            'distributefundsforperformedworkwindow [action=Distribute]': { click: { fn: me.onDistributePerformedWork, scope: me } },
            'distributefundsforperformedworkwindow [name=DistributionSum]': { change: { fn: me.onDistributionSumChange, scope: me } },
            'distributefundsforperformedworkwindow [action=Accept]': { click: { fn: me.onAcceptDistributionFundsForPerformedWork, scope: me } },
            'masspaccountaddgroupgrid [itemId=btnAddToGroup]': { click: { fn: me.processMassPersAccGroupOperation(true), scope: me } },
            'masspaccountaddgroupgrid [itemId=btnDeleteFromGroups]': { click: { fn: me.processMassPersAccGroupOperation(false), scope: me } },
            'massoperationspersaccgroupwindow [itemId=btnAddToGroup]': { click: { fn: me.processMassPersAccGroupOperation(true), scope: me } },
            'massoperationspersaccgroupwindow [itemId=btnDeleteFromGroups]': { click: { fn: me.processMassPersAccGroupOperation(false), scope: me } },
            'masssaldochangewin b4savebutton': { click: { fn: me.processSaldoChange, scope: me } },
            'masssaldochangewin [name=OperationType]': { change: { fn: me.saldoChangeOperationTypeChange, scope: me } },
            'masssaldochangewin [name=SaldoFrom]': { change: { fn: me.saldoChangeSaldoFromChange, scope: me } },
            'masssaldochangewin [name=SaldoTo]': { change: { fn: me.saldoChangeOperationTypeChange, scope: me }, expand: { fn: me.saldoChangeSaldoToExpand, scope: me } },
            'banrecalcwin b4savebutton': { click: { fn: me.banRecalc, scope: me } },
            'masscancelchargewin b4savebutton': { click: { fn: me.massCancelCharge, scope: me } },
            'calcdebtgrid button[action=calcDebt]': { click: { fn: me.calcDebt, scope: me } },
            'calcdebtwin b4savebutton': { click: { fn: me.saveDebtTransfer, scope: me } },
            'calcdebtwin button[action=export]': { click: { fn: me.onExportButtonClick, scope: me } },
            'setsignswin b4savebutton': { click: { fn: me.settingSigns, scope: me } },
            'calcdebtwin button[action=print]': { click: { fn: me.onPrintButtonClick, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView(),
            firstTime = view === null;

        view = view || Ext.widget('paccountgrid');

        me.bindContext(view);
        me.application.deployView(view);

        this.processParams();

        if (firstTime) {
            view.down('b4selectfield[name = ChargePeriod]').getStore().load();
            view.down('b4selectfield[name = CashPaymentCenter]').getStore().load();

            B4.Ajax.request({
                url: B4.Url.action('GetParams', 'GkhParams'),
                timeout: 9999999
            }).next(function(response) {
                var json = Ext.JSON.decode(response.responseText),
                    col = Ext.ComponentQuery.query('paccountgrid #SettlementColumn')[0];

                if (!json.ShowStlRealityGrid && col) {
                    col.hide();
                }
            }).error(function() {
                Log('Ошибка получения параметров приложения');
            });

            me.getAspect('personalAccImportAspect').loadImportStore();

            var allRows = view.urlToken.indexOf('regop_personal_account/onlyroom/') <= -1;
            me.disableFilter(allRows);
        }
    },

    processParams: function () {
        var token = Ext.History.getToken(),
            ps = token && token.indexOf('?'),
            params,
            o = {};

        if (ps > -1) {
            params = token.substring(ps + 1);
            params.split('&')
                .forEach(function(p) {
                    var parts = p.split('=');
                    if (parts && parts.length) {
                        o[parts[0]] = parts.length === 2 ? parts[1] : true;
                    }
                });

            Ext.History.setHash(token.substring(0, ps));
        }

        this.applyParams(o);
    },

    updateCache: function (button) {
        var me = this;

        Ext.Msg.confirm('Подтвердите действия',
            'Актуализация реестра лицевых счетов может занять несколько минут. Продолжить?',
            function(result) {
                if (result === 'yes') {
                    me.mask('Обновление...', me.getMainView());
                    B4.Ajax.request({
                        url: B4.Url.action('UpdateCache', 'BasePersonalAccount'),
                        timeout: 10 * 60 * 1000 // 10 минут
                    }).next(function () {
                        me.updateGrid();
                        me.unmask();
                    }).error(function (response) {
                        var message = 'Ошибка во время выполнения операции';
                        if (response) {
                            message = response.message;
                        }
                        me.unmask();

                        Ext.Msg.alert('Ошибка', message);
                    });
                }
            });
    },

    applyParams: function (o) {
        var me = this,
            view = this.getMainView(),
            store = view.getStore(),
            sfGroup;

        if (o['pagroup']) {
            var pag = parseInt(o['pagroup']),
                callback;
            if (pag) {
                sfGroup = view.down('[name=persAccGroup]');
                callback = function(rec) {
                    if (!store.isLoading()) {
                        sfGroup.setValue([rec.data]);
                    } else {
                        store.on('load',
                            function() {
                                Ext.defer(callback, 1000, me, [rec]);
                            },
                            this,
                            { single: true });
                    }
                };

                sfGroup.store.model.load(pag,
                    {
                        success: callback
                    });

                return;
            }
        }
        
    },

    // Если в реестр добавляется какойто фильтр то добавляйте его сюда
    // поскольку все действия завязаны на вильтрах и выгрузки и все должно работать через эти фильтры
    getFilterParams: function() {
        var getCrFoundTypeFilter = function() {
            var filterValue = grid.down('b4selectfield[name=crFoundType]').value;
            if (filterValue) {
                return Ext.JSON.encode(filterValue);
            }
            return filterValue;
        };

        var getCrOwnerTypeFilter = function() {
            var filterValue = grid.down('b4selectfield[name=crOwnerType]').value;
            if (filterValue) {
                return Ext.JSON.encode(filterValue);
            }
            return filterValue;
        };

        var getOwnershipTypeFilter = function () {
            var values = [];

            var filterValue = grid.down('b4selectfield[name=OwnershipType]').value;
            if (filterValue) {
                values = Ext.JSON.encode(Ext.Array.map(filterValue, function (el) { return el.Value }));
            }

            return values;
        };

        var getPersAccGroupFilter = function () {
            var filterValue = grid.down('b4selectfield[name=persAccGroup]').value;
            if (filterValue) {
                if (Array.isArray(filterValue)) {
                    return Ext.JSON.encode(Ext.Array.map(filterValue, function (el) { return el.Id }));
                } else if (filterValue == 'All') {
                    return true;
                }
            }
            return filterValue;
        };

        var getDeliveryAgentFilter = function () {
            var filter = {
                ids: [],
                showAll: false
            };

            var filterValue = grid.down('b4selectfield[name=DeliveryAgent]').value;
            if (filterValue) {
                if (Array.isArray(filterValue)) {
                    filter.ids = Ext.JSON.encode(Ext.Array.map(filterValue, function(el) {
                        return el.Id;
                    }));
                } else if (filterValue === 'All') {
                    filter.showAll = true;
                }
            }
            return filter;
        };

        var getHasChargesFilter = function () {
            var values = [];

            var filterValue = grid.down('b4selectfield[name=HasCharges]').value;
            if (filterValue) {
                values = Ext.JSON.encode(Ext.Array.map(filterValue, function (el) { return el.Value }));
            }

            return values;
        };

        var getPrivilegedCategoryFilter = function () {
            var filter = {
                ids: [],
                showAll: false
            };
            var filterValue = grid.down('b4selectfield[name=PrivilegedCategory]').value;

            if (filterValue) {
                if (Array.isArray(filterValue)) {
                    filter.ids = Ext.Array.map(filterValue, function (el) {
                        return el.Id;
                    });
                } else if (filterValue === 'All') {
                    filter.showAll = true;
                }
            }

            return Ext.JSON.encode(filter);
        };

        var getModeFilter = function () {
            var filterValue = grid.down('radiogroup[name=Mode]').getValue();
            if (filterValue) {
                return filterValue.Mode;
            }
            return 0;
        };

        var me = this,
            grid = me.getMainView(),
            showAll = grid.down('checkbox[action=ShowAll]').checked,
            crFund = getCrFoundTypeFilter(),
            crOwnerTypeValues = getCrOwnerTypeFilter(),
            ownershipTypeValues = getOwnershipTypeFilter(),
            deliveryAgentFilter = getDeliveryAgentFilter(),
            hasChargesValues = getHasChargesFilter(),
            persAccGroupValues = null,
            showAllGroupsValue = null,
            period = grid.down('b4selectfield[name=ChargePeriod]').getValue(),
            privilegedCategory = getPrivilegedCategoryFilter(),
            filters = grid.getHeaderFilters(),
            mode = getModeFilter();

        if (grid) {
            var paGroupFilter = getPersAccGroupFilter();

            persAccGroupValues = typeof paGroupFilter == 'string' ? paGroupFilter : null;
            showAllGroupsValue = typeof paGroupFilter == 'boolean' ? paGroupFilter : null;
        }

        var roomId = 0;
        if (grid.urlToken.indexOf('regop_personal_account/onlyroom/') > -1) {
            var roomIdFromUrl = grid.urlToken.replace('regop_personal_account/onlyroom/', '');
            if (roomIdFromUrl !== '' && roomIdFromUrl.length > 0) {
                if (roomIdFromUrl.indexOf('/') > -1) {
                    roomId = roomIdFromUrl.substring(0, roomIdFromUrl.indexOf('/'));
                } else {
                    roomId = roomIdFromUrl;
                }
            }
        }

        var filtersParams = {
            periodId: me.getPeriod().getValue(),
            cashPaymentCenterId: me.getCashPaymentCenter().getValue(),
            //providerCode: me.getProviderCode().getValue(),
            complexFilter: Ext.encode(filters),
            period: period,
            showAll: showAll,
            crFoundType: crFund,
            privilegedCategory: privilegedCategory,
            RoomId: roomId,
            crOwnerTypeValues: crOwnerTypeValues,
            ownershipTypeValues: ownershipTypeValues,
            deliveryAgentIds: deliveryAgentFilter.ids,
            deliveryAgentShowAll: deliveryAgentFilter.showAll,
            hasChargesValues: hasChargesValues,
            groupIds: persAccGroupValues,
            showAllGroups: showAllGroupsValue,
            mode: mode
        };

        Ext.applyIf(filtersParams, me.getMainView().getStore().lastOptions.params)

        return filtersParams;

    },

    closeImportWindow: function(btn) {
        btn.up('personalaccountimportstatuswindow').close();
    },

    showAll: function(cb, newVal) {
        var store = this.getMainView().getStore();

        store.clearFilter(true);
        store.filter('showAll', newVal);
    },

    showSums: function(cb, newVal) {
        var grid = cb.up('paccountgrid');

        var sumsDataIndices = ['SaldoIn', 'SaldoOut', 'CreditedWithPenalty', 'PaidWithPenalty', 'RecalculationWithPenalty'];
        var oldDataIndices = ['PersAccNumExternalSystems', 'OpenDate', 'CloseDate', 'HasCharges', 'AccuralByOwnersDecision'];

        var activate = newVal ? sumsDataIndices : oldDataIndices;
        var deactivate = newVal ? oldDataIndices : sumsDataIndices;

        Ext.each(grid.columns, function(column) {
            if (Ext.Array.contains(activate, column.dataIndex)) {
                column.setVisible(true);
            }
            if (Ext.Array.contains(deactivate, column.dataIndex)) {
                column.setVisible(false);
            }
        });
    },

    edit: function(id) {
        var me = this,
            view = me.getEditView() || Ext.widget('paccountedit', {
                accountId: id
            });

        me.bindContext(view);
        me.application.deployView(view);
    },

    onMainViewRender: function(grid) {
        var me = this;

        grid.getStore().on('beforeLoad', me.onStoreBeforeLoad, me);
        var deliveryAgent = grid.down('[name=DeliveryAgent]');
        if (deliveryAgent) {
            deliveryAgent.getStore().on('load', function() {
                this.insert(0, { Id: -1, Name: 'Без агента доставки' });
            });
        }

        me.getOpenPeriod();
        me.loadAccountOperations(grid);
    },

    updateGrid: function() {
        this.getMainView().getStore().load();
    },

    loadAccountOperations: function(grid) {
        var menuButton = grid.down('button[name=accountoperation]'),
            expButton = grid.down('[action=expcalculation]');

        B4.Ajax.request(
            B4.Url.action('ListAccountOperations', 'BasePersonalAccount')
        ).next(function(r) {
            var list = Ext.decode(r.responseText);

            menuButton.menu.removeAll();

            if (list && list.data) {
                Ext.each(list.data, function(item) {
                    menuButton.menu.add({
                        xtype: 'menuitem',
                        text: item.Name,
                        action: item.Code.toLowerCase()
                    });
                });
            }
        });

        B4.Ajax.request({
            url: B4.Url.action('GetImportProviders', 'Import'),
            params: {
                typeName: 'ChargeOutProxy,PersonalAccountInfoProxy,BenefitsCategoryInfoProxy,PrivilegedCategoryInfoProxy'
            }
        }).next(function(r) {
            var list = Ext.decode(r.responseText);

            expButton.menu.removeAll();

            if (list && list.data) {
                Ext.each(list.data, function(item) {
                    expButton.menu.add({
                        xtype: 'menuitem',
                        text: item.Name,
                        key: item.Key,
                        action: item.Code.toLowerCase()
                    });
                });
            }
        });
    },

    getOpenPeriod: function(callback) {
        var me = this,
            grid = me.getMainView();

        me.mask('Загрузка...', grid);
        B4.Ajax.request({
            url: B4.Url.action('GetOpenPeriod', 'ChargePeriod'),
            timeout: 9999999
        }).next(function(response) {
            if (response == null) {
                return;
            }
            var res = Ext.decode(response.responseText),
                sfPeriod;

            if (!res.data) {
                Ext.Msg.alert('Не создан период!', 'Необходимо создать период!');
                me.unmask();
                return;
            }

            me.openPeriod = res.data.Name;
            sfPeriod = grid.down('b4selectfield[name=ChargePeriod]');

            if (sfPeriod) {
                if (res.data)
                    sfPeriod.setValue(res.data);
                else
                    sfPeriod.setValue(null); // просто чтобы грид обновился 
            }
            me.unmask();

            if (callback && Ext.isFunction(callback)) {
                callback();
            }
        }).error(function(response) {
            me.updateGrid();
            var message = 'Ошибка загрузки данных. Попробуйте обновить старницу';
            if (response) {
                var res = Ext.decode(response.responseText);
                message = res.data.message;
            }
            me.unmask();
            Ext.Msg.alert('Результат', message);
        });
    },

    onStoreBeforeLoad: function(store, operation) {
        var params = this.getFilterParams(),
            mainView = this.getMainView(),
            selectionModel;
        operation.params.periodId = params.periodId;
        operation.params.cashPaymentCenterId = params.cashPaymentCenterId;
        operation.params.crFoundType = params.crFoundType;
        operation.params.privilegedCategory = params.privilegedCategory;
        operation.params.RoomId = params.RoomId;
        operation.params.crOwnerTypeValues = params.crOwnerTypeValues;
        operation.params.ownershipTypeValues = params.ownershipTypeValues;
        operation.params.groupIds = params.groupIds;
        operation.params.showAll = params.showAll;
        operation.params.showAllGroups = params.showAllGroups;
        operation.params.deliveryAgentIds = params.deliveryAgentIds;
        operation.params.deliveryAgentShowAll = params.deliveryAgentShowAll;
        operation.params.hasChargesValues = params.hasChargesValues;
        operation.params.mode = params.mode;

        // очищаем выбранные ЛС
        if (mainView) {
            selectionModel = mainView.getSelectionModel();
            if (selectionModel) {
                selectionModel.clearSelections();
            }
        }
    },

    makeCharge: function () {
        var me = this,
            grid = me.getMainView(),
            totalCount = grid.getStore().totalCount,
            sm = grid.getSelectionModel(),
            selected = Ext.Array.map(sm.getSelection(), function(el) { return el.get('Id'); }),
            filterParams = me.getFilterParams();

        Ext.Msg.prompt({
            title: 'Формирование начислений',
            msg: 'Сформировать Расчет по ' + (selected.length ? selected.length : totalCount) + ' выбранным  лицевым счетам за период ' + (me.openPeriod ? me.openPeriod : '') + '?' +
                '<br>Для счетов в статусе "Не активен" начисления проводиться не будут.',
            buttons: Ext.Msg.OKCANCEL,

            fn: function(btnId, text) {
                if (btnId === "ok") {
                    // Заполняем параметры дополнительными параметры 
                    var postParams = Ext.applyIf(filterParams, {
                        cprocName: text,
                        ids: selected
                    });

                    me.mask('Формирование начислений...', B4.getBody().getActiveTab());
                    B4.Ajax.request(
                        {
                            url: B4.Url.action('MakeUnacceptedCharge', 'Charge'),
                            timeout: 9999999,
                            method: 'POST',
                            params: postParams
                        }
                    ).next(function(resp) {
                        var response = Ext.isEmpty(resp.responseText) ? resp : Ext.JSON.decode(resp.responseText);
                        Ext.Msg.alert('Расчет',
                            response.message
                            || response.Message
                            || "Задача успешно поставлена в очередь на обработку.<br>Информацию по задаче можно увидеть в разделе 'Задачи'");
                        me.unmask();
                    }).error(function(resp) {
                        var response = Ext.isEmpty(resp.responseText) ? resp : Ext.JSON.decode(resp.responseText);
                        Ext.Msg.alert('Расчет', response.message || response.Message);
                        me.unmask();
                    });
                }
            }
        });
    },

    onPersonalAccountEdit: function(grid, actionName, record) {
        var me = this;

        if (actionName === 'edit') {
            if (!me.chargeWindow) {
                me.chargeWindow = Ext.widget('pachargewin', {
                    accountId: record.getId()
                });
            }
            me.chargeWindow.show();
        }
    },

    onAccountOperationClick: function (item) {
        var me = this,
            recs = me.getMainView().getSelectionModel().getSelection(),
            action = item.action.toLowerCase(),
            rec,
            operationRules = {
                'default': function(records) {
                    if (!records || records.length != 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать один лицевой счет!');
                        return false;
                    }
                    return true;
                },
                'mergeaccountoperation': function(records) {
                    if (!records || records.length < 2) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать не менее двух лицевых счетов!');
                        return false;
                    }
                    return true;
                },
                'cancelchargeoperation': function(records) {
                    if (!records) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать лицевой счет!');
                        return false;
                    }
                    return true;
                },
                'masscancelchargeoperation': function (records) {
                    if (!records) {
                        Ext.Msg.alert('Выбор ЛС', 'Ошибка при получении количества лицевых счетов');
                        return false;
                    }
                    return true;
                },
                'penaltycanceloperation': function(records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать лицевой счет!');
                        return false;
                    }
                    return true;
                },
                'correctpaymentsoperation': function (records) {
                    if (!records || records.length !== 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать один лицевой счет!');
                        return false;
                    }
                    return true;
                },
                'repaymentoperation': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать хотя бы один лицевой счет!');
                        return false;
                    }
                    return true;
                },
                'manuallyrecalcoperation': function(records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать лицевой счет!');
                        return false;
                    }
                    return true;
                },
                'distributefundsforperformedworkoperation': function(records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать хотя бы один лицевой счет.');
                        return false;
                    }
                    return true;
                },
                'masspersaccgroupoperation': function(records) {
                    if (!records) {
                        Ext.Msg.alert('Выбор ЛС', 'Ошибка при получении количества лицевых счетов');
                        return false;
                    }
                    return true;
                },
                'reopenaccountoperation': function(records) {
                    if (!records) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать один лицевой счет!');
                        return false;
                    }
                    var noCloce = false;
                    Ext.each(records, function(record) {
                        if (record.data.State.Code == '1' || record.data.State.Code == '4') {
                            noCloce = true;
                        }
                    });

                    if (noCloce) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать закрытые лс');
                        return false;
                    }
                    return true;
                },
                'closeaccountoperation': function(records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать хотя бы один лицевой счет.');
                        return false;
                    }
                    return true;

                },
                'masssaldochangeoperation': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать хотя бы один лицевой счет.');
                        return false;
                    }
                    return true;

                },
                'banrecalcoperation': function (records) {
                    if (!records) {
                        Ext.Msg.alert('Выбор ЛС', 'Ошибка при получении количества лицевых счетов');
                        return false;
                    }
                    return true;
                },

                'turnofflockfromcalculationoperation': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать хотя бы один лицевой счет.');
                        return false;
                    }
                    return true;

                },
                'personalaccountsplitoperation': function (records) {
                    if (!records || records.length !== 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать один лицевой счет!');
                        return false;
                    }

                    if (records[0].get('State').Code != '1') {
                        Ext.Msg.alert('Ошибка', 'Необходимо выбрать открытый лицевой счет!');
                        return false;
                    }

                    return true;

                },
                'calcdebtoperation': function (records) {
                    if (!records || records.length !== 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать один лицевой счет!');
                        return false;
                    }
                    return true;
                },
                'setsignoperation': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор ЛС', 'Необходимо выбрать хотя бы один лицевой счет.');
                        return false;
                    }
                    return true;

                },
            },
        rule = operationRules[action] || operationRules['default'];

        if (rule(recs) === false) {
            return false;
        }

        rec = recs[0];

        switch (action) {
            case 'mergeaccountoperation':
                me.showMergeAccountWin(recs);
                break;
            case 'closeaccountoperation':
                me.showcloseAccountWin(recs);
                break;
            case 'setnewowneraccountoperation':
                me.showChangeOwnerWindow(rec);
                break;
            case 'setbalanceaccountoperation':
                me.showSetBalanceWindow(rec);
                break;
            case 'setpenaltyaccountoperation':
                me.showSetPenaltyWindow(rec);
                break;
            case 'cancelpaymentaccountoperation':
                me.showCancelPaymentWin(rec);
                break;
            case 'cancelchargeoperation':
                me.showCancelChargeWin(recs);
                break;
            case 'masscancelchargeoperation':
                me.showMassCancelChargeWin(recs, item.action);
                break;
            case 'penaltycanceloperation':
                me.showPenaltyCancelWin(recs, item.action);
                break;
            case 'reopenaccountoperation':
                this.getAspect('ReopenAccountAspect').showReOpenAccountWin(recs);
                break;
            case 'manuallyrecalcoperation':
                me.showManuallyRecalcWin(recs);
                break;
            case 'distributefundsforperformedworkoperation':
                me.showDistributeFundsForPerformedWorkWindow(recs);
                break;
            case 'correctpaymentsoperation':
                me.showCorrectPaymentsWin(recs);
                break;
            case 'masspersaccgroupoperation':
                me.showMassPersAccGroupOperationWindow(recs);
                break;
            case 'banrecalcoperation':
                me.showBanRecalcOperationWindow(recs);
                break;
            case 'masssaldochangeoperation':
                me.showMassSaldoChangeWindow(recs, item.action);
                break;
            case 'turnofflockfromcalculationoperation':
                me.processTurnOffLockFromCalculation(recs);
                break;
            case 'repaymentoperation':
                me.showRepaymentOperationWindow(recs);
                break;
            case 'personalaccountsplitoperation':
                this.getAspect('SplitAccountAspect').showMainWindow(recs[0].getId());
                break;
            case 'calcdebtoperation':
                me.showCalcDebtOperationWindow(recs);
                break;
            case 'setsignoperation':
                me.showSetSignWin(recs, item.action);
                break;
        }
    },

    showCancelChargeWin: function(recs) {
        var me = this,
            persAccGrid = me.getMainView(),
            period = persAccGrid.down('b4selectfield[name=ChargePeriod]'),
            cancelChargeWin,
            cancelChargeGrid,
            persAccIds = [];

        if (me.openPeriod && period.getText()
            && me.openPeriod === period.getText()) {
            Ext.Msg.alert('Внимание', 'Для отмены начислений необходимо выбрать закрытый период');
        } else {
            Ext.Array.each(recs, function(rec) {
                persAccIds.push(rec.getId());
            });

            cancelChargeWin = Ext.create('B4.view.regop.personal_account.action.CancelChargeWin', {
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            cancelChargeWin.down('b4selectfield').setValue(period.getValue());
            cancelChargeWin.down('b4selectfield').setRawValue(period.getText());
            cancelChargeWin.down('b4selectfield').on('change', function() {
                cancelChargeGrid.getStore().load();
            });

            cancelChargeGrid = cancelChargeWin.down('cancelchargegrid');
            cancelChargeGrid.getStore().on('beforeload', function(store, operation) {
                operation.params.periodId = cancelChargeWin.down('b4selectfield').getValue();
                operation.params.persAccIds = persAccIds;
            });
            cancelChargeGrid.getStore().load();

            cancelChargeWin.down('form').getForm().isValid();
            cancelChargeWin.show();
        }
    },

    showMassCancelChargeWin: function (recs, action) {
        var massCancelChargeWin,
            massCancelChargeGrid,
            persAccIds = [];

        Ext.Array.each(recs, function (rec) {
            persAccIds.push(rec.getId());
        });

        massCancelChargeWin = Ext.create('B4.view.regop.personal_account.action.MassCancelChargeWin', {
            accountOperationCode: action,
            renderTo: B4.getBody().getActiveTab().getEl()
        });

        massCancelChargeWin.down('b4selectfield').on('change', function () {
            massCancelChargeGrid.getStore().load();
        });

        massCancelChargeGrid = massCancelChargeWin.down('masscancelchargegrid');

        massCancelChargeWin.down('[name=ChargePeriod]').getStore().on('beforeload', function(store, operation) {
            operation.params.closedOnly = true;
        });
        massCancelChargeGrid.getStore().on('beforeload', function (store, operation) {
            var me = this,
                periodIds = massCancelChargeWin.down('b4selectfield[name=ChargePeriod]').getValue(),
                params = me.getFilterParams(),
                cancelParamsCbs = Ext.ComponentQuery.query('masscancelchargewin container[name=CheckboxContainer] checkbox');

            Ext.Array.each(cancelParamsCbs,
                function(element) {
                    operation.params[element.action] = element.checked;
                });

            operation.params.persAccIds = Ext.encode(persAccIds);
            operation.params.periodIds = Ext.encode(periodIds);
            operation.params.complexFilterGrid = params.complexFilter;

            delete params.complexFilter; // нужно, чтобы apply не затёр complexFilter
            Ext.apply(operation.params, params);
        }, this);

        massCancelChargeGrid.getStore().load();

        massCancelChargeWin.down('form').getForm().isValid();
        massCancelChargeWin.show();
    },

    showCorrectPaymentsWin: function(recs) {
        var me = this,
            window,
            persAccGrid = me.getMainView(),
            period = persAccGrid.down('b4selectfield[name=ChargePeriod]'),
            persAcc = recs[0];

        if (me.openPeriod && period.getText() && me.openPeriod !== period.getText()) {
            Ext.Msg.alert('Внимание', 'Для корректировки оплат необходимо выбрать открытый период');
        } else {
            window = Ext.create('B4.view.regop.personal_account.action.CorrectPaymentsWin', {
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            window.down('textfield[name=PersonalAccountNum]').setValue(persAcc.get('PersonalAccountNum'));
            window.down('textfield[name=PersonalAccountNum]').setRawValue(persAcc.get('PersonalAccountNum'));

            window.persAccId = persAcc.get('Id');
            window.down('b4grid').getStore().filter('persAccId', persAcc.get('Id'));
            window.down('form').getForm().isValid();
            window.show();
        }
    },

    showDistributeFundsForPerformedWorkWindow: function(recs) {
        var me = this,
            distributionWindow,
            distributionGrid,
            persAccIds = [],
            operationDate,
            cbDistributeForBaseTariff,
            cbDistributeForDecisionTariff;

        B4.Ajax.request({
            url: B4.Url.action('GetStartDateOfFirstPeriod', 'ChargePeriod')
        }).next(function(response) {
            if (!response) {
                Ext.Msg.alert('Ошибка', 'При получении даты начала первого периода произошла ошибка');
                return;
            }
            var startDate = Ext.decode(response.responseText);

            Ext.Array.each(recs, function (rec) {
                persAccIds.push(rec.getId());
            });

            distributionWindow = Ext.create('B4.view.regop.personal_account.action.DistributeFundsForPerformedWorkWindow', {
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            operationDate = distributionWindow.down('[name=OperationDate]');
            cbDistributeForBaseTariff = distributionWindow.down('[name=CheckDistributeForBaseTariff]');
            cbDistributeForDecisionTariff = distributionWindow.down('[name=CheckDistributeForDecisionTariff]');

            operationDate.minValue = new Date(startDate);

            if (Gkh.config.RegOperator.GeneralConfig.PerfWorkChargeConfig.PerfWorkChargeType == B4.enums.PerfWorkChargeType.ForExistingCharges) {
                operationDate.maxValue = new Date();
                cbDistributeForBaseTariff.hide();
                cbDistributeForDecisionTariff.hide();
            }

            distributionGrid = distributionWindow.down('distributefundsforperformedworkgrid');
            distributionGrid.params = {};
            distributionGrid.params.persAccIds = persAccIds;

            me.onDistributePerformedWork();

            var cellEdit = distributionGrid.getPlugin('cellEdit'),
                balanceFld = distributionWindow.down('[name=Balance]');

            cellEdit.on('beforeedit', function (e) {
                return !e.disabled;
            });

            cellEdit.on('edit', function (editor, args) {
                var rest = parseFloat(balanceFld.getValue()),
                    orig = parseFloat(args.originalValue),
                    value = parseFloat(args.value);

                rest = !isNaN(rest) ? rest : 0;
                orig = !isNaN(orig) ? orig : 0;
                value = !isNaN(value) ? value : 0;

                if (orig === value) {
                    return;
                }

                rest = rest + orig - value;

                balanceFld.setValue(rest.toFixed(2));

                editor.view.store.proxy.data[args.record.index].DistributionSum = value;

                // не работает, нужно только для того чтобы пометка о изменении записи пропала
                editor.view.store.sync();
            });

            distributionWindow.show();

        }).error(function () {
            Ext.Msg.alert('Ошибка', 'При получении даты начала первого периода произошла ошибка');
        });
    },

    showMassPersAccGroupOperationWindow: function(recs) {
        var me = this,
            window,
            countField,
            selectedCount = me.getMainView().getStore().totalCount,
            persAccIds = null;
            params = {};

            if (recs && recs.length > 0) {
                selectedCount = recs.length;
                persAccIds = [];
                Ext.Array.each(recs, function(rec) { persAccIds.push(rec.getId()); });
            }

        window = Ext.create('B4.view.regop.personal_account.action.PersAccGroup.MassOperationsPersAccGroupWindow', {
            renderTo: B4.getBody().getActiveTab().getEl(),
            params: params,
            accIds: persAccIds
        });

        countField = window.down('gkhintfield[name=SelectedPaCount]');
        if (countField) {
            countField.setValue(selectedCount);
        }

        window.show();
    },

    processMassPersAccGroupOperation: function(isInclude) {
        return function() {
            var me = this,
                grid = me.getCmpInContext('masspaccountaddgroupgrid'),
                form = grid.up(),
                sm = grid.getSelectionModel(),
                selected = Ext.Array.map(sm.getSelection(), function(el) { return el.get('Id'); }),
                maskText;

            var params = Ext.apply(me.getFilterParams() || {},{
                accIds: form.accIds,
                workGroupIds: selected,
                isIncludeOperation: isInclude,
                operationCode: 'MassPersAccGroupOperation'
            });

            params.crOwnerType = params.crOwnerTypeValues;

            if (isInclude) {
                maskText = 'Включение в группы';
            } else {
                maskText = 'Исключение из групп';
            }

            if (selected.length === 0) {
                Ext.Msg.alert('Ошибка при выполнении операции', 'Необходимо выбрать хотя бы одну грппу');
                return;
            }

            var count = form.down('gkhintfield[name=SelectedPaCount]').getValue();
            Ext.Msg.prompt({
                title: 'Группы лицевых счетов',
                msg: 'Выполнить операцию с группами по ' + count + ' лицевым счетам?',
                buttons: Ext.Msg.OKCANCEL,
                fn: function (btnId) {
                    if (btnId === "ok") {
                        me.mask(maskText, grid);

                        B4.Ajax.request({
                            url: B4.Url.action('ExecuteAccountOperation', 'BasePersonalAccount'),
                            method: 'POST',
                            params: params,
                            timeout: 999999
                        }).next(function (response) {
                            me.unmask();
                            form.close();
                            me.getMainView().getStore().load();
                        }).error(function (e) {
                            me.unmask();
                            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при выполнении операции!');
                            form.close();
                        });
                    }
                }
            });
        }
    },

    distributeFundsForperformedWorkGridRowAction: function (grid, action, record) {
        var me = this,
            store = grid.getStore(),
            balanceFld = grid.up().down('[name=Balance]');

        switch (action.toLowerCase()) {
            case 'delete':

                Ext.Array.each(store.proxy.data, function(proxyRecord) {
                    if (record.get('Id') === proxyRecord.Id) {

                        var rest = parseFloat(balanceFld.getValue()),
                        value = parseFloat(proxyRecord.DistributionSum);

                        rest = !isNaN(rest) ? rest : 0;
                        value = !isNaN(value) ? value : 0;

                        rest = rest + value;

                        balanceFld.setValue(rest.toFixed(2));

                        Ext.Array.remove(store.proxy.data, proxyRecord);
                        return false;
                    }
                }, me);

                Ext.Array.remove(grid.params.persAccIds, record.get('Id'));
                store.load();

                break;
        }
    },

    onDistributePerformedWork: function () {
        var me = this,
            distributionWindow = Ext.ComponentQuery.query('distributefundsforperformedworkwindow')[0],
            distributionGrid = distributionWindow.down('distributefundsforperformedworkgrid'),
            distributionTypeField = distributionWindow.down('[name=DistributionType]'),
            distributionSumField = distributionWindow.down('[name=DistributionSum]'),
            acceptButton = distributionWindow.down('[action=Accept]'),
            balanceFld = distributionWindow.down('[name=Balance]'),
            store = distributionGrid.getStore();

        var params = {
            persAccIds: distributionGrid.params.persAccIds,
            distributionType: distributionTypeField.getValue(),
            distributionSum: distributionSumField.getValue()
        };

        me.mask('Распределение', distributionWindow);

        B4.Ajax.request({
            url: B4.Url.action('GetAccountsInfoForPerformedWorkDistribution', 'BasePersonalAccount'),
            method: 'POST',
            params: params,
            timeout: 999999
        }).next(function(response) {
            var decoded = Ext.decode(response.responseText);
            if (!decoded.success) {
                me.unmask();
                Ext.Msg.alert('Ошибка', decoded.message);
                distributionWindow.close();
                return;
            }

            store.proxy.data = decoded.data;
            store.load();

            var balance = 0;

            if (distributionTypeField.getValue() == B4.enums.PerformedWorkFundsDistributionType.Manual) {
                balance = distributionSumField.getValue();
            }
            balanceFld.setValue(balance);

            acceptButton.enable();

            me.unmask();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при распределении!');
            distributionWindow.close();
        });
    },

    onDistributionSumChange: function (fld, newValue, oldValue) {

        var distributionWindow = Ext.ComponentQuery.query('distributefundsforperformedworkwindow')[0],
        distributionGrid = distributionWindow.down('distributefundsforperformedworkgrid'),
        store = distributionGrid.getStore();

        if (oldValue !== undefined) {
            store.proxy.data = [];
            store.removeAll();

            distributionWindow.down('button[action=Accept]').disable();
        }
    },

    onAcceptDistributionFundsForPerformedWork: function(btn) {
        var me = this,
            distributionWindow = btn.up('distributefundsforperformedworkwindow'),
            distributionGrid = distributionWindow.down('distributefundsforperformedworkgrid'),
            distributionSumField = distributionWindow.down('[name=DistributionSum]'),
            distributionBalanceField = distributionWindow.down('[name=Balance]'),
            cbDistributeForBaseTariff = distributionWindow.down('[name=CheckDistributeForBaseTariff]'),
            cbDistributeForDecisionTariff = distributionWindow.down('[name=CheckDistributeForDecisionTariff]'),
            store = distributionGrid.getStore(),
            records = [];

        if (distributionBalanceField.getValue() !== 0) {
            Ext.Msg.alert('Распределение!', 'Распределение проведено не полностью. Имеется значение в поле "Остаток". Распределение не может быть выполнено');
            return;
        }

        if (distributionSumField.getValue() === 0) {
            Ext.Msg.alert('Распределение!', 'Распределяемая сумма должна быть больше 0');
            return;
        }

        if (!distributionWindow.getForm().isValid()) {
            return;
        }

        var hasZeroSum = false;
        Ext.each(store.proxy.data, function (item) {
            if (item.DistributionSum === 0) {
                hasZeroSum = true;
                return false;
            }

            records.push(item);
        });

        if (hasZeroSum) {
            Ext.Msg.alert('Распределение!', 'Для одного из ЛС Сумма = 0. Укажите сумму или удалите ЛС');
            return;
        }

        me.mask('Распределение', distributionWindow);

        var params = {
            records: Ext.encode(records),
            distributeForBaseTariff: cbDistributeForBaseTariff.checked,
            distributeForDecisionTariff: cbDistributeForDecisionTariff.checked
        };

        distributionWindow.submit({
            url: B4.Url.action('ApplyPerformedWorkDistribution', 'PersonAccountOperation'),
            params: params,
            timeout: 999999,
            success: function(f, action) {
                var decoded = Ext.decode(action.response.responseText);
                if (!decoded.success) {
                    Ext.Msg.alert('Ошибка', decoded.message);
                } else {
                    Ext.Msg.alert('Результат', decoded.message || 'Распределение выполнено успешно!');
                }
                me.unmask();
                distributionWindow.close();
            },
            failure: function (f, action) {
                me.unmask();
                var decoded = Ext.decode(action.response.responseText);
                Ext.Msg.alert('Ошибка', decoded.message || 'Ошибка при распределении!');
                distributionWindow.close();
            }
        });
    },

    showManuallyRecalcWin: function (recs) {
        var me = this;
        B4.Ajax.request({
            url: B4.Url.action('GetStartDateOfFirstPeriod', 'ChargePeriod')
        }).next(function (response) {
            if (!response) {
                Ext.Msg.alert('Ошибка', 'При получении даты начала первого периода произошла ошибка');
                return;
            }
            var startDate = Ext.decode(response.responseText);

            var win = Ext.create('Ext.window.Window', {
                modal: true,
                bodyPadding: 5,
                bodyStyle: Gkh.bodyStyle,
                title: 'Ручной перерасчет',
                closeAction: 'hide',
                layout: 'fit',
                callback: null,
                items: [
                    {
                        xtype: 'form',
                        unstyled: true,
                        border: false,
                        layout: { type: 'vbox', align: 'stretch' },
                        defaults: {
                            labelWidth: 150
                        },
                        items: [
                            {
                                xtype: 'hidden',
                                name: 'accIds'
                            },
                            {
                                xtype: 'datefield',
                                fieldLabel: 'Дата начала перерасчета',
                                minValue: new Date(startDate),
                                name: 'recalcDateStart',
                                allowBlank: false,
                                margin: '2px'
                            },
                            {
                                xtype: 'textfield',
                                name: 'Reason',
                                fieldLabel: 'Причина',
                                allowBlank: false,
                                margin: '2px'
                            },
                            {
                                xtype: 'b4filefield',
                                name: 'Document',
                                fieldLabel: 'Документ-основание',
                                margin: '2px'
                            }
                        ]
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        docked: 'top',
                        items: [
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4savebutton',
                                        text: 'Подтвердить',
                                        handler: function (b) {
                                            var w = b.up('window');

                                            if (w.down('form').getForm().isValid()) {
                                                w.close();
                                                if (w.callback) {
                                                    w.callback(w.down('datefield[name="recalcDateStart"]').getValue());
                                                }
                                            }
                                        }
                                    }
                                ]
                            },
                            '->',
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4closebutton',
                                        text: 'Отменить',
                                        handler: function (b) {
                                            b.up('window').close();
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });

            win.callback = function () {
                var accId = win.down('[name="accIds"]'),
                    recIds = [],
                    form = win.down('form');

                Ext.each(recs, function (rec) { recIds.push(rec.get('Id')); });


                accId.setValue(recIds);

                me.mask('Ожидание...');

                form.submit({
                    url: B4.Url.action('ManuallyRecalc', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
                    success: function (f, action) {
                        var resp = Ext.decode(action.response.responseText);
                        Ext.Msg.alert('Результат', resp.message || 'Выполнено успешно');
                        me.getMainView().getStore().load();
                        me.unmask();
                    },
                    failure: function (_, response) {
                        var resp = Ext.decode(response.response.responseText);

                        Ext.Msg.alert('Ошибка', resp.message);

                        me.unmask();
                    }
                });
            };

            me.closeAccountWin && me.closeAccountWin.destroy();

            me.closeAccountWin = win;

            win.show();

            me.unmask();
        }).error(function () {
            Ext.Msg.alert('Ошибка', 'При получении даты начала первого периода произошла ошибка');
        });
    },

    showRepaymentOperationWindow: function(recs) {
        var accountIds = [],
            win = Ext.widget('repaymentwindow'),
            paymentStore = win.down('[name=PaymentGrid]').getStore(),
            dateStartF = win.down('[name=DateStart]'),
            dateEndF = win.down('[name=DateEnd]');
        
        Ext.each(recs, function(rec) {
            accountIds.push(rec.getId());
        });

        win.accountIds = accountIds;

        dateStartF.on('change', function() {
            paymentStore.load();
        });
        dateEndF.on('change', function() {
            paymentStore.load();
        });

        win.show();
        win.down('[name=EnrollGrid]').getStore().load();
    },

    onApplyRepaymentOperation: function(btn) {
        var me = this,
            win = btn.up('window'),
            form = win.getForm().getForm(),
            selection = win.down('[name=PaymentGrid]').getSelectionModel().getSelection() || [],
            targetAccountSel = win.down('[name=EnrollGrid]').getSelectionModel().getSelection(),
            targetAccount = targetAccountSel && targetAccountSel[0],
            targetAccountId,
            roPayAccountNum,
            sourceAccount = form.getRecord(),
            filter,
            params,
            transferIds = [],
            sourceAccountIds = [];

        if (!form.isValid()) {
            Ext.Msg.alert('Ошибка', 'Не заполнены обязательные поля');
            return;
        }

        if (!selection || !selection.length) {
            Ext.Msg.alert('Ошибка', 'Необходимо выбрать сумму оплаты');
            return;
        }

        if (!targetAccount) {
            Ext.Msg.alert('Ошибка', 'Необходимо выбрать лицевой счет для зачисления оплаты');
            return;
        }

        targetAccountId = targetAccount.get('Id');

        filter = selection.filter(function(rec) {
                return rec.get('Id') === targetAccountId;
            }).length >
            0;

        if (filter) {
            Ext.Msg.alert('Ошибка', 'Лицевой счет списания совпадает с лицевым счетом списания');
            return;
        }

        Ext.each(selection,
            function(rec) {
                var val;

                sourceAccountIds.push(rec.get('AccountId'));

                if (val = rec.get('BaseTariffTransferId')) {
                    transferIds.push(val);
                }

                if (val = rec.get('DecisionTariffTransferId')) {
                    transferIds.push(val);
                }

                if (val = rec.get('PenaltyTransferId')) {
                    transferIds.push(val);
                }
            });

        params = {
            operationCode: win.accountOperationCode,
            sourceAccountIds: Ext.JSON.encode(sourceAccountIds),
            targetAccountId: targetAccount.get('Id'),
            reason: win.down('[name=Reason]').getValue(),
            transferIds: Ext.encode(transferIds)
        };

        roPayAccountNum = targetAccount.get('RoPayAccountNum');

        var filterAccountNum = selection.filter(function(rec) {
            return rec.get('RoPayAccountNum') === roPayAccountNum;
            }).length > 0;

        if (!filterAccountNum) {
            Ext.Msg.confirm('Внимание',
                'Выявлено несоответствие Р/С по выбранным лицевым счетам',
                function(result) {
                    if (result === 'yes') {
                        me.onContinueRepaymentOperation(win, form, params);
                    }
                });
        } else {
            me.onContinueRepaymentOperation(win, form, params);
        }
    },
        
    onContinueRepaymentOperation: function(win, form, params) {
        var me = this;

        me.mask('Перераспределение оплат...', win);

        form.submit({
            url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
            params: params,
            timeout: 1000 * 60 * 10, // 10 минут
            success: function () {
                me.unmask();
                Ext.Msg.alert('Успешно', 'Перераспределение оплат выполнено успешно');
                win.clearListeners();
                win.close();
            },
            failure: function (f, action) {
                me.unmask();
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Ошибка', json.message);
                win.clearListeners();
                win.close();
            }
        });
    },

    showBanRecalcOperationWindow: function(recs) {
        var me = this,
            banRecalcWin,
            banRecalcGrid,
            store,
            persAccIds = [],
            dateStart,
            responseData;

        Ext.Array.each(recs, function(rec) {
            persAccIds.push(rec.getId());
        });

        B4.Ajax.request({
            url: B4.Url.action('GetStartDateOfFirstPeriod', 'ChargePeriod'),
                method: 'POST'
            })
            .next(function (response) {
                var minDate,
                    responseData = Ext.decode(response.responseText);

                if (!responseData) {
                    Ext.Msg.alert('Ошибка', action.response.message || 'Ошибка при получении данных');
                    return;
                }
                
                minDate = new Date(responseData);
                banRecalcWin = Ext.create('B4.view.regop.personal_account.action.BanRecalcWin', {
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    minDate: minDate
                });

                banRecalcGrid = banRecalcWin.down('banrecalcgrid');
                store = banRecalcGrid.getStore();

                store.on('beforeload', function (store, operation) {
                    var params = me.getFilterParams();

                    operation.params.persAccIds = Ext.encode(persAccIds);
                    Ext.apply(operation.params, params);
                }, this);

                store.load();

                banRecalcWin.down('form').getForm().isValid();
                banRecalcWin.show();
            });
    },

    cancelCharge: function (btn) {
        var me = this,
            win = btn.up('window'),
            form = win.down('form'),
            grid = win.down('cancelchargegrid'),
            selField = win.down('b4selectfield'),
            store = grid.getStore(),
            array = [],
            params = form.getValues();

        store.each(function (rec) {
            var record = {
                Id: rec.get('Id'),
                CancellationSum: rec.get('CancellationSum')
            };
            array.push(record);
        });

        params.modifRecs = Ext.encode(array);
        params.chargePeriodId = selField.getValue();

        if (form.getForm().isValid()) {

            me.mask('Отмена начислений...', win);

            form.submit({
                url: B4.Url.action('CancelCharges', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
                params: params,
                timeout: 9999999,
                success: function () {
                    me.unmask();
                    Ext.Msg.alert('Успешно', 'Отмена начислений выполнена успешно');
                    win.close();
                },
                failure: function (f, action) {
                    me.unmask();
                    var json = Ext.JSON.decode(action.response.responseText);
                    Ext.Msg.alert('Ошибка', json.message);
                    win.close();
                }
            });
        } else {
            Ext.Msg.alert('Ошибка', 'Не заполнены обязательные поля');
        }
    },

    massCancelCharge: function (btn) {
        var me = this,
            win = btn.up('window'),
            form = win.down('form'),
            grid = win.down('masscancelchargegrid'),
            store = grid.getStore(),
            cancelParamsCbs = Ext.ComponentQuery.query('masscancelchargewin container[name=CheckboxContainer] checkbox'),
            params = store.lastOptions.params,
            hasAnyCancelParams = false;

        Ext.Array.each(cancelParamsCbs, function(element) { hasAnyCancelParams |= element.checked; });

        if (!hasAnyCancelParams) {
            Ext.Msg.alert('Ошибка', 'Выберите хотя бы один тип отмены');
            return;
        }

        // при подтверждении отмены фильтры грида окна не должны влиять ан выборку
        delete params.complexFilter;
        Ext.apply(params, form.getValues());

        if (form.getForm().isValid()) {

            me.mask('Отмена начислений и корректировок за периоды...', win);

            form.submit({
                url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
                params: params,
                timeout: 9999999,
                success: function () {
                    me.unmask();
                    Ext.Msg.alert('Успешно', 'Отмена начислений и корректировок выполнена успешно');
                    win.close();
                },
                failure: function (f, action) {
                    me.unmask();
                    var json = Ext.JSON.decode(action.response.responseText);
                    Ext.Msg.alert('Ошибка', json.message);
                    win.close();
                }
            });
        } else {
            Ext.Msg.alert('Ошибка', 'Не заполнены обязательные поля');
        }
    },

    showCancelPaymentWin: function (rec) {
        var me = this,
            id = rec.getId(),
            grid = me.getMainView(),
            periodId = grid.down('b4selectfield[name=ChargePeriod]').getValue(),
            win;

        me.mask(null, B4.getBody().getActiveTab());

        B4.Ajax.request({
            method: 'POST',
            url: B4.Url.action('GetAccountIncomeInPeriod', 'BasePersonalAccount'),
            timeout: 9999999,
            params: {
                periodId: periodId,
                accountId: id,
                validateOpenPeriod: true
            }
        }).next(function (r) {
            var obj = Ext.decode(r.responseText),
                paymentSum = 0;

            me.unmask();

            if (obj) {
                paymentSum = obj.Sum;
            }

            win = Ext.create('B4.view.regop.personal_account.action.CancelPaymentWin', {
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            win.down('[name=AvailableSum]').setValue(paymentSum);
            win.down('[name=Account]').setValue(id);
            win.down('form').getForm().isValid();
            win.show();

        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || 'Ошибка при получении доступной суммы', 'error');
        });
    },

    showSetPenaltyWindow: function (rec) {
        var me = this,
            win = Ext.create('B4.view.regop.personal_account.action.SetPenalty'),
            id = rec.getId(),
            model = me.getModel('regop.personal_account.BasePersonalAccount');

        win.down('form').getForm().isValid();

        model.load(id, {
            success: function (account) {
                win.down('[name=DebtPenalty]').setValue(account.get('DebtPenalty'));
            },
            failure: function () { }
        });

        win.down('[name=AccountId]').setValue(id);
        win.show();
    },

    showSetBalanceWindow: function (rec) {
        var me = this,
            id = rec.getId(),
            model = me.getModel('regop.personal_account.BasePersonalAccount'),
            activeTab = B4.getBody().getActiveTab(),
            winConfig = {
                constrain: true,
                renderTo: activeTab.getEl(),
                closeAction: 'destroy',
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : ''
            };

        me.mask('Получение информации...', activeTab);

        B4.Ajax.request({
            url: B4.Url.action('GetAccountSummaryInfoInCurrentPeriod', 'PersonalAccountPeriodSummary'),
            params: {
                accountId: id
            }
        }).next(function (resp) {
            var json = Ext.JSON.decode(resp.responseText),
                debt = json.data.SaldoOut.toFixed(2),
                penalty = json.data.Penalty.toFixed(2),
                win = Ext.create('B4.view.regop.personal_account.action.SetBalance', winConfig);

            me.unmask();
            win.down('[name=CurrentSaldo]').setValue(debt - penalty);
            win.down('[name=AccountId]').setValue(id);
            win.down('form').getForm().isValid();
            win.show();
        }).error(function () {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', 'Не удалось получить состояние лицевого счета', 'error');
        });

        //model.load(id, {
        //    success: function (account) {
        //        var debt = account.get('ChargeBalance').toFixed(2),
        //            win = Ext.create('B4.view.regop.personal_account.action.SetBalance', winConfig);

        //        me.unmask();

        //        win.down('[name=CurrentSaldo]').setValue(debt);
        //        win.down('[name=AccountId]').setValue(id);
        //        win.down('form').getForm().isValid();
        //        win.show();
        //    },
        //    failure: function () {
        //        me.unmask();
        //        B4.QuickMsg.msg('Ошибка', 'Не удалось получить состояние лицевого счета', 'error');
        //    }
        //});
    },

    //#region Отмена начисления пени
    showPenaltyCancelWin: function (recs, action) {
        var me = this,
            win,
            ids = [],
            accountsGrid,
            store,
            period = me.getMainView().down('b4selectfield[name=ChargePeriod]');

        Ext.Array.each(recs, function (rec) {
            ids.push(rec.getId());
        });

        win = Ext.widget('cancelpenaltywin', {
            accountOperationCode: action,
            renderTo: B4.getBody().getActiveTab().getEl()
        });

        accountsGrid = win.down('b4grid');
        store = accountsGrid.getStore();

        var combo = win.down('b4combobox[name="ChargePeriod"]');

        combo.setValue(period.getValue());

        store.on('beforeload', function (store, operation) {
            var periodId = combo.getValue();

            operation.params.periodId = periodId || period.getValue();
            operation.params.ids = ids;
        });
        store.load();

        combo.on('change', function () {
            store.load();
        });

        if (me.openPeriod && period.getText()
            && me.openPeriod === period.getText()) {
            Ext.Msg.alert('Внимание', 'Для отмены начислений необходимо выбрать закрытый период');
        } else {
            win.show();
        }
    },

    correctPayments: function(btn) {
        var me = this,
            win = btn.up('window'),
            form = win.down('form'),
            formData = form.getForm(),
            grid = win.down('b4grid'),
            store = grid.getStore(),
            array = [],
            sumFunc,
            takeSum,
            enrollSum;
        
        if (!formData.isValid()) {
            var invalidFields = '',
                fields = formData.getFields();
            Ext.each(fields.items, function(field) {
                if (!field.isValid()) {
                    invalidFields += '<br>' + field.fieldLabel;
                }
            });

            Ext.Msg.alert('Ошибка заполнения полей!', 'Не заполнены обязательные поля: ' + invalidFields);
            return;
        }

        var params = {
            persAccId: win.persAccId,
            DocumentNumber: win.down('textfield[name=DocumentNumber]').getValue(),
            Reason: win.down('textfield[name=Reason]').getValue(),
            DocumentDate: win.down('datefield[name=DocumentDate]').getValue(),
            OperationDate: win.down('datefield[name=OperationDate]').getValue(),
            Document: win.down('[name=Document]').getValue()
        };

        if (params.OperationDate.setHours(0, 0, 0, 0) > new Date().setHours(0, 0, 0, 0)) {
            Ext.Msg.alert('Ошибка', 'Дата операции не может быть больше текущей даты!');
            return;
        }

        sumFunc = function(index) {
            return function(previousValue, currentValue) {
                var prevValue =
                    Number.isFinite(previousValue)
                        ? previousValue
                        : Number.isFinite(previousValue[index])
                            ? previousValue[index]
                            : 0;

                return prevValue + (Number.isFinite(currentValue[index]) ? currentValue[index] : 0);
            }
        };

        store.each(function(rec) {
            array.push({
                PaymentType: rec.get('PaymentType'),
                TakeAmount: rec.get('TakeAmount'),
                EnrollAmount: rec.get('EnrollAmount')
            });
        });

        takeSum = array.reduce(sumFunc('TakeAmount'));
        enrollSum = array.reduce(sumFunc('EnrollAmount'));

        if (takeSum === 0 || enrollSum === 0) {
            Ext.Msg.alert('Ошибка', 'Необходимо заполнить значения для корректировки оплат!');
            return;
        }

        if (takeSum !== enrollSum) {
            Ext.Msg.alert('Ошибка', 'Сумма снятия не равна сумме зачисления!');
            return;
        }

        params.Payments = Ext.JSON.encode(array);
        params.operationCode = grid.accountOperationCode;

        me.mask('Корректировка начислений...', win.getEl());

        form.submit({
            url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
            params: params,
            timeout: 9999999,
            success: function(d, response) {
                me.unmask();
                if (response.result.success) {
                    Ext.Msg.alert('Успешно', 'Корректировка оплат выполнена успешно');
                } else {
                    Ext.Msg.alert('Успешно', response.result.message);
                }

                win.close();
            },
            failure: function(f, action) {
                me.unmask();
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Ошибка', json.message);
                win.close();
            }
        });
    },

    cancelPenalty: function (btn) {
        var me = this,
            win = btn.up('window'),
            form = win.down('form'),
            grid = win.down('b4grid'),
            store = grid.getStore(),
            array = [],
            params = {};

        store.each(function (rec) {
            array.push({ Id: rec.get('Id'), CancellationSum: rec.get('CancellationSum') });
        });

        params.records = Ext.JSON.encode(array);
        params.operationCode = win.accountOperationCode;
        params.periodId = win.down('b4combobox[name="ChargePeriod"]').getValue();

        me.mask('Отмена начислений пени...', win.getEl());

        form.submit({
            url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
            params: params,
            timeout: 9999999,
            success: function () {
                me.unmask();
                Ext.Msg.alert('Успешно', 'Отмена начислений пени выполнена успешно');
                win.close();
            },
            failure: function (f, action) {
                me.unmask();
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Ошибка', json.message);
                win.close();
            }
        });
    },
    //#endregion

    setBalance: function (btn) {
        var me = this,
            win = btn.up('setbalancewin');

        me.mask('Изменение основной задолженности...', win.getEl());
        win.down('form').submit({
            url: B4.Url.action('SetBalance', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
            params: {
                fakeParam: 1
            },
            success: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText),
                    grid = me.getMainView();
                me.unmask();	
                Ext.Msg.alert('Результат', 'Выполнено успешно');
                win.close();
                if (grid) {
                    grid.getStore().load();
                }
            },
            failure: function (f, action) {
                me.unmask();
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Результат', json.message);
                win.close();
            }
        });
    },

    setPenalty: function (btn) {
        var me = this,
            win = btn.up('setpenaltywin'),
            newPenalty = win.down('[name=NewPenalty]').getValue(),
            debtPenalty = win.down('[name=DebtPenalty]').getValue(),
            reason = win.down('[name=Reason]').getValue(),
            form = win.down('form');

        if (Ext.isEmpty(newPenalty) || Ext.isEmpty(debtPenalty) || Ext.isEmpty(reason)) {
            B4.QuickMsg.msg('Предупреждение', 'Не заполнены обязательные поля', 'warning');
            return;
        }

        form.submit({
            url: B4.Url.action('SetPenalty', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
            success: function () {
                var grid = me.getMainView();
                Ext.Msg.alert('Результат', 'Выполнено успешно');
                win.close();
                if (grid) {
                    grid.getStore().load();
                }
            },
            failure: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Результат', json.message);
                win.close();
            }
        });
    },

	showChangeOwnerWindow: function (rec) {
		
        var me = this,
            win = Ext.create('B4.view.regop.personal_account.action.ChangeOwner'),
            roomModel = me.getModel('Room'),
            roomId = rec.get('RoomId');

        win.down('[name=CurrentOwner]').setValue(rec.get('AccountOwner'));
        win.down('[name=AccountId]').setValue(rec.getId());

        if (roomModel && roomId) {
            roomModel.load(roomId, {
                success: function (roomRecord) {
                    var ownershipType = B4.enums.RoomOwnershipType.displayRenderer(roomRecord.get('OwnershipType'));
                    win.down('[name=CurrentOwnershipType]').setValue(ownershipType);
                },
                scope: me
            });
        }

        win.show();
    },

	changeOwner: function (btn) {
		
        var me = this,
            win = btn.up('changeownerwin'),
            newOwnerId = win.down('[name=NewOwner]'),
            actualFrom = win.down('[name="ActualFrom"]'),
            form = win.down('form');

        if (!newOwnerId.validate() || !actualFrom.validate()) {
            Ext.Msg.alert('Внимание', 'Не заполнены обязательные поля!');
            return false;
        }

        me.mask('Смена владельца счета...');

        form.submit({
            url: B4.Url.action('SetNewOwner', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
            success: function () {
                me.getMainView().getStore().load();
                me.unmask();
                win.close();
                var obj = Ext.JSON.decode(this.response.responseText);

                if (obj.data.data) {
                    Ext.Msg.alert('Предупреждение', 'Выполнено успешно! Необходимо скорректировать сведения о документах собственности абонентов в карточке ЛС');
                } else {
                    Ext.Msg.alert('Результат', 'Выполнено успешно');
                }
            },
            failure: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Результат', json.message);
                me.unmask();
            }
        });
    },

    showMergeAccountWin: function (recs) {
        var errors = this.validateBeforeMerging(recs);

        if (errors.length > 0) {
            Ext.Msg.alert('Ошибка!', errors.join('</br>'));
            return;
        }

        var win = Ext.create('B4.view.regop.personal_account.action.MergeAccount'),
            store = win.down('grid').getStore();

        Ext.Array.each(recs, function (el) {
            if (!el.get('State').FinalState) {
                store.add({
                    PersonalAccountNum: el.get('PersonalAccountNum'),
                    AreaShare: el.get('AreaShare'),
                    AccountId: el.get('Id'),
                    RoomId: el.get('RoomId'),
                    NewShare: el.get('AreaShare')
                });
            }
        });

        win.show();
    },

    mergeAccounts: function (btn) {
        var me = this,
            grid = btn.up('mergeaccountwin').down('[name="mergegrid"]'),
            recs = grid.getStore().getRange(),
            errors = me.validateApplyMerge(grid),
            win = btn.up('mergeaccountwin'),
            accs,
            mergeInfos = win.down('[name="MergeInfos"]'),
            form = win.down('form');;

        if (errors.length > 0) {
            Ext.Msg.alert('Ошибка!', errors.join('<br>'));
            return;
        }

        accs = Ext.Array.map(recs, function (rec) {
            return {
                BasePersonalAccountId: rec.get('AccountId'),
                NewShare: rec.get('NewShare'),
                Modified: rec.get('AreaShare') !== rec.get('NewShare')
            };
        });

        mergeInfos.setValue(Ext.JSON.encode(accs));

        me.mask('Слияние счетов...', win);
        
        form.submit({
            url: B4.Url.action('MergeAccounts', 'PersonAccountOperation', { 'b4_pseudo_xhr': true }),
            success: function (_, response) {
                var resp = Ext.decode(response.response.responseText);
                if (resp.success) {
                    Ext.Msg.alert('Результат', 'Выполнено успешно');
                    me.getMainView().getStore().load();
                    me.unmask();
                    win.close();
                } else {
                    me.unmask();
                    Ext.Msg.alert('Результат', resp.message);
                }
            },
            failure: function (_, response) {
                var resp = Ext.decode(response.response.responseText);
                me.unmask();
                Ext.Msg.alert('Результат', resp.message);
                win.close();
            }
        });
    },

    banRecalc: function (btn) {
        var me = this,
            win = btn.up('banrecalcwin'),
            form = win.down('form'),
            dateStart = win.down('b4monthpicker[name=DateStart]').getValue(),
            dateEnd = win.down('b4monthpicker[name=DateEnd]').getValue(),
            isCharge = win.down('checkbox[name=IsCharge]').getValue(),
            isPenalty = win.down('checkbox[name=IsPenalty]').getValue(),
            persAccIds = [];

        if (dateEnd < dateStart) {
            Ext.Msg.alert('Внимание', 'Период окончания действия не может быть меньше периода начала действия');
            return false;
        }

        if (!(isCharge || isPenalty)) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать хотя бы один тип операции');
            return false;
        }

        if (form.getForm().isValid()) {
        var params = {
            File: win.down('b4filefield[name=File]').getValue(),
            Reason: win.down('textarea[name=Reason]').getValue(),
            DateStart: new Date(dateStart.getFullYear(), dateStart.getMonth(), 1),
            DateEnd: new Date(dateEnd.getFullYear(), dateEnd.getMonth(), 1),
            IsCharge: isCharge,
            IsPenalty: isPenalty
        };

            Ext.apply(params, me.getFilterParams());

            me.getMainView().getSelectionModel().getSelection().forEach(function(element) {
                persAccIds.push(element.getId());
            });

            params.persAccIds = Ext.encode(persAccIds);
            params.operationCode = win.accountOperationCode;

            me.mask('Запрет перерасчета...', win.getEl());

            form.submit({
                url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
                params: params,
                timeout: 9999999,
                success: function() {
                    me.unmask();
                    Ext.Msg.alert('Успешно', 'Запрет перерасчета выполнен успешно');
                    win.close();
                },
                failure: function(f, action) {
                    me.unmask();
                    var json = Ext.JSON.decode(action.response.responseText);
                    Ext.Msg.alert('Ошибка', json.message);
                    win.close();
                }
            });
        } else {
            Ext.Msg.alert('Внимание', 'Не заполнены обязательные поля!');
            return false;
        }
    },

    showcloseAccountWin: function (rec) {
        var me = this,
            win = Ext.create('Ext.window.Window', {
                modal: true,
                bodyPadding: 5,
                bodyStyle: Gkh.bodyStyle,
                title: 'Закрытие счета',
                closeAction: 'hide',
                layout: 'fit',
                callback: null,
                items: [
                    {
                        xtype: 'form',
                        unstyled: true,
                        border: false,
                        layout: { type: 'vbox', align: 'stretch' },
                        width: 400,
                        defaults: {
                            labelWidth: 150
                        },
                        items: [
                            {
                                xtype: 'textfield',
                                name: 'Reason',
                                fieldLabel: 'Причина',
                                allowBlank: true
                            },
                            {
                                xtype: 'b4filefield',
                                name: 'Document',
                                fieldLabel: 'Документ-основание'
                            },
                            {
                                xtype: 'datefield',
                                fieldLabel: 'Дата закрытия',
                                name: 'closeDate',
                                allowBlank: false,
                                maxValue: new Date()
                            },
                            {
                                xtype: 'b4combobox',
                                fieldLabel: 'Статус',
                                dataIndex: 'Status',
                                editable: false,
                                name: 'status',
                                url: '/PersonAccountOperation/GetCloseStates',
                                storeAutoLoad: true,
                                operand: CondExpr.operands.eq,
                                listeners: {
                                    storeloaded: function(field) {
                                        field.setValue(0);
                                    }
                                }
                            }
                        ]
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        docked: 'top',
                        items: [
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4savebutton',
                                        text: 'Подтвердить',
                                        handler: function (button) {
                                            me.settingStatus(button);
                                                }
                                            }
                                ]
                            },
                            '->',
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4closebutton',
                                        text: 'Отменить',
                                        handler: function (b) {
                                            b.up('window').close();
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });

        me.closeAccountWin && me.closeAccountWin.destroy();

        me.closeAccountWin = win;

        win.show();
    },

    showSetSignWin: function (recs) {
        
        var me = this,
            accountIds = [],
            totalCount = 0;
        Ext.Array.each(recs, function (rec) {
            accountIds.push(rec.getId());
        });

        setSignsWin = Ext.create('B4.view.regop.personal_account.action.SetSignsWin', {
            renderTo: B4.getBody().getActiveTab().getEl(),
            accIds: accountIds
        });
        setSignsWin.down('numberfield[name=CountLS]').setValue(accountIds.length);
        setSignsWin.down('form').getForm().isValid();
        setSignsWin.show();
    },

    promptForActualChangeDate: function (callback) {
        var me = this,
            win = me.datePrompWin || Ext.create('Ext.window.Window', {
                modal: true,
                title: 'Фактическая дата изменения долей',
                closeAction: 'hide',
                width: 350,
                height: 100,
                layout: 'fit',
                items: [
                    {
                        xtype: 'form',
                        items: [
                            {
                                xtype: 'datefield',
                                padding: 5,
                                fieldLabel: 'Фактическая дата',
                                labelWidth: 150,
                                name: 'date',
                                allowBlank: false
                            }
                        ]
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        docked: 'top',
                        items: [
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4savebutton',
                                        handler: function (b) {
                                            var w = b.up('window');
                                            if (w.down('form').getForm().isValid()) {
                                                b.up('window').close();
                                                callback.call(me, w.down('[name="date"]').getValue());
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'tbfill'
                                    },
                                    {
                                        xtype: 'b4closebutton',
                                        handler: function (b) {
                                            b.up('window').close();
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });

        me.datePrompWin = win;

        win.show();
    },

    /*
    * Валидация перед слиянием
    */
    validateBeforeMerging: function (recs) {
        var errors = [],
            zeroAreaAccs = '',
            rooms = Ext.Array.unique(Ext.Array.map(recs, function (el) { return el.get('RoomId'); }));

        if (rooms.length > 1) {
            errors.push('Необходимо выбрать лицевые счета одной квартиры');
        }

        Ext.each(recs, function (rec) {
            if (rec.get('AreaShare') === 0) {
                if (zeroAreaAccs.length > 0) {
                    zeroAreaAccs += ', ';
                }
                zeroAreaAccs += rec.get('PersonalAccountNum');
            }
        });

        if (zeroAreaAccs.length > 0) {
            if (zeroAreaAccs.indexOf(',') === -1) {
                errors.push('У лицевого счета ' + zeroAreaAccs + ' доля собственности равна 0. Для него слияние невозможно');
            } else {
                errors.push('У лицевых счетов ' + zeroAreaAccs + ' доля собственности равна 0. Для них слияние невозможно');
            }

        }

        return errors;
    },

    /*
    * Валидация перед применением слиянием
    */
    validateApplyMerge: function (grid) {
        var recs = grid.getStore().getRange(),
            errors = [],
            accumulator,
            anyWithZeroShare = Ext.Array.filter(recs, function (rec) {
                return rec.get('NewShare') == 0;
            }).length > 0;

        if (!anyWithZeroShare) {
            errors.push('При слиянии лицевых счетов, должен быть лицевой счет с нулевой долей собственности');
        }

        accumulator = 0;
        Ext.Array.each(recs, function (rec) {
            accumulator += rec.get('NewShare');
        });

        if (accumulator > 1) {
            errors.push('Сумма значений новых долей выбранных ЛС не может превышать 1 (единицу)');
        }

        if (Ext.Array.some(recs, function (r) { return r.get('NewShare') > 0 && r.get('AreaShare') > r.get('NewShare'); })) {
            errors.push('Доля собственности не может уменьшится.');
        }

        return errors;
    },

    cancelPayment: function (btn) {
        var me = this,
            win = btn.up('window'),
            form = win.down('form');

        if (form.down('[name=Sum]').getValue() == 0) {
            B4.QuickMsg.msg('Предупреждение', 'Сумма списания должна быть больше 0', 'warning');
            return;
        }

        me.mask('Отмена оплаты...', B4.getBody().getActiveTab().getEl());

        form.submit({
            url: B4.Url.action('CancelPayment', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
            success: function () {
                me.unmask();
                Ext.Msg.alert('Успешно', 'Отмена оплаты выполнена успешно');
                win.close();
            },
            failure: function (f, action) {
                me.unmask();
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Ошибка', json.message);
                win.close();
            }
        });
    },

    // Выгрузка информации по ЛС
    exportPersonalAccounts: function (button) {
        var me = this,
            win = Ext.create('Ext.window.Window', {
                modal: true,
                bodyPadding: 5,
                bodyStyle: Gkh.bodyStyle,
                title: 'Выгрузка информации по ЛС',
                closeAction: 'hide',
                layout: 'fit',
                callback: null,
                items: [
                    {
                        xtype: 'form',
                        unstyled: true,
                        border: false,
                        layout: { type: 'vbox', align: 'stretch' },
                        defaults: {
                            labelWidth: 150
                        },
                        items: [
                            {
                                xtype: 'b4selectfield',
                                store: 'B4.store.regop.ChargePeriod',
                                windowCfg: { modal: true },
                                textProperty: 'Name',
                                labelWidth: 175,
                                width: 500,
                                allowBlank: false,
                                labelAlign: 'right',
                                fieldLabel: 'Период',
                                editable: false,
                                columns: [
                                    {
                                        text: 'Наименование',
                                        dataIndex: 'Name',
                                        flex: 1,
                                        filter: { xtype: 'textfield' }
                                    },
                                    {
                                        text: 'Дата открытия',
                                        xtype: 'datecolumn',
                                        format: 'd.m.Y',
                                        dataIndex: 'StartDate',
                                        flex: 1,
                                        filter: { xtype: 'datefield' }
                                    },
                                    {
                                        text: 'Дата закрытия',
                                        xtype: 'datecolumn',
                                        format: 'd.m.Y',
                                        dataIndex: 'EndDate',
                                        flex: 1,
                                        filter: { xtype: 'datefield' }
                                    },
                                    {
                                        text: 'Состояние',
                                        dataIndex: 'IsClosed',
                                        flex: 1,
                                        renderer: function (value) {
                                            return value ? 'Закрыт' : 'Открыт';
                                        }
                                    }
                                ],
                                name: 'ChargePeriod'
                            },
                            {
                                xtype: 'b4selectfield',
                                store: 'B4.store.dict.Municipality',
                                selectionMode: 'MULTI',
                                windowCfg: { modal: true },
                                textProperty: 'Name',
                                labelWidth: 175,
                                width: 500,
                                allowBlank: false,
                                labelAlign: 'right',
                                fieldLabel: 'Муниципальное образование:',
                                editable: false,
                                columns: [
                                    {
                                        dataIndex: 'Name',
                                        flex: 1
                                    }
                                ],
                                name: 'Municipality'
                            }
                        ]
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        docked: 'top',
                        items: [
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4savebutton',
                                        text: 'Подтвердить',
                                        handler: function (b) {
                                            var w = b.up('window');

                                            if (w.down('form').getForm().isValid()) {
                                                w.close();
                                                if (w.callback) {

                                                    Ext.Msg.confirm('Внимание', Ext.String.format('Сформировать выгрузку с информацией по лицевым счетам по району "{0}" за период "{1}"?  Процесс может занять некоторое время.',
                                                        w.down('[name="Municipality"]').getRawValue(),
                                                        w.down('[name="ChargePeriod"]').getRawValue()
                                                        ), function (result) {
                                                            if (result === 'yes') {
                                                                w.callback(w.down('[name="ChargePeriod"]').getValue(), w.down('[name="Municipality"]'));
                                                            }
                                                        });

                                                }
                                            }
                                        }
                                    }
                                ]
                            },
                            '->',
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4closebutton',
                                        text: 'Отменить',
                                        handler: function (b) {
                                            b.up('window').close();
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });

        win.callback = function (periodId, muObject) {
            var muIds = [],
                munArr = muObject.getValue(),
                params = {},
                form = win.down('form');

            me.mask('Обработка...', me.getMainView());

            Ext.Array.each(munArr, function (rec) {
                muIds.push(rec);
            });

            Ext.apply(params, {
                chargePeriodId: periodId,
                municipalityIds: Ext.encode(muIds),
                municipalityName: muObject.getRawValue()
            });

            form.submit({
                url: B4.Url.action('ExportPersonalAccounts', 'BasePersonalAccount'),
                params: params,
                timeout: 5 * 60 * 1000,
                success: function (f, action) {
                    var resp = Ext.decode(action.response.responseText);
                    Ext.Msg.alert('Выгрузка информации по ЛС', resp.message || "Задача успешно поставлена в очередь на обработку. " +
                        "Информация о статусе формирования выгрузки с информацией по ЛС содержится в пункте меню \"Задачи\"");
                    me.unmask();
                },
                failure: function (_, response) {
                    var resp = Ext.decode(response.response.responseText);
                    Ext.Msg.alert('Ошибка', resp.message);
                    me.unmask();
                }
            });
        };

        win.show();
    },

    //#region Payment docs
    getPaymentDocuments: function (button) {
        var me = this,
            filterParams = me.getFilterParams(),
            grid = button.up('paccountgrid'),
            records = grid.getSelectionModel().getSelection(),
            isZeroPaymentDoc = button.action === 'GetZeroPaymentDocs',
            accountIds = [],
            period = grid.down('b4selectfield[name=ChargePeriod]').value;      
            if (period === null) {
                Ext.Msg.alert('Внимание', 'Выберите из фильтра нужный период');
                return false;
            }
        var isPeriodClosed = period.IsClosed,
                isAvailablePrintingInOpenPeriod = Gkh.config.RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.PrintingInOpenPeriod;

        // Выполнить проверку перед выгрузкой по открытому периоду. Исключение - нулевые квитанции (isZeroPaymentDoc), для них проверки не нужны.
        if (!isPeriodClosed && !isZeroPaymentDoc) {
            // Должна быть включена галочка (в единых настройках приложения), разрешающая выгрузку в открытом периоде
            if (!isAvailablePrintingInOpenPeriod) {
                Ext.Msg.alert('Период не закрыт', 'Выгрузить документы на оплату можно только для закрытого периода!');
                return false;
            }
        }

        Ext.Array.each(records, function (rec) {
            accountIds.push(rec.getId());
        });

        var totalCount = grid.getStore().totalCount;
        totalCount = accountIds.length > 0 && accountIds.length < totalCount ? accountIds.length : totalCount;

        if (totalCount == 0) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать лицевые счета');
            return false;
        }

        Ext.Msg.confirm('Внимание', Ext.String.format('Сформировать документы на оплату по {0} выбранным лицевым счетам за период "{1}"? ' +
                                                      'Документы будут сформированы только по лицевым счетам с подтвержденными начислениями.' +
                                                      ' Процесс может занять некоторое время.',
            totalCount, period.Name), function (result) {
                if (result === 'yes') {

                    // Заполняем параметры дополнительными параметры 
                    var postParams = Ext.applyIf(filterParams, {
                        reportId: 'PaymentDocument',
                        accountIds: accountIds.join(","),
                        isZeroPaymentDoc: isZeroPaymentDoc
                    });

                    if (records.length === 1) {
                        var ownerType = records[0].get('OwnerType');

                        if (ownerType === 1) {
                            var win = Ext.create('B4.view.regop.personal_account.action.PaymentDocumentType', { params: postParams });
                            win.show();
                            return;
                        }
                    }

                    me.processPaymentDocument(postParams);
                }
            });
    },

    getPartialPaymentDocuments: function(button) {
        var me = this,
            grid = button.up('paccountgrid'),
            filterParams = me.getFilterParams(),
            records = grid.getSelectionModel().getSelection(),
            accountIds = [],
            legalownerIds = [],
            indiviudalOwnerIds = [],
            period = grid.down('b4selectfield[name=ChargePeriod]').value;

        if (period === null) {
            Ext.Msg.alert('Внимание', 'Выберите из фильтра нужный период');
            return false;
        }

        var isPeriodClosed = period.IsClosed,
            isAvailablePrintingInOpenPeriod = Gkh.config.RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.PrintingInOpenPeriod;

        if (!isPeriodClosed) {
            if (!isAvailablePrintingInOpenPeriod) {
                Ext.Msg.alert('Период не закрыт', 'Выгрузить документы на оплату можно только для закрытого периода!');
                return false;
            }
        }

        Ext.Array.each(records,
            function(rec) {
                accountIds.push(rec.getId());
                if (rec.get('OwnerType') === 1) {
                    legalownerIds.push(rec.getId());
                } else {
                    indiviudalOwnerIds.push(rec.getId());
                }
            });

        if (legalownerIds.length == 0) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать лицевые счета  юридических лиц');
            return false;
        }

        if (accountIds.length == 0) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать лицевые счета');
            return false;
        }

        if (legalownerIds.length > 1 || indiviudalOwnerIds.lenth > 0) {
            Ext.Msg.confirm('Внимание',
                Ext.String.format('Сформировать документы на оплату по выбранным лицевым счетам за периодe {0}? ' +
                    'Документы будут сформированы только для юридических лиц',
                    period.Name),
                function(result) {
                    if (result === 'no') {
                        return false;
                    }
                });
        }


        me.checkIsBaseSnapshots(accountIds, period, filterParams);
    },

    processPaymentDocument: function (params) {
        var me = this;

        me.mask("Обработка...");
        B4.Ajax.request({
            url: B4.Url.action('GetPaymentDocuments', 'BasePersonalAccount'),
            params: params,
            timeout: 9999999
        }).next(function (resp) {
            var tryDecoded, data;

            me.unmask();

            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            data = resp.data ? resp.data : tryDecoded.data;

            Ext.Msg.alert('Документы на оплату', tryDecoded.message || "Задача успешно поставлена в очередь на обработку. " +
                "Информация о статусе формирования документов на оплату содержится в пункте меню \"Задачи\"");

            if (data && data.Id > 0) { //Файл сформирован#1#
                window.open(B4.Url.action('Download', 'FileUpload', { Id: data.Id }));
            }
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    checkIsBaseSnapshots: function (accountIds, period, filterParams) {
        var me = this;
        me.mask("Обработка...");
        B4.Ajax.request({
            url: B4.Url.action('CheckIsBaseSnapshots', 'BasePersonalAccount'),
            params: { accountIds: accountIds, periodId: period.Id },
            timeout: 5 * 60 * 1000
        }).next(function (resp) {
            me.unmask();
            var response = Ext.JSON.decode(resp.responseText);

            if (response.success) {
                var postParams = Ext.applyIf(filterParams,
                    {
                        reportId: 'PaymentDocument',
                        accountIds: accountIds.join(","),
                        isPartially: true
                    });

                me.processPaymentDocument(postParams);
            } else {
                Ext.Msg.alert('Ошибка', response.message);
            }
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    paymentDocumentPreview: function () {
        var me = this,
            filterParams = me.getFilterParams(),
            records = me.getMainView().getSelectionModel().getSelection(),
            accountIds = [],
            periodFld = me.getMainView().down('b4selectfield[name=ChargePeriod]'),
            period = periodFld.getValue(),
            gridStore = Ext.create('Ext.data.Store', {
                fields: [
                   { name: 'id' },
                   { name: 'text' },
                   { name: 'RoomAddress' },
                   { name: 'PostCode' }
                ],
                proxy: {
                    type: 'pagingmemory',
                    enablePaging: true
                }
            });

        Ext.Array.each(records, function (rec) {
            accountIds.push(rec.getId());
        });

        Ext.applyIf(filterParams, {
            accountIds: accountIds.join(",")
        });

        me.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetPaymentDocumentsHierarchyPreview', 'BasePersonalAccount'),
            params: filterParams,
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.previewData = Ext.JSON.decode(response.responseText);

            if (me.previewData.leaf) {
                Ext.Msg.alert('Ошибка', 'Ни один лицевой счет не удовлетворяет настройкам печати');
            } else {
                var win = Ext.create('B4.form.Window', {
                    modal: true,
                    width: 900,
                    height: 700,
                    maximizable: true,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    border: false,
                    items: [
                        {
                            xtype: 'treepanel',
                            autoScroll: true,
                            flex: 1,
                            title: 'Иерархия документов на оплату',
                            store: Ext.create('Ext.data.TreeStore', {
                                root: {
                                    children: me.collectChildren(me.previewData.children)
                                },
                                listeners: {
                                    beforeload: function(store, operation) {
                                        var node = operation.node,
                                            children = operation.node.raw.vChildren;

                                        if (children.length > 0 && children[0].leaf) {
                                            return;
                                        }

                                        operation.records = me.collectChildren(children, node);
                                        delete operation.node.raw.vChildren;
                                    }
                                }
                            }),
                            rootVisible: false,
                            listeners: {
                                itemclick: function(treepanel, record) {
                                    var window = treepanel.up('window'),
                                        grid = window.down('b4grid'),
                                        store = grid.getStore(),
                                        proxy;

                                    // child есть гарантированно, так как в дереве отображаются только записи с child,
                                    // необходимо отобразить в гриде, если childы конечные
                                    var firstChild = record.raw.vChildren[0];

                                    if (firstChild.children === null) {
                                        store.removeAll();
                                        proxy = Ext.create('Ext.ux.data.PagingMemoryProxy', {
                                            enablePaging: true,
                                            data: record.raw.vChildren
                                        });

                                        store.currentPage = 1;
                                        store.setProxy(proxy);
                                        store.load();
                                    }
                                }
                            }
                        },
                        {
                            xtype: 'b4grid',
                            flex: 2,
                            stateful: false,
                            title: 'Лицевые счета',
                            store: gridStore,
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'text',
                                    header: 'Номер ЛС',
                                    flex: 1
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'PostCode',
                                    header: 'Индекс',
                                    width: 50
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'RoomAddress',
                                    header: 'Адрес',
                                    flex: 3
                                },
                                {
                                    xtype: 'actioncolumn',
                                    iconCls: 'icon-page-white-acrobat',
                                    width: 25,
                                    align: 'center',
                                    tooltip: 'Предпросмотр',
                                    handler: function(grid, rowIdx) {
                                        var rec = grid.getStore().getAt(rowIdx);

                                        Ext.create('B4.form.Window', {
                                            width: 600,
                                            height: 700,
                                            layout: 'fit',
                                            modal: true,
                                            maximizable: true,
                                            items: [
                                                {
                                                    xtype: 'component',
                                                    autoEl: {
                                                        tag: 'iframe',
                                                        style: 'height: 100%; width: 100%; border: none',
                                                        src: B4.Url.action(
                                                            Ext.String.format('/BasePersonalAccount/GetPaymentDocument?accountId={0}&periodId={1}&mode={2}', rec.get('id'), period, filterParams.mode))
                                                    }
                                                }
                                            ]
                                        }).show();
                                    }
                                }
                            ],
                            dockedItems: [
                                {
                                    xtype: 'pagingtoolbar',
                                    store: gridStore,
                                    pageSize: 5,
                                    dock: 'bottom',
                                    displayInfo: true
                                }
                            ]
                        }
                    ]
                });

                win.show();
            }

            delete me.previewData;

        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка', 'Ошибка при получении данных для предпросмотра');
        });
    },

    collectChildren: function (children, node) {
        return Ext.Array.map(children, function (child) {
            var obj = {
                id: child.id,
                text: child.text,
                leaf: child.leaf || child.children == null || child.children[0].leaf,
                iconCls: child.iconCls,
                vChildren: child.children
            };
            return node ? node.createNode(obj) : obj;
        });
    },
    //#endregion

    selectPaymentDocumentType: function (btn) {
        var me = this,
            win = btn.up('paymentdocumenttypewin'),
            params,
            paymentDocType = win.down('combobox').getValue();

        win.close();

        params = Ext.apply(win.params, {
            reportPerAccount: paymentDocType == 0
        });

        me.processPaymentDocument(params);
    },

    getCalculationDocuments: function (item) {
        var me = this,
            filterParams = me.getFilterParams(),
            grid = me.getMainView(),
            sm = grid.getSelectionModel(),
            selected = Ext.Array.map(sm.getSelection(), function (el) { return el.get('Id'); }),
            postParams;

        filterParams.providerCode = item.key;

        if (filterParams.providerCode === 'PersonalAccountInfoProxy|default|dbf|Export'
            || filterParams.providerCode === 'PersonalAccountInfoProxy|sber_no_fio|dbf|Export') {

            me.setContextValue(me.getMainView(), 'providerCode', filterParams.providerCode);
            me.sberExport();
            return true;
        }

        postParams = Ext.applyIf(filterParams, {
            accountIds: selected.join(",")
        });

        me.mask("Формирование выгрузки...");
        B4.Ajax.request({
            url: B4.Url.action('ChargeOut', 'PersonalAccountChargePayment', filterParams),
            timeout: 9999999
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText),
                id = json.data.data;

            me.unmask();
            //если включен сервер расчетов
            if (Ext.isEmpty(id)) {
                var message = json.data.message || json.data.Message;
                Ext.Msg.alert('Выгрузка начислений', message || 'Задача поставлена в очередь на обработку');
                return;
            } else {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { id: json.data.Id || json.data.data })
                });
            }
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', 'При получении выгрузки возникла ошибка:' + (e.message || e));
        });
    },

    sberExport: function () {
        var me = this,
            win = Ext.widget('persaccexportsberwindow', {
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            }),
            store = win.down('grid').getStore();

        store.clearFilter(true);

        me.mask("Формирование выгрузки...");
        store.on('load', function (st, records) {
            me.unmask();

            if (records.length > 1) {
                win.show();
            } else if (records.length == 1) {
                // Отправляем на формирвоание выгрузку
                me.onSberExportWithPaymentAgent(records[0].getId());
            } else {
                Ext.Msg.alert('Внимание!', 'Для выгрузки данных по начислениям необходимо заполнить данные по договорам (Путь:  Участники процесса / Роли контрагента / Платежные агенты)');
            }
        }, this, { single: true });

        store.load();
    },

    closeExportSberWindow: function (btn) {
        btn.up('persaccexportsberwindow').close();
    },

    onSberExportWithPaymentAgent: function (payAgId) {
        var me = this,
            filterParams = me.getFilterParams(),
            providerCode = me.getContextValue(me.getMainView(), 'providerCode'),

            // Заполняем параметры дополнительными параметры 
            postParams = Ext.applyIf(filterParams, {
                payAgentId: payAgId
            });

        postParams.providerCode = providerCode;

        me.mask("Формирование выгрузки...");

        B4.Ajax.request({
            url: B4.Url.action('ChargeOut', 'PersonalAccountChargePayment', postParams),
            timeout: 9999999
        }).next(function (response) {
            me.unmask();

            var json = Ext.JSON.decode(response.responseText),
                id = json.data.data;

            //если включен сервер расчетов
            if (Ext.isEmpty(id)) {
                var message = json.data.message || json.data.Message;
                Ext.Msg.alert('Выгрузка начислений', message || 'Задача поставлена в очередь на обработку');
                return;
            }

            Ext.DomHelper.append(document.body, {
                tag: 'iframe',
                id: 'downloadIframe',
                frameBorder: 0,
                width: 0,
                height: 0,
                css: 'display:none;visibility:hidden;height:0px;',
                src: B4.Url.action('Download', 'FileUpload', { id: id })
            });

        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', 'При получении выгрузки возникла ошибка:' + (e.message || e));
        });
    },

    onSelectPaymentAgent: function (btn) {
        var me = this,
            win = btn.up('persaccexportsberwindow'),
            grid = win.down('persaccpaymentagentgrid'),
            record = grid.getSelectionModel().getSelection()[0];

        if (record) {

            // Отправляем на формирвоание выгрузки
            me.onSberExportWithPaymentAgent(record.getId());

            win.close();

        } else {
            Ext.Msg.alert('Ошибка', "Необходимо выбрать платежного агента!");
        }
    },

    onUpdatePaymentAgentGrid: function (btn) {
        btn.up('persaccpaymentagentgrid').getStore().load();
    },

    removeAccounts: function (btn) {
        var me = this,
            filterParams = me.getFilterParams(),
            grid = btn.up('paccountgrid'),
            records = grid.getSelectionModel().getSelection(),
            accountIds = [],
            totalCount = grid.getStore().totalCount,
            message;

        Ext.Array.each(records, function (rec) {
            accountIds.push(rec.getId());
        });

        totalCount = accountIds.length > 0 && accountIds.length < totalCount ? accountIds.length : totalCount;

        // выдаем корректное предупреждение об удалении в зависимости от количества выбранных счетов
        if (totalCount == 1) {
            message = 'Удалить 1 выбранный лицевой счет? После удаления восстановить данные будет невозможно.';
        } else if (2 <= totalCount % 10 && totalCount % 10 <= 4 && !(12 <= totalCount % 100 && totalCount % 100 <= 14)) {
            message = Ext.String.format('Удалить {0} выбранных лицевых счета? После удаления восстановить данные будет невозможно.', totalCount);
        } else {
            message = Ext.String.format('Удалить {0} выбранных лицевых счетов? После удаления восстановить данные будет невозможно.', totalCount);
        }

        Ext.Msg.confirm('Внимание', message, function (result) {
            if (result != 'yes')
                return;

            var postParams = Ext.applyIf(filterParams, {
                accountIds: accountIds.join(",")
            });

            me.mask("Удаление...");
            B4.Ajax.request({
                url: B4.Url.action('RemoveAccounts', 'BasePersonalAccount'),
                params: postParams,
                timeout: 9999999
            }).next(function (resp) {
                me.unmask();
                var respObj = Ext.JSON.decode(resp.responseText);
                var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', respObj.data.LogfileId));
                var logLink = '<a href="' + url + '" target="_blank" style="color: #04468C !important; float: right;">Скачать лог</a>';

                Ext.Msg.show({
                    title: 'Удаление',
                    msg: Ext.String.format('Удаление завершено {0}.&nbsp;{1}', respObj.data.Errors ? 'с ошибками' : 'успешно', logLink),
                    width: 300,
                    buttons: Ext.Msg.OK,
                    icon: Ext.window.MessageBox.INFO
                });
                me.getMainView().getStore().load();
            }).error(function (err) {
                me.unmask();
                Ext.Msg.alert('Ошибка', err.message || err);
            });
        });
    },

    exportToVtscp: function (button) {
        var me = this,
            filterParams = me.getFilterParams(),
            grid = button.up('paccountgrid'),
            records = grid.getSelectionModel().getSelection(),
            accountIds = [];

        Ext.Array.each(records, function (rec) {
            accountIds.push(rec.getId());
        });

        // Заполняем параметры дополнительными параметры 
        var params = Ext.applyIf(filterParams, {
            accountIds: accountIds.join(",")
        });

        me.mask("Обработка...");
        B4.Ajax.request({
            url: B4.Url.action('ExportVtscp', 'BasePersonalAccount'),
            params: params,
            timeout: 9999999
        }).next(function (resp) {
            var tryDecoded,
                id;

            me.unmask();

            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            id = resp.data ? resp.data : tryDecoded.data;

            if (id > 0) {
                window.open(B4.Url.action('Download', 'FileUpload', { Id: id }));
            }
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    exportPenalty: function (button) {
        var me = this,
            filterParams = me.getFilterParams(),
            grid = button.up('paccountgrid'),
            records = grid.getSelectionModel().getSelection(),
            sfPeriod = grid.down('b4selectfield[name=ChargePeriod]'),
            period = sfPeriod.getStore().getById(sfPeriod.getValue()),
            accountIds = [];

        if (period.get('IsClosed') !== true) {
            Ext.Msg.alert('Период не закрыт', 'Выгрузить начисления пени можно только для закрытого периода!');
            return false;
        }

        Ext.Array.each(records, function (rec) {
            accountIds.push(rec.getId());
        });

        // Заполняем параметры дополнительными параметры 
        var params = Ext.applyIf(filterParams, {
            accountIds: accountIds.join(",")
        });

        me.mask("Обработка...");
        B4.Ajax.request({
            url: B4.Url.action('ExportPenalty', 'BasePersonalAccount'),
            params: params,
            timeout: 9999999
        }).next(function (resp) {
            var tryDecoded,
                id;

            me.unmask();

            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            id = resp.data ? resp.data : tryDecoded.data;

            if (id > 0) {
                window.open(B4.Url.action('Download', 'FileUpload', { Id: id }));
            }
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    exportPenaltyExcel: function (button) {
        var me = this,
            filterParams = me.getFilterParams(),
            grid = button.up('paccountgrid'),
            records = grid.getSelectionModel().getSelection(),
            sfPeriod = grid.down('b4selectfield[name=ChargePeriod]'),
            period = sfPeriod.getStore().getById(sfPeriod.getValue()),
            accountIds = [];

        if (period.get('IsClosed') !== true) {
            Ext.Msg.alert('Период не закрыт', 'Выгрузить начисления пени можно только для закрытого периода!');
            return false;
        }

        Ext.Array.each(records, function (rec) {
            accountIds.push(rec.getId());
        });

        // Заполняем параметры дополнительными параметры 
        var params = Ext.applyIf(filterParams, {
            accountIds: accountIds.join(",")
        });

        me.mask("Обработка...");
        B4.Ajax.request({
            url: B4.Url.action('ExportPenaltyExcel', 'BasePersonalAccount'),
            params: params,
            timeout: 9999999
        }).next(function (resp) {
            var tryDecoded,
                id;

            me.unmask();

            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            id = resp.data ? resp.data : tryDecoded.data;

            if (id > 0) {
                window.open(B4.Url.action('Download', 'FileUpload', { Id: id }));
            }
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },
    
    disableFilter: function (disable) {
        var elBtn = this.getMainView().down('#btnClearAllFilters');
        
        elBtn.setDisabled(disable);

        if (disable) {
            elBtn.el.fadeOut({
                duration: .25,
                callback: this.afterHide,
                scope: this
            });
        } else {
            elBtn.el.fadeIn({
                duration: .25,
                callback: this.afterHide,
                scope: this
            });
        }
    },
    
    afterHide: function () {
        this.getMainView().getDockedComponent('toptoolbar').doLayout();
    },

    goToPAccountsGrid: function () {
        B4.getBody().remove(B4.getBody().getActiveTab(), true);
        this.application.redirectTo('regop_personal_account');
    },

    settingSigns: function (btn) {
        var me = this,
            win = btn.up('setsignswin'),
            form = win.down('form'),
            isNotDebtor = win.down('checkbox[name=IsNotDebtor]').getValue(),
            installmentPlan = win.down('checkbox[name=InstallmentPlan]').getValue(),
            accIds = [];

        if (form.getForm().isValid()) {
            var params = {

                IsNotDebtor: isNotDebtor,
                InstallmentPlan: installmentPlan
            };

            Ext.apply(params, me.getFilterParams());

            me.getMainView().getSelectionModel().getSelection().forEach(function (element) {
                accIds.push(element.getId());
            });

            params.accIds = Ext.encode(accIds);
            params.operationCode = win.accountOperationCode;

            me.mask('Установка параметров...', win.getEl());

            form.submit({
                url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
                params: params,
                timeout: 9999999,
                success: function () {
                    me.unmask();
                    Ext.Msg.alert('Успешно', 'Установка параметров выполнена успешно');
                    win.close();
                },
                failure: function (f, action) {
                    me.unmask();
                    var json = Ext.JSON.decode(action.response.responseText);
                    Ext.Msg.alert('Ошибка', json.message);
                    win.close();
                }
            });
        } else {
            Ext.Msg.alert('Внимание', 'Не заполнены обязательные поля!');
            return false;
        }
    },

    settingStatus: function (btn) {
        var me = this,
            window = btn.up('window'),
            form = window.down('form').getForm(),

            grid = me.getMainView(),
            sm = grid.getSelectionModel(),
            selected = Ext.Array.map(sm.getSelection(), function (el) { return el.get('Id'); });

        if (window.down('form').getForm().isValid()) {
            Ext.Msg.confirm('Закрытие счета', 'Доля собственности лицевого счета станет 0. Продолжить?', function (res) {
                if (res === 'yes') {
                    me.mask('Закрытие счета...');

                    form.submit({
                        url: B4.Url.action('ClosePersonalAccounts', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
                        params: {
                            accIds: selected
                        },
                        success: function () {
                            window.close();
                            Ext.Msg.alert('Результат', 'Выполнено успешно');
                            me.getMainView().getStore().load();
                            me.unmask();
                        },
                        failure: function (_, response) {
                            var resp = Ext.decode(response.response.responseText);
                            window.close();
                            Ext.Msg.alert('Ошибка закрытия', resp.message);

                            me.unmask();
                        }
                    });
                }
            }, me);
        }
    },

    showMassSaldoChangeWindow: function(recs, action) {
        var me = this,
            massSaldoChangeWin,
            persAccIds = [];

        Ext.Array.each(recs, function (rec) {
            persAccIds.push(rec.getId());
        });

        me.mask('Загрузка', me.getMainView());
        B4.Ajax.request({
            url: B4.Url.action('GetOperationDataForUI', 'BasePersonalAccount'),
            timeout: 9999999,
            params: {
                operationCode: action,
                accIds: Ext.encode(persAccIds)
            }
        }).next(function (response) {
            me.unmask();

            var responseObj = Ext.decode(response.responseText);

            if (!responseObj.success) {
                Ext.Msg.alert('Ошибка', responseObj.message || 'Ошибка при выполнении операции!');
                return;
            }

            massSaldoChangeWin = Ext.create('B4.view.regop.personal_account.action.MassSaldoChangeWin', {
                accountOperationCode: action,
                accountData: responseObj.data,
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            massSaldoChangeWin.down('form').getForm().isValid();
            massSaldoChangeWin.down('grid').getStore().load();
            massSaldoChangeWin.down('grid')
                .getPlugin('cellEdit')
                .on('edit',
                    function (editor, e) {
                        var saldoBase = e.record.get('NewSaldoByBaseTariff'),
                            saldoDecision = e.record.get('NewSaldoByDecisionTariff'),
                            saldoPenalty = e.record.get('NewSaldoByPenalty');

                        if (e.field.indexOf('New') >= 0) {
                            e.record.set('NewSaldo', saldoBase + saldoDecision + saldoPenalty);
                        }
                    });

            massSaldoChangeWin.saldoFromToMap = [
                { from: B4.enums.SaldoChangeSaldoFromType.BaseTariff, to: B4.enums.SaldoChangeSaldoToType.BaseTariff },
                { from: B4.enums.SaldoChangeSaldoFromType.DecisionTariff, to: B4.enums.SaldoChangeSaldoToType.DecisionTariff },
                { from: B4.enums.SaldoChangeSaldoFromType.Penalty, to: B4.enums.SaldoChangeSaldoToType.Penalty }
            ];

            var saldoFromField = massSaldoChangeWin.down('[name=SaldoFrom]'),
                saldoToField = massSaldoChangeWin.down('[name=SaldoTo]'),
                saldoFromTypeColumns = [
                    { type: B4.enums.SaldoChangeSaldoFromType.BaseTariff, SaldoFrom: 'SaldoByBaseTariff', NewSaldoFrom: 'NewSaldoByBaseTariff' },
                    { type: B4.enums.SaldoChangeSaldoFromType.DecisionTariff, SaldoFrom: 'SaldoByDecisionTariff', NewSaldoFrom: 'NewSaldoByDecisionTariff' },
                    { type: B4.enums.SaldoChangeSaldoFromType.Penalty, SaldoFrom: 'SaldoByPenalty', NewSaldoFrom: 'NewSaldoByPenalty' }
                ],
                saldoToTypeColumns = [
                    { type: B4.enums.SaldoChangeSaldoToType.BaseTariff, SaldoTo: 'SaldoByBaseTariff', NewSaldoTo: 'NewSaldoByBaseTariff' },
                    { type: B4.enums.SaldoChangeSaldoToType.DecisionTariff, SaldoTo: 'SaldoByDecisionTariff', NewSaldoTo: 'NewSaldoByDecisionTariff' },
                    { type: B4.enums.SaldoChangeSaldoToType.Penalty, SaldoTo: 'SaldoByPenalty', NewSaldoTo: 'NewSaldoByPenalty' }
                ],

                disableFields = function(disabled) {
                    saldoFromField.setDisabled(disabled);
                    saldoToField.setDisabled(disabled);
                },
                getSaldoFromColumns = function() {
                    return Ext.Array.filter(saldoFromTypeColumns, function(t) { return t.type === saldoFromField.getValue(); }, me)[0];
                },
                getSaldoToColumns = function() {
                    return Ext.Array.filter(saldoToTypeColumns, function(t) { return t.type === saldoToField.getValue(); }, me)[0];
                },
                getArgs = function() {
                    return {
                        SaldoWindow: massSaldoChangeWin,

                        SaldoFrom: getSaldoFromColumns().SaldoFrom,
                        NewSaldoFrom: getSaldoFromColumns().NewSaldoFrom,

                        SaldoTo: getSaldoToColumns().SaldoTo,
                        NewSaldoTo: getSaldoToColumns().NewSaldoTo
                    }
                };

            massSaldoChangeWin.operations = [
                {
                    type: B4.enums.SaldoChangeOperationType.Manual,
                    execute: function () {
                        disableFields(true);
                        me.executeManualSaldoChangeOperation(getArgs());
                    }
                },
                {
                    type: B4.enums.SaldoChangeOperationType.DebtAndOverpayment,
                    execute: function () {
                        disableFields(false);
                        me.executeDebtOverpaymentSaldoChangeOperation(getArgs());
                    }
                },
                {
                    type: B4.enums.SaldoChangeOperationType.Overpayment,
                    execute: function () {
                        disableFields(false);
                        me.executeOverpaymentSaldoChangeOperation(getArgs());
                    }
                },
                {
                    type: B4.enums.SaldoChangeOperationType.SetToZero,
                    execute: function () {
                        saldoFromField.setDisabled(false);
                        saldoToField.setDisabled(true);

                        me.executeSetToZeroSaldoChangeOperation(getArgs());
                    }
                }
            ];

            massSaldoChangeWin.show();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при выполнении операции!');
        });

        
    },

    processSaldoChange: function(btn) {
        var me = this,
            win = btn.up('window'),
            form = win.down('form'),
            grid = win.down('grid'),
            params = form.getValues(),
            store = grid.getStore(),
            array = [];

        store.each(function(rec) {
            if (rec.get('SaldoByBaseTariff') !== rec.get('NewSaldoByBaseTariff') ||
                rec.get('SaldoByDecisionTariff') !== rec.get('NewSaldoByDecisionTariff') ||
                rec.get('SaldoByPenalty') !== rec.get('NewSaldoByPenalty')) {

                array.push({
                    Id: rec.get('Id'),
                    SaldoByBaseTariff: rec.get('SaldoByBaseTariff'),
                    SaldoByDecisionTariff: rec.get('SaldoByDecisionTariff'),
                    SaldoByPenalty: rec.get('SaldoByPenalty'),

                    NewSaldoByBaseTariff: rec.get('NewSaldoByBaseTariff'),
                    NewSaldoByDecisionTariff: rec.get('NewSaldoByDecisionTariff'),
                    NewSaldoByPenalty: rec.get('NewSaldoByPenalty')
                });
            }
        });

        params.operationCode = win.accountOperationCode;
        params.modifRecs = Ext.encode(array);

        if (form.getForm().isValid()) {

            me.mask('Установка и изменение сальдо...', win);

            form.submit({
                url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount', { 'b4_pseudo_xhr': true }),
                params: params,
                timeout: 9999999,
                success: function () {
                    me.unmask();
                    Ext.Msg.alert('Успешно', 'Установка и изменение сальдо выполнена успешно');
                    win.close();
                },
                failure: function (f, action) {
                    me.unmask();
                    var json = Ext.JSON.decode(action.response.responseText);
                    Ext.Msg.alert('Ошибка', json.message);
                    win.close();
                }
            });
        } else {
            Ext.Msg.alert('Ошибка', 'Не заполнены обязательные поля');
        }
    },

    saldoChangeSaldoFromChange: function (field) {
        var me = this,
            saldoChangeWindow = field.up('window'),
            saldoToField = saldoChangeWindow.down('[name=SaldoTo]'),
            saldoToNewValue = Ext.Array.filter(saldoChangeWindow.saldoFromToMap, function (o) { return o.from !== field.getValue(); }, me)[0];

        saldoToField.getStore().clearFilter();
        if (!saldoToField.disabled && saldoToNewValue) {
            saldoToField.setValue(saldoToNewValue.to);
        }

        me.saldoChangeOperationTypeChange(field);
    },

    saldoChangeSaldoToExpand: function (field) {
        var me = this,
            saldoChangeWindow = field.up('window'),
            saldoFromField = saldoChangeWindow.down('[name=SaldoFrom]'),
            saldoToExlude = Ext.Array.filter(saldoChangeWindow.saldoFromToMap, function (o) { return o.from === saldoFromField.getValue(); }, me)[0];

        if (saldoToExlude) {
            field.getStore().filterBy(function (rec) {
                return rec.get('Value') !== saldoToExlude.to;
            }, me);
        }
    },

    saldoChangeOperationTypeChange: function (field) {
        var me = this,
            saldoChangeWindow = field.up('window'),
            operationType = saldoChangeWindow.down('[name=OperationType]').getValue(),
            operation = Ext.Array.filter(saldoChangeWindow.operations, function(o) { return o.type === operationType; }, me)[0];

        if (!operation) {
            return false;
        }

        operation.execute();
    },

    executeManualSaldoChangeOperation: function (args) {
        var store = args.SaldoWindow.down('grid').getStore();
        store.load();
    },

    executeDebtOverpaymentSaldoChangeOperation: function (args) {
        var me = this,
            store = args.SaldoWindow.down('grid').getStore();

        store.load();
        store.each(function (record) {
            if (record.get(args.SaldoFrom) !== 0) {
                record.set(args.NewSaldoFrom, 0);
                record.set(args.NewSaldoTo, record.get(args.SaldoTo) + record.get(args.SaldoFrom));
                me.setNewSaldo(record);
            }
        });
    },

    executeOverpaymentSaldoChangeOperation: function (args) {
        var me = this,
            store = args.SaldoWindow.down('grid').getStore();

        store.load();
        store.each(function(record) {
            if (record.get(args.SaldoFrom) < 0) {
                record.set(args.NewSaldoFrom, 0);
                record.set(args.NewSaldoTo, record.get(args.SaldoTo) + record.get(args.SaldoFrom));
                me.setNewSaldo(record);
            }
        });
    },

    executeSetToZeroSaldoChangeOperation: function (args) {
        var me = this,
            store = args.SaldoWindow.down('grid').getStore();

        store.load();
        store.each(function(record) {
            if (record.get(args.SaldoFrom) !== 0) {
                record.set(args.NewSaldoFrom, 0);
                me.setNewSaldo(record);
            }
        });
    },

    setNewSaldo: function(record) {
        record.set('NewSaldo', record.get('NewSaldoByBaseTariff') + record.get('NewSaldoByDecisionTariff') + record.get('NewSaldoByPenalty'));
    },

    processTurnOffLockFromCalculation: function (recs) {
        var me = this,
            persAccIds = [];

        Ext.Array.each(recs, function (rec) {
            persAccIds.push(rec.getId());
        });

        Ext.Msg.confirm('Внимание!','По данным лицевым счетам были сформированы квитанции в открытом периоде. Удалить слепки?' +
                        'Внимание: При нажатии на "Нет", данные в квитанции останутся прежними!', function (result) {
                if (result === 'yes') {
                    var delsnapsh = true;               
                    var period = me.getMainView().down('b4selectfield[name=ChargePeriod]').getValue();
            
                    if (period === null) {
                        Ext.Msg.alert('Внимание', 'Выберите из фильтра нужный период');
                        return false;
                    }
                } 
                me.mask('Снятие блокировки расчета...', me.getMainView());
                B4.Ajax.request({
                    url: B4.Url.action('ExecuteAccountOperation', 'BasePersonalAccount'),
                    params: {
                        operationCode: 'TurnOffLockFromCalculationOperation',
                        accountsId: persAccIds,                   
                        periodId: period,
                        deletesnapshot: delsnapsh
                    },
                    method: 'POST',
                    timeout: 360000
                })
                .next(function () {
                    me.unmask();
                    me.getMainView().getStore().load();
                })
                .error(function (e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', e.message || 'Ошибка при выполнении операции!');
                });
            });
    },

    calcDebt: function (button) {
        var me = this,
            view = button.up('window'),
            grid = button.up('grid'),
            store = grid.getStore(),
            params = {},
            ownerId = view.down('b4selectfield[name=PreviousOwner]').getValue(),
            dateStart = view.down('datefield[name=DateStart]').getValue(),
            dateEnd = view.down('datefield[name=DateEnd]').getValue(),
            saveButton = view.down('b4savebutton');

        if (!view.getForm().isValid()) {
            return Ext.Msg.alert('Ошибка', 'Не заполнены обязательные поля');
        }

        if (dateEnd < dateStart) {
            return Ext.Msg.alert('Внимание', '"Период по" должен быть не ранее чем "Период с"');
        }

        params.persAccId = view.persAccId;
        params.ownerId = ownerId;
        params.dateStart = dateStart;
        params.dateEnd = dateEnd;
        params.operationCode = 'CalcDebtOperation';

        me.mask('Загрузка', me.getMainView());
        B4.Ajax.request({
            url: B4.Url.action('GetOperationDataForUI', 'BasePersonalAccount'),
            timeout: 2 * 60 * 1000,
            params: params
        }).next(function (response) {
            me.unmask();

            saveButton.setDisabled(false);

            var responseObj = Ext.decode(response.responseText);
            var models = [];

            models.push(Ext.create('B4.model.regop.personal_account.CalcDebtDetail', responseObj.data[0]));
            models.push(Ext.create('B4.model.regop.personal_account.CalcDebtDetail', responseObj.data[1]));
            store.loadData(models);

        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при выполнении операции!');
        });
    },

    saveDebtTransfer: function(button) {
        var me = this,
            view = button.up('window'),
            form = view.getForm(),
            grid = view.down('grid'),
            store = grid.getStore(),
            saldoSuccess,
            params = {},
            ownerId = view.down('b4selectfield[name=PreviousOwner]').getValue(),
            exportButton = view.down('button[action=export]'),
            printButton = view.down('button[action=print]');

        params.persAccId = view.persAccId;
        params.ownerId = ownerId;
        params.operationCode = 'CalcDebtOperation';

        saldoSuccess = Ext.each(store.getRange(), function(rec) {
            if (!Ext.isNumeric(rec.get('SaldoOutBaseTariff')) ||
                !Ext.isNumeric(rec.get('SaldoOutDecisionTariff')) ||
                !Ext.isNumeric(rec.get('SaldoOutPenalty'))) {
                return false;
            }
        });

        if (!saldoSuccess) {
            return Ext.Msg.alert('Внимание', 'Необходимо заполнить столбцы Исходящего сальдо');
        }

        if (form.isValid()) {
            me.mask('Загрузка', me.getMainView());
            form.submit({
                url: B4.Url.action('ExecuteAccountOperation', 'BasePersonalAccount'),
                timeout: 2 * 60 * 1000,
                params: params,
                success: function(f, action) {
                    me.unmask();

                    var calcDebtId = action.result.data.Id;

                    Ext.each(store.getRange(), function(rec) {
                        rec.set('CalcDebt', calcDebtId);
                    });

                    if (grid && grid.container) {
                        me.mask('Сохранение', grid);
                    } else {
                        me.mask('Сохранение');
                    }

                    store.sync({
                        callback: function() {
                            me.unmask();
                        },
                        failure: function(result) {
                            me.unmask();
                            if (result && result.exceptions[0] && result.exceptions[0].response) {
                                Ext.Msg.alert('Ошибка!',
                                    Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                            }
                        }
                    });

                    me.setContextValue(me.getMainView(), 'calcDebtId', calcDebtId);

                    button.setDisabled(true);
                    exportButton.setDisabled(false);
                    printButton.setDisabled(false);
                },
                failure: function(f, action) {
                    me.unmask();
                    var json = Ext.JSON.decode(action.response.responseText);
                    Ext.Msg.alert('Ошибка', json.message);
                }
            });
        } else {
            Ext.Msg.alert('Внимание', 'Не заполнены обязательные поля!');
            return false;
        }
    },

    showCalcDebtOperationWindow: function (recs) {
        var me = this,
            persAcc = recs[0],
            persAccId = persAcc.get('Id'),
            window = Ext.create('B4.view.regop.personal_account.action.CalcDebtWin', {
                renderTo: B4.getBody().getActiveTab().getEl()
            }),
            dateStartFeild = window.down('datefield[name=DateStart]'),
            dateEndField = window.down('datefield[name=DateEnd]');


        window.down('textfield[name=PersonalAccountNum]').setValue(persAcc.get('PersonalAccountNum'));
        window.down('textfield[name=Address]').setValue(persAcc.get('RoomAddress'));
        window.down('textfield[name=CurrentOwner]').setValue(persAcc.get('AccountOwner'));
        window.down('textfield[name=AreaShare]').setValue(persAcc.get('AreaShare'));
        window.down('textfield[name=Area]').setValue(persAcc.get('RoomArea'));

        B4.Ajax.request({
            url: B4.Url.action('GetperiodInfo', 'PersonalAccountCalcDebt')
        }).next(function (response) {
            var responseObj = Ext.decode(response.responseText);

            dateStartFeild.minValue = new Date(responseObj.StartDate);
            dateStartFeild.maxValue = new Date(responseObj.EndDate);
            dateEndField.minValue = new Date(responseObj.StartDate);
            dateEndField.maxValue = new Date(responseObj.EndDate);

        }).error(function (e) {
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при выполнении операции!');
        });

        window.persAccId = persAccId;
        window.down('form').getForm().isValid();
        window.show();
    },

    onExportButtonClick: function (btn) {
        var me = this;
        me.getAspect('calcdebtexportaspect').printReport(btn);
    },

    onPrintButtonClick: function (btn) {
        var me = this;
        me.getAspect('calcdebtaspect').printReport(btn);
    }
});