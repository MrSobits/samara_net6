Ext.define('B4.controller.gasequipmentorg.Navigation', {
    extend: 'B4.base.Controller',
    views: ['gasequipmentorg.NavigationPanel'],

    title: 'ВДГО',

    mainView: 'gasequipmentorg.NavigationPanel',
    mainViewSelector: 'gasequipmentorgNavigationPanel',

    stores: ['gasequipmentorg.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'gasequipmentorgNavigationPanel' },
        { ref: 'menuTree', selector: 'gasequipmentorgNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'gasequipmentorgNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'gasequipmentorgNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'gasequipmentorgNavigationPanel',
            menuContainer: 'gasequipmentorgNavigationPanel menutreepanel',
            tabContainer: 'gasequipmentorgNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'gasequipmentorgNavigationPanel breadcrumbs',
            deployKey: 'gasequipmentorg_info',
            contextKey: 'gasequipmentorgId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('Get', 'gasequipmentorg'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.Contragent) {
                        comp.update({ text: data.data.Contragent });
                    }
                });
            }
        }
    ],

    index: function (id) {
        var me = this,
        view = me.getMainView() || Ext.widget('gasequipmentorgNavigationPanel');
        me.bindContext(view);
        me.setContextValue(view, 'gasequipmentorgId', id);
        me.application.deployView(view);
    }
});