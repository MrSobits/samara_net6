Ext.define('B4.controller.belayorg.Edit', {
    /* 
    * Контроллер формы редактирования страховых орг
    */
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['BelayOrganization'],
    views: ['belayorg.EditPanel'],

    mainView: 'belayorg.EditPanel',
    mainViewSelector: '#belayOrgEditPanel',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.Belay.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: '#belayOrgEditPanel',
                    applyBy: function(component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                }
            ]
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела страховых орг
            */
            xtype: 'gkheditpanel',
            name: 'belayOrgEditPanelAspect',
            editPanelSelector: '#belayOrgEditPanel',
            modelName: 'BelayOrganization',
            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    var cbActivityGroundsTermination = panel.down('#cbActivityGroundsTermination');
                    var lbActivityGroundsTerminationLabel = panel.down('#lbActivityGroundsTerminationLabel');
                    if (rec.get('Contragent').ContragentState == 20 || rec.get('Contragent').ContragentState == 30 || rec.get('Contragent').ContragentState == 40) {
                        cbActivityGroundsTermination.setDisabled(true);
                        lbActivityGroundsTerminationLabel.setText('Контрагент закончил деятельность');
                    }
                    else {
                        cbActivityGroundsTermination.setDisabled(false);
                        lbActivityGroundsTerminationLabel.setText('');
                    }
                }
            }
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('belayOrgEditPanelAspect').setData(this.params.get('Id'));
        }
    }
});