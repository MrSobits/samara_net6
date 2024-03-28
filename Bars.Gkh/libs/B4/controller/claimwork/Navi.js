Ext.define('B4.controller.claimwork.Navi', {
    extend: 'B4.base.Controller',
    views: ['claimwork.NavigationPanel'],

    params: {},
    title: 'Претензионно-исковая работа',

    requires: [
        'B4.aspects.TreeNavigationMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'claimworknavigationpanel' },
        { ref: 'menuTree', selector: 'claimworknavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'claimworknavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'claimworknavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeClaimworkNavigationAspect',
            mainPanel: 'claimworknavigationpanel',
            menuContainer: 'claimworknavigationpanel menutreepanel',
            tabContainer: 'claimworknavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'claimworknavigationpanel breadcrumbs',
            deployKey: 'claim_work',
            contextKey: 'claimWorkId',
            contextType: 'type',
            onBeforeLoad: function (store, operation) {
                var me = this,
                    view = me.controller.getMainView(),
                    objectId = me.controller.getContextValue(view, me.contextKey),
                    type = me.controller.getContextValue(view, me.contextType);

                operation.params = operation.params || {};
                operation.params['type'] = type;
                operation.params['objectId'] = objectId;
            },
            prepareTitle: function (comp) {
                // пока непонятно как проставлять Заголовок, поскольку у всех оснований ПИР свои поля которые никак не обобщаются 
            },
            onMenuItemClick: function (view, record) {
                var me = this,
                    objectId,
                    type,
                    docId;

                if (record.get('leaf')) {
                    objectId = me.controller.getContextValue(view, me.contextKey);
                    type = me.controller.getContextValue(view, me.contextType);
                    docId = record.get('options').docId;

                    me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), type, objectId, docId, (record.get('options').SectionId ? record.get('options').SectionId : undefined)));

                    me.setExtraParams(record);
                }
            }
        }
    ],

    index: function (type, id) {
        var me = this,
            view = me.getMainView() || Ext.widget('claimworknavigationpanel');

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}', type, id);
        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.application.deployView(view);
    },

    aftercreatedoc: function (type, claimWorkId, docId, docUrl) {
        var me = this;
        me.getAspect('treeClaimworkNavigationAspect').loadMenu();

        Ext.History.add(Ext.String.format('claimwork/{0}/{1}/{2}/{3}', type, claimWorkId, docId, docUrl));
    },

    updateNaviAspectMenu: function(type, claimWorkId, nextroute) {
        var asp = this.getAspect('treeClaimworkNavigationAspect');
        asp.updateMenu();
    },

    deletedocument: function (type, id) {
        var me = this,
            asp = me.getAspect('treeClaimworkNavigationAspect'),
            tab = asp.getTabContainer(),
            view = me.getMainView() || Ext.widget('claimworknavigationpanel'),
            typeName;

        switch(type) {
            case 'LegalClaimWork':  
                typeName = 'legaledit';
                break;
            case 'IndividualClaimWork':
                typeName = 'individualedit';
                break;
            case 'UtilityDebtorClaimWork': 
                typeName = 'utilitydebtoredit';
                break;
            default:
                typeName = 'buildctredit';    
                break;
        }

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}', type, id);
        me.setContextValue(view, 'type', type);
        me.setContextValue(view, 'id', id);
        me.application.deployView(view);

        tab.removeAll();
        asp.updateMenu();

        Ext.History.add(Ext.String.format('claimwork/{0}/{1}/{2}', type, id, typeName));
    }
});
