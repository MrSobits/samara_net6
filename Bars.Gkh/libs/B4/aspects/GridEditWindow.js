Ext.define('B4.aspects.GridEditWindow', {
    extend: 'B4.aspects.GridEditForm',

    alias: 'widget.grideditwindowaspect',

    editWindowView: null,
    deleteConfirmMessage: 'Вы действительно хотите удалить запись?',

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.on('aftersetformdata', this.onAfterSetFormData, this);

        this.on('savesuccess', this.onSaveSuccess, this);
    },

    init: function (controller) {
        var me = this,
            actions = {};

        me.test = [];

        me.callParent(arguments);

        if (me.editFormSelector) {
            actions[me.editFormSelector + ' b4closebutton'] = {
                'click': {
                    fn: me.closeWindowHandler,
                    scope: me
                }
            };
        }

        controller.control(actions);
    },

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
                        ctxKey: me.controller.getCurrentContextKey ? me.controller.getCurrentContextKey() : ''
                    });

                editWindow.show();
            }
            return editWindow;
        }
    },

    closeWindowHandler: function () {
        var me = this,
            window;
        if (me.editFormSelector) {
            window = me.componentQuery(me.editFormSelector);
            if (window) {
                window.close();
            }
        }
    },

    onAfterSetFormData: function (aspect, rec, form) {
        if (form) {
            form.show();
        }
    },

    onSaveSuccess: function (aspect) {
        var form = aspect.getForm();
        if (form) {
            form.close();
        }
    },

    editRecord: function (record) {
        var me = this,
            id = record ? record.getId() : null,
            model = me.getModel(record);

        if (id) {
            model.load(id, {
                success: function (rec) {
                    me.setFormData(rec);
                },
                scope: me
            });
        } else {
            me.setFormData(new model({ Id: 0 }));
        }
    },

    deleteRecord: function (record) {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', me.deleteConfirmMessage, function (result) {
            if (result == 'yes') {
                var model = this.getModel(record);

                var rec = new model({ Id: record.getId() });
                me.mask('Удаление', B4.getBody());
                rec.destroy()
                    .next(function () {
                        this.fireEvent('deletesuccess', this);
                        me.updateGrid();
                        me.unmask();
                    }, this)
                    .error(function (result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        me.unmask();
                    }, this);
            }
        }, me);
    },
});