Ext.define('B4.controller.warninginspection.Navigation', {
    extend: 'B4.base.Controller',
    views: ['warninginspection.NavigationPanel'],


    params: null,
    title: 'Предостережение',

    mainView: 'warninginspection.NavigationPanel',
    mainViewSelector: '#warninginspectionNavigationPanel',

    containerSelector: '#warninginspectionMainTab',

    stores: ['warninginspection.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#warninginspectionMenuTree' },
        { ref: 'infoLabel', selector: '#warninginspectionInfoLabel' },
        { ref: 'mainTab', selector: '#warninginspectionMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели проверки по поручению руководства
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передавать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'warninginspectionNavigationAspect',
            panelSelector: '#warninginspectionNavigationPanel',
            treeSelector: '#warninginspectionMenuTree',
            tabSelector: '#warninginspectionMainTab',
            storeName: 'warninginspection.NavigationMenu',
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
                label.update({ text: "Предостережение" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('warninginspectionNavigationAspect').reload();
        }
    }
});