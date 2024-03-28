namespace Bars.GisIntegration.Base.Migrations.Version_2016110900
{
    using System.Collections.Generic;

    public partial class UpdateSchema
    {
        //Перечень таблиц
        private readonly List<TableProxy> tables = new List<TableProxy>
        {
            #region GI_CR_CONTRACT

            new TableProxy("gi_cr_contract", @"
                CREATE TABLE gi_cr_contract
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    operation smallint NOT NULL DEFAULT 0,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    gi_contragent_id bigint,
                    guid character varying(50),
                    plan_guid character varying(40) NOT NULL,
                    ""number"" character varying(40),
                    date timestamp without time zone,
                    start_date timestamp without time zone,
                    end_date timestamp without time zone,
                    sum numeric,
                    customer character varying(100) NOT NULL,
                    performer character varying(100) NOT NULL,
                    outlay_missing boolean,
                    CONSTRAINT ris_cr_contract_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_ris_cr_contract_contragent FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_cr_contract_contragent", @"
                    CREATE INDEX ind_ris_cr_contract_contragent
                    ON gi_cr_contract
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region GI_CR_PLAN

            new TableProxy("gi_cr_plan", @"
               CREATE TABLE gi_cr_plan
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  operation smallint NOT NULL DEFAULT 0,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  gi_contragent_id bigint,
                  guid character varying(50),
                  name character varying(500),
                  municipality_code character varying(11),
                  municipality_name character varying(500),
                  start_month_year date,
                  end_month_year date,
                  CONSTRAINT ris_cr_plan_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_cr_plan_contragent FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_cr_plan_contragent", @"
                   CREATE INDEX ind_ris_cr_plan_contragent
                    ON gi_cr_plan
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region GI_CR_PLANWORK

            new TableProxy("gi_cr_planwork", @"
                CREATE TABLE gi_cr_planwork
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  operation smallint NOT NULL DEFAULT 0,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  gi_contragent_id bigint,
                  guid character varying(50),
                  plan_guid character varying(50),
                  building_fias_guid character varying(50),
                  work_kind_code character varying(10),
                  work_kind_guid character varying(50),
                  end_month_year character varying(50),
                  municipality_code character varying(11),
                  municipality_name character varying(500),
                  CONSTRAINT ris_cr_planwork_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_cr_planwork_contragent FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_cr_planwork_contragent", @"
                    CREATE INDEX ind_ris_cr_planwork_contragent
                    ON gi_cr_planwork
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region GI_CR_ATTACHMENT_CONTRACT

            new TableProxy("gi_cr_attachment_contract", @"
                CREATE TABLE gi_cr_attachment_contract
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    operation smallint NOT NULL DEFAULT 0,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    gi_contragent_id bigint,
                    guid character varying(50),
                    contract_id bigint NOT NULL,
                    attachment_id bigint NOT NULL,
                    CONSTRAINT ris_cr_attachment_contract_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_fk_ris_cr_attachment_contract_a FOREIGN KEY (attachment_id)
                        REFERENCES gi_attachment (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_fk_ris_cr_attachment_contract_c FOREIGN KEY (contract_id)
                        REFERENCES gi_cr_contract (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_ris_cr_attachment_contract_contragent FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_fk_ris_cr_attachment_contract_a", @"
                    CREATE INDEX ind_fk_ris_cr_attachment_contract_a
                    ON gi_cr_attachment_contract
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_fk_ris_cr_attachment_contract_c", @"
                    CREATE INDEX ind_fk_ris_cr_attachment_contract_c
                    ON gi_cr_attachment_contract
                    USING btree
                    (contract_id);
                "),
                new IndexProxy("ind_ris_cr_attachment_contract_contragent", @"
                    CREATE INDEX ind_ris_cr_attachment_contract_contragent
                    ON gi_cr_attachment_contract
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region GI_CR_ATTACHMENT_OUTLAY

            new TableProxy("gi_cr_attachment_outlay", @"
                CREATE TABLE gi_cr_attachment_outlay
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    operation smallint NOT NULL DEFAULT 0,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    gi_contragent_id bigint,
                    guid character varying(50),
                    contract_id bigint NOT NULL,
                    attachment_id bigint NOT NULL,
                    CONSTRAINT ris_cr_attachment_outlay_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_fk_ris_cr_attachment_outlay_a FOREIGN KEY (attachment_id)
                        REFERENCES gi_attachment (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_fk_ris_cr_attachment_outlay_c FOREIGN KEY (contract_id)
                        REFERENCES gi_cr_contract (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_ris_cr_attachment_outlay_contragent FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_fk_ris_cr_attachment_outlay_a", @"
                    CREATE INDEX ind_fk_ris_cr_attachment_outlay_a
                    ON gi_cr_attachment_outlay
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_fk_ris_cr_attachment_outlay_c", @"
                    CREATE INDEX ind_fk_ris_cr_attachment_outlay_c
                    ON gi_cr_attachment_outlay
                    USING btree
                    (contract_id);
                "),
                new IndexProxy("ind_ris_cr_attachment_outlay_contragent", @"
                    CREATE INDEX ind_ris_cr_attachment_outlay_contragent
                    ON gi_cr_attachment_outlay
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region GI_CR_WORK

            new TableProxy("gi_cr_work", @"
                CREATE TABLE gi_cr_work
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    operation smallint NOT NULL DEFAULT 0,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    gi_contragent_id bigint,
                    guid character varying(50),
                    planwork_guid character varying(50),
                    building_fias_guid character varying(50),
                    work_kind_code character varying(10),
                    work_kind_guid character varying(50),
                    end_month_year character varying(50),
                    contract_id bigint NOT NULL,
                    start_date date,
                    end_date date,
                    cost numeric,
                    cost_plan numeric,
                    volume numeric,
                    other_unit character varying(50),
                    additional_info character varying(250),
                    okei character varying(50),
                    CONSTRAINT ris_cr_work_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_fk_ris_cr_work_contract FOREIGN KEY (contract_id)
                        REFERENCES gi_cr_contract (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_ris_cr_work_contragent FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_fk_ris_cr_work_contract", @"
                    CREATE INDEX ind_fk_ris_cr_work_contract
                    ON gi_cr_work
                    USING btree
                    (contract_id);
                "),
                new IndexProxy("ind_ris_cr_work_contragent", @"
                    CREATE INDEX ind_ris_cr_work_contragent
                    ON gi_cr_work
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region GI_INSPECTION_PLAN

            new TableProxy("gi_inspection_plan", @"
                CREATE TABLE gi_inspection_plan
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    guid character varying(50),
                    year smallint,
                    approval_date timestamp without time zone,
                    operation smallint NOT NULL DEFAULT 0,
                    gi_contragent_id bigint,
                    CONSTRAINT ris_inspection_plan_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_gi_inspect_plan_ctrg FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            "),

            #endregion

            #region GI_INSPECTION_EXAMINATION

            new TableProxy("gi_inspection_examination", @"
                CREATE TABLE gi_inspection_examination
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    guid character varying(50),
                    base_guid character varying(255),
                    examform_code character varying(255),
                    examform_guid character varying(255),
                    is_scheduled boolean,
                    lastname character varying(255),
                    firstname character varying(255),
                    middlename character varying(255),
                    oversight_act_code character varying(255),
                    oversight_act_guid character varying(255),
                    objective character varying(2000),
                    date_from timestamp without time zone,
                    date_to timestamp without time zone,
                    duration double precision,
                    tasks character varying(2000),
                    order_number character varying(255),
                    order_date timestamp without time zone,
                    inspectionnumber integer,
                    plan_id bigint,
                    base_code character varying(255),
                    operation smallint NOT NULL DEFAULT 0,
                    gi_contragent_id bigint,
                    subject_type integer,
                    org_root_entity_guid character varying(255),
                    activity_place character varying(255),
                    event_desc character varying(2000),
                    has_result boolean,
                    result_doc_type_code character varying(255),
                    result_doc_type_guid character varying(255),
                    result_doc_number character varying(255),
                    result_doc_datetime timestamp without time zone,
                    has_offence boolean,
                    CONSTRAINT ris_inspection_examination_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_gi_inspe_examin_ctrg FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            "),

            #endregion

            #region GI_INSPECTION_EXAM_ATTACH

            new TableProxy("gi_inspection_exam_attach", @"
                CREATE TABLE gi_inspection_exam_attach
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  examination_id bigint NOT NULL,
                  attachment_id bigint NOT NULL,
                  CONSTRAINT ris_inspection_exam_attach_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_insp_exam_attach_attach FOREIGN KEY (attachment_id)
                      REFERENCES gi_attachment (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_insp_exam_attach_exam FOREIGN KEY (examination_id)
                      REFERENCES gi_inspection_examination (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_insp_exam_attach_attach", @"
                    CREATE INDEX ind_ris_insp_exam_attach_attach
                    ON gi_inspection_exam_attach
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_ris_insp_exam_attach_exam", @"
                    CREATE INDEX ind_ris_insp_exam_attach_exam
                    ON gi_inspection_exam_attach
                    USING btree
                    (examination_id);
                ")
            }),

            #endregion

            #region GI_INSPECTION_EXAM_PLACE

            new TableProxy("gi_inspection_exam_place", @"
                CREATE TABLE gi_inspection_exam_place
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  examination_id bigint NOT NULL,
                  order_num integer,
                  fias_house_guid character varying(50),
                  CONSTRAINT ris_inspection_exam_place_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_insp_exam_place_exam FOREIGN KEY (examination_id)
                      REFERENCES gi_inspection_examination (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_insp_exam_place_exam", @"
                    CREATE INDEX ind_ris_insp_exam_place_exam
                    ON gi_inspection_exam_place
                    USING btree
                    (examination_id);
                ")
            }),

            #endregion

            #region GI_INSPECTION_OFFENCE

            new TableProxy("gi_inspection_offence", @"
                CREATE TABLE gi_inspection_offence
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  examination_id bigint,
                  ""number"" character varying(300),
                  date timestamp without time zone,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  is_cancelled boolean,
                  cancel_reason character varying(2000),
                  cancel_date timestamp without time zone,
                  cancel_decision_num character varying(300),
                  CONSTRAINT ris_inspection_offence_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_gi_inspe_offenc_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_insp_offence_exam FOREIGN KEY (examination_id)
                      REFERENCES gi_inspection_examination (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_insp_offence_exam", @"
                    CREATE INDEX ind_ris_insp_offence_exam
                    ON gi_inspection_offence
                    USING btree
                    (examination_id);
                ")
            }),

            #endregion

            #region GI_INSPECTION_OFFENCE_ATTACH

            new TableProxy("gi_inspection_offence_attach", @"
                CREATE TABLE gi_inspection_offence_attach
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  offence_id bigint NOT NULL,
                  attachment_id bigint NOT NULL,
                  CONSTRAINT ris_inspection_offence_attach_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_insp_offence_attach_attach FOREIGN KEY (attachment_id)
                      REFERENCES gi_attachment (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_insp_offence_attach_offence FOREIGN KEY (offence_id)
                      REFERENCES gi_inspection_offence (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_insp_offence_attach_attach", @"
                    CREATE INDEX ind_ris_insp_offence_attach_attach
                    ON gi_inspection_offence_attach
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_ris_insp_offence_attach_offence", @"
                    CREATE INDEX ind_ris_insp_offence_attach_offence
                    ON gi_inspection_offence_attach
                    USING btree
                    (offence_id);
                ")
            }),

            #endregion

            #region GI_INSPECTION_PRECEPT

            new TableProxy("gi_inspection_precept", @"
                CREATE TABLE gi_inspection_precept
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  examination_id bigint,
                  ""number"" character varying(300),
                  date timestamp without time zone,
                  cancel_reason character varying(200),
                  cancel_date timestamp without time zone,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  fias_house_guid character varying(50),
                  is_cancelled boolean,
                  CONSTRAINT ris_inspection_precept_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_gi_inspe_precep_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_insp_prec_exam FOREIGN KEY (examination_id)
                      REFERENCES gi_inspection_examination (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_insp_prec_exam", @"
                    CREATE INDEX ind_ris_insp_prec_exam
                    ON gi_inspection_precept
                    USING btree
                    (examination_id);
                ")
            }),

            #endregion

            #region GI_INSPECTION_PRECEPT_ATTACH

            new TableProxy("gi_inspection_precept_attach", @"
                CREATE TABLE gi_inspection_precept_attach
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  precept_id bigint NOT NULL,
                  attachment_id bigint NOT NULL,
                  is_cancel_attach boolean,
                  CONSTRAINT ris_inspection_precept_attach_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_insp_precept_attach_attach FOREIGN KEY (attachment_id)
                      REFERENCES gi_attachment (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_insp_precept_attach_precept FOREIGN KEY (precept_id)
                      REFERENCES gi_inspection_precept (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_insp_precept_attach_attach", @"
                    CREATE INDEX ind_ris_insp_precept_attach_attach
                    ON gi_inspection_precept_attach
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_ris_insp_precept_attach_precept", @"
                    CREATE INDEX ind_ris_insp_precept_attach_precept
                    ON gi_inspection_precept_attach
                    USING btree
                    (precept_id);
                ")
            }),

            #endregion

            #region RIS_PAYMENT_PERIOD

            new TableProxy("ris_payment_period", @"
                CREATE TABLE ris_payment_period
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  month integer,
                  year integer,
                  payment_period_type smallint NOT NULL DEFAULT 10,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  is_applied boolean NOT NULL DEFAULT false,
                  CONSTRAINT ris_payment_period_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_payme_perio_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            "),

            #endregion

            #region RIS_HOUSE

            new TableProxy("ris_house", @"
                CREATE TABLE ris_house
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  house_type smallint NOT NULL DEFAULT 10,
                  fiashouseguid character varying(50),
                  cadastralnumber character varying(300),
                  totalsquare numeric(19,5),
                  state_code character varying(50),
                  state_guid character varying(50),
                  projectseries character varying(300),
                  projecttype_code character varying(50),
                  projecttype_guid character varying(50),
                  buildingyear smallint,
                  totalwear numeric(19,5),
                  energydate timestamp without time zone,
                  energycategory_code character varying(50),
                  energycategory_guid character varying(50),
                  oktmo_code character varying(50),
                  oktmo_name character varying(50),
                  prevstateregnumber_cadastralnumber character varying(50),
                  prevstateregnumber_inventorynumber character varying(50),
                  prevstateregnumber_conditionalnumber character varying(50),
                  olsontz_code character varying(50),
                  olsontz_guid character varying(50),
                  builtuparea numeric(19,5),
                  minfloorcount integer,
                  overhaulyear smallint,
                  overhaulforming_code character varying(50),
                  overhaulforming_guid character varying(50),
                  housemanagementtype_code character varying(50),
                  housemanagementtype_guid character varying(50),
                  residentialhousetype_code character varying(50),
                  residentialhousetype_guid character varying(50),
                  usedyear smallint,
                  floorcount character varying(50),
                  residentialsquare numeric(18,5),
                  culturalheritage boolean,
                  undergroundfloorcount character varying(50),
                  nonresidentialsquare numeric(18,5),
                  adress character varying(500),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_house_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_house_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            "),

            #endregion

            #region RIS_IND

            new TableProxy("ris_ind", @"
                CREATE TABLE ris_ind
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  surname character varying(50),
                  firstname character varying(50),
                  patronymic character varying(50),
                  sex smallint NOT NULL DEFAULT 0,
                  dateofbirth timestamp without time zone,
                  idtype_guid character varying(50),
                  idtype_code character varying(50),
                  idseries character varying(50),
                  idnumber character varying(50),
                  idissuedate timestamp without time zone,
                  snils character varying(50),
                  placebirth character varying(50),
                  isregistered boolean,
                  isresides boolean,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_ind_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_ind_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            "),

            #endregion

            #region RIS_PAYMENT_INFO

            new TableProxy("ris_payment_info", @"
                CREATE TABLE ris_payment_info
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  recipient character varying(160) NOT NULL,
                  bank_bik character varying(9) NOT NULL,
                  recipient_kpp character varying(9),
                  operating_account_number character varying(20) NOT NULL,
                  correspondent_bank_account character varying(20) NOT NULL,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  recipient_inn character varying(12),
                  bank_name character varying(160),
                  CONSTRAINT ris_payment_info_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_paymen_info_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            "),

            #endregion

            #region RIS_ADDAGREEMENTPAYMENT

            new TableProxy("ris_addagreementpayment", @"
                CREATE TABLE ris_addagreementpayment
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  agreementpaymentversion character varying(50) NOT NULL,
                  datefrom timestamp without time zone NOT NULL,
                  dateto timestamp without time zone NOT NULL,
                  bill numeric(18,5),
                  debt numeric(18,5),
                  paid numeric(18,5),
                  CONSTRAINT ris_addagreementpayment_pkey PRIMARY KEY (id)
                )
            "),

            #endregion

            #region RIS_ADDRESS_INFO

            new TableProxy("ris_address_info", @"
                CREATE TABLE ris_address_info
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  living_person_number smallint,
                  residential_square numeric(18,5),
                  heated_area numeric(18,5),
                  total_square numeric(18,5),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_address_info_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_addres_info_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            "),

            #endregion

            #region RIS_ACCOUNT

            new TableProxy("ris_account", @"
                CREATE TABLE ris_account
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  typeaccount smallint NOT NULL DEFAULT 10,
                  ownerind_id bigint,
                  ownerorg_id bigint,
                  renterind_id bigint,
                  renterorg_id bigint,
                  livingpersonsnumber integer,
                  totalsquare numeric(19,5),
                  residentialsquare numeric(19,5),
                  heatedarea numeric(19,5),
                  closed boolean,
                  closereason_code character varying(50),
                  closereason_guid character varying(50),
                  closedate timestamp without time zone,
                  accountnumber character varying(50),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  begindate timestamp without time zone,
                  is_renter boolean,
                  CONSTRAINT ris_account_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_acc_ownerind FOREIGN KEY (ownerind_id)
                      REFERENCES ris_ind (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_acc_ownerorg FOREIGN KEY (ownerorg_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_acc_renterind FOREIGN KEY (renterind_id)
                      REFERENCES ris_ind (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_acc_renterorg FOREIGN KEY (renterorg_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_acc_ownerind", @"
                    CREATE INDEX ind_ris_acc_ownerind
                    ON ris_account
                    USING btree
                    (ownerind_id);
                "),
                new IndexProxy("ind_ris_acc_ownerorg", @"
                    CREATE INDEX ind_ris_acc_ownerorg
                    ON ris_account
                    USING btree
                    (ownerorg_id);
                "),
                new IndexProxy("ind_ris_acc_renterind", @"
                    CREATE INDEX ind_ris_acc_renterind
                    ON ris_account
                    USING btree
                    (renterind_id);
                "),
                new IndexProxy("ind_ris_acc_renterorg", @"
                    CREATE INDEX ind_ris_acc_renterorg
                    ON ris_account
                    USING btree
                    (renterorg_id);
                ")
            }),

            #endregion

            #region RIS_PAYMENT_DOC

            new TableProxy("ris_payment_doc", @"
                CREATE TABLE ris_payment_doc
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  account_id bigint,
                  payment_info_id bigint,
                  address_info_id bigint,
                  state smallint,
                  total_piecemeal_sum numeric(18,5),
                  date timestamp without time zone,
                  periodmonth smallint,
                  periodyear smallint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  advance_blling_period numeric,
                  debt_previous_periods numeric,
                  CONSTRAINT ris_payment_doc_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_payment_doc_account FOREIGN KEY (account_id)
                      REFERENCES ris_account (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_payment_doc_address_info FOREIGN KEY (address_info_id)
                      REFERENCES ris_address_info (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_payment_doc_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_payment_doc_payment_info FOREIGN KEY (payment_info_id)
                      REFERENCES ris_payment_info (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_payment_doc_account", @"
                    CREATE INDEX ind_ris_payment_doc_account
                    ON ris_payment_doc
                    USING btree
                    (account_id);
                "),
                new IndexProxy("ind_ris_payment_doc_address_info", @"
                    CREATE INDEX ind_ris_payment_doc_address_info
                    ON ris_payment_doc
                    USING btree
                    (address_info_id);
                "),
                new IndexProxy("ind_ris_payment_doc_payment_info", @"
                    CREATE INDEX ind_ris_payment_doc_payment_info
                    ON ris_payment_doc
                    USING btree
                    (payment_info_id);
                ")
            }),

            #endregion

            #region RIS_ADDITIONAL_SERVICE_CHARGE_INFO

            new TableProxy("ris_additional_service_charge_info", @"
                CREATE TABLE ris_additional_service_charge_info
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  service_type_code character varying(20) NOT NULL,
                  service_type_guid character varying(50) NOT NULL,
                  okei character varying(3),
                  rate numeric(18,5),
                  individual_consumption_current_value numeric(18,5),
                  house_overall_needs_current_value numeric(18,5),
                  house_total_individual_consumption numeric(18,5),
                  house_total_overall_needs numeric(18,5),
                  house_overall_needs_norm numeric(18,5),
                  individual_consumption_norm numeric(18,5),
                  individual_consumption numeric(18,5),
                  house_overall_needs_consumption numeric(18,5),
                  payment_doc_id bigint,
                  money_recalculation numeric(18,5),
                  money_discount numeric(18,5),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  total_payable numeric NOT NULL,
                  accounting_period_total numeric NOT NULL,
                  calc_explanation character varying(255),
                  volume_type character varying(1),
                  CONSTRAINT ris_additional_service_charge_info_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_additional_service_charge_payment_doc FOREIGN KEY (payment_doc_id)
                      REFERENCES ris_payment_doc (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_risaddserchainf_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_additional_service_charge_payment_doc", @"
                    CREATE INDEX ind_ris_additional_service_charge_payment_doc
                    ON ris_additional_service_charge_info
                    USING btree
                    (payment_doc_id);
                ")
            }),

            #endregion

            #region RIS_ADDITIONAL_SERVICE_EXT_CHARGE_INFO

            new TableProxy("ris_additional_service_ext_charge_info", @"
                CREATE TABLE ris_additional_service_ext_charge_info
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  payment_doc_id bigint,
                  money_recalculation numeric(18,5),
                  money_discount numeric(18,5),
                  service_type_code character varying(20) NOT NULL,
                  service_type_guid character varying(50) NOT NULL,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_additional_service_ext_charge_info_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_riadseextchainf_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_additional_service_ext_charge_payment_doc FOREIGN KEY (payment_doc_id)
                      REFERENCES ris_payment_doc (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_additional_service_ext_charge_payment_doc", @"
                    CREATE INDEX ind_ris_additional_service_ext_charge_payment_doc
                    ON ris_additional_service_ext_charge_info
                    USING btree
                    (payment_doc_id);
                ")
            }),

            #endregion

            #region RIS_HOUSING_SERVICE_CHARGE_INFO

            new TableProxy("ris_housing_service_charge_info", @"
                CREATE TABLE ris_housing_service_charge_info
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  service_type_code character varying(20) NOT NULL,
                  service_type_guid character varying(50) NOT NULL,
                  okei character varying(3),
                  rate numeric(18,5),
                  individual_consumption_current_value numeric(18,5),
                  house_overall_needs_current_value numeric(18,5),
                  house_total_individual_consumption numeric(18,5),
                  house_total_overall_needs numeric(18,5),
                  house_overall_needs_norm numeric(18,5),
                  individual_consumption_norm numeric(18,5),
                  individual_consumption numeric(18,5),
                  house_overall_needs_consumption numeric(18,5),
                  payment_doc_id bigint,
                  money_recalculation numeric(18,5),
                  money_discount numeric(18,5),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  total_payable numeric NOT NULL,
                  accounting_period_total numeric NOT NULL,
                  calc_explanation character varying(255),
                  volume_type character varying(1),
                  CONSTRAINT ris_housing_service_charge_info_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_housing_service_charge_payment_doc FOREIGN KEY (payment_doc_id)
                      REFERENCES ris_payment_doc (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_rishouserchainf_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_housing_service_charge_payment_doc", @"
                    CREATE INDEX ind_ris_housing_service_charge_payment_doc
                    ON ris_housing_service_charge_info
                    USING btree
                    (payment_doc_id);
                ")
            }),

            #endregion

            #region RIS_MUNICIPAL_SERVICE_CHARGE_INFO

            new TableProxy("ris_municipal_service_charge_info", @"
                CREATE TABLE ris_municipal_service_charge_info
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  service_type_code character varying(20) NOT NULL,
                  service_type_guid character varying(50) NOT NULL,
                  okei character varying(3),
                  rate numeric(18,5),
                  individual_consumption_current_value numeric(18,5),
                  house_overall_needs_current_value numeric(18,5),
                  house_total_individual_consumption numeric(18,5),
                  house_total_overall_needs numeric(18,5),
                  house_overall_needs_norm numeric(18,5),
                  individual_consumption_norm numeric(18,5),
                  individual_consumption numeric(18,5),
                  house_overall_needs_consumption numeric(18,5),
                  payment_doc_id bigint,
                  money_recalculation numeric(18,5),
                  money_discount numeric(18,5),
                  period_piecemal_sum numeric(18,5),
                  past_period_piecemal_sum numeric(18,5),
                  piecemal_percent_rub numeric(18,5),
                  piecemal_percent numeric(18,5),
                  piecemal_payment_sum numeric(18,5),
                  payment_recalculation_sum numeric(18,5),
                  payment_recalculation_reason character varying(1000),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  total_payable numeric NOT NULL,
                  accounting_period_total numeric NOT NULL,
                  calc_explanation character varying(255),
                  individual_consumption_payable numeric NOT NULL,
                  communal_consumption_payable numeric NOT NULL,
                  volume_type character varying(1),
                  CONSTRAINT ris_municipal_service_charge_info_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_municipal_service_charge_payment_doc FOREIGN KEY (payment_doc_id)
                      REFERENCES ris_payment_doc (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_rismunserchainf_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_municipal_service_charge_payment_doc", @"
                    CREATE INDEX ind_ris_municipal_service_charge_payment_doc
                    ON ris_municipal_service_charge_info
                    USING btree
                    (payment_doc_id);
                ")
            }),

            #endregion

            #region RIS_TECH_SERVICE

            new TableProxy("ris_tech_service", @"
                CREATE TABLE ris_tech_service
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  service_type_code character varying(20) NOT NULL,
                  service_type_guid character varying(50) NOT NULL,
                  okei character varying(3),
                  rate numeric(18,5),
                  individual_consumption_current_value numeric(18,5),
                  house_overall_needs_current_value numeric(18,5),
                  house_total_individual_consumption numeric(18,5),
                  house_total_overall_needs numeric(18,5),
                  house_overall_needs_norm numeric(18,5),
                  individual_consumption_norm numeric(18,5),
                  individual_consumption numeric(18,5),
                  house_overall_needs_consumption numeric(18,5),
                  additional_service_ext_charge_info_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_tech_service_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_tech_servic_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_tech_service_additional_ext_charge FOREIGN KEY (additional_service_ext_charge_info_id)
                      REFERENCES ris_additional_service_ext_charge_info (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_tech_service_additional_ext_charge", @"
                    CREATE INDEX ind_ris_tech_service_additional_ext_charge
                    ON ris_tech_service
                    USING btree
                    (additional_service_ext_charge_info_id);
                ")
            }),

            #endregion

            #region RIS_NONRESIDENTIALPREMISES

            new TableProxy("ris_nonresidentialpremises", @"
                CREATE TABLE ris_nonresidentialpremises
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  house_id bigint,
                  premisesnum character varying(50),
                  cadastralnumber character varying(50),
                  prevstateregnumber_cadastralnumber character varying(50),
                  prevstateregnumber_inventorynumber character varying(50),
                  prevstateregnumber_conditionalnumber character varying(50),
                  terminationdate timestamp without time zone,
                  purpose_code character varying(50),
                  purpose_guid character varying(50),
                  position_code character varying(50),
                  position_guid character varying(50),
                  grossarea numeric(19,5),
                  floor character varying(50),
                  totalarea numeric(18,5),
                  iscommonproperty boolean,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_nonresidentialpremises_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_nonresident_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_nresprem_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_nresprem_house", @"
                    CREATE INDEX ind_ris_nresprem_house
                    ON ris_nonresidentialpremises
                    USING btree
                    (house_id);
                ")
            }),

            #endregion

            #region RIS_RESIDENTIALPREMISES

            new TableProxy("ris_residentialpremises", @"
                CREATE TABLE ris_residentialpremises
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  house_id bigint,
                  premisesnum character varying(50),
                  cadastralnumber character varying(50),
                  prevstateregnumber_cadastralnumber character varying(50),
                  prevstateregnumber_inventorynumber character varying(50),
                  prevstateregnumber_conditionalnumber character varying(50),
                  terminationdate timestamp without time zone,
                  entrancenum smallint,
                  premisescharacteristic_code character varying(50),
                  premisescharacteristic_guid character varying(50),
                  roomsnum_code character varying(50),
                  roomsnum_guid character varying(50),
                  residentialhousetype_code character varying(50),
                  residentialhousetype_guid character varying(50),
                  grossarea numeric(19,5),
                  floor character varying(50),
                  totalarea numeric(18,5),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_residentialpremises_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_residential_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_resprem_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_resprem_house", @"
                    CREATE INDEX ind_ris_resprem_house
                    ON ris_residentialpremises
                    USING btree
                    (house_id);
                ")
            }),

            #endregion

            #region RIS_CONTRACT

            new TableProxy("ris_contract", @"
                CREATE TABLE ris_contract
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  docnum character varying(200),
                  signingdate timestamp without time zone,
                  effectivedate timestamp without time zone,
                  plandatecomptetion timestamp without time zone,
                  validity_month integer,
                  validity_year integer,
                  owners_type smallint,
                  org_id bigint,
                  protocol_purchasenumber character varying(200),
                  contractbase_code character varying(200),
                  contractbase_guid character varying(200),
                  operation smallint NOT NULL DEFAULT 0,
                  imd_values_begindate integer,
                  imd_values_enddate integer,
                  payment_doc_date integer,
                  drawing_pd_date_this_month boolean NOT NULL DEFAULT false,
                  license_request boolean NOT NULL DEFAULT false,
                  gi_contragent_id bigint,
                  period_metering_startdate_this_month boolean,
                  period_metering_enddate_this_month boolean,
                  payment_serv_date smallint,
                  payment_serv_date_this_month boolean,
                  CONSTRAINT ris_contract_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_contract_org FOREIGN KEY (org_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_contract_org", @"
                    CREATE INDEX ind_ris_contract_org
                    ON ris_contract
                    USING btree
                    (org_id);
                ")
            }),

            #endregion

            #region RIS_METERING_DEVICE_DATA

            new TableProxy("ris_metering_device_data", @"
                CREATE TABLE ris_metering_device_data
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  metering_device_type smallint NOT NULL DEFAULT 10,
                  metering_device_number character varying(50) NOT NULL,
                  metering_device_stamp character varying(50),
                  installation_date timestamp without time zone,
                  commissioning_date timestamp without time zone,
                  manual_mode_metering boolean,
                  first_verification_date timestamp without time zone,
                  verification_interval character varying(50),
                  device_type smallint NOT NULL DEFAULT 10,
                  metering_value_t1 numeric(18,5) NOT NULL,
                  metering_value_t2 numeric(18,5),
                  metering_value_t3 numeric(18,5),
                  readout_date timestamp without time zone NOT NULL,
                  readings_source character varying(50),
                  house_id bigint,
                  residential_premises_id bigint,
                  nonresidential_premises_id bigint,
                  municipal_resource_code character varying(50),
                  municipal_resource_guid character varying(50),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  archiving_reason_code character varying(50),
                  archiving_reason_guid character varying(50),
                  planned_verification boolean,
                  one_rate_device_value numeric(18,5),
                  archivation smallint,
                  base_value_t1 numeric(18,5),
                  base_value_t2 numeric(18,5),
                  base_value_t3 numeric(18,5),
                  begin_date timestamp without time zone,
                  replacing_metering_device_guid character varying(255),
                  verification_interval_guid character varying(255),
                  metering_device_model character varying(255),
                  factory_seal_date timestamp without time zone,
                  temperature_sensor boolean,
                  pressure_sensor boolean,
                  transformation_ratio numeric,
                  verification_date timestamp without time zone,
                  seal_date timestamp without time zone,
                  reason_verification_code character varying(255),
                  reason_verification_guid character varying(255),
                  manual_mode_information character varying(255),
                  temperature_sensor_information character varying(255),
                  pressure_sensor_information character varying(255),
                  replacing_metering_device_version_guid character varying(255),
                  CONSTRAINT ris_metering_device_data_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_met_dev_dat_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_metering_device_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_metering_device_nonresidential_premises FOREIGN KEY (nonresidential_premises_id)
                      REFERENCES ris_nonresidentialpremises (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_metering_device_residential_premises FOREIGN KEY (residential_premises_id)
                      REFERENCES ris_residentialpremises (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_metering_device_house", @"
                    CREATE INDEX ind_ris_metering_device_house
                    ON ris_metering_device_data
                    USING btree
                    (house_id);
                "),
                new IndexProxy("ind_ris_metering_device_nonresidential_premises", @"
                    CREATE INDEX ind_ris_metering_device_nonresidential_premises
                    ON ris_metering_device_data
                    USING btree
                    (nonresidential_premises_id);
                "),
                new IndexProxy("ind_ris_metering_device_residential_premises", @"
                    CREATE INDEX ind_ris_metering_device_residential_premises
                    ON ris_metering_device_data
                    USING btree
                    (residential_premises_id);
                ")
            }),

            #endregion

            #region RIS_METERING_DEVICE_CONTROL_VALUE

            new TableProxy("ris_metering_device_control_value", @"
                CREATE TABLE ris_metering_device_control_value
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    guid character varying(50),
                    metering_device_data_id bigint,
                    account_id bigint,
                    value_t1 numeric(18,5) NOT NULL,
                    value_t2 numeric(18,5),
                    value_t3 numeric(18,5),
                    readout_date timestamp without time zone NOT NULL,
                    reading_source character varying(50),
                    operation smallint NOT NULL DEFAULT 0,
                    gi_contragent_id bigint,
                    municipal_resource_code character varying(255),
                    municipal_resource_guid character varying(255),
                    CONSTRAINT ris_metering_device_control_value_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_ris_metering_device_control_value_account FOREIGN KEY (account_id)
                        REFERENCES ris_account (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_ris_metering_device_control_value_data FOREIGN KEY (metering_device_data_id)
                        REFERENCES ris_metering_device_data (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_rismetdevconval_ctrg FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_metering_device_control_value_account", @"
                    CREATE INDEX ind_ris_metering_device_control_value_account
                    ON ris_metering_device_control_value
                    USING btree
                    (account_id);
                "),
                new IndexProxy("ind_ris_metering_device_control_value_data", @"
                    CREATE INDEX ind_ris_metering_device_control_value_data
                    ON ris_metering_device_control_value
                    USING btree
                    (metering_device_data_id);
                ")
            }),

            #endregion

            #region RIS_METERING_DEVICE_CURRENT_VALUE

            new TableProxy("ris_metering_device_current_value", @"
                CREATE TABLE ris_metering_device_current_value
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  metering_device_data_id bigint,
                  account_id bigint,
                  value_t1 numeric(18,5) NOT NULL,
                  value_t2 numeric(18,5),
                  value_t3 numeric(18,5),
                  readout_date timestamp without time zone NOT NULL,
                  reading_source character varying(50),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  municipal_resource_code character varying(255),
                  municipal_resource_guid character varying(255),
                  CONSTRAINT ris_metering_device_current_value_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_metering_device_current_value_account FOREIGN KEY (account_id)
                      REFERENCES ris_account (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_metering_device_current_value_data FOREIGN KEY (metering_device_data_id)
                      REFERENCES ris_metering_device_data (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_rismetdevcurval_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_metering_device_current_value_account", @"
                    CREATE INDEX ind_ris_metering_device_current_value_account
                    ON ris_metering_device_current_value
                    USING btree
                    (account_id);
                "),
                new IndexProxy("ind_ris_metering_device_current_value_data", @"
                    CREATE INDEX ind_ris_metering_device_current_value_data
                    ON ris_metering_device_current_value
                    USING btree
                    (metering_device_data_id);
                ")
            }),

            #endregion

            #region RIS_METERING_DEVICE_VERIFICATION_VALUE

            new TableProxy("ris_metering_device_verification_value", @"
                CREATE TABLE ris_metering_device_verification_value
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  metering_device_data_id bigint,
                  account_id bigint,
                  start_verification_value_t1 numeric(18,5) NOT NULL,
                  start_verification_value_t2 numeric(18,5),
                  start_verification_value_t3 numeric(18,5),
                  start_verification_readout_date timestamp without time zone NOT NULL,
                  start_verification_reading_source character varying(50),
                  end_verification_value_t1 numeric(18,5) NOT NULL,
                  end_verification_value_t2 numeric(18,5),
                  end_verification_value_t3 numeric(18,5),
                  end_verification_readout_date timestamp without time zone NOT NULL,
                  end_verification_reading_source character varying(50),
                  planned_verification boolean,
                  verification_reason_code character varying(50),
                  verification_reason_guid character varying(50),
                  verification_reason_name character varying(50),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  seal_date timestamp without time zone,
                  municipal_resource_code character varying(255),
                  municipal_resource_guid character varying(255),
                  CONSTRAINT ris_metering_device_verification_value_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_metering_device_verification_value_account FOREIGN KEY (account_id)
                      REFERENCES ris_account (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_metering_device_verification_value_data FOREIGN KEY (metering_device_data_id)
                      REFERENCES ris_metering_device_data (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_rismetdevverval_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_metering_device_verification_value_account", @"
                    CREATE INDEX ind_ris_metering_device_verification_value_account
                    ON ris_metering_device_verification_value
                    USING btree
                    (account_id);
                "),
                new IndexProxy("ind_ris_metering_device_verification_value_data", @"
                    CREATE INDEX ind_ris_metering_device_verification_value_data
                    ON ris_metering_device_verification_value
                    USING btree
                    (metering_device_data_id);
                ")
            }),

            #endregion

            #region RIS_CHARTER

            new TableProxy("ris_charter", @"
                CREATE TABLE ris_charter
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    guid character varying(50),
                    docnum character varying(50),
                    docdate timestamp without time zone,
                    managers character varying(255),
                    approvalcharter boolean,
                    rollovercharter boolean,
                    attachment_id bigint,
                    head_id bigint,
                    operation smallint NOT NULL DEFAULT 0,
                    gi_contragent_id bigint,
                    this_month_payment_docdate boolean,
                    period_metering_startdate integer,
                    period_metering_enddate integer,
                    period_metering_lastday boolean,
                    payment_date_startdate integer,
                    payment_date_lastday boolean,
                    terminate_charter_date boolean,
                    terminate_charter_reason boolean,
                    period_metering_startdate_this_month boolean,
                    period_metering_enddate_this_month boolean,
                    payment_serv_date smallint,
                    payment_serv_date_this_month boolean,
                    is_managed_by_contract boolean,
                    CONSTRAINT ris_charter_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_ris_charter_ctrg FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_ris_charter_head_ind FOREIGN KEY (head_id)
                        REFERENCES ris_ind (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_charter_head_ind", @"
                    CREATE INDEX ind_ris_charter_head_ind
                    ON ris_charter
                    USING btree
                    (head_id);
                ")
            }),

            #endregion

            #region RIS_AGREEMENT

            new TableProxy("ris_agreement", @"
                CREATE TABLE ris_agreement
            (
              id serial NOT NULL,
              object_version bigint NOT NULL,
              object_create_date timestamp without time zone NOT NULL,
              object_edit_date timestamp without time zone NOT NULL,
              external_id bigint NOT NULL,
              external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
              guid character varying(50),
              attachment_id bigint,
              contract_id bigint,
              agreementdate timestamp without time zone,
              agreementnumber character varying(200),
              operation smallint NOT NULL DEFAULT 0,
              ris_contragent_id bigint,
              CONSTRAINT ris_agreement_pkey PRIMARY KEY (id),
              CONSTRAINT fk_ris_agreement_attach FOREIGN KEY (attachment_id)
                  REFERENCES gi_attachment (id) MATCH SIMPLE
                  ON UPDATE NO ACTION ON DELETE NO ACTION,
              CONSTRAINT fk_ris_agreement_contr FOREIGN KEY (contract_id)
                  REFERENCES ris_contract (id) MATCH SIMPLE
                  ON UPDATE NO ACTION ON DELETE NO ACTION
            )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_agreement_attach", @"
                    CREATE INDEX ind_ris_agreement_attach
                    ON ris_agreement
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_ris_agreement_contr", @"
                    CREATE INDEX ind_ris_agreement_contr
                    ON ris_agreement
                    USING btree
                    (contract_id);
                ")
            }),

            #endregion

            #region RIS_PROTOCOLMEETINGOWNER

            new TableProxy("ris_protocolmeetingowner", @"
                CREATE TABLE ris_protocolmeetingowner
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  charter_id bigint,
                  attachment_id bigint,
                  contract_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_protocolmeetingowner_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_protmeetown_attach FOREIGN KEY (attachment_id)
                      REFERENCES gi_attachment (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_protmeetown_charter FOREIGN KEY (charter_id)
                      REFERENCES ris_charter (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_protmeetown_contragent FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_protmeetown_attach", @"
                    CREATE INDEX ind_ris_protmeetown_attach
                    ON ris_protocolmeetingowner
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_ris_protmeetown_charter", @"
                    CREATE INDEX ind_ris_protmeetown_charter
                    ON ris_protocolmeetingowner
                    USING btree
                    (charter_id);
                "),
                new IndexProxy("ind_ris_protmeetown_contragent", @"
                    CREATE INDEX ind_ris_protmeetown_contragent
                    ON ris_protocolmeetingowner
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region RIS_CONTRACTOBJECT

            new TableProxy("ris_contractobject", @"
                CREATE TABLE ris_contractobject
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  charter_id bigint,
                  house_id bigint,
                  startdate timestamp without time zone,
                  enddate timestamp without time zone,
                  basemservise_currentcharter boolean,
                  basemservise_protocolmeetingowner_id bigint,
                  dateexclusion timestamp without time zone,
                  baseexclusion_currentcharter boolean,
                  baseexclusion_protocolmeetingowner_id bigint,
                  contract_id bigint,
                  statusobject smallint,
                  baseexclusion_agreement_id bigint,
                  basemservise_agreement_id bigint,
                  periodmetering_startdate integer,
                  periodmetering_enddate integer,
                  periodmetering_lastday boolean,
                  paymentdate_startdate integer,
                  paymentdate_lastday boolean,
                  paymentdate_currentmounth boolean,
                  paymentdate_nextmounth boolean,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  realityobject_id bigint,
                  fiashouse_guid character varying(200),
                  CONSTRAINT ris_contractobject_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_bexcl_controbj_pmo FOREIGN KEY (baseexclusion_protocolmeetingowner_id)
                      REFERENCES ris_protocolmeetingowner (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_bms_controbj_pmo FOREIGN KEY (basemservise_protocolmeetingowner_id)
                      REFERENCES ris_protocolmeetingowner (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_contractobj_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_controbj_charter FOREIGN KEY (charter_id)
                      REFERENCES ris_charter (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_controbj_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_bexcl_controbj_pmo", @"
                    CREATE INDEX ind_ris_bexcl_controbj_pmo
                    ON ris_contractobject
                    USING btree
                    (baseexclusion_protocolmeetingowner_id);
                "),
                new IndexProxy("ind_ris_bms_controbj_pmo", @"
                    CREATE INDEX ind_ris_bms_controbj_pmo
                    ON ris_contractobject
                    USING btree
                    (basemservise_protocolmeetingowner_id);
                "),
                new IndexProxy("ind_ris_controbj_charter", @"
                    CREATE INDEX ind_ris_controbj_charter
                    ON ris_contractobject
                    USING btree
                    (charter_id);
                "),
                new IndexProxy("ind_ris_controbj_house", @"
                    CREATE INDEX ind_ris_controbj_house
                    ON ris_contractobject
                    USING btree
                    (house_id);
                ")
            }),

            #endregion

            #region RIS_ADDSERVICE

            new TableProxy("ris_addservice", @"
                CREATE TABLE ris_addservice
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  contractobject_id bigint,
                  servicetype_code character varying(50),
                  servicetype_guid character varying(50),
                  startdate timestamp without time zone,
                  enddate timestamp without time zone,
                  baseservisecharter_currentcharter boolean,
                  baseservicecharter_protocolmeetingowner_id bigint,
                  baseservise_agreement_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_addservice_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_addserv_controbj FOREIGN KEY (contractobject_id)
                      REFERENCES ris_contractobject (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_addservice_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_bsc_addserv_pmo FOREIGN KEY (baseservicecharter_protocolmeetingowner_id)
                      REFERENCES ris_protocolmeetingowner (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_addserv_controbj", @"
                    CREATE INDEX ind_ris_addserv_controbj
                    ON ris_addservice
                    USING btree
                    (contractobject_id);
                "),
                new IndexProxy("ind_ris_bsc_addserv_pmo", @"
                    CREATE INDEX ind_ris_bsc_addserv_pmo
                    ON ris_addservice
                    USING btree
                    (baseservicecharter_protocolmeetingowner_id);
                ")
            }),

            #endregion

            #region RIS_BLOCK

            new TableProxy("ris_block", @"
                CREATE TABLE ris_block
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  operation smallint NOT NULL DEFAULT 0,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  gi_contragent_id bigint,
                  guid character varying(50),
                  house_id bigint,
                  cadastralnumber character varying(255),
                  blocknum character varying(255),
                  premisescharacteristiccode character varying(255),
                  premisescharacteristicguid character varying(255),
                  totalarea numeric,
                  grossarea numeric,
                  terminationdate date,
                  CONSTRAINT ris_block_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_block_contragent FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_block_house_id FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_block_contragent", @"
                    CREATE INDEX ind_ris_block_contragent
                    ON ris_block
                    USING btree
                    (gi_contragent_id);
                "),
                new IndexProxy("ind_ris_block_house_id", @"
                    CREATE INDEX ind_ris_block_house_id
                    ON ris_block
                    USING btree
                    (house_id);
                ")
            }),

            #endregion

            #region RIS_CONFIRMDOC

            new TableProxy("ris_confirmdoc", @"
                CREATE TABLE ris_confirmdoc
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  house_id bigint,
                  name character varying(50),
                  description character varying(50),
                  attachment_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  ris_contragent_id bigint,
                  CONSTRAINT ris_confirmdoc_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_confdoc_attach FOREIGN KEY (attachment_id)
                      REFERENCES gi_attachment (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_confdoc_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_confdoc_attach", @"
                    CREATE INDEX ind_ris_confdoc_attach
                    ON ris_confirmdoc
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_ris_confdoc_house", @"
                    CREATE INDEX ind_ris_confdoc_house
                    ON ris_confirmdoc
                    USING btree
                    (house_id);
                ")
            }),

            #endregion

            #region HOUSE_SERVICE

            new TableProxy("house_service", @"
                CREATE TABLE house_service
                (
                  id serial NOT NULL,
                  operation smallint NOT NULL DEFAULT 0,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  contractobject_id bigint,
                  servicetype_code character varying(50),
                  servicetype_guid character varying(50),
                  startdate timestamp without time zone,
                  enddate timestamp without time zone,
                  baseservice_currentdoc boolean,
                  baseservice_agreement_id bigint,
                  gi_contragent_id bigint,
                  CONSTRAINT house_service_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_house_service_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_housserv_agreement_id FOREIGN KEY (baseservice_agreement_id)
                      REFERENCES ris_agreement (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_housserv_controbj FOREIGN KEY (contractobject_id)
                      REFERENCES ris_contractobject (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_housserv_agreement_id", @"
                    CREATE INDEX ind_housserv_agreement_id
                    ON house_service
                    USING btree
                    (baseservice_agreement_id);
                "),
                new IndexProxy("ind_housserv_controbj", @"
                    CREATE INDEX ind_housserv_controbj
                    ON house_service
                    USING btree
                    (contractobject_id);
                ")
            }),

            #endregion

            #region RIS_LIVINGROOM

            new TableProxy("ris_livingroom", @"
                CREATE TABLE ris_livingroom
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  res_premises_id bigint,
                  house_id bigint,
                  roomnumber character varying(50),
                  square numeric(19,5),
                  terminationdate timestamp without time zone,
                  cadastralnumber character varying(50),
                  prevstateregnumber_cadastralnumber character varying(50),
                  prevstateregnumber_inventorynumber character varying(50),
                  prevstateregnumber_conditionalnumber character varying(50),
                  floor character varying(50),
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  block_id bigint,
                  CONSTRAINT ris_livingroom_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_livingroom_block FOREIGN KEY (block_id)
                      REFERENCES ris_block (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_livingroom_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_livroom_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_livroom_premises FOREIGN KEY (res_premises_id)
                      REFERENCES ris_residentialpremises (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_livingroom_block", @"
                    CREATE INDEX ind_ris_livingroom_block
                    ON ris_livingroom
                    USING btree
                    (block_id);
                "),
                new IndexProxy("ind_ris_livroom_house", @"
                    CREATE INDEX ind_ris_livroom_house
                    ON ris_livingroom
                    USING btree
                    (house_id);
                "),
                new IndexProxy("ind_ris_livroom_premises", @"
                    CREATE INDEX ind_ris_livroom_premises
                    ON ris_livingroom
                    USING btree
                    (res_premises_id);
                ")
            }),

            #endregion

            #region RIS_ACCOUNT_RELATIONS

            new TableProxy("ris_account_relations", @"
                CREATE TABLE ris_account_relations
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  account_id bigint,
                  house_id bigint,
                  residential_premise_id bigint,
                  nonresidential_premise_id bigint,
                  living_room_id bigint,
                  gi_contragent_id bigint,
                  external_system_name character varying(255),
                  guid character varying(255),
                  operation smallint,
                  external_id bigint,
                  CONSTRAINT ris_account_relations_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_account_relations_account FOREIGN KEY (account_id)
                      REFERENCES ris_account (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_account_relations_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_account_relations_nonresidential_premise FOREIGN KEY (nonresidential_premise_id)
                      REFERENCES ris_nonresidentialpremises (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_account_relations_residential_premise FOREIGN KEY (residential_premise_id)
                      REFERENCES ris_residentialpremises (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_account_relations_ris_contragent_id FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_account_relations_account", @"
                    CREATE INDEX ind_ris_account_relations_account
                    ON ris_account_relations
                    USING btree
                    (account_id);
                "),
                new IndexProxy("ind_ris_account_relations_house", @"
                    CREATE INDEX ind_ris_account_relations_house
                    ON ris_account_relations
                    USING btree
                    (house_id);
                "),
                new IndexProxy("ind_ris_account_relations_nonresidential_premise", @"
                    CREATE INDEX ind_ris_account_relations_nonresidential_premise
                    ON ris_account_relations
                    USING btree
                    (nonresidential_premise_id);
                "),
                new IndexProxy("ind_ris_account_relations_residential_premise", @"
                    CREATE INDEX ind_ris_account_relations_residential_premise
                    ON ris_account_relations
                    USING btree
                    (residential_premise_id);
                "),
                new IndexProxy("ind_ris_account_relations_ris_contragent_id", @"
                    CREATE INDEX ind_ris_account_relations_ris_contragent_id
                    ON ris_account_relations
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region RIS_PUBLICPROPERTYCONTRACT

            new TableProxy("ris_publicpropertycontract", @"
                CREATE TABLE ris_publicpropertycontract
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  startdate timestamp without time zone,
                  enddate timestamp without time zone,
                  contractnumber character varying(50),
                  contractobject character varying(50),
                  comments character varying(1000),
                  datesignature timestamp without time zone,
                  issignatured boolean,
                  house_id bigint,
                  entrepreneur_id bigint,
                  organization_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  protocolnumber character varying(200),
                  protocoldate timestamp without time zone,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_publicpropertycontract_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_pubpropcontr_cont FOREIGN KEY (organization_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_pubpropcontr_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_pubpropcontr_ind FOREIGN KEY (entrepreneur_id)
                      REFERENCES ris_ind (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_pubpropcontr_cont", @"
                    CREATE INDEX ind_ris_pubpropcontr_cont
                    ON ris_publicpropertycontract
                    USING btree
                    (organization_id);
                "),
                new IndexProxy("ind_ris_pubpropcontr_house", @"
                    CREATE INDEX ind_ris_pubpropcontr_house
                    ON ris_publicpropertycontract
                    USING btree
                    (house_id);
                "),
                new IndexProxy("ind_ris_pubpropcontr_ind", @"
                    CREATE INDEX ind_ris_pubpropcontr_ind
                    ON ris_publicpropertycontract
                    USING btree
                    (entrepreneur_id);
                ")
            }),

            #endregion

            #region RIS_CONTRACTATTACHMENT

            new TableProxy("ris_contractattachment", @"
                CREATE TABLE ris_contractattachment
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  publicpropertycontract_id bigint,
                  contract_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  charter_id bigint,
                  attachment_id bigint,
                  CONSTRAINT ris_contractattachment_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_contratt_attach FOREIGN KEY (attachment_id)
                      REFERENCES gi_attachment (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_contratt_pubcontract FOREIGN KEY (publicpropertycontract_id)
                      REFERENCES ris_publicpropertycontract (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_contrattach_contragent FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_contratt_attach", @"
                    CREATE INDEX ind_ris_contratt_attach
                    ON ris_contractattachment
                    USING btree
                    (attachment_id);
                "),
                new IndexProxy("ind_ris_contratt_pubcontract", @"
                    CREATE INDEX ind_ris_contratt_pubcontract
                    ON ris_contractattachment
                    USING btree
                    (publicpropertycontract_id);
                "),
                new IndexProxy("ind_ris_contrattach_contragent", @"
                    CREATE INDEX ind_ris_contrattach_contragent
                    ON ris_contractattachment
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region RIS_VOTINGPROTOCOL

            new TableProxy("ris_votingprotocol", @"
                CREATE TABLE ris_votingprotocol
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  house_id bigint,
                  protocolnum character varying(200),
                  protocoldate timestamp without time zone,
                  votingplace character varying(200),
                  enddate timestamp without time zone,
                  begindate timestamp without time zone,
                  discipline character varying(200),
                  meetingeligibility smallint,
                  votingtype smallint,
                  votingtimetype smallint,
                  revert boolean,
                  place boolean,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_votingprotocol_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_votingprot_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_votingproto_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_votingprot_house", @"
                    CREATE INDEX ind_ris_votingprot_house
                    ON ris_votingprotocol
                    USING btree
                    (house_id);
                ")
            }),

            #endregion

            #region SUPPLY_RESOURCE_CONTRACT

            new TableProxy("supply_resource_contract", @"
                CREATE TABLE supply_resource_contract
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  operation smallint NOT NULL DEFAULT 0,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  gi_contragent_id bigint,
                  guid character varying(50),
                  comptetion_date timestamp without time zone,
                  start_date smallint,
                  start_date_next_month boolean,
                  end_date smallint,
                  end_date_next_month boolean,
                  contract_base_code character varying(255),
                  contract_base_guid character varying(255),
                  contract_type smallint,
                  person_type smallint,
                  person_type_organization smallint,
                  contragent_id bigint,
                  ind_surname character varying(255),
                  ind_firstname character varying(255),
                  ind_patronymic character varying(255),
                  ind_sex smallint,
                  ind_date_of_birth timestamp without time zone,
                  ind_snils character varying(255),
                  ind_identity_type_code character varying(255),
                  ind_identity_type_guid character varying(255),
                  ind_identity_series character varying(255),
                  ind_identity_number character varying(255),
                  ind_identity_issue_date timestamp without time zone,
                  ind_place_birth character varying(255),
                  comm_metering_res_type smallint,
                  fias_house_guid character varying(255),
                  contract_number character varying(255),
                  signing_date timestamp without time zone,
                  effective_date timestamp without time zone,
                  billing_date smallint,
                  payment_date smallint,
                  providing_information_date smallint,
                  terminate_reason_code character varying(255),
                  terminate_reason_guid character varying(255),
                  terminate_date timestamp without time zone,
                  rollover_date timestamp without time zone,
                  CONSTRAINT supply_resource_contract_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_sr_contract_contragent FOREIGN KEY (contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_supply_resource_contract_contragent FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_sr_contract_contragent", @"
                    CREATE INDEX ind_sr_contract_contragent
                    ON supply_resource_contract
                    USING btree
                    (contragent_id);
                "),
                new IndexProxy("ind_supply_resource_contract_contragent", @"
                    CREATE INDEX ind_supply_resource_contract_contragent
                    ON supply_resource_contract
                    USING btree
                    (gi_contragent_id);
                ")
            }),

            #endregion

            #region RIS_DECISIONLIST

            new TableProxy("ris_decisionlist", @"
                CREATE TABLE ris_decisionlist
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  ""votingprotoсol_id"" bigint,
                  questionnumber integer,
                  questionname character varying(200),
                  decisionstype_code character varying(50),
                  decisionstype_guid character varying(50),
                  agree numeric(18,5),
                  against numeric(18,5),
                  abstent numeric(18,5),
                  votingresume smallint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  forming_funde_code character varying(255),
                  forming_fund_guid character varying(255),
                  CONSTRAINT ris_decisionlist_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_decisionlis_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_declist_votingprot FOREIGN KEY (""votingprotoсol_id"")
                      REFERENCES ris_votingprotocol (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_declist_votingprot", @"
                    CREATE INDEX ind_ris_declist_votingprot
                    ON ris_decisionlist
                    USING btree
                    (""votingprotoсol_id"");
                ")
            }),

            #endregion

            #region RIS_SHARE

            new TableProxy("ris_share", @"
                CREATE TABLE ris_share
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  isprivatized boolean,
                  termdate timestamp without time zone,
                  contragent_id bigint,
                  account_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_share_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_share_account FOREIGN KEY (account_id)
                      REFERENCES ris_account (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_share_contragent FOREIGN KEY (contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_share_account", @"
                    CREATE INDEX ind_ris_share_account
                    ON ris_share
                    USING btree
                    (account_id);
                "),
                new IndexProxy("ind_ris_share_contragent", @"
                    CREATE INDEX ind_ris_share_contragent
                    ON ris_share
                    USING btree
                    (contragent_id);
                ")
            }),

            #endregion

            #region RIS_ECNBR

            new TableProxy("ris_ecnbr", @"
                CREATE TABLE ris_ecnbr
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  enddate timestamp without time zone,
                  kindcode character varying(50),
                  kindguid character varying(50),
                  contragent_id bigint,
                  share_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_ecnbr_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_ecnbr_contragent FOREIGN KEY (contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_ecnbr_share FOREIGN KEY (share_id)
                      REFERENCES ris_share (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_ecnbr_contragent", @"
                    CREATE INDEX ind_ris_ecnbr_contragent
                    ON ris_ecnbr
                    USING btree
                    (contragent_id);
                "),
                new IndexProxy("ind_ris_ecnbr_share", @"
                    CREATE INDEX ind_ris_ecnbr_share
                    ON ris_ecnbr
                    USING btree
                    (share_id);
                ")
            }),

            #endregion

            #region RIS_ECNBRIND

            new TableProxy("ris_ecnbrind", @"
                CREATE TABLE ris_ecnbrind
                (
                    id serial NOT NULL,
                    object_version bigint NOT NULL,
                    object_create_date timestamp without time zone NOT NULL,
                    object_edit_date timestamp without time zone NOT NULL,
                    external_id bigint NOT NULL,
                    external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                    guid character varying(50),
                    ecnbr_id bigint,
                    ind_id bigint,
                    operation smallint NOT NULL DEFAULT 0,
                    gi_contragent_id bigint,
                    CONSTRAINT ris_ecnbrind_pkey PRIMARY KEY (id),
                    CONSTRAINT fk_ris_ecnbrind_ctrg FOREIGN KEY (gi_contragent_id)
                        REFERENCES gi_contragent (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_ris_ecnbrind_ecnbr FOREIGN KEY (ecnbr_id)
                        REFERENCES ris_ecnbr (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION,
                    CONSTRAINT fk_ris_ecnbrind_ind FOREIGN KEY (ind_id)
                        REFERENCES ris_ind (id) MATCH SIMPLE
                        ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_ecnbrind_ecnbr", @"
                    CREATE INDEX ind_ris_ecnbrind_ecnbr
                    ON ris_ecnbrind
                    USING btree
                    (ecnbr_id);
                "),
                new IndexProxy("ind_ris_ecnbrind_ind", @"
                    CREATE INDEX ind_ris_ecnbrind_ind
                    ON ris_ecnbrind
                    USING btree
                    (ind_id);
                ")
            }),

            #endregion

            #region RIS_ENTRANCE

            new TableProxy("ris_entrance", @"
                CREATE TABLE ris_entrance
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  house_id bigint,
                  entrancenum smallint,
                  storeyscount smallint,
                  creationdate timestamp without time zone,
                  terminationdate timestamp without time zone,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_entrance_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_entrance_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_entrance_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_entrance_house", @"
                    CREATE INDEX ind_ris_entrance_house
                    ON ris_entrance
                    USING btree
                    (house_id);
                ")
            }),

            #endregion

            #region RIS_HOUSE_INNER_WALL_MATERIAL

            new TableProxy("ris_house_inner_wall_material", @"
                CREATE TABLE ris_house_inner_wall_material
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  house_id bigint,
                  innerwallmaterialcode character varying(50) NOT NULL,
                  innerwallmaterialguid character varying(50) NOT NULL,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_house_inner_wall_material_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_house_inner_wall_material_house FOREIGN KEY (house_id)
                      REFERENCES ris_house (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_rishouinnwalmat_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_house_inner_wall_material_house", @"
                    CREATE INDEX ind_ris_house_inner_wall_material_house
                    ON ris_house_inner_wall_material
                    USING btree
                    (house_id);
                ")
            }),

            #endregion

            #region RIS_METERING_DEVICE_ACCOUNT

            new TableProxy("ris_metering_device_account", @"
                CREATE TABLE ris_metering_device_account
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  account_id bigint,
                  metering_device_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_metering_device_account_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_met_dev_acc_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_metering_device_account_account FOREIGN KEY (account_id)
                      REFERENCES ris_account (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_metering_device_account_metering_device FOREIGN KEY (metering_device_id)
                      REFERENCES ris_metering_device_data (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_metering_device_account_account", @"
                    CREATE INDEX ind_ris_metering_device_account_account
                    ON ris_metering_device_account
                    USING btree
                    (account_id);
                "),
                new IndexProxy("ind_ris_metering_device_account_metering_device", @"
                    CREATE INDEX ind_ris_metering_device_account_metering_device
                    ON ris_metering_device_account
                    USING btree
                    (metering_device_id);
                ")
            }),

            #endregion

            #region RIS_METERING_DEVICE_LIVING_ROOM

            new TableProxy("ris_metering_device_living_room", @"
                CREATE TABLE ris_metering_device_living_room
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  living_room_id bigint,
                  metering_device_id bigint,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_metering_device_living_room_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_metering_device_living_room_living_room FOREIGN KEY (living_room_id)
                      REFERENCES ris_livingroom (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_ris_metering_device_living_room_metering_device FOREIGN KEY (metering_device_id)
                      REFERENCES ris_metering_device_data (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION,
                  CONSTRAINT fk_rismetdevlivroo_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            ", new List<IndexProxy>
            {
                new IndexProxy("ind_ris_metering_device_living_room_living_room", @"
                    CREATE INDEX ind_ris_metering_device_living_room_living_room
                    ON ris_metering_device_living_room
                    USING btree
                    (living_room_id);
                "),
                new IndexProxy("ind_ris_metering_device_living_room_metering_device", @"
                    CREATE INDEX ind_ris_metering_device_living_room_metering_device
                    ON ris_metering_device_living_room
                    USING btree
                    (metering_device_id);
                ")
            }),

            #endregion

            #region RIS_NOTIFICATION

            new TableProxy("ris_notification", @"
                CREATE TABLE ris_notification
                (
                  id serial NOT NULL,
                  object_version bigint NOT NULL,
                  object_create_date timestamp without time zone NOT NULL,
                  object_edit_date timestamp without time zone NOT NULL,
                  external_id bigint NOT NULL,
                  external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                  guid character varying(50),
                  topic character varying(200),
                  isimportant boolean,
                  content text,
                  isall boolean,
                  isnotlimit boolean,
                  startdate timestamp without time zone,
                  enddate timestamp without time zone,
                  isshipoff boolean,
                  deleted boolean,
                  operation smallint NOT NULL DEFAULT 0,
                  gi_contragent_id bigint,
                  CONSTRAINT ris_notification_pkey PRIMARY KEY (id),
                  CONSTRAINT fk_ris_notificatio_ctrg FOREIGN KEY (gi_contragent_id)
                      REFERENCES gi_contragent (id) MATCH SIMPLE
                      ON UPDATE NO ACTION ON DELETE NO ACTION
                )
            "),

            #endregion

            #region RIS_NOTIFICATION_ADDRESSEE

            new TableProxy("ris_notification_addressee", @"
                    CREATE TABLE ris_notification_addressee
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      house_id bigint,
                      notification_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_notification_addressee_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_notif_addr_house FOREIGN KEY (house_id)
                          REFERENCES ris_house (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_notif_addr_notif FOREIGN KEY (notification_id)
                          REFERENCES ris_notification (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_notif_addre_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_notif_addr_house", @"
                        CREATE INDEX ind_ris_notif_addr_house
                        ON ris_notification_addressee
                        USING btree
                        (house_id);
                    "),
                    new IndexProxy("ind_ris_notif_addr_notif", @"
                        CREATE INDEX ind_ris_notif_addr_notif
                        ON ris_notification_addressee
                        USING btree
                        (notification_id);
                    ")
                }),

            #endregion

            #region RIS_NOTIFICATION_ATTACHMENT

            new TableProxy("ris_notification_attachment", @"
                    CREATE TABLE ris_notification_attachment
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      attachment_id bigint,
                      notification_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_notification_attachment_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_notif_att_attach FOREIGN KEY (attachment_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_notif_att_notif FOREIGN KEY (notification_id)
                          REFERENCES ris_notification (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_notif_attac_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_notif_att_attach", @"
                        CREATE INDEX ind_ris_notif_att_attach
                        ON ris_notification_attachment
                        USING btree
                        (attachment_id);
                    "),
                    new IndexProxy("ind_ris_notif_att_notif", @"
                        CREATE INDEX ind_ris_notif_att_notif
                        ON ris_notification_attachment
                        USING btree
                        (notification_id);
                    ")
                }),

            #endregion

            #region RIS_PROTOCOLOK

            new TableProxy("ris_protocolok", @"
                    CREATE TABLE ris_protocolok
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      attachment_id bigint,
                      contract_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_protocolok_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_protocolok_attach FOREIGN KEY (attachment_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_protocolok_contr FOREIGN KEY (contract_id)
                          REFERENCES ris_contract (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_protocolok_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_protocolok_attach", @"
                        CREATE INDEX ind_ris_protocolok_attach
                        ON ris_protocolok
                        USING btree
                        (attachment_id);
                    "),
                    new IndexProxy("ind_ris_protocolok_contr", @"
                        CREATE INDEX ind_ris_protocolok_contr
                        ON ris_protocolok
                        USING btree
                        (contract_id);
                    ")
                }),

            #endregion

            #region RIS_SHAREECNBRLIVINGHOUSE

            new TableProxy("ris_shareecnbrlivinghouse", @"
                    CREATE TABLE ris_shareecnbrlivinghouse
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      intpart character varying(50),
                      fracpart character varying(50),
                      ecnbr_id bigint,
                      share_id bigint,
                      house_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_shareecnbrlivinghouse_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_sharecnbrlh_ecnbr FOREIGN KEY (ecnbr_id)
                          REFERENCES ris_ecnbr (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_sharecnbrlh_house FOREIGN KEY (house_id)
                          REFERENCES ris_house (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_sharecnbrlh_share FOREIGN KEY (share_id)
                          REFERENCES ris_share (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_shareecnbrl_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_sharecnbrlh_ecnbr", @"
                        CREATE INDEX ind_ris_sharecnbrlh_ecnbr
                        ON ris_shareecnbrlivinghouse
                        USING btree
                        (ecnbr_id);
                    "),
                    new IndexProxy("ind_ris_sharecnbrlh_house", @"
                        CREATE INDEX ind_ris_sharecnbrlh_house
                        ON ris_shareecnbrlivinghouse
                        USING btree
                        (house_id);
                    "),
                    new IndexProxy("ind_ris_sharecnbrlh_share", @"
                        CREATE INDEX ind_ris_sharecnbrlh_share
                        ON ris_shareecnbrlivinghouse
                        USING btree
                        (share_id);
                    ")
                }),

            #endregion

            #region RIS_SHAREECNBRLIVINGROOM

            new TableProxy("ris_shareecnbrlivingroom", @"
                    CREATE TABLE ris_shareecnbrlivingroom
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      intpart character varying(50),
                      fracpart character varying(50),
                      ecnbr_id bigint,
                      share_id bigint,
                      livingroom_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_shareecnbrlivingroom_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_sharecnbrlr_ecnbr FOREIGN KEY (ecnbr_id)
                          REFERENCES ris_ecnbr (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_sharecnbrlr_lr FOREIGN KEY (livingroom_id)
                          REFERENCES ris_livingroom (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_sharecnbrlr_share FOREIGN KEY (share_id)
                          REFERENCES ris_share (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_shareecnbrl_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_sharecnbrlr_ecnbr", @"
                        CREATE INDEX ind_ris_sharecnbrlr_ecnbr
                        ON ris_shareecnbrlivingroom
                        USING btree
                        (ecnbr_id);
                    "),
                    new IndexProxy("ind_ris_sharecnbrlr_lr", @"
                        CREATE INDEX ind_ris_sharecnbrlr_lr
                        ON ris_shareecnbrlivingroom
                        USING btree
                        (livingroom_id);
                    "),
                    new IndexProxy("ind_ris_sharecnbrlr_share", @"
                        CREATE INDEX ind_ris_sharecnbrlr_share
                        ON ris_shareecnbrlivingroom
                        USING btree
                        (share_id);
                    ")
                }),

            #endregion

            #region RIS_SHAREECNBRNONRESPREM

            new TableProxy("ris_shareecnbrnonresprem", @"
                    CREATE TABLE ris_shareecnbrnonresprem
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      intpart character varying(50),
                      fracpart character varying(50),
                      ecnbr_id bigint,
                      share_id bigint,
                      nonresidentialpremises_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_shareecnbrnonresprem_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_sharecnbrnrespr_ecnbr FOREIGN KEY (ecnbr_id)
                          REFERENCES ris_ecnbr (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_sharecnbrnrespr_prem FOREIGN KEY (nonresidentialpremises_id)
                          REFERENCES ris_nonresidentialpremises (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_sharecnbrnrespr_share FOREIGN KEY (share_id)
                          REFERENCES ris_share (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_shareecnbrn_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_sharecnbrnrespr_ecnbr", @"
                        CREATE INDEX ind_ris_sharecnbrnrespr_ecnbr
                        ON ris_shareecnbrnonresprem
                        USING btree
                        (ecnbr_id);
                    "),
                    new IndexProxy("ind_ris_sharecnbrnrespr_prem", @"
                        CREATE INDEX ind_ris_sharecnbrnrespr_prem
                        ON ris_shareecnbrnonresprem
                        USING btree
                        (nonresidentialpremises_id);
                    "),
                    new IndexProxy("ind_ris_sharecnbrnrespr_share", @"
                        CREATE INDEX ind_ris_sharecnbrnrespr_share
                        ON ris_shareecnbrnonresprem
                        USING btree
                        (share_id);
                    ")
                }),

            #endregion

            #region RIS_SHAREECNBRRESIDENTPREM

            new TableProxy("ris_shareecnbrresidentprem", @"
                    CREATE TABLE ris_shareecnbrresidentprem
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      intpart character varying(50),
                      fracpart character varying(50),
                      ecnbr_id bigint,
                      share_id bigint,
                      residentprem_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_shareecnbrresidentprem_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_sharecnbrrespr_ecnbr FOREIGN KEY (ecnbr_id)
                          REFERENCES ris_ecnbr (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_sharecnbrrespr_rp FOREIGN KEY (residentprem_id)
                          REFERENCES ris_residentialpremises (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_sharecnbrrespr_share FOREIGN KEY (share_id)
                          REFERENCES ris_share (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_shareecnbrr_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_sharecnbrrespr_ecnbr", @"
                        CREATE INDEX ind_ris_sharecnbrrespr_ecnbr
                        ON ris_shareecnbrresidentprem
                        USING btree
                        (ecnbr_id);
                    "),
                    new IndexProxy("ind_ris_sharecnbrrespr_rp", @"
                        CREATE INDEX ind_ris_sharecnbrrespr_rp
                        ON ris_shareecnbrresidentprem
                        USING btree
                        (residentprem_id);
                    "),
                    new IndexProxy("ind_ris_sharecnbrrespr_share", @"
                        CREATE INDEX ind_ris_sharecnbrrespr_share
                        ON ris_shareecnbrresidentprem
                        USING btree
                        (share_id);
                    ")
                }),

            #endregion

            #region RIS_SHAREIND

            new TableProxy("ris_shareind", @"
                    CREATE TABLE ris_shareind
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      share_id bigint,
                      ind_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_shareind_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_shareind_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_shareind_ind FOREIGN KEY (ind_id)
                          REFERENCES ris_ind (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_shareind_share FOREIGN KEY (share_id)
                          REFERENCES ris_share (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_shareind_ind", @"
                        CREATE INDEX ind_ris_shareind_ind
                        ON ris_shareind
                        USING btree
                        (ind_id);
                    "),
                    new IndexProxy("ind_ris_shareind_share", @"
                        CREATE INDEX ind_ris_shareind_share
                        ON ris_shareind
                        USING btree
                        (share_id);
                    ")
                }),

            #endregion

            #region RIS_TRUSTDOCATTACHMENT

            new TableProxy("ris_trustdocattachment", @"
                    CREATE TABLE ris_trustdocattachment
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      publicpropertycontract_id bigint,
                      attachment_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      addagreementpayment_id bigint,
                      CONSTRAINT ris_trustdocattachment_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_trustdocatt_attach FOREIGN KEY (attachment_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_trustdocatt_pubcontract FOREIGN KEY (publicpropertycontract_id)
                          REFERENCES ris_publicpropertycontract (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_trustdocattachment_addagreementpayment FOREIGN KEY (addagreementpayment_id)
                          REFERENCES ris_addagreementpayment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_trustdocattachment_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_trustdocatt_attach", @"                     
                        CREATE INDEX ind_ris_trustdocatt_attach
                        ON ris_trustdocattachment
                        USING btree
                        (attachment_id);
                    "),
                    new IndexProxy("ind_ris_trustdocatt_pubcontract", @"
                        CREATE INDEX ind_ris_trustdocatt_pubcontract
                        ON ris_trustdocattachment
                        USING btree
                        (publicpropertycontract_id);
                    "),
                    new IndexProxy("ind_ris_trustdocattachment_addagreementpayment", @"
                        CREATE INDEX ind_ris_trustdocattachment_addagreementpayment
                        ON ris_trustdocattachment
                        USING btree
                        (addagreementpayment_id);
                    "),
                    new IndexProxy("ind_ris_trustdocattachment_contragent", @"
                        CREATE INDEX ind_ris_trustdocattachment_contragent
                        ON ris_trustdocattachment
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region RIS_VOTEINITIATORS

            new TableProxy("ris_voteinitiators", @"
                    CREATE TABLE ris_voteinitiators
                    (
                        id serial NOT NULL,
                        object_version bigint NOT NULL,
                        object_create_date timestamp without time zone NOT NULL,
                        object_edit_date timestamp without time zone NOT NULL,
                        external_id bigint NOT NULL,
                        external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                        guid character varying(50),
                        ind_id bigint,
                        org_id bigint,
                        votingprotocol_id bigint,
                        operation smallint NOT NULL DEFAULT 0,
                        gi_contragent_id bigint,
                        CONSTRAINT ris_voteinitiators_pkey PRIMARY KEY (id),
                        CONSTRAINT fk_ris_voteinits_ind FOREIGN KEY (ind_id)
                            REFERENCES ris_ind (id) MATCH SIMPLE
                            ON UPDATE NO ACTION ON DELETE NO ACTION,
                        CONSTRAINT fk_ris_voteinits_org FOREIGN KEY (org_id)
                            REFERENCES gi_contragent (id) MATCH SIMPLE
                            ON UPDATE NO ACTION ON DELETE NO ACTION,
                        CONSTRAINT fk_ris_voteinits_votingprot FOREIGN KEY (votingprotocol_id)
                            REFERENCES ris_votingprotocol (id) MATCH SIMPLE
                            ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_voteinits_ind", @"
                        CREATE INDEX ind_ris_voteinits_ind
                        ON ris_voteinitiators
                        USING btree
                        (ind_id);
                    "),
                    new IndexProxy("ind_ris_voteinits_org", @"
                        CREATE INDEX ind_ris_voteinits_org
                        ON ris_voteinitiators
                        USING btree
                        (org_id);
                    "),
                    new IndexProxy("ind_ris_voteinits_votingprot", @"
                        CREATE INDEX ind_ris_voteinits_votingprot
                        ON ris_voteinitiators
                        USING btree
                        (votingprotocol_id);

                    ")
                }),

            #endregion

            #region RIS_VOTINGPROTOCOL_ATTACHMENT

            new TableProxy("ris_votingprotocol_attachment", @"
                    CREATE TABLE ris_votingprotocol_attachment
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      attachment_id bigint,
                      votingprotocol_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_votingprotocol_attachment_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_votingprot_attach FOREIGN KEY (attachment_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_votingprot_votingprot FOREIGN KEY (votingprotocol_id)
                          REFERENCES ris_votingprotocol (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_votprot_attach_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_votingprot_attach", @"
                        CREATE INDEX ind_ris_votingprot_attach
                        ON ris_votingprotocol_attachment
                        USING btree
                        (attachment_id);
                    "),
                    new IndexProxy("ind_ris_votingprot_votingprot", @"
                        CREATE INDEX ind_ris_votingprot_votingprot
                        ON ris_votingprotocol_attachment
                        USING btree
                        (votingprotocol_id);
                    "),
                    new IndexProxy("ind_votprot_attach_contragent", @"
                        CREATE INDEX ind_votprot_attach_contragent
                        ON ris_votingprotocol_attachment
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region SUP_RES_CONTRACT_ATTACHMENT

            new TableProxy("sup_res_contract_attachment", @"
                    CREATE TABLE sup_res_contract_attachment
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      contract_id bigint,
                      attachment_id bigint,
                      CONSTRAINT sup_res_contract_attachment_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_sr_contract_attach_attach FOREIGN KEY (attachment_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_sr_contract_attach_contract FOREIGN KEY (contract_id)
                          REFERENCES supply_resource_contract (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_sup_res_contract_attachment_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_sr_contract_attach_attach", @"
                        CREATE INDEX ind_sr_contract_attach_attach
                        ON sup_res_contract_attachment
                        USING btree
                        (attachment_id);
                    "),
                    new IndexProxy("ind_sr_contract_attach_contract", @"
                        CREATE INDEX ind_sr_contract_attach_contract
                        ON sup_res_contract_attachment
                        USING btree
                        (contract_id);
                    "),
                    new IndexProxy("ind_sup_res_contract_attachment_contragent", @"
                        CREATE INDEX ind_sup_res_contract_attachment_contragent
                        ON sup_res_contract_attachment
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region SUP_RES_CONTRACT_SERVICE_RESOURCE

            new TableProxy("sup_res_contract_service_resource", @"
                    CREATE TABLE sup_res_contract_service_resource
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      contract_id bigint,
                      service_type_code character varying(255),
                      service_type_guid character varying(255),
                      municipal_resource_code character varying(255),
                      municipal_resource_guid character varying(255),
                      start_supply_date timestamp without time zone,
                      end_supply_date timestamp without time zone,
                      CONSTRAINT sup_res_contract_service_resource_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_sr_contract_serv_res_contract FOREIGN KEY (contract_id)
                          REFERENCES supply_resource_contract (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_sup_res_contract_service_resource_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_sr_contract_serv_res_contract", @"
                        CREATE INDEX ind_sr_contract_serv_res_contract
                        ON sup_res_contract_service_resource
                        USING btree
                        (contract_id);
                    "),
                    new IndexProxy("ind_sup_res_contract_service_resource_contragent", @"
                        CREATE INDEX ind_sup_res_contract_service_resource_contragent
                        ON sup_res_contract_service_resource
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region SUP_RES_CONTRACT_SUBJECT

            new TableProxy("sup_res_contract_subject", @"
                    CREATE TABLE sup_res_contract_subject
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      contract_id bigint,
                      service_type_code character varying(255),
                      service_type_guid character varying(255),
                      municipal_resource_code character varying(255),
                      municipal_resource_guid character varying(255),
                      heating_system_type smallint,
                      connection_scheme_type smallint,
                      start_supply_date timestamp without time zone,
                      end_supply_date timestamp without time zone,
                      planned_volume numeric(18,5),
                      unit character varying(255),
                      feeding_mode character varying(255),
                      CONSTRAINT sup_res_contract_subject_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_sr_contract_subj_contract FOREIGN KEY (contract_id)
                          REFERENCES supply_resource_contract (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_sup_res_contract_subject_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_sr_contract_subj_contract", @"
                        CREATE INDEX ind_sr_contract_subj_contract
                        ON sup_res_contract_subject
                        USING btree
                        (contract_id);
                    "),
                    new IndexProxy("ind_sup_res_contract_subject_contragent", @"
                        CREATE INDEX ind_sup_res_contract_subject_contragent
                        ON sup_res_contract_subject
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region SUP_RES_CONTRACT_SUBJECT_OTHER_QUALITY

            new TableProxy("sup_res_contract_subject_other_quality", @"
                    CREATE TABLE sup_res_contract_subject_other_quality
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      contract_subject_id bigint,
                      indicator_name character varying(255),
                      ""number"" numeric(18,5),
                      okei character varying(255),
                      CONSTRAINT sup_res_contract_subject_other_quality_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_sr_contr_subj_q_cs FOREIGN KEY (contract_subject_id)
                          REFERENCES sup_res_contract_subject (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_sup_res_contract_subject_other_quality_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_sr_contr_subj_q_cs", @"
                        CREATE INDEX ind_sr_contr_subj_q_cs
                        ON sup_res_contract_subject_other_quality
                        USING btree
                        (contract_subject_id);

                    "),
                    new IndexProxy("ind_sup_res_contract_subject_other_quality_contragent", @"
                        CREATE INDEX ind_sup_res_contract_subject_other_quality_contragent
                        ON sup_res_contract_subject_other_quality
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region SUP_RES_CONTRACT_TEMPERATURE_CHART

            new TableProxy("sup_res_contract_temperature_chart", @"
                    CREATE TABLE sup_res_contract_temperature_chart
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      contract_id bigint,
                      outside_temperature integer,
                      flowline_temperature character varying(255),
                      oppositeline_temperature character varying(255),
                      CONSTRAINT sup_res_contract_temperature_chart_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_sr_contract_temp_contract FOREIGN KEY (contract_id)
                          REFERENCES supply_resource_contract (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_sup_res_contract_temperature_chart_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_sr_contract_temp_contract", @"
                        CREATE INDEX ind_sr_contract_temp_contract
                        ON sup_res_contract_temperature_chart
                        USING btree
                        (contract_id);
                    "),
                    new IndexProxy("ind_sup_res_contract_temperature_chart_contragent", @"
                        CREATE INDEX ind_sup_res_contract_temperature_chart_contragent
                        ON sup_res_contract_temperature_chart
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region RIS_RKI_ITEM

            new TableProxy("ris_rki_item", @"
                    CREATE TABLE ris_rki_item
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      name character varying(200),
                      basecode character varying(200),
                      baseguid character varying(200),
                      endmanagmentdate timestamp without time zone,
                      indefinitemanagement boolean,
                      contragent_id bigint,
                      municipalities boolean,
                      typecode character varying(200),
                      typeguid character varying(200),
                      waterintakecode character varying(200),
                      waterintakeguid character varying(200),
                      esubstationcode character varying(200),
                      esubstationguid character varying(200),
                      powerplantcode character varying(200),
                      powerplantguid character varying(200),
                      fuelcode character varying(200),
                      fuelguid character varying(200),
                      gasnetworkcode character varying(200),
                      gasnetworkguid character varying(200),
                      oktmocode character varying(200),
                      oktmoname character varying(200),
                      independentsource boolean,
                      deterioration numeric(18,5),
                      countaccidents integer,
                      addinfo character varying(2000),
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      fias_address_id bigint,
                      commissioning_year smallint,
                      CONSTRAINT ris_rki_item_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_rki_item_contragent FOREIGN KEY (contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_rki_item_contragent", @"
                        CREATE INDEX ind_ris_rki_item_contragent
                        ON ris_rki_item
                        USING btree
                        (contragent_id);
                    ")
                }),

            #endregion

            #region RIS_ATTACHMENTS_ENERGYEFFICIENCY

            new TableProxy("ris_attachments_energyefficiency", @"
                    CREATE TABLE ris_attachments_energyefficiency
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      rkiitem_id bigint,
                      attachment_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_attachments_energyefficiency_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_atchms_eneffi_attach FOREIGN KEY (attachment_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_atchms_eneffic_item FOREIGN KEY (rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_attac_energ_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_atchms_eneffi_attach", @"
                        CREATE INDEX ind_ris_atchms_eneffi_attach
                        ON ris_attachments_energyefficiency
                        USING btree
                        (attachment_id);
                    "),
                    new IndexProxy("ind_ris_atchms_eneffic_item", @"
                        CREATE INDEX ind_ris_atchms_eneffic_item
                        ON ris_attachments_energyefficiency
                        USING btree
                        (rkiitem_id);
                    ")
                }),

            #endregion

            #region RIS_NET_PIECES

            new TableProxy("ris_net_pieces", @"
                    CREATE TABLE ris_net_pieces
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      rkiitem_id bigint,
                      name character varying(200),
                      diameter numeric(18,5),
                      length numeric(18,5),
                      needreplaced numeric(18,5),
                      wearout numeric(18,5),
                      pressurecode character varying(200),
                      pressureguid character varying(200),
                      pressurename character varying(200),
                      voltagecode character varying(200),
                      voltageguid character varying(200),
                      voltagename character varying(200),
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_net_pieces_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_net_pieces_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_net_pieces_item FOREIGN KEY (rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_net_pieces_item", @"
                        CREATE INDEX ind_ris_net_pieces_item
                        ON ris_net_pieces
                        USING btree
                        (rkiitem_id);
                    ")
                }),

            #endregion

            #region RIS_RESOURCE

            new TableProxy("ris_resource", @"
                    CREATE TABLE ris_resource
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      rkiitem_id bigint,
                      municipalresourcecode character varying(200),
                      municipalresourceguid character varying(200),
                      municipalresourcename character varying(200),
                      totalload numeric(18,5),
                      industrialload numeric(18,5),
                      socialload numeric(18,5),
                      populationload numeric(18,5),
                      setpower numeric(18,5),
                      sitingpower numeric(18,5),
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_resource_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_resource_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_resource_item FOREIGN KEY (rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_resource_item", @"
                        CREATE INDEX ind_ris_resource_item
                        ON ris_resource
                        USING btree
                        (rkiitem_id);
                    ")
                }),

            #endregion

            #region RIS_RKI_ATTACHMENT

            new TableProxy("ris_rki_attachment", @"
                    CREATE TABLE ris_rki_attachment
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      rkiitem_id bigint,
                      attachment_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_rki_attachment_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_rki_attachm_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_rki_attachment_attach FOREIGN KEY (attachment_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_rki_attachment_item FOREIGN KEY (rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_rki_attachment_attach", @"
                        CREATE INDEX ind_ris_rki_attachment_attach
                        ON ris_rki_attachment
                        USING btree
                        (attachment_id);
                    "),
                    new IndexProxy("ind_ris_rki_attachment_item", @"
                        CREATE INDEX ind_ris_rki_attachment_item
                        ON ris_rki_attachment
                        USING btree
                        (rkiitem_id);
                    ")
                }),

            #endregion

            #region RIS_RKI_COMMUNAL_SERVICE

            new TableProxy("ris_rki_communal_service", @"
                    CREATE TABLE ris_rki_communal_service
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      rkiitem_id bigint,
                      service_code character varying(20),
                      service_guid character varying(36),
                      service_name character varying(1200),
                      CONSTRAINT ris_rki_communal_service_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_rki_communal_service_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_rki_communal_service_rkiitem_id FOREIGN KEY (rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_rki_communal_service_contragent", @"
                        CREATE INDEX ind_ris_rki_communal_service_contragent
                        ON ris_rki_communal_service
                        USING btree
                        (gi_contragent_id);
                    "),
                    new IndexProxy("ind_ris_rki_communal_service_rkiitem_id", @"
                        CREATE INDEX ind_ris_rki_communal_service_rkiitem_id
                        ON ris_rki_communal_service
                        USING btree
                        (rkiitem_id);
                    ")
                }),

            #endregion

            #region RIS_RECEIVER_OKI

            new TableProxy("ris_receiver_oki", @"
                    CREATE TABLE ris_receiver_oki
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      rkiitem_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      receiver_rkiitem_id bigint,
                      CONSTRAINT ris_receiver_oki_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_rcvr_oki_item FOREIGN KEY (rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_receive_oki_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_receiver_oki_receiver_rkiitem_id FOREIGN KEY (receiver_rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_rcvr_oki_item", @"
                        CREATE INDEX ind_ris_rcvr_oki_item
                        ON ris_receiver_oki
                        USING btree
                        (rkiitem_id);
                    "),
                    new IndexProxy("ind_ris_receiver_oki_receiver_rkiitem_id", @"
                        CREATE INDEX ind_ris_receiver_oki_receiver_rkiitem_id
                        ON ris_receiver_oki
                        USING btree
                        (receiver_rkiitem_id);
                    ")
                }),

            #endregion

            #region ris_source_oki

            new TableProxy("ris_source_oki", @"
                    CREATE TABLE ris_source_oki
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      rkiitem_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      source_rkiitem_id bigint,
                      CONSTRAINT ris_source_oki_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_source_oki_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_source_oki_item FOREIGN KEY (rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_source_oki_source_rkiitem_id FOREIGN KEY (source_rkiitem_id)
                          REFERENCES ris_rki_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_source_oki_item", @"
                        CREATE INDEX ind_ris_source_oki_item
                        ON ris_source_oki
                        USING btree
                        (rkiitem_id);
                    "),
                    new IndexProxy("ind_ris_source_oki_source_rkiitem_id", @"
                        CREATE INDEX ind_ris_source_oki_source_rkiitem_id
                        ON ris_source_oki
                        USING btree
                        (source_rkiitem_id);
                    ")
                }),

            #endregion

            #region RIS_TRANSPORTATION_RESOURCES

            new TableProxy("ris_transportation_resources", @"
                    CREATE TABLE ris_transportation_resources
                    (
                        id serial NOT NULL,
                        object_version bigint NOT NULL,
                        object_create_date timestamp without time zone NOT NULL,
                        object_edit_date timestamp without time zone NOT NULL,
                        external_id bigint NOT NULL,
                        external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                        guid character varying(50),
                        rkiitem_id bigint,
                        municipalresourcecode character varying(200),
                        municipalresourceguid character varying(200),
                        municipalresourcename character varying(200),
                        totalload numeric(18,5),
                        industrialload numeric(18,5),
                        socialload numeric(18,5),
                        populationload numeric(18,5),
                        volumelosses numeric(18,5),
                        coolantcode character varying(200),
                        coolantguid character varying(200),
                        coolantname character varying(200),
                        operation smallint NOT NULL DEFAULT 0,
                        gi_contragent_id bigint,
                        CONSTRAINT ris_transportation_resources_pkey PRIMARY KEY (id),
                        CONSTRAINT fk_ris_trans_resou_ctrg FOREIGN KEY (gi_contragent_id)
                            REFERENCES gi_contragent (id) MATCH SIMPLE
                            ON UPDATE NO ACTION ON DELETE NO ACTION,
                        CONSTRAINT fk_ris_transport_ress_item FOREIGN KEY (rkiitem_id)
                            REFERENCES ris_rki_item (id) MATCH SIMPLE
                            ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_transport_ress_item", @"
                        CREATE INDEX ind_ris_transport_ress_item
                        ON ris_transportation_resources
                        USING btree
                        (rkiitem_id);
                    ")
                }),

            #endregion

            #region RIS_ADDITIONAL_SERVICE

            new TableProxy("ris_additional_service", @"
                    CREATE TABLE ris_additional_service
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      additional_service_type_name character varying(100) NOT NULL,
                      okei character varying(3),
                      string_dimension_unit character varying(100),
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_additional_service_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_addit_servi_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                "),

            #endregion

            #region RIS_MUNICIPAL_SERVICE

            new TableProxy("ris_municipal_service", @"
                    CREATE TABLE ris_municipal_service
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      municipal_service_ref_code character varying(20) NOT NULL,
                      municipal_service_ref_guid character varying(40) NOT NULL,
                      general_needs boolean,
                      main_municipal_service_name character varying(100) NOT NULL,
                      municipal_resource_ref_code character varying(20) NOT NULL,
                      municipal_resource_ref_guid character varying(40) NOT NULL,
                      sort_order character varying(3),
                      sort_order_not_defined boolean,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_municipal_service_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_munic_servi_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                "),

            #endregion

            #region RIS_ORGANIZATION_WORK

            new TableProxy("ris_organization_work", @"
                    CREATE TABLE ris_organization_work
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      name character varying(100),
                      service_type_code character varying(20),
                      service_type_guid character varying(40),
                      required_services bytea,
                      okei character varying(3),
                      string_dimension_unit character varying(100),
                      CONSTRAINT ris_organization_work_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_organization_work_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_organization_work_contragent", @"
                        CREATE INDEX ind_ris_organization_work_contragent
                        ON ris_organization_work
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region RIS_SUBSIDARY

            new TableProxy("ris_subsidary", @"
                    CREATE TABLE ris_subsidary
                    (
                      id serial NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      fullname character varying(300),
                      shortname character varying(300),
                      ogrn character varying(250),
                      inn character varying(20),
                      kpp character varying(20),
                      okopf character varying(50),
                      address character varying(500),
                      fiashouseguid character varying(1000),
                      activityenddate timestamp without time zone,
                      sourcename character varying(255),
                      gi_contragent_id bigint,
                      sourcedate date,
                      parent_id bigint,
                      CONSTRAINT ris_subsidary_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_parent_contragent_id FOREIGN KEY (parent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_parent_contragent_id", @"
                        CREATE INDEX ind_ris_parent_contragent_id
                        ON ris_subsidary
                        USING btree
                        (parent_id);
                    ")
                }),

            #endregion

            #region RIS_NOTIFORDEREXECUT

            new TableProxy("ris_notiforderexecut", @"
                    CREATE TABLE ris_notiforderexecut
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      supplier_id character varying(25),
                      supplier_name character varying(160),
                      recipient_inn character varying(12),
                      recipient_kpp character varying(9),
                      bank_name character varying(160),
                      recipient_bank_bik character varying(9),
                      recipient_bank_corracc character varying(120),
                      recipient_account character varying(20),
                      recipient_name character varying(160),
                      order_id character varying(32),
                      account_number character varying(30),
                      order_num character varying(9),
                      order_date timestamp without time zone,
                      amount numeric(18,5),
                      payment_purpose character varying(210),
                      comment character varying(210),
                      payment_doc_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      inn character varying(255),
                      recipient_entpr_surname character varying(255),
                      recipient_entpr_firstname character varying(255),
                      recipient_entpr_patronymic character varying(255),
                      recipient_legal_kpp character varying(255),
                      recipient_legal_name character varying(255),
                      payment_document_id character varying(255),
                      payment_document_number character varying(255),
                      year smallint,
                      month integer,
                      unified_account_number character varying(255),
                      fias_house_guid character varying(255),
                      apartment character varying(255),
                      placement character varying(255),
                      consumer_surname character varying(255),
                      consumer_first_name character varying(255),
                      consumer_patronymic character varying(255),
                      consumer_inn character varying(255),
                      service_id character varying(255),
                      ris_paym_doc_id bigint,
                      recipient_entpr_fio character varying(255),
                      non_living_apartment character varying(255),
                      CONSTRAINT ris_notiforderexecut_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_notifordere_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_notiforderex_paym_doc FOREIGN KEY (ris_paym_doc_id)
                          REFERENCES ris_payment_doc (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_notiforderex_paym_doc", @"
                        CREATE INDEX ind_ris_notiforderex_paym_doc
                        ON ris_notiforderexecut
                        USING btree
                        (ris_paym_doc_id);
                    ")
                }),

            #endregion

            #region RIS_ACKNOWLEDGMENT

            new TableProxy("ris_acknowledgment", @"
                    CREATE TABLE ris_acknowledgment
                    (
                      id serial NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      orderid character varying(32) NOT NULL,
                      paymentdocumentnumber character varying(18) NOT NULL,
                      hstype character varying(40),
                      mstype character varying(40),
                      astype character varying(40),
                      amount numeric(18,5),
                      gi_contragent_id bigint,
                      notify_id bigint,
                      pay_doc_id bigint,
                      CONSTRAINT ris_acknowledgment_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_fk_notify_id FOREIGN KEY (notify_id)
                          REFERENCES ris_notiforderexecut (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_fk_pay_doc_id FOREIGN KEY (pay_doc_id)
                          REFERENCES ris_payment_doc (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_acknowledgm_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_fk_notify_id", @"
                        CREATE INDEX ind_fk_notify_id
                        ON ris_acknowledgment
                        USING btree
                        (notify_id);
                    "),
                    new IndexProxy("ind_fk_pay_doc_id", @"
                        CREATE INDEX ind_fk_pay_doc_id
                        ON ris_acknowledgment
                        USING btree
                        (pay_doc_id);
                    ")
                }),

            #endregion

            #region RIS_WORKLIST

            new TableProxy("ris_worklist", @"
                    CREATE TABLE ris_worklist
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      contract_id bigint,
                      house_id bigint,
                      month_from integer,
                      year_from smallint,
                      month_to integer,
                      year_to smallint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      attachment_id bigint,
                      CONSTRAINT ris_worklist_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_worklist_attachment FOREIGN KEY (attachment_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_worklist_contract FOREIGN KEY (contract_id)
                          REFERENCES ris_contract (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_worklist_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_worklist_house FOREIGN KEY (house_id)
                          REFERENCES ris_house (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_worklist_attachment", @"
                        CREATE INDEX ind_ris_worklist_attachment
                        ON ris_worklist
                        USING btree
                        (attachment_id);
                    "),
                    new IndexProxy("ind_ris_worklist_contract", @"
                        CREATE INDEX ind_ris_worklist_contract
                        ON ris_worklist
                        USING btree
                        (contract_id);
                    "),
                    new IndexProxy("ind_ris_worklist_house", @"
                        CREATE INDEX ind_ris_worklist_house
                        ON ris_worklist
                        USING btree
                        (house_id);
                    ")
                }),

            #endregion

            #region RIS_WORKPLAN

            new TableProxy("ris_workplan", @"
                    CREATE TABLE ris_workplan
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      worklist_id bigint,
                      year smallint,
                      CONSTRAINT ris_workplan_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_fk_worklist_id FOREIGN KEY (worklist_id)
                          REFERENCES ris_worklist (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_workplan_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_fk_worklist_id", @"
                        CREATE INDEX ind_fk_worklist_id
                        ON ris_workplan
                        USING btree
                        (worklist_id);
                    "),
                    new IndexProxy("ind_ris_workplan_contragent", @"
                        CREATE INDEX ind_ris_workplan_contragent
                        ON ris_workplan
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region RIS_WORKLIST_ITEM

            new TableProxy("ris_worklist_item", @"
                    CREATE TABLE ris_worklist_item
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      worklist_id bigint,
                      total_cost numeric(18,5),
                      index smallint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      work_item_code character varying(10),
                      work_item_guid character varying(50),
                      CONSTRAINT ris_worklist_item_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_item_worklist FOREIGN KEY (worklist_id)
                          REFERENCES ris_worklist (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_workli_item_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_item_worklist", @"
                        CREATE INDEX ind_ris_item_worklist
                        ON ris_worklist_item
                        USING btree
                        (worklist_id);
                    ")
                }),
            
            #endregion

            #region RIS_WORKPLAN_ITEM

            new TableProxy("ris_workplan_item", @"
                   CREATE TABLE ris_workplan_item
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      workplan_id bigint,
                      worklist_item_id bigint,
                      year smallint,
                      month integer,
                      date_work timestamp without time zone,
                      count_work integer,
                      CONSTRAINT ris_workplan_item_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_fk_workplan_id FOREIGN KEY (workplan_id)
                          REFERENCES ris_workplan (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_worklist_item FOREIGN KEY (worklist_item_id)
                          REFERENCES ris_workplan (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_workplan_item_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_fk_workplan_id", @"
                        CREATE INDEX ind_fk_workplan_id
                        ON ris_workplan_item
                        USING btree
                        (workplan_id);
                    "),
                    new IndexProxy("ind_ris_worklist_item", @"
                        CREATE INDEX ind_ris_worklist_item
                        ON ris_workplan_item
                        USING btree
                        (worklist_item_id);
                    "),
                    new IndexProxy("ind_ris_workplan_item_contragent", @"
                        CREATE INDEX ind_ris_workplan_item_contragent
                        ON ris_workplan_item
                        USING btree
                        (gi_contragent_id);
                    ")
                }),

            #endregion

            #region RIS_COMPLETED_WORK

            new TableProxy("ris_completed_work", @"
                    CREATE TABLE ris_completed_work
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      work_plan_item_id bigint,
                      object_photo_id bigint,
                      act_file_id bigint,
                      act_date timestamp without time zone,
                      act_number character varying(255),
                      CONSTRAINT ris_completed_work_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_completed_work_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_riscompdwrk_act_file_id FOREIGN KEY (act_file_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_riscompdwrk_obj_photo_id FOREIGN KEY (object_photo_id)
                          REFERENCES gi_attachment (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_riscompdwrk_wrk_pln_itm_id FOREIGN KEY (work_plan_item_id)
                          REFERENCES ris_workplan_item (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_completed_work_contragent", @"
                        CREATE INDEX ind_ris_completed_work_contragent
                        ON ris_completed_work
                        USING btree
                        (gi_contragent_id);
                    "),
                    new IndexProxy("ind_riscompdwrk_act_file_id", @"
                        CREATE INDEX ind_riscompdwrk_act_file_id
                        ON ris_completed_work
                        USING btree
                        (act_file_id);
                    "),
                    new IndexProxy("ind_riscompdwrk_obj_photo_id", @"
                        CREATE INDEX ind_riscompdwrk_obj_photo_id
                        ON ris_completed_work
                        USING btree
                        (object_photo_id);
                    "),
                    new IndexProxy("ind_riscompdwrk_wrk_pln_itm_id", @"
                        CREATE INDEX ind_riscompdwrk_wrk_pln_itm_id
                        ON ris_completed_work
                        USING btree
                        (work_plan_item_id);
                    ")
                }),

            #endregion

            #region RIS_HOUSESERVICE

            new TableProxy("ris_houseservice", @"
                    CREATE TABLE ris_houseservice
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      guid character varying(50),
                      servicetype_code character varying(200),
                      servicetype_guid character varying(200),
                      startdate timestamp without time zone,
                      enddate timestamp without time zone,
                      baseservise_agreement_id bigint,
                      accountingtype_code character varying(200),
                      accountingtype_guid character varying(200),
                      house_id bigint,
                      operation smallint NOT NULL DEFAULT 0,
                      gi_contragent_id bigint,
                      CONSTRAINT ris_houseservice_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_houseservic_ctrg FOREIGN KEY (gi_contragent_id)
                          REFERENCES gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                "),

            #endregion

            #region RIS_CONFIG_PARAMETER

            new TableProxy("RIS_CONFIG_PARAMETER", @"
                    CREATE TABLE RIS_CONFIG_PARAMETER
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      KEY character varying(500) not null,
                      VALUE text
                    )
                "),

            #endregion

            #region RIS_SETTINGS

            new TableProxy("ris_settings", @"
                    CREATE TABLE ris_settings
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      code character varying(50),
                      name character varying(100),
                      value character varying(100),
                      CONSTRAINT ris_settings_pkey PRIMARY KEY (id)
                    )
                "),

            #endregion

            #region RIS_CAPITAL_REPAIR_DEBT

            new TableProxy("ris_capital_repair_debt", @"
                    CREATE TABLE public.ris_capital_repair_debt
                    (
                      id serial NOT NULL,
                      object_version bigint NOT NULL,
                      object_create_date timestamp without time zone NOT NULL,
                      object_edit_date timestamp without time zone NOT NULL,
                      operation smallint NOT NULL DEFAULT 0,
                      external_id bigint NOT NULL,
                      external_system_name character varying(50) NOT NULL DEFAULT 'gkh'::character varying,
                      gi_contragent_id bigint,
                      guid character varying(50),
                      payment_doc_id bigint,
                      month numeric,
                      year numeric,
                      total_payable numeric,
                      CONSTRAINT ris_capital_repair_debt_pkey PRIMARY KEY (id),
                      CONSTRAINT fk_ris_capital_repair_debt_contragent FOREIGN KEY (gi_contragent_id)
                          REFERENCES public.gi_contragent (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION,
                      CONSTRAINT fk_ris_payment_doc_id FOREIGN KEY (payment_doc_id)
                          REFERENCES public.ris_payment_doc (id) MATCH SIMPLE
                          ON UPDATE NO ACTION ON DELETE NO ACTION
                    )
                ",
                new List<IndexProxy>
                {
                    new IndexProxy("ind_ris_capital_repair_debt_contragent", @"
                        CREATE INDEX ind_ris_capital_repair_debt_contragent
                        ON public.ris_capital_repair_debt
                        USING btree
                        (gi_contragent_id);
                    "),
                    new IndexProxy("ind_ris_payment_doc_id", @"
                        CREATE INDEX ind_ris_payment_doc_id
                        ON public.ris_capital_repair_debt
                        USING btree
                        (payment_doc_id);
                    ")
                }),

            #endregion
        };

        private class TableProxy
        {
            public string Name { get; set; }
            public string CreationScript { get; set; }
            public List<IndexProxy> IndexList { get; set; }

            public TableProxy(string name, string creationScript, List<IndexProxy> indexList = null)
            {
                this.Name = name;
                this.CreationScript = creationScript;
                this.IndexList = indexList ?? new List<IndexProxy>();
            }
        }

        private class IndexProxy
        {
            public string Name { get; set; }
            public string CreationScript { get; set; }

            public IndexProxy(string name, string creationScript)
            {
                this.Name = name;
                this.CreationScript = creationScript;
            }
        }
    }
}