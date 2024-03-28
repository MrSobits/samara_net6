Ext.define('B4.controller.baseinscheck.Navigation', {
    extend: 'B4.base.Controller',
 views: [ 'baseinscheck.NavigationPanel' ], 


    params: null,
    title: 'Инспекционная проверка',
    mainView: 'baseinscheck.NavigationPanel',
    mainViewSelector: '#baseInsCheckNavigationPanel',
    containerSelector: '#baseInsCheckMainTab',

    stores: ['baseinscheck.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseInsCheckMenuTree' },
        { ref: 'infoLabel', selector: '#baseInsCheckInfoLabel' },
        { ref: 'mainTab', selector: '#baseInsCheckMainTab' }
    ],

    aspects: [
        {
            /*
            * Аспект взаимодействия Навигационной панели инспекционных обследований
            * onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            * параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseInsCheckNavigationAspect',
            panelSelector: '#baseInsCheckNavigationPanel',
            treeSelector: '#baseInsCheckMenuTree',
            tabSelector: '#baseInsCheckMainTab',
            storeName: 'baseinscheck.NavigationMenu',
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
            if(label)
                label.update({ text: "Инспекционное обследование по плану" });

            var mainView = this.getMainComponent();
            if(mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseInsCheckNavigationAspect').reload();
        }
    }
});