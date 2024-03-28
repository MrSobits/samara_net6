Ext.define('B4.controller.regop.loan.Loan', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],
    stores: [
       'regop.Loan'
    ],
    views:[
        'regop.loan.LoanGrid',
        'regop.loan.ResultWindow'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'loangrid' },
        { ref: 'resultWindow', selector: 'loanresultwindow' }
    ],
    
    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'loanStateTransferAspect',
            gridSelector: 'loangrid',
            menuSelector: 'loangridStateMenu',
            stateType: 'gkh_regop_reality_object_loan'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'realityObjectLoanButtonExportAspect',
            gridSelector: 'loangrid',
            buttonSelector: 'loangrid #btnExport',
            controllerName: 'RealityObjectLoan',
            actionName: 'Export'
        },
        {
            xtype: 'gkhpermissionaspect',
            name: 'permission',
            permissions: [
                {
                    name: 'GkhRegOp.RegionalFundUse.Loan.RepaymentAll', applyTo: '[name=repaymentAll]', selector: 'loangrid',
                    applyBy: function (component, allowed) {
                        component.setDisabled(!allowed);
                    }
                }
            ]
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'loangrid [name="repaymentAll"]': {
                click: me.onRepaymentAll
            }
        });
        
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('loangrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    onRepaymentAll: function () {
        var me = this,
            grid = me.getMainView(),
            store = grid.getStore(),
            selected = grid.getSelectionModel().selected.items,
            ids = selected.map(function (val) { return val.get('Id') }),
            filters = grid.getHeaderFilters(),
            params = {
                complexFilter: Ext.encode(filters),
                ids: Ext.encode(ids)
            },
            count = ids.length || store.totalCount;

        Ext.Msg.confirm(
            'Возврат займов',
            Ext.String.format('Осуществить возврат займа по {0} выбранным записям?' +
                ' Возврат займов будет осуществлен только по записям в статусе "Утверждено"', count),
                function(result) {
                    if (result === 'yes') {

                        me.mask('Возврат займов...', grid);

                        B4.Ajax.request({
                            timeout: 5 * 60 * 1000,
                            params: params,
                            url: B4.Url.action('RepaymentAll', 'RealityObjectLoan')
                        }).next(function (resp) {
                            var obj;
                            me.unmask();
                            obj = Ext.decode(resp.responseText);
                            store.load();
                            if (obj.success) {
                                me.showResult(obj.data);
                                B4.QuickMsg.msg('Успешно', 'Возврат займов успешно выполнен', 'success');
                            } else {
                                Ext.Msg.alert('Ошибка', obj.message || window.err);
                            }
                        });
                    }
                });
    },

    showResult: function(records) {
        var me = this,
            win, grid, store;

        win = me.getResultWindow() || Ext.widget('loanresultwindow');
        grid = win.down('b4grid');
        store = grid.getStore();

        win.show();

        store.insert(0, records);
    }
});