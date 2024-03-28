Ext.define('B4.controller.belayorg.Navigation', {
/*
* Контроллер навигационной панели страховых орг
*/
    extend: 'B4.base.Controller',
 views: [ 'belayorg.NavigationPanel' ], 


    params: null,
    title: 'Страховая организация',

    mainView: 'belayorg.NavigationPanel',
    mainViewSelector: '#belayOrgNavigationPanel',

    stores: ['belayorg.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#belayOrgMenuTree' },
        { ref: 'infoLabel', selector: '#belayOrgInfoLabel' },
        { ref: 'mainTab', selector: '#belayOrgMainTab' }
    ],

    aspects: [
        {
            /*
            * Аспект панели навигации раздела страховых орг
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'belayOrgNavigationAspect',
            panelSelector: '#belayOrgNavigationPanel',
            treeSelector: '#belayOrgMenuTree',
            tabSelector: '#belayOrgMainTab',
            storeName: 'belayorg.NavigationMenu'
        }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.get('ContragentName') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('belayOrgNavigationAspect').reload();
        }
    }
});
