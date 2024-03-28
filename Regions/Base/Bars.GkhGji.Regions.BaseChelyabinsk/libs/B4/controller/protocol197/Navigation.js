Ext.define('B4.controller.protocol197.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['protocol197.NavigationMenu'],
    
    views: ['baseprotocol197.NavigationPanel'],

    params: null,
    title: 'Протокол',

    mainView: 'baseprotocol197.NavigationPanel',
    mainViewSelector: '#baseProtocol197NavigationPanel',

    containerSelector: '#baseProtocol197MainTab',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseProtocol197MenuTree' },
        { ref: 'infoLabel', selector: '#baseProtocol197InfoLabel' },
        { ref: 'mainTab', selector: '#baseProtocol197MainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'protocol197NavigationAspect',
            panelSelector: '#baseProtocol197NavigationPanel',
            treeSelector: '#baseProtocol197MenuTree',
            tabSelector: '#baseProtocol197MainTab',
            storeName: 'protocol197.NavigationMenu',
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
                label.update({ text: "Протокол" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('protocol197NavigationAspect').reload();
        }
    }
});