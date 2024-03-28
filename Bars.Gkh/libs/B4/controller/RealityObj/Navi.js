Ext.define('B4.controller.realityobj.Navi', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.TreeNavigationMenu',
        'B4.view.realityobj.NavigationPanel'
    ],

    views: ['realityobj.NavigationPanel'],

    params: {},
    title: 'Жилой дом',

    stores: ['realityobj.NavigationMenu'],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'realityobjNavigationPanel' },
        { ref: 'menuTree', selector: 'realityobjNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'realityobjNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'realityobjNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'realityobjNavigationPanel',
            menuContainer: 'realityobjNavigationPanel menutreepanel',
            tabContainer: 'realityobjNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'realityobjNavigationPanel breadcrumbs',
            storeName: 'realityobj.NavigationMenu',
            deployKey: 'reality_object_info',
            contextKey: 'realityObjectId',
            onMenuItemClick: function (view, record) {
                var me = this,
                    objectId;
                if (record.get('leaf')) {
                    objectId = me.controller.getContextValue(view, me.contextKey);
                    me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), objectId, (record.get('options').SectionId ? record.get('options').SectionId : undefined)));
                    me.setExtraParams(record);
                }
            },
            loadMenu: function (menu) {
                var me = this,
                    treeMenu = menu || me.getMenu(),
                    store = treeMenu.getStore();

                store.on('beforeload', me.onBeforeLoad, me, { single: true });
                store.load({
                    scope: me,
                    callback: function (records) {
                        Ext.each(records, function (rec) {
                            if (rec.get('text') == "Паспорт технического объекта") {
                                rec.collapse();
                            }
                        });

                        me.controller.collapseMenu();
                    }
                });

                treeMenu.setLoading(true, true);
            }
        }
    ],

    init: function() {
        var me = this,
            actions = {};

        actions['realityobjNavigationPanel'] = { 'resize': { fn: me.collapseMenu, scope: me } };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjNavigationPanel');
        view.ctxKey = 'realityobjectedit/' + id; // bindContext заменен на прямое определение ctxKey, 
                                                 // так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view);
    },

    collapseMenu: function () {
        var normalMonitorWidth = 1360;
        if (Ext.getBody().getViewSize().width < normalMonitorWidth) {
            this.getMenuTree().collapse();
        } else {
            this.getMenuTree().expand();
        }
    }
});
