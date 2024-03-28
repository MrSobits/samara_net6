Ext.define('B4.controller.constructionobject.TypeWork', {
    /* 
    * Контроллер формы редактирования объектов капремонта
    */
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.constructionobject.TypeWork'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['constructionobject.TypeWork'],
    stores: ['constructionobject.TypeWork'],
    views: [
        'constructionobject.typework.Grid',
        'constructionobject.typework.EditWindow'
    ],

    mainView: 'constructionobject.typework.Grid',
    mainViewSelector: 'constructionobjtypeworkgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjtypeworkgrid'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjecttypeworkpermission',
            name: 'typeWorkPermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'constructionObjTypeWorkGridWindowAspect',
            gridSelector: 'constructionobjtypeworkgrid',
            editFormSelector: 'constructionobjeditwindow',
            modelName: 'constructionobject.TypeWork',
            editWindowView: 'constructionobject.typework.EditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.data.Id) {
                        record.data.ConstructionObject = asp.controller.getContextValue(asp.controller.getMainView(), 'constructionObjectId');
                    }
                }
            },
            otherActions: function(actions) {
                actions[this.editFormSelector + ' [name=Work]'] = {
                    'change': {
                        fn: this.onWorkChange,
                        scope: this
                    }
                };
            },
            onWorkChange: function(sf, val) {
                var unitMeasureField = this.componentQuery('[name=UnitMeasureName]'),
                    typeWorkField = this.componentQuery('[name=TypeWork]');

                unitMeasureField.setValue(val.UnitMeasureName || (val.UnitMeasure || { Name: val.UnitMeasureName }).Name);
                typeWorkField.setValue(val.TypeWork);
            }
        }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjtypeworkgrid');

        me.bindContext(view);
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view, 'construction_object_info');
        me.getAspect('typeWorkPermissionAspect').setPermissionsByRecord({ getId: function () { return id; } });

        view.getStore().load();
    },

    init: function() {
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