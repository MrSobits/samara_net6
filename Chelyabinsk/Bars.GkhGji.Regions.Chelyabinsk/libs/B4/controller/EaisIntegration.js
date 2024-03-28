Ext.define('B4.controller.EaisIntegration', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.eaisintegration.Panel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context',
    },

    mainView: 'eaisintegration.Panel',
    mainViewSelector: 'eaisintegrationpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'eaisintegrationpanel'
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'eaisintegrationpanel eaisintegrationgrid': {
                    render: me.updateGrid,
                    gotoappeal: me.gotoAppeal,
                    resendappeal: me.resendAppeal,
                    scope: me
                }
            };;

        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('eaisintegrationpanel');

        me.bindContext(view);
        me.application.deployView(view);
    },

    updateGrid: function(grid) {
        var me = this,
            store = (grid || me.getMainView().down('eaisintegrationgrid')).getStore();

        store.load();
    },

    gotoAppeal: function(record) {
        var me = this,
            id = record.get('AppealCitsId');

        me.getController('B4.controller.PortalController')
            .loadController('B4.controller.AppealCits',
                {
                    appealId: id
                });
    },

    resendAppeal: function(record) {
        var me = this,
            id = record.get('Id'),
            params = {};

        Ext.apply(params, {
            id: id
        });

        me.mask('Перезапуск отправки данных', me.getMainComponent());

        B4.Ajax.request({
                url: B4.Url.action('Restart', 'AppealCitsTransferResult'),
                method: 'POST',
                params: params
            })
            .next(function(response) {
                me.unmask();

                me.updateGrid();

            })
            .error(function(response) {
                me.unmask();

                me.updateGrid();
            });
    }
});