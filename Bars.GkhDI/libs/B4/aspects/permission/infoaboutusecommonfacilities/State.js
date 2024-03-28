Ext.define('B4.aspects.permission.infoaboutusecommonfacilities.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.infoaboutusecommonfacilitiesstateperm',

    permissions: [
        // Поле в шапке
        { name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.PlaceGeneralUse', applyTo: '#cbPlaceGeneralUse', selector: '#infoAboutUseCommonFacilitiesGridPanel' },

        // Нежилые помещения
        { name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Add', applyTo: '#addInfoAboutUseCommonFacilitiesButton', selector: '#infoAboutUseCommonFacilitiesEditPanel' },
        { name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Edit', applyTo: 'b4savebutton', selector: '#infoAboutUseCommonFacilitiesEditWindow' },
        {
            name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Delete', applyTo: 'b4deletecolumn', selector: '#infoAboutUseCommonFacilitiesGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.Patronymic', applyTo: '[name="Patronymic"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.Comment', applyTo: '[name="Comment"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.SigningContractDate', applyTo: '[name="SigningContractDate"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        // Период внесения платы по договору
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.DayMonthPeriodIn', applyTo: '[name="DayMonthPeriodIn"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.DayMonthPeriodOut', applyTo: '[name="DayMonthPeriodOut"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.IsLastDayMonthPeriodIn', applyTo: '[name="IsLastDayMonthPeriodIn"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.IsLastDayMonthPeriodOut', applyTo: '[name="IsLastDayMonthPeriodOut"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.IsNextMonthPeriodIn', applyTo: '[name="IsNextMonthPeriodIn"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutUseCommonFacilities.Field.IsNextMonthPeriodOut', applyTo: '[name="IsNextMonthPeriodOut"]', selector: 'infoaboutusecommonfacilitieseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});