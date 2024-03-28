Ext.define('B4.controller.manorg.Navigation', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhNavigationPanel',
        'B4.aspects.TreeNavigationMenu',
        'B4.enums.TypeContractManOrgRealObj'
    ],
    stores: ['manorg.NavigationMenu'],
    views: ['manorg.NavigationPanel'],
    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'manorgnavigationpanel',
            menuContainer: 'manorgnavigationpanel menutreepanel',
            tabContainer: 'manorgnavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'manorgnavigationpanel breadcrumbs',
            storeName: 'manorg.NavigationMenu',
            deployKey: 'manorgId_info',
            contextKey: 'manorgId',
            onMenuItemClick: function (view, record) {
                var me = this,
                    objectId;
                if (record.get('leaf')) {
                    objectId = me.controller.getContextValue(view, me.contextKey);
                    me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), objectId, (record.get('options').SectionId ? record.get('options').SectionId : undefined)));
                    me.setExtraParams(record);
                }
            },

            prepareTitle: function (comp) {
                // пока непонятно как проставлять Заголовок, поскольку у всех оснований ПИР свои поля которые никак не обобщаются 
            },
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

    params: null,
    title: 'Управляющая организация',

    /*mainView: 'manorg.NavigationPanel',
    mainViewSelector: 'manorgnavigationpanel',*/
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'manorgnavigationpanel' },
        { ref: 'menuTree', selector: 'manorgnavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'manorgnavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'manorgnavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('manorgnavigationpanel');
        view.ctxKey = 'managingorganizationedit/' + id; // bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view);
    }
});
