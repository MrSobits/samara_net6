Ext.define('B4.controller.builder.Activity', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhGridPermissionAspect'
    ],

    models: ['Builder'],
    views: ['builder.ActivityPanel'],
    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'builderActivityPanel',
            editPanelSelector: '#builderActivityPanel',
            modelName: 'Builder',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.btnUpdate, scope: this} };
            },
            btnUpdate: function (btn) {
                var form = btn.up(this.editPanelSelector);
                form.setLoading(true);
                this.setData(this.controller.params.get('Id'));
                form.setLoading(false);
            },

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
                },
                getdata: function(asp, rec) {
                    rec.modified['File'] = rec.get('File');
                    rec.modified['FileLearningPlan'] = rec.get('FileLearningPlan');
                    rec.modified['FileManningShedulle'] = rec.get('FileManningShedulle');
                }
            }
        }
    ],

    mainView: 'builder.ActivityPanel',
    mainViewSelector: '#builderActivityPanel',
    params: null,
    
    onLaunch: function () {
        if (this.params) {
            this.getAspect('builderActivityPanel').setData(this.params.get('Id'));
        }
    }
});