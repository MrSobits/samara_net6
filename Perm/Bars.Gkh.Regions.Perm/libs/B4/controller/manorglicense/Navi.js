Ext.define('B4.controller.manorglicense.Navi', {
    extend: 'B4.base.Controller',
    views: ['manorglicense.NavigationPanel'],

    params: {},
    title: 'Лицензия УО',

    stores: ['manorglicense.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'manorglicenseNavigationPanel' },
        { ref: 'menuTree', selector: 'manorglicenseNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'manorglicenseNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'manorglicenseNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeLicenseNavigationAspect',
            mainPanel: 'manorglicenseNavigationPanel',
            menuContainer: 'manorglicenseNavigationPanel menutreepanel',
            tabContainer: 'manorglicenseNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'manorglicenseNavigationPanel breadcrumbs',
            //storeName: 'manorglicense.NavigationMenu',
            deployKey: 'license_info',
            contextKey: 'id',
            contextType: 'type',
            onMenuItemClick: function (view, record) {
                var me = this,
                    objectId,
                    type;
                
                if (record.get('leaf')) {
                    objectId = me.controller.getContextValue(view, me.contextKey);
                    type = me.controller.getContextValue(view, me.contextType);
                    me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), type, objectId, (record.get('options').SectionId ? record.get('options').SectionId : undefined)));
                    me.setExtraParams(record);
                }
            },
            onBeforeLoad: function (store, operation) {
                var me = this,
                    view = me.controller.getMainView(),
                    objectId = me.controller.getContextValue(view, me.contextKey),
                    type = me.controller.getContextValue(view, me.contextType);
                
                operation.params = operation.params || {};
                operation.params["type"] = type;
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

    index: function (type, id) {
        var me = this;
        var view = me.getMainView() || Ext.widget('manorglicenseNavigationPanel');
        view.ctxKey = Ext.String.format('manorglicense/{0}/{1}', type, id); // bindContext заменен на прямое определение ctxKey, 

        me.setContextValue(view, 'type', type);// так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'id', id);
        me.application.deployView(view);
    },
    
    addlicense: function (type, id) {
        var me = this;

        me.getAspect('treeLicenseNavigationAspect').loadMenu();
        
        Ext.History.add(Ext.String.format("manorglicense/{0}/{1}/editrequest", type, id));
    },
    
    deletelicense: function (type, id) {
        var me = this,
            asp = me.getAspect('treeLicenseNavigationAspect'),
            tab = asp.getTabContainer(),
            view = me.getMainView() || Ext.widget('manorglicenseNavigationPanel');

        view.ctxKey = Ext.String.format('manorglicense/{0}/{1}', type, id); // bindContext заменен на прямое определение ctxKey, 
        me.setContextValue(view, 'type', type);// так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'id', id);
        me.application.deployView(view);

        tab.removeAll();
        asp.loadMenu();

        Ext.History.add(Ext.String.format("manorglicense/{0}/{1}/editrequest", type, id));
    }
});
