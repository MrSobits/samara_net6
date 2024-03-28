Ext.define('B4.controller.claimwork.JurJournalBuildContract', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax', 'B4.Url',
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [],

    stores: ['claimwork.JurJournalBuildContract'],

    views: ['claimwork.jurjournal.BuildContractGrid'],

    refs: [
        {
            ref: 'mainView',
            selector: 'jjbuildcontractgrid'
        }
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'exportAspect',
            gridSelector: 'jjbuildcontractgrid',
            buttonSelector: 'jjbuildcontractgrid button[action=export]',
            controllerName: 'JurJournalBuildContract',
            actionName: 'Export',
            usePost: true,
            downloadViaPost: function(params) {
                var action = B4.Url.action('/' + this.controllerName + '/' + this.actionName) + '?_dc=' + (new Date().getTime()),
                    form,
                    r = /"/gm,
                    inputs = [];
                
                Ext.apply(params, this.controller.getFilters());

                Ext.iterate(params, function (key, value) {
                    if (!value) {
                        return;
                    }

                    if (Ext.isArray(value)) {
                        Ext.each(value, function (item) {
                            inputs.push({
                                tag: 'input',
                                type: 'hidden',
                                name: key,
                                value: Ext.isDate(item) ? Ext.Date.format(item, 'd.m.Y') : item.toString().replace(r, "&quot;")
                            });
                        });
                    } else {
                        inputs.push({
                            tag: 'input',
                            type: 'hidden',
                            name: key,
                            value: Ext.isDate(value) ? Ext.Date.format(value, 'd.m.Y') : value.toString().replace(r, "&quot;")
                        });
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
                            'jjbuildcontractgrid': {
                                'afterrender': function (grid) {
                                    grid.getStore().on('beforeload', me.onBeforeLoad, me);
                                  },
                                scope: me
                            }
                        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('jjbuildcontractgrid');

        me.bindContext(view);
        me.application.deployView(view, 'jurjournal');
        view.getStore().load();
    },
    
    onBeforeLoad: function (store, operation) {
        Ext.apply(operation.params, this.getFilters());
    },

    getFilters: function () {
        return this.getMainView().up('panel').down('[name=filtercontainer]').getValues();
    }
});