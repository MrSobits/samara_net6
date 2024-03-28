Ext.define('B4.controller.admincase.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    stores: ['admincase.NavigationMenu'],
    
    views: ['baseadmincase.NavigationPanel'],

    params: null,
    title: 'Административное дело',

    mainView: 'baseadmincase.NavigationPanel',
    mainViewSelector: '#baseAdminCaseNavigationPanel',

    containerSelector: '#baseAdminCaseMainTab',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#baseAdminCaseMenuTree' },
        { ref: 'infoLabel', selector: '#baseAdminCaseInfoLabel' },
        { ref: 'mainTab', selector: '#baseAdminCaseMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'adminCaseNavigationAspect',
            panelSelector: '#baseAdminCaseNavigationPanel',
            treeSelector: '#baseAdminCaseMenuTree',
            tabSelector: '#baseAdminCaseMainTab',
            storeName: 'admincase.NavigationMenu',
            paramName: 'inspectionId',
            getObjectId: function () {
                if (this.controller.params && this.controller.params.get) {
                    if (this.controller.params.get('InspectionId')) {
                        return this.controller.params.get('InspectionId');
                    } else {
                        return this.controller.params.get('Inspection');
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
            if(label)
                label.update({ text: "Административное дело" });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('adminCaseNavigationAspect').reload();
        }
    }
});