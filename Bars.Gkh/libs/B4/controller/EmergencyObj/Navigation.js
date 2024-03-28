Ext.define('B4.controller.emergencyobj.Navigation', {
/*
* Контроллер навигационной панели аварийного дома
*/
    extend: 'B4.base.Controller',
 views: [ 'emergencyobj.NavigationPanel' ], 

    params: null,
    title: 'Аварийный дом',

    mainView: 'emergencyobj.NavigationPanel',
    mainViewSelector: '#emergencyObjNavigationPanel',

    stores: ['emergencyobj.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#emergencyObjMenuTree' },
        { ref: 'infoLabel', selector: '#emergencyObjInfoLabel' },
        { ref: 'mainTab', selector: '#emergencyObjMainTab' }
    ],

    aspects: [
        {
            /*
            * Аспект панели навигации раздела аварийного дома
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'emergencyObjNavigationAspect',
            panelSelector: '#emergencyObjNavigationPanel',
            treeSelector: '#emergencyObjMenuTree',
            tabSelector: '#emergencyObjMainTab',
            storeName: 'emergencyobj.NavigationMenu'
        }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.get('AddressName') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('emergencyObjNavigationAspect').reload();
        }
    }
});
