Ext.define('B4.controller.documentsgjiregister.Navigation', {
    extend: 'B4.base.Controller',
    params: null,
    title: 'ГЖИ: Реестр документов',
    containerSelector: '#docsGjiRegisterMainTab',

    requires: ['B4.aspects.GkhNavigationPanel'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'documentsgjiregister.NavigationMenu'
    ],
    
    views: ['documentsgjiregister.NavigationPanel'],

    mainView: 'documentsgjiregister.NavigationPanel',
    mainViewSelector: '#docsGjiRegisterNavigationPanel',
    
    refs: [
        { ref: 'MenuTree', selector: '#docsGjiRegisterMenuTree' },
        { ref: 'InfoLabel', selector: '#docsGjiRegisterInfoLabel' },
        { ref: 'MainTab', selector: '#docsGjiRegisterMainTab' }
    ],

    aspects: [
        {
            /*
            Аспект взаимодействия Навигационной панели проверки по поручению руководства
            onMenuItemClick перекрыли потомучто нужно чтобы при нажатию на пункт меню в дочерние контроллеры передаввать 
            параметры и containerSelector
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'docsGjiRegisterNavigationAspect',
            panelSelector: '#docsGjiRegisterNavigationPanel',
            treeSelector: '#docsGjiRegisterMenuTree',
            tabSelector: '#docsGjiRegisterMainTab',
            storeName: 'documentsgjiregister.NavigationMenu',

            otherActions: function(actions) {
                actions[this.panelSelector + ' #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions[this.panelSelector + ' #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions[this.panelSelector + ' #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObjects, scope: this } };
                actions[this.panelSelector + ' #updateActiveGrid'] = { 'click': { fn: this.btnUpdateActiveGrid, scope: this } };
                actions[this.panelSelector + ' #cbTypeDocument'] = { 'select': { fn: this.onChangeTypeDocument, scope: this } };
                actions[this.panelSelector] = { 'beforerender': { fn: this.onPanelBeforeShow, scope: this } };
            },

            getParams: function (menuRecord) {
                //перекрываем метод для того чтобы сделать свои параметры
                
                //Необходимо перед нажатием на пункт меню закрыть открытые табы
                var container = Ext.ComponentQuery.query(this.tabSelector)[0];
                if(container)
                    container.removeAll();
                
                var params = menuRecord.get('options');
                params.containerSelector = this.tabSelector;
                params.treeMenuSelector = this.treeSelector;
                params.filterParams = this.controller.params;
                
                return params;
            },
            
            btnUpdateActiveGrid: function () {
                var activeTab = Ext.ComponentQuery.query(this.controller.containerSelector)[0].getActiveTab();
                if (activeTab) {
                    var str = activeTab.getStore();
                    str.currentPage = 1;
                    str.load();
                }
            },

            onPanelBeforeShow: function () {
                var panel = this.getPanel();
                var dateStart = panel.down('#dfDateStart').getValue();
                    if (this.controller.params)
                        this.controller.params.dateStart = dateStart;

                var dateEnd = panel.down('#dfDateEnd').getValue();
                    if (this.controller.params)
                        this.controller.params.dateEnd= dateEnd;
            },
            
            onChangeTypeDocument: function (field, record) {
                var me = this,
                    panel = me.getPanel(),
                    roFilter = panel.down('#sfRealityObject');
                
                if(roFilter && !roFilter.isVisible()){
                    roFilter.show();
                }

                //Необходимо перед нажатием на пункт меню закрыть открытые табы
                var container = Ext.ComponentQuery.query(me.controller.containerSelector)[0];
                container.removeAll();
                
                var data = record[0].getData();
                var params = !Ext.isEmpty(data.moduleScript) ? data.options : {};

                params.containerSelector = me.controller.containerSelector;
                params.treeMenuSelector = me.treeSelector;
                params.filterParams = me.controller.params;
                if (!Ext.isEmpty(data.moduleScript)) {
                    //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                    if (!me.controller.hideMask) {
                        me.controller.hideMask = function () { me.controller.unmask(); };
                    }

                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    me.controller.loadController(data.moduleScript, params, me.controller.containerSelector, me.controller.hideMask);
                }
            },

            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params)
                    this.controller.params.dateStart = newValue;
            },

            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params)
                    this.controller.params.dateEnd = newValue;
            },

            onChangeRealityObjects: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    if (newValue) {
                        if (typeof newValue == 'object') {
                            this.controller.params.realityObjectId = newValue.Id;
                        } else {
                            this.controller.params.realityObjectId = newValue;
                        }
                    }
                    else {
                        this.controller.params.realityObjectId = null;
                    }
                }
            }
        }
    ],

    init: function () {
        this.params = {};
        this.callParent(arguments);
    },

    onLaunch: function () {

        var label = this.getInfoLabel();
        if (label)
            label.update({ text: "ГЖИ: Реестр документов" });
    }
});