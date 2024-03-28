Ext.define('B4.view.realityobj.HeatingStationContainer', {
    extend: 'Ext.form.FieldSet',
    alias: 'widget.realityobjheatingstationcontainer',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    title: 'Принадлежность дома к ЦТП',

    defaults: {
      flex: 1  
    },
    margin: '0 5',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150
                    },
                    flex: 4,
                    items: [
                        {
                            xtype: 'b4selectfield',
                            fieldLabel: 'Наименование ЦТП',
                            name: 'CentralHeatingStation',
                            store: 'B4.store.dict.CentralHeatingStation',
                            textProperty: 'Name',
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Адрес', dataIndex: 'AddressName', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'AddressCtp',
                            fieldLabel: 'Адрес ЦТП',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    margin: '0 0 0 30',
                    flex: 3,
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'NumberInCtp',
                            fieldLabel: 'Порядковый номер объекта в ЦТП',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            allowDecimals: false
                        },
                        {
                            xtype: 'numberfield',
                            name: 'PriorityCtp',
                            fieldLabel: 'Приоритет вывода из эксплуатации ЦТП',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            allowDecimals: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
