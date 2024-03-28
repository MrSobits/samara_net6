Ext.define('B4.controller.workscr.Navi', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.TreeNavigationMenu'],

    params: {},
    title: 'Объект КР (Работы)',

    stores: ['workscr.Navi'],

    views: ['workscr.NaviPanel'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'workscrnavipanel' },
        { ref: 'menuTree', selector: 'workscrnavipanel menutreepanel' },
        { ref: 'infoLabel', selector: 'workscrnavipanel breadcrumbs' },
        { ref: 'mainTab', selector: 'workscrnavipanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'workscrnavipanel',
            menuContainer: 'workscrnavipanel menutreepanel',
            tabContainer: 'workscrnavipanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'workscrnavipanel breadcrumbs',
            storeName: 'workscr.Navi',
            deployKey: 'works_cr_info',
            contextKey: 'twId',
            onMenuItemClick: function(view, record) {
                var me = this,
                    twId, objectId;
                if (record.get('leaf')) {
                    twId = me.controller.getContextValue(view, 'twId');
                    objectId = me.controller.getContextValue(view, 'objectId');
                    me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), twId, objectId, (record.get('options').SectionId ? record.get('options').SectionId : undefined)));
                    me.setExtraParams(record);
                }
            },
            prepareTitle: function (comp) {},
            loadMenu: function (menu) {
                var me = this,
                    treeMenu = menu || me.getMenu(),
                    store = treeMenu.getStore();

                store.on('beforeload', me.onBeforeLoad, me, { single: true });
                store.load();

                treeMenu.setLoading(true, true);
            }
        }
    ],

    index: function (id, objectId) {
        var me = this,
            view = me.getMainView() || Ext.widget('workscrnavipanel');
        view.ctxKey = 'workscredit/' + id; // bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view);
    }
});