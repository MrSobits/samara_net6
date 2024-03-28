namespace Bars.GkhCr.Migrations._2023.Version_2023123108
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2023123108")]
    [MigrationDependsOn(typeof(Version_2023123107.UpdateSchema))]
    //Является Version_2018102300 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var query = @"

--SpecialObjectCr
CREATE TABLE public.CR_SPECIAL_OBJECT
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  state_id bigint,
  program_id bigint,
  reality_object_id bigint NOT NULL,
  date_end_builder date,
  date_start_work date,
  date_end_work date,
  date_stop_work_gji date,
  date_cancel_reg date,
  date_accept_gji date,
  date_accept_reg date,
  date_gji_reg date,
  sum_dev_psd numeric(19,5),
  sum_smr numeric(19,5),
  sum_smr_approved numeric(19,5),
  sum_tech_insp numeric(19,5),
  description character varying(500),
  external_id character varying(36),
  allow_reneg boolean NOT NULL DEFAULT false,
  gji_num character varying(300),
  program_num character varying(300),
  federal_num character varying(300),
  before_delete_program_id bigint,
  max_kpkr_amount numeric(18,5),
  fact_amount_spent numeric(18,5),
  fact_start_date date,
  fact_end_date date,
  warranty_end_date date,
  import_entity_id bigint,
  export_id bigserial NOT NULL,
  CONSTRAINT cr_special_object_pkey PRIMARY KEY (id),
  CONSTRAINT fk_special_before_del_prog FOREIGN KEY (before_delete_program_id)
      REFERENCES public.cr_dict_program (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_object_pcr FOREIGN KEY (program_id)
      REFERENCES public.cr_dict_program (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_object_ro FOREIGN KEY (reality_object_id)
      REFERENCES public.gkh_reality_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_object_state FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT cr_special_object_export_id_key UNIQUE (export_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_object
  OWNER TO bars;

CREATE UNIQUE INDEX cr_special_object_export_id_idx
  ON public.cr_special_object
  USING btree
  (export_id);

CREATE INDEX ind_special_before_del_prog
  ON public.cr_special_object
  USING btree
  (before_delete_program_id);

CREATE INDEX ind_cr_special_object_pcr
  ON public.cr_special_object
  USING btree
  (program_id);

CREATE INDEX ind_cr_special_object_ro
  ON public.cr_special_object
  USING btree
  (reality_object_id);

CREATE INDEX ind_cr_special_object_state
  ON public.cr_special_object
  USING btree
  (state_id);


--SpecialTypeWorkCr
CREATE TABLE public.cr_special_obj_type_work
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  object_id bigint NOT NULL,
  fin_source_id bigint,
  work_id bigint,
  has_psd boolean NOT NULL DEFAULT false,
  volume numeric(19,5),
  sum_mat numeric(19,5),
  sum numeric(19,5),
  date_start_work date,
  date_end_work date,
  volume_completion numeric(19,5),
  fact_sum numeric(19,5),
  fact_volume numeric(19,5),
  percent_completion numeric(19,5),
  cost_sum numeric(19,5),
  count_worker numeric(19,5),
  stage_work_cr_id bigint,
  external_id character varying(36),
  manufacturer_name character varying(2000),
  description character varying(2000),
  add_date_end date,
  is_active boolean NOT NULL DEFAULT true,
  is_dpkr_created boolean NOT NULL DEFAULT false,
  year_repair smallint,
  state_id bigint,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_type_work_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_type_work_sta FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_type_work_fs FOREIGN KEY (fin_source_id)
      REFERENCES public.cr_dict_fin_source (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_type_work_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_type_work_sw FOREIGN KEY (stage_work_cr_id)
      REFERENCES public.cr_dict_stage_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_type_work_work FOREIGN KEY (work_id)
      REFERENCES public.gkh_dict_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_type_work
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_type_work_sta
  ON public.cr_special_obj_type_work
  USING btree
  (state_id);

CREATE INDEX ind_cr_special_type_work_fs
  ON public.cr_special_obj_type_work
  USING btree
  (fin_source_id);

CREATE INDEX ind_cr_special_type_work_ocr
  ON public.cr_special_obj_type_work
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_type_work_sw
  ON public.cr_special_obj_type_work
  USING btree
  (stage_work_cr_id);

CREATE INDEX ind_cr_special_type_work_work
  ON public.cr_special_obj_type_work
  USING btree
  (work_id);


--SpecialPersonalAccount
CREATE TABLE public.cr_special_obj_pers_account
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  object_id bigint NOT NULL,
  type_fin_group integer NOT NULL DEFAULT 10,
  closed boolean NOT NULL DEFAULT false,
  account character varying(300),
  external_id character varying(36),
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_pers_account_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_pers_acc_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_pers_account
  OWNER TO bars;

CREATE INDEX ind_cr_special_pers_acc_ocr
  ON public.cr_special_obj_pers_account
  USING btree
  (object_id);


--SpecialAdditionalParameters
CREATE TABLE public.cr_special_obj_additional_params
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  object_id bigint,
  request_kts_date timestamp without time zone,
  tech_cond_kts_date timestamp without time zone,
  tech_cond_kts_recipient character varying(255),
  request_vodokanal_date timestamp without time zone,
  tech_cond_vodokanal_date timestamp without time zone,
  tech_cond_vodokanal_recipient character varying(255),
  entry_for_approval_date timestamp without time zone,
  approval_kts_date timestamp without time zone,
  approval_vodokanal_date timestamp without time zone,
  install_percent numeric,
  client_accept integer,
  client_accept_change_date timestamp without time zone,
  inspector_accept integer,
  inspector_accept_change_date timestamp without time zone,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_additional_params_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_additional_params_obj_id FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_additional_params
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_additional_params_obj_id
  ON public.cr_special_obj_additional_params
  USING btree
  (object_id);


--SpecialContractCr
CREATE TABLE public.cr_special_obj_contract
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  state_id bigint,
  object_id bigint NOT NULL,
  contragent_id bigint,
  fin_source_id bigint,
  file_id bigint,
  document_name character varying(300),
  date_from date,
  sum numeric(19,5),
  external_id character varying(36),
  document_num character varying(300),
  description character varying(2000),
  budget_subj numeric(19,5),
  owner_means numeric(19,5),
  fund_means numeric(19,5),
  budget_mo numeric(19,5),
  type_work_id bigint,
  type_contract_id bigint,
  date_start_work date,
  date_end_work date,
  used_in_export integer NOT NULL DEFAULT 20,
  import_entity_id bigint,
  customer_id bigint,
  CONSTRAINT cr_special_obj_contract_pkey PRIMARY KEY (id),
  CONSTRAINT fk_special_contract_type_contract FOREIGN KEY (type_contract_id)
      REFERENCES public.gkh_dict_multiitem (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_contract_contr FOREIGN KEY (contragent_id)
      REFERENCES public.gkh_contragent (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_contract_file FOREIGN KEY (file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_contract_fs FOREIGN KEY (fin_source_id)
      REFERENCES public.cr_dict_fin_source (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_contract_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_contract_state FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_contract_contragent_id FOREIGN KEY (customer_id)
      REFERENCES public.gkh_contragent (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_contract_tw FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_contract
  OWNER TO bars;

CREATE INDEX ind_special_contract_type_contract
  ON public.cr_special_obj_contract
  USING btree
  (type_contract_id);

CREATE INDEX ind_cr_special_contract_contr
  ON public.cr_special_obj_contract
  USING btree
  (contragent_id);

CREATE INDEX ind_cr_special_contract_file
  ON public.cr_special_obj_contract
  USING btree
  (file_id);

CREATE INDEX ind_cr_special_contract_fs
  ON public.cr_special_obj_contract
  USING btree
  (fin_source_id);

CREATE INDEX ind_cr_special_contract_ocr
  ON public.cr_special_obj_contract
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_contract_state
  ON public.cr_special_obj_contract
  USING btree
  (state_id);

CREATE INDEX ind_cr_special_obj_contract_contragent_id
  ON public.cr_special_obj_contract
  USING btree
  (customer_id);

CREATE INDEX ind_cr_special_obj_contract_tw
  ON public.cr_special_obj_contract
  USING btree
  (type_work_id);
  
  
--SpecialProtocolCr
CREATE TABLE public.cr_special_obj_protocol
(
  id  bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  state_id bigint,
  object_id bigint NOT NULL,
  contragent_id bigint,
  file_id bigint,
  document_name character varying(300),
  count_accept numeric(19,5),
  count_vote numeric(19,5),
  count_vote_general numeric(19,5),
  date_from date,
  grade_occupant integer,
  grade_client integer,
  external_id character varying(36),
  document_num character varying(300),
  description character varying(2000),
  sum_act_ver_of_costs numeric(19,5),
  type_work_id bigint,
  type_document_cr_id bigint,
  owner_name character varying(300),
  used_in_export integer NOT NULL DEFAULT 20,
  decision_oms boolean NOT NULL DEFAULT false,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_protocol_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_protocol_tw FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_protocol_contr FOREIGN KEY (contragent_id)
      REFERENCES public.gkh_contragent (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_protocol_file FOREIGN KEY (file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_protocol_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_protocol_state FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_special_type_document_cr_id FOREIGN KEY (type_document_cr_id)
      REFERENCES public.gkh_dict_multiitem (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_protocol
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_protocol_tw
  ON public.cr_special_obj_protocol
  USING btree
  (type_work_id);

CREATE INDEX ind_cr_special_protocol_contr
  ON public.cr_special_obj_protocol
  USING btree
  (contragent_id);

CREATE INDEX ind_cr_special_protocol_file
  ON public.cr_special_obj_protocol
  USING btree
  (file_id);

CREATE INDEX ind_cr_special_protocol_ocr
  ON public.cr_special_obj_protocol
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_protocol_state
  ON public.cr_special_obj_protocol
  USING btree
  (state_id);

CREATE INDEX ind_special_type_document_cr_id
  ON public.cr_special_obj_protocol
  USING btree
  (type_document_cr_id);


--SpecialDefectList
CREATE TABLE public.cr_special_obj_defect_list
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  state_id bigint,
  object_id bigint NOT NULL,
  work_id bigint,
  file_id bigint,
  document_name character varying(300),
  document_date date,
  external_id character varying(36),
  sum numeric(19,5),
  dl_vol numeric(19,5),
  cost_per_unit numeric(19,5),
  type_work_id bigint,
  type_defect_list integer,
  used_in_export integer NOT NULL DEFAULT 20,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_defect_list_pkey PRIMARY KEY (id),
  CONSTRAINT fk_special_cr_defect_list_file FOREIGN KEY (file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_defect_list_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_defect_list_state FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_defect_list_work FOREIGN KEY (work_id)
      REFERENCES public.gkh_dict_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_defect_list_tw FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_defect_list
  OWNER TO bars;

CREATE INDEX ind_cr_special_defect_list_file
  ON public.cr_special_obj_defect_list
  USING btree
  (file_id);

CREATE INDEX ind_cr_special_defect_list_ocr
  ON public.cr_special_obj_defect_list
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_defect_list_state
  ON public.cr_special_obj_defect_list
  USING btree
  (state_id);

CREATE INDEX ind_cr_special_defect_list_work
  ON public.cr_special_obj_defect_list
  USING btree
  (work_id);

CREATE INDEX ind_cr_special_obj_defect_list_tw
  ON public.cr_special_obj_defect_list
  USING btree
  (type_work_id);


--SpecialFinanceSourceResource
CREATE TABLE public.cr_special_obj_fin_source_res
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  object_id bigint NOT NULL,
  fin_source_id bigint,
  budget_mu numeric(19,5),
  budget_sub numeric(19,5),
  owner_res numeric(19,5),
  fund_res numeric(19,5),
  external_id character varying(36),
  budget_mu_income numeric(19,5),
  budget_sub_income numeric(19,5),
  fund_res_income numeric(19,5),
  year integer,
  type_work_id bigint,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_fin_source_res_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_fin_sou_tw_wrk FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_fin_source_res_fs FOREIGN KEY (fin_source_id)
      REFERENCES public.cr_dict_fin_source (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_fin_source_res_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_fin_source_res
  OWNER TO bars;

CREATE INDEX ind_cr_special_fin_sou_tw_wrk
  ON public.cr_special_obj_fin_source_res
  USING btree
  (type_work_id);

CREATE INDEX ind_cr_special_fin_source_res_fs
  ON public.cr_special_obj_fin_source_res
  USING btree
  (fin_source_id);

CREATE INDEX ind_cr_special_fin_source_res_ocr
  ON public.cr_special_obj_fin_source_res
  USING btree
  (object_id);


--SpecialProtocolCrTypeWork
CREATE TABLE public.cr_special_obj_protocol_tw
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  protocol_id bigint,
  type_work_id bigint,
  external_id character varying(36),
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_protocol_tw_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_protocol_tw_p FOREIGN KEY (protocol_id)
      REFERENCES public.cr_special_obj_protocol (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_protocol_tw_t FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_protocol_tw
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_protocol_tw_p
  ON public.cr_special_obj_protocol_tw
  USING btree
  (protocol_id);

CREATE INDEX ind_cr_special_obj_protocol_tw_t
  ON public.cr_special_obj_protocol_tw
  USING btree
  (type_work_id);


--SpecialDesignAssignment
CREATE TABLE public.cr_special_obj_design_assignment
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  object_id bigint,
  type_work_id bigint,
  file_id bigint,
  state_id bigint,
  document character varying(50),
  date timestamp without time zone,
  used_in_export integer NOT NULL DEFAULT 20,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_design_assignment_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_dessign_file FOREIGN KEY (file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_dessign_state FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_dessign FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_dessign_tw FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_design_assignment
  OWNER TO bars;

CREATE INDEX ind_cr_special_dessign_file
  ON public.cr_special_obj_design_assignment
  USING btree
  (file_id);

CREATE INDEX ind_cr_special_dessign_state
  ON public.cr_special_obj_design_assignment
  USING btree
  (state_id);

CREATE INDEX ind_cr_special_obj_dessign
  ON public.cr_special_obj_design_assignment
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_obj_dessign_tw
  ON public.cr_special_obj_design_assignment
  USING btree
  (type_work_id);


--SpecialQualification
CREATE TABLE public.cr_special_qualification
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  object_id bigint NOT NULL,
  builder_id bigint NOT NULL,
  sum numeric(19,5),
  external_id character varying(36),
  import_entity_id bigint,
  CONSTRAINT cr_special_qualification_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_qual_builder FOREIGN KEY (builder_id)
      REFERENCES public.gkh_builder (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_qual_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_qualification
  OWNER TO bars;

CREATE INDEX ind_cr_special_qual_builder
  ON public.cr_special_qualification
  USING btree
  (builder_id);

CREATE INDEX ind_cr_special_qual_ocr
  ON public.cr_special_qualification
  USING btree
  (object_id);


--SpecialDesignAssignmentTypeWorkCr
CREATE TABLE public.cr_special_obj_design_asgmnt_type_work
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  assignment_id bigint NOT NULL,
  type_work_id bigint NOT NULL,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_design_asgmnt_type_work_pkey PRIMARY KEY (id),
  CONSTRAINT fk_special_design_asgmnt_id FOREIGN KEY (assignment_id)
      REFERENCES public.cr_special_obj_design_assignment (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_special_design_asgmnt_type_work_id FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_design_asgmnt_type_work
  OWNER TO bars;

CREATE INDEX ind_special_design_asgmnt_id
  ON public.cr_special_obj_design_asgmnt_type_work
  USING btree
  (assignment_id);

CREATE INDEX ind_special_design_asgmnt_type_work_id
  ON public.cr_special_obj_design_asgmnt_type_work
  USING btree
  (type_work_id);


--SpecialVoiceMember
CREATE TABLE public.cr_special_voice_qual_member
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  qualification_id bigint NOT NULL,
  qual_member_id bigint NOT NULL,
  type_accept_qual integer NOT NULL DEFAULT 10,
  document_date date,
  external_id character varying(36),
  reason character varying(2000),
  import_entity_id bigint,
  CONSTRAINT cr_special_voice_qual_member_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_vqualmem_qual FOREIGN KEY (qualification_id)
      REFERENCES public.cr_special_qualification (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_vqualmem_qualmem FOREIGN KEY (qual_member_id)
      REFERENCES public.cr_dict_qual_member (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_voice_qual_member
  OWNER TO bars;

CREATE INDEX ind_cr_special_vqualmem_qual
  ON public.cr_special_voice_qual_member
  USING btree
  (qualification_id);

CREATE INDEX ind_cr_special_vqualmem_qualmem
  ON public.cr_special_voice_qual_member
  USING btree
  (qual_member_id);


--SpecialTypeWorkCrHistory
CREATE TABLE public.cr_special_obj_type_work_hist
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  type_work_id bigint NOT NULL,
  fin_source_id bigint,
  type_action smallint NOT NULL DEFAULT 10,
  type_reason smallint NOT NULL DEFAULT 0,
  volume numeric(19,5),
  sum numeric(19,5),
  year_repair smallint,
  new_year_repair smallint,
  user_name character varying(300),
  struct_el character varying(500),
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_type_work_hist_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_type_work_hist_fs FOREIGN KEY (fin_source_id)
      REFERENCES public.cr_dict_fin_source (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_type_work_hist_tw FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_type_work_hist
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_type_work_hist_fs
  ON public.cr_special_obj_type_work_hist
  USING btree
  (fin_source_id);

CREATE INDEX ind_cr_special_obj_type_work_hist_tw
  ON public.cr_special_obj_type_work_hist
  USING btree
  (type_work_id);


--SpecialMonitoringSmr
CREATE TABLE public.cr_special_obj_monitoring_cmp
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  object_id bigint,
  state_id bigint,
  external_id character varying(36),
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_monitoring_cmp_pkey PRIMARY KEY (id),
  CONSTRAINT fk_c_specialr_obj_mon_cmp_obj FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_mon_cmp_st FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT unq_monitoring_cr_special_object UNIQUE (object_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_monitoring_cmp
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_mon_cmp_obj
  ON public.cr_special_obj_monitoring_cmp
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_obj_mon_cmp_st
  ON public.cr_special_obj_monitoring_cmp
  USING btree
  (state_id);


--SpecialTypeWorkCrRemoval
CREATE TABLE public.cr_special_obj_type_work_removal
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  type_work_id bigint NOT NULL,
  type_reason smallint NOT NULL DEFAULT 0,
  file_doc_id bigint,
  num_doc character varying(100),
  date_doc timestamp without time zone,
  description character varying(2000),
  year_repair smallint,
  new_year_repair smallint,
  struct_el character varying(500),
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_type_work_removal_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_type_work_rem_tw FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_type_work_removal
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_type_work_rem_tw
  ON public.cr_special_obj_type_work_removal
  USING btree
  (type_work_id);


--SpecialPerformedWorkAct
CREATE TABLE public.cr_special_obj_perfomed_work_act
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  state_id bigint,
  object_id bigint NOT NULL,
  type_work_cr_id bigint,
  volume numeric(19,5),
  sum numeric(19,5),
  date_from date,
  external_id character varying(36),
  document_num character varying(300),
  cost_file_id bigint,
  doc_file_id bigint,
  addit_file_id bigint,
  used_in_export integer NOT NULL DEFAULT 20,
  import_entity_id bigint,
  representative_signed integer NOT NULL DEFAULT 20,
  representative_name character varying(255),
  exploitation_accepted integer NOT NULL DEFAULT 20,
  warranty_start_date timestamp without time zone,
  warranty_end_date timestamp without time zone,
  representative_surname character varying(255),
  representative_patronymic character varying(255),
  CONSTRAINT cr_special_obj_perfomed_work_act_pkey PRIMARY KEY (id),
  CONSTRAINT fk_special_addition_file FOREIGN KEY (addit_file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_act_cost_file_id FOREIGN KEY (cost_file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_per_work_ac_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_per_work_ac_st FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_per_work_ac_w FOREIGN KEY (type_work_cr_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_special_document_file FOREIGN KEY (doc_file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_perfomed_work_act
  OWNER TO bars;

CREATE INDEX ind_special_addition_file
  ON public.cr_special_obj_perfomed_work_act
  USING btree
  (addit_file_id);

CREATE INDEX ind_cr_special_act_cost_file_id
  ON public.cr_special_obj_perfomed_work_act
  USING btree
  (cost_file_id);

CREATE INDEX ind_cr_special_obj_per_work_ac_ocr
  ON public.cr_special_obj_perfomed_work_act
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_obj_per_work_ac_st
  ON public.cr_special_obj_perfomed_work_act
  USING btree
  (state_id);

CREATE INDEX ind_cr_special_obj_per_work_ac_w
  ON public.cr_special_obj_perfomed_work_act
  USING btree
  (type_work_cr_id);

CREATE INDEX ind_special_document_file
  ON public.cr_special_obj_perfomed_work_act
  USING btree
  (doc_file_id);
";

            this.Database.ExecuteNonQuery(query);

            //ViewManager.Create(this.Database, "GkhCr", "CreateViewSpecialCrObject");
        }

        public override void Down()
        {
            this.Database.RemoveTable("cr_special_obj_perfomed_work_act");
            this.Database.RemoveTable("cr_special_obj_type_work_removal");
            this.Database.RemoveTable("cr_special_obj_monitoring_cmp");
            this.Database.RemoveTable("cr_special_obj_type_work_hist");
            this.Database.RemoveTable("cr_special_voice_qual_member");
            this.Database.RemoveTable("cr_special_obj_design_asgmnt_type_work");
            this.Database.RemoveTable("cr_special_qualification");
            this.Database.RemoveTable("cr_special_obj_design_assignment");
            this.Database.RemoveTable("cr_special_obj_protocol_tw");
            this.Database.RemoveTable("cr_special_obj_fin_source_res");
            this.Database.RemoveTable("cr_special_obj_defect_list");
            this.Database.RemoveTable("cr_special_obj_protocol");
            this.Database.RemoveTable("cr_special_obj_contract");
            this.Database.RemoveTable("cr_special_obj_additional_params");
            this.Database.RemoveTable("cr_special_obj_pers_account");

            this.Database.RemoveTable("CR_SPECIAL_OBJ_TYPE_WORK");

            //ViewManager.Drop(this.Database, "GkhCr", "DeleteViewSpecialCrObject");

            this.Database.RemoveTable("CR_SPECIAL_OBJECT");

        }
    }
}