Ext.define('B4.controller.wastecollection.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'wastecollection.WasteCollectionPlace'
    ],

    views: [
        'wastecollection.EditPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'wastecollectionplaceeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.WasteCollectionPlaces.Edit', applyTo: 'b4savebutton', selector: 'wastecollectionplaceeditpanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'editPanelAspect',
            editPanelSelector: 'wastecollectionplaceeditpanel',
            modelName: 'wastecollection.WasteCollectionPlace',
            afterSetPanelData: function (aspect, rec, panel) {
                panel.setDisabled(false);
                var winterDays = rec.data.ExportDaysWinter,
                    summerDays = rec.data.ExportDaysSummer,
                    hasCheckInWinter = false,
                    hasCheckInSummer = false;

                if (!winterDays && !summerDays) {
                    panel.down('[name=InFact]').setValue(true);
                } else {
                    for (var key in winterDays) {
                        if (winterDays.hasOwnProperty(key)) {
                            if (winterDays[key]) {
                                hasCheckInWinter = true;
                            }
                        }
                    }
                    for (var key in summerDays) {
                        if (summerDays.hasOwnProperty(key)) {
                            if (summerDays[key]) {
                                hasCheckInSummer = true;
                            }
                        }
                    }
                    if (hasCheckInWinter || hasCheckInSummer) {
                        panel.down('[name=Scheduled]').setValue(true);
                    } else {
                        panel.down('[name=InFact]').setValue(true);
                    }
                    
                }
            },
            listeners: {
                beforesave: function(aspect, rec) {
                    var panel = aspect.getPanel(),
                        winterGroup = panel.down('checkboxgroup[name=ExportDaysWinter]'),
                        summerGroup = panel.down('checkboxgroup[name=ExportDaysSummer]');

                    rec.set('ExportDaysWinter', {
                        Monday: winterGroup.down('checkbox[name=Monday]').getValue(),
                        Tuesday: winterGroup.down('checkbox[name=Tuesday]').getValue(),
                        Wednesday: winterGroup.down('checkbox[name=Wednesday]').getValue(),
                        Thursday: winterGroup.down('checkbox[name=Thursday]').getValue(),
                        Friday: winterGroup.down('checkbox[name=Friday]').getValue(),
                        Saturday: winterGroup.down('checkbox[name=Saturday]').getValue(),
                        Sunday: winterGroup.down('checkbox[name=Sunday]').getValue(),
                        All: winterGroup.down('checkbox[name=All]').getValue()
                    });

                    rec.set('ExportDaysSummer', {
                        Monday: summerGroup.down('checkbox[name=Monday]').getValue(),
                        Tuesday: summerGroup.down('checkbox[name=Tuesday]').getValue(),
                        Wednesday: summerGroup.down('checkbox[name=Wednesday]').getValue(),
                        Thursday: summerGroup.down('checkbox[name=Thursday]').getValue(),
                        Friday: summerGroup.down('checkbox[name=Friday]').getValue(),
                        Saturday: summerGroup.down('checkbox[name=Saturday]').getValue(),
                        Sunday: summerGroup.down('checkbox[name=Sunday]').getValue(),
                        All: summerGroup.down('checkbox[name=All]').getValue()
                    });
                } 
            }
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'radio[name=InFact]': { change: { fn: me.onInFactChange, scope: me } },
            'radio[name=Scheduled]': { change: { fn: me.onScheduledChange, scope: me } },
            'checkboxgroup[name=ExportDaysWinter] checkbox[name=All]': { change: { fn: me.onCheckAllChange, scope: me } },
            'checkboxgroup[name=ExportDaysSummer] checkbox[name=All]': { change: { fn: me.onCheckAllChange, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('wastecollectionplaceeditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'wastecollectionId', id);
        me.application.deployView(view, 'waste_collection');

        me.getAspect('editPanelAspect').setData(id);
    },

    onInFactChange: function (radio, val) {
        var fieldset = radio.up('fieldset[name=ExportWaste]'),
            scheduledRadio = fieldset.down('radio[name=Scheduled]'),
            exportDaysWinter = fieldset.down('fieldset[name=Winter]'),
            exportDaysSummer = fieldset.down('fieldset[name=Summer]');

        if (val) {
            scheduledRadio.setValue(false);
            exportDaysWinter.setDisabled(true);
            exportDaysWinter.items.each(function(item) {
                item.setValue(false);
            });
            exportDaysSummer.setDisabled(true);
            exportDaysSummer.items.each(function (item) {
                item.setValue(false);
            });
        }
    },

    onScheduledChange: function (radio, val) {
        var fieldset = radio.up('fieldset[name=ExportWaste]'),
            inFactRadio = fieldset.down('radio[name=InFact]'),
            exportDaysWinter = fieldset.down('fieldset[name=Winter]'),
            exportDaysSummer = fieldset.down('fieldset[name=Summer]');

        if (val) {
            inFactRadio.setValue(false);
            exportDaysWinter.setDisabled(false);
            exportDaysSummer.setDisabled(false);
        }
    },

    onCheckAllChange: function(check, val) {
        var group = check.up('checkboxgroup');

        group.down('checkbox[name=Monday]').setValue(val);
        group.down('checkbox[name=Tuesday]').setValue(val);
        group.down('checkbox[name=Wednesday]').setValue(val);
        group.down('checkbox[name=Thursday]').setValue(val);
        group.down('checkbox[name=Friday]').setValue(val);
        group.down('checkbox[name=Saturday]').setValue(val);
        group.down('checkbox[name=Sunday]').setValue(val);
    }
});