/*
   Данный аспект предназначен для взаимодействия инлайн грида и формы массового выбора элементов.
*/

Ext.define('B4.aspects.GkhInlineGridMultiSelectWindow', {
    extend: 'B4.aspects.GkhMultiSelectWindow',

    alias: 'widget.gkhinlinegridmultiselectwindowaspect',

    gridSelector: null,
    storeName: null,
    modelName: null,
    saveButtonSelector: null,

    /**
    * @property {String} cellEditPluginId
    * Идентификатор edit-плагина грида
    * @public
    */
    cellEditPluginId: 'cellEditing',

    /**
    * @property {String} firstEditColumnIndex
    * Номер колонки, с которой начинается редактирование
    * @public
    */
    firstEditColumnIndex: 0,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'beforerowaction',
            'beforegridaction'
        );
    },
    
    editRecord: function () {
    },
    
    init: function (controller) {
        var actions = {};
        this.callParent(arguments);
        
        actions[this.gridSelector] = {
            'rowaction': { fn: this.rowAction, scope: this },
            'gridaction': { fn: this.gridAction, scope: this }
        };

        actions[this.gridSelector + ' b4addbutton'] = { 'click': { fn: this.btnAction, scope: this} };
        actions[this.gridSelector + ' b4savebutton'] = { 'click': { fn: this.btnAction, scope: this} };
        actions[this.gridSelector + ' b4updatebutton'] = { 'click': { fn: this.btnAction, scope: this} };
        actions[this.saveButtonSelector] = { 'click': { fn: this.save, scope: this} };
        controller.control(actions);
    },

    getGrid: function () {
        return Ext.ComponentQuery.query(this.gridSelector)[0];
    },

    btnAction: function (btn) {
        this.getGrid().fireEvent('gridaction', this.getGrid(), btn.actionName);
    },

    rowAction: function (grid, action, record) {
        if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'delete':
                    this.deleteRecord(record);
                    break;
            }
        }
    },

    gridAction: function (grid, action) {
        if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
            switch (action.toLowerCase()) {
                case 'add':
                    {
                        this.getForm().show();
                        var grid = this.getSelectedGrid();
                        if (grid)
                            grid.getStore().removeAll();
                        this.updateSelectGrid();
                    }
                    break;
                case 'update':
                    this.updateGrid();
                    break;
                case 'save':
                    this.save();
                    break;
            }
        }
    },

    deleteRecord: function (record) {
        var me = this,
            store = me.getGrid().getStore();

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                store.remove(record);
            }
        }, me);
    },

    save: function () {
        var store = this.getGrid().getStore();
        
        if (this.beforeSave(this, store) === false) {
            return;
        }

        store.sync({
            callback: function () {
                store.load();
            },
            success: function () {
                B4.QuickMsg.msg('Сохранение записи!', 'Успешно сохранено', 'success');
            },
            failure: function (batch) {
                if (batch.exceptions.length > 0 && batch.exceptions[0].response && batch.exceptions[0].response.responseText) {
                    Ext.Msg.alert('Сохранение записи!', Ext.decode(batch.exceptions[0].response.responseText).message);
                }

            }
        });
    },

    updateGrid: function () {
        this.getGrid().getStore().load();
    },
    
    beforeSave: function() {
        
    },

    selectedGridRowActionHandler: function (action, record) {
        var me = this,
            gridSelect = me.getSelectGrid(),
            gridSelected = me.getSelectedGrid();

        if (gridSelect && gridSelected) {
            gridSelected.fireEvent('rowaction', gridSelected, action, record);
            gridSelect.getSelectionModel().deselect(gridSelect.getStore().find(me.idProperty, record.getId()));
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

        if (grid) {
            storeSelected = grid.getStore();
            storeSelected.removeAt(storeSelected.find(me.idProperty, record.get(me.idProperty), 0, false, false, true));
            me.changeTotalValue();
        }
    },

    changeTotalValue: function () {
        var me = this,
            grid = me.getSelectedGrid(),
            store = me.getSelectedGrid().getStore();

        grid.down('[ref=status]').setText(store.getCount() + ' записи');
    }
});