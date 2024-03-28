Ext.define('B4.controller.protocolmhc.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['protocolmhc.NavigationMenu'],
    
    views: ['baseprotocolmhc.NavigationPanel'],

    params: null,
    title: 'Протокол МЖК',

    mainView: 'baseprotocolmhc.NavigationPanel',
    mainViewSelector: '#baseProtocolMhcNavigationPanel',

    containerSelector: '#baseProtocolMhcMainTab',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseProtocolMhcMenuTree' },
        { ref: 'infoLabel', selector: '#baseProtocolMhcInfoLabel' },
        { ref: 'mainTab', selector: '#baseProtocolMhcMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'protocolMhcNavigationAspect',
            panelSelector: '#baseProtocolMhcNavigationPanel',
            treeSelector: '#baseProtocolMhcMenuTree',
            tabSelector: '#baseProtocolMhcMainTab',
            storeName: 'protocolmhc.NavigationMenu',
            paramName: 'inspectionId',
            getObjectId: function () {
                if (this.controller.params && this.controller.params.get) {
                    if (this.controller.params.get('InspectionId')) {
                        return this.controller.params.get('InspectionId');
                    } else {
                        return this.controller.params.get('Id');
                    }
                }
                return null;
            },
            getParams: function (menuRecord) {
                //перекрываем метод для того чтобы сделать свои параметры
                var params = menuRecord.get('options');
                params.containerSelector = this.tabSelector;
                params.treeMenuSelector = this.treeSelector;
                
                return params;
            }
        }
    ],

    onLaunch: function () {
        if (this.params) {

            var label = this.getInfoLabel();
            if(label)
                label.update({ text: "Протокол МЖК" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('protocolMhcNavigationAspect').reload();
        }
    }
});