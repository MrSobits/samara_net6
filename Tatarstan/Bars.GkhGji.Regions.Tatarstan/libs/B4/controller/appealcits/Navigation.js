Ext.define('B4.controller.appealcits.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['appealcits.NavigationMenu'],

    views: ['appealcits.NavigationPanel'],

    params: null,
    title: 'Мотивированное представление',

    mainView: 'appealcits.NavigationPanel',
    mainViewSelector: '#motivatedPresentationAppealCitsNavigationPanel',

    containerSelector: '#motivatedPresentationAppealCitsMainTab',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#motivatedPresentationAppealCitsMenuTree' },
        { ref: 'infoLabel', selector: '#motivatedPresentationAppealCitsInfoLabel' },
        { ref: 'mainTab', selector: '#motivatedPresentationAppealCitsMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'motivatedPresentationAppealCitsNavigationAspect',
            panelSelector: '#motivatedPresentationAppealCitsNavigationPanel',
            treeSelector: '#motivatedPresentationAppealCitsMenuTree',
            tabSelector: '#motivatedPresentationAppealCitsMainTab',
            storeName: 'appealcits.NavigationMenu',
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
            }
        }
    ],

    onLaunch: function () {
        if (this.params) {

            var label = this.getInfoLabel();
            if (label)
                label.update({ text: "Мотивированное представление" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('motivatedPresentationAppealCitsNavigationAspect').reload();
        }
    }
});