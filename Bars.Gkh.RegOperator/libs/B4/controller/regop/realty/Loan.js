Ext.define('B4.controller.regop.realty.Loan', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.view.regop.realty.loan.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realtyloangrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    
    init: function() {
        var me = this;

        me.control({            
            'realtyloangrid [action=returnloan]': {
                'click': {
                    fn: me.onClickReturnLoan,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realtyloangrid'),
            store = view.getStore();
        
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        store.clearFilter(true);
        store.filter('roId', id);
    },
    
    onClickReturnLoan: function() {
        var me = this,
            grid = me.getMainView(),
            store = grid.getStore(),
            selected = grid.getSelectionModel().selected.items,
            ids = selected.map(function (val) { return val.get('Id') }),
            filters = grid.getHeaderFilters(),
            params = {
                complexFilter: Ext.encode(filters),
                ids: Ext.encode(ids),
                roId: me.getContextValue(me.getMainView(), 'realityObjectId')
            },
            count = ids.length || store.totalCount;

        Ext.Msg.confirm(
            'Возврат займов',
            Ext.String.format('Осуществить возврат займа по {0} выбранным записям?' +
                ' Возврат займов будет осуществлен только по записям в статусе "Утверждено"', count),
                function (result) {
                    if (result === 'yes') {

                        me.mask('Возврат займа...', B4.getBody().getActiveTab());

                        B4.Ajax.request({
                            url: B4.Url.action('/RealityObjectLoan/Repayment'),
                            params: params
                        }).next(function () {
                            me.unmask();
                            B4.QuickMsg.msg('Успешно', 'Возврат займов выполнен успешно', 'success');
                            store.load();
                        }).error(function (e) {
                            me.unmask();
                            B4.QuickMsg.msg('Ошибка', e.message || 'Ошибка возврата займа', 'error');
                        });
                    }
                });
    }
});