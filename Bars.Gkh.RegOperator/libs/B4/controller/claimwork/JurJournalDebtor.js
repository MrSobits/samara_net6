Ext.define('B4.controller.claimwork.JurJournalDebtor', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.form.EnumCombo',
        'B4.enums.ClaimWorkDocumentType',
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [],

    stores: ['claimwork.JurJournalDebtor'],

    views: ['claimwork.jurjournal.DebtorGrid'],

    refs: [
        {
            ref: 'mainView',
            selector: 'jjdebtorgrid'
        }
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'realityObjectButtonExportAspect',
            gridSelector: 'jjdebtorgrid',
            buttonSelector: 'jjdebtorgrid button[action=export]',
            controllerName: 'JurJournalDebtor',
            actionName: 'Export',
            usePost: true,
            downloadViaPost: function (params) {
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
            'jjdebtorgrid': {
                'afterrender': {
                    fn: function (grid) {
                        grid.getStore().on('beforeload', me.onBeforeLoad, me);
                    },
                    scope: me
                }
            },

            'clwjurjournalpanel b4enumcombo[name=TypeDocument]': {
                'change': {
                    fn: me.fieldСhange,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('jjdebtorgrid');

        me.bindContext(view);
        me.application.deployView(view, 'jurjournal');
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            grid = me.getMainView(),
            panel = grid.up('panel'),
            cbTypeDocument = panel.down('b4enumcombo[name=TypeDocument]');

        Ext.apply(operation.params, me.getFilters());
    },

    getFilters: function () {
        return this.getMainView().up('panel').down('[name=filtercontainer]').getValues();
    },

    fieldСhange: function(field, newValue, oldValue) {
        var me = this,
            view = me.getMainView(),
            objColumn = view.down('gridcolumn[dataIndex=Objection]');

        view.getStore().load();

        objColumn.setVisible(newValue === B4.enums.ClaimWorkDocumentType.CourtOrderClaim)
    },
});