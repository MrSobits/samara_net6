Ext.define('B4.view.suggestion.TransitionEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.SelectField',
        'B4.store.suggestion.Rubric',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ExecutorType',
        'B4.form.ComboBox',
        'B4.form.EnumCombo'
    ],

    alias: 'widget.transitionwindow',

    width: 500,
    bodyPadding: 5,

    modal: true,

    title: 'Правило перехода',

    defaults: {
        labelWidth: 160,
        anchor: '100%',
        labelAlign: 'right'
    },

    items: [
        {
            xtype: 'hidden',
            name: 'Id'
        },
        {
            xtype: 'checkbox',
            name: 'IsFirst',
            fieldLabel: 'Начальный'
        },
        {
            xtype: 'textfield',
            name: 'Name',
            maxLength: 200,
            fieldLabel: 'Наименование',
            allowBlank: false
        },
        {
            xtype: 'b4enumcombo',
            fieldLabel: 'Смена исполнителя с',
            name: 'InitialExecutorType',
            enumName: 'B4.enums.ExecutorType',
            editable: false,
            allowBlank: false
        },
        {
            xtype: 'b4combobox',
            fieldLabel: 'Смена исполнителя на',
            name: 'TargetExecutorType',
            items: B4.enums.ExecutorType.getItemsWithEmpty([0, 'Пусто']),
            editable: false,
            operand: CondExpr.operands.eq,
            valueField: 'Value',
            displayField: 'Display'
        },
        /* b4enumcombo неподходит для пустого значения 
        {
            xtype: 'b4enumcombo',
            fieldLabel: 'Смена исполнителя на',
            name: 'TargetExecutorType',
            //enumitems: B4.enums.ExecutorType.getItemsWithEmpty([0, 'Пусто']),
            enumName: 'B4.enums.ExecutorType',
            includeEmpty: true,
            allowBlank: true
        },*/
        {
            xtype: 'textfield',
            fieldLabel: 'Email исполнителя',
            name: 'ExecutorEmail',
            allowBlank: true
        },
        {
            xtype: 'numberfield',
            fieldLabel: 'Количество дней',
            name: 'ExecutionDeadline',
            hideTrigger: true,
            allowDecimals: false,
            allowBlank: false
        },
        {
            xtype: 'textfield',
            name: 'EmailSubject',
            fieldLabel: 'Тема письма',
            maxLength: 100,
            allowBlank: false
        },
        {
            xtype: 'textareafield',
            name: 'EmailTemplate',
            fieldLabel: 'Шаблон письма',
            maxLength: 3000,
            allowBlank: false
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