Ext.define('B4.controller.longtermprobject.Navigation', {
    extend: 'B4.base.Controller',

    params: null,
    title: 'Объект региональной программы',

    mainView: 'longtermprobject.NavigationPanel',
    mainViewSelector: '#longtermprobjectNavigationPanel',

    stores: ['longtermprobject.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    views: ['longtermprobject.NavigationPanel'],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#longtermprobjectMenuTree' },
        { ref: 'infoLabel', selector: '#longtermprobjectInfoLabel' },
        { ref: 'mainTab', selector: '#longtermprobjectMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'longtermprobjectNavigationAspect',
            panelSelector: '#longtermprobjectNavigationPanel',
            treeSelector: '#longtermprobjectMenuTree',
            tabSelector: '#longtermprobjectMainTab',
            storeName: 'longtermprobject.NavigationMenu',
            getParams: function (menuRecord) {
                //перекрываем метод для того чтобы сделать свои параметры
                var params = menuRecord.get('options');

                if (this.controller.params)
                    params.longTermObjId = this.controller.params.getId();
                params.record = this.controller.params;

                return params;
            }
        }
    ],

    onLaunch: function (self) {
        if (this.params) {
            var label = this.getInfoLabel();
            if (label)
                label.update({ text: this.params.get('Address') });

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.title);

            this.getAspect('longtermprobjectNavigationAspect').reload();
        }
    }
});