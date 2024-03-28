Ext.define('B4.controller.baselicensereissuance.Navigation', {
    extend: 'B4.base.Controller',
    views: ['baselicensereissuance.NavigationPanel'],


    params: null,
    title: 'Проверка лицензиата',

    mainView: 'baselicensereissuance.NavigationPanel',
    mainViewSelector: 'baselicensereissuancenavigationpanel',

    containerSelector: '#baselicensereissuanceMainTab',

    stores: ['baselicensereissuance.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseLicenseReissuanceMenuTree' },
        { ref: 'infoLabel', selector: '#baseLicenseReissuanceInfoLabel' },
        { ref: 'mainTab', selector: '#baseLicenseReissuanceMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели Проверок по обращениям граждан
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseLicenseReissuanceNavigationAspect',
            panelSelector: 'baselicensereissuancenavigationpanel',
            treeSelector: '#baseLicenseReissuanceMenuTree',
            tabSelector: '#baseLicenseReissuanceMainTab',
            storeName: 'baselicensereissuance.NavigationMenu',
            paramName: 'inspectionId',
            getParams: function (menuRecord) {
                //перекрываем метод для того чтобы сделать свои параметры
                var me = this,
                    params = menuRecord.get('options');

                params.containerSelector = me.tabSelector;
                params.treeMenuSelector = me.treeSelector;
                
                return params;
            }
        }
    ],

    onLaunch: function () {
        var me = this;
        if (me.params) {
            var label = me.getInfoLabel();
            if (label)
                label.update({ text: "Проверка лицензиата" });

            var mainView = me.getMainComponent();
            if (mainView)
                mainView.setTitle(me.title);

            me.getAspect('baseLicenseReissuanceNavigationAspect').reload();
        }
    }
});