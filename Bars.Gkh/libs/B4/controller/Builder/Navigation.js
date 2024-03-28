Ext.define('B4.controller.builder.Navigation', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhNavigationPanel'],
    stores: ['builder.NavigationMenu'],
    views: ['builder.NavigationPanel'],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'builderNavigation',
            panelSelector: '#builderNavigationPanel',
            treeSelector: '#builderMenuTree',
            tabSelector: '#builderMainTab',
            storeName: 'builder.NavigationMenu'
        }
    ],

    params: null,
    title: 'Подрядная организация',

    mainView: 'builder.NavigationPanel',
    mainViewSelector: '#builderNavigationPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#builderMenuTree' },
        { ref: 'infoLabel', selector: '#builderInfoLabel' },
        { ref: 'mainTab', selector: '#builderMainTab' }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.get('ContragentName') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('builderNavigation').reload();
        }
    }
});
