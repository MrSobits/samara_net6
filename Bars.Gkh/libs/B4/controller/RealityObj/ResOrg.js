Ext.define('B4.controller.realityobj.ResOrg', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.ResOrg'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'realityobj.ResOrg'
    ],

    stores: [
        'realityobj.ResOrg'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'realityobj.ResOrgGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realobjresorggrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'resorgperm',
            name: 'resOrgPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realObjSupplyResOrgGridWindowAspect',
            gridSelector: 'realobjresorggrid',
            storeName: 'realityobj.ResOrg',
            modelName: 'realityobj.ResOrg',
            editRecord: function (record) {
                Ext.History.add(Ext.String.format('supplyresorgedit/{0}/realobjcontract?contractId={1}', record.get('ResourceOrgId'), record.getId()));
            }
        }
    ],

    init: function() {
        var me = this;

        me.getStore('realityobj.ResOrg').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realobjresorggrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.ResOrg').load();
        me.getAspect('resOrgPerm').setPermissionsByRecord({ getId: function () { return id } });
    },

    onBeforeLoad: function(store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
        operation.params.fromContract = false;
    }
});