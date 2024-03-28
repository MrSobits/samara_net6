Ext.define('B4.view.GisGmpPatternEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.store.GisGmpPattern',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField'
    ],

    alias: 'widget.gisgmppatterneditwindow',

    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'gisGmpPatternEditWindow',
    title: 'Шаблон',
    closeAction: 'hide',
    trackResetOnLoad: true,
    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: { labelWidth: 100, labelAlign: 'right', anchor: '100%' },
            items: [
                { xtype: 'textfield', name: 'PatternCode', fieldLabel: 'Код шаблона', allowBlank: false },
                {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    allowBlank: false,
                    editable: false,
                    store: 'B4.store.dict.Municipality'
                },
                { xtype: 'datefield', name: 'DateStart', fieldLabel: 'Дата начала', allowBlank: false },
                { xtype: 'datefield', name: 'DateEnd', fieldLabel: 'Дата окончания', allowBlank: false }
            ],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [
                    { xtype: 'buttongroup', columns: 1, items: [{ xtype: 'b4savebutton' }] },
                    { xtype: 'tbfill' },
                    { xtype: 'buttongroup', columns: 1, items: [{ xtype: 'b4closebutton' }] }
                ]
            }]
        });

        me.callParent(arguments);
    }
});
