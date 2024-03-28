Ext.define('B4.aspects.GkhNavigationPanel', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gkhnavigationpanelaspect',

    panelSelector: null,
    treeSelector: null,
    tabSelector: null,
    storeName: null,
    controller: null,
    paramName: 'objectId',
    objectId:null,

    init: function (controller) {

        var actions = {};
        this.callParent(arguments);

        this.controller = controller;
        
        actions[this.treeSelector] = {
            'itemclick': { fn: this.onMenuItemClick, scope: this },
            'load': { fn: this.onMenuLoad, scope: this }
        };

        actions[this.tabSelector] = { 'tabchange': { fn: this.onTabChange, scope: this} };

        this.otherActions(actions);

        this.controller.getStore(this.storeName).on('beforeload', this.onBeforeLoad, this);

        controller.control(actions);
    },

    otherActions: function (actions) {
        //Данный метод служит для перекрытия в контроллерах где используется данный аспект
        //наслучай если потребуется к данному аспекту добавить дополнительные обработчики
    },

    getPanel: function () {
        return Ext.ComponentQuery.query(this.panelSelector)[0];
    },

    //Метод переопределяется в контроллерах где он используется
    getObjectId: function () {
        if (this.controller.params && this.controller.params.get && this.controller.params.get('Id'))
            return this.controller.params.get('Id');
        return null;
    },
    
    //Метод который позволяет определить
    //Необходимость презагрузки Меню + Необходимость очистки открытых до этого вкладок + Открытия пункта меню по умолчанию
    reload: function () {

        var me = this;
        if (me.objectId != me.getObjectId()) {
            
            //Если пришел новый объект то закрываем вкладки
            var tabComponent = Ext.ComponentQuery.query(me.tabSelector)[0];
            if (tabComponent && tabComponent.items.length > 0) {
                tabComponent.removeAll();
                tabComponent.doLayout();
            }
            
            me.menuLoad();
        }
    },

    menuLoad: function () {
        var store = this.controller.getStore(this.storeName);
        store.load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.controller.params && this.getObjectId()) {
            var docId = 0;
            if (this.controller.params.defaultParams) {
                docId = this.controller.params.defaultParams.documentId;
            }            
            operation.params[this.paramName] = this.getObjectId();
            operation.params.documentId = docId;
        }
    },

    onTabChange: function (tp, newTab) {
        
    },

    onMenuLoad: function () {
        var me = this;
        // Пытаемся загрузить дефолтный контроллер данного контроллера если он есть
        var nodes = me.controller.getMenuTree().getView().getNodes();
        if (nodes.length > 0) {
            var params = me.controller.params;

            if (params && params.defaultController && params.defaultParams) {

                var view = me.controller.getMenuTree().getView();
                var arrlength = nodes.length;
                for (var i = 0; i < arrlength; i++) {
                    var rec = view.getRecord(nodes[i]);
                    var options = rec.get('options');
                    if (options) {
                        if (options.selected) {
                            var select = view.getSelectionModel();
                            if (select) {
                                view.getSelectionModel().deselectAll(true);
                                view.getSelectionModel().select(rec, true, true);
                            }
                        }
                    }
                   
                }
                //nodes.forEach(x => {
                //    var rec = view.getRecord(x);
                //    var options = rec.get('options');
                //    if (options) {
                //        if (options.selected) {
                //            var select = view.getSelectionModel();
                //            if (select) {
                //                view.getSelectionModel().deselectAll(true);
                //                view.getSelectionModel().select(rec, true, true);
                //            }
                //        }
                //    }
                //});              

                me.loadDefaultController(me.controller);

                //Обнуляем дефолтные параметры потому что если в карточке будут создавать документы то дерево будет обновлятся
                //и для того тобы еще раз неслучился вызов дефолтного параметра необходимо его занулить 

                params.defaultParams = null;
            } else {

                if( !params || !params.defaultController){
                    // Грузим первый элемент дерева, только если нет дефолного контроллера (дефолтный контроллер - тот который мы хотим что бы открылся первым по умолчанию)            
                    var view = me.controller.getMenuTree().getView();
                    var rec = view.getRecord(nodes[0]);

                    if (rec.get('text') == 'Разделы отсутствуют') {
                        //Если разделы отсутсвуют то закрываем навигационную панель
                        me.close();
                    } else {
                        if (me.objectId != me.getObjectId()) {
                            me.objectId = me.getObjectId();
                            this.onMenuItemClick(view, rec);
                        }
                    }
                }
            }
        }
    },

    getParams:function() {
        //по умолчанию возвращает параметры главного контроллера
        return this.controller.params;
    },

    onMenuItemClick: function (view, record) {
        var me = this;
        if (record.get('moduleScript')) {
            //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
            if (!me.controller.hideMask) {
                me.controller.hideMask = function () { me.controller.unmask(); };
            }
            me.controller.mask('Загрузка', me.controller.getMenuTree());

                me.controller.loadController(record.get('moduleScript'), me.getParams(record), me.tabSelector, me.controller.hideMask);
        }
    },

    close: function () {
        this.getPanel().close();
    },

    loadDefaultController: function (controller) {
        var params = controller.params;
        var cntrSelector = '#' + controller.getMainComponent({ token: controller.self.getName() }).down('.tabpanel').id;
        var treeSelector = '#' + controller.getMainComponent({ token: controller.self.getName() }).down('.menutreepanel').id;
        Ext.applyIf(params.defaultParams, { containerSelector: cntrSelector, treeMenuSelector: treeSelector });
        controller.loadController(params.defaultController, params.defaultParams, cntrSelector);        
    }
});