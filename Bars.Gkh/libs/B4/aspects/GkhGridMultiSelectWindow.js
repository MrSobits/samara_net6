/*
   Данный аспект предназначен для описания взаимодействия грида и формы массового выбора элементов. Сохранение здесь пустой метод
   поскольку обработку сохранения необходимо писать в тех контроллерах где этот аспект применяется.
   Данный аспект преминяется там где необходимо чтобы при нажатии на кнопку добавить показалась форма массового выбора
   Но в контроллерах необходимо вешаться на событие getdata и получив массив записей произвести свои действия
*/

Ext.define('B4.aspects.GkhGridMultiSelectWindow', {
    extend: 'B4.aspects.GkhMultiSelectWindow',

    alias: 'widget.gkhgridmultiselectwindowaspect',

    gridSelector: null,
    editFormSelector: null,
    editWindowView: null,
    storeName: null,
    modelName: null,
    addButtonSelector: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'beforerowaction',
            'beforegridaction',
            'beforesetformdata',
            'aftersetformdata',
            'beforesaverequest',
            'validate',
            'beforesave',
            'savesuccess',
            'savefailure',
            'geteditdata'
        );
    },

    init: function (controller) {
        var me = this,
            actions = {};
        me.callParent(arguments);

        actions[me.gridSelector] = {
            'rowaction': { fn: me.rowAction, scope: me },
            'gridaction': { fn: me.gridAction, scope: me },
            'itemdblclick': { fn: me.rowDblClick, scope: me }
        };

        actions[me.addButtonSelector || (me.gridSelector + ' b4addbutton')] = { 'click': { fn: me.btnAction, scope: me } };

        actions[me.gridSelector + ' b4updatebutton'] = { 'click': { fn: me.btnAction, scope: me } };

        if (me.editFormSelector) {

            actions[me.editFormSelector + ' b4savebutton'] = { 'click': { fn: me.saveRequestHandler, scope: me } };
            actions[me.editFormSelector + ' b4closebutton'] = { 'click': { fn: me.closeEditWindowHandler, scope: me } };

            me.on('aftersetformdata', me.onAfterSetFormData, me);
            me.on('savesuccess', me.onSaveSuccess, me);
        }

        controller.control(actions);
    },

    btnAction: function (btn) {
        this.getGrid().fireEvent('gridaction', this.getGrid(), btn.actionName);
    },

    getGrid: function () {
        var me = this,
            grid;
        
        if (me.componentQuery) {
            grid =  me.componentQuery(me.gridSelector);
        }

        if (!grid) {
            grid = Ext.ComponentQuery.query(me.gridSelector)[0];
        }

        return grid;
    },

    getEditForm: function () {
        var me = this,
            editWindow;

        if (me.editFormSelector) {
            
            if (me.componentQuery) {
                editWindow = me.componentQuery(me.editFormSelector);
            }
            
            if (!editWindow) {
                editWindow = Ext.ComponentQuery.query(me.editFormSelector)[0];   
            }
            
            if (!editWindow) {
                
                editWindow = me.controller.getView(me.editWindowView).create(
                {
                    constrain: true,
                    autoDestroy: true,
                    closeAction: 'destroy',
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    ctxKey: me.hasContext() ? me.controller.getCurrentContextKey() : null
                });

                editWindow.show();
            }

            return editWindow;
        }
    },

    rowDblClick: function (view, record) {
        if (this.editFormSelector)
            this.editRecord(record);
    },
    
    rowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'edit':
                if (this.editFormSelector)
                    this.editRecord(record);
                break;
            case 'delete':
                this.deleteRecord(record);
                break;
        }
    },

    gridAction: function (grid, action) {
        var me = this;
        if (me.fireEvent('beforegridaction', me, grid, action) !== false) {
            switch (action.toLowerCase()) {
            case 'add':
                {
                    me.getForm().show();
                    var grid = me.getSelectedGrid();
                    if (grid)
                        grid.getStore().removeAll();
                    me.updateSelectGrid();
                }
                break;
            case 'update':
                me.updateGrid();
                break;
            }
        }
    },
    
    editRecord: function (record) {
        var me = this,
            id = record ? record.getId() : null,
            model;

        model = me.controller.getModel(me.modelName);

        id ? model.load(id, {
            success: function(rec) {
                me.setFormData(rec);
            },
            scope: this
        }) : me.setFormData(new model({ Id: 0 }));
    },

    setFormData: function (rec) {
        var me = this,
            form = me.getEditForm();
        if (me.fireEvent('beforesetformdata', me, rec, form) !== false) {
            me.getEditForm().loadRecord(rec);
        }

        me.fireEvent('aftersetformdata', me, rec, form);
    },
    
    deleteRecord: function (record) {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(res) {
            if (res == 'yes') {
                record.destroy()
                    .next(function() {
                        me.updateGrid();
                    }, me)
                    .error(function(result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, me);
            }
        }, me);
    },

    updateGrid: function () {
        this.getGrid().getStore().load();
    },

    closeEditWindowHandler: function () {
        this.getEditForm().close();
    },

    saveRequestHandler: function () {
        var me = this,
            from = me.getEditForm(),
            rec;

        if (me.fireEvent('beforesaverequest', me) !== false) {
            from.getForm().updateRecord();
            rec = from.getRecord();

            me.fireEvent('geteditdata', me, rec);

            if (from.getForm().isValid() && me.fireEvent('validate', me)) {
                me.saveRecord(rec);
            }
        }
    },

    saveRecord: function (rec) {
        var me = this;

        if (me.fireEvent('beforesave', me, rec) !== false) {

            rec.save({ id: rec.getId() })
                .next(function (result) {
                    me.updateGrid();
                    me.fireEvent('savesuccess', me, result.record);
                }, me)
                .error(function (result) {
                    me.fireEvent('savefailure', result.record, result.responseData);

                    Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                }, me);
        }
    },

    onAfterSetFormData: function (aspect, rec, form) {
        form.show();
    },

    onSaveSuccess: function (aspect) {
        //Закрываем окно после добавления новой записи
        aspect.getEditForm().close();
    },

    selectedGridRowActionHandler: function (action, record) {
        var me = this,
            gridSelect = me.getSelectGrid(),
            gridSelected = me.getSelectedGrid(),
            selModel,
            rec;

        if (gridSelect && gridSelected) {
            gridSelected.fireEvent('rowaction', gridSelected, action, record);

            selModel = gridSelect.getSelectionModel();

            switch (action.toLowerCase()) {
                case 'delete':
                    selModel.isSelectedAll = false;

                    rec = selModel.selected.findBy(function(item, a,b,c) {
                        return item.getId() == record.getId();
                    }, me);

                    selModel.deselect(rec);
                    break;
            }
        }
    }
});