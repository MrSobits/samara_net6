Ext.define('B4.controller.objectoutdoorcr.Navi', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.TreeNavigationMenu',
        'B4.view.objectoutdoorcr.NavigationPanel'
    ],

    views: [
        'objectoutdoorcr.NavigationPanel'
    ],

    params: {},
    title: 'Программа благоустройства',

    stores: [
        'objectoutdoorcr.ObjectOutdoorCrNavigationMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'objectoutdoorcrnavigationpanel' },
        { ref: 'menuTree', selector: 'objectoutdoorcrnavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'objectoutdoorcrnavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'objectoutdoorcrnavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'objectOutdoorCrNavigationAspect',
            mainPanel: 'objectoutdoorcrnavigationpanel',
            menuContainer: 'objectoutdoorcrnavigationpanel menutreepanel',
            tabContainer: 'objectoutdoorcrnavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'objectoutdoorcrnavigationpanel breadcrumbs',
            storeName: 'objectoutdoorcr.ObjectOutdoorCrNavigationMenu',
            contextKey: 'objectOutdoorCrId',
            deployKey: 'object_outdoor_cr_info',

            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('get', 'objectoutdoorcr'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.RealityObjectOutdoorName) {
                        comp.update({ text: data.data.RealityObjectOutdoorName });
                    }
                });
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('objectoutdoorcrnavigationpanel');
        view.ctxKey = 'objectoutdoorcredit/' + id;// bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url

        me.setContextValue(me.getMainView(), 'objectOutdoorCrId', id);
        me.application.deployView(view);
    }
});