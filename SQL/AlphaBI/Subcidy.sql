select ro.id as "Id дома",
    st2c.plan_year "Скорректированный год",
    ceo.Name as "ООИ",
    ver.index_num as "Номер",
    ver.sum as "Сумма"
from OVHL_DPKR_CORRECT_ST2 st2c,
     gkh_reality_object ro,
     OVRHL_STAGE2_VERSION st2v,
     OVRHL_COMMON_ESTATE_OBJECT ceo,
     OVRHL_VERSION_REC ver
where ro.id = st2c.realityobject_id
and st2v.id = st2c.st2_version_id
and st2v.common_estate_id = ceo.id
and ver.id = st2v.st3_version_id