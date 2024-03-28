Ext.define('B4.aspects.GkhEditPanel', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gkheditpanel',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    requires: ['B4.QuickMsg'],
    
    editPanelSelector: null,

    modelName: null,

    controller: null,

    objectId: 0,

    constructor: function (config) {
        var me = this;
        Ext.apply(me, config);
        me.callParent(arguments);

        me.addEvents(
            'beforesaverequest',
            'beforesetdata',
            'beforesetpaneldata',
            'aftersetpaneldata',
            'buttonaction',
            'getdata',
            'validate',
            'beforesave',
            'savesuccess',
            'savefailure'
        );

        me.on('aftersetpaneldata', me.afterSetPanelData, me);
    },

    init: function (controller) {
        var me = this,
            actions = {};
        
        me.callParent(arguments);

        me.controller = controller;

        actions[me.editPanelSelector + ' b4savebutton'] = { 'click': { fn: me.saveRequestHandler, scope: me } };

        actions[me.editPanelSelector + ' b4addbutton'] = { 'click': { fn: me.btnAction, scope: me } };

        me.otherActions(actions);

        controller.control(actions);
    },

    otherActions: function () {
        //Данный метод служит для перекрытия в контроллерах где используется данный аспект
        //наслучай если потребуется к данному аспекту добавить дополнительные обработчики
    },

    getPanel: function () {
        //return Ext.ComponentQuery.query(this.editPanelSelector)[0];
        return this.componentQuery(this.editPanelSelector);
    },

    btnAction: function (btn) {
        this.fireEvent('buttonaction', this, btn.actionName);
    },

    setData: function(objectId) {
        var me = this,
            panel = me.getPanel(),
            id, model;
        
        panel.setDisabled(true);
        
        if (me.fireEvent('beforesetdata', me, objectId) !== false) {
            me.objectId = objectId;
            id = objectId > 0 ? objectId : 0;

            model = me.getModel();

            id > 0 ? model.load(id, {
                success: function(rec) {
                    me.setPanelData(rec);
                },
                scope: me
            }) : me.setPanelData(new model({ Id: 0 }));
        }
    },

    setPanelData: function (rec) {
        var me = this,
            panel = me.getPanel();
        
        if (panel) {
            if (me.fireEvent('beforesetpaneldata', me, rec, panel) !== false) {
                panel.loadRecord(rec);
            }

            if (me.fireEvent('aftersetpaneldata', me, rec, panel) !== false) {
                panel.getForm().isValid();
            }
        }
    },

    getRecord: function () {
        var panel = this.getPanel();
        panel.getForm().updateRecord();
        return panel.getForm().getRecord();
    },

    //Данный метод предназначен для того чтобы произвести произвести специфичные действия с record
    //Например перед сохранением надо поменять какието поля
    getRecordBeforeSave: function (record) {
        return record;
    },

    getModel: function () {
        return this.controller.getModel(this.modelName);
    },

    saveRequestHandler: function () {
        var me = this,
            rec;
        if (me.fireEvent('beforesaverequest', me) !== false) {
            rec = me.getRecordBeforeSave(me.getRecord());

            me.fireEvent('getdata', me, rec);

            if (me.checkFields()) {
                if (me.fireEvent('validate', me)) {
                    me.saveRecord(rec);
                }
            }
        }
    },

    checkFields: function() {
        var me = this,
            form = me.getPanel().getForm();

        if (form.isValid()) {
            return true;
        }

        var fields = form.getFields();

        var invalidFields = '';

        Ext.each(fields.items, function(field) {
            if (!field.isValid()) {
                invalidFields += '</br>' + field.fieldLabel.replace('</br>', ' ');
            }
        });

        Ext.Msg.alert('Ошибка', 'Не заполнены, или заполнены неверно, обязательные поля: <b>' + invalidFields + '</b>');

        return false;
    },

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

    saveRecordHasUpload: function (rec) {
        var me = this,
            panel = me.getPanel();

        me.mask('Сохранение', panel);
        panel.submit({
            url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
            params: {
                records: Ext.encode([rec.getData()])
            },
            success: function (form, action) {
                me.unmask();
                var model = me.getModel();
                if (action.result.data.length > 0) {
                    var id = action.result.data[0] instanceof Object ? action.result.data[0].Id : action.result.data[0];
                    model.load(id, {
                        success: function (newRec) {
                            me.onPreSaveSuccess(me, newRec);
                            me.setPanelData(newRec);
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

    onPreSaveSuccess: function(asp, record) {
        asp.fireEvent('savesuccess', asp, record);
        B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
    },

    saveRecordHasNotUpload: function (rec) {
        var me = this,
            panel = me.getPanel();

        me.mask('Сохранение', panel);
        rec.save({ id: rec.getId() })
            .next(function (result) {
                me.unmask();
                var model = me.getModel();
                model.load(result.record.getId(), {
                        success: function (newRec) {
                            me.onPreSaveSuccess(me, newRec);
                            me.setPanelData(newRec);
                        },        
                        failure: function (response) {
                            var obj = Ext.decode(response.responseText);
                            me.unmask();
                            Ext.Msg.alert('Ошибка', obj.message || 'Ошибка сохранения!');
                        }
                    });
            })
            .error(function (result) {
                me.unmask();
                me.fireEvent('savefailure', result.record, result.responseData);
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            });
    },

    hasUpload: function () {
        return this.getPanel().getForm().hasUpload();
    },

    isFileLoad: function () {
        return !!this.getPanel().getForm().getFields().findBy(function (f) {
            return Ext.isFunction(f.isFileLoad) && f.isFileLoad();
        });
    },

    afterSetPanelData: function (aspect, rec, panel) {
        panel.setDisabled(false);
    }
});