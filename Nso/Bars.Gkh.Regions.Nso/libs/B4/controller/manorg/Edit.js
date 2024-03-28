Ext.define('B4.controller.manorg.Edit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel'
    ],

    models: ['ManagingOrganization'],
    views: ['manorg.EditPanel'],
    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'manorgEditPanel',
            editPanelSelector: '#manorgEditPanel',
            modelName: 'ManagingOrganization',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbOfficialSite731'] = { 'change': { fn: this.cbOfficialSite731Change, scope: this } };
            },
            cbOfficialSite731Change: function (cb, newValue) {
                var panel = this.getPanel();
                panel.down('#tfOfficialSite').setDisabled(!newValue);
            },
            listeners: {
                //нужен для того, чтобы информация о типе управления была всегда актуальной для управления домами
                savesuccess: function (asp, record) {
                    asp.controller.params.set('TypeManagement', record.get('TypeManagement'));
                },
                beforesetpaneldata: function (asp, rec, panel) {
                    var manType = rec.get('TypeManagement');
                    if (manType === 20) {
                        var toShow = ['TsjHead', 'TsjHeadPhone', 'TsjHeadEmail', 'CaseNumber'],
                            fromHead = { 'TsjHeadPhone': 'Phone', 'TsjHeadEmail': 'Email' },
                            field = null,
                            tsjHead = rec.get('TsjHead');

                        Ext.Array.each(toShow, function (el) {
                            field = panel.down('[name="' + el + '"]');
                            if (field) {
                                field.show();

                                if (fromHead[el] && tsjHead) {
                                    field.setValue(tsjHead[fromHead[el]]);
                                }
                            }
                        });
                    }
                }
            }
        }
    ],

    params: null,
    mainView: 'manorg.EditPanel',
    mainViewSelector: '#manorgEditPanel',

    onLaunch: function () {
        if (this.params) {
            this.getAspect('manorgEditPanel').setData(this.params.get('Id'));
        }
    },

    init: function () {
        var me = this;
        me.control({
            '#manorgEditPanel [name="TsjHead"]': {
                beforeload: function (cmp, opts) {
                    if (me.params) {
                        var contr = me.params.get('Contragent');
                        if (contr) {
                            opts.params.contragentId = contr.Id;
                        }
                    }
                },
                change: me.onTsjHeadChange
            }
        });

        me.callParent(arguments);
    },

    onTsjHeadChange: function (tsjHeadField, newVal) {
        var editPanel = tsjHeadField.up('#manorgEditPanel'),
            phoneField = editPanel.down('[name=TsjHeadPhone]'),
            emailField = editPanel.down('[name=TsjHeadEmail]');

        phoneField.setValue('');
        emailField.setValue('');
        if (newVal) {
            if (newVal.Email) {
                emailField.setValue(newVal.Email);
            }

            if (newVal.Phone) {
                phoneField.setValue(newVal.Phone);
            }
        }

    }
});