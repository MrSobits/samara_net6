Ext.define('B4.controller.builder.Edit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.builder.BuilderInfo'
    ],

    models: ['Builder'],
    views: ['builder.EditPanel'],

    aspects: [
        {
            xtype: 'builderinfoperm',
            editFormAspectName: 'builderEditPanel'
        },
        {
            xtype: 'gkheditpanel',
            name: 'builderEditPanel',
            editPanelSelector: '#builderEditPanel',
            modelName: 'Builder'
        }
    ],
    
    params: null,
    mainView: 'builder.EditPanel',
    mainViewSelector: '#builderEditPanel',

    onLaunch: function () {
        if (this.params) {
            this.getAspect('builderEditPanel').setData(this.params.get('Id'));
        }

        var fileLearningPlanField = Ext.getCmp('FileLearningPlan');
        var fileManningShedulleField = Ext.getCmp('FileManningShedulle');
        var docFieldSet = Ext.getCmp('DocFieldSet');

        docFieldSet.setVisible(!fileLearningPlanField.hidden || !fileManningShedulleField.hidden);
    }
});