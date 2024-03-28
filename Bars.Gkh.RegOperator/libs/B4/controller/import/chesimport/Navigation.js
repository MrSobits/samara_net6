Ext.define('B4.controller.import.chesimport.Navigation', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhNavigationPanel',
        'B4.aspects.TreeNavigationMenu'
    ],
    models: ['regop.ChargePeriod'],
    stores: ['import.chesimport.NavigationMenu'],
    views: ['import.chesimport.NavigationPanel'],
    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'chesimportnavigationpanel',
            menuContainer: 'chesimportnavigationpanel menutreepanel',
            tabContainer: 'chesimportnavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'chesimportnavigationpanel breadcrumbs',
            storeName: 'import.chesimport.NavigationMenu',
            deployKey: 'chesPeriodId_Info',
            contextKey: 'periodId',
            prepareTitle: function(comp) {
                var me = this,
                    periodId = me.getObjectId();

                me.controller.getModel('regop.ChargePeriod').load(periodId, {
                    success: function(rec) {
                        comp.update({ text: Ext.String.format('Период: {0}',  rec && rec.get('Name')) });
                    }
                });
            },
            loadMenu: function(menu) {
                var me = this,
                    treeMenu = menu || me.getMenu(),
                    store = treeMenu.getStore();

                store.on('beforeload', me.onBeforeLoad, me, { single: true });
                store.load();
            },
            onBeforeLoad: function(store, operation) {
                operation.params.menuName = 'ChesImport';
                operation.params.periodId = this.getObjectId();
            }
        }
    ],

    params: null,

    title: 'Импорт сведений из биллинга',

    mainViewSelector: 'chesimportnavigationpanel',
    mainView: 'import.chesimport.NavigationPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'chesimportnavigationpanel' },
        { ref: 'menuTree', selector: 'chesimportnavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'chesimportnavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'chesimportnavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('chesimportnavigationpanel');

        view.ctxKey = 'chesimport_detail/' + id; // bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url
        me.setContextValue(view, 'periodId', id);
        me.application.deployView(view);
    }
});
