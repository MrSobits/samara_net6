Ext.define('B4.controller.baseheatseason.Navigation', {
    extend: 'B4.base.Controller',
    
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    params: null,
    title: 'ГЖИ: Отопительный сезон',

    containerSelector: '#baseHeatSeasonMainTab',

    views: ['baseheatseason.NavigationPanel'],
    
    stores: ['baseheatseason.NavigationMenu'],
    
    mainView: 'baseheatseason.NavigationPanel',
    mainViewSelector: '#baseHeatSeasonNavigationPanel',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseHeatSeasonMenuTree' },
        { ref: 'infoLabel', selector: '#baseHeatSeasonInfoLabel' },
        { ref: 'mainTab', selector: '#baseHeatSeasonMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели отопительног осезона
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseHeatSeasonNavigationAspect',
            panelSelector: '#baseHeatSeasonNavigationPanel',
            treeSelector: '#baseHeatSeasonMenuTree',
            tabSelector: '#baseHeatSeasonMainTab',
            storeName: 'baseheatseason.NavigationMenu',
            paramName: 'inspectionId',
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
            if (label)
                label.update({ text: "Отопительный сезон" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseHeatSeasonNavigationAspect').reload();
        }
    }
});