Ext.define('B4.controller.preventiveaction.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],
    title: 'Профилактическое мероприятие',
    params: null,

    stores: ['preventiveaction.NavigationMenu'],

    views: ['preventiveaction.NavigationPanel'],

    mainView: 'preventiveaction.NavigationPanel',
    mainViewSelector: '#preventiveActionNavigationPanel',
    containerSelector: '#preventiveActionMainTab',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#preventiveActionMenuTree' },
        { ref: 'infoLabel', selector: '#preventiveActionInfoLabel' },
        { ref: 'mainTab', selector: '#preventiveActionMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'preventiveActionNavigationAspect',
            panelSelector: '#preventiveActionNavigationPanel',
            treeSelector: '#preventiveActionMenuTree',
            tabSelector: '#preventiveActionMainTab',
            storeName: 'preventiveaction.NavigationMenu',
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
                label.update({ text: "Профилактическое мероприятие" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('preventiveActionNavigationAspect').reload();
        }
    }
});