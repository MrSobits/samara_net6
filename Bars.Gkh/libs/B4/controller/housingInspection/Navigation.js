Ext.define('B4.controller.housinginspection.Navigation', {
    extend: 'B4.base.Controller',
    views: ['housinginspection.NavigationPanel'],

    title: 'Жилищная инспекция',

    mainView: 'housinginspection.NavigationPanel',
    mainViewSelector: 'housinginspectionNavigationPanel',

    stores: ['housinginspection.NavigationMenu'],
    requires: ['B4.aspects.TreeNavigationMenu'],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'housinginspectionNavigationPanel' },
        { ref: 'menuTree', selector: 'housinginspectionNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'housinginspectionNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'housinginspectionNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'housinginspectionNavigationPanel',
            menuContainer: 'housinginspectionNavigationPanel menutreepanel',
            tabContainer: 'housinginspectionNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'housinginspectionNavigationPanel breadcrumbs',
            deployKey: 'housinginspection_info',
            contextKey: 'housinginspectionId',
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('Get', 'HousingInspection'),
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
        view = me.getMainView() || Ext.widget('housinginspectionNavigationPanel');
        me.bindContext(view);
        me.setContextValue(view, 'housinginspectionId', id);
        me.application.deployView(view);
    }
});