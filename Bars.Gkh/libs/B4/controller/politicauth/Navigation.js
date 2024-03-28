Ext.define('B4.controller.politicauth.Navigation', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    stores: ['politicauth.NavigationMenu'],
    views: ['politicauth.NavigationPanel'],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'politicAuthNavigation',
            panelSelector: '#politicAuthNavigationPanel',
            treeSelector: '#politicAuthMenuTree',
            tabSelector: '#politicAuthMainTab',
            storeName: 'politicauth.NavigationMenu'
        }
    ],

    params: null,
    title: 'Орган государственной власти',

    mainView: 'politicauth.NavigationPanel',
    mainViewSelector: '#politicAuthNavigationPanel',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#politicAuthMenuTree' },
        { ref: 'infoLabel', selector: '#politicAuthInfoLabel' },
        { ref: 'mainTab', selector: '#politicAuthMainTab' }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.get('ContragentName') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('politicAuthNavigation').reload();
        }
    }
});
