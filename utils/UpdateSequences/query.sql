select  'select setval(' || quote_literal(quote_ident(S.relname))|| ', MAX(' ||quote_ident(C.attname)|| ') + 1) FROM ' || quote_ident(T.relname)|| ';'
from pg_class as S, pg_depend as D, pg_class as T, pg_attribute as C
where S.relkind = 'S'
    and S.oid = D.objid
    and D.refobjid = T.oid
    and D.refobjid = C.attrelid
    and D.refobjsubid = C.attnum
order by S.relname;