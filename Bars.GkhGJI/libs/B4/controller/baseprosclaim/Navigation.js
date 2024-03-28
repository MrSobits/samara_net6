Ext.define('B4.controller.baseprosclaim.Navigation', {
    extend: 'B4.base.Controller',

    params: null,
    title: 'Проверка по требованию прокуратуры',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['baseprosclaim.NavigationMenu'],
    
    views: ['baseprosclaim.NavigationPanel'],

    mainView: 'baseprosclaim.NavigationPanel',
    mainViewSelector: '#baseProsClaimNavigationPanel',

    containerSelector: '#baseProsClaimMainTab',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseProsClaimMenuTree' },
        { ref: 'infoLabel', selector: '#baseProsClaimInfoLabel' },
        { ref: 'mainTab', selector: '#baseProsClaimMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели проверки по требованию прокуратуры
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseProsClaimNavigationAspect',
            panelSelector: '#baseProsClaimNavigationPanel',
            treeSelector: '#baseProsClaimMenuTree',
            tabSelector: '#baseProsClaimMainTab',
            storeName: 'baseprosclaim.NavigationMenu',
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
            this.getInfoLabel().update({ text: "Требование прокуратуры" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseProsClaimNavigationAspect').reload();
        }
    }
});