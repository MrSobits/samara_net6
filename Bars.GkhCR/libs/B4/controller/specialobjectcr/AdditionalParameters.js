Ext.define('B4.controller.specialobjectcr.AdditionalParameters', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Ajax'
    ],

    views: ['B4.view.specialobjectcr.AdditionalParametersPanel'],

    models: ['B4.model.specialobjectcr.AdditionalParameters'],

    stores: ['B4.store.specialobjectcr.AdditionalParameters'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'B4.view.specialobjectcr.AdditionalParametersPanel',
    mainViewSelector: 'specialobjectcradditionalparameterspanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'additionalParamsEditPanelAspect',
            editPanelSelector: 'specialobjectcradditionalparameterspanel',
            modelName: 'specialobjectcr.AdditionalParameters',
            getRecordBeforeSave: function (record) {
                var me = this, 
                    view = me.getPanel(),
                    id = me.controller.getContextValue(view, 'objectcrId');
                record.set('ObjectCr', { id: id });
                return record;
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            name: 'additionalParamsPermAspect',
            permissions: [
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParametersViewCreate.Create',
                    applyTo: 'b4savebutton',
                    selector: 'specialobjectcradditionalparameterspanel'
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'additionalParamsStatePermAspect',
            applyBy: function(component, allowed) {
                component.setDisabled(!allowed);
            },
            permissions: [
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParametersViewCreate.Create',
                    applyTo: 'b4savebutton',
                    selector: 'specialobjectcradditionalparameterspanel'
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'additionalParamsStatePermAspectView',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);

                var fieldset = component.up('fieldset'),
                    allFields = fieldset.query('container [name]'),
                    hiddenFields = fieldset.query('container [name]' + '[hidden=true]');

                fieldset.setVisible(hiddenFields.length < allFields.length);
            },
            permissions: [
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.RequestKtsDate_View',
                    applyTo: 'datefield[name=RequestKtsDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.TechConditionKtsDate_View',
                    applyTo: 'datefield[name=TechConditionKtsDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.TechConditionKtsRecipient_View',
                    applyTo: 'textfield[name=TechConditionKtsRecipient]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.RequestVodokanalDate_View',
                    applyTo: 'datefield[name=RequestVodokanalDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.TechConditionVodokanalDate_View',
                    applyTo: 'datefield[name=TechConditionVodokanalDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.TechConditionVodokanalRecipient_View',
                    applyTo: 'textfield[name=TechConditionVodokanalRecipient]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.EntryForApprovalDate_View',
                    applyTo: 'datefield[name=EntryForApprovalDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.ApprovalKtsDate_View',
                    applyTo: 'datefield[name=ApprovalKtsDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.ApprovalVodokanalDate_View',
                    applyTo: 'datefield[name=ApprovalVodokanalDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.InstallationPercentage_View',
                    applyTo: 'gkhdecimalfield[name=InstallationPercentage]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.ClientAccepted_View',
                    applyTo: 'b4enumcombo[name=ClientAccepted]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.ClientAcceptedChangeDate_View',
                    applyTo: 'datefield[name=ClientAcceptedChangeDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.InspectorAccepted_View',
                    applyTo: 'b4enumcombo[name=InspectorAccepted]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.View.InspectorAcceptedChangeDate_View',
                    applyTo: 'datefield[name=InspectorAcceptedChangeDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'additionalParamsStatePermAspectEdit',
            applyBy: function (component, allowed) {
                component.setReadOnly(!allowed);
            },
            permissions: [
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.RequestKtsDate_Edit',
                    applyTo: 'datefield[name=RequestKtsDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.TechConditionKtsDate_Edit',
                    applyTo: 'datefield[name=TechConditionKtsDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.TechConditionKtsRecipient_Edit',
                    applyTo: 'textfield[name=TechConditionKtsRecipient]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.RequestVodokanalDate_Edit',
                    applyTo: 'datefield[name=RequestVodokanalDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.TechConditionVodokanalDate_Edit',
                    applyTo: 'datefield[name=TechConditionVodokanalDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.TechConditionVodokanalRecipient_Edit',
                    applyTo: 'textfield[name=TechConditionVodokanalRecipient]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.EntryForApprovalDate_Edit',
                    applyTo: 'datefield[name=EntryForApprovalDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.ApprovalKtsDate_Edit',
                    applyTo: 'datefield[name=ApprovalKtsDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.ApprovalVodokanalDate_Edit',
                    applyTo: 'datefield[name=ApprovalVodokanalDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.InstallationPercentage_Edit',
                    applyTo: 'gkhdecimalfield[name=InstallationPercentage]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.ClientAccepted_Edit',
                    applyTo: 'b4enumcombo[name=ClientAccepted]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.ClientAcceptedChangeDate_Edit',
                    applyTo: 'datefield[name=ClientAcceptedChangeDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.InspectorAccepted_Edit',
                    applyTo: 'b4enumcombo[name=InspectorAccepted]',
                    selector: 'specialobjectcradditionalparameterspanel'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.AdditionalParameters.Edit.InspectorAcceptedChangeDate_Edit',
                    applyTo: 'datefield[name=InspectorAcceptedChangeDate]',
                    selector: 'specialobjectcradditionalparameterspanel'
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['specialobjectcradditionalparameterspanel b4enumcombo[name=ClientAccepted]'] = {
             'change': {
                 fn: me.setDateFieldAllowBlank,
                  scope: me
             }
        };

        actions['specialobjectcradditionalparameterspanel b4enumcombo[name=InspectorAccepted]'] = {
            'change': {
                fn: me.setDateFieldAllowBlank,
                scope: me
            }
        };

        me.control(actions);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('specialobjectcradditionalparameterspanel'),
            aspect = me.getAspect('additionalParamsEditPanelAspect');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');

        B4.Ajax.request(B4.Url.action('GetAdditionalParams', 'SpecialObjectCr', {
                objectCrId: id
            })).next(function(response) {
                var data = Ext.JSON.decode(response.responseText);
                aspect.setData(data.Id);
            }, me)
            .error(function (response) {
                Ext.Msg.alert('Ошибка', response.message);
            }, me);

        me.getAspect('additionalParamsStatePermAspectView').setPermissionsByRecord({ getId: function () { return id; } });
        me.getAspect('additionalParamsStatePermAspectEdit').setPermissionsByRecord({ getId: function () { return id; } });
        me.getAspect('additionalParamsStatePermAspect').setPermissionsByRecord({ getId: function () { return id; } });
    },

    setDateFieldAllowBlank: function (component, newValue, oldValue) {
        if (newValue) {
            if (oldValue != newValue) {
                component.up('container').down('datefield').allowBlank = false;
                component.up('form').getForm().isValid();
            }
        }
    }
});