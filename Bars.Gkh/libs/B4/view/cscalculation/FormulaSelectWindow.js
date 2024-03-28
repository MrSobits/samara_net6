Ext.define('B4.view.cscalculation.FormulaSelectWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.cscalculationformulaselectwindow',

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
                { xtype: 'textfield', allowBlank: false, fieldLabel: 'Код', name: 'Code', flex: 1 },
                { xtype: 'textfield', allowBlank: false, fieldLabel: 'Отображаемое наименование', name: 'DisplayName', flex: 1 }
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