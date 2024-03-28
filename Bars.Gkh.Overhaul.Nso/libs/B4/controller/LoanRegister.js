Ext.define('B4.controller.LoanRegister', {
    extend: 'B4.base.Controller',
    requires: ['B4.controller.longtermprobject.Navigation'],

    models: ['longtermprobject.Loan', 'LongTermPrObject'],

    stores: ['LoanRegister'],

    views: ['loanregister.Grid'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'loanregistergrid'
        }
    ],

    aspects: [],

    init: function() {
        this.control({
            'loanregistergrid': {
                rowaction: {
                    fn: this.onRowAction,
                    scope: this
                }
            },
            'loanregistergrid b4updatebutton': {
                click: {
                    fn: function(btn) {
                        btn.up('loanregistergrid').getStore().load();
                    }
                }
            }
        });

        this.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView() || Ext.widget('loanregistergrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    },

    onRowAction: function (grid, action, record) {
        var me = this,
            portal,
            model,
            params;
        
        me.mask('Загрузка', me.getMainView());
        if (action.toLowerCase() === 'edit') {
            portal = me.getController('PortalController');
            model = this.getModel('LongTermPrObject');

            params = new model({ Id: record.get('Object').Id, Address: record.get('ObjectAddress') });

            params.defaultController = 'B4.controller.longtermprobject.Loan';
            params.defaultParams = { longTermObjId: record.get('Object').Id };

            me.loadController('B4.controller.longtermprobject.Navigation', params, portal.containerSelector, function () { me.unmask(); });
        }
    }
});