Ext.define('B4.controller.documentregister.Lawsuit', {
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

    stores: ['claimwork.lawsuit.Petition'],

    views: ['claimwork.lawsuit.Grid'],

    refs: [
        {
            ref: 'mainView',
            selector: 'lawsuitgrid'
        }
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'exportAspect',
            gridSelector: 'lawsuitgrid',
            buttonSelector: 'lawsuitgrid button[action=export]',
            controllerName: 'Petition',
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
            'lawsuitgrid': {
                'afterrender': function (grid) {
                    grid.getStore().on('beforeload', me.onBeforeLoad, me);
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('lawsuitgrid');

        me.bindContext(view);
        me.application.deployView(view, 'documentregister');
        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        Ext.apply(operation.params, this.getFilters());
    },

    getFilters: function () {
        return this.getMainView().up('panel').down('[name=filtercontainer]').getValues();
    }
});