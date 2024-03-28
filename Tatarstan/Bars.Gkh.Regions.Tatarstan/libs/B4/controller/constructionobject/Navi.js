Ext.define('B4.controller.constructionobject.Navi', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.TreeNavigationMenu',
        'B4.view.constructionobject.NavigationPanel'
    ],

    views: ['constructionobject.NavigationPanel'],

    params: {},
    title: 'Жилой дом',

    stores: ['constructionobject.NavigationMenu'],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'constructionobjNavigationPanel' },
        { ref: 'menuTree', selector: 'constructionobjNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'constructionobjNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'constructionobjNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'constructionobjNavigationPanel',
            menuContainer: 'constructionobjNavigationPanel menutreepanel',
            tabContainer: 'constructionobjNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'constructionobjNavigationPanel breadcrumbs',
            storeName: 'constructionobject.NavigationMenu',
            deployKey: 'construction_object_info',
            contextKey: 'constructionObjectId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('get', 'constructionobject'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.Address) {
                        comp.update({ text: data.data.Address });
                    }
                });
            },
            onMenuItemClick: function (view, record) {
                var me = this,
                    objectId;
                if (record.get('moduleScript')) {
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

                treeMenu.setLoading(true, true);
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjNavigationPanel');
        view.ctxKey = 'constructionobjectedit/' + id;
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view);
    }
});
