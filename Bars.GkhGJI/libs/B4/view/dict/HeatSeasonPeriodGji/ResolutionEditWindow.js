Ext.define('B4.view.dict.heatseasonperiodgji.ResolutionEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    alias: 'widget.resolutioneditwin',

    modal: true,
    width: 500,

    defaults: {
        labelWidth: 180
    },

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        {
            xtype: 'hiddenfield',
            name: 'Municipality'
        },
        {
            xtype: 'hiddenfield',
            name: 'HeatSeasonPeriodGji'
        },
        {
            xtype: 'datefield',
            margin: '5 5 5 5',
            fieldLabel: 'Дата принятия постановления о пуске тепла',
            name: 'AcceptDate',
            allowBlank: false
        },
        {
            xtype: 'b4filefield',
            margin: '5 5 5 5',
            fieldLabel: 'Файл',
            name: 'Doc',
            allowBlank: false,
            possibleFileExtensions: 'pdf,jpg,doc'
        }
    ],
    dockedItems: [
        {
            xtype: 'toolbar',
            dock: 'top',
            items: [
                {
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [
                        {
                            xtype: 'b4savebutton'
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