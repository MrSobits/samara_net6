Ext.define('B4.controller.servorg.Navigation', {
    extend: 'B4.base.Controller',
    views: ['servorg.NavigationPanel'],

    params: null,
    title: 'Поставщик жилищных услуг',

    stores: ['servorg.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'servorgnavigationpanel' },
        { ref: 'menuTree', selector: 'servorgnavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'servorgnavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'servorgnavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'servorgnavigationpanel',
            menuContainer: 'servorgnavigationpanel menutreepanel',
            tabContainer: 'servorgnavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'servorgnavigationpanel breadcrumbs',
            storeName: 'servorg.NavigationMenu',
            deployKey: 'serv_org',
            contextKey: 'servorgId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('Get', 'ServiceOrganization'),
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
            view = me.getMainView() || Ext.widget('servorgnavigationpanel');

        me.bindContext(view);
        me.setContextValue(view, 'servorgId', id);
        me.application.deployView(view);
    }
});
