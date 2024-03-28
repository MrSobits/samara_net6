Ext.define('B4.controller.repairobject.Navigation', {
    /*
    * Контроллер навигационной панели объекта текущего ремонта
    */
    extend: 'B4.base.Controller',
    views: ['repairobject.NavigationPanel'],


    params: null,
    title: 'Объект текущего ремонта',

    mainView: 'repairobject.NavigationPanel',
    mainViewSelector: '#repairObjectNavigationPanel',

    stores: ['repairobject.NavigationMenu'],
    requires: ['B4.aspects.GkhNavigationPanel'],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'menuTree', selector: '#repairObjectMenuTree' },
        { ref: 'infoLabel', selector: '#repairObjectInfoLabel' },
        { ref: 'mainTab', selector: '#repairObjectMainTab' }
    ],

    aspects: [
        {
            /*
            * Аспект панели навигации раздела объектов капремонта
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'repairObjectNavigationAspect',
            panelSelector: '#repairObjectNavigationPanel',
            treeSelector: '#repairObjectMenuTree',
            tabSelector: '#repairObjectMainTab',
            storeName: 'repairobject.NavigationMenu',
            onMenuLoad: function (store) {
                var me = this;
                var nodes = me.controller.getMenuTree().getView().getNodes();
                if (nodes[0]) {
                    var view = me.controller.getMenuTree().getView();
                    var rec = view.getRecord(nodes[0]);

                    if (rec.get('text') == 'Разделы отсутствуют') {
                        //Если разделы отсутсвуют то закрываем навигационную панель
                        this.close();
                    }
                    else {
                        if (this.controller.params.showPassportObject) {
                            if (me.objectId != me.getObjectId()) {
                                me.objectId = me.getObjectId();
                                this.onMenuItemClick(view, rec);
                            }
                        }
                    }
                }
            }
        }
    ],

    onLaunch: function () {
        var me = this;
        
        if (me.params) {

            me.params.showPassportObject = true;

            var roName = me.params.get('RealityObjName');
            var realtyObject = me.params.get('RealityObject');
            me.getInfoLabel().update({
                text: Ext.isEmpty(roName)
                    ? (realtyObject && realtyObject.Address ? realtyObject.Address : "")
                    : roName
            });
            me.getMainComponent().setTitle(me.title);

            me.getAspect('repairObjectNavigationAspect').reload();

            if (me.params.childController) {

                me.params.showPassportObject = false;
                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                if (!me.hideMask) {
                    me.hideMask = function () { me.unmask(); };
                }

                me.mask('Загрузка', me.getMainComponent());
                me.loadController(me.params.childController, me.params, "#repairObjectMainTab", me.hideMask);
            }
        }
    }
});