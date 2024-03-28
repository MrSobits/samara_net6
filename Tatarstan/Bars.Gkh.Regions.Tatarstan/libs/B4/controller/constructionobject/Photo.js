Ext.define('B4.controller.constructionobject.Photo', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.constructionobject.Photo'
    ],

    views: [
        'constructionobject.photo.Grid',
        'constructionobject.photo.EditWindow'
    ],

    models: ['constructionobject.Photo'],

    stores: ['constructionobject.Photo'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'constructionobject.photo.Grid',
    mainViewSelector: 'constructionobjectphotogrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjectphotogrid'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjectphotopermission',
            name: 'photoPermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'constructobjPhotoGridWindowAspect',
            gridSelector: 'constructionobjectphotogrid',
            editFormSelector: 'constructobjphotoeditwindow',
            modelName: 'constructionobject.Photo',
            editWindowView: 'constructionobject.photo.EditWindow',
            listeners: {
                getdata: function(asp, record) {
                    var me = this;
                    record.set('ConstructionObject', me.controller.getContextValue(me.controller.getMainView(), 'constructionObjectId'));
                }
            }
        }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjectphotogrid');

        me.bindContext(view);
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view, 'construction_object_info');
        me.getAspect('photoPermissionAspect').setPermissionsByRecord({ getId: function() { return id; } });

        view.getStore().load();
    },

    init: function() {
        var me = this,
            actions = {};

        actions[me.mainViewSelector] = {
            'store.beforeload': {
                fn: me.onBeforeLoad,
                scope: me
            }
        };

        me.control(actions);
        me.callParent(arguments);
    },

    onBeforeLoad: function(store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'constructionObjectId');
    }
});