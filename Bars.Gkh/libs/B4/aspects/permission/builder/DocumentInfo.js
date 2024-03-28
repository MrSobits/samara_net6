Ext.define('B4.aspects.permission.builder.DocumentInfo', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.documentinfoperm',

    requires: ['B4.Ajax', 'B4.Url'],

    init: function () {
        var me = this;

        me.permissions = [
            { name: 'Gkh.Orgs.Builder.Register.Document.Field.DocumentExist_View', applyTo: '[name=DocumentExist]', selector: '#builderDocumentEditWindow', applyBy: me.setVisible },
            { name: 'Gkh.Orgs.Builder.Register.Document.Field.DocumentExist_Edit', applyTo: '[name=DocumentExist]', selector: '#builderDocumentEditWindow' },

            { name: 'Gkh.Orgs.Builder.Register.Document.Field.Period_View', applyTo: '[name=Period]', selector: '#builderDocumentEditWindow', applyBy: me.setVisible },
            { name: 'Gkh.Orgs.Builder.Register.Document.Field.Period_Edit', applyTo: '[name=Period]', selector: '#builderDocumentEditWindow' },
            
            {
                name: 'Gkh.Orgs.Builder.Register.Document.Column.Period',
                applyTo: '[dataIndex=Period]',
                selector: '#builderDocumentGrid',
                applyBy: function (component, allowed) {
                    if (component)
                        component.setVisible(allowed);
                }
            }
        ];

        this.callParent(arguments);
    }
});