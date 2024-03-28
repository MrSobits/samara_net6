Ext.define('B4.aspects.permission.dict.TypeSurvey', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.typesurveyperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        { name: 'GkhGji.Dict.TypeSurvey.Create', applyTo: 'b4addbutton', selector: '#typeSurveyGrid' },
        { name: 'GkhGji.Dict.TypeSurvey.Edit', applyTo: 'b4savebutton', selector: '#typeSurveyEditWindow' },
        { name: 'GkhGji.Dict.TypeSurvey.Delete', applyTo: 'b4deletecolumn', selector: '#typeSurveyGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});