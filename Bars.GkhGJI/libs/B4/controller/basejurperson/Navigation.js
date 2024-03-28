Ext.define('B4.controller.basejurperson.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    params: null,
    title: 'Плановая проверка юридического лица',

    stores: ['basejurperson.NavigationMenu'],

    views: ['basejurperson.NavigationPanel'],

    mainView: 'basejurperson.NavigationPanel',
    mainViewSelector: '#baseJurPersonNavigationPanel',

    containerSelector: '#baseJurPersonMainTab',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseJurPersonMenuTree' },
        { ref: 'infoLabel', selector: '#baseJurPersonInfoLabel' },
        { ref: 'mainTab', selector: '#baseJurPersonMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели Плановой проверки юр лиц
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseJurPersonNavigationAspect',
            panelSelector: '#baseJurPersonNavigationPanel',
            treeSelector: '#baseJurPersonMenuTree',
            tabSelector: '#baseJurPersonMainTab',
            storeName: 'basejurperson.NavigationMenu',
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
                label.update({ text: "Плановая проверка юридического лица" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseJurPersonNavigationAspect').reload();
        }
    }
});