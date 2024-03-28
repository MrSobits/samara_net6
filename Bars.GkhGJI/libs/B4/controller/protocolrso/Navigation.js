Ext.define('B4.controller.protocolrso.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['protocolrso.NavigationMenu'],
    
    views: ['baseprotocolrso.NavigationPanel'],

    params: null,
    title: 'Протокол РСО',

    mainView: 'baseprotocolrso.NavigationPanel',
    mainViewSelector: '#baseProtocolRSONavigationPanel',

    containerSelector: '#baseProtocolRSOMainTab',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseProtocolRSOMenuTree' },
        { ref: 'infoLabel', selector: '#baseProtocolRSOInfoLabel' },
        { ref: 'mainTab', selector: '#baseProtocolRSOMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'protocolRSONavigationAspect',
            panelSelector: '#baseProtocolRSONavigationPanel',
            treeSelector: '#baseProtocolRSOMenuTree',
            tabSelector: '#baseProtocolRSOMainTab',
            storeName: 'protocolrso.NavigationMenu',
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
                label.update({ text: "Протокол РСО" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('protocolRSONavigationAspect').reload();
        }
    }
});