Ext.define('B4.view.analysisreport.AnalysisReportForm', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 800,
    height: 300,
    bodyPadding: 5,
    alias: 'widget.analysisreportform',
    title: 'Протокол расчета начислений по услуге',
    itemId: 'analysisreportformid',
    trackResetOnLoad: true,
    modal: true,

    requires: [
        'B4.ux.button.Close'
    ],

    initComponent: function () {
        var me = this;

        var store = Ext.create('B4.store.AnalysisReport');

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'fieldcontainer',
                            margin: 5,
                            flex: 1,
                            fieldLabel: 'Лицевой счет',
                            labelWidth: 110,
                            items: [
                                {
                                    xtype: 'label',
                                    name: 'LS',
                                    text: '-'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldcontainer',
                            margin: '5 5 5 8',
                            flex: 2,
                            fieldLabel: 'Адрес',
                            labelWidth: 70,
                            items: [
                                {
                                    xtype: 'label',
                                    name: 'Address',
                                    text: '-'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'fieldcontainer',
                            margin: '0 5 5 5',
                            flex: 1,
                            fieldLabel: 'Расчетный месяц',
                            labelWidth: 110,
                            items: [
                                {
                                    xtype: 'label',
                                    name: 'CalcMonth',
                                    text: '-'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldcontainer',
                            margin: '0 5 5 8',
                            flex: 2,
                            fieldLabel: 'Поставщик',
                            labelWidth: 70,
                            items: [
                                {
                                    xtype: 'label',
                                    name: 'Supplier',
                                    text: '-'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'treepanel',
                    anchor: '0 -55',
                    name: 'reportTree',
                    store: store,
                    rootVisible: false,
                    columns: [{
                        xtype: 'treecolumn',
                        text: 'Начисления',
                        flex: 2,
                        sortable: true,
                        dataIndex: 'Title'
                    }]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'b4closebutton',
                            handler: function () {
                                me.close();
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});