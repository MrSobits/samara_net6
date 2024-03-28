Ext.define('B4.controller.competition.Protocol', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'competition.Protocol'
    ],
    
    stores: [
        'Competition',
        'competition.Protocol'
    ],
    
    views: [
        'competition.ProtocolGrid',
        'competition.ProtocolEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'competitionprotocolgrid'
        }
    ],

    mainView: 'competition.ProtocolGrid',
    mainViewSelector: 'competitionprotocolgrid',
    
    parentCtrlCls: 'B4.controller.competition.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'competitionprotpermission',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'GkhCr.Competition.Protocol.Edit', applyTo: 'b4savebutton', selector: 'competitionprotocoleditwindow' },
                { name: 'GkhCr.Competition.Protocol.Delete', applyTo: 'b4deletecolumn', selector: 'competitionprotocolgrid' },
                { name: 'GkhCr.Competition.Protocol.Create', applyTo: 'b4addbutton', selector: 'competitionprotocolgrid' }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'competitionProtocolGridWindowAspect',
            gridSelector: 'competitionprotocolgrid',
            editFormSelector: 'competitionprotocoleditwindow',
            modelName: 'competition.Protocol',
            editWindowView: 'competition.ProtocolEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Competition', me.controller.getContextValue(me.controller.getMainComponent(), 'competitionId'));
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        me.control(actions);
        
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('competitionprotocolgrid'),
            model = me.getModel('Competition'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'competitionId', id);
        me.application.deployView(view, 'competition_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('competitionId', id);
        
        me.getAspect('competitionprotpermission').setPermissionsByRecord(new model({ Id: id }));
    }
});