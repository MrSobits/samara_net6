Ext.define('B4.view.cmnestateobj.group.formula.ParamEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.groupformulaparameditwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        
        'B4.store.cmnestateobj.GroupAttributeWithResolvers',
        'B4.model.cmnestateobj.GroupAttributeWithResolvers',
        'B4.view.cmnestateobj.group.attributes.Grid'
    ],

    layout: 'form',
    modal:true,
    width: 500,
    bodyPadding: 5,
    title: 'Добавление/редактирование параметра формулы',
    itemId: 'groupFormulaParamEditWindow',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id'
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    anchor: '100%',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Характеристика',
                    name: 'Attribute',

                    store: 'B4.store.cmnestateobj.GroupAttributeWithResolvers',
                    model: 'B4.model.cmnestateobj.GroupAttributeWithResolvers',
                    isGetOnlyIdProperty: false,
                    editable: false,
                    allowBlank: false,
                    getValue: function () {
                        return this.value ? {
                            Id: this.value.Id,
                            Name: this.value.Name,
                            ValueResolverCode: this.value.ValueResolverCode,
                            ValueResolverName: this.value.ValueResolverName
                        } : null;
                    }
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

        me.callParent(arguments);
    }
});