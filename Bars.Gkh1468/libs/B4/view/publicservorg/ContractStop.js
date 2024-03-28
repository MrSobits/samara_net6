Ext.define('B4.view.publicservorg.ContractStop', {
    extend: 'Ext.form.Panel',
    alias: 'widget.contractstoppanel',

    requires: [
        'B4.form.SelectField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    bodyPadding: 3,

    closeAction: 'hide',

    closable: false,
    title: 'Расторжение',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    trackResetOnLoad: true,
    header: true,
    border: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'StopReason',
                    fieldLabel: 'Основания расторжения',
                    textProperty: 'Name',
                    store: 'B4.store.dict.StopReason',
                    editable: false,
                    allowBlank: true,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ]
                },
                {
                    xtype: 'datefield',
                    flex: 1,
                    format: 'd.m.Y',
                    name: 'DateStop',
                    fieldLabel: 'Дата расторжения'
                }
            ]
        });

        me.callParent(arguments);
    }
});