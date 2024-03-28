Ext.define('B4.controller.gkuinfo.Navigation', {
    extend: 'B4.base.Controller',
    views: [ 'gkuinfo.NavigationPanel' ],

    params: null,
    title: 'Сведения по дому',

    mainView: 'gkuinfo.NavigationPanel',
    mainViewSelector: '#gkuinfoNavigationPanel',

    stores: ['gkuinfo.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#gkuinfoMenuTree' },
        { ref: 'infoLabel', selector: '#gkuinfoInfoLabel' },
        { ref: 'mainTab', selector: '#gkuinfoMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'gkuinfoNavigationAspect',
            panelSelector: '#gkuinfoNavigationPanel',
            treeSelector: '#gkuinfoMenuTree',
            tabSelector: '#gkuinfoMainTab',
            storeName: 'gkuinfo.NavigationMenu',
            getParams: function (menuRecord) {
                //перекрываем метод для того чтобы сделать свои параметры
                var params = menuRecord.get('options');
                
                if (this.controller.params)
                    params.realityObjectId = this.controller.params.getId();
                    params.record = this.controller.params;
                
                return params;
            }
        }
    ],

    onLaunch: function () {
        if (this.params) {
            var label = this.getInfoLabel();
            if(label)
                label.update({ text: this.params.get('Address') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);
            
            this.getAspect('gkuinfoNavigationAspect').reload();
        }
    }
});
