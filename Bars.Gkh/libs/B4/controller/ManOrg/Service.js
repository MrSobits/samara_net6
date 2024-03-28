Ext.define('B4.controller.manorg.Service', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.manorg.Service'
    ],

    models: ['manorg.Service'],
    stores: ['manorg.Service'],
    views: [
        'manorg.ServiceEditWindow',
        'manorg.ServiceGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgserviceperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgServiceGridWindowAspect',
            gridSelector: 'manorgservicegrid',
            editFormSelector: '#manorgServiceEditWindow',
            storeName: 'manorg.Service',
            modelName: 'manorg.Service',
            editWindowView: 'manorg.ServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.ManagingOrganization = this.controller.params.id;
                    }
                }
            }
        }
    ],

    params: {},
    mainView: 'manorg.ServiceGrid',
    mainViewSelector: 'manorgservicegrid',

    init: function () {
        this.getStore('manorg.Service').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('manorg.Service').load();
    },
    
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.manorgId = this.params.id;
        }
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgservicegrid');
        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        me.getStore('manorg.Service').load();
    }
});