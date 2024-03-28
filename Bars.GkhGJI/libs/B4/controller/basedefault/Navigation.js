Ext.define('B4.controller.basedefault.Navigation', {
    extend: 'B4.base.Controller',

    params: null,
    title: 'ГЖИ: Проверка без основания',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['basedefault.NavigationMenu'],
    views: ['basedefault.NavigationPanel'],

    mainView: 'basedefault.NavigationPanel',
    mainViewSelector: '#baseDefaultNavigationPanel',

    containerSelector: '#baseDefaultMainTab',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseDefaultMenuTree' },
        { ref: 'infoLabel', selector: '#baseDefaultInfoLabel' },
        { ref: 'mainTab', selector: '#baseDefaultMainTab' }
    ],

    aspects: [
        {
           /*
            * Аспект взаимодействия Навигационной панели проверки по поручению руководства
            * onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            * параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseDefaultNavigationAspect',
            panelSelector: '#baseDefaultNavigationPanel',
            treeSelector: '#baseDefaultMenuTree',
            tabSelector: '#baseDefaultMainTab',
            storeName: 'basedefault.NavigationMenu',
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
                label.update({ text: "Проверка ГЖИ без основания" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseDefaultNavigationAspect').reload();
        }
    }
});