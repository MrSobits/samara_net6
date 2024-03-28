Ext.define('B4.controller.paymentagent.Navigation', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    stores: ['paymentagent.NavigationMenu'],
    views: ['paymentagent.NavigationPanel'],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'paymentAgentNavigation',
            panelSelector: '#paymentAgentNavigationPanel',
            treeSelector: '#paymentAgentMenuTree',
            tabSelector: '#paymentAgentMainTab',
            storeName: 'paymentagent.NavigationMenu'
        }
    ],

    params: null,
    title: 'Платежный агент',

    mainView: 'paymentagent.NavigationPanel',
    mainViewSelector: '#paymentAgentNavigationPanel',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#paymentAgentMenuTree' },
        { ref: 'infoLabel', selector: '#paymentAgentInfoLabel' },
        { ref: 'mainTab', selector: '#paymentAgentMainTab' }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.get('CtrName') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('paymentAgentNavigation').reload();
        }
    }
});
