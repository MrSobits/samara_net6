Ext.define('B4.controller.manorglicense.NaviGis', {
    extend: 'B4.base.Controller',
    views: ['manorglicense.NavigationPanelGis'],

    params: {},
    title: 'Лицензия УО',

    stores: ['manorglicense.NavigationMenuGis'],
    requires: ['B4.aspects.TreeNavigationMenuGis'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'manorglicenseGisNavigationPanel' },
        { ref: 'menuTree', selector: 'manorglicenseGisNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'manorglicenseGisNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'manorglicenseGisNavigationPanel tabpanel[nId="navtabpanelgis"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenugisaspect',
            name: 'treeLicenseGisNavigationAspect',
            mainPanel: 'manorglicenseGisNavigationPanel',
            menuContainer: 'manorglicenseGisNavigationPanel menutreepanel',
            tabContainer: 'manorglicenseGisNavigationPanel tabpanel[nId="navtabpanelgis"]',
            breadcrumbs: 'manorglicenseGisNavigationPanel breadcrumbs',
         //   storeName: 'manorglicense.NavigationMenuGis',
            deployKey: 'licensegis_info',
            contextKey: 'mcid',
          //  contextType: 'type',
            onMenuItemClick: function (view, record) {
                var me = this,
                    objectId,
                    type;
               
                if (record.get('leaf')) {
                    objectId = me.controller.getContextValue(view, me.contextKey);
                    me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), objectId));
                    me.setExtraParams(record);
                }
            },
            onBeforeLoad: function (store, operation) {
                var me = this,
                    view = me.controller.getMainView(),
                    objectId = me.controller.getContextValue(view, me.contextKey);
                
                operation.params = operation.params || {};
                operation.params["objectId"] = objectId;
            },
            prepareTitle: function (comp) {
                // пока непонятно что тут должно отображатся
            },
            loadMenu: function (menu) {
                var me = this,
                    treeMenu = menu || me.getMenu(),
                    store = treeMenu.getStore(),
                    view = me.controller.getMainView(),
                    objectId = me.controller.getContextValue(view, me.contextKey);

                if (objectId > 0) {
                    store.on('beforeload', me.onBeforeLoad, me, { single: true });
                    store.load();
                    treeMenu.setLoading(true, true);
                }
            },
            afterMenuLoad: function () {
                var me = this;
                // Перекрыл метд потому что в данную карточку осуществляется переход из 2х реестров и следовательно 
                // там происходят непонятные вещи когда открывается из реестра лицензий то 2 вкладки сразу открываются
                me.getMenu().setLoading(false);
            }
            
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('manorglicenseGisNavigationPanel');
        view.ctxKey = 'manorglicensegis/' + id; // bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'mcid', id);
        me.application.deployView(view);
    }

    
   
});
