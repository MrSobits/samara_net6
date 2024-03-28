Ext.define('B4.view.desktop.BaseCrStatisticWidgetPanel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.form.ComboBox'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    bodyStyle: Gkh.bodyStyle,
    border: false,
    bodyPadding: '10 5',

    getWidget: Ext.emptyFn,
    getPanelItems: Ext.emptyFn,
    widgetTitle: '',
    widgetTitleHeight: '',

    initComponent: function () {
        var me = this,
            items = [],
            widget = me.getWidget(),
            panelItems = me.getPanelItems() || me.getDefaultPanel();

        items.push(panelItems);

        if (!Ext.isEmpty(widget)){
            items.push(widget);
        }

        Ext.applyIf(me, {
            items: items
        });

        me.callParent(arguments);
    },

    getDefaultPanel: function() {
        var me = this;

        return {
            xtype: 'panel',
            itemId: 'moChangePanel',
            border: false,
            bodyStyle: 'border-top-width: 0!important; border-bottom-width: 0!important;',
            layout: {
                type: 'vbox',
                align: 'stretch',
            },
            defaults: {
                margin: '0 0 10 0'
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch',
                        pack: 'center'
                    },
                    items: [
                        {
                            xtype: 'label',
                            text: me.widgetTitle,
                            height: me.widgetTitleHeight,
                            width: '100%',
                            style: {
                                fontSize: '16px',
                                fontWeight: 'bold',
                                textAlign: 'center',
                            }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'Municipality',
                            storeAutoLoad: false,
                            editable: false,
                            emptyItem: {Id: 0, Name: 'По всем МО'},
                            url: '/DpkrService/MunicipalityListWithoutPaging',
                            margin: '0 5 0 5',
                            onTriggerClick: function() {
                                var me = this;

                                if (!me.readOnly && !me.disabled) {
                                    if (me.isExpanded) {
                                        me.collapse();
                                    } else {
                                        me.expand();
                                    }
                                    me.inputEl.focus();
                                }
                            },
                            flex: 1
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                xtype: 'button',
                                margin: '0 5 0 5'
                            },
                            items: [
                                {
                                    itemId: 'showBtn',
                                    text: 'Показать',
                                    flex: 1
                                },
                                {
                                    iconCls: 'icon-table-go',
                                    itemId: 'exportBtn',
                                    width: 22
                                }
                            ],
                            flex: 0.5
                        }
                    ],
                    flex: 1
                }
            ]
        };
    },

    calculateSolidGuageCenter: function (element, labelWidth){
        return {
            x: (element.chart.chartWidth - labelWidth) / 2,
            y: (element.chart.plotHeight / 2) - 20
        };
    }
});