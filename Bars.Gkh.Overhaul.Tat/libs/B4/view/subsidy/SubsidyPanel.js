Ext.define('B4.view.subsidy.SubsidyPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.subsidypanel',

    requires: [
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.subsidy.SubsidyRecordGrid',
        'B4.store.version.ProgramVersion'
    ],

    minWidth: 750,
    width: 750,
    autoScroll: true,
    bodyPadding: 5,
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border',
        padding: 3
    },
    defaults: {
        split: true
    },
    title: 'Субсидирование',
    trackResetOnLoad: true,
    frame: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'tbseparator'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Рассчитать собираемость',
                                    action: 'CalcOwnerCollection',
                                    iconCls: 'icon-table-go'
                                },
                                {
                                    xtype: 'tbseparator'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Рассчитать показатели',
                                    action: 'CalcValues',
                                    iconCls: 'icon-table-go'
                                },
                                {
                                    xtype: 'tbseparator'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Результат корректировки',
                                    iconCls: 'icon-table-go',
                                    action: 'CorrectResult'
                                },
                                {
                                    xtype: 'tbseparator'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'Export'
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    region:'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'subsidymunicipalityrecordgrid',
                            flex: 1,
                            border: 0
                        }
                    ]
                }
            ]
        });
        
        me.callParent(arguments);
    }
});