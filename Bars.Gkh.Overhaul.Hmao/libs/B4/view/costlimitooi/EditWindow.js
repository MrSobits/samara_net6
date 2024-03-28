Ext.define('B4.view.costlimitooi.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.CostLimitOOI',
        'B4.store.CommonEstateObject',
        'B4.store.dict.Municipality'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    //bodyPadding: 10,
    width: 500,
    itemId: 'costlimitEditWindowOOI',
    title: 'Предельная стоимость услуг или работ в разрезе ООИ',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            defaults: {},
            items:
            [{
                xtype: 'container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                defaults: {
                    labelWidth: 200,
                    labelAlign: 'right',
                },
                items: [
                    {
                        xtype: 'b4selectfield',
                        name: 'CommonEstateObject',
                        fieldLabel: 'ООИ',
                        displayField: 'Name',
                        store: 'B4.store.CommonEstateObject',
                        //selectionMode: 'MULTI',
                        idProperty: 'Id',
                        textProperty: 'Name',
                        columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                        editable: false,
                        allowBlank: false,
                        valueField: 'Value',
                        flex: 1
                    },
                    {
                        xtype: 'b4selectfield',
                        name: 'Municipality',
                        fieldLabel: 'МО',
                        displayField: 'Name',
                        store: 'B4.store.dict.Municipality',
                        //selectionMode: 'MULTI',
                        idProperty: 'Id',
                        textProperty: 'Name',
                        columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                        editable: false,
                        allowBlank: true,
                        valueField: 'Value',
                    },
                    {
                        xtype: 'numberfield',
                        name: 'Cost',
                        fieldLabel: 'Стоимость',
                        allowDecimals: true,
                        minValue: 0.0000001,
                        allowBlank: false,
                    },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    allowBlank: false,
                    name: 'DateStart',
                    fieldLabel: 'Дата начала действия условий',
                    allowBlank: true,
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    allowBlank: false,
                    name: 'DateEnd',
                    fieldLabel: 'Дата окончания действия условий',
                    allowBlank: true,
                },
                {
                    xtype: 'numberfield',
                    name: 'FloorStart',
                    fieldLabel: 'Этаж от',
                    allowDecimals: true,
                    minValue: 1,
                    allowBlank: true,
                },
                {
                    xtype: 'numberfield',
                    name: 'FloorEnd',
                    fieldLabel: 'Этаж до',
                    allowDecimals: false,
                    minValue: 1,
                    allowBlank: true,
                },
                ]
            }],
            dockedItems: [
                {
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [{
                        xtype: 'b4savebutton'
                    }]
                },
                {
                    xtype: 'tbfill'
                },
                {
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [{
                        xtype: 'b4closebutton'
                    }]
                }]
            }]
        });

        me.callParent(arguments);
    }
});