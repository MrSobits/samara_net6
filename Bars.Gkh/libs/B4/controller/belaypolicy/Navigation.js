Ext.define('B4.controller.belaypolicy.Navigation', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhNavigationPanel'],
    stores: ['belaypolicy.NavigationMenu'],
    views: ['belaypolicy.NavigationPanel'],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'belayPolicyNavigation',
            panelSelector: '#belayPolicyNavigationPanel',
            treeSelector: '#belayPolicyMenuTree',
            tabSelector: '#belayPolicyMainTab',
            storeName: 'belaypolicy.NavigationMenu'
        }
    ],

    params: null,
    title: 'Страховой полис',

    mainView: 'belaypolicy.NavigationPanel',
    mainViewSelector: '#belayPolicyNavigationPanel',
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#belayPolicyMenuTree' },
        { ref: 'infoLabel', selector: '#belayPolicyInfoLabel' },
        { ref: 'mainTab', selector: '#belayPolicyMainTab' }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if(label)
                label.update({ text: 'Страховой полис' });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('belayPolicyNavigation').reload();
        }
    }
});