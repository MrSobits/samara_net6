Ext.define('B4.controller.objectcr.AdditionalParameters', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Ajax'
    ],

    views: ['B4.view.objectcr.AdditionalParametersPanel'],

    models: ['B4.model.objectcr.AdditionalParameters'],

    stores: ['B4.store.objectcr.AdditionalParameters'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'B4.view.objectcr.AdditionalParametersPanel',
    mainViewSelector: 'additionalparameterspanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'additionalParamsEditPanelAspect',
            editPanelSelector: 'additionalparameterspanel',
            modelName: 'objectcr.AdditionalParameters',
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
                    name: 'GkhCr.ObjectCr.AdditionalParametersViewCreate.Create',
                    applyTo: 'b4savebutton',
                    selector: 'additionalparameterspanel'
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'additionalParamsStatePermAspect',
            applyBy: function (component, allowed) {
                component.setDisabled(!allowed);
            },
            permissions: [
                {
                    name: 'GkhCr.ObjectCr.AdditionalParametersViewCreate.Create',
                    applyTo: 'b4savebutton',
                    selector: 'additionalparameterspanel'
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
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.RequestKtsDate_View',
                    applyTo: 'datefield[name=RequestKtsDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.TechConditionKtsDate_View',
                    applyTo: 'datefield[name=TechConditionKtsDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.TechConditionKtsRecipient_View',
                    applyTo: 'textfield[name=TechConditionKtsRecipient]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.RequestVodokanalDate_View',
                    applyTo: 'datefield[name=RequestVodokanalDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.TechConditionVodokanalDate_View',
                    applyTo: 'datefield[name=TechConditionVodokanalDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.TechConditionVodokanalRecipient_View',
                    applyTo: 'textfield[name=TechConditionVodokanalRecipient]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.EntryForApprovalDate_View',
                    applyTo: 'datefield[name=EntryForApprovalDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.ApprovalKtsDate_View',
                    applyTo: 'datefield[name=ApprovalKtsDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.ApprovalVodokanalDate_View',
                    applyTo: 'datefield[name=ApprovalVodokanalDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.InstallationPercentage_View',
                    applyTo: 'gkhdecimalfield[name=InstallationPercentage]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.ClientAccepted_View',
                    applyTo: 'b4enumcombo[name=ClientAccepted]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.ClientAcceptedChangeDate_View',
                    applyTo: 'datefield[name=ClientAcceptedChangeDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.InspectorAccepted_View',
                    applyTo: 'b4enumcombo[name=InspectorAccepted]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.View.InspectorAcceptedChangeDate_View',
                    applyTo: 'datefield[name=InspectorAcceptedChangeDate]',
                    selector: 'additionalparameterspanel'
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
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.RequestKtsDate_Edit',
                    applyTo: 'datefield[name=RequestKtsDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.TechConditionKtsDate_Edit',
                    applyTo: 'datefield[name=TechConditionKtsDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.TechConditionKtsRecipient_Edit',
                    applyTo: 'textfield[name=TechConditionKtsRecipient]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.RequestVodokanalDate_Edit',
                    applyTo: 'datefield[name=RequestVodokanalDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.TechConditionVodokanalDate_Edit',
                    applyTo: 'datefield[name=TechConditionVodokanalDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.TechConditionVodokanalRecipient_Edit',
                    applyTo: 'textfield[name=TechConditionVodokanalRecipient]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.EntryForApprovalDate_Edit',
                    applyTo: 'datefield[name=EntryForApprovalDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.ApprovalKtsDate_Edit',
                    applyTo: 'datefield[name=ApprovalKtsDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.ApprovalVodokanalDate_Edit',
                    applyTo: 'datefield[name=ApprovalVodokanalDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.InstallationPercentage_Edit',
                    applyTo: 'gkhdecimalfield[name=InstallationPercentage]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.ClientAccepted_Edit',
                    applyTo: 'b4enumcombo[name=ClientAccepted]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.ClientAcceptedChangeDate_Edit',
                    applyTo: 'datefield[name=ClientAcceptedChangeDate]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.InspectorAccepted_Edit',
                    applyTo: 'b4enumcombo[name=InspectorAccepted]',
                    selector: 'additionalparameterspanel'
                },
                {
                    name: 'GkhCr.ObjectCr.AdditionalParameters.Edit.InspectorAcceptedChangeDate_Edit',
                    applyTo: 'datefield[name=InspectorAcceptedChangeDate]',
                    selector: 'additionalparameterspanel'
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['additionalparameterspanel b4enumcombo[name=ClientAccepted]'] = {
             'change': {
                 fn: me.setDateFieldAllowBlank,
                  scope: me
             }
        };

        actions['additionalparameterspanel b4enumcombo[name=InspectorAccepted]'] = {
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
            view = me.getMainView() || Ext.widget('additionalparameterspanel'),
            aspect = me.getAspect('additionalParamsEditPanelAspect');

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        B4.Ajax.request(B4.Url.action('GetAdditionalParams', 'ObjectCr', {
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