Ext.define('B4.controller.paysize.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.form.SelectField',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'paysize.Record',
        'paysize.RealEstateType'
    ],

    models: [
        'Paysize',
        'paysize.Record',
        'paysize.RealEstateType'
    ],

    views: [
        'paysize.EditPanel',
        'paysize.RecordGrid',
        'paysize.RetWindow',
        'paysize.RetGrid'
    ],

    aspects: [],

    refs: [
        { ref: 'mainView', selector: 'paysizeeditpanel' }
    ],

    init: function() {
        var me = this;

        me.control({
            'paysizerecordgrid': {
                'render': function(cmp) {
                    var field = cmp.up('paysizeeditpanel').down('[name=Id]');

                    cmp.getStore().on('beforeload', function(store, operation) {
                        operation.params.paysizeId = (field ? field.getValue() : 0);
                    });

                    cmp.getStore().load();
                },
                'rowaction': {
                    fn: me.onGridRowAction,
                    scope: me
                }
            },
            'paysizeeditpanel b4savebutton': {
                'click': {
                    fn: me.onClickSave,
                    scope: me
                }
            },
            'paysizeretgrid': {
                'render': function(cmp) {
                    var field = cmp.up('paysizeretwindow').down('[name=Id]');
                    cmp.getStore().on('beforeload', function(store, operation) {
                        operation.params.recordId = (field ? field.getValue() : 0);
                    });
                }
            },
            'paysizeretwindow b4savebutton' : {
                'click': {
                    fn: me.onClickSaveRets,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function(id) {
        var me = this,
            model = me.getModel('Paysize');

        if (+id) {
            model.load(id, {
                success: function(record) {
                    me.setPanelData(record);
                }
            });
        } else {
            me.setPanelData(new model());
        }
    },

    setPanelData: function (record) {
        var me = this,
            canEdit = record.getId() > 0 && !record.get('HasCharges'),
            isNewRecord = !(record.getId() > 0),
            view = me.getMainView() || Ext.widget('paysizeeditpanel'),
            editor = {
                xtype: 'numberfield',
                decimalSeparator: ',',
                minValue: 0,
                allowDecimal: true
            };

        view.canEdit = canEdit;

        view.down('[name=DateStart]').setDisabled(record.getId() > 0);
        view.down('paysizerecordgrid').down('[dataIndex=Value]').setEditor(canEdit ? editor : null);
        view.down('[name=Warning]').setVisible(!canEdit && !isNewRecord);

        view.loadRecord(record);

        me.bindContext(view);
        me.application.deployView(view);
    },

    onGridRowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'edit':
                this.editPayRecord(record);
                break;
        }
    },

    onClickSaveRets: function(btn) {
        var me = this,
            win = me.getRetWin(),
            store = win.down('paysizeretgrid').getStore();
        me.mask('Сохранение...', win);
        store.sync({
            success: function () {
                me.unmask();
                if (win) {
                    win.close();
                }
            },
            failure: function(b) {
                me.unmask();
                Ext.each(b.exceptions, function(exception) {
                    var error = Ext.decode(exception.response.responseText);
                    B4.QuickMsg.msg('Ошибка сохранения', error.message, 'error');
                });
            }
        });
    },

    editPayRecord: function(record) {
        var me = this,
            win = me.getRetWin();

        win.down('[name=Id]').setValue(record.getId());
        win.show();
        win.down('paysizeretgrid').getStore().load();
    },

    getRetWin: function() {
        var win = Ext.ComponentQuery.query('paysizeretwindow')[0];

        if (!win) {
            win = Ext.create('B4.view.paysize.RetWindow', {
                renderTo: B4.getBody().getActiveTab().getEl()
            });
        }

        return win;
    },

    onClickSave: function() {
        var me = this,
            panel = me.getMainView(),
            recordgrid = panel.down('paysizerecordgrid'),
            record,
            model = me.getModel('Paysize');

        panel.getForm().updateRecord();
        record = panel.getForm().getRecord();

        me.mask('Сохранение', B4.getBody());

        B4.Ajax.request({
            url: B4.Url.action(record.phantom ? 'Create' : 'Update', 'Paysize'),
                method: 'POST',
                timeout: 9999999,
                params: {
                    id: record.getId(),
                    records: Ext.encode([record.data])
                }
            }
        ).next(function (response) {
            var resp = Ext.JSON.decode(response.responseText),
                rec = resp.data[0];
            
            model.load(rec.Id, {
                success: function (res) {
                    me.setPanelData(res);
                }
            });

            me.updateRecords(recordgrid);

            me.unmask();
        }).error(function (e) {
            e = e || {};
            me.unmask();
            Ext.Msg.alert('Ошибка сохранения', Ext.isString(e.message) ? e.message : 'Произошла ошибка при сохранении');
        });
    },

    updateRecords: function(panel) {
        var store = panel.getStore(),
            modifRecords = panel.getStore().getModifiedRecords(),
            records = [];

        Ext.each(modifRecords, function(rec) {
            records.push(rec.data);
        });

        B4.Ajax.request({
            url: B4.Url.action('Update', 'PaysizeRecord'),
            method: 'POST',
            params: {
                records: Ext.JSON.encode(records)
            }
        }).next(function () {
            store.load();
        }).error(function(response) {
            Ext.Msg.alert('Ошибка', response.message);
        });
    }
});