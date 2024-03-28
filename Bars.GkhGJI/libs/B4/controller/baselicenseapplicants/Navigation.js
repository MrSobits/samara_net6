Ext.define('B4.controller.baselicenseapplicants.Navigation', {
    extend: 'B4.base.Controller',
    views: ['baselicenseapplicants.NavigationPanel'],


    params: null,
    title: 'Проверка соискателей лицензии',

    mainView: 'baselicenseapplicants.NavigationPanel',
    mainViewSelector: 'baselicenseappnavigationpanel',

    containerSelector: '#baselicenseapplicantsMainTab',

    stores: ['baselicenseapplicants.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseLicenseApplicantsMenuTree' },
        { ref: 'infoLabel', selector: '#baseLicenseApplicantsInfoLabel' },
        { ref: 'mainTab', selector: '#baseLicenseApplicantsMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели Проверок по обращениям граждан
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'baseLicenseApplicantsNavigationAspect',
            panelSelector: 'baselicenseappnavigationpanel',
            treeSelector: '#baseLicenseApplicantsMenuTree',
            tabSelector: '#baseLicenseApplicantsMainTab',
            storeName: 'baselicenseapplicants.NavigationMenu',
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
                label.update({ text: "Проверка соискателей лицензии" });

            var mainView = me.getMainComponent();
            if (mainView)
                mainView.setTitle(me.title);

            me.getAspect('baseLicenseApplicantsNavigationAspect').reload();
        }
    }
});