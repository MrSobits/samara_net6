Ext.define('B4.controller.activitytsj.Navigation', {
    extend: 'B4.base.Controller',
    views: ['activitytsj.NavigationPanel'],
    requires: [
        'B4.aspects.GkhNavigationPanel'
    ],

    params: null,
    title: 'Деятельность ТСЖ и ЖСК',

    mainView: 'activitytsj.NavigationPanel',
    mainViewSelector: '#activityTsjNavigationPanel',

    stores: ['activitytsj.NavigationMenu'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    refs: [
        { ref: 'menuTree', selector: '#activityTsjMenuTree' },
        { ref: 'infoLabel', selector: '#activityTsjInfoLabel' },
        { ref: 'mainTab', selector: '#activityTsjMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'activityTsjNavigationAspect',
            panelSelector: '#activityTsjNavigationPanel',
            treeSelector: '#activityTsjMenuTree',
            tabSelector: '#activityTsjMainTab',
            storeName: 'activitytsj.NavigationMenu',
            title: 'Деятельность ТСЖ и ЖСК'
        }
    ],

    onLaunch: function () {
        if (this.params) {

            var label = this.getInfoLabel();
            if(label)
                label.update({ text: 'Деятельность ТСЖ и ЖСК (' + this.params.get('ManOrgName') + ')' });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('activityTsjNavigationAspect').reload();
        }
    }
});
