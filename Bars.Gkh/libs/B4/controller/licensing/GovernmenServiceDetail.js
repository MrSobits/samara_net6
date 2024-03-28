Ext.define('B4.controller.licensing.GovernmenServiceDetail',
{
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ButtonDataExport',

        'B4.enums.ServiceDetailSectionType',

        'B4.view.Control.GkhDecimalField'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['licensing.FormGovernmentService'],
    views: ['licensing.formgovernmentservice.EditPanel', 'licensing.formgovernmentservice.DetailForm'],

    mainView: 'licensing.formgovernmentservice.EditPanel',
    mainViewSelector: 'formgovernmentserviceeditpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'formgovernmentserviceeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Licensing.FormGovService.Edit', applyTo: 'b4savebutton', selector: 'formgovernmentserviceeditpanel' },
                { name: 'GkhGji.Licensing.FormGovService.Export', applyTo: 'button[action=export]', selector: 'formgovernmentserviceeditpanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'formGovernmentServiceGridEditPanel',
            editPanelSelector: 'formgovernmentserviceeditpanel',
            modelName: 'licensing.FormGovernmentService',
            otherActions: function (actions) {
                delete actions[this.editPanelSelector + ' b4savebutton'];
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'вфеdataExportExcelAspect',
            gridSelector: 'formgovernmentserviceeditpanel',
            buttonSelector: 'formgovernmentserviceeditpanel button[action=export]',
            controllerName: 'FormGovernmentService',
            actionName: 'Export',
            usePost: true,
            btnAction: function () {
                var params = {
                    id: this.controller.getContextValue('objectId'),
                    reportId: 'FormGovernmentServiceReport'
                };

                this.downloadViaPost(params);
            },
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'formgovernmentservicedetailform': { 'afterrender': { fn: me.setPanelData, scope: me } },
            'formgovernmentservicedetailform b4updatebutton': { 'click': { fn: function (btn) { me.setPanelData(btn.up('formgovernmentservicedetailform')) }, scope: me } },
            'formgovernmentservicedetailform b4savebutton': { 'click': { fn: function (btn) { me.saveData(btn.up('formgovernmentservicedetailform')) }, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('formgovernmentserviceeditpanel');
        
        me.bindContext(view);
        me.setContextValue(view, 'objectId', id);
        me.application.deployView(view);

        me.getAspect('formGovernmentServiceGridEditPanel').setData(id);
    },

    saveData: function (panel) {
        var me = this,
            form = panel.getForm(),
            formValues = form.getValues(false, true, false, true),
            records = [];

        for (var prop in formValues) {
            if (formValues.hasOwnProperty(prop)) {
                records.push({ Id: parseInt(prop), Value: formValues[prop] });
            }
        }

        if (!records.length) {
            return;
        }

        me.mask('Сохранение', panel);
        B4.Ajax.request({
            url: B4.Url.action('Update', 'GovernmenServiceDetail'),
            method: 'POST',
            params: {
                records: Ext.encode(records)
            }
        })
            .next(function (response) {
                me.unmask();
                me.setPanelData(panel);
            })
            .error(function (response) {
                var obj = Ext.decode(response.responseText);
                me.unmask();
                Ext.Msg.alert('Ошибка!', obj.message || 'При выполнении запроса произошла ошибка!');
            });
    },

    setPanelData: function(panel) {
        var me = this,
            id = me.getContextValue('objectId');

        panel.down('label[name=ServiceDetailSectionType]').setText(Ext.String.format('<b>{0}. {1}</b>', panel.title, B4.enums.ServiceDetailSectionType.displayRenderer(panel.type)), false);

        B4.Ajax.request({
            url: B4.Url.action('Get', 'GovernmenServiceDetail'),
            params: {
                id: id,
                type: panel.type
            }
        }).next(function(response) {
            var obj = Ext.JSON.decode(response.responseText);
            me.fillPanel(panel, obj.data.Descriptors);
        });
    },

    fillPanel: function (panel, data) {
        var me = this;

        me.mask('Загрузка...', panel);
        panel.removeAll(true);

        me.fillRecursive(panel, data);

        me.unmask();
    },

    fillRecursive: function (panel, data, parameters) {
        var me = this,
            params = {
                labelWidth: 350,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                }
            };

        Ext.apply(params, parameters);

        Ext.Array.each(data, function (el) {
            var fieldSet;

            if (el.InnerDescriptors) {
                fieldSet = Ext.widget('fieldset', {
                    title: el.DisplayValue + ':',
                    layout: params.layout
                });

                panel.add(fieldSet);
                me.fillRecursive(fieldSet, el.InnerDescriptors, { labelWidth: params.labelWidth - 10 });

            } else {
                panel.add(Ext.widget('gkhdecimalfield', {
                    value: el.Value,
                    name: el.Id,
                    fieldLabel: el.DisplayValue,
                    labelWidth: params.labelWidth
                }));
            }
        });
    }
});