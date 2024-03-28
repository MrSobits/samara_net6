Ext.define('B4.controller.bankstatement.Navigation', {
   /*
   * Контроллер навигационной панели банковской выписки
   */
    extend: 'B4.base.Controller',
    
    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['bankstatement.NavigationMenu'],
    
    views: ['bankstatement.NavigationPanel'],

    params: null,
    title: 'Банковская выписка',

    mainView: 'bankstatement.NavigationPanel',
    mainViewSelector: '#bankStatementNavigationPanel',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#bankStatementMenuTree' },
        { ref: 'infoLabel', selector: '#bankStatementInfoLabel' },
        { ref: 'mainTab', selector: '#bankStatementMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'bankStatementNavigationAspect',
            panelSelector: '#bankStatementNavigationPanel',
            treeSelector: '#bankStatementMenuTree',
            tabSelector: '#bankStatementMainTab',
            storeName: 'bankstatement.NavigationMenu'
        }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.get('ObjectCrName') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('bankStatementNavigationAspect').reload();
        }
    }
});
