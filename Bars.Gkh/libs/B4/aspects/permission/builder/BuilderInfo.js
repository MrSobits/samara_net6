Ext.define('B4.aspects.permission.builder.BuilderInfo', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.builderinfoperm',

    requires: ['B4.Ajax', 'B4.Url'],

    init: function () {
        var me = this;

        me.permissions = [
            { name: 'Gkh.Orgs.Builder.Field.AdvancedTechnologies_View', applyTo: '[name=AdvancedTechnologies]', selector: '#builderEditPanel', applyBy: me.setVisible },
            { name: 'Gkh.Orgs.Builder.Field.AdvancedTechnologies_Edit', applyTo: '[name=AdvancedTechnologies]', selector: '#builderEditPanel' },
            
            { name: 'Gkh.Orgs.Builder.Field.ConsentInfo_View', applyTo: '[name=ConsentInfo]', selector: '#builderEditPanel', applyBy: me.setVisible },
            { name: 'Gkh.Orgs.Builder.Field.ConsentInfo_Edit', applyTo: '[name=ConsentInfo]', selector: '#builderEditPanel' },
            
            { name: 'Gkh.Orgs.Builder.Field.FileLearningPlan_View', applyTo: '[name=FileLearningPlan]', selector: '#builderEditPanel', applyBy: me.setVisible },
            { name: 'Gkh.Orgs.Builder.Field.FileLearningPlan_Edit', applyTo: '[name=FileLearningPlan]', selector: '#builderEditPanel' },
            
            { name: 'Gkh.Orgs.Builder.Field.FileManningShedulle_View', applyTo: '[name=FileManningShedulle]', selector: '#builderEditPanel', applyBy: me.setVisible },
            { name: 'Gkh.Orgs.Builder.Field.FileManningShedulle_Edit', applyTo: '[name=FileManningShedulle]', selector: '#builderEditPanel' },
            {
                name: 'Gkh.Orgs.Builder.GoToContragent.View',
                applyTo: 'buttongroup[action=GoToContragent]',
                selector: '#builderEditPanel',
                applyBy: function (component, allowed) {
                    if (component) {
                        component.setVisible(allowed);
                    }
                }
            }
        ];

        this.callParent(arguments);
    }
});