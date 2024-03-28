namespace Bars.GkhCr.Migrations._2023.Version_2023123109
{

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    [Migration("2023123109")]
    [MigrationDependsOn(typeof(Version_2023123108.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var query = @"
--SpecialDocumentWorkCr
CREATE TABLE public.cr_special_obj_document_work
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  object_id bigint NOT NULL,
  contragent_id bigint,
  file_id bigint,
  document_name character varying(300),
  document_num character varying(50),
  description character varying(500),
  date_from date,
  external_id character varying(36),
  type_work_id bigint,
  used_in_export integer NOT NULL DEFAULT 20,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_document_work_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_doc_work_ctr FOREIGN KEY (contragent_id)
      REFERENCES public.gkh_contragent (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_doc_work_file FOREIGN KEY (file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_doc_work_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_special_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_document_work_tw FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_document_work
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_doc_work_ctr
  ON public.cr_special_obj_document_work
  USING btree
  (contragent_id);

CREATE INDEX ind_cr_special_obj_doc_work_file
  ON public.cr_special_obj_document_work
  USING btree
  (file_id);

CREATE INDEX ind_cr_special_obj_doc_work_name
  ON public.cr_special_obj_document_work
  USING btree
  (document_name COLLATE pg_catalog.default);

CREATE INDEX ind_cr_special_obj_doc_work_ocr
  ON public.cr_special_obj_document_work
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_obj_document_work_tw
  ON public.cr_special_obj_document_work
  USING btree
  (type_work_id);

--SpecialPerformedWorkActRecord
CREATE TABLE public.cr_special_obj_perfomed_wact_rec
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  perfomed_act_id bigint NOT NULL,
  base_salary numeric(19,5),
  mech_work numeric(19,5),
  base_work numeric(19,5),
  total_count numeric(19,5),
  total_cost numeric(19,5),
  on_unit_count numeric(19,5),
  on_unit_cost numeric(19,5),
  mat_cost numeric(19,5),
  machine_operating_cost numeric(19,5),
  mech_salary numeric(19,5),
  external_id character varying(36),
  unit_measure character varying(300),
  reason character varying(1000),
  document_num character varying(250),
  document_name character varying(1000),
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_perfomed_wact_rec_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_per_wac_rec_pact FOREIGN KEY (perfomed_act_id)
      REFERENCES public.cr_special_obj_perfomed_work_act (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_perfomed_work_act
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_per_wac_rec_pact
  ON public.cr_special_obj_perfomed_wact_rec
  USING btree
  (perfomed_act_id);

--SpecialEstimateCalculation
CREATE TABLE public.cr_special_obj_estimate_calc
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  state_id bigint,
  object_id bigint NOT NULL,
  type_work_cr_id bigint,
  file_res_statment_id bigint,
  file_estimate_id bigint,
  file_estimate_file_id bigint,
  res_stat_doc_name character varying(300),
  res_stat_doc_date date,
  estimate_doc_name character varying(300),
  estimate_doc_date date,
  estimate_file_doc_name character varying(300),
  estimate_file_doc_date date,
  nds numeric(19,5),
  other_cost numeric(19,5),
  total_estimate numeric(19,5),
  overhead_sum numeric(19,5),
  estimate_profit numeric(19,5),
  total_direct_cost numeric(19,5),
  external_id character varying(36),
  res_stat_doc_num character varying(300),
  estimate_doc_num character varying(300),
  estimate_file_doc_num character varying(300),
  is_without_nds boolean NOT NULL DEFAULT false,
  estimation_type integer NOT NULL DEFAULT 0,
  used_in_export integer NOT NULL DEFAULT 20,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_estimate_calc_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_est_calc_ef FOREIGN KEY (file_estimate_file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_est_calc_est FOREIGN KEY (file_estimate_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_est_calc_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_est_calc_res FOREIGN KEY (file_res_statment_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_est_calc_st FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_estimate_calc
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_est_calc_ef_id
  ON public.cr_special_obj_estimate_calc
  USING btree
  (file_estimate_file_id);

CREATE INDEX ind_cr_special_obj_est_calc_ef_name
  ON public.cr_special_obj_estimate_calc
  USING btree
  (estimate_file_doc_name COLLATE pg_catalog.default);

CREATE INDEX ind_cr_special_obj_est_calc_est_id
  ON public.cr_special_obj_estimate_calc
  USING btree
  (file_estimate_id);

CREATE INDEX ind_cr_special_obj_est_calc_est_name
  ON public.cr_special_obj_estimate_calc
  USING btree
  (estimate_doc_name COLLATE pg_catalog.default);

CREATE INDEX ind_cr_special_obj_est_calc_ocr
  ON public.cr_special_obj_estimate_calc
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_obj_est_calc_res_id
  ON public.cr_special_obj_estimate_calc
  USING btree
  (file_res_statment_id);

CREATE INDEX ind_cr_special_obj_est_calc_res_name
  ON public.cr_special_obj_estimate_calc
  USING btree
  (res_stat_doc_name COLLATE pg_catalog.default);

CREATE INDEX ind_cr_special_obj_est_calc_st
  ON public.cr_special_obj_estimate_calc
  USING btree
  (state_id);

--SpecialEstimate
CREATE TABLE public.cr_special_est_calc_estimate
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  estimate_calc_id bigint NOT NULL,
  base_salary numeric(19,5),
  mech_work numeric(19,5),
  base_work numeric(19,5),
  total_count numeric(19,5),
  total_cost numeric(19,5),
  on_unit_count numeric(19,5),
  on_unit_cost numeric(19,5),
  mat_cost numeric(19,5),
  machine_operating_cost numeric(19,5),
  mech_salary numeric(19,5),
  external_id character varying(36),
  unit_measure character varying(300),
  reason character varying(1000),
  document_num character varying(300),
  document_name character varying(2000),
  import_entity_id bigint,
  CONSTRAINT cr_special_est_calc_estimate_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_est_cal_est_ec FOREIGN KEY (estimate_calc_id)
      REFERENCES public.cr_special_obj_estimate_calc (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_est_calc_estimate
  OWNER TO bars;

CREATE INDEX ind_cr_special_est_cal_est_ec
  ON public.cr_special_est_calc_estimate
  USING btree
  (estimate_calc_id);

--SpecialResourceStatement
CREATE TABLE public.cr_special_est_calc_res_statem
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  estimate_calc_id bigint NOT NULL,
  total_count numeric(19,5),
  total_cost numeric(19,5),
  on_unit_count numeric(19,5),
  external_id character varying(36),
  unit_measure character varying(300),
  reason character varying(1000),
  document_num character varying(300),
  document_name character varying(1000),
  import_entity_id bigint,
  CONSTRAINT cr_special_est_calc_res_statem_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_est_cal_res_ec FOREIGN KEY (estimate_calc_id)
      REFERENCES public.cr_special_obj_estimate_calc (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_est_calc_res_statem
  OWNER TO bars;

CREATE INDEX ind_cr_special_est_cal_res_ec
  ON public.cr_special_est_calc_res_statem
  USING btree
  (estimate_calc_id);

--SpecialBuildContract
CREATE TABLE public.cr_special_obj_build_contract
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  state_id bigint,
  object_id bigint NOT NULL,
  inspector_id bigint,
  builder_id bigint,
  document_file_id bigint,
  protocol_file_id bigint,
  type_contract_build integer NOT NULL DEFAULT 10,
  date_start_work date,
  date_end_work date,
  date_gji date,
  document_date_from date,
  protocol_date_from date,
  date_cancel date,
  date_accept date,
  document_name character varying(300),
  protocol_name character varying(300),
  document_num character varying(50),
  protocol_num character varying(50),
  description character varying(500),
  sum numeric(19,5),
  external_id character varying(36),
  budget_mo numeric(19,5),
  budget_subj numeric(19,5),
  owner_means numeric(19,5),
  fund_means numeric(19,5),
  type_work_id bigint,
  used_in_export integer NOT NULL DEFAULT 20,
  contragent_id bigint,
  termination_date timestamp without time zone,
  termination_reason character varying(255),
  termination_document_file_id bigint,
  guarantee_period integer,
  url_result_trading character varying(255),
  import_entity_id bigint,
  termination_document_number character varying(255),
  termination_reason_id bigint,
  is_law_provided integer NOT NULL DEFAULT 20,
  website character varying(255),
  build_contract_state integer NOT NULL DEFAULT 10,
  CONSTRAINT cr_special_obj_build_contract_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_build_cn_bldr FOREIGN KEY (builder_id)
      REFERENCES public.gkh_builder (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_build_cn_dfile FOREIGN KEY (document_file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_build_cn_insp FOREIGN KEY (inspector_id)
      REFERENCES public.gkh_dict_inspector (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_build_cn_ocr FOREIGN KEY (object_id)
      REFERENCES public.cr_object (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_build_cn_pfile FOREIGN KEY (protocol_file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_build_cn_state FOREIGN KEY (state_id)
      REFERENCES public.b4_state (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_build_contract_termination_document_file FOREIGN KEY (termination_document_file_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_build_contract_termination_reason FOREIGN KEY (termination_reason_id)
      REFERENCES public.cr_dict_termination_reason (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_build_contract_tw FOREIGN KEY (type_work_id)
      REFERENCES public.cr_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_fk_contragent_id FOREIGN KEY (contragent_id)
      REFERENCES public.gkh_contragent (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_build_contract
  OWNER TO bars;

CREATE INDEX ind_cr_special_build_cn_bldr
  ON public.cr_special_obj_build_contract
  USING btree
  (builder_id);

CREATE INDEX ind_cr_special_build_cn_dfile
  ON public.cr_special_obj_build_contract
  USING btree
  (document_file_id);

CREATE INDEX ind_cr_special_build_cn_insp
  ON public.cr_special_obj_build_contract
  USING btree
  (inspector_id);

CREATE INDEX ind_cr_special_build_cn_ocr
  ON public.cr_special_obj_build_contract
  USING btree
  (object_id);

CREATE INDEX ind_cr_special_build_cn_pfile
  ON public.cr_special_obj_build_contract
  USING btree
  (protocol_file_id);

CREATE INDEX ind_cr_special_build_cn_state
  ON public.cr_special_obj_build_contract
  USING btree
  (state_id);

CREATE INDEX ind_cr_special_obj_build_contract_termination_document_file
  ON public.cr_special_obj_build_contract
  USING btree
  (termination_document_file_id);

CREATE INDEX ind_cr_special_obj_build_contract_termination_reason
  ON public.cr_special_obj_build_contract
  USING btree
  (termination_reason_id);

CREATE INDEX ind_cr_special_obj_build_contract_tw
  ON public.cr_special_obj_build_contract
  USING btree
  (type_work_id);

CREATE INDEX ind_fk_special_contragent_id
  ON public.cr_special_obj_build_contract
  USING btree
  (contragent_id);

--SpecialBuildContract
CREATE TABLE public.cr_special_bld_contr_type_wrk
(
  id bigint NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  build_contract_id bigint NOT NULL,
  type_work_id bigint NOT NULL,
  sum numeric(19,5),
  import_entity_id bigint,
  CONSTRAINT cr_special_bld_contr_type_wrk_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_bld_ctr_tw_ctr FOREIGN KEY (build_contract_id)
      REFERENCES public.cr_special_obj_build_contract (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_bld_ctr_tw_wrk FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_bld_contr_type_wrk
  OWNER TO bars;

CREATE INDEX ind_cr_special_bld_ctr_tw_ctr
  ON public.cr_special_bld_contr_type_wrk
  USING btree
  (build_contract_id);

CREATE INDEX ind_cr_special_bld_ctr_tw_wrk
  ON public.cr_special_bld_contr_type_wrk
  USING btree
  (type_work_id);

--SpecialContractCrTypeWork
CREATE TABLE public.cr_special_contr_cr_type_wrk
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  contract_cr_id bigint NOT NULL,
  type_work_id bigint NOT NULL,
  sum numeric,
  import_entity_id bigint,
  CONSTRAINT cr_special_contr_cr_type_wrk_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_contr_cr_type_wrk_cr_id FOREIGN KEY (contract_cr_id)
      REFERENCES public.cr_special_obj_contract (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_contr_cr_type_wrk_id FOREIGN KEY (type_work_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_contr_cr_type_wrk
  OWNER TO bars;

CREATE INDEX ind_cr_special_contr_cr_type_wrk_cr_id
  ON public.cr_special_contr_cr_type_wrk
  USING btree
  (contract_cr_id);

CREATE INDEX ind_cr_special_contr_cr_type_wrk_id
  ON public.cr_special_contr_cr_type_wrk
  USING btree
  (type_work_id);

--SpecialArchiveSmr
CREATE TABLE public.cr_special_obj_cmp_archive
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  type_work_cr_id bigint,
  stage_work_cr_id bigint,
  percent_completion numeric(19,5),
  cost_sum numeric(19,5),
  count_worker numeric(19,5),
  volume_completion numeric(19,5),
  date_change_rec date,
  type_archive_cmp integer NOT NULL DEFAULT 10,
  external_id character varying(36),
  manufacturer_name character varying(2000),
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_cmp_archive_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_obj_cmp_arc_sw FOREIGN KEY (stage_work_cr_id)
      REFERENCES public.cr_dict_stage_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_obj_cmp_arc_tw FOREIGN KEY (type_work_cr_id)
      REFERENCES public.cr_special_obj_type_work (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_cmp_archive
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_cmp_arc_sw
  ON public.cr_special_obj_cmp_archive
  USING btree
  (stage_work_cr_id);

CREATE INDEX ind_cr_special_obj_cmp_arc_tw
  ON public.cr_special_obj_cmp_archive
  USING btree
  (type_work_cr_id);

--SpecialPerformedWorkActPayment
CREATE TABLE public.cr_special_obj_per_act_payment
(
  id bigserial NOT NULL,
  object_version bigint NOT NULL,
  object_create_date timestamp without time zone NOT NULL,
  object_edit_date timestamp without time zone NOT NULL,
  act_id bigint NOT NULL,
  type_act_payment integer NOT NULL DEFAULT 10,
  date_payment date,
  sum numeric(19,5) NOT NULL DEFAULT 0,
  percent numeric(19,5) NOT NULL DEFAULT 0,
  sum_paid numeric(19,5) NOT NULL DEFAULT 0,
  date_disposal timestamp without time zone,
  is_cancelled boolean NOT NULL DEFAULT false,
  transfer_guid character varying(40),
  document_id bigint,
  import_entity_id bigint,
  CONSTRAINT cr_special_obj_per_act_payment_pkey PRIMARY KEY (id),
  CONSTRAINT fk_cr_special_obj_per_act_payment_d FOREIGN KEY (document_id)
      REFERENCES public.b4_file_info (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT fk_cr_special_per_act_payment FOREIGN KEY (act_id)
      REFERENCES public.cr_special_obj_perfomed_work_act (id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.cr_special_obj_per_act_payment
  OWNER TO bars;

CREATE INDEX ind_cr_special_obj_per_act_payment_d
  ON public.cr_special_obj_per_act_payment
  USING btree
  (document_id);

CREATE INDEX ind_cr_special_per_act_payment
  ON public.cr_special_obj_per_act_payment
  USING btree
  (act_id);
";
            this.Database.ExecuteNonQuery(query);

            //ViewManager.Create(this.Database, "GkhCr", "CreateViewCrSpecialObjEstCalcDet");
            //ViewManager.Create(this.Database, "GkhCr", "CreateViewCrSpecialObjectEstCalc");
        }

        public override void Down()
        {
            //ViewManager.Drop(this.Database, "GkhCr", "DeleteViewCrSpecialObjEstCalcDet");
            //ViewManager.Drop(this.Database, "GkhCr", "DeleteViewCrSpecialObjectEstCalc");

            this.Database.RemoveTable("cr_special_obj_document_work");
            this.Database.RemoveTable("cr_special_obj_perfomed_wact_rec");
            this.Database.RemoveTable("cr_special_est_calc_res_statem");
            this.Database.RemoveTable("cr_special_est_calc_estimate");
            this.Database.RemoveTable("cr_special_obj_estimate_calc");
            this.Database.RemoveTable("cr_special_bld_contr_type_wrk");
            this.Database.RemoveTable("cr_special_contr_cr_type_wrk");
            this.Database.RemoveTable("cr_special_obj_build_contract");
            this.Database.RemoveTable("cr_special_obj_cmp_archive");
            this.Database.RemoveTable("cr_special_obj_per_act_payment");
        }
    }
}