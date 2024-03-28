Ext.define('B4.controller.dict.BaseGkhDict', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.model.dict.BaseDict',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mainView: 'dict.BaseGkhDictGrid',
    mainViewSelector: 'basegkhdictgrid',
    dictStore: 'B4.store.dict.BaseDict',
    dictModelName: 'dict.BaseDict',
    controllerName: '',
    title: '',
    permissionPrefix: '',

    mixins: {
        context: 'B4.mixins.Context'
    },

    constructor: function (config) {
        var me = this,
            config = config || {},
            selector = Ext.String.format('{0}[controllerName={1}]', me.mainViewSelector, me.controllerName);
        Ext.apply(me, config);

        me.aspects = me.aspects || [];
        if (!Ext.isEmpty(me.mainViewSelector) && !Ext.isEmpty(me.dictModelName)) {
            me.aspects.push({
                xtype: 'gkhinlinegridaspect',
                name: 'gridAspect',
                modelName: me.dictModelName,
                gridSelector: selector
            });
        } else {
            Ext.Error.raise({
                msg: 'Не верно сконфигурирован gkhinlinegridaspect',
                option: {
                    mainViewSelector: me.mainViewSelector,
                    dictModelName: me.dictModelName
                }
            });
        }

        if (!Ext.isEmpty(me.mainViewSelector) && !Ext.isEmpty(me.permissionPrefix)) {
            me.aspects.push({
                xtype: 'inlinegridpermissionaspect',
                gridSelector: selector,
                permissionPrefix: me.permissionPrefix
            });
        }

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            store = {},
            pagingToolBar = {},
            view = me.getMainView();

        if (!view) {
            store = me.getDictStore();
            view = Ext.widget(me.mainViewSelector, {
                title: me.title,
                store: store,
                controllerName: me.controllerName
            });
            pagingToolBar = view.down('b4pagingtoolbar');
            if (pagingToolBar) {
                pagingToolBar.bindStore(store);
            }
        }

        me.bindContext(view);
        me.application.deployView(view);
        store.load();
    },

    getDictStore: function () {
        var me = this;
        if (!me.dictStore) {
            Ext.Error.raise({
                msg: 'Не задан store справочника',
                option: {
                    dictStore: me.dictStore
                }
            });
        } else if (me.dictStore.isStore) {
            return me.dictStore;
        } else if (Ext.isString(me.dictStore) && !Ext.isEmpty(me.dictStore)) {
            if (Ext.isEmpty(me.controllerName)) {
                Ext.Error.raise({
                    msg: 'Не задан контроллер справочника',
                    option: {
                        dictStore: me.dictStore,
                        controllerName: me.controllerName
                    }
                });
            }
            return Ext.create(me.dictStore, {
                proxy: {
                    type: 'b4proxy',
                    controllerName: me.controllerName
                }
            });
        } else {
            Ext.Error.raise('Не верно сконфигурирован store справочника');
        }
    }
});