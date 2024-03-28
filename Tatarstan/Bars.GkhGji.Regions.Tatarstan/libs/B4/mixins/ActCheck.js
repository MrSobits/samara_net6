Ext.define('B4.mixins.ActCheck',
{
    requires: [
        'B4',
        'B4.enums.PeriodKind'
    ],

    // метод возвращает срок устранения нарушений 
    getTimeline: function() {
        var timeForCorrectingViol = Gkh.config.HousingInspection.GeneralConfig.TimeForCorrectingViol;

        if (timeForCorrectingViol.IsLimitDate && timeForCorrectingViol.PeriodKind > 0 && timeForCorrectingViol.Period) {
            return {
                period: timeForCorrectingViol.Period,
                kind: timeForCorrectingViol.PeriodKind
            };
        } else {
            return {
                period: 12,
                kind: B4.enums.PeriodKind.Month
            };
        }
    }
});