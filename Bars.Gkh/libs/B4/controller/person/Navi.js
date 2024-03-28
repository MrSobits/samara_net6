Ext.define('B4.controller.person.Navi', {
    extend: 'B4.base.Controller',
    views: ['person.NavigationPanel'],

    title: 'Должностное лицо',

    stores: ['person.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'personNavigationPanel' },
        { ref: 'menuTree', selector: 'personNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'personNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'personNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'personNavigationPanel',
            menuContainer: 'personNavigationPanel menutreepanel',
            tabContainer: 'personNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'personNavigationPanel breadcrumbs',
            storeName: 'person.NavigationMenu',
            deployKey: 'person_info',
            contextKey: 'personId',
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
                B4.Ajax.request({
                    url: B4.Url.action('get', 'person'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.FullName) {
                        comp.update({ text: data.data.FullName });
                    }
                });
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

    index: function (id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('personNavigationPanel');
        view.ctxKey = 'personedit/' + id; // bindContext заменен на прямое определение ctxKey, 
                                                 // так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'personId', id);
        me.application.deployView(view);
    }
});
