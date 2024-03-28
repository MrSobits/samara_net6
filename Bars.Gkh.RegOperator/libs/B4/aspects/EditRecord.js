Ext.define('B4.aspects.EditRecord', {
    extend: 'B4.base.Aspect',
    alias: 'widget.editrecordaspect',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    requires: ['B4.mixins.MaskBody'],

    modelName: null,
    formWindowCls: null,
    formWindowSelector: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);
        this.addEvents(
           'aftersetformdata',
           'beforesetformdata',
           'aftersetformdata',
           'beforesaverequest',
           'getdata',
           'validate',
           'beforesave',
           'savesuccess',
           'deletesuccess',
           'savefailure'
       );

        this.on('aftersetformdata', this.onAfterSetFormData, this);

        this.on('savesuccess', this.onSaveSuccess, this);
    },

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        if (me.formWindowSelector) {
            actions[me.formWindowSelector + ' b4closebutton'] = {
                'click': {
                    fn: me.closeWindowHandler,
                    scope: me
                }
            };
        }

        if (me.formWindowSelector) {
            actions[me.formWindowSelector + ' b4savebutton'] = {
                'click': {
                    fn: me.saveRequestHandler,
                    scope: me
                }
            };
        }

        controller.control(actions);
    },

    getModel: function () {
        return this.controller.getModel(this.modelName);
    },

    editRecord: function (record) {
        var me = this,
            id = record ? record.getId() : null,
            model;

        model = me.getModel(record);

        id ? model.load(id, {
            success: function (rec) {
                me.setFormData(rec);
            },
            scope: me
        }) : me.setFormData(new model({ Id: 0 }));

        me.getForm().getForm().isValid();
    },

    getForm: function () {
        var me = this,
            editWindow;

        if (me.formWindowSelector) {
            editWindow = me.componentQuery(me.formWindowSelector);

            if (editWindow && !editWindow.getBox().width) {
                editWindow = editWindow.destroy();
            }

            if (!editWindow) {

                editWindow = me.controller.getView(me.formWindowCls).create(
                    {
                        constrain: true,
                        renderTo: B4.getBody().getActiveTab().getEl(),
                        closeAction: 'destroy',
                        ctxKey: me.controller.getCurrentContextKey ? me.controller.getCurrentContextKey() : ''
                    });
            }

            return editWindow;
        } else {
            throw new Error('[' + Ext.getDisplayName(arguments.callee) + '] Не указан selector для окна редактирования.');
        }
    },

    setFormData: function (rec) {
        var me = this,
            form = this.getForm();
        if (me.fireEvent('beforesetformdata', me, rec, form) !== false) {
            form.loadRecord(rec);
            form.getForm().updateRecord();
            form.getForm().isValid();
        }

        me.fireEvent('aftersetformdata', me, rec, form);
    },

    closeWindowHandler: function () {
        var form = this.getForm();
        if (form) {
            form.close();
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


    /**
   * @method getRecordBeforeSave
   * Данный метод предназначен для того чтобы произвести специфичные действия с record
   * Например перед сохранением надо поменять какие-то поля.
   * @template
   * @param record Запись стора грида
   */
    getRecordBeforeSave: function (record) {
        return record;
    },

    /**
     * @method saveRequestHandler
     */
    saveRequestHandler: function () {
        var me = this,
            from = this.getForm(),
            rec,
            fields,
            invalidFields;
        if (me.fireEvent('beforesaverequest', me) !== false) {
            from.getForm().updateRecord();
            rec = me.getRecordBeforeSave(from.getRecord());

            me.fireEvent('getdata', me, rec);

            if (from.getForm().isValid()) {
                if (me.fireEvent('validate', me)) {
                    me.saveRecord(rec);
                }
            } else {
                //получаем все поля формы
                fields = from.getForm().getFields();

                invalidFields = '';

                //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
                Ext.each(fields.items, function (field) {
                    if (!field.isValid()) {
                        invalidFields += '<br>' + field.fieldLabel;
                    }
                });

                //выводим сообщение
                Ext.Msg.alert('Ошибка сохранения!', 'Не заполнены обязательные поля: ' + invalidFields);
            }
        }
    },

    /**
     * @method saveRecord
     */
    saveRecord: function (rec) {
        var me = this;
        if (me.fireEvent('beforesave', me, rec) !== false) {
            if (me.hasUpload()) {
                me.saveRecordHasUpload(rec);
            } else {
                me.saveRecordHasNotUpload(rec);
            }
        }
    },

    /**
     * @method saveRecordHasUpload
     */
    saveRecordHasUpload: function (rec) {
        var me = this;
        var frm = me.getForm();
        me.mask('Сохранение', frm);
        frm.submit({
            url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
            params: {
                records: Ext.encode([rec.getData()])
            },
            success: function (form, action) {
                me.unmask();

                var model = me.getModel(rec);

                if (action.result.data.length > 0) {
                    var id = action.result.data[0] instanceof Object ? action.result.data[0].Id : action.result.data[0];
                    model.load(id, {
                        success: function (newRec) {
                            me.setFormData(newRec);
                            me.fireEvent('savesuccess', me, newRec);
                        }
                    });
                }
            },
            failure: function (form, action) {
                me.unmask();
                me.fireEvent('savefailure', rec, action.result.message);
                Ext.Msg.alert('Ошибка сохранения!', action.result.message);
            }
        });
    },

    /**
     * @method saveRecordHasNotUpload
     */
    saveRecordHasNotUpload: function (rec) {
        var me = this,
            frm = me.getForm();
        me.mask('Сохранение', frm);
        rec.save({ id: rec.getId() })
            .next(function (result) {
                me.unmask();
                me.fireEvent('savesuccess', me, result.record);
            })
            .error(function (result) {
                me.unmask();
                me.fireEvent('savefailure', result.record, result.responseData);

                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            });
    },

    /**
     * @method hasUpload
     */
    hasUpload: function () {
        return this.getForm().getForm().hasUpload();
    }
});