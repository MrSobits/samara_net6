Ext.define('B4.controller.documentregister.ActViolIdentification', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [],

    stores: ['claimwork.ActViolIdentification'],

    views: ['claimwork.actviolidentification.Grid'],

    refs: [
        {
            ref: 'mainView',
            selector: 'actviolidentifgrid'
        }
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'exportAspect',
            gridSelector: 'actviolidentifgrid',
            buttonSelector: 'actviolidentifgrid button[action=export]',
            controllerName: 'ActViolIdentificationClw',
            actionName: 'Export',
            usePost: true,
            downloadViaPost: function (params) {
                var action = B4.Url.action('/' + this.controllerName + '/' + this.actionName) + '?_dc=' + (new Date().getTime()),
                    form,
                    r = /"/gm,
                    inputs = [];

                Ext.iterate(params, function (key, value) {
                    if (!value) {
                        return;
                    }

                    if (Ext.isArray(value)) {
                        Ext.each(value, function (item) {
                            inputs.push({ tag: 'input', type: 'hidden', name: key, value: item.toString().replace(r, "&quot;") });
                        });
                    } else {
                        inputs.push({ tag: 'input', type: 'hidden', name: key, value: value.toString().replace(r, "&quot;") });
                    }
                });

                form = Ext.DomHelper.append(document.body, { tag: 'form', action: action, method: 'POST', target: '_blank' });
                Ext.DomHelper.append(form, inputs);

                form.submit();
                form.remove();
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'actviolidentifgrid': {
                'afterrender': function (grid) {
                    grid.getStore().on('beforeload', me.onBeforeLoad, me);

                    //var combobox = grid.up('panel').down('b4combobox');
                    //combobox.suspendEvent('change');
                    //combobox.setValue({ DisplayName: 'Уведомление', Route: 'documentregister/notification' });
                    //combobox.resumeEvent('change');
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('actviolidentifgrid');

        me.bindContext(view);
        me.application.deployView(view, 'documentregister');
        view.getStore().load();

        //view.up('panel').down('b4combobox[name=TypeDocument]').select('notification');
    },

    onBeforeLoad: function (store, operation) {
        Ext.apply(operation.params, this.getFilters());
    },

    getFilters: function () {
        return this.getMainView().up('panel').down('[name=filtercontainer]').getValues();
    }
});