Ext.define('B4.view.actcheck.actioneditwindowbaseitem.ResultInfoFieldSet', {
    extend: 'Ext.form.FieldSet',

    alias: 'widget.actcheckactionresultinfofieldset',

    requires: [
        'B4.form.EnumCombo',
        'B4.enums.YesNoNotSet',
        'B4.enums.HasValuesNotSet',
        'B4.view.actcheck.actioneditwindowbaseitem.FileGrid',
        'B4.view.actcheck.actioneditwindowbaseitem.RemarkGrid',
        'B4.view.actcheck.actioneditwindowbaseitem.ViolationGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    padding: '0 10 10 10',

    // Элементы до нарушений
    beforeViolationItems: null,

    // Элементы после нарушений
    afterViolationItems: null,

    // Приписка к itemId
    itemIdInnerMessage: '',

    initComponent: function () {
        var me = this,
            items = [],
            violationItems = [
                {
                    xtype: 'b4enumcombo',
                    name: 'HasViolation',
                    fieldLabel: 'Нарушения выявлены',
                    enumName: 'B4.enums.YesNoNotSet',
                    labelWidth: 200,
                    labelAlign: 'right',
                    margin: '10 0 10 0',
                    listeners: {
                        change: function (field, newValue) {
                            var fieldSet = field.up('actcheckactionresultinfofieldset'),
                                carriedOutEvents = fieldSet.down('#actCheckActionCarriedOutEventSelectField'),
                                actCheckActionViolationGrid = fieldSet.down('#actCheckActionViolationGrid' + me.itemIdInnerMessage),
                                usingEquipment = fieldSet.down('textfield[name=UsingEquipment]');

                            if (newValue === B4.enums.YesNoNotSet.Yes) {
                                carriedOutEvents.show();
                                actCheckActionViolationGrid.show();
                                
                                if (usingEquipment) {
                                    usingEquipment.show();
                                }
                            }
                            else {
                                carriedOutEvents.hide();
                                carriedOutEvents.value = null;
                                carriedOutEvents.updateDisplayedText(null);

                                actCheckActionViolationGrid.hide();

                                if (usingEquipment) {
                                    usingEquipment.hide();
                                    usingEquipment.setValue(null);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'actcheckactionviolationgrid',
                    itemIdInnerMessage: me.itemIdInnerMessage,
                    height: 200
                }
            ],
            lastItems = [
                {
                    xtype: 'actcheckactionfilegrid',
                    itemIdInnerMessage: me.itemIdInnerMessage,
                    height: 200
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'HasRemark',
                    fieldLabel: 'Замечания',
                    enumName: 'B4.enums.HasValuesNotSet',
                    margin: '10 0 10 0',
                    labelWidth: 200,
                    labelAlign: 'right',
                    listeners: {
                        change: function (field, newValue) {
                            var fieldSet = field.up('actcheckactionresultinfofieldset'),
                                actCheckActionRemarkGrid = fieldSet.down('#actCheckActionRemarkGrid' + me.itemIdInnerMessage);

                            if (newValue === B4.enums.HasValuesNotSet.Yes) {
                                actCheckActionRemarkGrid.show();
                            }
                            else {
                                actCheckActionRemarkGrid.hide();
                            }
                        }
                    }
                },
                {
                    xtype: 'actcheckactionremarkgrid',
                    itemIdInnerMessage: me.itemIdInnerMessage,
                    height: 200
                }
            ];

        if (me.beforeViolationItems) {
            Array.prototype.push.apply(items, me.beforeViolationItems);
        }

        Array.prototype.push.apply(items, violationItems);

        if (me.afterViolationItems) {
            Array.prototype.push.apply(items, me.afterViolationItems);
        }

        Array.prototype.push.apply(items, lastItems);

        Ext.applyIf(me, { items: items });

        me.callParent(arguments);
    }
});