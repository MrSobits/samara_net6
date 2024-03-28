Ext.define('B4.controller.basedisphead.Navigation', {
    extend: 'B4.base.Controller',
 views: [ 'basedisphead.NavigationPanel' ], 


    params: null,
    title: 'Проверка по поручению руководителя',

    mainView: 'basedisphead.NavigationPanel',
    mainViewSelector: '#baseDispHeadNavigationPanel',

    containerSelector: '#baseDispHeadMainTab',

    stores: ['basedisphead.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseDispHeadMenuTree' },
        { ref: 'infoLabel', selector: '#baseDispHeadInfoLabel' },
        { ref: 'mainTab', selector: '#baseDispHeadMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели проверки по поручению руководства
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передавать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseDispHeadNavigationAspect',
            panelSelector: '#baseDispHeadNavigationPanel',
            treeSelector: '#baseDispHeadMenuTree',
            tabSelector: '#baseDispHeadMainTab',
            storeName: 'basedisphead.NavigationMenu',
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
        debugger;
        if (this.params) {
            var label = this.getInfoLabel();
            if(label)
                label.update({ text: "Поручение руководителя органа государственного контроля" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseDispHeadNavigationAspect').reload();
        }
    }
});