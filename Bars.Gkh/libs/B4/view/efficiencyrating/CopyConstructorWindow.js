Ext.define('B4.view.efficiencyrating.CopyConstructorWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.copyconstructorwindow',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.EfficiencyRatingPeriod'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    width: 640,

    autoDestroy: true,
    closeAction: 'destroy',
    closable: true,

    bodyPadding: 10,

    layout: 'form',
    title: 'Копирование ',
    defaults: {
        xtype: 'b4selectfield',
        editable: false,
        idProperty: 'Id',
        textProperty: 'Name',
        allowBlank: false,
        labelWidth: 200,
        labelAlign: 'right',
        store: 'B4.store.dict.EfficiencyRatingPeriod',
        columns: [
            {
                dataIndex: 'Name',
                flex: 1,
                text: 'Период',
                filter: {
                    xtype: 'textfield',
                    maxLength: 255
                }
            },
            {
                xtype: 'datecolumn',
                format: 'd.m.Y',
                dataIndex: 'DateStart',
                width: 100,
                text: 'Дата начала',
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            },
            {
                xtype: 'datecolumn',
                format: 'd.m.Y',
                dataIndex: 'DateEnd',
                width: 100,
                text: 'Дата окончания',
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            }
        ]
    },
    items: [
        {
            fieldLabel: 'Период из которого копируем:',
            name: 'EfficiencyRatingPeriodFrom'
        },
        {
            fieldLabel: 'Период в который копируем',
            name: 'EfficiencyRatingPeriodTo',
            readOnly: true
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
                 {
                     xtype: 'tbfill' 
                 },
                 {
                     xtype: 'buttongroup',
                     columns: 1,
                     items: [
                         {
                             xtype: 'b4closebutton' ,
                             handler: function() { this.up('window').close(); }
                         }
                     ]
                 }
             ]
         }
    ]
});