Ext.define('B4.controller.publicservorg.Navigation', {
    extend: 'B4.base.Controller',

    views: ['publicservorg.NavigationPanel'],
    
    params: {},
    title: 'Поставщик ресурсов',

    stores: ['publicservorg.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'publicservorgnavigationpanel' },
        { ref: 'menuTree', selector: 'publicservorgnavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'publicservorgnavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'publicservorgnavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'publicservorgnavigationpanel',
            menuContainer: 'publicservorgnavigationpanel menutreepanel',
            tabContainer: 'publicservorgnavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'publicservorgnavigationpanel breadcrumbs',
            storeName: 'publicservorg.NavigationMenu',
            deployKey: 'public_servorg',
            contextKey: 'publicservorgid',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('Get', 'PublicServiceOrg'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.Contragent && data.data && data.data.Contragent.Name) {
                        comp.update({
                            text: data.data.Contragent.Name
                    });
                    }
                });
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('publicservorgnavigationpanel');

        me.bindContext(view);
        me.setContextValue(view, 'publicservorgid', id);
        me.application.deployView(view);
    }
});
