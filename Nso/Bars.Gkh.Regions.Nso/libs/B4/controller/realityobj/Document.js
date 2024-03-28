Ext.define('B4.controller.realityobj.Document', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'realityobj.Document'
    ],

    stores: [
        'realityobj.Document'
    ],
    
    views: [
        'realityobj.Document.Grid',
        'realityobj.Document.EditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjDocumentGrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
            { name: 'Gkh.RealityObject.Register.Document.Create', applyTo: 'b4addbutton', selector: 'realityobjDocumentGrid' },
            { name: 'Gkh.RealityObject.Register.Document.Edit', applyTo: 'b4savebutton', selector: 'realityobjDocumentEditWindow' },
            {
                name: 'Gkh.RealityObject.Register.Document.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjDocumentGrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjDocumentGridWindowAspect',
            gridSelector: 'realityobjDocumentGrid',
            editFormSelector: 'realityobjDocumentEditWindow',
            storeName: 'realityobj.Document',
            modelName: 'realityobj.Document',
            editWindowView: 'realityobj.Document.EditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.RealityObject = asp.controller.getContextValue(asp.controller.getMainView(), 'realityObjectId');
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;
        
        me.getStore('realityobj.Document').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjDocumentGrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.Document').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
    }
});