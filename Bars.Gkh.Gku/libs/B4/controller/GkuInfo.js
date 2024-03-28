Ext.define('B4.controller.GkuInfo', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['GkuInfo'],
    stores: ['GkuInfo'],
    views: [
        'gkuinfo.Grid'
    ],

    mainView: 'gkuinfo.Grid',
    mainViewSelector: 'gkuinfogrid',

    refs: [{
            ref: 'mainView',
            selector: 'gkuinfogrid'
        }, {
            ref: 'tarifsGrid',
            selector: 'gkuinfotarifgrid'
        },
        {
            ref: 'gkuInfoEditPanel',
            selector: 'gkuinfoeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgrideditformaspect',
            name: 'gkuinfogridWindowAspect',
            gridSelector: 'gkuinfogrid',
            storeName: 'GkuInfo',
            modelName: 'GkuInfo',
            controllerEditName: 'B4.controller.gkuinfo.Navigation'
        }
    ],

    init: function() {
        var me = this,
            actions = {
                'gkuinfogrid': {
                    render: me.onMainViewRender
                },
                'gkuinfotarifgrid': {
                    render: me.onTarifsGridRender
                },
                'gkuinfogrid b4updatebutton': {
                    click: me.loadGkuInfoGrid
                },
                'gkuinfogrid checkbox': {
                    change: me.loadGkuInfoGrid
                },
                'hiddenfield[name=Id]': {
                    change: me.reloadTarfifGrid
                },
                'datefield[name=month]': {
                    change: me.reloadTarfifGrid
                }
            };
        me.control(actions);
        me.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView() || Ext.widget('gkuinfogrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    },

    loadGkuInfoGrid: function(cmp) {
        this.getMainView().getStore().load();
    },

    beforeGkuInfoStoreLoad: function(store, operation) {
        var me = this, grid = me.getMainView();
        operation.params = operation.params || {};
        operation.params.showIndividual = grid.down('checkbox[name=showIndividual]').getValue();
        operation.params.showBlocked = grid.down('checkbox[name=showBlocked]').getValue();
    },

    beforeTarifsStoreLoad: function(store, operation) {
        var editPanel = Ext.ComponentQuery.query('gkuinfoeditpanel')[0],
            monthField = editPanel.up('panel').down('gkuinfotarifgrid datefield'); // это хак
        operation.params = operation.params || {};
        operation.params.monthField = monthField.getValue();

        operation.params.id = editPanel.down('hiddenfield[name=Id]').getValue();
    },

    onMainViewRender: function(grid) {
        grid.getStore().on('beforeload', this.beforeGkuInfoStoreLoad, this);
    },

    onTarifsGridRender: function(grid) {
        grid.getStore().on('beforeload', this.beforeTarifsStoreLoad, this);

    },
    reloadTarfifGrid: function(f) {
        Ext.ComponentQuery.query('gkuinfotarifgrid')[0].getStore().load();
    }
});