Ext.define('B4.view.gisdatabank.Window', {
    extend: 'B4.form.Window',
    alias: 'widget.gisdatabankwindow',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField'
    ],
    title: 'Банк данных',
    closable: true,
    constrain: true,
    modal: true,
    width: 540,
    height: 205,
    bodyPadding: 5,
    layout: 'fit',
    
    resizable: false,

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    frame: true,
                    anchor: '100%',
                    layout: 'anchor',
                    defaults: {
                        allowBlank: false,
                        labelWidth: 100,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            fieldLabel: 'Поставщик информации',
                            store: 'B4.store.Contragent',
                            allowBlank: false,
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListMoAreaWithoutPaging'
                                    }
                                },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Municipality',
                            fieldLabel: 'Район',
                            store: 'B4.store.dict.municipality.MoArea',
                            allowBlank: false,
                            editable: false,
                            columns: [
                                { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Key',
                            fieldLabel: 'Ключ банка данных'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4savebutton'
                        },
                        '->',
                        {
                            xtype: 'b4closebutton'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});