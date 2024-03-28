Ext.define('B4.controller.competition.Navi', {
    extend: 'B4.base.Controller',
    views: ['competition.NavigationPanel'],

    title: 'Конкурс',

    stores: ['competition.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'competitionNavigationPanel' },
        { ref: 'menuTree', selector: 'competitionNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'competitionNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'competitionNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'competitionNavigationPanel',
            menuContainer: 'competitionNavigationPanel menutreepanel',
            tabContainer: 'competitionNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'competitionNavigationPanel breadcrumbs',
            storeName: 'competition.NavigationMenu',
            deployKey: 'competition_info',
            contextKey: 'competitionId',
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
                    url: B4.Url.action('get', 'competition'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.NotifNumber) {
                        comp.update({ text: 'Номер извещения: '+data.data.NotifNumber });
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
        var view = me.getMainView() || Ext.widget('competitionNavigationPanel');
        view.ctxKey = 'competitionedit/' + id; // bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'competitionId', id);
        me.application.deployView(view);
    }
});