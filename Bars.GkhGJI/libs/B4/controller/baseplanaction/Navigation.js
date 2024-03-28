Ext.define('B4.controller.baseplanaction.Navigation', {
    extend: 'B4.base.Controller',
    views: ['baseplanaction.NavigationPanel'],

    params: null,
    title: 'Проверка по плану мероприятий',
    mainView: 'baseplanaction.NavigationPanel',
    mainViewSelector: 'basePlanActionNavigationPanel',
    containerSelector: '#basePlanActionMainTab',

    stores: ['baseplanaction.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#basePlanActionMenuTree' },
        { ref: 'infoLabel', selector: '#basePlanActionInfoLabel' },
        { ref: 'mainTab', selector: '#basePlanActionMainTab' }
    ],

    aspects: [
        {
            /*
            * Аспект взаимодействия Навигационной панели инспекционных обследований
            * onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            * параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'basePlanActionNavigationAspect',
            panelSelector: 'basePlanActionNavigationPanel',
            treeSelector: '#basePlanActionMenuTree',
            tabSelector: '#basePlanActionMainTab',
            storeName: 'baseplanaction.NavigationMenu',
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
                label.update({ text: "Проверка по плану мероприятий" });

            var mainView = this.getMainComponent();
            if(mainView)
                mainView.setTitle(this.title);

            this.getAspect('basePlanActionNavigationAspect').reload();
        }
    }
});