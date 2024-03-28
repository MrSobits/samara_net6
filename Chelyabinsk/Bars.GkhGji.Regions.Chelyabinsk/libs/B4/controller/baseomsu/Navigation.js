Ext.define('B4.controller.baseomsu.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    params: null,
    title: 'Плановая проверка органа местного самоуправления',

    stores: ['baseomsu.NavigationMenu'],

    views: ['baseomsu.NavigationPanel'],

    mainView: 'baseomsu.NavigationPanel',
    mainViewSelector: '#baseOMSUNavigationPanel',

    containerSelector: '#baseOMSUMainTab',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseOMSUMenuTree' },
        { ref: 'infoLabel', selector: '#baseOMSUInfoLabel' },
        { ref: 'mainTab', selector: '#baseOMSUMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели Плановой проверки юр лиц
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseOMSUNavigationAspect',
            panelSelector: '#baseOMSUNavigationPanel',
            treeSelector: '#baseOMSUMenuTree',
            tabSelector: '#baseOMSUMainTab',
            storeName: 'baseomsu.NavigationMenu',
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
                label.update({ text: "Плановая проверка органа местного самоуправления" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseOMSUNavigationAspect').reload();
        }
    }
});