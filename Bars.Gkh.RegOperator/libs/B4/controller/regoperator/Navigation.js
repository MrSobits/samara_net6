Ext.define('B4.controller.regoperator.Navigation', {
    extend: 'B4.base.Controller',
    views: ['contragent.NavigationPanel'],

    params: null,
    title: 'Региональный оператор',

    mainView: 'regoperator.NavigationPanel',
    mainViewSelector: '#regoperatorNavigationPanel',

    stores: ['regoperator.Navigation'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#regoperatorMenuTree' },
        { ref: 'infoLabel', selector: '#regoperatorInfoLabel' },
        { ref: 'mainTab', selector: '#regoperatorMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'navigationAspect',
            panelSelector: '#regoperatorNavigationPanel',
            treeSelector: '#regoperatorMenuTree',
            tabSelector: '#regoperatorMainTab',
            storeName: 'regoperator.Navigation'
        }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label) {
                var contragent = this.params.get('Contragent');
                label.update({
                    text: Ext.isString(contragent)
                        ? contragent
                        : Ext.isObject(contragent) ? contragent.Name : ""
                });
            }

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('navigationAspect').reload();
        }
    }
});