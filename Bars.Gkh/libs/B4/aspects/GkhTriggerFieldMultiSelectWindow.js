/*
   Данный аспект предназначен для описания взаимодействия триггер-поля и формы массового выбора элементов. Метод сохранения пустой,
   поскольку его обработку необходимо писать в тех контроллерах где этот аспект применяется.
   Преминяется там где необходимо чтобы при нажатии на кнопку добавить показалась форма массового выбора
   В контроллерах необходимо вешаться на событие getdata и получив массив записей произвести свои действия
*/
Ext.define('B4.aspects.GkhTriggerFieldMultiSelectWindow', {
    extend: 'B4.aspects.GkhMultiSelectWindow',
    alias: 'widget.gkhtriggerfieldmultiselectwindowaspect',

    fieldSelector: null,
    valueProperty: 'Id',
    textProperty: 'Name',

    constructor: function (config) {
        var me = this;

        Ext.apply(me, config);
        me.callParent(arguments);
    },

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[me.fieldSelector] = { 'triggerOpenForm': { fn: me.triggerOpenForm, scope: me } };

        controller.control(actions);
    },

    triggerOpenForm: function () {
        var me = this,
            field = me.getSelectField(),
            grid;

        me.getForm().show();
        if (field && !!field.getValue()) {
            me.updateSelectedGrid();
            return;
        }
        else {
            grid = me.getSelectedGrid();
            if (grid) {
                grid.getStore().removeAll();
            }
        }

        me.updateSelectGrid();
    },

    updateSelectedGrid: function () {
        var me = this,
            grid = me.getSelectedGrid();
        if (grid) {
            grid.getStore().load({
                scope: me,
                callback: me.updateSelectGrid
            });

        }
    },

    onBeforeLoad: function (store, operation) {
        this.setIgnoreChanges(true);
    },

    onLoad: function (store, operation) {
        this.setIgnoreChanges(false);
    },

    getIgnoreChanges: function () {
        return this.controller.ignoreChanges;
    },

    setIgnoreChanges: function (value) {
        this.controller.ignoreChanges = value;
    },

    getSelectField: function () {
        return this.componentQuery(this.fieldSelector);
    },

    onSelectedBeforeLoad: function (store, operation) {
        var me = this,
            field = me.getSelectField();

        if (field) {
            operation.params[me.valueProperty] = field.getValue();
        }
    },

    /*
     * В данном методе мы просто файрим событие гриду
     */
    selectedGridRowActionHandler: function (action, record) {
        var me = this,
            gridSelect = me.getSelectGrid(),
            gridSelected = me.getSelectedGrid();

        if (gridSelected) {
            gridSelected.fireEvent('rowaction', gridSelected, action, record);
            gridSelect.getSelectionModel().deselect(gridSelect.getStore().find(me.idProperty, record.getId()));
            me.changeTotalValue();
        }
    },

    onRowSelect: function (rowModel, record) {
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
    },

    onRowDeselect: function (rowModel, record) {
        var me = this,
            grid = me.getSelectedGrid(),
            storeSelected;

        if (grid && !me.getIgnoreChanges()) {
            storeSelected = grid.getStore();
            storeSelected.removeAt(storeSelected.find(me.idProperty, record.get(me.idProperty), 0, false, false, true));
            me.changeTotalValue();
        }
    },

    onPageChange: function () {
        var me = this,
            gridSelect = me.getSelectGrid(),
            gridSelected = me.getSelectedGrid(),
            storeSelect = gridSelect.getStore(),
            storeSelected = gridSelected.getStore(),
            records = [];

        if (storeSelected.getCount()) {
            storeSelected.data.each(function(edRec) {
                storeSelect.data.each(function(tRec) {
                    if (tRec.get('Id') === edRec.get('Id')) {
                        records.push(tRec);
                    }
                }, me);
            }, me);

            if (records.length > 0) {
                gridSelect.getSelectionModel().select(records);
                me.changeTotalValue();
            }
        }
    },

    parseRecord: function (record, property) {
        var propertyValue = '';
        if (record && record.items[0]) {
            propertyValue = record.items[0].getData()[property];
            for (var i = 1; i <= record.items.length - 1; i++) {
                propertyValue += ", " + record.items[i].getData()[property];
            }
        }
        return propertyValue;
    },

    /*
     * Перекрываем метод сохранения,
     * в качестве данных возвращаются records из выбранных
     */
    onSelectRequestHandler: function () {
        var me = this,
            grid = me.getSelectedGrid(),
            storeSelected,
            field;

        if (grid) {
            storeSelected = grid.getStore();

            if (me.fireEvent('getdata', me, storeSelected.data)) {
                field = me.getSelectField();
                field.updateDisplayedText(me.parseRecord(storeSelected.data, me.textProperty));
                field.setValue(me.parseRecord(storeSelected.data, me.valueProperty));
                me.getForm().close();
            }
        }
    },

    changeTotalValue: function () {
        var me = this,
            grid = me.getSelectedGrid(),
            store = me.getSelectedGrid().getStore();

        grid.down('[ref=status]').setText(store.getCount() + ' записи');
    }
});