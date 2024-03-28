Ext.define('B4.controller.contragent.Navigation', {
    extend: 'B4.base.Controller',
    views: ['contragent.NavigationPanel'],

    params: null,
    title: 'Контрагент',

    mainView: 'contragent.NavigationPanel',
    mainViewSelector: '#contragentNavigationPanel',

    stores: ['contragent.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#contragentMenuTree' },
        { ref: 'infoLabel', selector: '#contragentInfoLabel' },
        { ref: 'mainTab', selector: '#contragentMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'contragentNavigationAspect',
            panelSelector: '#contragentNavigationPanel',
            treeSelector: '#contragentMenuTree',
            tabSelector: '#contragentMainTab',
            storeName: 'contragent.NavigationMenu'
        }
    ],

    onLaunch: function () {
        var me = this;
        
        if (me.params) {
            var label = me.getInfoLabel();
            if (label)
                label.update({ text: me.params.Name || me.params.get('Name') });

            var mainView = me.getMainComponent();
            if (mainView)
                mainView.setTitle(me.title);

            me.getAspect('contragentNavigationAspect').reload();
        }
    }
});