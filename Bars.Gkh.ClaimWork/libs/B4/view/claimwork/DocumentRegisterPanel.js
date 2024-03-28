Ext.define('B4.view.claimwork.DocumentRegisterPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    alias: 'widget.documentregisterpanel',
    title: 'Реестр документов ПИР',
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    padding: 0,
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.RealityObject',
        'B4.ux.button.Update',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function () {
        var me = this,
            ExtEvent = Ext.util.Event;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    data: {
                        text: 'ПИР: Реестр документов'
                    }
                },
                {
                    xtype: 'b4combobox',
                    name: 'TypeDocument',
                    fieldLabel: 'Тип документа',
                    labelWidth: 100,
                    margin: '10 0 0 0',
                    labelAlign: 'right',
                    url: '/DocumentRegister/ListTypeDocument',
                    displayField: 'DisplayName',
                    valueField: 'Route',
                    editable: false,
                    width: 600,
                    maxWidth: 600
                    //suspendEvent: function (eventName) {
                    //    var len = arguments.length,
                    //        events = this.events,
                    //        i, event, ename;

                    //    for (i = 0; i < len; i++) {
                    //        ename = arguments[i];
                    //        event = events[ename];
                    //        if (!event || typeof event == 'boolean') {
                    //            events[ename] = event = new ExtEvent(this, ename);
                    //        }

                    //        // Forcibly create the event, since we may not have anything bound when asking to suspend
                    //        event.suspend();
                    //    }
                    //},

                    //resumeEvent: function () {
                    //    var len = arguments.length,
                    //        i, event;

                    //    for (i = 0; i < len; i++) {

                    //        // If it exists, and is an Event object (not still a boolean placeholder), resume it
                    //        event = this.events[arguments[i]];
                    //        if (event && event.resume) {
                    //            event.resume();
                    //        }
                    //    }
                    //}
                },

                {
                    xtype: 'container',
                    margin: '5 0 10 0',
                    name: 'filtercontainer',
                    layout: { type: 'vbox', align: 'stretch' },
                    items: [
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
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            width: 750,
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    labelWidth: 100,
                                    width: 600,
                                    maxWidth: 600,
                                    labelAlign: 'right',
                                    name: 'Address',
                                    store: 'B4.store.RealityObject',
                                    textProperty: 'Address',
                                    columns: [
                                        {
                                            text: 'Муниципальное образование',
                                            dataIndex: 'Municipality',
                                            flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        },
                                        { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    editable: false,
                                    fieldLabel: 'Адрес'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    margin: '0 0 0 10'
                                }
                            ]
                        }
                    ],
                    getValues: function() {
                        return {                            
                            dateStart: me.down('datefield[name=PeriodStart]').getValue(),
                            dateEnd: me.down('datefield[name=PeriodEnd]').getValue(),
                            address: me.down('[name=Address]').getValue()
                        };
                    }
                },
                {
                    xtype: 'container',
                    name: 'gridcontainer',
                    layout: 'fit',
                    flex: 1,
                    style: {
                        background: '#fff',
                        borderTop: '1px solid #99bce8'
                    }
                }
            ],
            dockedItems: []
        });

        me.callParent(arguments);
    }
});