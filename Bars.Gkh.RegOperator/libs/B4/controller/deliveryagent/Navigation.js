Ext.define('B4.controller.deliveryagent.Navigation', {
    extend: 'B4.base.Controller',
    views: ['deliveryagent.NavigationPanel'],

    params: {},
    title: 'Агент доставки',

    stores: ['deliveryagent.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'deliveryagentnavpanel' },
        { ref: 'menuTree', selector: 'deliveryagentnavpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'deliveryagentnavpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'deliveryagentnavpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'deliveryagentnavpanel',
            menuContainer: 'deliveryagentnavpanel menutreepanel',
            tabContainer: 'deliveryagentnavpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'deliveryagentnavpanel breadcrumbs',
            storeName: 'deliveryagent.NavigationMenu',
            deployKey: 'delivery_agent',
            contextKey: 'delAgentId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('get', 'deliveryagent'),
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
            view = me.getMainView() || Ext.widget('deliveryagentnavpanel');

        me.bindContext(view);
        me.setContextValue(view, 'delAgentId', id);
        me.application.deployView(view);
    }
});
