Ext.define('B4.controller.claimwork.BuildContractNavi', {
    extend: 'B4.base.Controller',
    views: ['claimwork.BuildContractNavigationPanel'],

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
        { ref: 'mainView', selector: 'claimworkbcnavigationpanel' },
        { ref: 'menuTree', selector: 'claimworkbcnavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'claimworkbcnavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'claimworkbcnavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeClaimworkNavigationAspect',
            mainPanel: 'claimworkbcnavigationpanel',
            menuContainer: 'claimworkbcnavigationpanel menutreepanel',
            tabContainer: 'claimworkbcnavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'claimworkbcnavigationpanel breadcrumbs',
            deployKey: 'claim_work_bc',
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

                    me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), objectId, docId, (record.get('options').SectionId ? record.get('options').SectionId : undefined)));

                    me.setExtraParams(record);
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('claimworkbcnavigationpanel');

        view.ctxKey = Ext.String.format('claimworkbc/BuildContractClaimWork/{0}', id);
        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', 'BuildContractClaimWork');
        me.application.deployView(view);
    },

    aftercreatedoc: function (claimWorkId, docId, docUrl) {
        var me = this;
        me.getAspect('treeClaimworkNavigationAspect').loadMenu();

        Ext.History.add(Ext.String.format('claimworkbc/BuildContractClaimWork/{0}/{1}/{2}', claimWorkId, docId, docUrl));
    },

    updateNaviAspectMenu: function(claimWorkId, nextroute) {
        var asp = this.getAspect('treeClaimworkNavigationAspect');
        asp.updateMenu();
    },

    deletedocument: function (id) {
        var me = this,
            asp = me.getAspect('treeClaimworkNavigationAspect'),
            tab = asp.getTabContainer(),
            view = me.getMainView() || Ext.widget('claimworkbcnavigationpanel');

        view.ctxKey = Ext.String.format('claimworkbc/BuildContractClaimWork/{0}', id);
        me.setContextValue(view, 'type', 'BuildContractClaimWork');
        me.setContextValue(view, 'id', id);
        me.application.deployView(view);

        tab.removeAll();
        asp.updateMenu();

        Ext.History.add(Ext.String.format('claimworkbc/BuildContractClaimWork/{0}/{1}', id, 'buildctredit'));
    }
});
