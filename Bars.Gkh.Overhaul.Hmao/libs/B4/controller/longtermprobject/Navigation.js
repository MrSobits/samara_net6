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
            },
            reload: function () {
                var tabComponent = Ext.ComponentQuery.query(this.tabSelector)[0];
                if (tabComponent && tabComponent.items.length > 0) {
                    tabComponent.removeAll();
                    tabComponent.doLayout();
                }

                this.menuLoad();
            },
            onMenuLoad: function () {
                var me = this;
                // Пытаемся загрузить дефолтный контроллер данного контроллера если он есть
                var nodes = me.controller.getMenuTree().getView().getNodes();

                if (nodes.length > 0) {
                    var params = me.controller.params;

                    if (params && params.defaultController && params.defaultParams) {

                        this.loadDefaultController(me.controller);

                        //Обнуляем дефолтные параметры потомучто если в карточке будут создавать документы то дерево будет обновлятся
                        //и для того тобы еще раз неслучился вызов дефолтного параметра необходимо его занулить 

                        params.defaultParams = null;
                    } else {

                        // Грузим первый элемент дерева, только если нет дефолного контроллера (дефолтный контроллер - тот который мы хотим что бы открылся первым по умолчанию)            
                        var view = me.controller.getMenuTree().getView();
                        var rec = view.getRecord(nodes[0]);

                        if (rec.get('text') == 'Разделы отсутствуют') {
                            //Если разделы отсутсвуют то закрываем навигационную панель
                            this.close();
                        } else {
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