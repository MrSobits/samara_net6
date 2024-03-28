Ext.define('B4.controller.regop.RegistryAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.regop.RegistryAccountGrid',
        'B4.mixins.Context',
        'B4.model.RealityObject',
        'B4.aspects.ButtonDataExport',
        'B4.Ajax',
        'B4.Url'
    ],

    models: ['RealityObject'],

    stores: ['calcaccount.SpecialRegister'],

    views: [
        'regop.RegistryAccountGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'regaccountgrid'
        }
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'RegistryAccountBtExportAspect',
            gridSelector: 'regaccountgrid',
            buttonSelector: 'regaccountgrid #btnExport',
            controllerName: 'RegopCalcAccount',
            actionName: 'Export'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'regaccountgrid': {
                rowaction: {
                    fn: me.onAccGridRowAction,
                    scope: me
                }
            },
            'regaccountgrid b4updatebutton': {
                click: function(b) {
                    b.up('grid').getStore().load();
                }
            }
        });
        
        this.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('regaccountgrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    onAccGridRowAction: function(scope, action, record) {
        var me = this;

        if (action === 'edit') {
            me.application.redirectTo(Ext.String.format('realityobjectedit/{0}/realtychargeaccount', record.get('RealityObjectId')));
        }
    }
});