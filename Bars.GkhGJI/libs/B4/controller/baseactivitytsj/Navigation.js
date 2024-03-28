Ext.define('B4.controller.baseactivitytsj.Navigation', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GkhNavigationPanel'
    ],

    stores: ['baseactivitytsj.NavigationMenu'],
    
    views: ['baseactivitytsj.NavigationPanel'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    params: null,
    title: 'Проверка Дятельности ТСЖ',

    mainView: 'baseactivitytsj.NavigationPanel',
    mainViewSelector: '#baseActivityTsjNavigationPanel',

    containerSelector: '#baseActivityTsjMainTab',

    refs: [
        { ref: 'menuTree', selector: '#baseActivityTsjMenuTree' },
        { ref: 'infoLabel', selector: '#baseActivityTsjInfoLabel' },
        { ref: 'mainTab', selector: '#baseActivityTsjMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели деятельности ТСЖ
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseActivityTsjNavigationAspect',
            panelSelector: '#baseActivityTsjNavigationPanel',
            treeSelector: '#baseActivityTsjMenuTree',
            tabSelector: '#baseActivityTsjMainTab',
            storeName: 'baseactivitytsj.NavigationMenu',
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
                label.update({ text: "Проверка активности деятельности ТСЖ" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseActivityTsjNavigationAspect').reload();
        }
    }
});