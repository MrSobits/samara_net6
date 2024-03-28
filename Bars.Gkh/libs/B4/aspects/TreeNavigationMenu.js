/**
 * Аспект реализующий логику взаимодействия navigationmenu и роутингов.
 *
 * Пример использования:
 * <pre>
 * requires: [
 *     'B4.aspects.TreeNavigationMenu',
 * ],
 *
 * aspects: [{
 *       xtype: 'treenavigationmenuaspect',
 *       name: 'treeNavigationAspect',
 *       mainPanel: 'navigationPanel',
 *       menuContainer: 'navigationPanel menutreepanel',
 *       tabContainer: 'navigationPanel tabpanel',
 *       breadcrumbs: 'navigationPanel breadcrumbs',
 *       storeName: 'navigationMenu',
 *       deployKey: 'object_info',
 *       contextKey: 'objId'
 *  }]
 * </pre>
 */

Ext.define('B4.aspects.TreeNavigationMenu', {
    extend: 'B4.base.Aspect',

    alias: 'widget.treenavigationmenuaspect',

    requires: ['B4.view.realityobj.NavigationPanel'],

    /*
    * @cfg {String}
    */
    paramName: 'objectId',

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);
        me.initDeployInfo(controller, me.deployKey);
        
        actions[me.menuContainer] = {
            'itemclick': { fn: me.onMenuItemClick, scope: me },
            'load': { fn: me.afterMenuLoad, scope: me },
            'viewready': { fn: me.loadMenu, scope: me }
        };
        if (me.breadcrumbs) {
            actions[me.breadcrumbs] = {
                'boxready': { fn: me.prepareTitle, scope: me }
            };
        }

        controller.control(actions);
    },
    
    getMenu: function() {
        var me = this;

        return me.componentQuery(me.menuContainer);
    },

    getTabContainer: function () {
        var me = this;

        return me.componentQuery(me.tabContainer);
    },
    
    /*
     * @method getObjectId 
     * Метод возвращает текущий id объекта
     */
    getObjectId: function () {
        var me = this;
        return me.controller.getContextValue(me.componentQuery(me.mainPanel), me.contextKey);
    },
    
    /*
     * @method onBeforeLoad
     * Добавляет параметры в store перед запросом
     */
    onBeforeLoad: function (store, operation) {
        var me = this,
            objectId = me.getObjectId();
        if (objectId) {
            operation.params = operation.params || {};
            operation.params[me.paramName] = me.getObjectId();
        }
    },

    /*
     * @method updateMenu
     * Метод который позволяет определить
     * Необходимость презагрузки Меню
     */
    updateMenu: function () {
        var me = this;
        if (me.objectId != me.getObjectId()) {
            me.loadMenu();
        }
    },

    /*
     * @method loadMenu
     * Метод подгружает меню
     */
    loadMenu: function (menu) {
        var me = this,
            treeMenu = menu || me.getMenu(),
            store = treeMenu.getStore();
        
        store.on('beforeload', me.onBeforeLoad, me, { single: true });
        store.load();
        //menu.setLoading(true, true);
    },

    /*
     * @method afterMenuLoad
     * Метод выполняется когда подгрузиться меню
     */
    afterMenuLoad: function () {
        var me = this,
            record = Array.prototype.slice.call(arguments, 1, 2)[0].firstChild,
            mainCmp, tabPanel;

        if (record && record.get('leaf')) {
            mainCmp = me.controller.getMainComponent();
            tabPanel = mainCmp.down('[nId=navtabpanel]');
            if (tabPanel && !tabPanel.getActiveTab()) {
                me.onMenuItemClick(me.controller.getMainComponent(), record);
            }
            me.getMenu().setLoading(false);
        }
    },

    /*
     * @method onMenuItemClick
     * Метод обработки нажатия пунктов в меню
     */
    onMenuItemClick: function (view, record) {
        var me = this,
            objectId;
        if (record.get('leaf')) {
            objectId = me.controller.getContextValue(view, me.contextKey);
            me.controller.application.redirectTo(Ext.String.format(record.get('moduleScript'), objectId));
            me.setExtraParams(record);
        }
    },

    /*
     * @method 
     * @public
     * Метод-хук вызывается после обработки нажатия пункта меню
     */
    setExtraParams: function (record) { },

    /*
     * @method prepareTitle
     * Метод делает запрос на сервер (дастает Адресс)
     */
    // ToDo: Этот метод должен реализовыватся тольк одля конкретного контроллера потмоу как аспект используется в разных местах и нетолько в домах 
    prepareTitle: function (comp) {
        B4.Ajax.request({
            url: B4.Url.action('get', 'realityobject'),
            method: 'POST',
            params: { id: this.getObjectId() }
        }).next(function (response) {
            var data = Ext.decode(response.responseText);
            if (data.data && data.data.Address) {
                comp.update({ text: data.data.Address });
            }
        });
    },

    /*
     *  @method initDeployInfo
     *  @private
     */
    initDeployInfo: function (controller, key) {
        var me = this,
            depViewKeys = {},
            fn = me.deployTabs.bind(me, me.tabContainer);
        if (key) {
            depViewKeys[key] = 'deployTabs';
            controller.deployTabs = fn;
            controller.deployViewKeys = depViewKeys;
        }
    },

    /*
     *  @method deployTabs
     *  @private
     */
    deployTabs: function (sel, controller, view) {
        var me = this,
            container = me.componentQuery(sel),
            viewSelector = Ext.String.format('#{0}', view.getItemId());
        if (!container.down(viewSelector)) {
            container.add(view);
        }
        container.setActiveTab(view);
    }
});