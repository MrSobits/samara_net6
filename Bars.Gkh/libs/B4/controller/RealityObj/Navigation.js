Ext.define('B4.controller.realityobj.Navigation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    params: null,
    title: 'Жилой дом',

    mainView: 'realityobj.NavigationPanel',
    mainViewSelector: '#realityobjNavigationPanel',

    stores: ['realityobj.NavigationMenu'],

    views: ['realityobj.NavigationPanel'],

    refs: [
        { ref: 'menuTree', selector: '#realityobjMenuTree' },
        { ref: 'infoLabel', selector: '#realityobjInfoLabel' },
        { ref: 'mainTab', selector: '#realityobjMainTab' }
    ],

    aspects: [
        {
            xtype: 'gkhnavigationpanelaspect',
            name: 'realityobjNavigationAspect',
            panelSelector: '#realityobjNavigationPanel',
            treeSelector: '#realityobjMenuTree',
            tabSelector: '#realityobjMainTab',
            storeName: 'realityobj.NavigationMenu',
            getParams: function(menuRecord) {
                //перекрываем метод для того чтобы сделать свои параметры
                var me = this,
                    params = menuRecord.get('options');

                if (me.controller.params) {
                    params.realityObjectId = me.controller.params.getId();
                }
                params.record = me.controller.params;

                return params;
            },
            menuLoad: function() {
                var me = this,
                    store = me.controller.getStore(me.storeName);
                store.load({
                    scope: me,
                    callback: function(records) {
                        // Так как класс MenuItem не имеет сеттера свойства expanded, то тут вот такой говнокодик
                        Ext.each(records, function(rec) {
                            if (rec.get('text') == "Паспорт технического объекта") {
                                rec.collapse();
                            }
                        });
                    }
                });
            }
        }
    ],

    onLaunch: function() {
        var me = this,
            label, mainView;
        if (me.params) {
            label = me.getInfoLabel();
            if (label) {
                label.update({ text: me.params.get('Address') });
            }

            mainView = me.getMainComponent();
            if (mainView) {
                mainView.setTitle(me.title);
            }

            me.getAspect('realityobjNavigationAspect').reload();
        }
    }
});