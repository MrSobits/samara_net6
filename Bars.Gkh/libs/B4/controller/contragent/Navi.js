Ext.define('B4.controller.contragent.Navi', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.TreeNavigationMenu',
        'B4.view.contragent.NavigationPanel'
    ],

    views: ['contragent.NavigationPanel'],

    params: {},
    title: 'Контрагент',

    stores: ['contragent.NavigationMenu'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'contragentNavigationPanel' },
        { ref: 'menuTree', selector: 'contragentNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'contragentNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'contragentNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'contragentNavigationPanel',
            menuContainer: 'contragentNavigationPanel menutreepanel',
            tabContainer: 'contragentNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'contragentNavigationPanel breadcrumbs',
            storeName: 'contragent.NavigationMenu',
            deployKey: 'contragent_info',
            contextKey: 'contragentId',

            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('get', 'contragent'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.Name) {
                        comp.update({ text: data.data.Name });
                    }
                });
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentNavigationPanel');
        view.ctxKey = 'contragentedit/' + id;// bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url

        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view);
    }
});