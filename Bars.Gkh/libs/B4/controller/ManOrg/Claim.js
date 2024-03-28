Ext.define('B4.controller.manorg.Claim', {
    extend: 'B4.controller.MenuItemController',
    requires: ['B4.aspects.GridEditWindow'],

    models: ['manorg.Claim'],
    stores: ['manorg.Claim'],
    views: [
        'manorg.ClaimEditWindow',
        'manorg.ClaimGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgClaimGridWindowAspect',
            gridSelector: 'manorgclaimgrid',
            editFormSelector: '#manorgClaimEditWindow',
            storeName: 'manorg.Claim',
            modelName: 'manorg.Claim',
            editWindowView: 'manorg.ClaimEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.get('Id')) {
                        record.set('ManagingOrganization', this.controller.params.id);
                    }
                }
            }
        }
    ],

    params: {},
    mainView: 'manorg.ClaimGrid',
    mainViewSelector: 'manorgclaimgrid',

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    init: function () {
        this.getStore('manorg.Claim').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },
    
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('manorgclaimgrid');
        me.getStore('manorg.Claim').load();

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.manorgId = this.params.id;
        }
    }
});