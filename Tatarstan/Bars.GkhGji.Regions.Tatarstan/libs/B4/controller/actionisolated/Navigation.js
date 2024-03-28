Ext.define('B4.controller.actionisolated.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['actionisolated.NavigationMenu'],

    views: ['actionisolated.NavigationPanel'],

    params: null,
    title: 'Мероприятие без взаимодействия с контролируемыми лицами',

    mainView: 'actionisolated.NavigationPanel',
    mainViewSelector: '#actionIsolatedNavigationPanel',

    containerSelector: '#actionisolatedMainTab',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#actionisolatedMenuTree' },
        { ref: 'infoLabel', selector: '#actionisolatedInfoLabel' },
        { ref: 'mainTab', selector: '#actionisolatedMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели проверки 
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'actionisolatedNavigationAspect',
            panelSelector: '#actionIsolatedNavigationPanel',
            treeSelector: '#actionisolatedMenuTree',
            tabSelector: '#actionisolatedMainTab',
            storeName: 'actionisolated.NavigationMenu',
            paramName: 'inspectionId',
            getObjectId: function () {
                if (this.controller.params && this.controller.params.get) {
                    if (this.controller.params.get('InspectionId')) {
                        return this.controller.params.get('InspectionId');
                    } else {
                        return this.controller.params.get('Id');
                    }
                }
                return null;
            },
            getParams: function (menuRecord) {
                //перекрываем метод для того чтобы сделать свои параметры
                var params = menuRecord.get('options');
                params.containerSelector = this.tabSelector;
                params.treeMenuSelector = this.treeSelector;

                return params;
            },
            reload: function () {
                var me = this;
                if (me.objectId != me.getObjectId()) {

                    //Если пришел новый объект то закрываем вкладки
                    var tabComponent = Ext.ComponentQuery.query(me.tabSelector)[0];
                    if (tabComponent && tabComponent.items.length > 0) {
                        tabComponent.removeAll();
                        tabComponent.doLayout();
                    }
                }

                me.menuLoad();
            }
        }
    ],

    onLaunch: function () {
        if (this.params) {

            var label = this.getInfoLabel();
            if (label)
                label.update({ text: "Мероприятие без взаимодействия с контролируемыми лицами" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('actionisolatedNavigationAspect').reload();
        }
    }
});