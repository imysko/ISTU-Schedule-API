--
-- PostgreSQL database dump
--

-- Dumped from database version 14.7 (Ubuntu 14.7-0ubuntu0.22.10.1)
-- Dumped by pg_dump version 14.7 (Ubuntu 14.7-0ubuntu0.22.10.1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: classrooms; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.classrooms (
    classroom_id integer NOT NULL,
    name text
);


ALTER TABLE public.classrooms OWNER TO imysko;

--
-- Name: classrooms_classroom_id_seq; Type: SEQUENCE; Schema: public; Owner: imysko
--

CREATE SEQUENCE public.classrooms_classroom_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.classrooms_classroom_id_seq OWNER TO imysko;

--
-- Name: classrooms_classroom_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: imysko
--

ALTER SEQUENCE public.classrooms_classroom_id_seq OWNED BY public.classrooms.classroom_id;


--
-- Name: disciplines; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.disciplines (
    discipline_id integer NOT NULL,
    title text,
    real_title text
);


ALTER TABLE public.disciplines OWNER TO imysko;

--
-- Name: disciplines_discipline_id_seq; Type: SEQUENCE; Schema: public; Owner: imysko
--

CREATE SEQUENCE public.disciplines_discipline_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.disciplines_discipline_id_seq OWNER TO imysko;

--
-- Name: disciplines_discipline_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: imysko
--

ALTER SEQUENCE public.disciplines_discipline_id_seq OWNED BY public.disciplines.discipline_id;


--
-- Name: groups; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.groups (
    group_id integer NOT NULL,
    name text,
    course integer,
    institute_id integer,
    is_active boolean
);


ALTER TABLE public.groups OWNER TO imysko;

--
-- Name: groups_group_id_seq; Type: SEQUENCE; Schema: public; Owner: imysko
--

CREATE SEQUENCE public.groups_group_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.groups_group_id_seq OWNER TO imysko;

--
-- Name: groups_group_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: imysko
--

ALTER SEQUENCE public.groups_group_id_seq OWNED BY public.groups.group_id;


--
-- Name: institutes; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.institutes (
    institute_id integer NOT NULL,
    institute_title text
);


ALTER TABLE public.institutes OWNER TO imysko;

--
-- Name: institutes_institute_id_seq; Type: SEQUENCE; Schema: public; Owner: imysko
--

CREATE SEQUENCE public.institutes_institute_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.institutes_institute_id_seq OWNER TO imysko;

--
-- Name: institutes_institute_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: imysko
--

ALTER SEQUENCE public.institutes_institute_id_seq OWNED BY public.institutes.institute_id;


--
-- Name: lessons_time; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.lessons_time (
    lesson_number text,
    begtime text,
    endtime text,
    lesson_id integer NOT NULL
);


ALTER TABLE public.lessons_time OWNER TO imysko;

--
-- Name: lessons_time_lesson_id_seq; Type: SEQUENCE; Schema: public; Owner: imysko
--

CREATE SEQUENCE public.lessons_time_lesson_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.lessons_time_lesson_id_seq OWNER TO imysko;

--
-- Name: lessons_time_lesson_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: imysko
--

ALTER SEQUENCE public.lessons_time_lesson_id_seq OWNED BY public.lessons_time.lesson_id;


--
-- Name: other_disciplines; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.other_disciplines (
    other_discipline_id integer NOT NULL,
    discipline_title text,
    is_online boolean DEFAULT false,
    type integer,
    is_active boolean,
    project_active boolean,
    projfair_project_id integer
);


ALTER TABLE public.other_disciplines OWNER TO imysko;

--
-- Name: queries; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.queries (
    query_id integer NOT NULL,
    description text
);


ALTER TABLE public.queries OWNER TO imysko;

--
-- Name: schedule; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.schedule (
    schedule_id integer NOT NULL,
    groups_verbose text,
    teachers_verbose text,
    classroom_id integer,
    classroom_verbose text,
    discipline_id integer,
    discipline_verbose text,
    lesson_id integer,
    subgroup integer,
    lesson_type integer,
    date date,
    schedule_type text,
    other_discipline_id integer,
    query_id integer
);


ALTER TABLE public.schedule OWNER TO imysko;

--
-- Name: schedule_groups; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.schedule_groups (
    group_id integer,
    schedule_id integer
);


ALTER TABLE public.schedule_groups OWNER TO imysko;

--
-- Name: schedule_schedule_id_seq; Type: SEQUENCE; Schema: public; Owner: imysko
--

CREATE SEQUENCE public.schedule_schedule_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.schedule_schedule_id_seq OWNER TO imysko;

--
-- Name: schedule_schedule_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: imysko
--

ALTER SEQUENCE public.schedule_schedule_id_seq OWNED BY public.schedule.schedule_id;


--
-- Name: schedule_teachers; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.schedule_teachers (
    schedule_id integer,
    teacher_id integer
);


ALTER TABLE public.schedule_teachers OWNER TO imysko;

--
-- Name: teachers; Type: TABLE; Schema: public; Owner: imysko
--

CREATE TABLE public.teachers (
    teacher_id integer NOT NULL,
    fullname text,
    shortname text
);


ALTER TABLE public.teachers OWNER TO imysko;

--
-- Name: teachers_teacher_id_seq; Type: SEQUENCE; Schema: public; Owner: imysko
--

CREATE SEQUENCE public.teachers_teacher_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.teachers_teacher_id_seq OWNER TO imysko;

--
-- Name: teachers_teacher_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: imysko
--

ALTER SEQUENCE public.teachers_teacher_id_seq OWNED BY public.teachers.teacher_id;


--
-- Name: classrooms classroom_id; Type: DEFAULT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.classrooms ALTER COLUMN classroom_id SET DEFAULT nextval('public.classrooms_classroom_id_seq'::regclass);


--
-- Name: disciplines discipline_id; Type: DEFAULT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.disciplines ALTER COLUMN discipline_id SET DEFAULT nextval('public.disciplines_discipline_id_seq'::regclass);


--
-- Name: groups group_id; Type: DEFAULT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.groups ALTER COLUMN group_id SET DEFAULT nextval('public.groups_group_id_seq'::regclass);


--
-- Name: institutes institute_id; Type: DEFAULT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.institutes ALTER COLUMN institute_id SET DEFAULT nextval('public.institutes_institute_id_seq'::regclass);


--
-- Name: lessons_time lesson_id; Type: DEFAULT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.lessons_time ALTER COLUMN lesson_id SET DEFAULT nextval('public.lessons_time_lesson_id_seq'::regclass);


--
-- Name: schedule schedule_id; Type: DEFAULT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule ALTER COLUMN schedule_id SET DEFAULT nextval('public.schedule_schedule_id_seq'::regclass);


--
-- Name: teachers teacher_id; Type: DEFAULT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.teachers ALTER COLUMN teacher_id SET DEFAULT nextval('public.teachers_teacher_id_seq'::regclass);


--
-- Data for Name: classrooms; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.classrooms (classroom_id, name) FROM stdin;
\.


--
-- Data for Name: disciplines; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.disciplines (discipline_id, title, real_title) FROM stdin;
\.


--
-- Data for Name: groups; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.groups (group_id, name, course, institute_id, is_active) FROM stdin;
\.


--
-- Data for Name: institutes; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.institutes (institute_id, institute_title) FROM stdin;
\.


--
-- Data for Name: lessons_time; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.lessons_time (lesson_number, begtime, endtime, lesson_id) FROM stdin;
\.


--
-- Data for Name: other_disciplines; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.other_disciplines (other_discipline_id, discipline_title, is_online, type, is_active, project_active, projfair_project_id) FROM stdin;
\.


--
-- Data for Name: queries; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.queries (query_id, description) FROM stdin;
\.


--
-- Data for Name: schedule; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.schedule (schedule_id, groups_verbose, teachers_verbose, classroom_id, classroom_verbose, discipline_id, discipline_verbose, lesson_id, subgroup, lesson_type, date, schedule_type, other_discipline_id, query_id) FROM stdin;
\.


--
-- Data for Name: schedule_groups; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.schedule_groups (group_id, schedule_id) FROM stdin;
\.


--
-- Data for Name: schedule_teachers; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.schedule_teachers (schedule_id, teacher_id) FROM stdin;
\.


--
-- Data for Name: teachers; Type: TABLE DATA; Schema: public; Owner: imysko
--

COPY public.teachers (teacher_id, fullname, shortname) FROM stdin;
\.


--
-- Name: classrooms_classroom_id_seq; Type: SEQUENCE SET; Schema: public; Owner: imysko
--

SELECT pg_catalog.setval('public.classrooms_classroom_id_seq', 1, false);


--
-- Name: disciplines_discipline_id_seq; Type: SEQUENCE SET; Schema: public; Owner: imysko
--

SELECT pg_catalog.setval('public.disciplines_discipline_id_seq', 1, false);


--
-- Name: groups_group_id_seq; Type: SEQUENCE SET; Schema: public; Owner: imysko
--

SELECT pg_catalog.setval('public.groups_group_id_seq', 1, false);


--
-- Name: institutes_institute_id_seq; Type: SEQUENCE SET; Schema: public; Owner: imysko
--

SELECT pg_catalog.setval('public.institutes_institute_id_seq', 1, false);


--
-- Name: lessons_time_lesson_id_seq; Type: SEQUENCE SET; Schema: public; Owner: imysko
--

SELECT pg_catalog.setval('public.lessons_time_lesson_id_seq', 1, false);


--
-- Name: schedule_schedule_id_seq; Type: SEQUENCE SET; Schema: public; Owner: imysko
--

SELECT pg_catalog.setval('public.schedule_schedule_id_seq', 1, false);


--
-- Name: teachers_teacher_id_seq; Type: SEQUENCE SET; Schema: public; Owner: imysko
--

SELECT pg_catalog.setval('public.teachers_teacher_id_seq', 1, false);


--
-- Name: classrooms classrooms_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.classrooms
    ADD CONSTRAINT classrooms_pkey PRIMARY KEY (classroom_id);


--
-- Name: disciplines disciplines_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.disciplines
    ADD CONSTRAINT disciplines_pkey PRIMARY KEY (discipline_id);


--
-- Name: groups groups_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.groups
    ADD CONSTRAINT groups_pkey PRIMARY KEY (group_id);


--
-- Name: institutes institutes_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.institutes
    ADD CONSTRAINT institutes_pkey PRIMARY KEY (institute_id);


--
-- Name: lessons_time lessons_time_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.lessons_time
    ADD CONSTRAINT lessons_time_pkey PRIMARY KEY (lesson_id);


--
-- Name: other_disciplines other_disciplines_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.other_disciplines
    ADD CONSTRAINT other_disciplines_pkey PRIMARY KEY (other_discipline_id);


--
-- Name: queries query_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.queries
    ADD CONSTRAINT query_pkey PRIMARY KEY (query_id);


--
-- Name: schedule schedule_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule
    ADD CONSTRAINT schedule_pkey PRIMARY KEY (schedule_id);


--
-- Name: teachers teachers_pkey; Type: CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.teachers
    ADD CONSTRAINT teachers_pkey PRIMARY KEY (teacher_id);


--
-- Name: schedule discipline_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule
    ADD CONSTRAINT discipline_fk FOREIGN KEY (discipline_id) REFERENCES public.disciplines(discipline_id);


--
-- Name: groups institute_id; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.groups
    ADD CONSTRAINT institute_id FOREIGN KEY (institute_id) REFERENCES public.institutes(institute_id);


--
-- Name: schedule lesson_time_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule
    ADD CONSTRAINT lesson_time_fk FOREIGN KEY (lesson_id) REFERENCES public.lessons_time(lesson_id);


--
-- Name: schedule other_discipline_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule
    ADD CONSTRAINT other_discipline_fk FOREIGN KEY (other_discipline_id) REFERENCES public.other_disciplines(other_discipline_id);


--
-- Name: schedule query_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule
    ADD CONSTRAINT query_fk FOREIGN KEY (query_id) REFERENCES public.queries(query_id);


--
-- Name: schedule schedule_classrooms_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule
    ADD CONSTRAINT schedule_classrooms_null_fk FOREIGN KEY (classroom_id) REFERENCES public.classrooms(classroom_id);


--
-- Name: schedule_groups schedule_groups_groups_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule_groups
    ADD CONSTRAINT schedule_groups_groups_null_fk FOREIGN KEY (group_id) REFERENCES public.groups(group_id);


--
-- Name: schedule_groups schedule_groups_schedule_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule_groups
    ADD CONSTRAINT schedule_groups_schedule_null_fk FOREIGN KEY (schedule_id) REFERENCES public.schedule(schedule_id);


--
-- Name: schedule_teachers schedule_teachers_schedule_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule_teachers
    ADD CONSTRAINT schedule_teachers_schedule_null_fk FOREIGN KEY (schedule_id) REFERENCES public.schedule(schedule_id);


--
-- Name: schedule_teachers schedule_teachers_teachers_null_fk; Type: FK CONSTRAINT; Schema: public; Owner: imysko
--

ALTER TABLE ONLY public.schedule_teachers
    ADD CONSTRAINT schedule_teachers_teachers_null_fk FOREIGN KEY (teacher_id) REFERENCES public.teachers(teacher_id);


--
-- PostgreSQL database dump complete
--

