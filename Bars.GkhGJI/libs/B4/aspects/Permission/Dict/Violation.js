Ext.define('B4.aspects.permission.dict.Violation', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.violationperm',

    permissions: [
        /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        //основной грид и панель редактирования жилого дома
        { name: 'GkhGji.Dict.Violation.Create', applyTo: 'b4addbutton', selector: 'violationGjiGrid' },
        { name: 'GkhGji.Dict.Violation.Edit', applyTo: 'b4savebutton', selector: '#violationGjiEditWindow' },
        {
            name: 'GkhGji.Dict.Violation.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'violationGjiGrid',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.PpRf170',
            applyTo: '[name="PpRf170"]',
            selector: '#violationGjiEditWindow',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.PpRf25',
            applyTo: '[name="PpRf25"]',
            selector: '#violationGjiEditWindow',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.PpRf307',
            applyTo: '[name="PpRf307"]',
            selector: '#violationGjiEditWindow',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.PpRf491',
            applyTo: '[name="PpRf491"]',
            selector: '#violationGjiEditWindow',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.OtherNormativeDocs',
            applyTo: '[name="OtherNormativeDocs"]',
            selector: '#violationGjiEditWindow',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.GkRf',
            applyTo: '[name="GkRf"]',
            selector: '#violationGjiEditWindow',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.PpRf25',
            applyTo: '[dataIndex="PpRf25"]',
            selector: 'violationGjiGrid',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.PpRf307',
            applyTo: '[dataIndex="PpRf307"]',
            selector: 'violationGjiGrid',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhGji.Dict.Violation.Field.PpRf491',
            applyTo: '[dataIndex="PpRf491"]',
            selector: 'violationGjiGrid',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        /*Панель Характеристики нарушений*/
        { name: 'GkhGji.Dict.FeatureViol.Create', applyTo: 'b4addbutton', selector: '#violationFeatureGjiGrid' },
        {
            name: 'GkhGji.Dict.FeatureViol.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#violationFeatureGjiGrid',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        /*Панель Мероприятия по устранению нарушений*/
        { name: 'GkhGji.Dict.ActionsRemovViol.Create', applyTo: 'b4addbutton', selector: '#violationActionsRemovGrid' },
        {
            name: 'GkhGji.Dict.ActionsRemovViol.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#violationActionsRemovGrid',
            applyBy: function(component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});