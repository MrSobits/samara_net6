Ext.define('B4.controller.manorg.Membership', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.manorg.Membership'
    ],

    models: ['manorg.Membership'],
    stores: ['manorg.Membership'],
    views: [
        'manorg.MembershipEditWindow',
        'manorg.MembershipGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgmembershipperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgMembershipGridWindowAspect',
            gridSelector: 'manorgmembershipgrid',
            editFormSelector: '#manorgMembershipEditWindow',
            storeName: 'manorg.Membership',
            modelName: 'manorg.Membership',
            editWindowView: 'manorg.MembershipEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.set('ManagingOrganization', this.controller.params.id);
                    }
                }
            }
        }
    ],

    params: {},
    mainView: 'manorg.MembershipGrid',
    mainViewSelector: 'manorgmembershipgrid',

    init: function () {
        this.getStore('manorg.Membership').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.manorgId = this.params.id;
        }
    },

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorgmembershipgrid');
        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');

        me.getStore('manorg.Membership').load();
    }
});