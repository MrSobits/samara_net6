Ext.define('B4.aspects.GkhInlineGrid', {
    extend: 'B4.aspects.InlineGrid',

    alias: 'widget.gkhinlinegridaspect',

    saveButtonSelector: null,

    // Проверить обязательность заполнения полей
    requiredFields: false,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'beforeaddrecord'
        );
    },

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.saveButtonSelector] = { click: { fn: this.save, scope: this} };

        this.otherActions(actions);

        controller.control(actions);
    },
    
    otherActions: function () {
        //Данный метод служит для перекрытия в контроллерах где используется данный аспект
        //наслучай если потребуется к данному аспекту добавить дополнительные обработчики
    },

    addRecord: function () {
        var plugin,
            store = this.getStore(),
            rec = this.getModel(),
            grid = this.getGrid();

        this.fireEvent('beforeaddrecord', this, rec);

        store.insert(0, rec);

        if (this.cellEditPluginId && grid) {
            plugin = grid.getPlugin(this.cellEditPluginId);
            plugin.startEditByPosition({ row: 0, column: this.firstEditColumnIndex });
        }
    },

    getModel: function () {
        var me = this;
        if (Ext.isString(me.modelName)) {
            return me.controller.getModel(me.modelName).create();
        }
        if (Ext.isObject(me.modelName) && me.modelName.isModel == true) {
            return me.modelName;
        }
        return me.getStore().model.create();
    },
    
    save: function () {
        var me = this,
            store = me.getStore(),
            grid;

        var modifiedRecs = store.getModifiedRecords();
        var removedRecs = store.getRemovedRecords();
        if (modifiedRecs.length > 0 || removedRecs.length > 0) {
            grid = me.getGrid();

            if (me.requiredFields && modifiedRecs.length > 0) {
                var result = me.requiredFieldsCheck(grid, modifiedRecs);
                
                if (result.success === false) {
                    Ext.Msg.alert('Ошибка сохранения', 'Не заполнено одно или несколько обязательных полей: ' + result.requiredFields.join(', '));
                    return;
                }
            }

            if (me.fireEvent('beforesave', me, store) !== false) {
                if (grid && grid.container) {
                    me.mask('Сохранение', grid);
                } else {
                    me.mask('Сохранение');
                }

                store.sync({
                    callback: function () {
                        me.unmask();
                        store.load();
                    },
                    // выводим сообщение при ошибке сохранения
                    failure: function (result) {
                        me.unmask();
                        if (result && result.exceptions[0] && result.exceptions[0].response) {
                            Ext.Msg.alert('Ошибка!', Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                        }
                    }
                });
            }
        }
    },

    requiredFieldsCheck: function (grid, modifiedRecs) {
        var success = true,
            requiredFields = [];

        grid.columns.forEach(function (column) {
            var editor = column.getEditor(),
                fieldName = column.dataIndex,
                fieldText = column.text;

            if (editor && modifiedRecs.some(function (record) { return editor.allowBlank === false && Ext.isEmpty(record.get(fieldName)) })) {
                success = false;
                requiredFields.push(Ext.String.format('<br><b>{0}</b>', fieldText));
            }
        });

        return { success: success, requiredFields: requiredFields };
    }
});