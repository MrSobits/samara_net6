Ext.define('B4.controller.regop.cproc.ComputingProcess', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.view.regop.cproc.types.PaymentDownloadWindow',
        'B4.view.regop.cproc.types.PaymentDocumentWindow',
        'B4.view.regop.cproc.types.PaymentUploadWindow',
        'B4.view.regop.cproc.types.ChargeWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'ComputingProcess'
    ],

    stores: [
        'regop.ComputingProcess'
    ],

    views: [
        'regop.cproc.ComputingProcessGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'cprocgrid'
        }
    ],

    activeRecord: null,

    init: function() {
        var me = this;
        me.control({
            'cprocgrid': {
                'rowaction': {
                    fn: me.onGridAction
                }
            },
            'comprchargewindow gridpanel': {
                'render': function(grid) {
                    grid.getStore().on('beforeload', me.onChargesBeforeLoad, me);
                }
            }
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('cprocgrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },

    onGridAction: function (grid, action, record) {
        this.activeRecord = record;
        this.onShowDownloadWindow(record);
    },

    onShowDownloadWindow: function (rec) {
        var wins = {
            10: 'B4.view.regop.cproc.types.ChargeWindow',
            21: 'B4.view.regop.cproc.types.PaymentDownloadWindow',
            22: 'B4.view.regop.cproc.types.PaymentUploadWindow'
        };

        if (!wins[rec.get('Type')]) {
            return;
        }

        var win = Ext.ComponentQuery.query(wins[rec.get('Type')])[0];

        if (!win) {
            win = Ext.create(wins[rec.get('Type')],
                {
                    constrain: true,
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    closeAction: 'destroy',
                    taskId: rec.get('Id')
                });
        }

        win.show();
    },

    onChargesBeforeLoad: function (store) {
        var data = this.activeRecord.get('Data');

        if (!(data && data.Data)) {
            return;
        }

        var packetId = data.data.data;

        Ext.apply(store.getProxy().extraParams, {
            packetId: packetId
        });
    }
});