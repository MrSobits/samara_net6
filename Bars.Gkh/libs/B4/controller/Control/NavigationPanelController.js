Ext.define('B4.controller.Control.NavigationPanelController', {
    extend: 'B4.base.Controller',
 views: [ 'Control.NavigationPanel' ], 

    params: null,

    paramName: null,
    paramValue: null,
    title: null,
    
    mainView: 'Control.NavigationPanel',
    mainViewSelector: '#navigationPanel',

    containerSelector: '#mainTab',

    stores: ['Control.NavigationMenu'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    refs: [
        { ref: 'menuTree', selector: '#menuTree' },
        { ref: 'informLabel', selector: '#informLabel' },
        { ref: 'mainTab', selector: '#mainTab' }
    ],

    init: function () {
        this.control({
            '#menuTree': {
                itemclick: this.onMenuItemClick,
                'load': this.onMenuLoad
            },
            '#mainTab': {
                'tabchange': this.onTabChange
            },
            '#navigationPanel': {
                'activate': function () {
                    var store = this.getStore('Control.NavigationMenu');
                    store.load();
                }
            }
        });

        var store = this.getStore('Control.NavigationMenu');
        store.on('beforeload', this.onBeforeLoad, this);
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params[this.paramName] = this.paramValue;
        }
    },

    onTabChange: function (tp, newTab) {
        /*
        var nodes = this.getMenuTree().getView().getNodes();
        Ext.each(nodes, function (item, index, allNodes) {
            var rec = this.getMenuTree().getView().getRecord(item);
            if (rec.get('moduleScript') == newTab.moduleName) {
                var selectedRecord = this.getMenuTree().getSelectionModel().getLastSelected();
                if (rec == selectedRecord) {
                    return;
                }
                this.getMenuTree().getSelectionModel().select(index);
                return;
            }
        }, this);
        */
    },

    onMenuLoad: function () {
        var me = this;
        var nodes = this.getMenuTree().getView().getNodes();
        if (nodes[0]) {
            var rec = this.getMenuTree().getView().getRecord(nodes[0]);
            if (rec.get('leaf')) {
                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                if (!me.hideMask) {
                    me.hideMask = function () { me.unmask(); };
                }
                me.mask('Загрузка', me.getMainComponent());
                me.loadController(rec.get('moduleScript'), me.params, null, me.hideMask);
            }
        }
    },

    onMenuItemClick: function (view, record) {
        var me = this;
        if (record.get('leaf')) {
            //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
            if (!me.hideMask) {
                me.hideMask = function () { me.unmask(); };
            }
            me.mask('Загрузка', me.getMainComponent());
            me.loadController(record.get('moduleScript'), this.params, null, me.hideMask);
        }
            
    }
});