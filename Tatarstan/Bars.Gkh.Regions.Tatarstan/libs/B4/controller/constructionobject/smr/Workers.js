Ext.define('B4.controller.constructionobject.smr.Workers', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.constructionobject.smr.Workers'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['constructionobject.TypeWork'],
    stores: ['constructionobject.TypeWork'],
    views: [
        'constructionobject.smr.WorkersGrid'
    ],

    mainView: 'constructionobject.smr.WorkersGrid',
    mainViewSelector: 'constructionobjsmrworkersgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjsmrworkersgrid'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjectsmrworkerspermission',
            name: 'smrWorkersPermissionAspect'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела количество рабочих
            */
            xtype: 'inlinegridaspect',
            name: 'smrWorkersGridAspect',
            modelName: 'constructionobject.TypeWork',
            gridSelector: 'constructionobjsmrworkersgrid'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjsmrworkersgrid');

        me.bindContext(view);
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view, 'construction_object_info');
        me.getAspect('smrWorkersPermissionAspect').setPermissionsByRecord({ getId: function () { return id; } });

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