Ext.define('B4.controller.basestatement.Navigation', {
    extend: 'B4.base.Controller',
 views: [ 'basestatement.NavigationPanel' ], 


    params: null,
    title: 'Процесс по обращению',

    mainView: 'basestatement.NavigationPanel',
    mainViewSelector: '#baseStatementNavigationPanel',

    containerSelector: '#baseStatementMainTab',

    stores: ['basestatement.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseStatementMenuTree' },
        { ref: 'infoLabel', selector: '#baseStatementInfoLabel' },
        { ref: 'mainTab', selector: '#baseStatementMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели Проверок по обращениям граждан
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseStatementNavigationAspect',
            panelSelector: '#baseStatementNavigationPanel',
            treeSelector: '#baseStatementMenuTree',
            tabSelector: '#baseStatementMainTab',
            storeName: 'basestatement.NavigationMenu',
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
                label.update({ text: "Процесс ГЖИ по обращению" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('baseStatementNavigationAspect').reload();
        }
    }
});