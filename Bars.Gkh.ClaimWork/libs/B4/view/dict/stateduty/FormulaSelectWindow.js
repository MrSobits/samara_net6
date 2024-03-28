Ext.define('B4.view.dict.stateduty.FormulaSelectWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.formulaselectwindow',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    width: 500,
    layout: 'form',
    modal: true,
    bodyPadding: 5,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                { xtype: 'textfield', allowBlank: false, fieldLabel: 'Наименование', name: 'Name', flex: 1 },
                { xtype: 'hidden', name: 'Code' },
                {
                    xtype: 'b4selectfield',
                    textProperty: 'DisplayName',
                    idProperty: 'DisplayName',
                    fieldLabel: 'Характеристика',
                    name: 'DisplayName',
                    allowBlank: false,
                    flex: 1,
                    store: Ext.create('B4.store.FormulaParameter', {
                        listeners: {
                            beforeload: function (store, opts) {
                                opts.params.scope = 'StateDuty';
                            }
                        }
                    }),
                    columns: [{ header: 'Наименование', dataIndex: 'DisplayName', flex: 1 }],
                    listeners: {
                        change: function (fld, data) {
                            if (data && data.Code) {
                                fld.up('formulaselectwindow').down('[name="Code"]').setValue(data.Code);
                            }
                        }
                    }
                }
            ],
            tbar: [
                {
                    xtype: 'b4savebutton', handler: function (btn) {
                        var win = btn.up('window'),
                            form = win.getForm(),
                            rec;

                        form.updateRecord();
                        rec = form.getRecord();

                        win.fireEvent('saverequest', win, rec);
                    }
                },
                '->',
                { xtype: 'b4closebutton', handler: function (btn) { btn.up('window').destroy(); } }
            ]
        });

        me.callParent(arguments);
    }
});