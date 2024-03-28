Ext.define('B4.controller.resolpros.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['resolpros.NavigationMenu'],
    
    views: ['baseresolpros.NavigationPanel'],

    params: null,
    title: 'Постановление прокуратуры',

    mainView: 'baseresolpros.NavigationPanel',
    mainViewSelector: '#baseResolProsNavigationPanel',

    containerSelector: '#baseResolProsMainTab',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseResolProsMenuTree' },
        { ref: 'infoLabel', selector: '#baseResolProsInfoLabel' },
        { ref: 'mainTab', selector: '#baseResolProsMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели проверки по поручению руководства
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'resolProsNavigationAspect',
            panelSelector: '#baseResolProsNavigationPanel',
            treeSelector: '#baseResolProsMenuTree',
            tabSelector: '#baseResolProsMainTab',
            storeName: 'resolpros.NavigationMenu',
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
                label.update({ text: "Постановление прокуратуры" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('resolProsNavigationAspect').reload();
        }
    }
});