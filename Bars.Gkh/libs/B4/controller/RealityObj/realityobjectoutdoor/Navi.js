Ext.define('B4.controller.realityobj.realityobjectoutdoor.Navi', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.TreeNavigationMenu',
        'B4.view.realityobj.realityobjectoutdoor.NavigationPanel'
    ],

    views: ['realityobj.realityobjectoutdoor.NavigationPanel'],

    params: {},
    title: 'Двор',

    stores: ['realityobj.RealityObjectOutdoorNavigationMenu'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'realityobjectoutdoornavigationpanel' },
        { ref: 'menuTree', selector: 'realityobjectoutdoornavigationpanel menutreepanel' },
        { ref: 'infoLabel', selector: 'realityobjectoutdoornavigationpanel breadcrumbs' },
        { ref: 'mainTab', selector: 'realityobjectoutdoornavigationpanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'outdoorNavigationAspect',
            mainPanel: 'realityobjectoutdoornavigationpanel',
            menuContainer: 'realityobjectoutdoornavigationpanel menutreepanel',
            tabContainer: 'realityobjectoutdoornavigationpanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'realityobjectoutdoornavigationpanel breadcrumbs',
            storeName: 'realityobj.RealityObjectOutdoorNavigationMenu',
            contextKey: 'outdoorId',
            deployKey: 'realityobject_outdoor_info',

            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('get', 'realityobjectoutdoor'),
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
            view = me.getMainView() || Ext.widget('realityobjectoutdoornavigationpanel');
        view.ctxKey = 'realityobjectoutdooredit/' + id;// bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url

        me.setContextValue(view, 'outdoorId', id);
        me.application.deployView(view);
    }
});