/*
 * Данный аспект описывает события для массовой формы выбора элементов.
 * Используется вместе с полем типа gkhtriggerfield 
 */
Ext.define('B4.aspects.GkhMultiSelectWindow', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gkhmultiselectwindowaspect',

    requires: ['B4.view.SelectWindow.MultiSelectWindow'],

    multiSelectWindow: null,
    multiSelectWindowSelector: null,
    multiSelectWindowWidth: null,

    storeSelect: null,
    storeSelectSelector: null,
    storeSelected: null,

    columnsGridSelect: null,
    columnsGridSelected: null,

    titleSelectWindow: null,
    titleGridSelect: null,
    titleGridSelected: null,

    leftTopToolbarConfig: {}, // настройки тулбара для левого грида
    leftGridConfig: {}, // настройки для грида, в котором выбираем
    rightGridConfig: {}, // настройик для грида, куда выбранные упадут
    saveButtonText: null, // текстовка кнопки сохранения/применения изменений
    toolbarItems: [], // доп. элементы в тулбар. Справа от кнопки Применить
    isPaginable: true, // нужно ли рисовать paging toolbar

    selectGridSelector: '#multiSelectGrid',

    /*
     * Так как у перечислений нет поля Id, нам нужна возможность переопределить поле по которому мы будем проверять
     * наличие уже выбраных записей.
     */
    idProperty: 'Id',

    /*
     * Если нужен выбор только 1 дома, то ставить значение 'SINGLE'. 
     * в этом случае при нажатии на "Выбрать все" выделится только последняя запись
     */
    selModelMode: 'MULTI',

    controller: null,

    constructor: function(config) {
        var me = this;

        Ext.apply(me, config);

        me.callParent(arguments);
        me.addEvents(
            'getdata',
            'panelrendered'
        );
    },

    init: function(controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[me.multiSelectWindowSelector] = { 'selectedgridrowactionhandler': { fn: me.selectedGridRowActionHandler, scope: me } };
        actions[me.multiSelectWindowSelector + ' b4savebutton'] = { 'click': { fn: me.onSelectRequestHandler, scope: me } };
        actions[me.multiSelectWindowSelector + ' b4closebutton'] = { 'click': { fn: me.onCloseWindowHandler, scope: me } };
        actions[me.multiSelectWindowSelector + ' ' + me.selectGridSelector] = {
            'select': { fn: me.onRowSelect, scope: me },
            'deselect': { fn: me.onRowDeselect, scope: me }
        };
        actions[me.multiSelectWindowSelector + ' ' + me.selectGridSelector + ' b4pagingtoolbar'] = { 'change': { fn: me.onPageChange, scope: me } };
        actions[me.multiSelectWindowSelector + ' #multiSelectedGrid'] = { 'rowaction': { fn: me.selectedGridRowAction, scope: me } };
        actions[me.multiSelectWindowSelector + ' b4updatebutton'] = { 'click': { fn: me.updateSelectGrid, scope: me } };
        actions[me.multiSelectWindowSelector + ' b4deletebutton[action=deselectAll]'] = { 'click': { fn: me.onDeselectAll, scope: me } };

        me.otherActions(actions);

        controller.control(actions);
    },

    /*
     * Данный метод служит для перекрытия в контроллерах где используется данный аспект
     * наслучай если потребуется к данному аспекту добавить дополнительные обработчики
     */
    otherActions: function(actions) {},

    /*
     * этот метод перекрывается в аспекте там где нужно передать параметры стору
     */
    onBeforeLoad: function(store, operation) {},

    /*
    * этот метод перекрывается в аспекте там где нужно передать параметры стору
    */
    onLoad: function(store, operation) {},

    /*
     * Этот метод перекрывается в аспекте там где нужно передать параметры стору
     */
    onSelectedBeforeLoad: function(store, operation) {},

    formAfterrender: function(panel, eOpts) {
        this.fireEvent('panelrendered', this, { window: panel });
    },

    getForm: function() {
        var me = this,
            win,
            stSelected,
            stSelect,
            selectGrid;

        if (me.componentQuery) {
            win = me.componentQuery(me.multiSelectWindowSelector);
        }

        if (!win) {
            win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0];
        }

        if (win && !win.getBox().width) {
            win = win.destroy();
        }

        if (!win) {

            if (Ext.isEmpty(me.storeSelected)) {
                me.storeSelected = me.storeSelect;
            }

            stSelected = me.storeSelected instanceof Ext.data.AbstractStore ? me.storeSelected : Ext.create('B4.store.' + me.storeSelected);
            stSelected.on('beforeload', me.onSelectedBeforeLoad, me);

            stSelect = me.storeSelect instanceof Ext.data.AbstractStore ? me.storeSelect : Ext.create('B4.store.' + me.storeSelect);
            stSelect.on('beforeload', me.onBeforeLoad, me);
            stSelect.on('load', me.onLoad, me);

            win = me.controller.getView(me.multiSelectWindow).create({
                itemId: me.multiSelectWindowSelector.replace('#', ''),
                storeSelect: stSelect,
                storeSelected: stSelected,
                columnsGridSelect: me.columnsGridSelect,
                columnsGridSelected: me.columnsGridSelected,
                title: me.titleSelectWindow,
                titleGridSelect: me.titleGridSelect,
                titleGridSelected: me.titleGridSelected,
                leftTopToolbarConfig: me.leftTopToolbarConfig,
                leftGridConfig: me.leftGridConfig,
                rightGridConfig: me.rightGridConfig,
                saveButtonText: me.saveButtonText,
                toolbarItems: me.toolbarItems,
                selModelMode: me.selModelMode,
                isPaginable: me.isPaginable,
                constrain: true,
                modal: true,
                closeAction: 'destroy',
                renderTo: B4.getBody().getActiveTab().getEl(),
                ctxKey: me.hasContext() ? me.controller.getCurrentContextKey() : null,
                listeners: {
                    afterrender: {
                        fn: me.formAfterrender,
                        scope: me
                    }
                }
            });

            if (Ext.isNumber(me.multiSelectWindowWidth) && win.setWidth) {
                win.setWidth(me.multiSelectWindowWidth);
            }
            
            //Короче Данная форма масоового выбора Может быть одновременно открыта
            //В нескольких вкладках, соответсвенно Если Открыть 1ю форму и отсортировать по колонке
            //Затем открыть следующую форму массового выбора, где такой отсортированной колонки НЕТ,
            //То возникает такая ситуация Что в только что созданный стор Сортировка всеравно попадает
            //И соответтсвенно после первого вызова load у Стора на серверный метод уходит также и сортировка (Которой быть там недолжно)
            //Благодаря такой очистке мы можем Открывать сколько угодно одновременных форм и фильтроват ьи сортировать их независимо друг от друга
            stSelected.sorters.clear();
            stSelect.sorters.clear();
        }

        return win;
    },

    /*
     * Метод для перекрытия
     * Обработчик события смены страницы
     */
    onPageChange: function() {},

    getSelectGrid: function() {
        var me = this,
            win;

        if (me.componentQuery) {
            win = me.componentQuery(me.multiSelectWindowSelector);
        }

        if (!win) {
            win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0];
        }

        if (win) {
            return win.down(me.selectGridSelector);
        }
    },

    getSelectedGrid: function() {

        var me = this;

        if (me.componentQuery) {
            win = me.componentQuery(me.multiSelectWindowSelector);
        }

        if (!win) {
            win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0];
        }

        if (win) {
            return win.down('#multiSelectedGrid');
        }
    },

    /*
     * В данном методе мы просто файрим событие гриду
     */
    selectedGridRowActionHandler: function (action, record) {
        var me = this,
            gridSelected = this.getSelectedGrid();
        
        if (gridSelected) {
            gridSelected.fireEvent('rowaction', gridSelected, action, record);
            me.changeTotalValue();
        }
    },

    selectedGridRowAction: function(grid, action, record) {
        var me = this;
        switch (action.toLowerCase()) {
        case 'delete':
            grid.getStore().remove(record);
            me.changeTotalValue();
            break;
        }
    },

    /*
     * Здесь событие beforeload вешается именно так с параметром single: true
     * Для того чтобы параметр передавался только 1 раз, поскольку есть места где необходимо
     * чтобы один и тот же стор принимал разные параметры, если не сделать single: true то получится каша
     * при первом вызове передастся один параметр при следующем вызове передастся другой параметр.
     * при такой схеме сейчас проблема возникает только при сортировке в гриде на колонке
     */
    updateSelectedGrid: function() {
        var grid = this.getSelectedGrid();
        if (grid) {
            grid.getStore().load();
        }
    },

    /*
     * Читай комент из updateSelectedGrid, тут по аналогии
     */
    updateSelectGrid: function() {
        var grid = this.getSelectGrid();
        if (grid) {
            grid.getStore().load();
        }
    },

    validateRowSelect: function() {
        return true;
    },

    onRowSelect: function (rowModel, record) {
        if (this.validateRowSelect()) {
            var me = this,
            grid = me.getSelectedGrid(),
            storeSelected;

            if (grid) {
                storeSelected = grid.getStore();

                if (storeSelected.find(me.idProperty, record.get(me.idProperty), 0, false, false, true) == -1) {
                    storeSelected.add(record);
                    me.changeTotalValue();
                }
            }
        }
    },

    /*
     * Обработчик события отмены выбора
     * удаляет строку из грида выбранных элементов
     */
    onRowDeselect: function(rowModel, record) {
        var me = this,
            grid = me.getSelectedGrid(),
            storeSelected;

        if (grid) {
            storeSelected = grid.getStore();
            storeSelected.removeAt(storeSelected.find(me.idProperty, record.get(me.idProperty), 0, false, false, true));
            me.changeTotalValue();
        }
    },
    
    /*
     * Перекрываем метод сохранения
     */
    onSelectRequestHandler: function() {
        var me = this,
            grid = me.getSelectedGrid(),
            storeSelected;

        if (grid) {
            storeSelected = grid.getStore();

            if (me.fireEvent('getdata', this, storeSelected.data)) {
                me.getForm().close();
            }
        }
    },

    onCloseWindowHandler: function() {
        var form = this.getForm();
        if (form) {
            form.close();
        }
    },

    /*
    * метод отображения количества записей в гриде
    */
    changeTotalValue: function() {
        var me = this,
            grid = me.getSelectedGrid(),
            selectedStore;

        if (grid) {
            selectedStore = grid.getStore();
            if (grid.down('[ref=status]')) {
                grid.down('[ref=status]').setText(selectedStore.getCount() + ' записи');
                
            }
        }
    },

    onDeselectAll: function() {
        this.getSelectGrid().getSelectionModel().b4deselectAll();
    }
});