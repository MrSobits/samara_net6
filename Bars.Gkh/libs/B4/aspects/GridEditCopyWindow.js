Ext.define('B4.aspects.GridEditCopyWindow', {
    extend: 'B4.aspects.GridEditWindow',

    alias: 'widget.grideditcopywindowaspect',

    /**
     * Массив имен полей record, которые будут скопированы из существующей записи в
     * создаваемую при операции копирования.
     */
    copyFields: [],

    /**
     * Перекрыто для корректной работы с контекстом
     */
    getForm: function () {
        var me = this,
            editWindow;

        if (me.editFormSelector) {
            editWindow = me.componentQuery(me.editFormSelector);

            if (editWindow && !editWindow.getBox().width) {
                editWindow = editWindow.destroy();
            }

            if (!editWindow) {

                editWindow = me.controller.getView(me.editWindowView).create(
                    {
                        constrain: true,
                        renderTo: B4.getBody().getActiveTab().getEl(),
                        closeAction: 'destroy',
                        ctxKey: me.controller.getCurrentContextKey()
                    });

                editWindow.show();
            }

            return editWindow;
        }
    },

    /**
     * @method rowAction
     */
    rowAction: function (grid, action, record) {
        var me = this;
        if (!grid || grid.isDestroyed) {
            return;
        }
        if (me.fireEvent('beforerowaction', me, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'edit':
                    me.editRecord(record);
                    break;
                case 'delete':
                    me.deleteRecord(record);
                    break;
                case 'copy':
                    me.copyRecord(record);
                    break;
            }
        }
    },

    copyRecord: function(record) {
        var me = this,
            model = me.controller.getModel(me.modelName),
            copyRecord;

        copyRecord = new model();

        Ext.Array.each(me.copyFields, function(field) {
            copyRecord.set(field, record.get(field));
        });

        me.setFormData(copyRecord);
    }

});