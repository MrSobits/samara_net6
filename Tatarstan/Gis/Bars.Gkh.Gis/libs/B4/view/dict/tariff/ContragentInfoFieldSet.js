Ext.define('B4.view.dict.tariff.ContragentInfoFieldSet', {
    extend: 'Ext.form.FieldSet',
    alias: 'widget.contragentinfofieldset',

    title: 'Информация о регулируемой организации',

    requires: [
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.config.ContragentSelectField'
    ],
    layout: 'fit',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'contragentselectfield',
                    name: 'Contragent',
                    fieldLabel: 'Поставщик',
                    allowBlank: false,
                    padding: '0 0 5 0',
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'ActivityKind',
                    fieldLabel: 'Вид деятельности',
                    padding: '5 0 0 0'
                },
                {
                    xtype: 'textfield',
                    name: 'ContragentName',
                    fieldLabel: 'Наименование контрагента в базовом периоде',
                    padding: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});