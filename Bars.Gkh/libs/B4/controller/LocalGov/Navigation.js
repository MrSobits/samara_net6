Ext.define('B4.controller.localgov.Navigation', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    stores: ['localgov.NavigationMenu'],
    views: ['localgov.NavigationPanel'],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'localGovNavigation',
            panelSelector: '#localGovNavigationPanel',
            treeSelector: '#localGovMenuTree',
            tabSelector: '#localGovMainTab',
            storeName: 'localgov.NavigationMenu'
        }
    ],

    params: null,
    title: 'Орган местного самоуправления',

    mainView: 'localgov.NavigationPanel',
    mainViewSelector: '#localGovNavigationPanel',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#localGovMenuTree' },
        { ref: 'infoLabel', selector: '#localGovInfoLabel' },
        { ref: 'mainTab', selector: '#localGovMainTab' }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.get('ContragentName') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('localGovNavigation').reload();
        }
    }
});
