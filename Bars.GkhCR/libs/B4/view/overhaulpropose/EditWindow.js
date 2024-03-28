Ext.define('B4.view.overhaulpropose.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.overhaulproposeeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    //layout: { type: 'vbox', align: 'stretch' },
    layout: 'form',
    width: 1000,
    minWidth: 520,
    minHeight: 400,
    height: 450,
    bodyPadding: 5,
    itemId: 'overhaulproposeEditWindow',
    title: 'Предложение о проведении капитального ремонта',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.ProgramCr',
        'B4.view.Control.GkhButtonPrint',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.ObjectCr',
        'B4.view.overhaulpropose.WorkGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                 
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    fieldLabel: 'Программа КР',
                    itemId:'sfProgramCr',
                    flex: 1,
                    textProperty: 'Name',
                    anchor: '100%',
                    store: 'B4.store.dict.ProgramCr',
                    columns: [
                        {
                            text: 'Программа КР', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }
                        }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ObjectCr',
                    fieldLabel: 'Объект КР',
                    itemId: 'sfObjectCr',
                    flex: 1,
                    textProperty: 'Address',
                    anchor: '100%',
                    store: 'B4.store.ObjectCr',
                    columns: [
                        {
                            text: 'МО', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Адрес объекта', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' }
                        }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'overhaulproposeworkgrid',
                            flex: 1
                        }
                    ]
                }               
              
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: '',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                               
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});