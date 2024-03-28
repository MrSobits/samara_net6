Ext.define('B4.controller.regoperator.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['RegOperator'],

    views: [
        'regoperator.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'regoperator.EditPanel',
    mainViewSelector: '#regoperatorEditPanel',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.RegOperator.Edit', applyTo: 'b4savebutton', selector: '#regoperatorEditPanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'editPanelAspect',
            editPanelSelector: '#regoperatorEditPanel',
            modelName: 'RegOperator'
        }
    ],

    onLaunch: function () {
        this.getAspect('editPanelAspect').setData(this.params.get('Id'));
    }
});