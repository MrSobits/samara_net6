Ext.define('B4.controller.regop.SpecialAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.model.RealityObject'
    ],

    models: ['RealityObject'],

    stores: [
        'calcaccount.SpecialRegister'
    ],

    views: [
        'regop.special_account.Grid',
        'regop.special_account.Edit'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'specaccgrid'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'specaccgrid': {
                render: function(grid) {
                    grid.getStore().on('beforeload', me.onAccsBeforeLoad, me);
                },
                rowaction: {
                    fn: me.onAccGridRowAction,
                    scope: me
                }
            },
            'specaccgrid b4updatebutton': {
                click: function(b) {
                    b.up('grid').getStore().load();
                }
            },
            'specaccgrid checkbox[action="showall"]': {
                change: function(c) {
                    c.up('grid').getStore().load();
                }
            }
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('specaccgrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },

    onAccsBeforeLoad: function(store) {
        var me = this,
            grid = me.getMainView(),
            showAll = grid.down('checkbox[action="showall"]').getValue();

        Ext.apply(store.getProxy().extraParams, {
            showall: showAll
        });
    },

    onAccGridRowAction: function (scope, action, record) {
        var me = this;

        if (action === 'edit') {
            me.application.redirectTo(Ext.String.format('realityobjectedit/{0}/realtychargeaccount', record.get('RealityObjectId')));
        }
    }
});