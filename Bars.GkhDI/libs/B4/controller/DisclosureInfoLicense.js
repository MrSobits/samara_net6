Ext.define('B4.controller.DisclosureInfoLicense', {
    extend: 'B4.base.Controller',
    requires:[
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models:[
        'DisclosureInfoLicense',
        'DisclosureInfo'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores:[
        'DisclosureInfoLicense'
    ],

    views: [
        'license.EditWindow',
        'license.DisclosureInfoGrid',
        'license.DisclosureInfoLicensePanel'
    ],

    mainView: 'license.DisclosureInfoLicensePanel',
    mainViewSelector: 'disinfolicensepanel',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'licenseAspect',
            gridSelector: 'disinfolicensegrid',
            editFormSelector: 'disinfolicenseeditwindow',
            modelName: 'DisclosureInfoLicense',
            editWindowView: 'license.EditWindow',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                    return true;
                }
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'licensepermisionaspect',
            permissions: [
                { name: 'GkhDi.Disinfo.License.Create', applyTo: 'b4addbutton', selector: 'disinfolicensegrid' },
                { name: 'GkhDi.Disinfo.License.Edit', applyTo: 'b4savebutton', selector: 'disinfolicenseeditwindow' },
                {
                    name: 'GkhDi.Disinfo.License.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'disinfolicensegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    
    ],

    init: function () {
        var me = this;

        me.control({
            'disinfolicensegrid': {
                'render': {
                    fn: function(grid) {
                        var m = this;
                        grid.getStore().on('beforeload', m.onBeforeLoad, m);
                        grid.getStore().load();
                    },
                    scope: me
                }
            },
            'disinfolicensepanel [name=HasLicense]': {
                'change': {
                    fn: function(cmp, newval) {
                        var grid = cmp.up('disinfolicensepanel').down('disinfolicensegrid');
                        this.updateGrid(grid, newval);
                    },
                    scope: me
                }
            },
            'disinfolicensepanel [action=savehaslicense]': {
                'click': {
                    fn: me.onClickMeSaveLicense,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },
    
    onLaunch: function () {
        var me = this,
        model = me.getModel('DisclosureInfo');


        model.load(me.params.disclosureInfoId, {
            success: function(record) {
                var hasLicense = record.get('HasLicense');
                me.getMainView().down('[name=HasLicense]').setValue(hasLicense);
                me.getAspect('licensepermisionaspect').setPermissionsByRecord(record);
            }
        });
    },
    
    onBeforeLoad: function (store, operation) {
        var me = this;
        if (me.params) {
            operation.params.disclosureInfoId = me.params.disclosureInfoId;
        }
    },
    
    onClickMeSaveLicense: function(btn) {
        var me = this,
            panel = btn.up('disinfolicensepanel'),
            cmb = panel.down('[name=HasLicense]'),
            grid = panel.down('disinfolicensegrid'),
            model = me.getModel('DisclosureInfo'),
            rec = new model({ Id: me.params.disclosureInfoId }),
            cmbValue = cmb.getValue();

        rec.set('HasLicense', cmbValue);
        me.mask("Обработка...");
        
        rec.save({ Id: me.params.disclosureInfoId })
            .next(function () {
                me.unmask();
                me.updateGrid(grid, cmbValue);
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            })
            .error(function (result) {
                me.unmask();
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            });

    },

    updateGrid: function (grid, value) {
        if (value != 10) {
            grid.setDisabled(true);
            grid.getStore().removeAll();
        } else {
            grid.setDisabled(false);
            grid.getStore().load();
        }
    },
});