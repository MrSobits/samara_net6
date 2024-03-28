Ext.define('B4.controller.objectcr.Navi', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.TreeNavigationMenu',
        'B4.view.objectcr.NavigationPanel'
    ],

    views: ['objectcr.NavigationPanel'],

    params: {},
    title: 'Объект КР',

    stores: ['objectcr.NavigationMenu'],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'objectcrNavigationPanel' },
        { ref: 'menuTree', selector: 'objectcrNavigationPanel menutreepanel' },
        { ref: 'infoLabel', selector: 'objectcrNavigationPanel breadcrumbs' },
        { ref: 'mainTab', selector: 'objectcrNavigationPanel tabpanel[nId="navtabpanel"]' }
    ],

    aspects: [
        {
            xtype: 'treenavigationmenuaspect',
            name: 'treeNavigationAspect',
            mainPanel: 'objectcrNavigationPanel',
            menuContainer: 'objectcrNavigationPanel menutreepanel',
            tabContainer: 'objectcrNavigationPanel tabpanel[nId="navtabpanel"]',
            breadcrumbs: 'objectcrNavigationPanel breadcrumbs',
            storeName: 'objectcr.NavigationMenu',
            deployKey: 'objectcr_info',
            contextKey: 'objectcrId',
            onMenuLoad: function (store) {
                var me = this;
                var nodes = me.controller.getMenuTree().getView().getNodes();
                if (nodes[0]) {
                    var view = me.controller.getMenuTree().getView();
                    var rec = view.getRecord(nodes[0]);

                    if (rec.get('text') == 'Разделы отсутствуют') {
                        //Если разделы отсутсвуют то закрываем навигационную панель
                        this.close();
                    }
                    else {
                        if (this.controller.params.showPassportObject) {
                            if (me.objectId != me.getObjectId()) {
                                me.objectId = me.getObjectId();
                                this.onMenuItemClick(view, rec);
                            }
                        }
                    }
                }
            },
            prepareTitle: function (comp) {
                B4.Ajax.request({
                    url: B4.Url.action('get', 'objectcr'),
                    method: 'POST',
                    params: { id: this.getObjectId() }
                }).next(function (response) {
                    var data = Ext.decode(response.responseText);
                    if (data.data && data.data.RealityObject.Address) {
                        comp.update({ text: data.data.RealityObject.Address });
                    }
                });
            },

            onMenuItemClick: function (view, record) {
                var me = this,
                    objectId;
                if (record.get('moduleScript')) {
                    objectId = me.controller.getContextValue(view, me.contextKey);
                    me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), objectId));
                    me.setExtraParams(record);
                }
            }
        }
    ],


    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('objectcrNavigationPanel');
        view.ctxKey = 'objectcredit/' + id;// bindContext заменен на прямое определение ctxKey, 
        // так как view может быть добавлено из дочернего url

        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view);
    }
});