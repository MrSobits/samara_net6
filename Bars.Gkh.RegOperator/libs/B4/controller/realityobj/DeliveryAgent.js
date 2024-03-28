Ext.define('B4.controller.realityobj.DeliveryAgent', {
    extend: 'B4.controller.MenuItemController',

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
        'realityobj.DeliveryAgentGrid'
    ],

    models: [
        'deliveryagent.RealityObject'
    ],

    stores: [
        'deliveryagent.RealityObject'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'rodeliveryagentgrid'
        }
    ],

    init: function() {
        var me = this;

        this.control({
            'rodeliveryagentgrid b4updatebutton': { click: { fn: me.onClickUpdate, scope: me } }
        });

        this.callParent(arguments);
    },

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    onClickUpdate: function () {
        var view = this.getMainView();
        view.getStore().load();
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('rodeliveryagentgrid');
        
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        view.getStore().filter('realityObjectId', id);
    }
});