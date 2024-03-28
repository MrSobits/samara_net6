Ext.define('B4.controller.contragentclw.Navigation', {
    extend: 'B4.base.Controller',
    views: ['contragentclw.NavigationPanel'],

    params: {},
    title: 'Контрагент ПИР',

    stores: ['contragentclw.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'contragentclwnavpanel' },
        { ref: 'menuTree', selector: 'contragentclwnavpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'contragentclwnavpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'contragentclwnavpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'contragentclwnavpanel',
            menuContainer: 'contragentclwnavpanel menutreepanel',
            tabContainer: 'contragentclwnavpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'contragentclwnavpanel breadcrumbs',
            storeName: 'contragentclw.NavigationMenu',
            deployKey: 'contragent_clw',
            contextKey: 'contragentClwId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('get', 'contragentclw'),
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
            view = me.getMainView() || Ext.widget('contragentclwnavpanel');

        me.bindContext(view);
        me.setContextValue(view, 'contragentClwId', id);
        me.application.deployView(view);
    }
});
