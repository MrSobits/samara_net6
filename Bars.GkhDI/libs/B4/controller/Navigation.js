Ext.define('B4.controller.Navigation', {
    extend: 'B4.base.Controller',
 views: [ 'NavigationPanel' ], 


    params: null,
    title: 'Редактирование жилого дома',

    mainView: 'NavigationPanel',
    mainViewSelector: '#disclosureInfoRealityObjNavigationPanel',

    containerSelector: '#disclosureInfoRealityObjMainTab',

    stores: ['menu.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#disclosureInfoRealityObjMenuTree' },
        { ref: 'infoLabel', selector: '#disclosureInfoRealityObjInfoLabel' },
        { ref: 'mainTab', selector: '#disclosureInfoRealityObjMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'disclosureInfoRealityObjNavigationAspect',
            panelSelector: '#disclosureInfoRealityObjNavigationPanel',
            treeSelector: '#disclosureInfoRealityObjMenuTree',
            tabSelector: '#disclosureInfoRealityObjMainTab',
            storeName: 'menu.NavigationMenu',
            getObjectId: function () {
                if (this.controller.params)
                    return this.controller.params.disclosureInfoRealityObjId;
                return null;
            },
            onBeforeLoad: function (store, operation) {
                if (this.controller.params) {
                    operation.params.disclosureInfoId = this.controller.params.disclosureInfoId;
                    operation.params.disclosureInfoRealityObjId = this.controller.params.disclosureInfoRealityObjId;
                }
            }
        }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.Address });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);
            //this.getAspect('disclosureInfoRealityObjNavigationAspect').reload();
        }
        this.getStore('menu.NavigationMenu').load();
    }
});
