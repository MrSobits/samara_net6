Ext.define('B4.view.jurjournal.Panel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.clwjurjournalpanel',
    title: 'Журнал судебной практики',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.EnumCombo',
        'B4.form.ComboBox'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'TypeBase',
                    fieldLabel: 'Тип основания',
                    labelWidth: 100,
                    labelAlign: 'right',
                    url: '/JurJournal/ListTypeBase',
                    displayField: 'DisplayName',
                    valueField: 'Route',
                    editable: false,
                    width: 600,
                    maxWidth: 600
                },
                {
                    xtype: 'container',
                    name: 'filtercontainer',
                    layout: 
                        { type: 'vbox', align: 'stretch' },
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            name: 'TypeDocument',  
                            fieldLabel: 'Тип документа',
                            labelAlign: 'right',
                            labelWidth: 100,
                            enumName: 'B4.enums.ClaimWorkDocumentType',
                            enumItems: ['Lawsuit', 'CourtOrderClaim'],
                            includeEmpty: false,
                            width: 600,
                            maxWidth: 600
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            width: 600,
                            maxWidth: 600,
                            defaults: {
                                xtype: 'datefield',
                                format: 'd.m.Y',
                                labelWidth: 100,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'PeriodStart',
                                    fieldLabel: 'Период с'
                                },
                                {
                                    name: 'PeriodEnd',
                                    fieldLabel: 'по'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Address',
                            fieldLabel: 'Адрес',
                            labelWidth: 100,
                            labelAlign: 'right',
                            width: 600,
                            maxWidth: 600
                        }
                    ],
                    getValues: function () {
                        return {
                            dateStart: me.down('datefield[name=PeriodStart]').getValue(),
                            dateEnd: me.down('datefield[name=PeriodEnd]').getValue(),
                            address: me.down('[name=Address]').getValue(),
                            typeDocument: me.down('[name=TypeDocument]').getValue()
                        };
                    }
                },
                {
                    xtype: 'container',
                    name: 'gridcontainer',
                    layout: 'fit',
                    flex: 1,
                    border: false,
                    style: 'background: none repeat scroll 0 0 #DFE9F6'
                }
            ]
        });

        me.callParent(arguments);
    }
});