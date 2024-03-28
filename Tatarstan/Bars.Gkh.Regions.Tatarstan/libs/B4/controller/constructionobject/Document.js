Ext.define('B4.controller.constructionobject.Document', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.constructionobject.Document'
    ],

    views: [
        'constructionobject.document.Grid',
        'constructionobject.document.EditWindow'
    ],

    models: ['constructionobject.Document'],

    stores: ['constructionobject.Document'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'constructionobject.document.Grid',
    mainViewSelector: 'constructionobjectdocumentgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjectdocumentgrid'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjectdocumentpermission',
            name: 'documentPermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'constructobjDocGridWindowAspect',
            gridSelector: 'constructionobjectdocumentgrid',
            editFormSelector: 'constructobjdocumenteditwindow',
            modelName: 'constructionobject.Document',
            editWindowView: 'constructionobject.document.EditWindow',
            listeners: {
                getdata: function(asp, record) {
                    record.set('ConstructionObject', this.controller.getContextValue(this.controller.getMainView(), 'constructionObjectId'));
                }
            }
        }
    ],

    index: function(id) {
        var view = this.getMainView() || Ext.widget('constructionobjectdocumentgrid');

        this.bindContext(view);
        this.setContextValue(view, 'constructionObjectId', id);
        this.application.deployView(view, 'construction_object_info');
        this.getAspect('documentPermissionAspect').setPermissionsByRecord({ getId: function () { return id; } });

        view.getStore().load();
    },

    init: function () {
        var actions = {};

        actions[this.mainViewSelector] = {
            'store.beforeload': {
                fn: this.onBeforeTypeWorkLoad,
                scope: this
            }
        };

        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeTypeWorkLoad: function (_, opts) {
        var view = this.getMainView();
        if (view) {
            opts.params.objectId = this.getContextValue(view, 'constructionObjectId');
        }
    }
});