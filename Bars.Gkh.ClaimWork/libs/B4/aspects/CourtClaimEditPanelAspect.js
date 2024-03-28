Ext.define('B4.aspects.CourtClaimEditPanelAspect', {
    extend: 'B4.aspects.ClaimWorkDocument',

    alias: 'widget.courtclaimeditpanelaspect',

    requires: [
        'B4.enums.YesNo',
        'B4.enums.FillType'
    ],

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);
        actions['clwlawsuitcourtclaiminfopanel [name=Suspended]'] = { 'change': { fn: me.onChangeCheckBox, scope: me } };
        actions['clwlawsuitcourtclaiminfopanel [name=ObjectionArrived]'] = { 'change': { fn: me.onObjectionChange, scope: me } };

        controller.control(actions);
    },

    onChangeCheckBox: function (chkbox, newValue) {
        var panel = chkbox.up('clwlawsuitcourtclaiminfopanel'),
            numberField = panel.down('[name=DeterminationNumber]'),
            dateField = panel.down('[name=DeterminationDate]');

        numberField.setReadOnly(!newValue);
        dateField.setReadOnly(!newValue);
    },
    onObjectionChange: function (cmb, newValue, oldValue) {
        var panel = cmb.up('clwlawsuitcourtclaiminfopanel'),
            claimDateFld = panel.down('[name="ClaimDate"]'),
            documentFld = panel.down('[name="Document"]');

        if (newValue !== oldValue) {
            if (newValue == B4.enums.YesNo.Yes) {
                claimDateFld.setReadOnly(false);
                documentFld.setReadOnly(false);
            } else if (newValue == B4.enums.YesNo.No) {
                claimDateFld.setReadOnly(true);
                documentFld.setReadOnly(true);
            }
        }
    },

    listeners: {
        beforesetpaneldata: function (asp, record) {
            if (!record) {
                asp.getPanel().close();
                return false;
            }

            return true;
        },
        aftersetpaneldata: function (asp, record) {
            if (!record) {
                return false;
            }

            var panel = asp.getPanel(),
                tabPanel = panel.up('tabpanel'),
                resultConsideration = panel.down('[name=ResultConsideration]').getValue();
            panel.setDisabled(false);
            tabPanel.setActiveTab(panel);

            panel.down('[name=JuridicalSectorMu]').setDisabled(Gkh.config.ClaimWork.Common.General.CourtName == B4.enums.FillType.Automatic);

            if (resultConsideration == B4.enums.LawsuitResultConsideration.CourtOrderIssued) {
                panel.down('[xtype=acceptmenubutton]').setDisabled(true);
            } else {
                panel.down('[xtype=acceptmenubutton]').setDisabled(false);
            }

            return true;
        }
    }
});