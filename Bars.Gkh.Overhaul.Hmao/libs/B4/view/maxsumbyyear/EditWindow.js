Ext.define('B4.view.maxsumbyyear.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse',
        'B4.enums.Condition',
        'B4.store.RealityObjectStateStore'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    bodyPadding: 10,
    width: 600,
    itemId: 'maxsumbyyearEditWindow',
    title: 'Предельная стоимость',
    closeAction: 'hide',
    trackResetOnLoad: true,
    initComponent: function() {
        var me = this,
            storeMu = Ext.create('B4.store.dict.municipality.ByParam', {
                autoLoad: true
            }),
            storeProgram = Ext.create('B4.store.version.ProgramVersion', {
                autoLoad: true
            });

        Ext.applyIf(me, {
            defaults: {},
            items: [{
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                    },
                    items: [{
                        xtype: 'b4selectfield',
                        name: 'Municipality',
                        fieldLabel: 'МО',
                        displayField: 'Name',
                        itemId: 'sfMunicipality',
                        store: storeMu,
                        //selectionMode: 'MULTI',
                        idProperty: 'Id',
                        textProperty: 'Name',
                        columns: [{
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
                        }],
                        editable: true,
                        allowBlank: true,
                        valueField: 'Value',
                        flex: 1
                    }]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 100,
                    labelAlign: 'right',
                },
                items: [
                    {
                        xtype: 'checkbox',
                        itemId: 'cbShowActive',
                        boxLabel: 'Только для основной программы',
                        labelAlign: 'right',
                        flex:1,
                        checked: true,
                        margin: '0px 50px 0px 105px'
                    }
                ]
            },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                    },
                    items: [{
                        xtype: 'b4selectfield',
                        name: 'Program',
                        fieldLabel: 'Программа',
                        displayField: 'Name',
                        itemId: 'sfProgram',
                        store: storeProgram,
                        //selectionMode: 'MULTI',
                        idProperty: 'Id',
                        textProperty: 'Name',
                        columns: [{
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
                        }],
                        editable: false,
                        allowBlank: true,
                        valueField: 'Value',
                        flex: 1
                    }]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                    },
                    items: [
                    {
                        xtype: 'numberfield',
                        name: 'Year',
                        fieldLabel: 'Год',
                        itemId: 'nfYear',
                        allowDecimals: false,
                        minValue: 2000,
                        maxValue: 3000,
                        flex: 1,
                        allowBlank: false,
                    },
                    {
                        xtype: 'numberfield',
                        name: 'Sum',
                        fieldLabel: 'Сумма',
                        itemId: 'nfSum',
                        allowDecimals: true,
                        minValue: 1,
                        flex: 1,
                        allowBlank: false,
                    }]
                },
            ],
            dockedItems: [
                {
                    xtype: 'buttongroup',
                    columns: 3,
                    items: [                    
                    {
                        xtype: 'b4savebutton'
                    },
                    {
                    xtype: 'tbfill'
                    },
                    {
                        xtype: 'b4closebutton'
                    }]
                }
            ]
        });

        me.callParent(arguments);
    }
});