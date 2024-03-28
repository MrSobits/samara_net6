Ext.define('B4.controller.supplyresourceorg.Navigation', {
    extend: 'B4.base.Controller',
    views: ['supplyresourceorg.NavigationPanel'],
    params: null,
    title: 'Поставщик коммунальных услуг',

    stores: ['supplyresourceorg.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'supplyresorgnavigationpanel' },
        { ref: 'menuTree', selector: 'supplyresorgnavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'supplyresorgnavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'supplyresorgnavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'supplyresorgnavigationpanel',
            menuContainer: 'supplyresorgnavigationpanel menutreepanel',
            tabContainer: 'supplyresorgnavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'supplyresorgnavigationpanel breadcrumbs',
            storeName: 'servorg.NavigationMenu',
            deployKey: 'supplyres_org',
            contextKey: 'supplyresorgId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('Get', 'SupplyResourceOrg'),
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
            view = me.getMainView() || Ext.widget('supplyresorgnavigationpanel');

        me.bindContext(view);
        me.setContextValue(view, 'supplyresorgId', id);
        me.application.deployView(view);
    }
});
