Ext.define('B4.aspects.StateTransfer', {
    extend: 'B4.base.Aspect',

    alias: 'widget.statetransferaspect',

    typeComboBoxSelector: '#fiasTransferPanel #cbType',
    roleComboBoxSelector: '#fiasTransferPanel #cbRole',
    gridSelector: '#fiasTransferPanel #stateTransferGrid',
    panelSelector: '#fiasTransferPanel',

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        //Смена значения в комбобоксе 'Тип объекта'
        actions[this.typeComboBoxSelector] = { 'change': { fn: this.onTypeChanged, scope: this } };

        //Смена значения в комбобоксе 'Роль'
        actions[this.roleComboBoxSelector] = { 'change': { fn: this.onRoleChanged, scope: this } };

        //Перед показом Эдитора у ячейки грида переходов
        actions[this.gridSelector] = { 'beforeedit': { fn: this.onBeforeEdit, scope: this } };

        //Нажатие на 'Сохранить'
        actions[this.panelSelector + ' #saveBtn'] = { 'click': { fn: this.onSave, scope: this } };

        controller.control(actions);
    },

    getPanel: function () {
        return Ext.ComponentQuery.query(this.panelSelector)[0];
    },

    getRoleComboBox: function () {
        return Ext.ComponentQuery.query(this.roleComboBoxSelector)[0];
    },

    getTypeComboBox: function () {
        return Ext.ComponentQuery.query(this.typeComboBoxSelector)[0];
    },

    getGrid: function () {
        return Ext.ComponentQuery.query(this.gridSelector)[0];
    },

    onSave: function (btn) {
        var me = this;
        var roleId = this.getRoleComboBox().getValue();
        var typeId = this.getTypeComboBox().getValue();

        if (!roleId || !typeId) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать Тип объекта и Роль!');
        }

        btn.disable();

        var store = this.getGrid().getStore();
        var values = [];
        store.each(function (record) {
            var id = record.data.Id;
            Ext.iterate(record.data, function (key, value) {
                if (value == true && key.indexOf('Id_') != -1) {
                    var newId = parseInt(key.substring(3, key.length));
                    values.push({ CurrentState: id, NewState: newId, Role: roleId, TypeId: typeId });
                }
            });
        });

        Ext.Ajax.request({
            url: B4.Url.action('/StateTransfer/SaveTransfer'),
            params: {
                typeId: typeId,
                roleId: roleId,
                values: Ext.JSON.encode(values)
            },
            success: function () {
                btn.enable();
                me.changeFilter(typeId, roleId);
                Ext.Msg.alert('Сохранение!', 'Сохранение переходов статусов сохранено успешно');
            },
            failure: function () {
                btn.enable();
                me.changeFilter(typeId, roleId);
                Ext.Msg.alert('Сохранение!', 'Не удалось сохранить переходы');
            }
        });
    },

    onBeforeEdit: function (editor, e) {
        return e.column.id != e.record.data.Id;
    },

    onTypeChanged: function (field, newValue) {
        var roleId = this.getRoleComboBox().getValue();
        if (newValue && roleId > 0)
            this.changeFilter(newValue, roleId);
    },

    onRoleChanged: function (field, newValue) {
        if (newValue > 0) {
            this.getTypeComboBox().setValue({});
            this.getStateObjects(newValue);
        }
    },

    getStateObjects: function (roleId) {
        var typeComboboxStore = this.getTypeComboBox().getStore();
        Ext.Ajax.request({
            url: B4.Url.action('/State/FiltredStatefulEntityList'),
            params: {
                roleId: roleId
            },
            success: function (response) {
                var json = Ext.JSON.decode(response.responseText);
                if (json) {
                    typeComboboxStore.load(function () { typeComboboxStore.loadData(json.data) });
                };
            }
        });
    },

    changeFilter: function (typeId, roleId) {
        var me = this;
        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action('/StateTransfer/GetStateTransfer'),
            params: {
                typeId: typeId,
                roleId: roleId
            },
            success: function (response) {
                var json = Ext.JSON.decode(response.responseText);
                if (!json.success)
                    me.updateGrid(json.states, json.transfers);
            }
        });
    },

    updateGrid: function (states, transfers) {
        if (states && states.length > 0) {

            var fields = ['Id', 'Name'];
            var data = [];
            var columns = [{ dataIndex: 'Name', header: 'Текущий статус', flex: 1, xtype: 'gridcolumn', sortable: false }];
            Ext.each(states, function (value) {
                var id = value.Id;
                fields.push('Id_' + id);
            }, this);

            Ext.each(states, function (value) {
                var row = {};
                Ext.each(fields, function (v) {
                    row[v] = false;
                });
                row.Id = value.Id;
                row.Name = value.Name;

                data.push(row);
                columns.push({
                    id: value.Id,
                    dataIndex: 'Id_' + value.Id,
                    flex: 1,
                    header: value.Name,
                    xtype: 'gridcolumn',
                    editor: { xtype: 'b4combobox', editable: false, selectOnFocus: true, items: [[false, 'Нет'], [true, 'Да']] },
                    renderer: function (v) { return v ? 'Да' : ''; },
                    sortable: false
                });
            }, this);

            if (transfers && transfers.length > 0) {
                Ext.each(transfers, function (rec) {
                    Ext.each(data, function (value) {
                        if (value.Id === rec.CurrentStateId) {
                            value['Id_' + rec.NewStateId] = true;
                            return false;
                        }
                    });
                }, this);
            }

            var store = Ext.create('Ext.data.Store', {
                autoLoad: false,
                fields: fields
            });

            if (data.length > 0) {
                Ext.each(data, function (value) {
                    store.add(value);
                });
            }

            var grid = this.getGrid();
            grid.reconfigure(store, columns);
            grid.doLayout();
        }
    }
});