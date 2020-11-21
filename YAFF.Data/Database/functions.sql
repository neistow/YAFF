create or replace function get_user_by_id(user_id uuid) returns users as
$$
select *
from users u
where u.id = user_id
limit 1;
$$
    language sql;

create or replace function get_user_by_email(user_email varchar) returns users as
$$
select *
from users u
where u.email = user_email
limit 1
$$
    language sql;


create or replace function get_user_roles(user_id uuid) returns setof roles as
$$
select r.id, r.name
from userroles ur
         join roles r on ur.roleid = r.id
where ur.userid = user_id
$$
    language sql;


create or replace function get_user_posts(user_id uuid) returns setof posts as
$$
select *
from posts p
where p.AuthorId = user_id;
$$
    language sql;


create or replace function find_refresh_token(user_id uuid, _token text) returns refreshtokens as
$$
select *
from refreshtokens t
where t.userid = user_id
  and t.token = _token
limit 1;
$$
    language sql;