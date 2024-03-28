Ext.define('B4.controller.wastecollection.Navi', {
    extend: 'B4.base.Controller',
    params: {},
    title: 'Площадка сбора ТБО и ЖБО',

    stores: ['wastecollection.NavigationMenu'],
    views: ['wastecollection.NavigationPanel'],
    requires: [
        'B4.aspects.TreeNavigationMenu'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    refs: [
        { ref: 'mainView', selector: 'wastecollectionnavigationpanel' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeWasteCollectionNavigationAspect',
            mainPanel: 'wastecollectionnavigationpanel',
            menuContainer: 'wastecollectionnavigationpanel menutreepanel',
            tabContainer: 'wastecollectionnavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'wastecollectionnavigationpanel breadcrumbs',
            deployKey: 'waste_collection',
            contextKey: 'wastecollectionId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('Get', 'WasteCollectionPlace'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data) {
                        comp.update({ text: Ext.String.format('{0}, {1}', data.data.Municipality, data.data.Address)});
                    }
                });
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('wastecollectionnavigationpanel');

        me.bindContext(view);
        me.setContextValue(view, 'wastecollectionId', id);
        me.application.deployView(view);
    }
});