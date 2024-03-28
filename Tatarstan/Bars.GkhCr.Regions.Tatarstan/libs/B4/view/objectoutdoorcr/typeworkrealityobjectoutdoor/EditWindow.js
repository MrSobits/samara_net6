Ext.define('B4.view.objectoutdoorcr.typeworkrealityobjectoutdoor.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 400,
    maxWidth: 600,
    bodyPadding: 5,
    title: 'Вид работы',

    alias: 'widget.typeworkrealityobjectoutdooreditwindow',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.dict.WorkRealityObjectOutdoor',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Enum',
        'B4.enums.KindWorkOutdoor'
],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'WorkRealityObjectOutdoor',
                    fieldLabel: 'Вид работы',
                    store: 'B4.store.dict.WorkRealityObjectOutdoor',
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'TypeWork',
                            flex: 1,
                            text: 'Тип работы',
                            enumName: 'B4.enums.KindWorkOutdoor',
                            filter: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Volume',
                            fieldLabel: 'Объем'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Sum',
                            fieldLabel: 'Сумма (руб.)',
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    maxLength: 255,
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                     xtype: 'b4savebutton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});